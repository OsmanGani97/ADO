using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Project_1281404
{
    public partial class AddformBatch : Form
    {
        List<Student> students = new List<Student>();
        string currentFile = "";
        public AddformBatch()
        {
            InitializeComponent();
        }
        public Form1 TheForm {  get; set; }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Student s = new Student { Name=textBox1.Text,Tuitionfee=numericUpDown1.Value,Phone=textBox2.Text,PictureFullPath=currentFile,Picture=Path.GetFileName(currentFile)};
            s.Image = File.ReadAllBytes(currentFile);
            students.Add(s);
            currentFile = "";
            label4.Text = "";
            
            numericUpDown1.Value = 0;
            BindDataToGrid();
        }

        private void BindDataToGrid()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1 .DataSource = students;
        }

        public class Student
        {
            public string Name { get; set; }
            public decimal Tuitionfee { get; set; }
            public string Phone { get; set; }
            public string Picture { get; set; }
            public string PictureFullPath { get; set; }
            public byte[] Image { get; set; }

        }

        private void AddformBatch_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            BindDataToGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentFile = openFileDialog1.FileName;
                label4.Text = Path.GetFileName(currentFile);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["bs"].ConnectionString))
            {
                con.Open();
                using (SqlTransaction trx = con.BeginTransaction())
                {
                    string sql = "INSERT INTO batch ( batchname, startdate ,enddate,  sitavailable) VALUES(@bn, @sd, @ed,@sv); SELECT SCOPE_IDENTITY();";
                    using (SqlCommand cmd = new SqlCommand(sql, con, trx))
                    {
                        cmd.Parameters.AddWithValue("@bn", textBox3.Text);
                        cmd.Parameters.AddWithValue("@sd", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@ed", dateTimePicker2.Value);
                        cmd.Parameters.AddWithValue("@sv", checkBox1.Checked);
                        try
                        {
                            object bid = cmd.ExecuteScalar();
                            foreach (var s in students)
                            {

                                string ext = Path.GetExtension(s.Picture);
                                string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                                string savePath = @"..\..\Pictures\" + f;
                                FileStream fs = new FileStream(savePath, FileMode.Create);
                                fs.Write(s.Image, 0, s.Image.Length);
                                fs.Close();
                                cmd.CommandText = "INSERT INTO students (studentname, tuitionfee, phone, picture, batchid ) VALUES (@sn, @tf, @p, @pic, @bi)";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@sn", s.Name);
                                cmd.Parameters.AddWithValue("@tf", s.Tuitionfee);
                                cmd.Parameters.AddWithValue("@p", s.Phone);
                                
                                cmd.Parameters.AddWithValue("@pic", f);
                                cmd.Parameters.AddWithValue("@bi", bid);
                                cmd.ExecuteNonQuery();

                            }
                            trx.Commit();
                            MessageBox.Show("Data saved", "Success");
                            textBox1.Clear();
                            numericUpDown1.Value = 0;
                            textBox2.Clear();
                            label4.Text = "No Picture";
                            BindDataToGrid();
                            TheForm.BindDataToForm();

                        }
                        catch (Exception ex)
                        {
                            trx.Rollback();
                            MessageBox.Show("ERR: " + ex.Message, "Error");
                        }
                    }
                }
            }

        }
    }
  
}
