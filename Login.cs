using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Teams_Register
{
    public partial class Login : Form
    {

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();

        private void YourForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    
    public Login()
        {
            InitializeComponent();
            this.MouseDown += YourForm_MouseDown;
            username.Focus();
        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            panel2.Enabled = true;
            panel2.Visible = true;
            label1.Text = "SignIn";
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            rjTextBox2.PasswordChar = false;
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            rjTextBox2.PasswordChar = true;
        }

        private void rjTextBox1__TextChanged(object sender, EventArgs e)
        {

            if (rjTextBox1.Texts == "")
            {
                rjTextBox1.BorderFocusColor = System.Drawing.Color.Red;
            }
            else
            {
                rjTextBox1.BorderFocusColor = System.Drawing.Color.Indigo;

            }

            if (rjTextBox2.Texts == "") 
            {
                rjTextBox2.BorderFocusColor = System.Drawing.Color.Red;
            }
            else
            {
                rjTextBox2.BorderFocusColor = System.Drawing.Color.Indigo;

            }
        }

        private void rjTextBox2__TextChanged(object sender, EventArgs e)
        {
            if (rjTextBox1.Texts == "")
            {
                rjTextBox1.BorderFocusColor = System.Drawing.Color.Red;
            }
            else
            {
                rjTextBox1.BorderFocusColor = System.Drawing.Color.Indigo;

            }

            if (rjTextBox2.Texts == "")
            {
                rjTextBox2.BorderFocusColor = System.Drawing.Color.Red;
            }
            else
            {
                rjTextBox2.BorderFocusColor = System.Drawing.Color.Indigo;

            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void rjButton3_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel2.Enabled = false;
            panel3.Visible = true;
            panel3.Enabled = true;
            rjButton3.Visible = false;
            label1.Text = "SignUp";

        }

        private void rjButton5_Click(object sender, EventArgs e)
        {
            panel2.Visible=true;
            panel2.Enabled=true;
            panel3.Visible=false;
            panel3.Enabled=false;
            rjButton3.Visible=true;
            label1.Text = "SignIn";
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void rjButton2_Click(object sender, EventArgs e)
        {
           
            
        }

       



        // SignUp user code: 
        private void rjButton4_Click(object sender, EventArgs e)
        {
            string username1 = username.Texts;
            string password1 = pass.Texts;

            RegisterUser(username1, password1);
        }

        private bool RegisterUser(string username, string password)
        {
            string ipAddress = GetIPAddress();

            // Check if the IP address is already associated with a user
            int userId = GetUserIdByIPAddress(ipAddress);

            if (userId != -1)
            {
                MessageBox.Show("User with this IP address has already registered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Register the new user
            int newUserId = InsertUser(username, password);
            MessageBox.Show("SignUp successfully :)", "Success SignUp");
            // Log the IP address associated with the user
            LogIPAddress(newUserId, ipAddress);

            return true;
        }

        private int GetUserIdByIPAddress(string ipAddress)
        {
            using (SqlConnection connection = DatabaseHandler.GetSqlConnection())
            {
                try
                {
                    connection.Open();

                    // Check if the IP address is associated with a user
                    string query = "SELECT UserId FROM RegisteredIPs WHERE IPAddress = @IPAddress";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IPAddress", ipAddress);

                        object result = command.ExecuteScalar();

                        return result != null ? (int)result : -1; // Return UserId if IP is associated, otherwise -1
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            }
        }

        private int InsertUser(string username, string password)
        {
            using (SqlConnection connection = DatabaseHandler.GetSqlConnection())
            {
                try
                {
                    connection.Open();

                    // Insert a new user and return the generated UserId
                    string query = "INSERT INTO Users (Username, Password) OUTPUT INSERTED.IdUser VALUES (@Username, @Password)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        return (int)command.ExecuteScalar(); // Return the generated UserId
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            }
        }

        private void LogIPAddress(int userId, string ipAddress)
        {
            using (SqlConnection connection = DatabaseHandler.GetSqlConnection())
            {
                try
                {
                    connection.Open();

                    // Log the IP address associated with the user
                    string query = "INSERT INTO RegisteredIPs (UserId, IPAddress) VALUES (@UserId, @IPAddress)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@IPAddress", ipAddress);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetIPAddress()
        {
            // Use a simple method to retrieve the local machine's IP address
            string ipAddress = "";
            try
            {
                ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return ipAddress;
        }

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            pass.PasswordChar = false;
        }

        private void pictureBox4_MouseUp(object sender, MouseEventArgs e)
        {
            pass.PasswordChar = true;
        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
            string username = rjTextBox1.Texts.Trim().ToLower();
            string password = rjTextBox2.Texts.Trim();

            if (LoginUser(username, password))
            {
                this.Hide();
                RegisterTeam teamRegistration = new RegisterTeam(username);
                teamRegistration.ShowDialog();
                this.Show();
              
            }
            else
            {
                MessageBox.Show("Username or password incorrect, please try again", "Invalid Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                rjTextBox1.BorderFocusColor = Color.Red;
            }
        }

        private bool LoginUser(string username, string password)
        {
            using (SqlConnection connection = DatabaseHandler.GetSqlConnection())
            {
                try
                {
                    connection.Open();

                    // Check if the user with the provided username and password exists
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        int count = (int)command.ExecuteScalar();

                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
    }
}
