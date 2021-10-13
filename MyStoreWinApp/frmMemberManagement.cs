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
            ReloadPage();
            list = memberRepository.GetMembers();
            dgvMemberList.CellDoubleClick += dgv_CellDoubleClick;
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmMemberDetails details = new frmMemberDetails
            {
                Text = "Update member",
                CreateOrUpdate = true,
                btnText = "Save",
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

        private void ReloadPage()
        {
            btnDelete.Enabled = false;
            btnSort.Enabled = false;
            txtSearchID.Text = "";
            txtSearchName.Text = "";
            cboCountryFilter.Items.Clear();
            cboCountryFilter.Items.Add("Choose a country");
            cboCountryFilter.SelectedIndex = 0;
            foreach(string country in memberRepository.GetCountryList())
            {
                cboCountryFilter.Items.Add(country);
            }
            cboCityFilter.Enabled = false;
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
                    btnSort.Enabled = false;
                } else
                {
                    btnDelete.Enabled = true;
                    btnSort.Enabled = true;
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
                ReloadPage();
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
            try
            {
                string IDValue = txtSearchID.Text;
                string NameValue = txtSearchName.Text;
                if (!NameValue.Equals("") && !IDValue.Equals(""))
                {
                    int ID = int.Parse(IDValue);
                    list = memberRepository.SearchMember(NameValue, ID);
                    if (list.Count() == 0)
                        throw new Exception("No match member found");
                    else
                        LoadMemberList();
                } else
                {
                    throw new Exception("Missing information to search! Please, fill in both ID and Name");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Search member");
            }
            
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            list = memberRepository.SortMember();
            LoadMemberList();
        }

        private void btnLogout_Click(object sender, EventArgs e) => Close();

        private void cboCountryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string country = cboCountryFilter.Text;
            if (!country.Equals("Choose a country"))
            {
                if (cboCityFilter.Enabled == false)
                {
                    cboCityFilter.Enabled = true;
                }

                cboCityFilter.Items.Clear();
                cboCityFilter.Items.Add("Choose a city");
                foreach (string city in memberRepository.GetCityByCountry(country))
                {
                    cboCityFilter.Items.Add(city);
                }
                cboCityFilter.SelectedIndex = 0;
            } else
            {
                cboCityFilter.Enabled = false;
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                string country = cboCountryFilter.Text;
                string city = cboCityFilter.Text;
                if (!country.Equals("Choose a country") && !city.Equals("Choose a city"))
                {
                    list = memberRepository.FilterMember(city, country);
                    if (list.Count() == 0)
                        throw new Exception("No match member found");
                    else
                        LoadMemberList();
                } else
                {
                    throw new Exception("Missing information to filter! Please, fill in country and city");
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Filter member");
            }
        }
    }
}
