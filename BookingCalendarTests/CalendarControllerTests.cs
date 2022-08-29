

using BookingCalendar.Controllers;
using BookingCalendar.Dto.Request;
using BookingCalendar.Dto.Response;
using BookingCalendar.Models.Domain;
using BookingCalendar.Models.Instance;
using BookingCalendar.Models.Interface;
using BookingCalendar.UseCase;
using BookingCalendar.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Moq;
using System.Collections;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;

namespace BookingCalendarTests
{
    [TestClass]
    public class CalendarControllerTests
    {
        CalendarController controller = null;
        [TestInitialize]
        public void BeforeEachTest()
        {
            // Arrange
            var mock = new Mock<GeneralHelper>();
            controller = new CalendarController(logger);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //controller.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";
            var identity = controller.ControllerContext.HttpContext.User.Identity as ClaimsIdentity;
            identity.AddClaim(new Claim("un", "satriamilan"));

        }


        private readonly ILoggerFactory logger = LoggerFactory.Create(config =>
        {
            config.AddConsole();
        });

        [TestMethod]
        [DataTestMethod]
        [DataRow("")]
        [DataRow("2022-10-29 ")]
        [DataRow("2022-10-29 18:00")]
        [DataRow("2022-10-2A ")]
        [DataRow("")]
        public void Test_Post_CalDateWrong_Return_false(string calDate)
        {
            // Arrange
            KalendarReqDto dto = new KalendarReqDto { CalDate = calDate };
            var controller = new CalendarController(logger);

            //Act
            var response = controller.Post(dto).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(response.IsSuccess);
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow("")]
        [DataRow("12:90")]
        [DataRow("WR:23")]
        [DataRow("12:56 ")]
        [DataRow("")]
        public void Test_Post_CalTimeStartWrong_Return_false(string calTime)
        {
            // Arrange
            KalendarReqDto dto = new KalendarReqDto { CalTimeStart = calTime };
            var controller = new CalendarController(logger);

            //Act
            var response = controller.Post(dto).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(response.IsSuccess);
        }
        [TestMethod]
        [DataTestMethod]
        [DataRow("")]
        [DataRow("12:90")]
        [DataRow("WR:23")]
        [DataRow("12:56 ")]
        [DataRow("")]
        public void Test_Post_CalTimeEndWrong_Return_false(string calTime)
        {
            // Arrange
            KalendarReqDto dto = new KalendarReqDto { CalTimeEnd = calTime };
            var controller = new CalendarController(logger);

            //Act
            var response = controller.Post(dto).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(response.IsSuccess);
        }

        [TestMethod]
        public void Test_Post_AllDay_True_Return_True()
        {


            var currentDate = DateOnly.FromDateTime(DateTime.Now).AddYears(3456).ToString("yyyy-MM-dd");


            KalendarReqDto dtoTest = new KalendarReqDto
            {
                CalDate = currentDate,
                CalTimeStart = "09:00",
                CalTimeEnd = "09:30",
                IsAllDay = true,
                EventName = "event1"
            };

            IKalendar calDao = InsKalendar.GetKalendar();
            //Arrange

            DateOnly.TryParse(currentDate, out DateOnly calDate);

            Kalendar kalendar = new Kalendar
            {
                UserName = "satriamilan",
                EventName = "myEventNamex1",
                IsAllDay = true,
                CalTimeStart = new TimeOnly(00, 15),
                CallTimeEnd = new TimeOnly(00, 30)
            };
            kalendar.CalDate = calDate;

            //Act
            Kalendar kalendarSave = calDao.Save(kalendar).GetAwaiter().GetResult();
            var response = controller.Post(dtoTest).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual("Calendar event created  but conflict with other events", response.Message.Trim());
        }
        [TestMethod]
        public void Test_Post_AllDay_False_Return_True()
        {
            // Arrange

            var currentDate = DateOnly.FromDateTime(DateTime.Now).AddYears(3457).ToString("yyyy-MM-dd");


            KalendarReqDto dtoTest = new KalendarReqDto
            {
                CalDate = currentDate,
                CalTimeStart = "09:00",
                CalTimeEnd = "09:30",
                IsAllDay = false,
                EventName = "eventx"
            };

            IKalendar calDao = InsKalendar.GetKalendar();
            //Arrange

            DateOnly.TryParse(currentDate, out DateOnly calDate);

            Kalendar kalendar = new Kalendar
            {
                UserName = "satriamilan",
                EventName = "myEventNamex1",
                IsAllDay = false,
                CalTimeStart = new TimeOnly(00, 15),
                CallTimeEnd = new TimeOnly(00, 30)
            };
            kalendar.CalDate = calDate;

            //Act
            Kalendar kalendarSave = calDao.Save(kalendar).GetAwaiter().GetResult();
            var response = controller.Post(dtoTest).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual("Calendar event created", response.Message.Trim());
        }

