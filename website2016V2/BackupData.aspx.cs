using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

using System.IO;
using System.Web.UI.WebControls;

namespace website2016V2
{

    partial class BackupData : System.Web.UI.Page
    {
        protected void Button1_Click(object sender, System.EventArgs e)
        {

            if (string.IsNullOrEmpty(txtSQLBackupPath.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select backup path.";

                return;
            }

            if (string.IsNullOrEmpty(txtBackupSQLTime.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select backup time.";

                return;
            }
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //Dim Myfunc As New LiveMonitoring.SharedFuncs

            MyRem.LiveMonServer.SetConfigSetting("SQL.Backup.Path", txtSQLBackupPath.Text);
            int MyInt = 0;
            for (MyInt = 0; MyInt <= 6; MyInt++)
            {
                MyRem.LiveMonServer.SetConfigSetting("SQL.Backup.Day." + MyInt.ToString(), cbxlSQLDays.Items[MyInt].Selected.ToString());
            }
            MyRem.LiveMonServer.SetConfigSetting("SQL.Backup.Time", txtBackupSQLTime.Text);
        }

        public void LoadFolders()
        {
            FolderBrowser.Nodes.Clear();
            string Drive = null;
            string[] Drives = Directory.GetLogicalDrives();
            foreach (string Drive_loopVariable in Drives)
            {
                Drive = Drive_loopVariable;
                Infragistics.WebUI.UltraWebNavigator.Node n = FolderBrowser.Nodes.Add(Drive, Drive);
                n.ImageUrl = "Images/LargeIcons/cdrive.gif";
                //"images/folder.gif"
            }
        }
        // As Collection
        public void ListDirectory(DirectoryInfo dir, Infragistics.WebUI.UltraWebNavigator.Node MyNode)
        {
            // List directories and files individually
            //Dim mycollection As New Collection
            DirectoryInfo d = null;
            try
            {
                foreach (DirectoryInfo d_loopVariable in dir.GetDirectories())
                {
                    d = d_loopVariable;
                    Console.WriteLine(d.FullName);
                    Infragistics.WebUI.UltraWebNavigator.Node n = MyNode.Nodes.Add(d.Name, d.FullName);
                    n.ImageUrl = "images/folder.gif";
                    //ListDirectory(d)
                    //mycollection.Add(d)
                }
            }
            catch (Exception ex)
            {
            }
            //Return mycollection
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (((string)Session["LoggedIn"] == "True"))
            {
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                System.Web.UI.WebControls.Label user = this.Master.FindControl("lblUser") as Label;
                Label loginUser = this.Master.FindControl("lblUser2") as Label;
                Label LastLogin = this.Master.FindControl("LastLogin") as Label;
                loginUser.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
                user.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
                LastLogin.Text = " LL:" + MyUser.LoginDT.ToString();

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }
                if (IsPostBack == false)
                {
                    LoadFolders();
                    LoadSettings();
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        public void LoadSettings()
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            txtSQLBackupPath.Text = MyRem.LiveMonServer.GetConfigSetting("SQL.Backup.Path");
            txtVideoDBBackupPath.Text = MyRem.LiveMonServer.GetConfigSetting("Video.Backup.Path");
            int MyInt = 0;
            for (MyInt = 0; MyInt <= 6; MyInt++)
            {
                if (!string.IsNullOrEmpty(MyRem.LiveMonServer.GetConfigSetting("SQL.Backup.Day." + MyInt.ToString())))
                {
                    cbxlSQLDays.Items[MyInt].Selected = Convert.ToBoolean(MyRem.LiveMonServer.GetConfigSetting("SQL.Backup.Day." + MyInt.ToString()));
                }
            }
            for (MyInt = 0; MyInt <= 6; MyInt++)
            {
                if (!string.IsNullOrEmpty(MyRem.LiveMonServer.GetConfigSetting("Video.Backup.Day." + MyInt.ToString())))
                {
                    cbxlVideoDays.Items[MyInt].Selected = Convert.ToBoolean(MyRem.LiveMonServer.GetConfigSetting("Video.Backup.Day." + MyInt.ToString()));
                }
            }
            txtBackupSQLTime.Text = MyRem.LiveMonServer.GetConfigSetting("SQL.Backup.Time");
            txtBackupVideoTime.Text = MyRem.LiveMonServer.GetConfigSetting("Video.Backup.Time");
        }

        protected void FolderBrowser_NodeClicked(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
        {
            ListDirectory(new DirectoryInfo(e.Node.Tag.ToString()), e.Node);
            if (lblEdit.Text == "SQL Backup Path")
            {
                txtSQLBackupPath.Text = e.Node.Tag.ToString();
            }
            else
            {
                txtVideoDBBackupPath.Text = e.Node.Tag.ToString();
            }
        }

        protected void btnSQLBrowse_Click(object sender, System.EventArgs e)
        {
            lblEdit.Visible = true;
            FolderBrowser.Visible = true;
            lblEdit.Text = "SQL Backup Path";
        }

        protected void btnVideoBrowse_Click(object sender, System.EventArgs e)
        {
            lblEdit.Visible = true;
            FolderBrowser.Visible = true;
            lblEdit.Text = "Video DB Backup Path";
        }

        protected void btnVideoDBBackupPath_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(txtVideoDBBackupPath.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select backup path.";
                return;
            }
            if (string.IsNullOrEmpty(txtBackupVideoTime.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select backup time.";
                return;
            }
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //Dim Myfunc As New LiveMonitoring.SharedFuncs

            MyRem.LiveMonServer.SetConfigSetting("Video.Backup.Path", txtVideoDBBackupPath.Text);
            int MyInt = 0;
            for (MyInt = 0; MyInt <= 6; MyInt++)
            {
                MyRem.LiveMonServer.SetConfigSetting("Video.Backup.Day." + MyInt.ToString(), cbxlVideoDays.Items[MyInt].Selected.ToString());
            }
            MyRem.LiveMonServer.SetConfigSetting("Video.Backup.Time", txtBackupVideoTime.Text);
        }
        public BackupData()
        {
            Load += Page_Load;
        }
    }
}