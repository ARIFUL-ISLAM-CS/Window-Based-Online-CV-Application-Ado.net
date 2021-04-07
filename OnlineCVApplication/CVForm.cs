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
    public partial class CVForm : Form
    {
        Image file;
        SqlConnection con = null;
        SqlCommand cmd = null;
        SqlDataAdapter adapt = null;
        public static int CvID = 0;
        public CVForm()
        {
            InitializeComponent();
            con = new SqlConnection("Data Source=DESKTOP-N7PU9BJ;Initial Catalog=ProfessionalCV;Integrated Security=True;");
            AddButtonColumn();
            LoadCVs();
            Reset();
        }

        private void LoadCVs()
        {
            con.Open();
            DataTable dt = new DataTable();
            string query = @"SELECT [CvID],[Name],[FathersName],[MothersName],[DoB],[Gender],[AddressPresent]
                  ,[AddressPermanent],[Phone],[Email],[SscYear],[SscBoard] ,[SscResult],[SscGroup],[HscYear]
                  ,[HscBoard],[HscGroup] ,[HscResult],[BachelorYear] ,[BachelorBoard] ,[BachelorResult]
                  ,[BachelorGroup] ,[1OrganizationName] ,[1Designation],[1Duration] ,[2OrganizationName] ,[2Designation] ,[2Duration]
                  ,[Ref1Name] ,[Ref1Address] ,[Ref1CompanyName] ,[Ref1Phone],[Ref1Relation] ,[Ref2Name]
                  ,[Ref2Address],[Ref2CompanyName] ,[Ref2Phone],[Ref2Relation]
                  ,[FilePath]
              FROM [CV]";
            adapt = new SqlDataAdapter(query, con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }
        private void AddButtonColumn()
        {
            DataGridViewButtonColumn btnReport = new DataGridViewButtonColumn();
            btnReport.HeaderText = "#";
            btnReport.Text = "Report";
            btnReport.Name = "btnReport";
            btnReport.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btnReport);

            DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
            btnEdit.HeaderText = "#";
            btnEdit.Text = "Edit";
            btnEdit.Name = "btnEdit";
            btnEdit.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btnEdit);

            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.HeaderText = "#";
            btnDelete.Text = "Delete";
            btnDelete.Name = "btnDelete";
            btnDelete.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btnDelete);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1 && dataGridView1.Rows.Count > e.RowIndex + 1)
            {
                var v = dataGridView1.Rows[e.RowIndex].Cells["CvID"].Value;
                CvID = dataGridView1.Rows[e.RowIndex].Cells["CvID"].Value == null ? 0 : Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["CvID"].Value);
                if ("Report" == dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    CRForm f2 = new CRForm();
                    f2.Show();
                }
                if ("Edit" == dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    Edit();
                }
                if ("Delete" == dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    Delete();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CvID > 0)
            {
                Updates();
            }
            else
            {
                AddNew();
            }
            Reset();
            LoadCVs();
        }

        private void AddNew()
        {
            string strFilePath = AddFile();

            string query = @"INSERT INTO [dbo].[CV]
                  ([Name],[FathersName],[MothersName],[DoB] ,[Gender],[AddressPresent],[AddressPermanent]
                  ,[Phone],[Email],[SscYear],[SscBoard],[SscResult],[SscGroup],[HscYear] ,[HscBoard],[HscGroup],[HscResult],[BachelorYear],[BachelorBoard]
                  ,[BachelorResult],[BachelorGroup] ,[1OrganizationName] ,[1Designation] ,[1Duration] ,[2OrganizationName] ,[2Designation] ,[2Duration]
                  ,[Ref1Name]  ,[Ref1Address]  ,[Ref1CompanyName],[Ref1Phone],[Ref1Relation],[Ref2Name],[Ref2Address]
                  ,[Ref2CompanyName],[Ref2Phone] ,[Ref2Relation] ,[FilePath])
                VALUES
               (@Name ,@FathersName,@MothersName,@DoB,@Gender,@AddressPresent,@AddressPermanent
               ,@Phone,@Email,@SscYear ,@SscBoard,@SscResult,@SscGroup,@HscYear,@HscBoard,@HscGroup,@HscResult,@BachelorYear,@BachelorBoard
               ,@BachelorResult,@BachelorGroup ,@1OrganizationName,@1Designation,@1Duration,@2OrganizationName,@2Designation
               ,@2Duration ,@Ref1Name,@Ref1Address,@Ref1CompanyName,@Ref1Phone,@Ref1Relation,@Ref2Name,@Ref2Address
               ,@Ref2CompanyName,@Ref2Phone,@Ref2Relation
               ,@FilePath)";
            cmd = new SqlCommand(query, con);
            con.Open();
            // personal info
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@FathersName", txtFathersName.Text.Trim());
            cmd.Parameters.AddWithValue("@MothersName", txtMothersName.Text.Trim());
            cmd.Parameters.AddWithValue("@DoB", pickDoB.Value);
            cmd.Parameters.AddWithValue("@Gender", radioFemale.Checked == true ? "Female" : "Male");
            cmd.Parameters.AddWithValue("@AddressPresent", txtAddressPresent.Text.Trim());
            cmd.Parameters.AddWithValue("@AddressPermanent", txtAddressPermanent.Text.Trim());
            cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

            // Academic Qualification
            cmd.Parameters.AddWithValue("@SscYear", txtSscPassYear.Text.Trim());
            cmd.Parameters.AddWithValue("@SscBoard", txtSscBoard.Text.Trim());
            cmd.Parameters.AddWithValue("@SscResult", txtSscResult.Text.Trim());
            cmd.Parameters.AddWithValue("@SscGroup", txtSscGroup.Text.Trim());
            cmd.Parameters.AddWithValue("@HscYear", txtHscPassYear.Text.Trim());
            cmd.Parameters.AddWithValue("@HscBoard", txtHscBoard.Text.Trim());
            cmd.Parameters.AddWithValue("@HscResult", txtHscResult.Text.Trim());
            cmd.Parameters.AddWithValue("@HscGroup", txtHscGroup.Text.Trim());
            cmd.Parameters.AddWithValue("@BachelorYear", txtBssPassYear.Text.Trim());
            cmd.Parameters.AddWithValue("@BachelorBoard", txtBssBoard.Text.Trim());
            cmd.Parameters.AddWithValue("@BachelorResult", txtBssResult.Text.Trim());
            cmd.Parameters.AddWithValue("@BachelorGroup", txtBssGroup.Text.Trim());

            // experience
            cmd.Parameters.AddWithValue("@1OrganizationName", txtExp1Name.Text.Trim());
            cmd.Parameters.AddWithValue("@1Designation", txtExp1Designation.Text.Trim());
            cmd.Parameters.AddWithValue("@1Duration", txtExp1Duration.Text.Trim());
            cmd.Parameters.AddWithValue("@2OrganizationName", txtExp2Name.Text.Trim());
            cmd.Parameters.AddWithValue("@2Designation", txtExp2Designation.Text.Trim());
            cmd.Parameters.AddWithValue("@2Duration", txtExp2Duration.Text.Trim());

            // Recommandation or reference
            cmd.Parameters.AddWithValue("@Ref1Name", txtRef1Name.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref1Address", txtRef1Address.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref1CompanyName", txtRef1Company.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref1Phone", txtRef1Phone.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref1Relation", txtRef1Relation.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref2Name", txtRef2Name.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref2Address", txtRef2Address.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref2CompanyName", txtRef2Company.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref2Phone", txtRef2Phone.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref2Relation", txtRef2Relation.Text.Trim());

            cmd.Parameters.AddWithValue("@FilePath", strFilePath);
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Data added successfully.");
        }
        private void Reset()
        {
            CvID = 0;
            btnSave.Text = "Add";
            newFilePath = "";
            pictureBox1.Image = null;
            using (var img = new Bitmap(Application.StartupPath + "\\images\\default_img.png"))
            {
                pictureBox1.Image = new Bitmap(img);
                lblFile.Text = "\\images\\default_img.png";
            }

            // personal info
            txtName.Text = "";
            txtFathersName.Text = "";
            txtMothersName.Text = "";
            pickDoB.Text = "";
            radioFemale.Checked = true;
            txtAddressPresent.Text = "";
            txtAddressPermanent.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";

            // career
            txtExp1Name.Text = "";
            txtExp1Designation.Text = "";
            txtExp1Duration.Text = "";
            txtExp2Name.Text = "";
            txtExp2Designation.Text = "";
            txtExp2Duration.Text = "";

            // academic
            txtSscBoard.Text = "";
            txtSscGroup.Text = "";
            txtSscResult.Text = "";
            txtSscPassYear.Text = "";
            //
            txtHscBoard.Text = "";
            txtHscGroup.Text = "";
            txtHscResult.Text = "";
            txtHscPassYear.Text = "";
            //
            txtBssPassYear.Text = "";
            txtBssBoard.Text = "";
            txtBssGroup.Text = "";
            txtBssResult.Text = "";

            // Recommandation or reference
            txtRef1Name.Text = "";
            txtRef1Address.Text = "";
            txtRef1Company.Text = "";
            txtRef1Phone.Text = "";
            txtRef1Relation.Text = "";
            txtRef2Name.Text = "";
            txtRef2Address.Text = "";
            txtRef2Company.Text = "";
            txtRef2Phone.Text = "";
            txtRef2Relation.Text = "";
        }

        private void Edit()
        {
            con.Open();
            DataTable dt = new DataTable();
            string query = @"SELECT [CvID]
                  ,[Name],[FathersName],[MothersName],[DoB] ,[Gender],[AddressPresent],[AddressPermanent],[Phone],[Email] ,[SscYear],[SscBoard]
                  ,[SscResult] ,[SscGroup],[HscYear],[HscBoard],[HscGroup],[HscResult],[BachelorYear],[BachelorBoard],[BachelorResult]
                  ,[BachelorGroup],[1OrganizationName],[1Designation],[1Duration] ,[2OrganizationName],[2Designation],[2Duration],[Ref1Name],[Ref1Address]
                  ,[Ref1CompanyName],[Ref1Phone],[Ref1Relation] ,[Ref2Name],[Ref2Address],[Ref2CompanyName],[Ref2Phone],[Ref2Relation],[FilePath]
              FROM [CV] WHERE [CvID] = " + CvID;
            adapt = new SqlDataAdapter(query, con);
            adapt.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                btnSave.Text = "Update";

                CvID = Convert.ToInt32(dt.Rows[0]["CvID"].ToString());
                // personal info
                txtName.Text = dt.Rows[0]["Name"].ToString();
                txtFathersName.Text = dt.Rows[0]["FathersName"].ToString();
                txtMothersName.Text = dt.Rows[0]["MothersName"].ToString();
                pickDoB.Value = Convert.ToDateTime(dt.Rows[0]["DoB"].ToString());
                radioFemale.Checked = dt.Rows[0]["Gender"].ToString() == "Female" ? true : false;
                radioMale.Checked = dt.Rows[0]["Gender"].ToString() == "Female" ? false : true;
                txtAddressPresent.Text = dt.Rows[0]["AddressPresent"].ToString();
                txtAddressPermanent.Text = dt.Rows[0]["AddressPermanent"].ToString();
                txtPhone.Text = dt.Rows[0]["Phone"].ToString();
                txtEmail.Text = dt.Rows[0]["Email"].ToString();

                // career
                txtExp1Name.Text = dt.Rows[0]["1OrganizationName"].ToString();
                txtExp1Designation.Text = dt.Rows[0]["1Designation"].ToString();
                txtExp1Duration.Text = dt.Rows[0]["1Duration"].ToString();
                txtExp2Name.Text = dt.Rows[0]["2OrganizationName"].ToString();
                txtExp2Designation.Text = dt.Rows[0]["2Designation"].ToString();
                txtExp2Duration.Text = dt.Rows[0]["2Duration"].ToString();


                // academic
                txtSscBoard.Text = dt.Rows[0]["SscBoard"].ToString();
                txtSscGroup.Text = dt.Rows[0]["SscGroup"].ToString();
                txtSscResult.Text = dt.Rows[0]["SscResult"].ToString();
                txtSscPassYear.Text = dt.Rows[0]["SscYear"].ToString();
                //
                txtHscBoard.Text = dt.Rows[0]["HscBoard"].ToString();
                txtHscGroup.Text = dt.Rows[0]["HscGroup"].ToString();
                txtHscResult.Text = dt.Rows[0]["HscResult"].ToString();
                txtHscPassYear.Text = dt.Rows[0]["HscYear"].ToString();
                //
                txtBssBoard.Text = dt.Rows[0]["BachelorBoard"].ToString();
                txtBssGroup.Text = dt.Rows[0]["BachelorGroup"].ToString();
                txtBssResult.Text = dt.Rows[0]["BachelorResult"].ToString();
                txtBssPassYear.Text = dt.Rows[0]["BachelorYear"].ToString();

                // Recommandation or reference
                txtRef1Name.Text = dt.Rows[0]["Ref1Name"].ToString();
                txtRef1Address.Text = dt.Rows[0]["Ref1Address"].ToString();
                txtRef1Company.Text = dt.Rows[0]["Ref1CompanyName"].ToString();
                txtRef1Phone.Text = dt.Rows[0]["Ref1Phone"].ToString();
                txtRef1Relation.Text = dt.Rows[0]["Ref1Relation"].ToString();
                txtRef2Name.Text = dt.Rows[0]["Ref2Name"].ToString();
                txtRef2Address.Text = dt.Rows[0]["Ref2Address"].ToString();
                txtRef2Company.Text = dt.Rows[0]["Ref2CompanyName"].ToString();
                txtRef2Phone.Text = dt.Rows[0]["Ref2Phone"].ToString();
                txtRef2Relation.Text = dt.Rows[0]["Ref2Relation"].ToString();


                // set image
                if (dt.Rows[0]["FilePath"].ToString() != null)
                {
                    if (File.Exists(Application.StartupPath + dt.Rows[0]["FilePath"].ToString()))
                    {
                        using (var img = new Bitmap(Application.StartupPath + dt.Rows[0]["FilePath"].ToString()))
                        {
                            pictureBox1.Image = new Bitmap(img);
                            lblFile.Text = dt.Rows[0]["FilePath"].ToString();
                            isNewFile = false;
                            oldFilePath = dt.Rows[0]["FilePath"].ToString();
                        }
                    }
                    else
                    {
                        using (var img = new Bitmap(Application.StartupPath + "\\images\\default_img.png"))
                        {
                            pictureBox1.Image = new Bitmap(img);
                            lblFile.Text = "\\images\\default_img.png";
                        }
                    }
                }
                else
                {
                    using (var img = new Bitmap(Application.StartupPath + "\\images\\default_img.png"))
                    {
                        pictureBox1.Image = new Bitmap(img);
                        lblFile.Text = "\\images\\default_img.png";
                    }
                }
            }
        }
        private void Updates()
        {
            // save image
            string strFilePath = UpdateFile();

            cmd = new SqlCommand(@"UPDATE [CV]
               SET [Name]=@Name
                  ,[FathersName]=@FathersName
                  ,[MothersName]=@MothersName
                  ,[DoB]=@DoB
                  ,[Gender]=@Gender
                  ,[AddressPresent]=@AddressPresent
                  ,[AddressPermanent]=@AddressPermanent
                  ,[Phone]=@Phone
                  ,[Email]=@Email
                  ,[SscYear]=@SscYear
                  ,[SscBoard]=@SscBoard
                  ,[SscResult]=@SscResult
                  ,[SscGroup]=@SscGroup
                  ,[HscYear]=@HscYear
                  ,[HscBoard]=@HscBoard
                  ,[HscGroup]=@HscGroup
                  ,[HscResult]=@HscResult
                  ,[BachelorYear]=@BachelorYear
                  ,[BachelorBoard]=@BachelorBoard
                  ,[BachelorResult]=@BachelorResult
                  ,[BachelorGroup]=@BachelorGroup
                  ,[1OrganizationName]=@1OrganizationName
                  ,[1Designation]=@1Designation
                  ,[1Duration]=@1Duration
                  ,[2OrganizationName]=@2OrganizationName
                  ,[2Designation]=@2Designation
                  ,[2Duration]=@2Duration
                  ,[Ref1Name]=@Ref1Name
                  ,[Ref1Address]=@Ref1Address
                  ,[Ref1CompanyName]=@Ref1CompanyName
                  ,[Ref1Phone]=@Ref1Phone
                  ,[Ref1Relation]=@Ref1Relation
                  ,[Ref2Name]=@Ref2Name
                  ,[Ref2Address]=@Ref2Address
                  ,[Ref2CompanyName]=@Ref2CompanyName
                  ,[Ref2Phone]=@Ref2Phone
                  ,[Ref2Relation]=@Ref2Relation
                  ,[FilePath] = @FilePath
             WHERE [CvID] = @CvID", con);
            con.Open();
            cmd.Parameters.AddWithValue("@CvID", CvID);
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@FathersName", txtFathersName.Text.Trim());
            cmd.Parameters.AddWithValue("@MothersName", txtMothersName.Text.Trim());
            cmd.Parameters.AddWithValue("@DoB", pickDoB.Value);
            cmd.Parameters.AddWithValue("@Gender", radioFemale.Checked == true ? "Female" : "Male");
            cmd.Parameters.AddWithValue("@AddressPresent", txtAddressPresent.Text.Trim());
            cmd.Parameters.AddWithValue("@AddressPermanent", txtAddressPermanent.Text.Trim());
            cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

            // Academic Qualification
            cmd.Parameters.AddWithValue("@SscYear", txtSscPassYear.Text.Trim());
            cmd.Parameters.AddWithValue("@SscBoard", txtSscBoard.Text.Trim());
            cmd.Parameters.AddWithValue("@SscResult", txtSscResult.Text.Trim());
            cmd.Parameters.AddWithValue("@SscGroup", txtSscGroup.Text.Trim());
            cmd.Parameters.AddWithValue("@HscYear", txtHscPassYear.Text.Trim());
            cmd.Parameters.AddWithValue("@HscBoard", txtHscBoard.Text.Trim());
            cmd.Parameters.AddWithValue("@HscResult", txtHscResult.Text.Trim());
            cmd.Parameters.AddWithValue("@HscGroup", txtHscGroup.Text.Trim());
            cmd.Parameters.AddWithValue("@BachelorYear", txtBssPassYear.Text.Trim());
            cmd.Parameters.AddWithValue("@BachelorBoard", txtBssBoard.Text.Trim());
            cmd.Parameters.AddWithValue("@BachelorResult", txtBssResult.Text.Trim());
            cmd.Parameters.AddWithValue("@BachelorGroup", txtBssGroup.Text.Trim());

            // experience
            cmd.Parameters.AddWithValue("@1OrganizationName", txtExp1Name.Text.Trim());
            cmd.Parameters.AddWithValue("@1Designation", txtExp1Designation.Text.Trim());
            cmd.Parameters.AddWithValue("@1Duration", txtExp1Duration.Text.Trim());
            cmd.Parameters.AddWithValue("@2OrganizationName", txtExp2Name.Text.Trim());
            cmd.Parameters.AddWithValue("@2Designation", txtExp2Designation.Text.Trim());
            cmd.Parameters.AddWithValue("@2Duration", txtExp2Duration.Text.Trim());

            // Recommandation or reference
            cmd.Parameters.AddWithValue("@Ref1Name", txtRef1Name.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref1Address", txtRef1Address.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref1CompanyName", txtRef1Company.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref1Phone", txtRef1Phone.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref1Relation", txtRef1Relation.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref2Name", txtRef2Name.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref2Address", txtRef2Address.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref2CompanyName", txtRef2Company.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref2Phone", txtRef2Phone.Text.Trim());
            cmd.Parameters.AddWithValue("@Ref2Relation", txtRef2Relation.Text.Trim());

            cmd.Parameters.AddWithValue("@FilePath", strFilePath);
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Data updated successfully.");
        }
      
        private void OpenFile()
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "JPG (*.JPG)|*.jpg";
            if (f.ShowDialog() == DialogResult.OK)
            {
                file = Image.FromFile(f.FileName);
                pictureBox1.Image = file;
                lblFile.Text = f.FileName;
            }
        }
        string newFilePath = string.Empty;
        string oldFilePath = string.Empty;
        bool isNewFile = true;
        OpenFileDialog open = new OpenFileDialog();
        private void SelectFile()
        {
            open.Filter = "JPG (*.JPG)|*.jpg";
            if (open.ShowDialog() == DialogResult.OK)
            {
                using (var img = new Bitmap(open.FileName))
                {
                    pictureBox1.Image = new Bitmap(img);
                }
                newFilePath = open.FileName;
                isNewFile = true;
            }
        }

        private string AddFile()
        {
            string strFilePath = string.Empty;
            if (isNewFile)
            {
                strFilePath = "\\images\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                File.Copy(newFilePath, Application.StartupPath + strFilePath);
            }

            return strFilePath;
        }

        private string UpdateFile()
        {
            string strFilePath = string.Empty;
            if (isNewFile)
            {
                strFilePath = "\\images\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                File.Copy(newFilePath, Application.StartupPath + strFilePath);
                RemoveFile(Application.StartupPath + oldFilePath);
            }
            else
            {
                strFilePath = oldFilePath;
            }

            return strFilePath;
        }

        private void RemoveFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                if (!filePath.Contains("default"))
                {
                    File.Delete(filePath);
                }
                pictureBox1.Image = null;
            }
        }

        private void Delete()
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to remove?", "Confirm Message", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                con.Open();
                DataTable dt = new DataTable();
                string query = @"SELECT [CvID]
                      ,[Name]
                  ,[FathersName]
                  ,[MothersName]
                  ,[DoB]
                  ,[Gender]
                  ,[AddressPresent]
                  ,[AddressPermanent]
                  ,[Phone]
                  ,[Email]
                  ,[SscYear]
                  ,[SscBoard]
                  ,[SscResult]
                  ,[SscGroup]
                  ,[HscYear]
                  ,[HscBoard]
                  ,[HscGroup]
                  ,[HscResult]
                  ,[BachelorYear]
                  ,[BachelorBoard]
                  ,[BachelorResult]
                  ,[BachelorGroup]
                  ,[1OrganizationName]
                  ,[1Designation]
                  ,[1Duration]
                  ,[2OrganizationName]
                  ,[2Designation]
                  ,[2Duration]
                  ,[Ref1Name]
                  ,[Ref1Address]
                  ,[Ref1CompanyName]
                  ,[Ref1Phone]
                  ,[Ref1Relation]
                  ,[Ref2Name]
                  ,[Ref2Address]
                  ,[Ref2CompanyName]
                  ,[Ref2Phone]
                  ,[Ref2Relation]
                  ,[FilePath]
                  FROM [CV] WHERE [CvID] = " + CvID;
                adapt = new SqlDataAdapter(query, con);
                adapt.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["FilePath"] != null)
                    {
                        // remove old file
                        RemoveFile(Application.StartupPath + dt.Rows[0]["FilePath"].ToString());
                    }

                    string q = @"DELETE FROM [CV]
                    WHERE CvID = @CvID";
                    cmd = new SqlCommand(q, con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@CvID", CvID);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Reset();
                    LoadCVs();
                    MessageBox.Show("Data removed successfully.");

                }
            }
            else if (dialogResult == DialogResult.No)
            {
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SelectFile();
        }
    }
}
