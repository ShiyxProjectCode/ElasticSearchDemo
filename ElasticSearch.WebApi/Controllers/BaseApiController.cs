using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ElasticSearch.WebApi.Controllers
{
    public class BaseApiController : ApiController
    {  
        public BaseApiController() { }

        public string GetStringRequest(string paramter)
        {
            return HttpContext.Current.Request.QueryString[paramter] ?? "";
        }
        public int? GetIntRequest(string paramter)
        {
            string tmp = HttpContext.Current.Request.QueryString[paramter] ?? "";
            int tag = 0;
            if (!int.TryParse(tmp, out tag))
            {
                return null;
            }
            return tag;
        }
    }
}
