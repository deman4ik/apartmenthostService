using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class SeedDbController : ApiController
    {
        public ApiServices Services { get; set; }
        private readonly apartmenthostContext _context = new apartmenthostContext();
        // GET api/SeedDb
        [HttpPost]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        public string Post()
        {
            DBPopulator.Populate(_context);
            return "ok";
        }

    }
}
