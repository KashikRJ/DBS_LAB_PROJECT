using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace WindowsFormsApplication1
{
    public partial class ModifyDatabaseForm : Form
    {
        OracleConnection conn;

        public ModifyDatabaseForm()
        {
            InitializeComponent();
            LoadThumbnail();
            ConnectDB();
            PopulateTableSelectComboBox();
        }

        public void ConnectDB()
        {
            conn = new OracleConnection("DATA SOURCE=192.168.56.1:1521/XE;USER ID=SYSTEM;PASSWORD=student");
            try
            {
                conn.Open();
                MessageBox.Show("Connected");
            }
            catch (Exception e1)
            {
                MessageBox.Show("Error connecting to database: " + e1.Message);
            }
        }

        private void LoadThumbnail()
        {
            try
            {
                string path = "C:\\songs\\AF.png";
                pictureBox1.Image = Image.FromFile(path);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading thumbnail: " + ex.Message);
            }
        }

        private void ModifyDatabaseForm_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void PopulateTableSelectComboBox()
        {
            comboBoxTableSelect.Items.AddRange(new string[] { "genre", "artists", "Albums", "songs", "Type", "works_in", "listens", "likes", "favourites" });
        }

        private DataTable GetTableColumnInfo(string tableName)
        {
            DataTable schemaTable = new DataTable();
            string query = $"SELECT COLUMN_NAME, DATA_TYPE FROM ALL_TAB_COLUMNS WHERE TABLE_NAME = '{tableName.ToUpper()}'";

            using (OracleCommand command = new OracleCommand(query, conn))
            {
                using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                {
                    adapter.Fill(schemaTable);
                }
            }
            return schemaTable;
        }

        private void comboBoxTableSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTable = comboBoxTableSelect.SelectedItem.ToString();
            DataTable columnInfo = GetTableColumnInfo(selectedTable);
            GenerateInputFields(columnInfo);
        }

        private void GenerateInputFields(DataTable columnInfo)
        {
            panelDynamicControls.Controls.Clear();
            int yPos = 20;
            foreach (DataRow row in columnInfo.Rows)
            {
                string columnName = row["COLUMN_NAME"].ToString();
                Label label = new Label();
                label.Text = columnName;
                label.Location = new Point(10, yPos);
                panelDynamicControls.Controls.Add(label);

                TextBox textBox = new TextBox();
                textBox.Name = "textBox" + columnName;
                textBox.Location = new Point(150, yPos);
                textBox.Width = 200;
                panelDynamicControls.Controls.Add(textBox);

                yPos += 30;
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTableSelect.SelectedItem.ToString();
            InsertDataIntoTable(tableName);
        }

        private void InsertDataIntoTable(string tableName)
        {
            StringBuilder columns = new StringBuilder();
            StringBuilder values = new StringBuilder();
            OracleCommand command = new OracleCommand();
            command.Connection = conn;

            foreach (Control control in panelDynamicControls.Controls)
            {
                if (control is TextBox textBox)
                {
                    string columnName = control.Name.Substring("textBox".Length);
                    columns.Append($"{columnName}, ");
                    values.Append($":{columnName}, ");

                    OracleParameter parameter = new OracleParameter($":{columnName}", textBox.Text);
                    command.Parameters.Add(parameter);
                }
            }


            if (columns.Length > 0 && values.Length > 0)
            {
                columns.Length -= 2; // Remove last comma
                values.Length -= 2; // Remove last comma
                command.CommandText = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

                try
                {
                    //conn.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Data inserted successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error inserting data: {ex.Message}");
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void ModifyDatabaseForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
