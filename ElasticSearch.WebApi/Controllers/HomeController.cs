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
            string index = "test_index";
            string type = "person";
            string key = "追求自己";
            int from = 0;
            int size = 50;
            //var result = ElasticSearchHelper.Instance.Search(index, type, key, from, size);

            var result1 = ElasticSearchHelper.Instance.SearchFullFileds(index, type, key, from, size);
            var result2 = ElasticSearchHelper.Instance.SearchFullFiledss(index, type, key, from, size);

            return View();
        }


    }
}
