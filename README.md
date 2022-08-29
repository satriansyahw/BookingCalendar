# BookingCalendar

A simple Api to create calendar

## Tech stacks :
1.Dotnet 6.0  with C# <br/>
2.EFCore with InMemory Database <br/>
3.Jwt Token <br/>

## Running
from the soluton folder <br/>
dotnet run --project ./BookingCalendar/BookingCalendar/BookingCalendar.csproj  <br/>
running Test Projects <br/>
dotnet test ./BookingCalendarTests/BookingCalendarTests.csproj  <br/>

IDE : Visual Studio <br/>

Available API : <br/>
1. POST {{base_url}}/login, to get auth, this auth will be used for accessing other  endpoints <br/>
2. POST {{base_url}}/Calendar, to create calendar <br/>
3. POST {{base_url}}/Calendar/availibility , to check availibility, detect conflicting appointments. <br/>
4. PATCH {{base_url}}/Calendar , to update value of event <br/>
5. GET {{base_url}}/Calendar/2,  to get event info by eventId <br/>
6. GET {{base_url}}/Calendar, to get all events <br/>
7. DELETE {{base_url}}/Calendar/1, to delete event by id <br/>

