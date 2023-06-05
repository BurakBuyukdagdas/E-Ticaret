using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_eticaret.Models;
using Web_eticaret.MVVM;
using PagedList;
using PagedList.Mvc;
using System.Collections.Specialized;

namespace Web_eticaret.Controllers
{
    public class HomeController : BaseController
    {

        //sql deki tbl_products tablosunda statusID 2 slider
        //sql deki tbl_products tablosunda statusID 1 günün ürünleri
        // En yeni ürünler adddate kolonu tersten sıralayacak en yeniden en eskiye
        // GET: Home

        cls_Urunler cu = new cls_Urunler(); // Static olmayan herşeye cu ile ulaşacağız
        AnaSayfaModel ans = new AnaSayfaModel(); // static bu nedenle new kullanıldı.

        int anasayfaadet;
        int altsayfaadet;

        public HomeController() 
        {

             anasayfaadet = Convert.ToInt32(ViewBag.anasayfaadet);
             altsayfaadet = Convert.ToInt32(ViewBag.altsayfaadet);

        } 



        public ActionResult Index()
        {
           ans.Sliderurunler = cu.urunleri_getir("slider",anasayfaadet, "anasayfa",altsayfaadet,0); // static değil
            ans.gununurunu = cls_Urunler.gunun_urununu_getir();   // static
            ans.yeniurunler = cu.urunleri_getir("yeni", anasayfaadet, "anasayfa", altsayfaadet, 0);
            ans.ozelurunler = cu.urunleri_getir("Özel", anasayfaadet, "anasayfa", altsayfaadet, 0);
            ans.yildizliurunler = cu.urunleri_getir("yıldız", anasayfaadet, "anasayfa", altsayfaadet, 0);
            ans.firsaturunler = cu.urunleri_getir("fırsat", anasayfaadet, "anasayfa", altsayfaadet, 0);
            ans.onecikanlar = cu.urunleri_getir("öneçıkanlar", anasayfaadet, "anasayfa", altsayfaadet, 0);
            ans.coksatanlar = cu.urunleri_getir("çoksatanlar", anasayfaadet, "anasayfa", altsayfaadet, 0);
            ans.indirimliurunler = cu.urunleri_getir("indirimli", anasayfaadet, "anasayfa", altsayfaadet, 0);
            return View(ans);
           
        }


        public ActionResult Cart()
        {
            cls_siparisler s = new cls_siparisler();

            if (Request.QueryString["scid"] != null)
            {
                //sil ikonuna tıklarsam
                int scid = Convert.ToInt32(Request.QueryString["scid"]);
                HttpCookie cSetCookie = Request.Cookies.Get("sepetim");
                s.sepet = cSetCookie.Value;
                s.sepetten_sil(scid.ToString());
                HttpCookie cKuki = new HttpCookie("sepetim", s.sepet);
                Response.Cookies.Add(cKuki);
                cKuki.Expires = DateTime.Now.AddMinutes(231312);
                Session["Mesaj"] = "Ürün Sepetinizden Silindi.";
                List<cls_siparisler> sepet = s.sepetiyazdir();
                ViewBag.Sepetim = sepet;
                ViewBag.sepet_tablo_detay = sepet;
            }
            else
            {
                //sağ üst kösde sepet sayfama git tıklarsam
                HttpCookie cSetCookie = Request.Cookies.Get("sepetim");
                List<cls_siparisler> sepet;
                if (cSetCookie == null)
                {
                    cSetCookie = new HttpCookie("sepetim");
                    s.sepet = "";
                    sepet = s.sepetiyazdir();
                    ViewBag.Sepetim = sepet;
                    ViewBag.sepet_tablo_detay = sepet;
                }
                else
                {
                    s.sepet = cSetCookie.Value.ToString();
                    sepet = s.sepetiyazdir();
                    ViewBag.Sepetim = sepet;
                    ViewBag.sepet_tablo_detay = sepet;
                }
            }
            return View();
        }






        public ActionResult EnYeniUrunler()
        {
            ans.yeniurunler = cu.urunleri_getir("yeni", anasayfaadet,"altsayfa",altsayfaadet,0);
            return View(ans);
        }


        public PartialViewResult _partialEnYeniUrunler(string sonrakisayfa)
        {
            int sayfano=Convert.ToInt32(sonrakisayfa);
            ans.yeniurunler= cu.urunleri_getir("yeni", anasayfaadet, "Ajax", altsayfaadet,sayfano);
            return PartialView(ans);
        }









        public ActionResult IndirimliUrunler()
        {
            ans.indirimliurunler = cu.urunleri_getir("indirimli", anasayfaadet, "altsayfa", altsayfaadet, 0);
            return View(ans);
        }






