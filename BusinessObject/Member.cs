﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Member : IComparable<Member>
    {
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public int CompareTo(Member other) => String.Compare(this.MemberName, other.MemberName);
    }
}
