using hotelsearch.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotelsearch.Utils
{
    public class GeoDistance
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Algorithm to calculate the distance between two coordinates
        /// </summary>
        /// <param name="currentLocation">user location</param>
        /// <param name="hotelLocation">hotel location</param>
        /// <returns>meters of distance between to locations</returns>
        public double CalculateDistace(Location currentLocation, Location hotelLocation)
        {
            try
            {
                var d1 = currentLocation.Lat * (Math.PI / 180.0);
                var num1 = currentLocation.Long * (Math.PI / 180.0);
                var d2 = hotelLocation.Lat * (Math.PI / 180.0);
                var num2 = hotelLocation.Long * (Math.PI / 180.0) - num1;
                var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

                return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
            }
            catch(Exception ex)
            {
                _logger.LogError("CalulateDistance", ex);
                return -1;
            }
        }



        


    }
}
