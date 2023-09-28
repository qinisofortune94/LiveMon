using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2.Metering
{
    public partial class MeteringProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
              if (((string)Session["LoggedIn"] == "True"))
            {
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                Label user = this.Master.FindControl("lblUser") as Label;
                Label loginUser = this.Master.FindControl("lblUser2") as Label;
                Label LastLogin = this.Master.FindControl("LastLogin") as Label;
                loginUser.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
                user.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
                LastLogin.Text = " LL:" + MyUser.LoginDT.ToString();
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }
        
        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (GridView1.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                GridView1.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        public void LoadData()
        {

          
            //GridView1.DataSource = list;
            //GridView1.DataBind();

        }
        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = GridView1.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                //string Name = GridView1.Rows[myRow.RowIndex].Cells[3].Text;
                //string Surname = GridView1.Rows[myRow.RowIndex].Cells[4].Text;
                //string IDNumber = GridView1.Rows[myRow.RowIndex].Cells[5].Text;
                //string DOB = GridView1.Rows[myRow.RowIndex].Cells[6].Text;
                //DateTime date = Convert.ToDateTime(DOB);
                //string Address = GridView1.Rows[myRow.RowIndex].Cells[7].Text;


                ViewState["Id"] = Id;


            }

            else if (commandName == "DeleteItem")
            {

                ViewState["Id"] = Id;

                //   SampleLogic business = new SampleLogic();

                //int RecordId = Convert.ToInt16(Id);
                //business.Delete(RecordId);

                //Refresh Grid
                LoadData();
            }

        }
        private bool ValidateRole()
        {
            bool valid = true;

            //if ((txtName.Text == string.Empty) && (txtSurname.Text == string.Empty))
            //{
            //    txtName.Style.Add("border", "1px solid red");
            //    txtSurname.Style.Add("border", "1px solid red");
            //    valid = false;
            //}
            //else
            //{
            //    txtName.Style.Add("border", "");
            //    txtSurname.Style.Add("border", "");
            //}

            return valid;
        }
    }
}
