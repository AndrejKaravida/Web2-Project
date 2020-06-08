using System.ComponentModel.DataAnnotations;

namespace WEB2Project.Dtos
{
    public class UserRole
    {
        [Key]
        public int MyId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
