using autolandjepcore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;

namespace autolandjepcore.Data
{
    public class Businesslogic
    {
        private readonly jepcon _cc;
        private readonly jepconautoland _dd;

        public Businesslogic(jepcon cc, jepconautoland dd)
        {

            _cc = cc;
            _dd = dd;
        }
        public List<string> acidd = new List<string>();
        public List<string> curr = new List<string>();
        public List<Result> prevresults = new List<Result>();
        public List<Result> currresults = new List<Result>();
        DataTable resulttable = new DataTable();



        public (DataTable getdatasorted, DataTable curmis4sorted) getfltdetails()
        {
            List<jepsonmodel> show = new List<jepsonmodel>();
            DataTable getdatasorted = new DataTable();
            DataTable curmis4sorted = new DataTable();
            string bal = "true";
            try
            {
               // List<jepsonmodel> customers = (from customer in _cc.SP_xmldoc_bak.Take(10)
                                            //select customer).ToList();
                 show = _cc.SP_xmldoc_bak.FromSql($"exec SP_xmldoc_bak @repdtst={Convert.ToDateTime(DateTime.Now).AddDays(-1).ToString("yyyy-MM-dd")},@repdted={Convert.ToDateTime(DateTime.Now).AddDays(1).ToString("yyyy-MM-dd")}").ToList();

                DataTable getdata = CreateDataTable(show);

                getdata.Columns.Add(new DataColumn("startdatelt", typeof(string)));
                getdata.Columns.Add(new DataColumn("enddatelt", typeof(string)));
                getdata.Columns.Add(new DataColumn("stdlt", typeof(string)));
                getdata.Columns.Add(new DataColumn("stalt", typeof(string)));
                try
                {
             
                    for (int i = 0; i < getdata.Rows.Count; i++)
                    {
                        getdata.Rows[i]["startdatelt"] = Convert.ToDateTime(Convert.ToDateTime(getdata.Rows[i]["Schedulestarttime"]).AddMinutes(Convert.ToInt32(getdata.Rows[i]["starttimeoffset"]))).ToString("yyyy-MM-dd");
                        getdata.Rows[i]["enddatelt"] = Convert.ToDateTime(Convert.ToDateTime(getdata.Rows[i]["scheduleendtime"]).AddMinutes(Convert.ToInt32(getdata.Rows[i]["endtimeoffset"]))).ToString("yyyy-MM-dd");

                        getdata.Rows[i]["stdlt"] = Convert.ToDateTime(Convert.ToDateTime(getdata.Rows[i]["Schedulestarttime"]).AddMinutes(Convert.ToInt32(getdata.Rows[i]["starttimeoffset"]))).ToString("HH:mm");
                        getdata.Rows[i]["stalt"] = Convert.ToDateTime(Convert.ToDateTime(getdata.Rows[i]["scheduleendtime"]).AddMinutes(Convert.ToInt32(getdata.Rows[i]["endtimeoffset"]))).ToString("HH:mm");
                    }
                }
                catch (Exception ex)
                {

                }

                getdatasorted = getdata.Clone();

                int sdadsa = getdata.Rows.Count;

                DataRow[] draims = getdata.Select("startdatelt >= '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd") + "' and startdatelt <= '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd") + "' and OPERATIONALSTATUS='s' and AIRCRAFTTYPE <> 'ATR'");
                foreach (DataRow row in draims)
                {
                    getdatasorted.ImportRow(row);
                }
                getdatasorted.AcceptChanges();

                getdatasorted.DefaultView.Sort = "Fltnbr ASC";


                var fourmonths = _cc.SP_xmldoc_bak.FromSql($"exec SP_xmldoc_autoland @repdtst={Convert.ToDateTime(DateTime.Now).AddMonths(-4).AddDays(-1).ToString("yyyy-MM-dd")},@repdted={Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd")},@sts=true").ToList();

                DataTable currdatamis4 = CreateDataTable(fourmonths);