        public PartialViewResult _partialIndirimliUrunler(string sonrakisayfa)
        {
            int sayfano = Convert.ToInt32(sonrakisayfa);
            ans.indirimliurunler = cu.urunleri_getir("indirimli", anasayfaadet, "Ajax", altsayfaadet, sayfano);
            return PartialView(ans);
        }




        public ActionResult OzelUrunler()
        {
            ans.ozelurunler = cu.urunleri_getir("Özel", anasayfaadet, "altsayfa", altsayfaadet, 0);
            return View(ans);
        }


        public PartialViewResult _partialOzelUrunler(string sonrakisayfa)
        {
            int sayfano = Convert.ToInt32(sonrakisayfa);
            ans.ozelurunler = cu.urunleri_getir("Özel", anasayfaadet, "Ajax", altsayfaadet, sayfano);
            return PartialView(ans);
        }



        public ActionResult OneCikanlar()
        {
            ans.onecikanlar = cu.urunleri_getir("öneçıkanlar", anasayfaadet, "altsayfa", altsayfaadet, 0);
            return View(ans);
        }


        public PartialViewResult _partialOneCikanlar(string sonrakisayfa)
        {
            int sayfano = Convert.ToInt32(sonrakisayfa);
            ans.onecikanlar = cu.urunleri_getir("öneçıkanlar", anasayfaadet, "Ajax", altsayfaadet, sayfano);
            return PartialView(ans);
        }

       // int? Anlamı Null Gelebilmesi

        public ActionResult CokSatanlar(int? page)
        {
            using (burak_eticaretEntities db = new burak_eticaretEntities())
            {
                var ulist = db.vw_urunler.OrderByDescending(p => p.CokSatanlar).ToList();
                var pagenumber = page ?? 1; //page null Gelirse sayfa sayısı 1 olacak
                var sayfalidata = ulist.ToPagedList(pagenumber, 4);
                ViewBag.urunler=sayfalidata;
                return View();
            }
        }

        /*   Tip Dönüştürme Şekilleri :
         * 1) Convert.Toint32(isim)
         * 2) (int)isim
         * 3) isim as int
         
         */


        


    



        public ActionResult Markalar(int id,int? page)
        {
            using (burak_eticaretEntities db = new burak_eticaretEntities())
            {
                var ulist = db.vw_urunler.Where(p => p.SupplierID==id).OrderBy(p=>p.ProductName).ToList();
                var pagenumber = page ?? 1; //page null Gelirse sayfa sayısı 1 olacak
                var sayfalidata = ulist.ToPagedList(pagenumber, 4);
                ViewBag.urunler = sayfalidata;
                ViewBag.baslik = db.tbl_Suppliers.FirstOrDefault(s=>s.SupplierID==id).BrandName;
                return View();
            }
        }


        //vw_urunler urun = db2.vw_urunler.FirstOrDefault(s=>s.statusID == 1);




        public ActionResult Kategoriler(int id, int? page)
        {
            using (burak_eticaretEntities db = new burak_eticaretEntities())
            {
                var ulist = db.vw_urunler.Where(p => p.CategoryID == id).OrderBy(p => p.ProductName).ToList();
                var pagenumber = page ?? 1; //page null Gelirse sayfa sayısı 1 olacak
                var sayfalidata = ulist.ToPagedList(pagenumber, 4);
                ViewBag.urunler = sayfalidata;
                ViewBag.baslik = db.tbl_Categories.FirstOrDefault(s => s.CategoryID == id).CategoryName;
                return View();
            }
        }


        [HttpGet]

        public ActionResult UyeOl()
        {
            return View();
        }







        [HttpPost]

        public ActionResult UyeOl(tbl_Users usr)
        {

           bool cevap = cls_users.uyeekle(usr);
            if (cevap)
            {
                Session["Mesaj"] = "başarılı";
            }
            else
            {
                Session["Mesaj"] = "başarısız";
            }
            
            return View();
        }

        //Giris

        public ActionResult Giris()
        {
            return View();
        }

        [HttpPost]


        public ActionResult Giris(tbl_Users usr)
        {
            string sonuc = cls_users.uyekontrol(usr);
            if (sonuc == "Yanlış")
            {
                Session["Mesaj"] = "Email,Şifre yanlış girildi";
                return View();
            }
            else if (sonuc == "admin")
            {
                Session["Admin"] = "Admin";
                Session["email"] = sonuc;
                return RedirectToAction("index", "Admin");
            }
            else
            {
                Session["email"] = sonuc;
                return RedirectToAction("Index");
            }
            
        }

