using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsJump.DataAccess;
using LetsJump.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LetsJump.Controllers.Admin
{
    [Route("[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private OrganizationAccess orgAccess;

        public OrganizationController()
        {
            orgAccess = new OrganizationAccess();
        }
        /// <summary>
        /// Gets the list(or individual) of organizations. Pass the admin token to retrieve the entire list.
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public ActionResult<List<Organization>> Get(Organization organization)
        {
            var _organization = orgAccess.Get(organization);
            return _organization.IsSuccess ? StatusCode(200, _organization.organization) : StatusCode(400, null);
        }
        [HttpPost("GetList/Requests")]
        public ActionResult<List<Organization>> GetRequests(Organization organization)
        {
            var _organization = orgAccess.GetRequests(organization);
            return _organization.IsSuccess ? StatusCode(200, _organization.organization) : StatusCode(400, null);
        }

        [HttpPost("Create")]
        public ActionResult<Organization> Create(Organization_Add organization)
        {
            var _organization = orgAccess.Create(organization);
            return _organization ? StatusCode(200, "Successfully created") : StatusCode(400, null);
        }
        [HttpPost("Remove")]
        public ActionResult<Organization> Remove(Organization organization)
        {
            var _organization = orgAccess.RemoveRestore(organization);
            return _organization ? StatusCode(200, "Successfully removed/restored") : StatusCode(400, null);
        }

        [HttpPost("Create/Request")]
        public ActionResult<Organization> RequestCreate(Organization_Add organization)
        {
            var _organization = orgAccess.RequestCreate(organization);
            return _organization ? StatusCode(200, "Successfully created") : StatusCode(400, null);
        }
    }
}