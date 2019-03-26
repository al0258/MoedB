using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        
        int id = int.Parse(tbid.Text);
        Userclass c = Userclass.GetUserFromDB(id);
        if (c == null)
        {
            this.lberror.Text = "Details Not Correct!!";
        }
        else
        {
            Session["User"] = Userclass.GetUserFromDB(id);
            Response.Redirect("ManagerZone.aspx");
        }
    }
}