namespace ElasticSearchDemo.Entity
{
    public class PartialDoctorEntity
    {
        /// <summary>
        /// 医生ID
        /// </summary>
        public string DoctorId { get; set; }

        /// <summary>
        /// 供应号编号
        /// </summary>
        public string SupplierNumber { get; set; }

        /// <summary>
        /// 医院ID
        /// </summary>
        public string HospitalId { get; set; }

        /// <summary>
        /// 医院号码
        /// </summary>
        public string HospitalNumber { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// 医院部门
        /// </summary>
        public string HospitalDepartmentId { get; set; }

        /// <summary>
        /// 医院部门名称
        /// </summary>
        public string HospitalDepartmentName { get; set; }

        /// <summary>
        /// 部门号
        /// </summary>
        public string HepartmentNumber { get; set; }

        /// <summary>
        /// 专业Id
        /// </summary>
        public string ProfessionalDepartmentId { get; set; }

        /// <summary>
        /// 专业名称
        /// </summary>
        public string ProfessionalDepartmentName { get; set; }

        /// <summary>
        /// 医生号码
        /// </summary>
        public string DoctorNumber { get; set; }

        /// <summary>
        /// 医生名称
        /// </summary>
        public string DoctorName { get; set; }

        public static PartialDoctorEntity Generate(DoctorEntity doctorEntity)
        {
            return new PartialDoctorEntity()
            {
                SupplierNumber = doctorEntity.SupplierNumber,
                HospitalId = doctorEntity.HospitalId,
                HospitalNumber = doctorEntity.HospitalNumber,
                HospitalName = doctorEntity.HospitalName,
                HospitalDepartmentId = doctorEntity.HospitalDepartmentId,
                HospitalDepartmentName = doctorEntity.HospitalDepartmentName,
                HepartmentNumber = doctorEntity.DepartmentNumber,
                ProfessionalDepartmentId = doctorEntity.ProfessionalDepartmentId,
                ProfessionalDepartmentName = doctorEntity.ProfessionalDepartmentName,
                DoctorNumber = doctorEntity.DoctorNumber,
                DoctorName = doctorEntity.DoctorName,
            };
        }
    }
}