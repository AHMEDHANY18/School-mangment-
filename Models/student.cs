using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace shoolnew.Models
{
    public class student
    {
       
        public int StudentId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [ValidateNever]

        public List<AssignmentSubmission> AssignmentSubmissions { get; set; }
        [ValidateNever]
        public List<classStudent> ClassStudents { get; set; }
    }
}
