using BookingCalendar.Dto.Request;
using BookingCalendar.UseCase;
using BookingCalendar.Utils;

namespace BookingCalendarTests
{
    [TestClass]
    public class LoginUseCaseTests
    {
        [TestMethod]
        public void Test_DoAuthentication_JwtSettingsEmpty_Return_DataResponseIsSuccess_False()
        {
            //Arrange
            LoginReqDto reqDto = new LoginReqDto { UserName = "satriamilan" };
            JwtSettings jwtSettings = new JwtSettings();

            //Act
            LoginUseCase loginUseCase = new LoginUseCase();
            DataResponse result = loginUseCase.DoAuthentication(reqDto, jwtSettings).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(result.IsSuccess);

        }

        [TestMethod]
        public void Test_DoAuthentication_JwtSettingsNotEmpty_Return_DataResponseIsSuccess_True()
        {
            //Arrange
            LoginReqDto reqDto = new LoginReqDto { UserName = "satriamilan" };
            JwtSettings jwtSettings = new JwtSettings { ValidIssuer = "mylocalhost", IssuerSigningKey = "55HG5T6S-900L-0912-0987-HG67KR982AYQ", TokenExpiredInMinutes = 1 };

            //Act
            LoginUseCase loginUseCase = new LoginUseCase();
            DataResponse result = loginUseCase.DoAuthentication(reqDto, jwtSettings).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.IsSuccess);

        }
    }
}