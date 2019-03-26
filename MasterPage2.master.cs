using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage2 : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
            HandleGuess();
        else
        {
            HandleLogin();
        }
    }
    private void HandleLogin() // קבלת פנים של משתמש רשום באתר
    {

        Userclass c = Session["User"] as Userclass;

        string username = c.Pname;
        this.lbName.Text = "שלום" + " " + username;
        this.logstat.ImageUrl = "img/logout.png";
        this.logstat.NavigateUrl = "logout.aspx";

    }
    private void HandleGuess() //קבלת פנים של מבקר באתר
    {

        this.lbName.Text = "שלום אורח ";
        this.logstat.ImageUrl = "img/login.png";
        this.logstat.NavigateUrl = "Login.aspx";
    }
}