        [TestMethod]
        public void Test_Patch0_KalendarId_0_Return_DataResponseIsSuccess_False()
        {
            // Arrange
            var currentDate = DateOnly.FromDateTime(DateTime.Now).AddYears(7497).ToString("yyyy-MM-dd");

            //Arrange
            Kalendar kalendar = new Kalendar();

            //Act
            var response = controller.Patch(new KalendarWithIdReqDto()).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(response.IsSuccess);

        }
        [TestMethod]
        public void Test_Patch1_KalendarId_GreaterThan_0_ButNoData_Return_DataResponseIsSuccess_False()
        {
            //Arrange
            KalendarWithIdReqDto dto = new KalendarWithIdReqDto { Id = 10000000000, EventName = "myEventName" };
            //Act
            CalendarUseCase calendarUseCase = new CalendarUseCase();
            DataResponse result = controller.Patch(dto).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(result.IsSuccess);

        }
        [TestMethod]
        public void Test_Patch2_KalendarId_GreaterThan_0_WithData_Return_DataResponseIsSuccess_True()
        {
            IKalendar calDao = InsKalendar.GetKalendar();
            // Arrange
            var currentDate = DateOnly.FromDateTime(DateTime.Now).AddYears(4497).ToString("yyyy-MM-dd");
            //Arrange
            Kalendar kalendar = new Kalendar { UserName = "satriamilan", EventName = "myEventName" };

            //Act
            Kalendar kalendarSave = calDao.Save(kalendar).GetAwaiter().GetResult();
            KalendarWithIdReqDto dto = new KalendarWithIdReqDto
            {
                Id = kalendarSave.Id,
                EventName = "myEventName",
                CalDate = currentDate,
                CalTimeStart = "09:00",
                CalTimeEnd = "09:30"
            };

            DataResponse result = controller.Patch(dto).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.IsSuccess);

        }
        [TestMethod]
        public void Test_Delete0_Id_0_Return_GenericResponseIsSuccess_False()
        {
            IKalendar calDao = InsKalendar.GetKalendar();
            //Arrange

            //Act
            GenericResponse result = controller.Delete(0).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(result.IsSuccess);

        }

