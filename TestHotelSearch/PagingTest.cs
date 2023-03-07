
using hotelsearch.Data;
using System.Collections.Generic;
using Xunit;

namespace TestHotelSearch
{
    public class PagingTest
    {
        private static List<Hotel> testhotels = new List<Hotel>
             {
                new Hotel{
                    Id=1,
                    Name="Best",
                    Price=100.00,
                    GeoLocation= new Location{ Lat=44.87336673652513,Long=13.85024579793014}
                },
                new Hotel{
                    Id=2,
                    Name="Medium",
                    Price=50.00,
                    GeoLocation= new Location{ Lat=44.86337895183883,Long=13.841254087479248}
                }
             };



        [Fact]
        public void Paging_WhenCalled_ListEmpty()
        {
            List<Hotel> list = new List<Hotel>();
            // Act
            var result = hotelsearch.Utils.Paging.PageFilter(list, 3, 1);
            // Assert
            Assert.IsType<List<Hotel>>(result);
        }

        [Fact]
        public void Paging_WhenCalled_PagingZero()
        {
            // Act
            var result = hotelsearch.Utils.Paging.PageFilter(testhotels, 0, 0);
            // Assert
            Assert.IsType<List<Hotel>>(result);
        }


    }
}
