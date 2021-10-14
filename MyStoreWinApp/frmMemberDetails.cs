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
        public string btnText { get; set; }
        public Member memberInfo { get; set; }

        public frmMemberDetails()
        {
            InitializeComponent();
        }

        private void frmMemberDetails_Load(object sender, EventArgs e)
        {
            txtMemberID.Enabled = !CreateOrUpdate;
            btnConfirm.Text = btnText;
            cboCountry.Items.Clear();
            cboCity.Items.Clear();
            if (CreateOrUpdate == true)
            {
                txtMemberID.Text = memberInfo.MemberID.ToString();
                txtMemberName.Text = memberInfo.MemberName;
                txtEmail.Text = memberInfo.Email;
                txtPassword.Text = memberInfo.Password;

                foreach (var country in MemberRepository.GetCountryList())
                    cboCountry.Items.Add(country);

                foreach (var city in MemberRepository.GetCityByCountry(memberInfo.Country.Trim()))
                    cboCity.Items.Add(city);

                cboCountry.SelectedIndex = 0;
                cboCity.SelectedIndex = 0;
                cboCountry.Text = memberInfo.Country.Trim();
                cboCity.Text = memberInfo.City.Trim();
            } else
            {
                cboCountry.Items.Add("Select your country");
                foreach (var country in MemberRepository.GetCountryList())
                    cboCountry.Items.Add(country);
                cboCity.Enabled = false;
                cboCountry.SelectedIndex = 0;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string memberID = txtMemberID.Text.Trim();
                string memberName = txtMemberName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text.Trim();
                string city = cboCity.Text;
                string country = cboCountry.Text;

                if (city.Equals("Select your city") || country.Equals("Select your country") || 
                    (memberName.Equals("") || memberName.Equals(" ")) || 
                    (memberID.Equals("") || memberID.Equals(" ")) ||
                    (email.Equals("") || email.Equals(" ")) ||
                    (password.Equals("") || password.Equals(" ")) )
                {
                    throw new Exception("Please fill in all the information to finish");
                }

                var member = new Member
                {
                    MemberID = int.Parse(memberID),
                    MemberName = memberName,
                    Email = email,
                    Password = password,
                    City = city,
                    Country = country
                };

                if (CreateOrUpdate == true)
                {
                    MemberRepository.UpdateMember(member);
                }
                else
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

        private void cboCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            string country = cboCountry.Text;
            if (!country.Equals("Select your country"))
            {
                if (cboCity.Enabled == false)
                {
                    cboCity.Enabled = true;
                }

                cboCity.Items.Clear();
                cboCity.Items.Add("Select your city");
                foreach (string city in MemberRepository.GetCityByCountry(country))
                {
                    cboCity.Items.Add(city);
                }
                cboCity.SelectedIndex = 0;
            }
            else
            {
                cboCity.Enabled = false;
            }
        }
    }
}
