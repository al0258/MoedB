using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class addtest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Userclass c = Session["User"] as Userclass;
        if (c == null || !c.IsAdmin)
            Response.Redirect("Index.aspx");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DateTime date = Calendar1.SelectedDate;
        TimeSpan time = TimeSpan.Parse(TextBox1.Text);
        DateTime combined = date.Add(time);
        string classroom = TextBox2.Text;
        if (classroom != null && combined != null && combined > DateTime.Now)
        {
            string sql = string.Format("insert into tbltests (tdate, subject) values ('{0}', '{1}')",
            combined, classroom);
            Dal dal = new Dal();
            dal.ExecuteNonQuery(sql);
        }
        Response.Redirect("ManagerZone.aspx");
    }
    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        this.lbdate.Visible = true;
        this.lbdate.Text = Calendar1.SelectedDate.ToString();
    }
}