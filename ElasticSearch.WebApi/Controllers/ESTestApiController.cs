using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ElasticSearchDemo.CommonLib;
using ElasticSearchDemo.Entity;

namespace ElasticSearch.WebApi.Controllers
{
    [RoutePrefix("api")]
    public class ESTestApiController : BaseApiController
    {
        [Route("estest")]
        [HttpGet]
        public object Search()
        {
            //1 搜索数据
            string key = GetStringRequest("Key");
            int? from = GetIntRequest("from");
            int? size = GetIntRequest("size");

            return ElasticSearchHelper.Instance.Search("db_test", "person", key ?? "方鸿渐", from == null ? 0 : from.Value, size == null ? 20 : size.Value);



        }
        [Route("estest/SearchFullFileds")]
        [HttpGet]
        public object SearchFullFileds()
        {
            //1 搜索数据
            string key = GetStringRequest("Key");
            int? from = GetIntRequest("from");
            int? size = GetIntRequest("size");
            return ElasticSearchHelper.Instance.SearchFullFileds("db_test", "person", key ?? "方鸿渐", from == null ? 0 : from.Value, size == null ? 20 : size.Value);



        }
        [Route("estest/SearchFullFiledss")]
        [HttpGet]
        public object SearchFullFiledss()
        {
            //1 搜索数据
            string key = GetStringRequest("Key");
            int? from = GetIntRequest("from");
            int? size = GetIntRequest("size");
            return ElasticSearchHelper.Instance.SearchFullFiledss("db_test", "person", string.IsNullOrWhiteSpace(key) ? "方鸿渐":key, from == null ? 0 : from.Value, size == null ? 20 : size.Value);
        }

        /// <summary>
        /// 索引数据
        /// </summary>
        /// <returns></returns>
        [Route("estest/index")]
        [HttpGet]
        public object index()
        {
            int length = S.test.Length;
            Random rd = new Random();
            Random rdName = new Random();
            ParallelOptions _po = new ParallelOptions();
            _po.MaxDegreeOfParallelism = 4;
            Parallel.For(0, 10000000, _po, c =>
            {

                var start = rd.Next(0, S.test.Length - 700);
                var startName = rd.Next(0, S.test.Length - 30);
                person p = new person() { age = DateTime.Now.Millisecond, birthday = DateTime.Now, id = Guid.NewGuid().ToString(), intro = S.test.Substring(start, 629) + c, name = S.test.Substring(startName, 29) + c, sex = true };
                ElasticSearchHelper.Instance.Index("db_test", "person", Guid.NewGuid().ToString(), p);
            });
            return 1;
        }
    }
}
