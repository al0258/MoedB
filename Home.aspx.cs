using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Home : System.Web.UI.Page
{
    private Userclass user;
    private Tests test;
    private string sql = "";
    private bool flag = false;
    private int count = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.user = Session["User"] as Userclass;
        if (this.user != null)
        {
            this.Label1.Visible = false;
            this.Panel1.Visible = false;
            if (!IsPostBack)
            {
                LoadGV();
            }
        }
        else
        {
            Response.Redirect("Index.aspx");
        }
    }
    private void LoadGV()
    {
        string sql = "select * from tbltests";
        Dal dal = new Dal();
        this.GridView1.DataSource = dal.GetDataSet(sql);
        this.GridView1.DataBind();
    }
    //protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    //{
    //    int id = int.Parse((sender as CheckBox).ToolTip);
    //    GridViewRow row = this.GridView1.Rows[id];
    //    int testid = int.Parse(row.Cells[0].Text);
    //    this.test = Tests.GetTestFromDB(testid);
    //    int studentid = this.user.Idnum;
    //    Studentintest s = new Studentintest(studentid, testid);
    //    //this.test.InsertStudent(s);
    //}
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header &&
           e.Row.RowType != DataControlRowType.Footer &&
           e.Row.RowType != DataControlRowType.Pager)
        {
            if (e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Normal))
            {
                foreach (Control c in e.Row.Cells[3].Controls)
                {
                    if (c is CheckBox)
                    {
                        (c as CheckBox).ToolTip = e.Row.RowIndex.ToString();
                    }
                }
            }
        }
    }
    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //    foreach (Control c in GridView1.Rows[GridView1.EditIndex].Controls)
    //    {
    //        if (c is CheckBox)
    //        {
    //            CheckBox b = c as CheckBox;
    //            if (b.Checked)
    //            {
    //                int id = int.Parse(b.ToolTip);
    //                GridViewRow row = this.GridView1.Rows[id];
    //                int testid = int.Parse(row.Cells[0].Text);
    //                this.test = Tests.GetTestFromDB(testid);
    //                int studentid = this.user.idnum;
    //                Studentintest s = new Studentintest(studentid, testid);
    //                this.test.InsertStudent(s);
    //            }
    //        }
    //    }
    //}
    protected void Button1_Click(object sender, EventArgs e)
    {
        List<TestView> lsView = new List<TestView>();
        foreach (GridViewRow gr in this.GridView1.Rows)
        {
            CheckBox cb = gr.FindControl("cbGo") as CheckBox;
            this.sql = string.Format("SELECT * FROM tblstudentsintest where testid={0} and studentid={1}", int.Parse(gr.Cells[0].Text), this.user.Idnum);
            Dal dal = new Dal();
            this.flag = dal.IsExist(sql);
            if (cb.Checked)
            {
                if (flag)
                {
                    this.Label1.Visible = true;
                    break;
                }
                this.count++;
                lsView.Add(new TestView() { TestId = int.Parse(gr.Cells[0].Text), TestName = gr.Cells[1].Text, DateHour = gr.Cells[2].Text });
            }
        }
        if (!flag && count > 0)
        {
            Session["selectedTestToRegister"] = lsView;
            this.GridView2.DataSource = lsView;
            this.GridView2.DataBind();
            this.Panel1.Visible = true;
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        List<Studentintest> SIT = new List<Studentintest>();
        foreach (GridViewRow gr in this.GridView1.Rows)
        {
            CheckBox cb = gr.FindControl("cbGo") as CheckBox;
            if (cb.Checked)
                SIT.Add(new Studentintest() { SId = this.user.Idnum, TId = int.Parse(gr.Cells[0].Text) });
        }
        Tests.InsertStudentsInTest(SIT);
    }
}