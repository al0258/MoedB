using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for User
/// </summary>
public class Userclass
{
    public int Idnum { get; set; }
    public string Classroom { get; set; }
    public string Pname { get; set; }
    public bool IsAdmin { get; set; }
	public Userclass()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static Userclass GetUserFromDB(int id)
    {
        string sql = string.Format("select * from tblusers where idnum={0}", id);
        Dal dal = new Dal();
        if (dal.GetDataSet(sql).Tables.Count == 0 || dal.GetDataSet(sql).Tables[0].Rows.Count == 0)
        {
            return null;
        }
        DataRow row = dal.GetDataSet(sql).Tables[0].Rows[0];
        Userclass c = new Userclass()
        {
            Idnum = int.Parse(row["idnum"].ToString()),
            Classroom = row["classroom"].ToString(),
            Pname = row["pname"].ToString(),
            IsAdmin = bool.Parse(row["isAdmin"].ToString())
        };
        return c;
    }
}