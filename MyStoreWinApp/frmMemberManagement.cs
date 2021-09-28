using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccess.Repository;
using BusinessObject;

namespace MyStoreWinApp
{
    public partial class frmMemberManagement : Form
    {
        public IMemberRepository memberRepository { get; set; }
        private BindingSource source;
        private IEnumerable<Member> list;

        public frmMemberManagement()
        {
            InitializeComponent();
        }

        private void frmMemberManagement_Load(object sender, EventArgs e)
        {
            btnDelete.Enabled = false;
            dgvMemberList.CellDoubleClick += dgv_CellDoubleClick;
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmMemberDetails details = new frmMemberDetails
            {
                Text = "Update member",
                CreateOrUpdate = true,
                btnText = "Save",
                isMember = false,
                memberInfo = GetMemberInfo(),
                MemberRepository = memberRepository
            };
            if (details.ShowDialog() == DialogResult.OK)
            {
                LoadMemberList();
                source.Position = source.Count - 1;
            }
        }

        private Member GetMemberInfo()
        {
            Member member = null;
            try
            {
                member = memberRepository.GetMemberByID(int.Parse(txtMemberID.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get member");
            }
            return member;
        }

        private void ClearText()
        {
            txtMemberID.Text = string.Empty;
            txtMemberName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtCountry.Text = string.Empty;
            txtCity.Text = string.Empty;
        }

        public void LoadMemberList()
        {
            var members = list;

            try
            {
                source = new BindingSource();
                source.DataSource = members;

                txtMemberID.DataBindings.Clear();
                txtMemberName.DataBindings.Clear();
                txtEmail.DataBindings.Clear();
                txtPassword.DataBindings.Clear();
                txtCountry.DataBindings.Clear();
                txtCity.DataBindings.Clear();

                txtMemberID.DataBindings.Add("Text", source, "MemberID");
                txtMemberName.DataBindings.Add("Text", source, "MemberName");
                txtEmail.DataBindings.Add("Text", source, "Email");
                txtPassword.DataBindings.Add("Text", source, "Password");
                txtCountry.DataBindings.Add("Text", source, "Country");
                txtCity.DataBindings.Add("Text", source, "City");

                dgvMemberList.DataSource = null;
                dgvMemberList.DataSource = source;

                if(members.Count() == 0)
                {
                    ClearText();
                    btnDelete.Enabled = false;
                } else
                {
                    btnDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load member list");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearchValue.Text = "";
                var members = memberRepository.GetMembers();
                list = members;
                LoadMemberList();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load member list");
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            frmMemberDetails details = new frmMemberDetails
            {
                Text = "Create new member",
                CreateOrUpdate = false,
                MemberRepository = this.memberRepository,
                isMember = true,
                btnText = "Create"
            };
            if (details.ShowDialog() == DialogResult.OK)
            {
                LoadMemberList();
                source.Position = source.Count - 1;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var member = GetMemberInfo();
                memberRepository.DeleteMember(member.MemberID);
                LoadMemberList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete member");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string SearchValue = txtSearchValue.Text;
            list = memberRepository.SearchMember(SearchValue);
            LoadMemberList();
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            list = memberRepository.SortMember();
            LoadMemberList();
        }
    }
}
