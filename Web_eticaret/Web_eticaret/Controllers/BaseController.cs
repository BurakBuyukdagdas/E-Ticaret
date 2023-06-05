using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Web_eticaret.Models;

namespace Web_eticaret.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base

        burak_eticaretEntities db = new burak_eticaretEntities();
        public BaseController()
        {
            ViewBag.Markalar = db.tbl_Suppliers.OrderBy(m=>m.BrandName).ToList();
            ViewBag.Kategoriler=db.tbl_Categories.OrderBy(c=>c.CategoryName).ToList();
            ViewBag.telefon=db.tbl_Settings.FirstOrDefault(i=>i.ID==1).telefon;
            ViewBag.email=db.tbl_Settings.FirstOrDefault(t=>t.ID==1).email;
            ViewBag.anasayfaadet = db.tbl_Settings.FirstOrDefault(t => t.ID == 1).anasayfaadet;
            ViewBag.altsayfaadet = db.tbl_Settings.FirstOrDefault(t => t.ID == 1).altsayfaadet;
            ViewBag.x = "38.436273";
            ViewBag.y = "27.142299";



            /*
            //4 numaralı ürünün tüm kolon bilgileri
            tbl_Products tumbilgileri = db.tbl_Products.FirstOrDefault(u => u.ProductID == 4);


            //4 numaralı ürünün sadece ürün adı
            string urunadi=db.tbl_Products.FirstOrDefault(u=>u.ProductID==4).ProductName;


            //Bütün ürünler için
            List<tbl_Products> butunurunler = db.tbl_Products.ToList();
            */

            //HAVA DURUMU WEB APİ
            string api_key = "52b72dad903d5a0244a91d029fce3686";

            string baglanti = "https://api.openweathermap.org/data/2.5/weather?q=izmir&mode=xml&lang=tr&units=metric&appid=" + api_key;

            XDocument hava = XDocument.Load(baglanti);

            var sicaklik = hava.Descendants("temperature").ElementAt(0).Attribute("value").Value;
            ViewBag.havadurumu = sicaklik;

            var durum = hava.Descendants("weather").ElementAt(0).Attribute("value").Value;
            ViewBag.durum = durum;

            var icon = hava.Descendants("weather").ElementAt(0).Attribute("icon").Value;
            ViewBag.icon = "https://openweathermap.org/img/w/" + icon + ".png";

            ViewBag.sehir = "İzmir";

            //DÖVİZ WEB APİ

            string url = "http://www.tcmb.gov.tr/kurlar/today.xml";

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(url);
            string USD_Satis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
            string usd_satis = USD_Satis.Substring(0, 5);

            ViewBag.dolar = "$ ".ToUpper() + usd_satis;




        }
    }
}