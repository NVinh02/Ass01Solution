using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessObject;
using DataAccess.Repository;

namespace MyStoreWinApp
{
    public partial class frmMemberDetails : Form
    {
        public IMemberRepository MemberRepository { get; set; }
        public bool CreateOrUpdate { get; set; }
        public bool isMember { get; set; }
        public string btnText { get; set; }
        public Member memberInfo { get; set; }

        public frmMemberDetails()
        {
            InitializeComponent();
        }

        private void frmMemberDetails_Load(object sender, EventArgs e)
        {
            txtMemberID.Enabled = !CreateOrUpdate;
            txtPassword.Enabled = isMember;
            btnConfirm.Text = btnText;
            if (CreateOrUpdate == true)
            {
                txtMemberID.Text = memberInfo.MemberID.ToString();
                txtMemberName.Text = memberInfo.MemberName;
                txtEmail.Text = memberInfo.Email;
                if (isMember)
                    txtPassword.Text = memberInfo.Password;
                else
                    txtPassword.Text = "********";
                txtCountry.Text = memberInfo.Country;
                txtCity.Text = memberInfo.City;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string tmp;
                if (isMember)
                    tmp = txtPassword.Text;
                else
                    tmp = memberInfo.Password;

                var member = new Member
                {
                    MemberID = int.Parse(txtMemberID.Text),
                    MemberName = txtMemberName.Text,
                    Email = txtEmail.Text,
                    Password = tmp,
                    City = txtCity.Text,
                    Country = txtCountry.Text
                };

                if (CreateOrUpdate == true)
                {
                    MemberRepository.UpdateMember(member);
                } else
                {
                    MemberRepository.CreateNewMember(member);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CreateOrUpdate == true ? "Update member" : "Create new member");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();
    }
}