        public ActionResult Cikis()
        {
            Session.Abandon();
            Session.Remove("email");
            return RedirectToAction("Index");
        }


        public ActionResult Sepet(int id)
        {
            cls_Urunler.one_cikan_degerini_arttir(id);

            HttpCookie cSetCookie;
            cls_siparisler s = new cls_siparisler();
            s.ProductID = id;
            s.adet = 1;

            //tarayıcıda sepetim isminde cookie (çerez) varmı diye bakıyorum
            cSetCookie = Request.Cookies.Get("sepetim");
            if (cSetCookie == null)
            {
                //sepetim boşsa
                cSetCookie = new HttpCookie("sepetim");
                s.sepet = "";
            }
            else
            {
                //sepetim dolu
                //tarayıcıdaki bilgileri alıp,sepet property sine yazdırıyorum
                s.sepet = cSetCookie.Value;
            }
            //aynı ürün sepette varmı kontrolü yaptıktan sonra ,ürünü sepete ekleyecegim
            if (s.sepete_ekle(id.ToString()) == true)
            {
                cSetCookie.Values.Add(id.ToString(), "1");
                cSetCookie.Expires = DateTime.Now.AddMinutes(60 * 24 * 7); //1 haftalık Sepetimde Durcak (Yarattı)
                Response.Cookies.Add(cSetCookie); //tarayıcıda oluştu
                Session["Mesaj"] = "Ürün Sepetinize Eklendi.";
                /*string url = Request.UrlReferrer.ToString(); //o an hangi sayfadaysam,sayfanın linkini yakalıyorum
                return RedirectToAction(url); */
            }
            else
            {
                Session["Mesaj"] = "Bu Ürün Zaten Sepetinizde Var.";
            }
            return RedirectToAction("Index");
        }
        public ActionResult order()
        {
            if (Session["email"] != null)
            {
                tbl_Users usr = cls_users.uye_bilgileri_getir(Session["email"].ToString());
                return View(usr);
            }
            else
            {
                return RedirectToAction("Giris");
            }
        }

        //BurakEticaretModel
        string tckimlikno = "";
        string vergino = "";
        cls_siparisler cs = new cls_siparisler();

        [HttpPost]
        public ActionResult order(FormCollection frm)
        {
            tckimlikno = Request.Form["txt_tckimlikno"];
            vergino = Request.Form["txt_vergino"];

            if (tckimlikno != null)
            {
                cs.tcno_vergino = tckimlikno;
            }
            else
            {
                cs.tcno_vergino = vergino;
            }

            string kkno = Request.Form["kredikartno"];
            string ay = Request.Form["ay"];
            string yil = Request.Form["yil"];
            string cvs = Request.Form["cvs"];


            /*
             * Sipariş alırken ödeme aşaması bankadan hazır gelir. Altta ki örnek. Payu ve iyzico banka ile çalışan 2 farklı uygulama.
            //payu , iyzico

            NameValueCollection data = new NameValueCollection();
            string url = "https://www.sedattefci.com/backref";

            data.Add("BACK_REF", url);
            data.Add("CC_CVV", cvs);
            data.Add("CC_NUMBER", kkno);
            data.Add("EXP_MONTH", ay);
            data.Add("EXP_YEAR", yil);

            var deger = "";

            foreach (var item in data)
            {
                var value = item as string;
                var byteCount = Encoding.UTF8.GetByteCount(data.Get(value));
                deger += byteCount + data.Get(value);
            }

            var signatureKey = "size verilen SECRET_KEY değeri buraya yazılacak";
            var hash = HashWithSignature(deger, signatureKey);

            data.Add("ORDER_HASH", hash);
            var x = POSTFormPAYU("https://secure.payu.com.tr/order/....", data);

            //sanal kart
            if (x.Contains("<STATUS>SUCCESS</STATUS>") && x.Contains("<RETURN_CODE>3DS_ENROLLED</RETURN_CODE>"))
            {
                //sanal kart alıs veriş yaptı ok.
            }
            else
            {
                //gerçek kart
                //"<RETURN_CODE>AUTHORIZED</RETURN_CODE>"
            }
            */

            return RedirectToAction("backref");
        }

        public ActionResult backref()
        {
            Siparisi_Onayla();
            return RedirectToAction("Onay");
        }

