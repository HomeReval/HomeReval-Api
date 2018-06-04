using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class StatusController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "online";
        }

    }
}
