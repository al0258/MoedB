using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Tests
/// </summary>
public class Tests
{
    public int TestId { get; set; }
    public string TSubject { get; set; }
    public DateTime TTime { get; set; }
    public List<Userclass> SIT { get; set; }
    public Tests()
	{
        SIT = new List<Userclass>();
	}
    public bool Insert()
    {
        string sql = string.Format("insert into tbltests (testid, tdate, subject) values ('{0}', {1}, '{2}')",
            this.TestId, this.TTime, this.TSubject);
        Dal dal = new Dal();
        dal.ExecuteNonQuery(sql);
        return true;
    }
    public static void InsertStudentsInTest(List<Studentintest> s)
    {
        DataTable dt = ConvertToDT(s);
        Dal dal = new Dal();
        dal.FlashDTDB(dt);
    }
    public static DataTable ConvertToDT(List<Studentintest> s)
    {
        DataTable dt = new DataTable("tblstudentsintest");
        dt.Columns.Add("testid", typeof(Int32));
        dt.Columns.Add("studentid", typeof(Int32));
        for (int i = 0; i < s.Count; i++)
        {
            DataRow row = dt.NewRow();
            row["testid"] = s[i].TId;
            row["studentid"] = s[i].SId;
            dt.Rows.Add(row);
        }
        return dt;
    }
    public static Tests GetTestFromDB(int id)
    {
        string sql = string.Format("select * from tbltests where testid={0}", id);
        Dal dal = new Dal();
        if (dal.GetDataSet(sql).Tables.Count == 0 || dal.GetDataSet(sql).Tables[0].Rows.Count == 0)
        {
            return null;
        }
        DataRow row = dal.GetDataSet(sql).Tables[0].Rows[0];
        Tests c = new Tests()
        {
            TestId = int.Parse(row["testid"].ToString()),
            TSubject = row["subject"].ToString(),
            TTime = DateTime.Parse(row["tdate"].ToString())
        };
        return c;
    }


    public static void FillStudentList(Tests test)
    {
        string sql = string.Format("select tblusers.* from tblusers,tblstudentsintest  where tblstudentsintest.testid={0} and tblstudentsintest.studentid= tblusers.idnum", test.TestId);
        Dal dal = new Dal();
        DataTable dt = dal.GetDataSet(sql).Tables[0];
        foreach (DataRow row in dt.Rows)
        {
            test.SIT.Add(new Userclass(){Idnum=int.Parse (row["idnum"].ToString()), Classroom=row["classroom"].ToString(), IsAdmin = false, Pname=row["pname"].ToString()});

        }
    }
}