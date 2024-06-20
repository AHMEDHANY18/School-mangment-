using System.ComponentModel.DataAnnotations;
using MessagePack;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace shoolnew.Models
{
    public class classes
    {
        
        public int classesID { get; set; }
        public int TeacherID { get; set; }
        [ValidateNever]
        public teacher Teacher { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Schedule { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        [ValidateNever]
        public List<classe_recource> ClasseResources { get; set; }
        [ValidateNever]
        public List<Assignment> Assignments { get; set; }
        [ValidateNever]
        public List<classStudent> ClassStudents { get; set; }
    }
}
