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
    [Route("[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private OfferAccess offerAccess;
        public OffersController()
        {
            offerAccess = new OfferAccess();
        }

        [HttpPost("Create")]
        public ActionResult<Offer> Create(Offer offer)
        {
            var _offer = offerAccess.Create(offer);
            return _offer ? StatusCode(200, "Successfully created offer") : StatusCode(400, null);
        }
        [HttpPost("Get")]
        public ActionResult<Offer> Get(Offer offer)
        {
            var _offer = offerAccess.Get(offer);
            return _offer.IsSuccess ? StatusCode(200, _offer.offers) : StatusCode(400, null);
        }
    }
}