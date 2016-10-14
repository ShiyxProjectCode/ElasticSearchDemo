using Nest;
using System;

namespace ElasticSearchDemo.Entity
{
    [ElasticsearchType(IdProperty = "", Name = "doctor")]
    public class DoctorEntity
    {
        [String(Index = FieldIndexOption.Analyzed)]
        public string DoctorId { get; set; }

        public string SupplierNumber { get; set; }

        public string HospitalId { get; set; }

        public string HospitalNumber { get; set; }

        [String(TermVector = TermVectorOption.WithPositionsOffsets, Index = FieldIndexOption.Analyzed)]
        public string HospitalName { get; set; }

        [String(Index = FieldIndexOption.NotAnalyzed)]
        public string HospitalDepartmentId { get; set; }

        [String(Index = FieldIndexOption.Analyzed)]
        public string HospitalDepartmentName { get; set; }

        [String(Index = FieldIndexOption.NotAnalyzed)]
        public string DepartmentNumber { get; set; }

        [String(Index = FieldIndexOption.NotAnalyzed)]
        public string ProfessionalDepartmentId { get; set; }

        [String(Index = FieldIndexOption.NotAnalyzed)]
        public string ProfessionalDepartmentName { get; set; }

        [String(Index = FieldIndexOption.NotAnalyzed)]
        public string DoctorNumber { get; set; }

        [String(TermVector = TermVectorOption.WithPositionsOffsets, Index = FieldIndexOption.Analyzed)]
        public string DoctorName { get; set; }
    }
}