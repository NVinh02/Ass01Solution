using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccess.Repository;
using DataAccess;

namespace DataAccess.Repository
{
    public class MemberRepository : IMemberRepository
    {
        public IEnumerable<Member> GetMembers() => MemberDAO.Instance.GetAllMemberList();
        public IEnumerable<Member> SearchMember(string SearchValue) => MemberDAO.Instance.Search(SearchValue);
        public IEnumerable<Member> SortMember() => MemberDAO.Instance.Sort();
        public Member CheckLogin(string email, string password) => MemberDAO.Instance.CheckLogin(email, password);
        public Member GetMemberByID(int MemberID) => MemberDAO.Instance.GetMemberByID(MemberID);
        public void CreateNewMember(Member member) => MemberDAO.Instance.AddNewMember(member);
        public void UpdateMember(Member member) => MemberDAO.Instance.Update(member);
        public void DeleteMember(int MemberID) => MemberDAO.Instance.Remove(MemberID);
    }
}