        [TestMethod]
        public void Test_Delete2_Id_Greather_0_Return_GenericResponseIsSuccess_True()
        {
            IKalendar calDao = InsKalendar.GetKalendar();
            //Arrange
            var currentDate = DateOnly.FromDateTime(DateTime.Now).AddYears(3545).ToString("yyyy-MM-dd");
            DateOnly.TryParse(currentDate, out DateOnly calDate);
            Kalendar kalendar = new Kalendar
            {
                UserName = "satriamilan",
                EventName = "myEventName2A",
                IsAllDay = false,
                CalTimeStart = new TimeOnly(00, 15),
                CallTimeEnd = new TimeOnly(00, 30)
            };
            kalendar.CalDate = calDate;

            //Act
            Kalendar kalendarSave = calDao.Save(kalendar).GetAwaiter().GetResult();

            GenericResponse result = controller.Delete(kalendarSave.Id).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.IsSuccess);

        }
        [TestMethod]
        public void Test_Delete3_Id_NotFound_Return_GenericResponseIsSuccess_False()
        {
            //Arrange

            //Act

            GenericResponse result = controller.Delete(2345609482588).GetAwaiter().GetResult();

            //Asserts
            Assert.IsFalse(result.IsSuccess);

        }

        [TestMethod]
        public void Test_CheckAvailibility0_IsAllDay_True_Return_GenericResponseIsSuccess_False()
        {
            IKalendar calDao = InsKalendar.GetKalendar();
            //Arrange
            var currentDate = DateOnly.FromDateTime(DateTime.Now).AddYears(1490).ToString("yyyy-MM-dd");
            DateOnly.TryParse(currentDate, out DateOnly calDate);
            Kalendar kalendar = new Kalendar
            {
                UserName = "satriamilan",
                EventName = "myEventName",
                IsAllDay = true,
                CalTimeStart = new TimeOnly(00, 15),
                CallTimeEnd = new TimeOnly(00, 30)
            };
            kalendar.CalDate = calDate;

            KalendarReqDto reqDto = new KalendarReqDto 
            {
                EventName = "myEventName",
                IsAllDay = true,
                CalTimeStart ="00:31",
                CalTimeEnd ="00:45",
                CalDate=currentDate
            };

            //Act
            Kalendar kalendarSave = calDao.Save(kalendar).GetAwaiter().GetResult();

            GenericResponse result = controller.PostCheckAvailability(reqDto).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(result.IsSuccess);

        }
        [TestMethod]
        public void Test_CheckAvailibility1_IsAllDay_False_DifferentTime_Return_GenericResponseIsSuccess_True()
        {
            IKalendar calDao = InsKalendar.GetKalendar();
            //Arrange
            var currentDate = DateOnly.FromDateTime(DateTime.Now).AddYears(3445).ToString("yyyy-MM-dd");
            DateOnly.TryParse(currentDate, out DateOnly calDate);
            Kalendar kalendar = new Kalendar
            {
                UserName = "satriamilan",
                EventName = "myEventName1A",
                IsAllDay = false,
                CalTimeStart = new TimeOnly(00, 15),
                CallTimeEnd = new TimeOnly(00, 30)
            };
            kalendar.CalDate = calDate;

            KalendarReqDto reqDto = new KalendarReqDto
            {
                EventName = "myEventName",
                IsAllDay = false,
                CalTimeStart = "00:31",
                CalTimeEnd = "00:45",
                CalDate = currentDate
            };

            //Act
            Kalendar kalendarSave = calDao.Save(kalendar).GetAwaiter().GetResult();

            GenericResponse result = controller.PostCheckAvailability(reqDto).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.IsSuccess);

        }
        [TestMethod]
        public void Test_CheckAvailibility2_IsAllDay_False_SameTime_Return_GenericResponseIsSuccess_False()
        {
            IKalendar calDao = InsKalendar.GetKalendar();
            //Arrange
            var currentDate = DateOnly.FromDateTime(DateTime.Now).AddYears(1645).ToString("yyyy-MM-dd");
            DateOnly.TryParse(currentDate, out DateOnly calDate);
            Kalendar kalendar = new Kalendar
            {
                UserName = "satriamilan",
                EventName = "myEventName2A",
                IsAllDay = false,
                CalTimeStart = new TimeOnly(00, 15),
                CallTimeEnd = new TimeOnly(00, 30)
            };
            kalendar.CalDate = calDate;

            KalendarReqDto reqDto = new KalendarReqDto
            {
                EventName = "myEventName",
                IsAllDay = false,
                CalTimeStart = "00:25",
                CalTimeEnd = "00:45",
                CalDate = currentDate
            };

            //Act
            Kalendar kalendarSave = calDao.Save(kalendar).GetAwaiter().GetResult();

            GenericResponse result = controller.PostCheckAvailability(reqDto).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(result.IsSuccess);

        }
        [TestMethod]
        public void Test_Get_Id_Found_Return_DataResponseData_Id_Greater_0()
        {
            IKalendar calDao = InsKalendar.GetKalendar();
            //Arrange
            var currentDate = DateOnly.FromDateTime(DateTime.Now).AddYears(1585).ToString("yyyy-MM-dd");
            DateOnly.TryParse(currentDate, out DateOnly calDate);
            Kalendar kalendar = new Kalendar
            {
                UserName = "satriamilan",
                EventName = "myEventName2A1",
                IsAllDay = false,
                CalTimeStart = new TimeOnly(00, 15),
                CallTimeEnd = new TimeOnly(00, 30)
            };
            kalendar.CalDate = calDate;

            //Act
            Kalendar kalendarSave = calDao.Save(kalendar).GetAwaiter().GetResult();

            DataResponse result = controller.GetById(kalendarSave.Id).GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual(kalendarSave.Id, ((KalendarResDto)result.Data).Id);
        }

        [TestMethod]
        public void Test_Get_FoundUserName_Return_DataResponseData_Count_Greater_0()
        {
            IKalendar calDao = InsKalendar.GetKalendar();
            //Arrange
            var currentDate = DateOnly.FromDateTime(DateTime.Now).AddYears(1545).ToString("yyyy-MM-dd");
            DateOnly.TryParse(currentDate, out DateOnly calDate);
            Kalendar kalendar = new Kalendar
            {
                UserName = "satriamilan",
                EventName = "myEventName2A1",
                IsAllDay = false,
                CalTimeStart = new TimeOnly(00, 15),
                CallTimeEnd = new TimeOnly(00, 30)
            };
            kalendar.CalDate = calDate;

            //Act
            Kalendar kalendarSave = calDao.Save(kalendar).GetAwaiter().GetResult();

            DataResponse result = controller.Get().GetAwaiter().GetResult();
            var listDataCount = ((IEnumerable)result.Data).Cast<object>().ToList().Count();

            //Assert
            Assert.AreNotEqual(0, listDataCount);
        }
    }
}