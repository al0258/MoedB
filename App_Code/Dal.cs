using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;

/// <summary>
/// Summary description for dal
/// </summary>
public class Dal
{
	public Dal()
	{
		
	}
    private static string GetconnectionsString()
    {
        string dbPath = HttpContext.Current.Server.MapPath("~/App_Data/usersdb.accdb");
        string conPath = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPath;
        return conPath;
    }
    public void ExecuteNonQuery(string sql)
    {
        OleDbConnection con = new OleDbConnection(GetconnectionsString());
        OleDbCommand cmd = new OleDbCommand(sql, con);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
    }
    public bool IsExist(string sql)
    {
        OleDbConnection con = new OleDbConnection(GetconnectionsString());
        con.Open();
        OleDbCommand cmd = new OleDbCommand(sql, con);
        OleDbDataReader data = cmd.ExecuteReader();
        bool found;
        found = (bool)data.Read();
        con.Close();
        return found;
    }
    public object ExecuteScalar(string sql)
    {
        OleDbConnection con = new OleDbConnection(GetconnectionsString());
        OleDbCommand cmd = new OleDbCommand(sql, con);
        con.Open();
        object o = cmd.ExecuteScalar();
        con.Close();
        return o;
    }
    public DataSet GetDataSet(string sql)
    {
        OleDbConnection con = new OleDbConnection(GetconnectionsString());
        OleDbDataAdapter da = new OleDbDataAdapter(sql, con);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    public void FlashDTDB(DataTable dt)
    {
        string sql = "select * from tblstudentsintest";
        OleDbConnection con = new OleDbConnection(GetconnectionsString());
        OleDbCommand cmd = new OleDbCommand(sql, con);
        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
        OleDbCommandBuilder builder = new OleDbCommandBuilder(da);
        da.UpdateCommand = builder.GetUpdateCommand();
        da.Update(dt);
    }
    public bool FlashDataTableToDB(string dbTableName, DataTable dtToInsert)
    {
        try
        {
            string sql = "select * from " + dbTableName;
            OleDbConnection con = new OleDbConnection(GetconnectionsString());
            OleDbCommand cmd = new OleDbCommand(sql, con);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            OleDbCommandBuilder builder = new OleDbCommandBuilder(da);
            da.UpdateCommand = builder.GetUpdateCommand();
            da.Update(dtToInsert);
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }
}