using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.Services;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using OfficeOpenXml;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using System.Drawing.Imaging;
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.XmlAccess;
using OfficeOpenXml.Table;

public partial class ManagerZone : System.Web.UI.Page
{
    static bool? changeadmin;
    protected void Page_Load(object sender, EventArgs e)
    {
        Userclass c = Session["User"] as Userclass;
        if (c == null || !c.IsAdmin)
            Response.Redirect("Index.aspx");
        if (!IsPostBack)
        {
            this.ExcelXport.Visible = false;
            LoadGV();
        }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header &&
           e.Row.RowType != DataControlRowType.Footer &&
           e.Row.RowType != DataControlRowType.Pager)
        {
            if (e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Normal))
            {
                foreach (Control c in e.Row.Cells[4].Controls)
                {
                    if (c is LinkButton)
                    {
                        (c as LinkButton).ToolTip = e.Row.RowIndex.ToString();
                    }
                }
            }
        }
    }
    private void LoadGV()
    {
        string sql = "select * from tbltests";
        Dal dal = new Dal();
        this.GridView1.DataSource = dal.GetDataSet(sql);
        this.GridView1.DataBind();
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int id = int.Parse(this.GridView1.Rows[e.RowIndex].Cells[1].Text);
        string sql = string.Format("delete from tbltests where testid={0}", id);
        Dal dal = new Dal();
        dal.ExecuteNonQuery(sql);
        LoadGV();
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        this.GridView1.EditIndex = e.NewEditIndex;
        LoadGV();

    }
    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        this.GridView1.EditIndex = -1;
        LoadGV();
    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int id = int.Parse(this.GridView1.Rows[e.RowIndex].Cells[1].Text);
        string date = ((TextBox)(this.GridView1.Rows[e.RowIndex].Cells[3].Controls[0])).Text;
        DateTime tdate = Convert.ToDateTime(date);
        string subject = (this.GridView1.Rows[e.RowIndex].Cells[2].Controls[0] as TextBox).Text;
        string sql = string.Format("UPDATE tbltests SET tdate='{1}', subject='{2}' where testid={0}", id, tdate, subject);
        Dal dal = new Dal();
        dal.ExecuteNonQuery(sql);
        this.GridView1.EditIndex = -1;
        LoadGV();
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GridView1.PageIndex = e.NewPageIndex;
        LoadGV();
    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect("addtest.aspx");
    }
    protected void Studentintest_Click(object sender, EventArgs e)
    {
        int id = int.Parse((sender as LinkButton).ToolTip);
        GridViewRow row = this.GridView1.Rows[id];
        int testid = int.Parse(row.Cells[1].Text);
        Session["test"] = Tests.GetTestFromDB(testid);
        Response.Redirect("StudentInTest.aspx");
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id = int.Parse(this.GridView1.Rows[this.GridView1.SelectedIndex].Cells[1].Text);
        Tests test = Tests.GetTestFromDB(id);
        this.lbdetails.Text = string.Format("תלמידים שנבחנים במקצוע: {0} בתאריך: {1} הם: ", test.TSubject, test.TTime.ToShortDateString());
        Tests.FillStudentList(test);
        this.ExcelXport.Visible = true;
        this.GridView2.DataSource = test.SIT;
        this.GridView2.DataBind();
    }

    //When you have Excel on PC
    protected void GenerateExcel(object sender, EventArgs e)
    {
        int id = int.Parse(this.GridView1.Rows[this.GridView1.SelectedIndex].Cells[1].Text);
        Tests test = Tests.GetTestFromDB(id);
        Excel.Application oXL;
        Excel._Workbook oWB;
        Excel._Worksheet oSheet;
        //Start Excel and get Application object.
        oXL = new Excel.Application();
        oXL.Visible = true;
        //Get a new workbook.
        oWB = (Excel._Workbook)(oXL.Workbooks.Add(HttpContext.Current.Server.MapPath("~/ExcelFiles/EmptyFile.xlsx")));
        oSheet = (Excel._Worksheet)oWB.ActiveSheet;

        //Add table headers going cell by cell.
        oSheet.Cells[1, 1] = "שם";
        oSheet.Cells[1, 2] = "כיתה";

        // [num , letter ]
        //מילוי תלמידים וציונים

        string sql = string.Format("select tblusers.* from tblusers,tblstudentsintest  where tblstudentsintest.testid={0} and tblstudentsintest.studentid= tblusers.idnum", test.TestId);
        Dal dal = new Dal();
        DataTable dt = dal.GetDataSet(sql).Tables[0];
        /*foreach (DataRow row in dt.Rows)
        {
            oSheet.Cells[
        }
         */
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Userclass b = new Userclass() { Idnum = int.Parse(dt.Rows[i]["idnum"].ToString()), Classroom = dt.Rows[i]["classroom"].ToString(), IsAdmin = false, Pname = dt.Rows[i]["pname"].ToString() };
            oSheet.Cells[i + 2, 1] = b.Pname;
            oSheet.Cells[i + 2, 2] = b.Classroom;
        }
        oXL.UserControl = true;
        string name = test.TSubject;
        oWB.SaveAs(HttpContext.Current.Server.MapPath("~/ExcelFiles/") + name);
        oWB.Close(false);
        oXL.Quit();
        string Url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
        HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/ExcelFiles/" + name;
        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + name + ".xlsx");
        Response.TransmitFile(Server.MapPath("~/ExcelFiles/" + name + ".xlsx"));
        Response.End();
        /*
        response.ClearContent();
        response.Clear();
        response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        response.AddHeader("Content-Disposition", "attachment; filename=" + name + ";");
        response.TransmitFile(Server.MapPath("~/ExcelFiles/" + name + ".xlsx"));
        System.IO.FileInfo file = new System.IO.FileInfo(Server.MapPath("~/ExcelFiles/" + name + ".xlsx"));
        response.Flush();
        Console.WriteLine(file.FullName);
        response.End();
         */
    }
    //When you have Excel on PC
    protected void GenerateExcelGraph(object sender, EventArgs e)
    {
        int id = int.Parse(this.GridView1.Rows[this.GridView1.SelectedIndex].Cells[1].Text);
        Tests test = Tests.GetTestFromDB(id);
        Excel.Application oXL;
        Excel._Workbook oWB;
        Excel._Worksheet oSheet;
        //Start Excel and get Application object.
        oXL = new Excel.Application();
        oXL.Visible = true;
        //Get a new workbook.
        oWB = (Excel._Workbook)(oXL.Workbooks.Add(HttpContext.Current.Server.MapPath("~/ExcelFiles/EmptyFile.xlsx")));
        oSheet = (Excel._Worksheet)oWB.ActiveSheet;

        //מילוי נתונים
        oSheet.Cells[1, 2] = "מספר תלמידים";
        oSheet.Cells[1, 1] = "כיתה";



        string sql = string.Format("select tblusers.* from tblusers,tblstudentsintest  where tblstudentsintest.testid={0} and tblstudentsintest.studentid= tblusers.idnum", test.TestId);
        Dal dal = new Dal();
        DataTable dt = dal.GetDataSet(sql).Tables[0];
        Dictionary<string, int> IDontKnowWhatTheFuckThisIs = new Dictionary<string, int>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Userclass b = new Userclass() { Idnum = int.Parse(dt.Rows[i]["idnum"].ToString()), Classroom = dt.Rows[i]["classroom"].ToString(), IsAdmin = false, Pname = dt.Rows[i]["pname"].ToString() };
            if (!IDontKnowWhatTheFuckThisIs.Keys.Contains(b.Classroom))
            {
                IDontKnowWhatTheFuckThisIs.Add(b.Classroom, 1);
            }
            else
            {
                IDontKnowWhatTheFuckThisIs[b.Classroom]++;
            }
        }
        int count = 0;
        foreach (KeyValuePair<string, int> kvp in IDontKnowWhatTheFuckThisIs)
        {
            oSheet.Cells[count + 2, 2] = kvp.Value;
            oSheet.Cells[count + 2, 1] = kvp.Key;
            count++;
        }

        //Chart Creation Check In Future
        Excel.Range rng = oXL.get_Range("A1", "B" + (count + 1));
        Excel.ChartObjects xlCharts = (Excel.ChartObjects)oSheet.ChartObjects();
        Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(150, 10, 500, 400);
        Excel.Chart chartPage = myChart.Chart;
        chartPage.HasTitle = true;
        chartPage.ChartTitle.Text = "";
        chartPage.SetSourceData(rng);
        chartPage.ChartType = Excel.XlChartType.xl3DColumnClustered;

        oXL.UserControl = true;
        string name = test.TSubject;
        oWB.SaveAs(HttpContext.Current.Server.MapPath("~/ExcelFiles/") + name);
        oWB.Close(false);
        oXL.Quit();
        string Url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
        HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/ExcelFiles/" + name;
        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + name + ".xlsx");
        Response.TransmitFile(Server.MapPath("~/ExcelFiles/" + name + ".xlsx"));
        Response.End();
    }
    //When you Dont have Excel on PC
    private DataTable ConvertToDT(Dictionary<string, int> dic)
    {
        DataTable dt = new DataTable("tblproductsinorder");
        dt.Columns.Add("class", typeof(string));
        dt.Columns.Add("count", typeof(Int32));

        foreach (KeyValuePair<string, int> kvp in dic)
        {
            DataRow row = dt.NewRow();
            row["class"] = kvp.Key.ToString();
            row["count"] = kvp.Value;
            dt.Rows.Add(row);
        }
        return dt;
    }

    protected void GenerateExcels(object sender, EventArgs e)
    {
        int id = int.Parse(this.GridView1.Rows[this.GridView1.SelectedIndex].Cells[1].Text);
        Tests test = Tests.GetTestFromDB(id);
        string name = test.TSubject;
        String path2 = HttpContext.Current.Server.MapPath("~/ExcelFiles/") + name + ".xlsx";
        Response.Clear();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(name + ".xlsx", System.Text.Encoding.UTF8));

        using (ExcelPackage pck = new ExcelPackage())
        {
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("דוח");
            ws.Cells["A2"].Value = "שם תלמיד";
            ws.Cells["B2"].Value = "כיתה";
            ws.Cells["A1:B1"].Merge = true;
            ws.Cells["A1:B1"].Value = "התלמידים הנבחנים במועד ב' ב" + name;
            ws.Cells["A1:B1"].Style.Font.Size = 18;
            ws.Cells["A2:B2"].Style.Font.Bold = true;
            ws.Cells["A1:B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify;
            ws.Column(1).Width = 30;
            ws.Column(2).Width = 30;

            string sql = string.Format("select tblusers.pname, tblusers.classroom from tblusers,tblstudentsintest  where tblstudentsintest.testid={0} and tblstudentsintest.studentid= tblusers.idnum", test.TestId);

            Dal dal = new Dal();
            DataTable dt = dal.GetDataSet(sql).Tables[0];
            dt.Columns["pname"].ColumnName = " ";
            dt.Columns["classroom"].ColumnName = "  ";
            ws.Cells["A3"].LoadFromDataTable(dt, true);


            string sqls = string.Format("select tblusers.* from tblusers,tblstudentsintest  where tblstudentsintest.testid={0} and tblstudentsintest.studentid= tblusers.idnum", test.TestId);
            Dal dals = new Dal();
            DataTable dts = dals.GetDataSet(sqls).Tables[0];
            Dictionary<string, int> IDontKnowWhatTheFuckThisIs = new Dictionary<string, int>();
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                Userclass b = new Userclass() { Idnum = int.Parse(dts.Rows[i]["idnum"].ToString()), Classroom = dts.Rows[i]["classroom"].ToString(), IsAdmin = false, Pname = dts.Rows[i]["pname"].ToString() };
                if (!IDontKnowWhatTheFuckThisIs.Keys.Contains(b.Classroom))
                {
                    IDontKnowWhatTheFuckThisIs.Add(b.Classroom, 1);
                }
                else
                {
                    IDontKnowWhatTheFuckThisIs[b.Classroom]++;
                }
            }
            // List<Dictionary<string, int>> gg = new List<Dictionary<string, int>>();
            //gg.Add(IDontKnowWhatTheFuckThisIs);
            //DataTable dtt = ToDataTable(gg);
            DataTable dtt = ConvertToDT(IDontKnowWhatTheFuckThisIs);
            /*int count = 0;
            foreach (KeyValuePair<string, int> kvp in IDontKnowWhatTheFuckThisIs)
            {
                oSheet.Cells[count + 2, 2] = kvp.Value;
                oSheet.Cells[count + 2, 1] = kvp.Key;
                count++;
            }
            */
            var ss = pck.Workbook.Worksheets.Add("גרף");
            ss.View.ShowGridLines = false;
            ss.Cells["A1"].LoadFromDataTable(dtt, true);
            ss.Cells["A1:B1"].Style.Font.Bold = true;


            var barChart = ss.Drawings.AddChart("crtFiles", eChartType.BarClustered3D) as ExcelBarChart;
            barChart.View3D.RotX = 0;
            barChart.View3D.Perspective = 0;
            barChart.SetPosition(1, 0, 2, 0);
            barChart.SetSize(800, 398);
            barChart.Series.Add(ss.Cells["B2:B4"], ss.Cells["A2:A4"]);
            //barChart.Series.Add(ss.Cells["A2:A4"], ss.Cells["B2:B4"]);
            barChart.Title.Text = "כמה תלמידים בכל כיתה במבחן מועד ב' ב" + name;
            ss.PrinterSettings.Orientation = eOrientation.Landscape;
            ss.PrinterSettings.FitToPage = true;
            ss.PrinterSettings.Scale = 67;

            var ms = new System.IO.MemoryStream();
            pck.SaveAs(ms);
            ms.WriteTo(Response.OutputStream);
        }
    }
}