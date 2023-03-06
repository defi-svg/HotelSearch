using hotelsearch.Controllers;
using hotelsearch.Data;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging;

namespace TestHotelSearch
{
    public class HotelControllerTest
    {
        private readonly HotelController _controller;
        public HotelControllerTest()
        {
            _controller = new HotelController();
        }

        #region GetAll
        [Fact]
        public void GetAll_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.GetAll(0,0);
            // Assert
            Assert.IsType<Task<ActionResult<List<Hotel>>>>(okResult);
        }
        [Fact]
        public void GetAll_WhenCalled_ReturnsAllItems()
        {
            // Act
            var actionResult = _controller.GetAll(0,0);
            // Assert
            var items = Assert.IsType<OkObjectResult>(actionResult.Result.Result);
            List<Hotel> hotels = (List<Hotel>)items.Value;
            Assert.Equal(6, hotels.Count);
        }
        #endregion

        #region GetCount
        [Fact]
        public void GetCount_WhenCalled_ReturnInt()
        {
            // Act
            var okResult = _controller.GetCount();
            // Assert
            var aResult = Assert.IsType<OkObjectResult>(okResult.Result.Result);
            Assert.IsType<String>(aResult.Value);
        }
        #endregion

        #region GetByName
        [Fact]
        public void GetByName_WhenCalled_HotelNotFound()
        {
            // Act
            var actionResult = _controller.GetByName("Test");
            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult.Result.Result);
        }

        [Fact]
        public void GetByName_WhenCalled_HotelFound()
        {
            // Act
            var actionResult = _controller.GetByName("Ok");
            // Assert
            Assert.IsType<OkObjectResult>(actionResult.Result.Result);
        }

        #endregion



    }
}
