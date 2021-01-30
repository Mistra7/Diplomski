using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WCFContract;
using WebdScadaBackend.Models;
using WebdScadaBackend.WCFClasses;

namespace WebdScadaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private static WCFClient client = null;
        private static Dictionary<int, PointData> Points = new Dictionary<int, PointData>();
        private static Dictionary<int, ConfigItem> ConfigItems = new Dictionary<int, ConfigItem>();

        public PointController()
        {
        }

        [HttpGet]
        [Route("Connect")]
        public async Task<Object> Connect()
        {
            try
            {
                if(client == null)
                    client = new WCFClient(new NetTcpBinding(), new EndpointAddress(new Uri("net.tcp://localhost:9999/Server")));

                var pointsList = client.GetPoints();

                if (pointsList == null)
                {
                    ResetVariables();
                    return NotFound("ConnectionFailiure");
                }
                
                PutPointsInDictionary(pointsList);

                var configList = client.GetConfigItems();

                if (configList == null)
                {
                    ResetVariables();
                    return NotFound("ConnectionFailiure");
                }

                PutConfigsInDictionary(configList);

                return new
                {
                    Points = Points.Values.ToList(),
                    ConfigItems = ConfigItems.Values.ToList()
                };
            }
            catch
            {
                return NotFound("ConnectionFailiure");
            }
        }

        [HttpGet]
        [Route("ReadSingleRegister")]
        public async Task<Object> ReadSingleRegister(int pid)
        {
            try
            {
                var point = Points[pid];
                if (point == null)
                    return BadRequest("Wrong point id");

                var newValue = client.ReadCommand(pid);

                if(newValue == null)
                    return NotFound("Read Command Failed");

                point.RawValue = newValue.RawValue;
                point.Timestamp = newValue.Timestamp;
                point.Alarm = newValue.Alarm;
                if (point.Type == PointType.ANALOG_INPUT || point.Type == PointType.ANALOG_OUTPUT)
                    point.EguValue = newValue.EguValue;
                else
                    point.State = newValue.State;
                 
                return Ok(point);
            }
            catch
            {
                ResetVariables();
                return NotFound("ConnectionFailiure");
            }
        }

        [HttpGet]
        [Route("CommandSingleRegister")]
        public async Task<Object> CommandSingleRegister(int pid, int value)
        {
            try
            {
                var point = Points[pid];
                if (point == null)
                    return BadRequest("Wrong point id");

                var newValue = client.WriteCommand(pid, (ushort)value);

                if (newValue == null)
                    return NotFound("Read Command Failed");

                point.RawValue = newValue.RawValue;
                point.Timestamp = newValue.Timestamp;
                point.Alarm = newValue.Alarm;
                if (point.Type == PointType.ANALOG_INPUT || point.Type == PointType.ANALOG_OUTPUT)
                    point.EguValue = newValue.EguValue;
                else
                    point.State = newValue.State;

                return Ok(point);
            }
            catch
            {
                ResetVariables();
                return NotFound("ConnectionFailiure");
            }
        }

        [HttpPost]
        [Route("DoTheAcqusition")]
        public async Task<Object> DoTheAcqusition([FromBody]List<int> identifiers)
        {
            try
            {
                var pointIds = new List<AcqusitionData>();
                var configItems = new List<ConfigItem>();
                var checkList = ConfigItems.Values.ToList();
                var points = new List<RegisterData>();
                
                identifiers.ForEach(i => configItems.Add(checkList.Find(ci => ci.Id == i)));
                configItems.ForEach(ci => pointIds.Add(new AcqusitionData(ci.RegistryType, ci.StartAddress, ci.NumberOfRegisters)));

                points = client.DoAcquisiton(pointIds);

                if(points == null)
                {
                    return NotFound("Acqusition Command Failed");
                }

                return Ok(UpdatePoints(points));
            }
            catch
            {
                ResetVariables();
                return NotFound("ConnectionFailiure");
            }
        }

        private void PutPointsInDictionary(List<PointData> points)
        {
            Points = new Dictionary<int, PointData>();
            foreach(PointData point in points)
            {
                Points.Add(point.PointId, point);
            }
        }
        
        private void PutConfigsInDictionary(List<ConfigItemData> configItems)
        {
            ConfigItems = new Dictionary<int, ConfigItem>();

            foreach(ConfigItemData configItem in configItems)
            {
                var confItem = new ConfigItem() 
                { 
                    NumberOfRegisters = configItem.NumberOfRegisters, 
                    RegistryType = configItem.RegistryType, 
                    StartAddress = configItem.StartAddress, 
                    SecondsPassedSinceLastPoll = 0,
                    AcquisitionInterval = configItem.AcquisitionInterval
                };

                ConfigItems.Add(confItem.Id ,confItem);
            }
        }

        private List<PointData> UpdatePoints(List<RegisterData> registers)
        {
            var returnValue = new List<PointData>();

            foreach(RegisterData register in registers)
            {
                var point = Points[register.PointId];

                point.Alarm = register.Alarm;
                point.RawValue = register.RawValue;
                point.Timestamp = register.Timestamp;
                if (point.Type == PointType.ANALOG_INPUT || point.Type == PointType.ANALOG_OUTPUT)
                    point.EguValue = register.EguValue;
                else
                    point.State = register.State;

                returnValue.Add(point);
            }
            
            return returnValue;
        }

        private void ResetVariables()
        {
            client = null;
            Points = new Dictionary<int, PointData>();
            ConfigItems = new Dictionary<int, ConfigItem>();
        }
    }
}