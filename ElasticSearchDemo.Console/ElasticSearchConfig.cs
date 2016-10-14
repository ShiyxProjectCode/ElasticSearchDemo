using Nest;
using System;

namespace ElasticSearchDemo.Console
{
    public class ElasticSearchConfig
    {
        private static string esUrl = "http://192.168.0.180:9200";

        /// <summary>
        /// 索引别名
        /// </summary>
        public static string IndexName = "esDemo-index";

        private static IElasticClient _esClient;

        private static readonly object syncRoot = new object();

        public static IElasticClient EsClient
        {
            get
            {
                if (_esClient == null)
                {
                    lock (syncRoot)
                    {
                        if (_esClient == null)
                            _esClient = new ElasticClient(new ConnectionSettings(new Uri(esUrl)));
                    }
                }
                return _esClient;
            }
        }

        private ElasticSearchConfig()
        {
        }
    }
}