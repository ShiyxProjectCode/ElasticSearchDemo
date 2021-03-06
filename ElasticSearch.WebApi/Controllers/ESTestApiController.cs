﻿using ElasticSearchDemo.CommonLib;
using ElasticSearchDemo.Entity;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace ElasticSearch.WebApi.Controllers
{
    [RoutePrefix("api")]
    public class EsTestApiController : BaseApiController
    {
        [Route("search")]
        [HttpGet]
        public object Search()
        {
            string key = GetStringRequest("Key");
            int? from = GetIntRequest("from");
            int? size = GetIntRequest("size");

            return ElasticSearchHelper.Instance.Search("test_index", "person", key ?? "方鸿渐个人", from == null ? 0 : from.Value, size == null ? 20 : size.Value);
        }

        [Route("esapi/SearchFullFileds")]
        [HttpGet]
        public object SearchFullFileds()
        {
            string key = GetStringRequest("Key");
            int? from = GetIntRequest("from");
            int? size = GetIntRequest("size");
            return ElasticSearchHelper.Instance.SearchFullFileds("test_index", "person", key ?? "方鸿渐个人", @from ?? 0, size ?? 20);
        }

        [Route("esapi/SearchFullFileds1")]
        [HttpGet]
        public object SearchFullFileds1()
        {
            string key = GetStringRequest("Key");
            int? from = GetIntRequest("from");
            int? size = GetIntRequest("size");
            return ElasticSearchHelper.Instance.SearchFullFiledss("test_index", "person", string.IsNullOrWhiteSpace(key) ? "方鸿渐个人" : key, @from ?? 0, size ?? 20);
        }

        /// <summary>
        /// 索引数据
        /// </summary>
        /// <returns></returns>
        [Route("esapi/CreateData")]
        [HttpGet]
        public object CreateData()
        {
            string index = "test_index";
            string type = "person";

            //生成索引
            ElasticSearchHelper.Instance.CreateIndexSetting(index);

            //生成map
            ElasticSearchHelper.Instance.CreateMap(index, type);

            int length = S.test.Length;
            Random rd = new Random();
            Random rdName = new Random();
            ParallelOptions _po = new ParallelOptions();
            _po.MaxDegreeOfParallelism = 4;
            Parallel.For(0, 1000000, _po, c =>
            {
                var start = rd.Next(0, S.test.Length - 700);
                var startName = rd.Next(0, S.test.Length - 30);
                person p = new person() { age = DateTime.Now.Millisecond, birthday = DateTime.Now, id = Guid.NewGuid().ToString(), intro = S.test.Substring(start, 629) + c, name = S.test.Substring(startName, 29) + c, sex = true };
                ElasticSearchHelper.Instance.Index(index, type, Guid.NewGuid().ToString(), p);
            });
            return 1;
        }
    }
}