                currdatamis4.Columns.Add(new DataColumn("startdatelt", typeof(string)));
                currdatamis4.Columns.Add(new DataColumn("enddatelt", typeof(string)));
                currdatamis4.Columns.Add(new DataColumn("stdlt", typeof(string)));
                currdatamis4.Columns.Add(new DataColumn("stalt", typeof(string)));
                try
                {

                    for (int i = 0; i < currdatamis4.Rows.Count; i++)
                    {
                        currdatamis4.Rows[i]["startdatelt"] = Convert.ToDateTime(Convert.ToDateTime(currdatamis4.Rows[i]["Schedulestarttime"]).AddMinutes(Convert.ToInt32(currdatamis4.Rows[i]["starttimeoffset"]))).ToString("yyyy-MM-dd");
                        currdatamis4.Rows[i]["enddatelt"] = Convert.ToDateTime(Convert.ToDateTime(currdatamis4.Rows[i]["scheduleendtime"]).AddMinutes(Convert.ToInt32(currdatamis4.Rows[i]["endtimeoffset"]))).ToString("yyyy-MM-dd");

                        currdatamis4.Rows[i]["stdlt"] = Convert.ToDateTime(Convert.ToDateTime(currdatamis4.Rows[i]["Schedulestarttime"]).AddMinutes(Convert.ToInt32(currdatamis4.Rows[i]["starttimeoffset"]))).ToString("HH:mm");
                        currdatamis4.Rows[i]["stalt"] = Convert.ToDateTime(Convert.ToDateTime(currdatamis4.Rows[i]["scheduleendtime"]).AddMinutes(Convert.ToInt32(currdatamis4.Rows[i]["endtimeoffset"]))).ToString("HH:mm");
                    }
                }
                catch (Exception ex)
                {

                }


                 curmis4sorted = getdata.Clone();

                DataRow[] draim= currdatamis4.Select("startdatelt >= '" + Convert.ToDateTime(DateTime.Now).AddMonths(-4).ToString("yyyy-MM-dd") + "' and startdatelt <= '" + Convert.ToDateTime(DateTime.Now).AddDays(-1).ToString("yyyy-MM-dd") + "' and OPERATIONALSTATUS='s' and AIRCRAFTTYPE <> 'ATR' and autoland='true'");

                var dateetest = DateTime.Now.AddDays(-1);

                foreach (DataRow row in draim)
                {
                    curmis4sorted.ImportRow(row);
                }
                curmis4sorted.AcceptChanges();


                curmis4sorted = curmis4sorted.AsEnumerable().GroupBy(r => new { Col1 = r["AIRCRAFT"] }).Select(g => g.OrderBy(r => r["startdatelt"]).Last())
                 .CopyToDataTable();

