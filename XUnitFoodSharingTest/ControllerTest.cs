using Moq;
using WebAPI.Authentication;
using WebAPI.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using FoodSharing.Models;

namespace XUnitFoodSharingTest
{
    public class ControllerTests
    {
        private readonly Mock<ApplicationDbContext> _mockApp;
        private readonly FoodsController _foodsController;
        public ControllerTests()
        {
            _mockApp = new Mock<ApplicationDbContext>();
            _foodsController = new FoodsController(_mockApp.Object);
        }

        [Fact]
        public void Index_ActionExecutes_ReturnsViewForIndex()
        {
            var result = _foodsController.GetFood("1");
            Assert.IsType<Food>(result);
        }
    }
}
