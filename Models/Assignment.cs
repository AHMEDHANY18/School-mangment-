using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace shoolnew.Models
{
    public class Assignment
    {
        public int AssignmentID { get; set; }
        [ValidateNever]
        public int ClassID { get; set; }
        [ValidateNever]
        public classes Class { get; set; }

        public string Question { get; set; }
        public bool Option { get; set; }
        public DateTime Deadline { get; set; }
        public string Attachment { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        [ValidateNever]

        public List<AssignmentSubmission> AssignmentSubmissions { get; set; }

    }
}
