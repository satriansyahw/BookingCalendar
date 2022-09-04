using BookingCalendar.Dto.Request;
using BookingCalendar.Utils;

namespace BookingCalendar.UseCase
{
    public interface ILoginUseCase
    {
        Task<DataResponse> DoAuthentication(LoginReqDto reqDto, JwtSettings jwtSettings);
    }
}