                var testtt = curmis4sorted.Rows.Count;
            }
            catch (Exception ex)
            {

            }
            return (getdatasorted,curmis4sorted);
        }

        public List<string> getacid( DataTable? curdate)
        {

            acidd = curdate.AsEnumerable().OrderBy(r => r.Field<string>("AIRCRAFT")).Select(l => l.Field<string>("AIRCRAFT")).Distinct().ToList();

            return (acidd);

        }

        public DataTable currentdatefetch( DataTable curdate, DataTable pervfourdate)
        {

            DataTable autolandtruecurrdate = new DataTable();
            autolandtruecurrdate.Columns.Add(new DataColumn("S.NO", typeof(string)));
            autolandtruecurrdate.Columns.Add(new DataColumn("date", typeof(string)));
            autolandtruecurrdate.Columns.Add(new DataColumn("Aircraft", typeof(string)));
            autolandtruecurrdate.Columns.Add(new DataColumn("FltNbr", typeof(string)));
            autolandtruecurrdate.Columns.Add(new DataColumn("actype", typeof(string)));
            autolandtruecurrdate.Columns.Add(new DataColumn("sector", typeof(string)));
            autolandtruecurrdate.Columns.Add(new DataColumn("std", typeof(string)));
            autolandtruecurrdate.Columns.Add(new DataColumn("sta", typeof(string)));
            autolandtruecurrdate.Columns.Add(new DataColumn("autolastperform", typeof(string)));
            autolandtruecurrdate.Columns.Add(new DataColumn("autovalidupto", typeof(string)));
            autolandtruecurrdate.Columns.Add(new DataColumn("expirein", typeof(string)));
            autolandtruecurrdate.Columns.Add(new DataColumn("expired", typeof(string)));
            autolandtruecurrdate.Columns.Add(new DataColumn("alexpiresince", typeof(string)));
            autolandtruecurrdate.Columns.Add(new DataColumn("Remarks", typeof(string)));

            int count =1;
            DataRow[] truee = curdate.Select("Autoland='TRUE'");
            foreach (DataRow row in truee)
            {
                DataRow de = autolandtruecurrdate.NewRow();
                de["s.no"] = Convert.ToString(count++);
                de["Date"] = Convert.ToDateTime(row["stdlt"]).ToString("dd-MM-yyyy");
                de["Aircraft"] = row["aircraft"];
                de["FltNbr"] = row["fltnbr"];
                de["actype"] = row["aircrafttype"];
                de["sector"] = Convert.ToString(row["startstation"])+(Convert.ToString(row["endstation"]));
                de["std"] = Convert.ToDateTime(row["schedulestarttime"]).ToString("HH:mm");
                de["sta"] = Convert.ToDateTime(row["scheduleendtime"]).ToString("HH:mm");
                de["autolastperform"] = Convert.ToDateTime(row["stdlt"]).ToString("dd-MM-yyyy");
                de["expirein"] = "30";
                de["autovalidupto"] = Convert.ToDateTime(row["stdlt"]).AddDays(30).ToString("dd-MM-yyyy");
                de["remarks"] = "";

                autolandtruecurrdate.Rows.Add(de);
            }
          //  autolandtrue.AcceptChanges();

            DataTable autolandfalse = curdate.Clone();
           // DataTable autolandfourm = curdate.Clone();

            DataRow[] falss = curdate.Select("Autoland <> 'TRUE'");
            foreach (DataRow row in falss)
            {
                autolandfalse.ImportRow(row);
            }
            autolandfalse.AcceptChanges();



            DataTable autolandss = autolandtruecurrdate.Clone();
            int due = 0;
            count =0;
            foreach(DataRow dd in autolandfalse.Rows)
            {
                DataRow[] aa = pervfourdate.Select("AIRCRAFT = '" + Convert.ToString(dd["AIRCRAFT"]) + "'");
                if(aa.Length>0)
                {
                    foreach (DataRow row in aa)
                    {
                        DataRow de = autolandss.NewRow();
                        de["s.no"] = Convert.ToString(count++);
                        de["Date"] = Convert.ToDateTime(dd["startdatelt"]).ToString("dd-MM-yyyy");
                        de["Aircraft"] = row["aircraft"];
                        de["FltNbr"] = row["fltnbr"];
                        de["actype"] = row["aircrafttype"];
                        de["sector"] = Convert.ToString(row["startstation"]) + (Convert.ToString(row["endstation"]));
                        de["std"] = Convert.ToDateTime(row["stdlt"]).ToString("HH:mm");
                        de["sta"] = Convert.ToDateTime(row["stalt"]).ToString("HH:mm");
                        de["autolastperform"] = Convert.ToDateTime(row["startdatelt"]).ToString("dd-MM-yyyy");
                        de["autovalidupto"] = Convert.ToDateTime(row["startdatelt"]).AddDays(30).ToString("dd-MM-yyyy");
                        var expired = Convert.ToDateTime(dd["startdatelt"]) -Convert.ToDateTime(row["startdatelt"]);
                        int comp = expired.Days;
                        if (comp <= 30)
                        {
                           due = 30-comp;
                           de["expirein"] = Convert.ToString(due);
                        }
                        else if (comp > 30)
                        {
                            due=comp - 30;
                            de["expired"] = Convert.ToString(due);
                        }
                          
                        de["remarks"] = "";

                        autolandss.Rows.Add(de);
                    }

                }
                else
                {
                    DataRow de = autolandss.NewRow();
                    de["s.no"] = Convert.ToString(count++);
                    de["date"] = Convert.ToDateTime(dd["startdatelt"]).ToString("dd-MM-yyyy");
                    de["AIRCRAFT"] = dd["AIRCRAFT"];
                    de["actype"] = dd["AIRCRAFTTYPE"];
                    de["remarks"] = "DATA NOT FOUND PLEASE VALIDATE MANUALLY";
                    autolandss.Rows.Add(de);

                }
            }

            var testww = autolandss.Rows.Count;


            autolandss = autolandss.AsEnumerable().GroupBy(r => new { Col1 = r["AIRCRAFT"] }).Select(g => g.OrderBy(r => r["AIRCRAFT"]).First())
                .CopyToDataTable();

            autolandtruecurrdate = autolandtruecurrdate.AsEnumerable().GroupBy(r => new { Col1 = r["AIRCRAFT"] }).Select(g => g.OrderBy(r => r["AIRCRAFT"]).First())
                .CopyToDataTable();
            var testwww = autolandss.Rows.Count;

            autolandtruecurrdate.Merge(autolandss);



             return (autolandtruecurrdate);
        }

        public (DataTable previousdate, DataTable prevousdatas) previousdataget (string Aircraftselect,string start)
        {
                     
            var curdate = _cc.SP_xmldoc_bak.FromSql($"exec SP_xmldoc_bak @repdtst={Convert.ToDateTime(start).AddDays(-1).ToString("yyyy-MM-dd")},@repdted={Convert.ToDateTime(start).AddDays(1).ToString("yyyy-MM-dd")}").ToList();

            var months = _cc.SP_xmldoc_bak.FromSql($"exec SP_xmldoc_autoland @repdtst={Convert.ToDateTime(start).AddMonths(-4).AddDays(-1).ToString("yyyy-MM-dd")},@repdted={Convert.ToDateTime(start).AddDays(1).ToString("yyyy-MM-dd")},@sts=true").ToList();

            // var test= JsonConvert.SerializeObject(fourmonths);
            //DataTable getdatapre=JsonConvert.DeserializeObject<DataTable>((string)test);
            DataTable getdatapre = CreateDataTable(curdate);
            var test1 = getdatapre.Rows.Count;
            DataTable getdatamoneth = CreateDataTable(months);
            var test2 = getdatamoneth.Rows.Count;
            getdatapre.Merge(getdatamoneth);
            var test3 = getdatapre.Rows.Count;
            getdatapre.Columns.Add(new DataColumn("stdtotal", typeof(string)));
            getdatapre.Columns.Add(new DataColumn("statotal", typeof(string)));
            getdatapre.Columns.Add(new DataColumn("startdatelt", typeof(string)));
            getdatapre.Columns.Add(new DataColumn("enddatelt", typeof(string)));
            getdatapre.Columns.Add(new DataColumn("stdlt", typeof(string)));
            getdatapre.Columns.Add(new DataColumn("stalt", typeof(string)));

            try
            {

                for (int i = 0; i < getdatapre.Rows.Count; i++)
                {
                    getdatapre.Rows[i]["startdatelt"] = Convert.ToDateTime(Convert.ToDateTime(getdatapre.Rows[i]["Schedulestarttime"]).AddMinutes(Convert.ToInt32(getdatapre.Rows[i]["starttimeoffset"]))).ToString("yyyy-MM-dd");
                    getdatapre.Rows[i]["enddatelt"] = Convert.ToDateTime(Convert.ToDateTime(getdatapre.Rows[i]["scheduleendtime"]).AddMinutes(Convert.ToInt32(getdatapre.Rows[i]["endtimeoffset"]))).ToString("yyyy-MM-dd");

                    getdatapre.Rows[i]["stdlt"] = Convert.ToDateTime(Convert.ToDateTime(getdatapre.Rows[i]["Schedulestarttime"]).AddMinutes(Convert.ToInt32(getdatapre.Rows[i]["starttimeoffset"]))).ToString("HH:mm");
                    getdatapre.Rows[i]["stalt"] = Convert.ToDateTime(Convert.ToDateTime(getdatapre.Rows[i]["scheduleendtime"]).AddMinutes(Convert.ToInt32(getdatapre.Rows[i]["endtimeoffset"]))).ToString("HH:mm");

                }
            }
            catch (Exception ex)
            {

            }

            DataTable previousdate = getdatapre.Clone();

            DataRow[] draims = getdatapre.Select("startdatelt >= '" + Convert.ToDateTime(start).AddDays(0).ToString("yyyy-MM-dd") + "' and startdatelt <= '" + Convert.ToDateTime(start).AddDays(0).ToString("yyyy-MM-dd") + "' and OPERATIONALSTATUS='s' and AIRCRAFTTYPE <> 'ATR'");

            foreach (DataRow row in draims)
            {
                previousdate.ImportRow(row);
            }
            previousdate.AcceptChanges();

            previousdate = previousdate.AsEnumerable().GroupBy(r => new { Col1 = r["fltnbr"], Col2 = r["startstation"], Col3 = r["endstation"], Col4 = r["autoland"], Col5 = r["AIRCRAFT"] }).Select(g => g.OrderBy(r => r["startdatelt"]).First())
            .CopyToDataTable();

           var test5 = previousdate.Rows.Count;

            DataRow[] draimsdsd = getdatapre.Select("startdatelt >= '" + Convert.ToDateTime(start).AddMonths(-4).ToString("yyyy-MM-dd") + "' and startdatelt <= '" + Convert.ToDateTime(start).AddDays(0).ToString("yyyy-MM-dd") + "' and OPERATIONALSTATUS='s' and AIRCRAFTTYPE <> 'ATR' and autoland = 'true' ");

            DataTable prevousdatas = getdatapre.Clone();
            foreach (DataRow row in draimsdsd)
            {
                prevousdatas.ImportRow(row);
            }
            prevousdatas.AcceptChanges();

            prevousdatas = prevousdatas.AsEnumerable().GroupBy(r => new { Col1 = r["AIRCRAFT"] }).Select(g => g.OrderBy(r => r["startdatelt"]).Last())
             .CopyToDataTable();
            var tets6 = prevousdatas.Rows.Count;

            return (previousdate, prevousdatas);

        }

        public List<Result> statusdown( DataTable datawithoutfilter,string Aircraftselect, string statusdrop)
        {
            string exp1 = string.Empty;

            if ((DataTable)datawithoutfilter != null)
            {

                if (Convert.ToString(Aircraftselect) != null)
                {
                    exp1 += "aircraft= '" + Convert.ToString(Aircraftselect).Trim() + "' AND \n";
                }
                if (Convert.ToString(statusdrop) == "1")
                {
                    exp1 += "expirein = '30' AND \n";

                }
                if (Convert.ToString(statusdrop) == "2")
                {
                    {
                        exp1 += "expirein <= '10' AND \n";

                    }

                }
                if (Convert.ToString(statusdrop) == "3")
                {
                    {
                        exp1 += "expired >= '0' AND \n";

                    }

                }

                DataRow[] dr2;

                if (exp1.Trim() != "")
                {
                    dr2 = ((DataTable)datawithoutfilter).Select(exp1.Substring(0, exp1.LastIndexOf("AND")));
                }
                else
                {
                    dr2 = ((DataTable)datawithoutfilter).Select();
                }
                 resulttable = ((DataTable)datawithoutfilter).Clone();

                foreach (DataRow dr in dr2)
                {
                    resulttable.ImportRow(dr);
                }
                resulttable.AcceptChanges();
            }

            currresults = (from DataRow dr in resulttable.Rows
                       select new Result()
                       {
                           date = (dr["date"]).ToString(),
                           AIRCRAFT = dr["aircraft"].ToString(),
                           FltNbr = dr["fltnbr"].ToString(),
                           actype = dr["actype"].ToString(),
                           sector = dr["sector"].ToString(),
                          // startdate = dr["Actype"].ToString(),
                          // enddate = dr["STS"].ToString(),
                           std = dr["std"].ToString(),
                           sta = dr["sta"].ToString(),
                           autolastperform = dr["autolastperform"].ToString(),
                           autovalidupto = dr["autovalidupto"].ToString(),
                           expirein = dr["expirein"].ToString(),
                           expired = dr["expired"].ToString(),
                           remarks = dr["remarks"].ToString(),

                       }).ToList();

            return (currresults);
        }

        public List<Result> previousdatacal(DataTable previousdate,DataTable prevousdatas)
        {
            DataTable prviousfinal = new DataTable();
            prviousfinal.Columns.Add(new DataColumn("S.NO", typeof(string)));
            prviousfinal.Columns.Add(new DataColumn("date", typeof(string)));
            prviousfinal.Columns.Add(new DataColumn("Aircraft", typeof(string)));
            prviousfinal.Columns.Add(new DataColumn("FltNbr", typeof(string)));
            prviousfinal.Columns.Add(new DataColumn("actype", typeof(string)));
            prviousfinal.Columns.Add(new DataColumn("sector", typeof(string)));
            prviousfinal.Columns.Add(new DataColumn("startdate", typeof(string)));
            prviousfinal.Columns.Add(new DataColumn("enddate", typeof(string)));
            prviousfinal.Columns.Add(new DataColumn("std", typeof(string)));
            prviousfinal.Columns.Add(new DataColumn("sta", typeof(string)));
            prviousfinal.Columns.Add(new DataColumn("autolastperform", typeof(string)));
            prviousfinal.Columns.Add(new DataColumn("autovalidupto", typeof(string)));
            prviousfinal.Columns.Add(new DataColumn("remarks", typeof(string)));
            int count = 1;
            foreach (DataRow dd in previousdate.Rows)
            {
                DataRow[] ss = prevousdatas.Select("AIRCRAFT = '" + Convert.ToString(dd["AIRCRAFT"]) + "'");
                
                if(ss.Length>0)
                {
                    foreach(DataRow sd in ss)
                    {
                        DataRow de = prviousfinal.NewRow();
                        de["date"] = Convert.ToDateTime(dd["stdlt"]).ToString("dd-MM-yyyy");
                        de["AIRCRAFT"] = sd["AIRCRAFT"];
                        de["FltNbr"] = sd["FltNbr"];
                        de["actype"] = sd["AIRCRAFTTYPE"];
                        de["sector"] = Convert.ToString(sd["startstation"]) + (Convert.ToString(sd["endstation"]));
                        de["startdate"] = Convert.ToDateTime(sd["startdatelt"]).ToString("yyyy-MM-dd");
                        de["enddate"] = Convert.ToDateTime(sd["enddatelt"]).ToString("yyyy-MM-dd");
                        de["std"] = Convert.ToDateTime(sd["stdlt"]).ToString("HH:mm");
                        de["sta"] = Convert.ToDateTime(sd["stalt"]).ToString("HH:mm");
                        de["autolastperform"] = Convert.ToDateTime(sd["stdlt"]).ToString("dd-MM-yyyy");
                        de["autovalidupto"] = Convert.ToDateTime(sd["stdlt"]).AddDays(30).ToString("dd-MM-yyyy"); 

                        prviousfinal.Rows.Add(de);

                    }
                }
                else
                {
                    DataRow de = prviousfinal.NewRow();
                    de["date"] = Convert.ToDateTime(dd["stdlt"]).ToString("dd-MM-yyyy");
                    de["AIRCRAFT"] = dd["AIRCRAFT"];
                    de["actype"] = dd["AIRCRAFTTYPE"];
                    de["remarks"] = "DATA NOT FOUND PLEASE VALIDATE MANUALLY";
                    prviousfinal.Rows.Add(de);
                }
            }

            prevresults = (from DataRow dr in prviousfinal.Rows
                       select new Result()
                       {
                           Sno = count++,
                           date = dr["date"].ToString(),
                           AIRCRAFT = dr["AIRCRAFT"].ToString(),
                           FltNbr = dr["FltNbr"].ToString(),
                           actype = dr["actype"].ToString(),
                           sector = dr["sector"].ToString(),
                           //startdate = dr["startdate"].ToString(),
                           //enddate = dr["enddate"].ToString(),
                           sta = dr["sta"].ToString(),
                           std = dr["std"].ToString(),
                           autolastperform = dr["autolastperform"].ToString(),
                           autovalidupto = dr["autovalidupto"].ToString(),
                           remarks = dr["remarks"].ToString(),
                       }).ToList();

            return (prevresults);
        }

        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();
            DataTable dataTable = new DataTable();
            dataTable.TableName = typeof(T).FullName;
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }
            // dataTable.Columns.Add("flightload");

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

     
    }
}
