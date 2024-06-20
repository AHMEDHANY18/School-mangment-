using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace shoolnew.Models
{
    public class classStudent
    {
        public int ClassStudentId { get; set; }
       
       

        public int VisitCount { get; set; }
        public DateTime ClassTime { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public int ClassID { get; set; }
        [ValidateNever]
        public classes Class { get; set; }

        public int StudentID { get; set; }
        [ValidateNever]
        public student Student { get; set; }
    }
}
