﻿using Common;
using dCom.Configuration;
using Modbus.Connection;
using ProcessingModule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using WCFContract;

namespace dCom.ViewModel
{
	internal class MainViewModel : ViewModelBase, IDisposable, IStateUpdater, IStorage
	{
		public ObservableCollection<BasePointItem> Points { get; set; }

		#region Fields

		private object lockObject = new object();
		private Thread timerWorker;
		private ConnectionState connectionState;
		private Acquisitor acquisitor;
		private AutoResetEvent acquisitionTrigger = new AutoResetEvent(false);
		private AutoResetEvent automationTrigger = new AutoResetEvent(false);
		private AutoResetEvent serverTrigger = new AutoResetEvent(false);
		private TimeSpan elapsedTime = new TimeSpan();
		private Dispatcher dispather = Dispatcher.CurrentDispatcher;
		private string logText;
		private StringBuilder logBuilder;
		private DateTime currentTime;
		private WCFServer server;
		private IFunctionExecutor commandExecutor;
		private IAutomationManager automationManager;
		private bool timerThreadStopSignal = true;
		private bool disposed = false;
		IConfiguration configuration;
		private IProcessingManager processingManager = null;
		
		#endregion Fields

		Dictionary<int, IPoint> pointsCache = new Dictionary<int, IPoint>();

		#region Properties

		public DateTime CurrentTime
		{
			get
			{
				return currentTime;
			}

			set
			{
				currentTime = value;
				OnPropertyChanged("CurrentTime");
			}
		}

		public ConnectionState ConnectionState
		{
			get
			{
				return connectionState;
			}

			set
			{
				connectionState = value;
				if(connectionState == ConnectionState.CONNECTED)
				{
					automationManager.Start(configuration.DelayBetweenCommands);
				}
				OnPropertyChanged("ConnectionState");
			}
		}

		public string LogText
		{
			get
			{
				return logText;
			}

			set
			{
				logText = value;
				OnPropertyChanged("LogText");
			}
		}

		public TimeSpan ElapsedTime
		{
			get
			{
				return elapsedTime;
			}

			set
			{
				elapsedTime = value;
				OnPropertyChanged("ElapsedTime");
			}
		}

		#endregion Properties

		public MainViewModel()
		{
			configuration = new ConfigReader();
			commandExecutor = new FunctionExecutor(this, configuration);
            this.processingManager = new ProcessingManager(this, commandExecutor);
			this.acquisitor = new Acquisitor(acquisitionTrigger, this.processingManager, this, configuration);
			this.automationManager = new AutomationManager(this, processingManager, automationTrigger, configuration);
			var thread = new Thread(StartWCFServer);
			thread.Name = "WCF Server";
			thread.Start();
			InitializePointCollection();
			InitializeAndStartThreads();
			logBuilder = new StringBuilder();
			ConnectionState = ConnectionState.DISCONNECTED;
			Thread.CurrentThread.Name = "Main Thread";
		}

		void StartWCFServer()
		{
			this.server = new WCFServer(this, processingManager, configuration, serverTrigger);
		}

		#region Private methods

		private void InitializePointCollection()
		{
			Points = new ObservableCollection<BasePointItem>();
			foreach (var c in configuration.GetConfigurationItems())
			{
				for (int i = 0; i < c.NumberOfRegisters; i++)
				{
					BasePointItem pi = CreatePoint(c, i, this.processingManager);
					if (pi != null)
					{
						Points.Add(pi);
						pointsCache.Add(pi.PointId, pi as IPoint);
                        processingManager.InitializePoint(pi.Type, pi.Address, pi.RawValue);
					}
				}
			}
		}

		private BasePointItem CreatePoint(IConfigItem c, int i, IProcessingManager processingManager)
		{
			switch (c.RegistryType)
			{
				case Common.PointType.DIGITAL_INPUT:
					return new DigitalInput(c, processingManager, this, configuration, i);

				case Common.PointType.DIGITAL_OUTPUT:
					return new DigitalOutput(c, processingManager, this, configuration, i);

				case Common.PointType.ANALOG_INPUT:
					return new AnalaogInput(c, processingManager, this, configuration, i);

				case Common.PointType.ANALOG_OUTPUT:
					return new AnalogOutput(c, processingManager, this, configuration, i);

				default:
					return null;
			}
		}

		private void InitializeAndStartThreads()
		{
			InitializeTimerThread();
			StartTimerThread();
		}

		private void InitializeTimerThread()
		{
			timerWorker = new Thread(TimerWorker_DoWork);
			timerWorker.Name = "Timer Thread";
		}

		private void StartTimerThread()
		{
			timerWorker.Start();
		}

		/// <summary>
		/// Timer thread:
		///		Refreshes timers on UI and signalizes to acquisition thread that one second has elapsed
		/// </summary>
		private void TimerWorker_DoWork()
		{
			while (timerThreadStopSignal)
			{
				if (disposed)
					return;

				CurrentTime = DateTime.Now;
				ElapsedTime = ElapsedTime.Add(new TimeSpan(0, 0, 1));
				acquisitionTrigger.Set();
                automationTrigger.Set();
				serverTrigger.Set();
				Thread.Sleep(1000);
			}
		}

		#endregion Private methods

		#region IStateUpdater implementation

		public void UpdateConnectionState(ConnectionState currentConnectionState)
		{
			dispather.Invoke((Action)(() =>
			{
				ConnectionState = currentConnectionState;
			}));
		}

		public void LogMessage(string message)
		{
			if (disposed)
				return;
			string threadName = Thread.CurrentThread.Name;

			/*string[] vs = message.Split(' ');

			ushort address = 0;
			ushort.TryParse(vs[6], out address);
			Transactions.ForEach(t =>
			{
				if(t.Address == address)
				{
					t.Finished = true;
				}
			});*/


			dispather.Invoke((Action)(() =>
			{
				lock (lockObject)
				{
					logBuilder.Append($"{DateTime.Now} [{threadName}]: {message}{Environment.NewLine}");
					LogText = logBuilder.ToString();
				}
			}));
		}

		#endregion IStateUpdater implementation

		public void Dispose()
		{
			disposed = true;
			timerThreadStopSignal = false;
			(commandExecutor as IDisposable).Dispose();
			this.acquisitor.Dispose();
			acquisitionTrigger.Dispose();
			automationManager.Stop();
            automationTrigger.Dispose();
			serverTrigger.Dispose();
		}

		public List<IPoint> GetPoints(List<PointIdentifier> pointIds)
		{
			List<IPoint> retVal = new List<IPoint>(pointIds.Count);
			foreach (var pid in pointIds)
			{
				int id = PointIdentifierHelper.GetNewPointId(pid);
				IPoint p = null;
				if (pointsCache.TryGetValue(id, out p))
				{
					retVal.Add(p);
				}
			}
			return retVal;
		}

		public List<IPoint> GetAllPoints()
		{
			return pointsCache.Values.ToList();
		}

		public IPoint GetPoint(int pointId)
		{
			var points = Points.Where(p => pointId == p.PointId).ToList();

			return points[0];
		}

		public List<IPoint> GetPointsWithNewValues(List<PointIdentifier> pointIds)
		{
			List<IPoint> retVal = new List<IPoint>(pointIds.Count);
			foreach (var pid in pointIds)
			{
				IPoint p;
				if ( ( p = Points.ToList().Find(i => i.Address == pid.Address && i.Type == pid.PointType)) != null)
				{
					retVal.Add(p);
				}
			}
			return retVal;
		}

		public bool GetDisposed()
		{
			return disposed;
		}
	}
}