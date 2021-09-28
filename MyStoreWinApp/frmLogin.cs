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
    public partial class frmLogin : Form
    {
        public IMemberRepository memberRepository = new MemberRepository();

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e) => Close();

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtPassword.Text = "";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            Member member = memberRepository.CheckLogin(email, password);
            if (member != null)
            {
                if (member.Email.Equals("admin@fstore.com"))
                {
                    frmMemberManagement manage = new frmMemberManagement
                    {
                        memberRepository = memberRepository
                    };
                    this.Hide();
                    if (manage.ShowDialog() == DialogResult.Cancel)
                    {
                        this.Show();
                    }
                }
                else
                {
                    frmMemberDetails details = new frmMemberDetails
                    {
                        Text = "Profile",
                        CreateOrUpdate = true,
                        isMember = true,
                        btnText = "Save change",
                        memberInfo = member,
                        MemberRepository = memberRepository
                    };
                    this.Hide();
                    if (details.ShowDialog() == DialogResult.OK || details.ShowDialog() == DialogResult.Cancel)
                    {
                        this.Show();
                    }
                }
            }
            else
            {
                lbError.Text = "Wrong email or password";
                txtPassword.Text = "";
            }
            
        }
    }
}
