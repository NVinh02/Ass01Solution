using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject
{
    public class Member : IComparable<Member>
    {
        [Required]
        [Range(1,100)]
        public int MemberID { get; set; }

        [Required]
        [StringLength(30)]
        public string MemberName { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){1,4})+)$")]
        public string Email { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 6)]
        [RegularExpression(@"^([\w\d]+)$")]
        public string Password { get; set; }

        [Required]
        public string City { get; set; }
        
        [Required]
        public string Country { get; set; }

        public int CompareTo(Member other) => String.Compare(this.MemberName, other.MemberName);
    }
}
