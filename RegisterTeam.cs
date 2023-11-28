using CustomControls.RJControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Teams_Register
{
    public partial class RegisterTeam : Form
    {
        private int counter = 1;
        

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
        public RegisterTeam(string user)
        {
            InitializeComponent();
            label1.Text = user;
            rjTextBox1.Enabled = false;
            
            rjButton1.Enabled = true;
            rjButton2.Enabled = false;
            rjButton3.Enabled = false;
            rFCFS.Focus();

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RegisterTeam_Load(object sender, EventArgs e)
        {

        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
            


            if (counter <= 5)
            {
                rjButton3.Enabled = true; 
                string fullName = rjTextBox1.Texts; 

                ListViewItem item = new ListViewItem(counter.ToString());
                item.SubItems.Add(fullName);

                if (counter != 1)
                {
                    item.SubItems.Add("Member");
                }
                else
                {
                    item.SubItems.Add("Leader");
                }

                listView1.Items.Add(item);

                counter++;
            }
            else
            {
                label7.Text = "You cannot add more than 5 members !";
                rjButton1.Enabled = false;
           
            }
            rjButton2.Enabled = true;
        }


        private void rjButton2_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0 )
            {
                listView1.Items.Remove(listView1.SelectedItems[0]);
                counter--;
                
            }
            if (counter < 5)
            {
                rjButton1.Enabled = true;
                rjButton3.Enabled = true;
                label7.Text = ""; 
                
            }
            if (listView1.Items.Count == 0)
                rjButton3.Enabled = false;
           
            
        }

       
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void rjTextBox1__TextChanged(object sender, EventArgs e)
        {

        }

        private void rjTextBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void Project_CheckedChanged(object sender, EventArgs e)
        {
            if(sender is RJRadioButton radioButton)
            {
                rjTextBox1.Enabled = radioButton.Checked;
                label8.Visible = !radioButton.Checked;
            }
               
                
        }

        private void rjButton3_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show("لاحظ: تأكد من إكمال بيانات التيم بشكل صحيح لأن اذا تم الحفظ لا يمكنك التعديل أو الإضافة مرة اخري", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult result = MessageBox.Show("Do you sure about save?" ,"Confirmation",MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(result == DialogResult.Yes)
            {
                MessageBox.Show("Team saved successfully !");

                // Code save team data to database
            }
            
        }
    }
}
