using ElasticSearchDemo.Console;
using Nest;
using System;
using ElasticSearchDemo.Entity;

namespace ElasticSearchDemo.CommonLib
{
    /// <summary>
    /// 提供对 Console项目的操作
    /// </summary>
    public class ElasticSearchService
    {
        private IElasticClient _esClient;

        public ElasticSearchService()
        {
            _esClient = ElasticSearchConfig.EsClient;
        }

        /// <summary>
        /// 创建索引，并设置别名
        /// </summary>
        /// <returns></returns>
        public string CreateIndex()
        {
            string newIndex = ElasticSearchConfig.IndexName + DateTime.Now.ToString("yyyyMMddHHmm");
            
            //创建索引，设置mapping
            var response = _esClient.CreateIndex(newIndex, c =>
                c.Mappings(m => m.Map<DoctorEntity>(map => map.AutoMap()))
                .Settings(setting => setting.NumberOfShards(3).NumberOfReplicas(1)));//3个分片，1个副本
            if (!response.Acknowledged)
            {
                return "创建索引失败" + response.ServerError.Error.Reason;
            }

            //设置别名
            var aliasResponse = _esClient.Alias(new BulkAliasDescriptor().Add(c => c.Index(newIndex).Alias(ElasticSearchConfig.IndexName)));
            if (!aliasResponse.Acknowledged)
            {
                return "设置索引别名失败" + aliasResponse.ServerError.Error.Reason;
            }

            return "创建索引并设置别名成功！";
        }
    }
}