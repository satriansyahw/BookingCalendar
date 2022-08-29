using BookingCalendar.Dto.Request;
using BookingCalendar.Models.Domain;
using BookingCalendar.Models.Instance;
using BookingCalendar.Models.Interface;
using BookingCalendar.UseCase;
using BookingCalendar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingCalendarTests
{
    [TestClass]
    public class CalendarUseCaseTests
    {
       

        [TestMethod]
        public void Test_Save0_KalendarEmpty_Return_DataResponseIsSuccess_False()
        {
            //Arrange
            Kalendar kalendar = new Kalendar();

            //Act
            CalendarUseCase calendarUseCase = new CalendarUseCase();
            DataResponse result = calendarUseCase.Save(kalendar).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(result.IsSuccess);

        }
        [TestMethod]
        public void Test_Save1_KalendarNotEmpty_Return_DataResponseIsSuccess_True()
        {
            //Arrange
            Kalendar kalendar = new Kalendar { UserName = "satriamilan", EventName = "myEventName" };

            //Act
            CalendarUseCase calendarUseCase = new CalendarUseCase();
            DataResponse result = calendarUseCase.Save(kalendar).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.IsSuccess);

        }
        [TestMethod]
        public void Test_Update0_KalendarId_0_Return_DataResponseIsSuccess_False()
        {
            //Arrange
            Kalendar kalendar = new Kalendar();

            //Act
            CalendarUseCase calendarUseCase = new CalendarUseCase();
            DataResponse result = calendarUseCase.Update(kalendar).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(result.IsSuccess);

        }
        [TestMethod]
        public void Test_Update1_KalendarId_GreaterThan_0_ButNoData_Return_DataResponseIsSuccess_False()
        {
            //Arrange
            Kalendar kalendar = new Kalendar { Id = 10000000000, UserName = "satriamilan", EventName = "myEventName" };

            //Act
            CalendarUseCase calendarUseCase = new CalendarUseCase();
            DataResponse result = calendarUseCase.Update(kalendar).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(result.IsSuccess);

        }
        [TestMethod]
        public  void Test_Update2_KalendarId_GreaterThan_0_WithData_Return_DataResponseIsSuccess_True()
        {
            IKalendar calDao = InsKalendar.GetKalendar();
            //Arrange
            Kalendar kalendar = new Kalendar { Id = 1, UserName = "satriamilan", EventName = "myEventName" };

            //Act
            CalendarUseCase calendarUseCase = new CalendarUseCase();
            Kalendar kalendarSave =  calDao.Save(kalendar).GetAwaiter().GetResult();
            kalendar.Id = kalendarSave.Id;

            DataResponse result = calendarUseCase.Update(kalendar).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.IsSuccess);

        }
        [TestMethod]
        public void Test_CheckAvailibility0_IsAllDay_True_Return_GenericResponseIsSuccess_False()
        {
            IKalendar calDao = InsKalendar.GetKalendar();
            //Arrange
            var currentDate = DateOnly.FromDateTime(DateTime.Now).AddYears(1000).ToString("yyyy-MM-dd");
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

            //Act
            CalendarUseCase calendarUseCase = new CalendarUseCase();
            Kalendar kalendarSave = calDao.Save(kalendar).GetAwaiter().GetResult();

            GenericResponse result = calendarUseCase.CheckAvailibility(kalendar).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(result.IsSuccess);

        }
        [TestMethod]
        public void Test_CheckAvailibility1_IsAllDay_False_DifferentTime_Return_GenericResponseIsSuccess_True()
        {
            IKalendar calDao = InsKalendar.GetKalendar();
            //Arrange
            var currentDate = DateOnly.FromDateTime(DateTime.Now).AddYears(1345).ToString("yyyy-MM-dd");
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

            Kalendar kalendarCheck = new Kalendar
            {
                UserName = "satriamilan",
                EventName = "myEventNameiB",
                IsAllDay = false,
                CalTimeStart = new TimeOnly(00, 31),
                CallTimeEnd = new TimeOnly(00, 40)
            };
            kalendarCheck.CalDate = calDate;

            //Act
            CalendarUseCase calendarUseCase = new CalendarUseCase();
            Kalendar kalendarSave = calDao.Save(kalendar).GetAwaiter().GetResult();

            GenericResponse result = calendarUseCase.CheckAvailibility(kalendarCheck).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.IsSuccess);

        }
        [TestMethod]
        public void Test_CheckAvailibility2_IsAllDay_False_SameTime_Return_GenericResponseIsSuccess_False()
        {
            IKalendar calDao = InsKalendar.GetKalendar();
            //Arrange
            var currentDate = DateOnly.FromDateTime(DateTime.Now).AddYears(1545).ToString("yyyy-MM-dd");
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

            Kalendar kalendarCheck = new Kalendar
            {

                UserName = "satriamilan",
                EventName = "myEventName2B",
                IsAllDay = false,
                CalTimeStart = new TimeOnly(00, 25),
                CallTimeEnd = new TimeOnly(00, 40)
            };
            kalendarCheck.CalDate = calDate;

            //Act
            CalendarUseCase calendarUseCase = new CalendarUseCase();
            Kalendar kalendarSave = calDao.Save(kalendar).GetAwaiter().GetResult();

            GenericResponse result = calendarUseCase.CheckAvailibility(kalendarCheck).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(result.IsSuccess);

        }

        [TestMethod]
        public void Test_Delete0_Id_0_Return_GenericResponseIsSuccess_False()
        {
            IKalendar calDao = InsKalendar.GetKalendar();
            //Arrange
           
            //Act
            CalendarUseCase calendarUseCase = new CalendarUseCase();
            GenericResponse result = calendarUseCase.Delete(0).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(result.IsSuccess);

        }

        [TestMethod]
        public void Test_Delete2_Id_Greather_0_Return_GenericResponseIsSuccess_True()
        {
            IKalendar calDao = InsKalendar.GetKalendar();
            //Arrange
            var currentDate = DateOnly.FromDateTime(DateTime.Now).AddYears(1545).ToString("yyyy-MM-dd");
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
            CalendarUseCase calendarUseCase = new CalendarUseCase();
            Kalendar kalendarSave = calDao.Save(kalendar).GetAwaiter().GetResult();

            GenericResponse result = calendarUseCase.Delete(kalendarSave.Id).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.IsSuccess);

        }
        [TestMethod]
        public void Test_Delete3_Id_NotFound_Return_GenericResponseIsSuccess_False()
        {
            //Arrange
         
            //Act
            CalendarUseCase calendarUseCase = new CalendarUseCase();

            GenericResponse result = calendarUseCase.Delete(2345609482588).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(result.IsSuccess);

        }

    }
}
