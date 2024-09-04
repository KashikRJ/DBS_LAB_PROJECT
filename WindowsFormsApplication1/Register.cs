using System.Windows.Forms;
using System;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;

namespace WindowsFormsApplication1
{
    public partial class Register : Form
    {
        OracleConnection conn;
        public Register()
        {
            InitializeComponent();
            LoadThumbnail();
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
        private bool AreTextFieldsEmpty()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                return true; // Returns true if any text box is empty
            }
            return false; // Returns false if all text boxes are filled
        }
        private void button1_Click(object sender, EventArgs e)
        {

            if (AreTextFieldsEmpty())
            {
                MessageBox.Show("Please fill in all fields before submitting.");
            }
            else
            {
               
                ConnectDB();
                OracleCommand command1 = conn.CreateCommand();
                int val = new Random().Next(50, 100);
                command1.CommandText = "insert into users values(" + val + "," + "'" + textBox1.Text + "'" + "," + "'" + textBox2.Text + "'" + "," + "'" + textBox3.Text + "'" + "," + "'" + textBox4.Text + "'" + ")";
                command1.CommandType = CommandType.Text;
                command1.ExecuteNonQuery();
                MessageBox.Show("Inserted into Users Table");
                command1.Dispose();
                conn.Close();

                //Give code to connect to Home page
                Form1 f1 = new Form1(val.ToString());
                f1.Show();
                this.Hide();
            }




        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void Register_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login f2 = new Login();
            f2.Show();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}








