using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace shoolnew.Models
{
    public class teacher
    {
        public int TeacherID { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public int Password { get; set; }
        public string Status { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        [ValidateNever]
        public List<classes> Classes { get; set; }


    }
}
