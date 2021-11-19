using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsJump.DataAccess;
using LetsJump.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LetsJump.Controllers
{
    [Route("user/[controller]")]
    [ApiController]
    public class CheckInController : ControllerBase
    {
        private CheckinAccess checkinAccess;

        public CheckInController()
        {
            checkinAccess = new CheckinAccess();
        }
        [HttpPost]
        public ActionResult Create(Checkin checkin)
        {
            var newCheckin = checkinAccess.Create(checkin);
            return newCheckin.IsSuccess ? StatusCode(200, newCheckin.CheckInID) : StatusCode(400, "There was a problem adding the data");
        }
    }
}