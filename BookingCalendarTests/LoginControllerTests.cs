using BookingCalendar.Controllers;
using BookingCalendar.Dto.Request;
using BookingCalendar.Utils;
using Microsoft.Extensions.Logging;

namespace BookingCalendarTests
{
    [TestClass]
    public class LoginControllerTests
    {
        private readonly ILoggerFactory logger = LoggerFactory.Create(config =>
        {
            config.AddConsole();
        });

        [TestMethod]
        public void Test_Post_Login_EmptyUserName_Return_false()
        {
            // Arrange
            LoginReqDto reqDto = new LoginReqDto { UserName = "" };
            JwtSettings jwtSettings = new JwtSettings { ValidIssuer = "mylocalhost", IssuerSigningKey = "55HG5T6S-900L-0912-0987-HG67KR982AYQ", TokenExpiredInMinutes = 1 };
            var controller =new LoginController(jwtSettings, logger);

            //Act
            var response = controller.Post(reqDto).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(response.IsSuccess);
        }

        [TestMethod]
        public void Test_Post_Login_EmptyValidIssuer_Return_false()
        {
            // Arrange
            LoginReqDto reqDto = new LoginReqDto { UserName = "" };
            JwtSettings jwtSettings = new JwtSettings { ValidIssuer = "", IssuerSigningKey = "55HG5T6S-900L-0912-0987-HG67KR982AYQ", TokenExpiredInMinutes = 1 };
            var controller = new LoginController(jwtSettings, logger);

            //Act
            var response = controller.Post(reqDto).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(response.IsSuccess);
        }
        [TestMethod]
        public void Test_Post_Login_CompleteData_Return_True()
        {
            // Arrange
            LoginReqDto reqDto = new LoginReqDto { UserName = "satriamilan" };
            JwtSettings jwtSettings = new JwtSettings { ValidIssuer = "mylocalhost", IssuerSigningKey = "55HG5T6S-900L-0912-0987-HG67KR982AYQ", TokenExpiredInMinutes = 1 };
            var controller = new LoginController(jwtSettings, logger);

            //Act
            var response = controller.Post(reqDto).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(response.IsSuccess);
        }
    }
}
