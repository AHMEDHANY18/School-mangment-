using MessagePack;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace shoolnew.Models
{
    public class classe_recource
    {
        
        public int classe_recourceID { get; set; }
        [ValidateNever]
        public int ClassID { get; set; }
        [ValidateNever]
        public classes Class { get; set; }

        public string Name { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
       
    }
}
