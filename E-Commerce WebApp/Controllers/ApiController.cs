using E_Commerce_WebApp.Models.MVVM;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;

namespace E_Commerce_WebApp.Controllers
{
    public class ApiController : Controller
    {
        public async Task<IActionResult> PharmacyOnDuty()
        {
            using var client = new HttpClient();
            var json = await client.GetStringAsync(
                "https://openapi.izmir.bel.tr/api/ibb/nobetcieczaneler"
            );
            var pharmacy = JsonConvert.DeserializeObject<List<Pharmacy>>(json);
            return View(pharmacy);
        }

        public async Task<IActionResult> ArtAndCulture()
        {
            using var client = new HttpClient();
            var json = await client.GetStringAsync(
                "https://openapi.izmir.bel.tr/api/ibb/kultursanat/etkinlikler"
            );
            var activite = JsonConvert.DeserializeObject<List<Activite>>(json);
            return View(activite);
        }

        public IActionResult ExchangeRate()
        {
            string url = "http://www.tcmb.gov.tr/kurlar/today.xml";

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(url);

            string dolaralis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;
            ViewBag.dolaralis = dolaralis.Substring(0, 5);

            string dolarsatis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
            ViewBag.dolarsatis = dolarsatis.Substring(0, 5);

            string euroalis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;
            ViewBag.euroalis = euroalis.Substring(0, 5);

            string eurosatis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;
            ViewBag.eurosatis = eurosatis.Substring(0, 5);

            return View();
        }

        public IActionResult WeatherForecast()
        {
            string apikey = "52b72dad903d5a0244a91d029fce3686";
            string city = "izmir";

            //https://api.openweathermap.org/data/2.5/weather?q=izmir&mode=xml&lang=tr&units=metric&appid=52b72dad903d5a0244a91d029fce3686

            string url = "https://api.openweathermap.org/data/2.5/weather?q=" + city + "&mode=xml&lang=tr&units=metric&appid=" + apikey;

            XDocument weather = XDocument.Load(url);

            ViewBag.temperature = weather.Descendants("temperature").ElementAt(0).Attribute("value").Value;

            ViewBag.City = city.ToUpper();

            string icon = weather.Descendants("weather").ElementAt(0).Attribute("icon").Value;
            ViewBag.iconurl = "https://openweathermap.org/img/wn/" + icon + ".png";

            return View();
        }
    }
}