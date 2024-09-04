using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Oracle.ManagedDataAccess.Client;
namespace WindowsFormsApplication1
{
    public partial class Login : Form
    {

        OracleConnection conn;
        public Login()
        {
            InitializeComponent();
            LoadThumbnail();
           // this.button1.Click += new System.EventHandler(this.button1_Click_1);
        }

        

        private void Form2_Load(object sender, EventArgs e)
        {
           
        }
        private void LoadThumbnail()
        {
            try
            {
                string path = "C:\\songs\\AF.png";
                pictureBox1.Image = Image.FromFile(path);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                //pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading thumbnail: " + ex.Message);
            }
        }
        public void ConnectDB()
        {

            conn = new OracleConnection("DATA SOURCE= 192.168.56.1:1521/XE;USER ID=SYSTEM;PASSWORD=student");
            try
            {
                conn.Open();
                MessageBox.Show("Connected");
            }

            catch (Exception e1)
            {
            }

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private bool AreTextFieldsEmpty()
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                return true; // Returns true if any text box is empty
            }
            return false; // Returns false if all text boxes are filled
        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (AreTextFieldsEmpty())
            {
                MessageBox.Show("Please fill in all fields before submitting.");
            }
            else
            {
                ConnectDB();

                string email = textBox2.Text;
                string inputPassword = textBox4.Text;

                using (OracleCommand command = new OracleCommand("AuthenticateUser", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email;
                    command.Parameters.Add("p_password", OracleDbType.Varchar2).Value = inputPassword;
                    command.Parameters.Add("p_user_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    command.Parameters.Add("p_message", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    string message = command.Parameters["p_message"].Value.ToString();
                    if (message == "Authentication successful")
                    {
                        int userId = Convert.ToInt32(command.Parameters["p_user_id"].Value.ToString());
                        MessageBox.Show("Correct Password");
                        Form2 f1 = new Form2(userId.ToString());
                        f1.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show(message);
                    }
                }
                conn.Close();
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            Register register = new Register();
            register.Show();
            this.Hide();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string enteredUsername = textBox2.Text;  // Assuming textBox2 is for the username
            string enteredPassword = textBox4.Text;  // Assuming textBox4 is for the password

            if (AreTextFieldsEmpty())
            {
                MessageBox.Show("Please fill in all fields before submitting.");
            }
            else if (enteredUsername == "GOD" && enteredPassword == "DEVIL")
            {
                // Open the Admin Form
                Admin adminForm = new Admin();
                adminForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid admin credentials.");
            }
        }
    }
}
