using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Common;
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
        private static Dictionary<int, PointItem> points = new Dictionary<int, PointItem>();
        private static WCFClient client = null;
        private PointContext _dataBase;

        public PointController(PointContext dataBase)
        {
            this._dataBase = dataBase;
        }

        [HttpGet]
        [Route("Connect")]
        public async Task<Object> Connect()
        {
            client = new WCFClient(new NetTcpBinding(), new EndpointAddress(new Uri("net.tcp://localhost:9999/Server")));

            var pointsList = client.GetPoints();

            if(pointsList == null)
            {
                return NotFound("ConnectionFailiure");
            }
            else
            {
                PutPointsInDataBase(pointsList);
            }

            var configList = client.GetConfigItems();

            if(configList == null)
            {
                return NotFound("ConnectionFailiure");
            }
            else
            {
                PutConfigsInDataBase(configList);
            }

            var Points = _dataBase.Points.ToList();

            return new
            {
                Points,
                ConfigItems = _dataBase.ConfigItems.ToArray()
            }; //Ok(_dataBase.Points.ToArray());
        }

        [HttpGet]
        [Route("GetAllPoints")]
        public async Task<Object> GetAllPoints()
        {
            var points = _dataBase.Points.ToList();

            return points;
        }

        [HttpGet]
        [Route("ReadSingleRegister")]
        public async Task<Object> ReadSingleRegister(int pid)
        {
            try
            {
                var point = _dataBase.Points.Find(pid);
                if (point == null)
                    return BadRequest("Wrong point id");

                try
                {
                    var newValue = client.ReadCommand(new PointIdentifier() { Address = point.Address, PointType = point.Type });

                    if(newValue == null)
                        return NotFound("Read Command Failed");

                    point.RawValue = newValue.RawValue;
                    point.Timestamp = newValue.Timestamp;
                    if (point.Type == PointType.ANALOG_INPUT || point.Type == PointType.ANALOG_OUTPUT)
                        point.EguValue = newValue.EguValue;
                    else
                        point.State = newValue.State;

                    _dataBase.SaveChanges();
                }
                catch
                {
                    return NotFound("ConnectionFailiure");
                }

                 
                return Ok(point);
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet]
        [Route("CommandSingleRegister")]
        public async Task<Object> CommandSingleRegister(int pid, int value)
        {
            try
            {
                var point = _dataBase.Points.Find(pid);
                if (point == null)
                    return BadRequest("Wrong point id");

                try
                {
                    var newValue = client.WriteCommand(new PointIdentifier() { Address = point.Address, PointType = point.Type }, (ushort)value);

                    if (newValue == null)
                        return NotFound("Read Command Failed");

                    point.RawValue = newValue.RawValue;
                    point.Timestamp = newValue.Timestamp;
                    if (point.Type == PointType.ANALOG_INPUT || point.Type == PointType.ANALOG_OUTPUT)
                        point.EguValue = newValue.EguValue;
                    else
                        point.State = newValue.State;

                    _dataBase.SaveChanges();
                }
                catch
                {
                    return NotFound("ConnectionFailiure");
                }

                return Ok(point);
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPost]
        [Route("DoTheAcqusition")]
        public async Task<Object> DoTheAcqusition([FromBody]List<int> identifiers)
        {
            try
            {
                var pointIdentifiers = new List<PointIdentifier>();
                var configItems = new List<ConfigItem>();
                var checkList = _dataBase.ConfigItems.ToList();
                var points = new List<RegisterData>();

                identifiers.ForEach(i => configItems.Add(checkList.Find(ci => ci.DataBaseId == i)));
                configItems.ForEach(ci => pointIdentifiers.Add(new PointIdentifier(ci.RegistryType, ci.StartAddress)));

                points = client.DoAcquisiton(pointIdentifiers);

                if(points == null)
                {
                    return NotFound("Acqusition Command Failed");
                }

                return Ok(UpdatePoints(points));
                /*configItems.ForEach(ci =>
                {
                    for (int i = 0; i < ci.NumberOfRegisters; i++)
                    {
                        var point = client.ReadCommand(new PointIdentifier(ci.RegistryType, (ushort)(ci.StartAddress + i)));
                        if (point == null)
                        {
                            points = null;
                            return;
                        }
                        points.Add(point);
                    }
                });

                if(points == null)
                    return NotFound("Acqusition Command Failed");

                return Ok(UpdatePoints(points));*/
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }
        }

        private void PutPointsInDataBase(List<PointData> points)
        {
            if(_dataBase.Points.ToArray().Length != 0)
            {
                if (checkIfPointsListIsSame(points))
                    return;
                _dataBase.Points.RemoveRange(_dataBase.Points.ToArray());
            }

            foreach(PointData point in points)
            {
                PointItem newPoint = new PointItem(point);

                _dataBase.Points.Add(newPoint);
            }

            _dataBase.SaveChanges();
        }
        
        private void PutConfigsInDataBase(List<ConfigItemData> configItems)
        {
            if (_dataBase.ConfigItems.ToArray().Length != 0)
            {
                if (checkIfConfingListIsSame(configItems))
                    return;
                _dataBase.ConfigItems.RemoveRange(_dataBase.ConfigItems.ToArray());
            }

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

                _dataBase.ConfigItems.Add(confItem);
            }

            _dataBase.SaveChanges();

        }

        private List<PointItem> UpdatePoints(List<RegisterData> registers)
        {
            var points = _dataBase.Points.ToList();
            var returnValue = new List<PointItem>();

            foreach(RegisterData register in registers)
            {
                var point = points.Find(p => p.Address == register.Address && p.Type == register.Type);

                point.RawValue = register.RawValue;
                point.Timestamp = register.Timestamp;
                if (point.Type == PointType.ANALOG_INPUT || point.Type == PointType.ANALOG_OUTPUT)
                    point.EguValue = register.EguValue;
                else
                    point.State = register.State;

                returnValue.Add(point);
            }

            _dataBase.SaveChangesAsync();
            
            return returnValue;
        }

        private bool checkIfPointsListIsSame(List<PointData> points)
        {
            var list = _dataBase.Points.ToList();
            if (points.Count != list.Count)
                return false;

            foreach(PointItem pi in list)
            {
                if(points.Find(p => pi.Address == p.Address && pi.Type == p.Type) == null)
                {
                    return false;
                }
            }

            return true;
        }

        private bool checkIfConfingListIsSame(List<ConfigItemData> configs)
        {
            var list = _dataBase.ConfigItems.ToList();
            if (configs.Count != list.Count)
                return false;

            foreach(ConfigItem ci in list)
            {
                var configItem = new ConfigItemData();
                if ((configItem = configs.Find(c => c.StartAddress == ci.StartAddress && ci.RegistryType == c.RegistryType && ci.NumberOfRegisters == c.NumberOfRegisters)) == null)
                    return false;
            }

            return true;
        }
    }
}