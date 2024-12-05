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

namespace Project_1281404
{
    public partial class Form1 : Form
    {
        BindingSource bsb = new BindingSource();
        BindingSource bsS = new BindingSource();
        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AddformBatch { TheForm = this }.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            BindDataToForm();
        }

        public void BindDataToForm()
        {
            using(SqlConnection con=new SqlConnection(ConfigurationManager.ConnectionStrings["bs"].ConnectionString))
            {


                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM batch", con))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "batch");
                    da.SelectCommand.CommandText = "SELECT * FROM students";
                    da.Fill(ds, "students");
                    ds.Relations.Add(new DataRelation("FK_B_S", ds.Tables["batch"].Columns["batchid"], ds.Tables["students"].Columns["batchid"]));
                    ds.Tables["students"].Columns.Add(new DataColumn("image", typeof(byte[])));
                    for (int i = 0; i < ds.Tables["students"].Rows.Count; i++)
                    {
                        ds.Tables["students"].Rows[i]["image"] = File.ReadAllBytes(@"..\..\Pictures\" + ds.Tables["students"].Rows[i]["picture"]);
                    }
                    bsb.DataSource = ds;
                    bsb.DataMember = "batch";
                    bsS.DataSource = bsb;
                    bsS.DataMember = "FK_B_S";
                    dataGridView1.DataSource = bsS;
                    lblname.DataBindings.Clear();
                    lblname.DataBindings.Add(new Binding("Text", bsb, "batchname"));
                    Binding bst = new Binding("Text", bsb, "startdate");
                    bst.Format += Bst_Format;
                    lblstart.DataBindings.Clear();
                    lblstart.DataBindings.Add(bst);
                    Binding bsp = new Binding("Text", bsb, "enddate");
                    bsp.Format += Bsp_Format;
                    lblend.DataBindings.Clear();
                    lblend.DataBindings.Add(bsp);
                }
            }
        }

        private void Bsp_Format(object sender, ConvertEventArgs e)
        {
            DateTime dt = (DateTime)e.Value;
            e.Value = dt.ToString("yyyy-MM-dd");
        }

        private void Bst_Format(object sender, ConvertEventArgs e)
        {
            DateTime dt = (DateTime)e.Value;
            e.Value = dt.ToString("yyyy-MM-dd");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (bsb.Position < bsb.Count - 1)
            {
                bsb.MoveNext();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bsb.MoveLast();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (bsb.Position > 0)
            {
                bsb.MovePrevious();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bsb.MoveFirst();
        }

        private void groupReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormReport1().Show();
        }

        private void subReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormReport2().Show();
        }
    }
}
