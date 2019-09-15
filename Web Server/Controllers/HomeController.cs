using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using latency_web.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json;

namespace latency_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        bool flag = false;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.flagvalue = flag;
            return View();
        }

        public IActionResult Documentation()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Index(IFormCollection form)
        {
            flag = true;
            ViewBag.flagvalue = flag;
            double latency = 0;
            ViewBag.model = form["modelradio"].ToString();
            string model = form["modelradio"].ToString();
            if(model == "I")
            {
                double requestUnixTimestamp = Math.Round(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds, 0);
                double responseUnixTimestamp = await CallInternetDB();
                double roundTripUnixTimestamp = Math.Round(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds, 0);
                latency = roundTripUnixTimestamp - requestUnixTimestamp;
                ViewBag.text = "Latency for I Model is";
                ViewBag.internetTrip = (responseUnixTimestamp - requestUnixTimestamp).ToString();
                ViewBag.intranetTrip = "-";
            }
            else if(model == "H")
            {
                double requestUnixTimestamp = Math.Round(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds, 0);
                double responseUnixTimestamp = await CallIntranetDB();
                double roundTripUnixTimestamp = Math.Round(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds, 0);
                latency = roundTripUnixTimestamp - requestUnixTimestamp;
                ViewBag.text = "Latency for H Model is";
                ViewBag.intranetTrip = (responseUnixTimestamp - requestUnixTimestamp).ToString();
                ViewBag.internetTrip = "-";
            }
            else
            {
                double internet = await CompareInternet();
                double intranet = await CompareIntranet();
                latency = intranet - internet;
                ViewBag.text = "Intranet direction requires additional";
            }

            ViewBag.result = latency.ToString();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<double> CallInternetDB()
        {
            using var http = new HttpClient();
            string requestUrl = "http://12.0.0.36:5000/api/time";
            var response = await http.GetAsync(requestUrl);
            var result = await response.Content.ReadAsStringAsync();
            DBTimeObject jsonObj = JsonConvert.DeserializeObject<DBTimeObject>(result);
            return Convert.ToDouble(jsonObj.UnixTimestamp);
        }

        private async Task<double> CallIntranetDB()
        {
            using var http = new HttpClient();
            string requestUrl = "http://12.0.0.36:5000/api/intranet/time";
            var response = await http.GetAsync(requestUrl);
            var result = await response.Content.ReadAsStringAsync();
            DBTimeObject jsonObj = JsonConvert.DeserializeObject<DBTimeObject>(result);
            return Convert.ToDouble(jsonObj.UnixTimestamp);
        }

        private async Task<double> CompareInternet()
        {
            double requestUnixTimestamp = Math.Round(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds, 0);
            double responseInternetUnixTimestamp = await CallInternetDB();
            double roundTripUnixTimestamp = Math.Round(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds, 0);
            double latency = roundTripUnixTimestamp - requestUnixTimestamp;
            ViewBag.internetTrip = (responseInternetUnixTimestamp - requestUnixTimestamp).ToString();
            return latency;
        }

        private async Task<double> CompareIntranet()
        {
            double requestUnixTimestamp = Math.Round(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds, 0);
            double responseIntranetUnixTimestamp = await CallIntranetDB();
            double roundTripUnixTimestamp = Math.Round(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds, 0);
            double latency = roundTripUnixTimestamp - requestUnixTimestamp;
            ViewBag.intranetTrip = (responseIntranetUnixTimestamp - requestUnixTimestamp).ToString();
            return latency;
        }
    }
}
