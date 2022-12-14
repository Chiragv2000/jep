using autolandjepcore.Data;
using autolandjepcore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace autolandjepcore.Controllers
{
    public class AutolandController : Controller
    {
        private readonly jepcon _cc;
        private readonly jepconautoland _dd;
        DataTable oneday = new DataTable();
        DataTable fmonth = new DataTable();
        public List<string> acidd = new List<string>();
        public List<string> curr = new List<string>();
        DataTable prevoiusdate = new DataTable();
        DataTable prevousdatas = new DataTable();
        DataTable datawithoutfilter = new DataTable();
        public List<Result> prevresults = new List<Result>();
        public List<Result> currresults = new List<Result>();

        public AutolandController(jepcon cc, jepconautoland dd)
        {

            _cc = cc;
            _dd = dd;
        }

        public IActionResult Index()
        {
            if (TempData["curday"] == null)
            {
                Businesslogic ojbclass1 = new Businesslogic(_cc,_dd);
                (oneday,fmonth) = ojbclass1.getfltdetails();
                TempData["curday"] = JsonConvert.SerializeObject(oneday);
                TempData["current4"] = JsonConvert.SerializeObject(fmonth);

            }
            TempData.Keep();
            return View();
        }
        [HttpGet]
        public IActionResult Currentday()
        {
            DataTable curdate = JsonConvert.DeserializeObject<DataTable>((string)TempData["curday"]);

            Businesslogic ojbclass2 = new Businesslogic(_cc,_dd);
            acidd = ojbclass2.getacid(curdate);
            TempData["aircraftidcurday"] = acidd;
            TempData.Keep();
            return View();
        }

        [HttpPost]
        public IActionResult Currentday(string Aircraftselect, string statusdrop)
        {

            DataTable curdate = JsonConvert.DeserializeObject<DataTable>((string)TempData["curday"]);

            DataTable pervfourdate = JsonConvert.DeserializeObject<DataTable>((string)TempData["current4"]);

            Businesslogic ojbclass1 = new Businesslogic(_cc, _dd);
            datawithoutfilter=ojbclass1.currentdatefetch(curdate, pervfourdate);

            Businesslogic ojbclass2 = new Businesslogic(_cc, _dd);
            currresults= ojbclass2.statusdown(datawithoutfilter, Aircraftselect, statusdrop);



            TempData.Keep();

            return View(currresults);
        }

        [HttpGet]
        public IActionResult PreviousDays()
        {
            DataTable curdate = JsonConvert.DeserializeObject<DataTable>((string)TempData["curday"]);

            Businesslogic ojbclass2 = new Businesslogic(_cc, _dd);
            acidd = ojbclass2.getacid(curdate);
            TempData["aircraftidpreviousday"] = acidd;
            TempData.Keep();
            return View();
        }

        [HttpPost]
        public IActionResult PreviousDays(string Aircraftselect, string startdate)
        {

            TempData["prenewdate"] = startdate;

            if (Convert.ToDateTime(TempData["prenewdate"]).ToString("yyyy-MM-dd") != Convert.ToDateTime(TempData["preolddate"]).ToString("yyyy-MM-dd"))
            {
                Businesslogic ojbclass1 = new Businesslogic(_cc, _dd);
                (prevoiusdate, prevousdatas) = ojbclass1.previousdataget(Aircraftselect, startdate);

                Businesslogic ojbclass2 = new Businesslogic(_cc, _dd);
                prevresults= ojbclass2.previousdatacal(prevoiusdate, prevousdatas);
            }

            TempData["preolddate"] = startdate;
            TempData.Keep();
            return View(prevresults);
        }
    }
}
