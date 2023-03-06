using hotelsearch.Data;
using hotelsearch.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotelsearch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : ControllerBase
    {

        private readonly ILogger<HotelController> _logger;

        //list of hotels
        private static List<Hotel> hotels = new List<Hotel>
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
                },
            new Hotel{
                    Id=3,
                    Name="Terrible",
                    Price=15.00,
                    GeoLocation= new Location{ Lat=44.886133669638184,Long=13.847889563718066}
                },
            new Hotel{
                    Id=4,
                    Name="Ok",
                    Price=25.00,
                    GeoLocation= new Location{ Lat=44.89034794779482,Long=13.845543074542379}
                },
            new Hotel{
                    Id=5,
                    Name="Ideal",
                    Price=125.00,
                    GeoLocation= new Location{ Lat=44.8619614184365,Long=13.805077206000158}
                },
             new Hotel{
                    Id=6,
                    Name="Nightmare",
                    Price=15,
                    GeoLocation= new Location{ Lat=44.88390715383354,Long=13.900494951663815}
                },
        };

        /// <summary>
        /// Get all the hotels from the example list
        /// </summary>
        /// <param name="pageNumber">what page number</param>
        /// <param name="pageSize">how many record per page</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Hotel>>> GetAll(int pageNumber, int pageSize)
        {
            if (hotels.Count==0)
            {
                return Ok("There is 0 hotel available");
            }

            var response = Paging.PageFilter(hotels, pageNumber, pageSize);
            return Ok(response);
        }
        
        /// <summary>
        /// Get number of hotels in the list
        /// </summary>
        /// <returns></returns>
        [HttpGet("Count")]
        public async Task<ActionResult<int>> GetCount()
        {
            if (hotels.Count <= 1)
                return Ok($"There is {hotels.Count} hotel available");
            return Ok($"There is {hotels.Count} hotels available");
        }


        [HttpGet("{hotelname}")]
        public async Task<ActionResult<Hotel>> GetByName(string hotelname)
        {
            var hotel = hotels.Find(a => a.Name == hotelname);
            if (hotel==null)
            {
                _logger.LogInformation($"Can't find hotel: {hotelname}");
                return BadRequest("Hotel not found");
            }
            return Ok(hotel);
        }



        /// <summary>
        /// Get list of hotels sort by price and distance
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="pageNumber">what page number</param>
        /// <param name="pageSize">how many record per page</param>
        /// <returns>sorted list of Hotel items</returns>
        [HttpGet("{latitude}:{longitude}")]
        public async Task<ActionResult<List<Hotel>>> GetByDistanceFromCurrentPosition(double latitude, double longitude,int pageNumber, int pageSize)
        {
            var currentlocation = new Location { Lat = latitude, Long = longitude };
            GeoDistance geo = new GeoDistance();

            //calculate distance difference from user location for each hotel from the list
            for (int i = 0; i < hotels.Count(); i++)
            {
                var distance = geo.CalculateDistace(currentlocation, hotels[i].GeoLocation);
                if(distance==-1)
                {
                    _logger.LogInformation($"Error with calculating the distance for the hotel: {hotels[i].Name}");
                    return BadRequest("Error with calculating the distance between current position and the hotel");
                }

                hotels[i].Distance = distance;
            }

            //get the ordered list of the hotels by price (cheap->high) and distance (near->far)
            var hotelsSort=hotels.OrderBy(a=>a.Price).ThenBy(a=>a.Distance).ToList();

            if (hotelsSort == null)
            {
                _logger.LogInformation($"There is no hotel near you");
                return BadRequest("There is no hotel");
            }

            var response = Paging.PageFilter(hotelsSort, pageNumber, pageSize);

            return Ok(response);
        }


        /// <summary>
        /// Add new hotel in the base list
        /// </summary>
        /// <param name="hotel"></param>
        /// <param name="pageNumber">what page number</param>
        /// <param name="pageSize">how many record per page</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<List<Hotel>>> AddHotel(Hotel hotel,int pageNumber, int pageSize)
        {
            hotels.Add(hotel);

            var response = Paging.PageFilter(hotels, pageNumber, pageSize);

            return Ok(response);
        }

        /// <summary>
        /// Update values of a specific Hotel 
        /// </summary>
        /// <param name="requestHotel"></param>
        /// <param name="pageNumber">what page number</param>
        /// <param name="pageSize">how many record per page</param>
        /// <returns>list of all hotels</returns>
        [HttpPut]
        public async Task<ActionResult<List<Hotel>>> UpdateHotel(Hotel requestHotel, int pageNumber, int pageSize)
        {
            //verify if hotel that we want to edit exists
            var hotel = hotels.Find(a => a.Id == requestHotel.Id);
            if (hotel == null)
            {
                return BadRequest("Hotel not found");
            }
            
            // update the values of the hotel with the new values
            hotel.Name = requestHotel.Name;
            hotel.GeoLocation = requestHotel.GeoLocation;
            hotel.Price = requestHotel.Price;

            var response = Paging.PageFilter(hotels, pageNumber, pageSize);

            return Ok(response);
        }

        /// <summary>
        /// Delete hotel
        /// </summary>
        /// <param name="deleteHotel"></param>
        /// <param name="pageNumber">what page number</param>
        /// <param name="pageSize">how many record per page</param>
        /// <returns>list of all hotels that reamins after the delete</returns>
        [HttpDelete]
        public async Task<ActionResult<List<Hotel>>> DeleteHotel(Hotel deleteHotel, int pageNumber, int pageSize)
        {
            //verify if hotel that we want to delete exists
            var hotel = hotels.Find(a => a.Id == deleteHotel.Id);
            if (hotel == null)
            {
                return BadRequest("Hotel not found");
            }

            hotels.Remove(deleteHotel);
            var response = Paging.PageFilter(hotels, pageNumber, pageSize);

            return Ok(response);
        }


        
    }
}
