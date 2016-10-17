using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElasticSearchDemo.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string result=String.Empty;

            ElasticSearchService service=new ElasticSearchService();

            //清除已建的索引
            result = service.DeleteIndex();
            System.Console.WriteLine(result);

            //创建索引
            result = service.CreateIndex();
            System.Console.WriteLine(result);

            //添加数据
            var doctorList = service.BuiDoctorEntities();
            result = service.AddDoctorInfoToIndex(doctorList);
            System.Console.WriteLine(result);

            Thread.Sleep(2000);//等待索引数据生成

            //更新数据
            var doctors = service.QueryDoctors("55882350");
            doctors.ForEach(o=>o.DoctorName="Zery000");
            result = service.UpdateDoctorInfoToIndex(doctors);
            System.Console.WriteLine(result);

            //删除索引中数据
            result = service.DeleteDoctorInfoToIndex(doctors);
            System.Console.WriteLine(result);

            System.Console.ReadKey();

        }
    }
}
