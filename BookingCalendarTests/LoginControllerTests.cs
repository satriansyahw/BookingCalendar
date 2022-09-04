using BookingCalendar.Controllers;
using BookingCalendar.Dto.Request;
using BookingCalendar.Models.Instance;
using BookingCalendar.UseCase;
using BookingCalendar.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;

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
        public void cobaMock()
        {
            //Arrange
            var mock = new Mock<ILoginUseCase>();
            var mockLogger = new Mock<ILoggerFactory>();
            var mockJwt = new Mock<JwtSettings>();

            var controller = new LoginController(mockJwt.Object, mockLogger.Object,mock.Object);

            //Act

            var mockResult = new Mock<DataResponse>();
            DataResponse dr = new DataResponse(true, "failed token creation","1");
            var x = new LoginReqDto();
            mock.Setup(a => a.DoAuthentication( x,mockJwt.Object)).ReturnsAsync(dr);
            //  .Returns(Task.FromResult<ArticleDbModel?>(articleDbModel));
            var response = controller.Post(x).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(response.IsSuccess);
        }
        //[TestMethod]
        //public void Test_Post_Login_EmptyUserName_Return_false()
        //{
        //    // Arrange
        //    LoginReqDto reqDto = new LoginReqDto { UserName = "" };
        //    JwtSettings jwtSettings = new JwtSettings { ValidIssuer = "mylocalhost", IssuerSigningKey = "55HG5T6S-900L-0912-0987-HG67KR982AYQ", TokenExpiredInMinutes = 1 };
        //    var controller =new LoginController(jwtSettings, logger);

        //    //Act
        //    var response = controller.Post(reqDto).GetAwaiter().GetResult();

        //    //Assert
        //    Assert.IsFalse(response.IsSuccess);
        //}

        //[TestMethod]
        //public void Test_Post_Login_EmptyValidIssuer_Return_false()
        //{
        //    // Arrange
        //    LoginReqDto reqDto = new LoginReqDto { UserName = "" };
        //    JwtSettings jwtSettings = new JwtSettings { ValidIssuer = "", IssuerSigningKey = "55HG5T6S-900L-0912-0987-HG67KR982AYQ", TokenExpiredInMinutes = 1 };
        //    var controller = new LoginController(jwtSettings, logger);

        //    //Act
        //    var response = controller.Post(reqDto).GetAwaiter().GetResult();

        //    //Assert
        //    Assert.IsFalse(response.IsSuccess);
        //}
        //[TestMethod]
        //public void Test_Post_Login_CompleteData_Return_True()
        //{
        //    // Arrange
        //    LoginReqDto reqDto = new LoginReqDto { UserName = "satriamilan" };
        //    JwtSettings jwtSettings = new JwtSettings { ValidIssuer = "mylocalhost", IssuerSigningKey = "55HG5T6S-900L-0912-0987-HG67KR982AYQ", TokenExpiredInMinutes = 1 };
        //    var controller = new LoginController(jwtSettings, logger);

        //    //Act
        //    var response = controller.Post(reqDto).GetAwaiter().GetResult();

        //    //Assert
        //    Assert.IsTrue(response.IsSuccess);
        //}
    }
}
