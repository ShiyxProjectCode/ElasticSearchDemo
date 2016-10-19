using ElasticSearchDemo.Console;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using ElasticSearchDemo.Entity;

namespace ElasticSearchDemo.Console
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

        /// <summary>
        /// 移除别名与索引
        /// </summary>
        /// <returns></returns>
        public string DeleteIndex()
        {
            //获取别名下的索引
            var alias = _esClient.CatAliases(a => a.Name(ElasticSearchConfig.IndexName));
            BulkAliasDescriptor aliasBulk = new BulkAliasDescriptor();
            var indexNames = new List<string>();
            foreach (var record in alias.Records)
            {
                //移除别名下的索引 
                aliasBulk.Add(new AliasRemoveDescriptor().Index(record.Index).Alias(record.Alias));
                indexNames.Add(record.Index);
            }
            var response = _esClient.Alias(aliasBulk);

            //删除旧索引
            indexNames.ForEach(index => _esClient.DeleteIndex(new DeleteIndexRequest(index)));

            if (!response.Acknowledged && alias.Records.Any())
            {
                return "索引与别名关系移除失败！" + response.ServerError.Error.Reason;
            }

            return "索引与别名关系移除成功！";
        }

        /// <summary>
        /// 切换索引别名
        /// </summary>
        public string ChangeIndexAliase(string newIndexName)
        {
            //1 创建新的索引后，将别名指向的索引改为新索引名
            var alias = _esClient.CatAliases(new CatAliasesRequest(ElasticSearchConfig.IndexName));

            BulkAliasDescriptor aliasDescriptor = new BulkAliasDescriptor();

            foreach (var record in alias.Records)
            {
                aliasDescriptor.Add(new AliasRemoveDescriptor().Index(record.Index).Alias(record.Alias));
            }

            aliasDescriptor.Add(new AliasAddDescriptor().Alias(ElasticSearchConfig.IndexName).Index(newIndexName));
            var response = _esClient.Alias(aliasDescriptor);
            if (!response.Acknowledged)
            {
                return "切换索引别名失败" + response.ServerError.Error.Reason;
            }

            return string.Empty;
        }

        /// <summary>
        /// 向索引中添加数据
        /// </summary>
        /// <param name="doctorEntities"></param>
        /// <returns></returns>
        public string AddDoctorInfoToIndex(List<DoctorEntity> doctorEntities)
        {

            BulkRequest bulk = new BulkRequest(ElasticSearchConfig.IndexName)
            {
                Operations = new List<IBulkOperation>()
            };
            foreach (var doctorEntity in doctorEntities)
            {
                bulk.Operations.Add(new BulkIndexOperation<DoctorEntity>(doctorEntity));
            }

            var response = _esClient.Bulk(bulk);
            if (response.Errors)
            {
                return "添加索引数据失败" + response.Items.First().Error;
            }

            return "添加索引数据成功！";
        }

        /// <summary>
        /// 更新索引数据
        /// </summary>
        public string UpdateDoctorInfoToIndex(List<DoctorEntity> doctorEntities)
        {
            var bulk = new BulkRequest(ElasticSearchConfig.IndexName)
            {
                Operations = new List<IBulkOperation>()
            };
            BulkUpdateDescriptor<DoctorEntity, PartialDoctorEntity> updateDescriptor = new BulkUpdateDescriptor<DoctorEntity, PartialDoctorEntity>();

            foreach (var doctorEntity in doctorEntities)
            {
                var updatedescript = updateDescriptor.IdFrom(doctorEntity)//会自动推断出document的id 
                     .Doc(PartialDoctorEntity.Generate(doctorEntity))
                     .Upsert(doctorEntity)
                     .RetriesOnConflict(3);

                bulk.Operations.Add(updatedescript);
            }

            var response = _esClient.Bulk(bulk);

            if (response.Errors)
            {
                return "更新索引数据失败" + response.ItemsWithErrors;
            }

            return "更新索引数据成功！";
        }

        /// <summary>
        /// 删除索引中数据
        /// </summary>
        /// <param name="doctors"></param>
        /// <returns></returns>
        public string DeleteDoctorInfoToIndex(List<DoctorEntity> doctors)
        {

            BulkRequest bulk = new BulkRequest(ElasticSearchConfig.IndexName)
            {
                Operations = new List<IBulkOperation>()
            };

            foreach (var doctor in doctors)
            {
                var deleteDescript = new BulkDeleteDescriptor<DoctorEntity>().Document(doctor);
                bulk.Operations.Add(deleteDescript);
            }

            var response = _esClient.Bulk(bulk);
            if (response.Errors)
            {
                return "删除索引数据失败" + response.Items.First().Error;
            }

            return "删除索引数据成功";
        }

        /// <summary>
        /// 单个属性搜索
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        public List<DoctorEntity> QueryDoctors(string doctorId)
        {
            SearchDescriptor<DoctorEntity> searchDescriptor = new SearchDescriptor<DoctorEntity>();

            searchDescriptor.Query(q => q.Term(t => t.Field("doctorId").Value(doctorId)));

            var result = _esClient.Search<DoctorEntity>(searchDescriptor);

            //第二种
            QueryContainer termQuery = new TermQuery { Field = "lastname", Value = "keyword" };
            var searchResults = _esClient.Search<DoctorEntity>(s => s
                .Index(ElasticSearchConfig.IndexName)
                .Query(o => termQuery)
            );

            return result.Documents.ToList();

        }

        /// <summary>
        /// 全文搜索
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns></returns>
        public IEnumerable<DoctorEntity> Query(string keyword)
        {
            var wholeKeyword = keyword;
            keyword = $"*{keyword}*";//模糊查询需要在关键词外加“**”；

            QueryContainer query = new QueryStringQuery() { Query = keyword, DefaultOperator = Operator.And, };
            if (!String.IsNullOrEmpty(wholeKeyword))
            {
                QueryContainer wholeWordQuery = new QueryStringQuery() { Query = wholeKeyword };
                query = query || wholeWordQuery;
            }

            var searchResults = _esClient.Search<DoctorEntity>(s => s
                .Index(ElasticSearchConfig.IndexName)
                .Query(o => query).Sort(c => c.Ascending(p => p.HospitalNumber)));//针对中文排序有问题

            return searchResults.Documents;
        }

        public List<DoctorEntity> BuiDoctorEntities()
        {
            var doctorList = new List<DoctorEntity>();
            string[] doctorNames = new string[] { "石霖", "陆桂香", "蔡江云", "刘玉凤", "谭志团", "贾雁平", "周琼华", "张平", "周华", "赵子龙" };
            for (int i = 0; i < 10; i++)
            {
                doctorList.Add(new DoctorEntity()
                {
                    DoctorId = "5588235" + i,
                    DoctorName = doctorNames[i],
                    DoctorNumber = "134" + i,
                    DepartmentNumber = "654",
                    HospitalDepartmentId = Guid.NewGuid().ToString(),
                    HospitalDepartmentName = "内科",
                    HospitalId = Guid.NewGuid().ToString(),
                    HospitalName = "北京大学深圳医院",
                    HospitalNumber = "bjdxszyy"+i,
                    ProfessionalDepartmentId = "225" + i,
                    ProfessionalDepartmentName = "心胸内科",
                    SupplierNumber = "Thirdpart"
                });
            }

            return doctorList;
        }
    }
}