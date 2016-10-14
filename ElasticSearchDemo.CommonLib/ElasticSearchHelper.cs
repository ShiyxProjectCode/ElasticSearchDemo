using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElasticSearchDemo.Console;
using PlainElastic.Net;

namespace ElasticSearchDemo.CommonLib
{
    /// <summary>
    /// es 操作类
    /// </summary>
   public class ElasticSearchHelper
    {
        public static readonly ElasticSearchHelper Intance = new ElasticSearchHelper();

        private ElasticConnection Client;
        private ElasticSearchHelper()
        {

            var node = new Uri(ElasticSearchConfig.EsUrl);

            Client = new ElasticConnection("localhost", 9200);



        }
    }
}
