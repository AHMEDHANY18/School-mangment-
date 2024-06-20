using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace shoolnew.Models
{
    public class AssignmentSubmission
    {
        public int AssignmentSubmissionID { get; set; }

        public int AssignmentID { get; set; }
        [ValidateNever]
        public Assignment Assignment { get; set; }

        public int StudentID { get; set; }
        [ValidateNever]
        public student Student { get; set; }

        public bool SelectedOption { get; set; }
        public string TextInput { get; set; }
        public string Attachment { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }
}