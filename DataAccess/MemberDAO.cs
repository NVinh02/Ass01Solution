using System;
using System.Collections.Generic;
using System.Linq;
using BusinessObject;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;

namespace DataAccess
{
    public class MemberDAO
    {
        private static IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true).Build();

        private static Member Admin = new Member()
        {
            MemberID = int.Parse(config["Admin:MemberID"]),
            MemberName = config["Admin:MemberName"],
            Email = config["Admin:Email"],
            Password = config["Admin:Password"],
            Country = config["Admin:Country"],
            City = config["Admin:City"]
        };

        private static List<Member> MemberList = new List<Member>()
        {
            Admin,
            new Member
            {
                MemberID=1, MemberName="Hoang Van Thu", Email="HoangVT@gmail.com", Password="0123456", City="HCM", Country="Vietname"
            },
            new Member
            {
                MemberID=2, MemberName="Nguyen Tat To", Email="ToNT@gmail.com", Password="0123456", City="HCM", Country="Vietname"
            },
            new Member
            {
                MemberID=3, MemberName="Mathew Aven", Email="MathewA@gmail.com", Password="0123456", City="New York", Country="USA"
            }
        };

        private static MemberDAO instance = null;
        private static readonly object instanceLock = new object();
        private MemberDAO(){}
        public static MemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MemberDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Member> GetAllMemberList()
        {
            List<Member> list = MemberList;
            foreach(Member member in list) 
            {
                member.Password = "********";
            }
            return list;
        }

        public Member GetMemberByID (int MemberID)
        {
            Member member = MemberList.SingleOrDefault(mem => mem.MemberID == MemberID);
            return member;
        }

        public void AddNewMember (Member member)
        {
            Member tmp = GetMemberByID(member.MemberID);
            if(tmp == null)
            {
                MemberList.Add(member);
            } else
            {
                throw new Exception("Member is already Exist");
            }
        }

        public void Update (Member member)
        {
            Member tmp = GetMemberByID(member.MemberID);
            if (tmp != null)
            {
                var index = MemberList.IndexOf(tmp);
                MemberList[index] = member;
            }
            else
            {
                throw new Exception("Member does not already Exist");
            }
        }

        public void Remove (int MemberID)
        {
            Member tmp = GetMemberByID(MemberID);
            if (tmp != null)
            {
                MemberList.Remove(tmp);
            }
            else
            {
                throw new Exception("Member does not already Exist");
            }
        }

        public List<Member> Search(string SearchValue)
        {
            List<Member> tmp = new List<Member>();
            foreach (Member mem in MemberList)
            {
                if (mem.MemberName.Contains(SearchValue))
                {
                    tmp.Add(mem);
                }
                if (mem.MemberID.ToString().Contains(SearchValue))
                {
                    tmp.Add(mem);
                }
            }
            return tmp;
        }

        public List<Member> Sort()
        {
            List<Member> tmp = MemberList;
            tmp.Reverse();
            return tmp;
        }

        public Member CheckLogin(string email, string password) => MemberList.SingleOrDefault
                (mem => mem.Email == email && mem.Password == password);
    }
}
