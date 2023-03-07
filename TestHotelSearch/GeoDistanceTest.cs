using hotelsearch.Data;
using hotelsearch.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestHotelSearch
{
    public class GeoDistanceTest
    {
        private readonly GeoDistance _geodistance;
        public GeoDistanceTest()
        {
            _geodistance = new GeoDistance();
        }

        #region CalculateDistace
        [Fact]
        public void CalculateDistance_WhenCalled_LocationZero()
        {
            var currentLocation = new Location { Lat = 0, Long = 0 };
            var hotelLocation= new Location { Lat = 0, Long = 0 };
            // Act
            var result = _geodistance.CalculateDistace(currentLocation, hotelLocation);
            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void CalculateDistance_WhenCalled_LocationNull()
        {
            var hotelLocation = new Location { Lat = 0, Long = 0 };
            // Act
            var result = _geodistance.CalculateDistace(null, hotelLocation);
            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void CalculateDistance_WhenCalled_Location()
        {
            var currentLocation = new Location { Lat = 44.888490460386855, Long = 13.848393013359066 };
            var hotelLocation = new Location { Lat = 44.87336673652513, Long = 13.85024579793014 };
            // Act
            var result = _geodistance.CalculateDistace(currentLocation, hotelLocation);
            // Assert
            Assert.IsType<double>(result);
        }

        #endregion
    }
}
