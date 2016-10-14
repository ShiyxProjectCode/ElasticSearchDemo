using Nest;
using System;
using System.Configuration;

namespace ElasticSearchDemo.Console
{
    public class ElasticSearchConfig
    {
        private ElasticSearchConfig()
        {
        }

        private static IElasticClient _esClient;

        private static readonly object SyncRoot = new object();

        /// <summary>
        /// es 服务地址
        /// </summary>
        public static string EsUrl = ConfigurationManager.AppSettings["esHostUrl"];

        /// <summary>
        /// es 服务端口
        /// </summary>
        public static string EsUrlPort = ConfigurationManager.AppSettings["esHostUrlPort"];

        /// <summary>
        /// 索引别名
        /// </summary>
        public static string IndexName = ConfigurationManager.AppSettings["esIndexName"];

        /// <summary>
        /// es 服务实例
        /// </summary>
        public static IElasticClient EsClient
        {
            get
            {
                if (_esClient == null)
                {
                    lock (SyncRoot)
                    {
                        if (_esClient == null)
                            _esClient = new ElasticClient(new ConnectionSettings(new Uri(EsUrl)));
                    }
                }
                return _esClient;
            }
        }
    }
}