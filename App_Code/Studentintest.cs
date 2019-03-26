using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Studentintest
/// </summary>
public class Studentintest
{
    public int SId { get; set; }
    public int TId { get; set; }
	public Studentintest(int SId, int TId)
	{
        this.SId = SId;
        this.TId = TId;
	}
    public Studentintest()
    {
    }
}