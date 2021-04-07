using OnlineCVApplication.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineCVApplication
{
    public partial class CRForm : Form
    {
        public CRForm()
        {
            InitializeComponent();
        }

        private void CRForm_Load(object sender, EventArgs e)
        {
            Report2WithSqlConn();
            //Report2WithEF();
        }
        private void Report2WithSqlConn()
        {
            string quary = "SELECT * FROM [CV] WHERE [CvID] = " + CVForm.CvID;
            string connectionString = "Data Source=DESKTOP-N7PU9BJ;Initial Catalog=ProfessionalCV;Integrated Security=True;";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(quary, con);
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds, "CV");
            for (var i = 0; i < ds.Tables["CV"].Rows.Count; i++)
            {
                if (ds.Tables["CV"].Rows[i]["FilePath"] != null)
                {
                    if (!string.IsNullOrEmpty(ds.Tables["CV"].Rows[i]["FilePath"].ToString()))
                    {
                        string strFilePath = Application.StartupPath + ds.Tables["CV"].Rows[i]["FilePath"].ToString();
                        if (File.Exists(strFilePath))
                        {
                            ds.Tables["CV"].Rows[i]["FilePath"] = strFilePath;
                        }
                    }
                }
            }

            CrystalReport2 cr2 = new CrystalReport2();
            cr2.SetDataSource(ds);
            crystalReportViewer1.ReportSource = cr2;
            con.Close();
            crystalReportViewer1.Refresh();
        }

    }
}