        public ActionResult Siparisi_Onayla()
        {
            //siparis tablosuna kaydet
            //cookie sepeti sil
            //e-fatura olustur diye xml dosyasını olusturan metodu çağırırsınız
            HttpCookie csetCookie;
            csetCookie = Request.Cookies.Get("sepetim");
            if (csetCookie != null)
            {
                cs.sepet = csetCookie.Value.ToString();
                OrderGroupGUID = cs.cookie_sepetini_siparis_tablosuna_yaz(Session["email"].ToString());
                Response.Cookies["sepetim"].Expires = DateTime.Now.AddDays(-1);
                // cls_user.send_sms(OrderGroupGUID);
                //  cls_user.send_email(OrderGroupGUID);
            }

            return RedirectToAction("Onay");
        }

        public static string OrderGroupGUID = "";


        public ActionResult Onay()
        {
            ViewBag.OrderGroupGUID = OrderGroupGUID;
            return View();
        }


        public static string HashWithSignature(string hashString, string signature)
        {
            return "";
        }

        public static string POSTFormPAYU(string url, NameValueCollection data)
        {
            return "";
        }



        public ActionResult Siparislerim()
        {
            if (Session["email"] != null)
            {
                List<vw_siparislerim> usr = cs.siparislerimi_getir(Session["email"].ToString());
                return View(usr);
            }
            else
            {
                return RedirectToAction("Giris");
            }
        }

        public ActionResult DetayliArama()
        {
            return View();
        }


        public ActionResult dproducts(int CategoryID, string[] SupplierID, string price, string stoktavarmi)
        {
            price = price.Replace(" ", "").Replace("$","");
            string[] fiyatdizi = price.Split('-');
            string baslangicpara = fiyatdizi[0];
            string bitispara = fiyatdizi[1];

            string isaret = ">";
            if (stoktavarmi == "0")
            {
                isaret = ">=";
            }

            int count = 0;
            string marka = "";
            for (int i = 0; i < SupplierID.Length; i++)
            {
                if (count == 0)
                {
                    marka = " SupplierID= " + SupplierID[i];
                    count++;
                }
                else
                {
                    marka += " or SupplierID= " + SupplierID[i];
                }
            }

            string sorgu = "select * from tbl_Products where CategoryID = " + CategoryID + " and (" + marka + ") and (Price >= " + baslangicpara + " and Price <= " + bitispara + ") and Stok " + isaret + " 0";

            ViewBag.urunler = cs.detayli_sorgu_urunleri_getir(sorgu);

            return View();
        }

        public JsonResult urunlergetir(string id)
        {
            id = id.ToUpper(new System.Globalization.CultureInfo("tr-TR"));
            List<vw_arama> ulist = cls_Urunler.arama_getir(id);
            return Json(ulist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult detaylar(int id)
        {
            cls_Urunler.one_cikan_degerini_arttir(id);
            ans.mesajlar = cls_Urunler.mesaj_getir(id);
            ans.urun = cls_Urunler.urun_detayini_getir(id);
            ans.bunabaktilar = cls_Urunler.buna_bakan_getir(ans.urun, "bunabakan");
            ans.aynikategorili_urunler = cls_Urunler.buna_bakan_getir(ans.urun, "aynikategori");
            ans.aynimarkali_urunler = cls_Urunler.buna_bakan_getir(ans.urun, "marka");
            return View(ans);
        }

        [HttpPost]
        public ActionResult detaylar(tbl_Messages msg)
        {
            if (Session["email"] != null)
            {
                tbl_Users userinfo = cls_users.uye_bilgileri_getir(Session["email"].ToString());
                msg.UserID = userinfo.UserID;
                bool usr = cls_Urunler.mesajekle(msg);
                if (usr == true)
                {
                    Session["Mesaj"] = "Mesaj alınmıştır.";
                    ans.urun = cls_Urunler.urun_detayini_getir(msg.ProductID);
                    ans.bunabaktilar = cls_Urunler.buna_bakan_getir(ans.urun, "bunabakan");
                    ans.aynikategorili_urunler = cls_Urunler.buna_bakan_getir(ans.urun, "aynikategori");
                    ans.aynimarkali_urunler = cls_Urunler.buna_bakan_getir(ans.urun, "marka");
                    ans.mesajlar = cls_Urunler.mesaj_getir(msg.ProductID);
                    return View(ans);
                }
                else
                {
                    Session["Mesaj"] = "Mesaj kaydedilirken HATA oldu.Tekrar yazarmısınız.";
                    return View();
                }
            }
            else
            {
                Session["Mesaj"] = "Mesaj yazabilmeniz için giriş yapmanız gerekiyor.";
                return RedirectToAction("Giris");
            }
        }

        public ActionResult Mesaj(tbl_Messages msg)
        {
            return RedirectToAction("detaylar", new { id = msg.ProductID });
        }



        public ActionResult Hakkimizda()
        {
            return View();
        }

        public ActionResult İletisim()
        {
            return View();
        }


    }
}