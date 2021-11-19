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
    public class FavoriteController : ControllerBase
    {
        private FavoriteAccess favoriteAccess;

        public FavoriteController()
        {
            favoriteAccess = new FavoriteAccess();
        }
        [HttpPost]
        public ActionResult Create(Favorite favorite)
        {
            var newFavorite = favoriteAccess.Create(favorite);
            return newFavorite ? StatusCode(200, "Favorite added/deleted successfully") : StatusCode(400, "There was an error adding the data.");
        }
        [HttpPost("get")]
        public ActionResult Get(Favorite favorite)
        {
            var newFavorite = favoriteAccess.Get(favorite);
            return StatusCode(200, newFavorite ? "Favorite exists" : "No Record");
        }
    }
}