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
    public partial class FormReport2 : Form
    {
        public FormReport2()
        {
            InitializeComponent();
        }

        private void FormReport2_Load(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["bs"].ConnectionString))
            {


                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM batch", con))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "batch");
                    da.SelectCommand.CommandText = "SELECT * FROM students";
                    da.Fill(ds, "students1");

                    ds.Tables["students1"].Columns.Add(new DataColumn("image", typeof(byte[])));
                    for (int i = 0; i < ds.Tables["students1"].Rows.Count; i++)
                    {
                        ds.Tables["students1"].Rows[i]["image"] = File.ReadAllBytes(@"..\..\Pictures\" + ds.Tables["students1"].Rows[i]["picture"]);
                    }
                    Reports.CrystalReport2 rpt = new Reports.CrystalReport2();
                    rpt.SetDataSource(ds);
                    crystalReportViewer1.ReportSource = rpt;
                    rpt.Refresh();
                    crystalReportViewer1.Refresh();
                }
            }
        }
    }
}
