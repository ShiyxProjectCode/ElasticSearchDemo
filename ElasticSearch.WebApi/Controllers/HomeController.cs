using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElasticSearchDemo.CommonLib;
using PlainElastic.Net;
using PlainElastic.Net.Serialization;

namespace ElasticSearch.WebApi.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        
    }
}
