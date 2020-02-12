using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static EventSourcing.Application.ScreeningCommands.V1;

namespace EventSourcing.Application.Screenings
{
    [ApiController]
    [Route("/screening")]
    public class ScreeningApi : ControllerBase
    {
        readonly ScreeningAppService _appService;

        public ScreeningApi(ScreeningAppService appService) => _appService = appService;

        [HttpGet]
        public async Task<string> Get(string id)
        {
            var command = new ScheduleScreening
            {
                MovieId     = "thor",
                ScreeningId = id,
                StartsAt    = DateTimeOffset.Now,
                TheaterId   = "vue1"
            };

            await _appService.Handle(command);

            return "done!";
        }

        [HttpGet]
        [Route("reserve")]
        public async Task<string> Get(string id, int row, int seat)
        {
            var command = new ReserveSeat
            {
                ScreeningId = id,
                Row         = row,
                Seat        = seat
            };

            await _appService.Handle(
                command,
                this.User.Identity.Name
            );

            return "done";
        }
    }
}