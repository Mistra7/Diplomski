using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public PointController()
        {

        }

        [HttpGet]
        [Route("Connect")]
        public async Task<Object> Connect()
        {
            client = new WCFClient(new NetTcpBinding(), new EndpointAddress(new Uri("net.tcp://localhost:9999/Server")));

            List<IPoint> pointsList = null;

            pointsList = client.Connect();

            if(pointsList == null)
            {
                return NotFound("ConnectionFailiure");
            }
            else
            {
                pointsList.ForEach(point => points.Add(point.PointId, point as BasePointItem));
                return Ok(points.Values);
            }
        }

        [HttpGet]
        [Route("ReadSingleRegister")]
        public async Task<Object> ReadSingleRegister(int pid)
        {
            try
            {
                IPoint point = client.ReadCommand(points[pid]);
                points[pid] = point as BasePointItem;
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
                var conf = new ConfigItem();
                var point1 = new AnalogOutput(conf, pid);
                IPoint point = client.ReadCommand(point1);
                points[pid] = point as BasePointItem;
                return Ok(point);
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet]
        [Route("GetMinAndMaxValue")]
        public async Task<Object> GetMinAndMaxValue(int pid)
        {
            try
            {
                //var minValue = points[pid].ConfigItem.MinValue;
                //var maxValue = points[pid].ConfigItem.MaxValue;
                var minValue = 0;
                var maxValue = 1000;
                return new
                {
                    minValue,
                    maxValue
                };
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }
        }
    }
}