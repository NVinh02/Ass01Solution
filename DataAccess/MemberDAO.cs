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
                MemberID=1, MemberName="Hoa Yena", Email="HoangVT@gmail.com", Password="0123456", City="George Town", Country="Malaysia"
            },
            new Member
            {
                MemberID=2, MemberName="Nguyen Tat To", Email="ToNT@gmail.com", Password="0123456", City="Ho Chi Minh", Country="Vietnam"
            },
            new Member
            {
                MemberID=3, MemberName="Mathew Aven", Email="MathewA@gmail.com", Password="0123456", City="Singapore", Country="Singapore"
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

        public List<Member> GetAllMemberList() => MemberList;

        public Member GetMemberByID (int MemberID) => MemberList.SingleOrDefault(mem => mem.MemberID == MemberID);

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

        public List<Member> Search(string Name, int ID)
        {
            List<Member> tmp = new List<Member>();
            foreach (Member mem in MemberList)
            {
                if (mem.MemberName.Contains(Name) && mem.MemberID == ID)
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

        public List<Member> filter(string city, string country) => MemberList.FindAll
            (mem => mem.City.Equals(city) && mem.Country.Equals(country));

        private static Dictionary<string, List<string>> Map = new Dictionary<string, List<string>>()
        {
            {"Vietnam", new List<string>() {"Ho Chi Minh", "Hanoi" } },
            {"Brunei", new List<string>() {"Bandar Seri Begawan", "Kampong Ayer" } },
            {"Cambodia", new List<string>() {"Phnom Penh", "Preah Sihanouk" } },
            {"Indonesia", new List<string>() {"Jakarta", "Semarang" } },
            {"Laos", new List<string>() { "Vientiane", "Pakse" } },
            {"Malaysia", new List<string>() { "Kuala Lumpur", "George Town" } },
            {"Myanmar", new List<string>() { "Yangon", "Mandalay" } },
            {"Philippines", new List<string>() { "Manila", "Quezon" } },
            {"Singapore", new List<string>() { "Singapore", "Queen Town" } },
            {"Thailand", new List<string>() { "Bangkok", "Chiang Mai" } }
        };

        public List<string> GetAllCountry()
        {
            List<string> list = new List<string>();
            foreach (var country in Map)
            {
                list.Add(country.Key);
            }
            return list;
        }

        public List<string> GetCityByCountry(string countryName)
        {
            List<string> list = new List<string>();
            foreach (var country in Map)
            {
                if (country.Key.Equals(countryName))
                {
                    foreach (var city in country.Value)
                    {
                        list.Add(city);
                    }
                }
            }
            return list;
        }
    }
}
