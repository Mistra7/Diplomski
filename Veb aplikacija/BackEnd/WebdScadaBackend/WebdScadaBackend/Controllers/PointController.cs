using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
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
        private static Dictionary<int, BasePointItem> points = new Dictionary<int, BasePointItem>();
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

            List<PointData> pointsList = null;

            pointsList = client.Connect();

            if(pointsList == null)
            {
                return NotFound("ConnectionFailiure");
            }
            else
            {
                PutPointsInDataBase(pointsList);
                return Ok(_dataBase.Points.ToArray());
            }
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
                    if (point.Type == PointType.ANALOG_INPUT || point.Type == PointType.ANALOG_OUTPUT)
                        ((AnalogBase)point).EguValue = newValue.EguValue;
                    else
                        ((DigitalBase)point).State = newValue.State;

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
        public async Task<Object> CommandSingleRegister(int pid, short value)
        {
            try
            {
                var point = _dataBase.Points.Find(pid);
                if (point == null)
                    return BadRequest("Wrong point id");
                

                try
                {
                    var newValue = client.WriteCommand(new PointIdentifier() { Address = point.Address, PointType = point.Type }, value);

                    if (newValue == null)
                        return NotFound("Read Command Failed");

                    point.RawValue = newValue.RawValue;
                    if (point.Type == PointType.ANALOG_INPUT || point.Type == PointType.ANALOG_OUTPUT)
                        ((AnalogBase)point).EguValue = newValue.EguValue;
                    else
                        ((DigitalBase)point).State = newValue.State;

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

        private void PutPointsInDataBase(List<PointData> points)
        {
            _dataBase.Points.RemoveRange(_dataBase.Points.ToArray());
            
            foreach(PointData point in points)
            {
                BasePointItem newPoint = null;

                if(point.Type == PointType.ANALOG_INPUT || point.Type == PointType.ANALOG_OUTPUT)
                {
                    newPoint = point.Type == PointType.ANALOG_INPUT ? (BasePointItem)(new AnalogInput(point)) : (BasePointItem)(new AnalogOutput(point));
                }
                else if(point.Type == PointType.DIGITAL_INPUT || point.Type == PointType.DIGITAL_OUTPUT)
                {
                    newPoint = point.Type == PointType.DIGITAL_INPUT ? (BasePointItem)(new DigitalInput(point)) : (BasePointItem)(new DigitalOutput(point));
                }

                _dataBase.Points.Add(newPoint);
            }

            _dataBase.SaveChanges();
        }
    }
}