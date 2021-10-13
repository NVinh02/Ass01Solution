using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace DataAccess.Repository
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetMembers();
        IEnumerable<Member> SearchMember(string Name, int ID);
        IEnumerable<Member> SortMember();
        IEnumerable<Member> FilterMember(string City, string Country);
        IEnumerable<string> GetCountryList();
        IEnumerable<string> GetCityByCountry(string Country);
        Member CheckLogin(string email, string password);
        Member GetMemberByID(int MemberID);
        void CreateNewMember(Member member);
        void UpdateMember(Member member);
        void DeleteMember(int MemberID);
    }
}
