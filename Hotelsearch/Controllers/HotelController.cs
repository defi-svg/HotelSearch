using hotelsearch.Data;
using hotelsearch.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
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
                    Name="Amfiteatar",
                    Price=100.00,
                    GeoLocation= new Location{ Lat=44.87233861773503,Long=13.848351117878774}
                },
            new Hotel{
                    Id=2,
                    Name="Histria",
                    Price=50.00,
                    GeoLocation= new Location{ Lat=44.83545063396626,Long=13.840489817515614}
                },
            new Hotel{
                    Id=3,
                    Name="Riviera",
                    Price=15.00,
                    GeoLocation= new Location{ Lat=44.8763120435846,Long=13.849076297930283}
                },
            new Hotel{
                    Id=4,
                    Name="Karmen",
                    Price=25.00,
                    GeoLocation= new Location{ Lat=44.91732221354066,Long=13.767732736008423}
                },
            new Hotel{
                    Id=5,
                    Name="Brioni",
                    Price=125.00,
                    GeoLocation= new Location{ Lat=44.83892326490474,Long=13.830629070959796}
                },
            new Hotel{
                    Id=6,
                    Name="Lone",
                    Price=350,
                    GeoLocation= new Location{ Lat=45.073198967494804,Long=13.63922152862437}
                }
        };

        /// <summary>
        /// Get all the hotels from the example list
        /// </summary>
        /// <param name="pageNumber">what page number</param>
        /// <param name="pageSize">how many record per page</param>
        /// <returns>list of all hotels</returns>
        [SwaggerOperation(Description = "Gets the list with all the Hotels without any condition")]
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
        /// <returns>total number</returns>
        [SwaggerOperation(Description = "Gets the number of objects found in the existing Hotel list")]
        [HttpGet("Count")]
        public async Task<ActionResult<int>> GetCount()
        {
            if (hotels.Count <= 1)
                return Ok($"There is {hotels.Count} hotel available");
            return Ok($"There is {hotels.Count} hotels available");
        }

        /// <summary>
        /// Get the Hotel searched by name
        /// </summary>
        /// <param name="hotelname">name of the hotel</param>
        /// <returns>Hotel object with that name if found else return a Bad request</returns>
        [SwaggerOperation(Description = "Search a the Hotel in the list by name. If not found return Bad Request with informative text")]
        [HttpGet("{hotelname}")]
        public async Task<ActionResult<Hotel>> GetByName(string hotelname)
        {
            var hotel = hotels.Find(a => a.Name == hotelname);
            if (hotel==null)
            {
                _logger?.LogInformation($"Can't find hotel: {hotelname}");
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
        [SwaggerOperation(Description = "Sort the list of Hotels by price (low -> high) and distance (near -> far) and the return the sorted list")]
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
                    _logger?.LogInformation($"Error with calculating the distance for the hotel: {hotels[i].Name}");
                    return BadRequest("Error with calculating the distance between current position and the hotel");
                }

                hotels[i].Distance = distance;
            }

            //get the ordered list of the hotels by price (cheap->high) and distance (near->far)
            var hotelsSort=hotels.OrderBy(a=>a.Price).ThenBy(a=>a.Distance).ToList();

            if (hotelsSort == null)
            {
                _logger?.LogInformation($"There is no hotel near you");
                return BadRequest("There is no hotel");
            }

            var response = Paging.PageFilter(hotelsSort, pageNumber, pageSize);

            return Ok(response);
        }


        /// <summary>
        /// Add new hotel in the existing list
        /// </summary>
        /// <param name="hotel"></param>
        /// <param name="pageNumber">what page number</param>
        /// <param name="pageSize">how many record per page</param>
        /// <returns></returns>
        [SwaggerOperation(Description = "Add a new Hotel object in the existing list")]
        [HttpPost]
        public async Task<ActionResult<List<Hotel>>> AddHotel(Hotel hotel,int pageNumber, int pageSize)
        {
            if(hotel is null)
            {
                return BadRequest("No hotel to add");
            }

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
        [SwaggerOperation(Description = "Update all the values changed on a specific Hotel. Update the complete Hotel object")]
        [HttpPut]
        public async Task<ActionResult<List<Hotel>>> UpdateHotel(Hotel requestHotel, int pageNumber, int pageSize)
        {
            //verify if hotel that we want to edit exists
            var hotel = hotels.Find(a => a.Id == requestHotel?.Id);
            if (hotel == null || requestHotel==null)
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
        [SwaggerOperation(Description = "Delete Hotel object using the Hotel object passed as parameter. Reeturns the list of hotels that remains after the delete")]
        [HttpDelete]
        public async Task<ActionResult<List<Hotel>>> DeleteHotel(Hotel deleteHotel, int pageNumber, int pageSize)
        {
            //verify if hotel that we want to delete exists
            var hotel = hotels.Find(a => a.Id == deleteHotel?.Id);
            if (hotel == null)
            {
                return BadRequest("Hotel not found");
            }

            hotels.Remove(hotel);
            var response = Paging.PageFilter(hotels, pageNumber, pageSize);

            return Ok(response);
        }


        /// <summary>
        /// Delete hotel by hotel id
        /// </summary>
        /// <param name="id">id of the hotel</param>
        /// <param name="pageNumber">what page number</param>
        /// <param name="pageSize">how many record per page</param>
        /// <returns>list of all hotels that reamins after the delete</returns>
        [SwaggerOperation(Description  = "Delete Hotel object using the Hotels Id passed as parameter. Reeturns the list of hotels that remains after the delete")]
        [HttpDelete("ById")]
        public async Task<ActionResult<List<Hotel>>> DeleteHotelById(int id, int pageNumber, int pageSize)
        {
            //verify if hotel that we want to delete exists
            var hotel = hotels.Find(a => a.Id == id);
            if (hotel == null)
            {
                return BadRequest("Hotel not found");
            }

            hotels.Remove(hotel);
            var response = Paging.PageFilter(hotels, pageNumber, pageSize);

            return Ok(response);
        }



    }
}
