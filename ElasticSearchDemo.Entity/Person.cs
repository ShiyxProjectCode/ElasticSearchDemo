using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchDemo.Entity
{
    /// <summary>
    /// ik分词结果对象
    /// </summary>
    public class IKAnalyerEntity
    {
        public string took { get; set; }

        public bool timed_out { get; set; }

        public _shards _shards { get; set; }
        
        public hitsEntity hits { get; set; }
    }

    public class _shards
    {
        public int total { get; set; }
        public int successful { get; set; }
        public int failed { get; set; }
    }

    public class hitsEntity
    {
        public string total { get; set; }
        public string max_score { get; set; }
        public List<recoard> hits { get; set; }
    }

    public class recoard
    {
        public string _index { get; set; }
        public string _type { get; set; }
        public string _id { get; set; }

        public string _score { get; set; }

        public person _source { get; set; }
    }

    /// <summary>
    /// 测试数据对象
    /// </summary>
    public class personList
    {
        public personList()
        {
            this.list = new List<person>();
        }
        public int hits { get; set; }
        public int took { get; set; }
        public List<person> list { get; set; }
    }
    public class person
    {
        public string id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public bool sex { get; set; }
        public DateTime birthday { get; set; }
        public string intro { get; set; }
    }
}
