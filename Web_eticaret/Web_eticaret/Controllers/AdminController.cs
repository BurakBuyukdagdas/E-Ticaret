using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_eticaret.Models;
using System.IO;

namespace Web_eticaret.Controllers
{
    public class AdminController : Controller
    {
        burak_eticaretEntities db = new burak_eticaretEntities();
        cls_Urunler cu = new cls_Urunler();

        // GET: Admin


        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult KategoriGiris()
        {
            List<tbl_Categories> catlist = db.tbl_Categories.ToList();
            dropDownListDoldur();
            return View(catlist);
        }

        void dropDownListDoldur()
        {
            List<tbl_Categories> menuList = db.tbl_Categories.Where(c => c.ParentID == 0).ToList();
            ViewData["sitemenuList"] = menuList.Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });
        }

        [HttpPost]
        public ActionResult KategoriGiris(tbl_Categories cat)
        {
            if (cat.ParentID == null)
            {
                cat.ParentID = 0;
            }
            cat.Active = true;
            db.tbl_Categories.Add(cat);
            db.SaveChanges();

            return RedirectToAction("KategoriGiris"); // [HttpGet] e gider, oradan .cshtml sayfasına veri gönderiliyor
        }


        [HttpGet]
        public ActionResult MarkaGiris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MarkaGiris(tbl_Suppliers sup, HttpPostedFileBase fileuploader)
        {
            sup.Active = true;
            sup.PhotoPath = fileuploader.FileName; //resmin ismini tabloya yazar

            //resim dosyasını fiziksel olarak /Content/resimler klasörüne kopyalamam lazım
            if (fileuploader != null)
            {
                string path = Path.Combine(Server.MapPath("~/Content/resimler"), Path.GetFileName(fileuploader.FileName));
                fileuploader.SaveAs(path);
            }

            try
            {
                db.tbl_Suppliers.Add(sup);
                db.SaveChanges();
                Session["Mesaj"] = sup.BrandName + " markası başarıyla kaydedildi.";
            }
            catch (Exception)
            {
                Session["Mesaj"] = "HATA " + sup.BrandName + " markası kaydedilemedi.";
            }
            return View();
        }



        [HttpGet]
        public ActionResult StatusGiris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StatusGiris(tbl_Status stat)
        {
            try
            {
                db.tbl_Status.Add(stat);
                db.SaveChanges();
                Session["Mesaj"] = "Başarıyla kaydedildi.";
            }
            catch (Exception)
            {
                Session["Mesaj"] = "HATA... kaydedilemedi.";
            }


            return View();
        }



        [HttpGet]
        public ActionResult UrunGiris()
        {
            List<tbl_Categories> catList = db.tbl_Categories.ToList();
            ViewData["CategoryList"] = catList.Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });

            List<tbl_Suppliers> supList = db.tbl_Suppliers.ToList();
            ViewData["SupplierList"] = supList.Select(c => new SelectListItem { Text = c.BrandName, Value = c.SupplierID.ToString() });

            List<tbl_Status> statList = db.tbl_Status.ToList();
            ViewData["StatusList"] = statList.Select(c => new SelectListItem { Text = c.StatusName, Value = c.statusID.ToString() });

            return View();
        }

        [HttpPost]
        public ActionResult UrunGiris(tbl_Products pro, HttpPostedFileBase fileuploader)
        {

            if (pro.AddDate == null)
            {
                pro.AddDate = DateTime.Now;
            }
            if (pro.BunaBakanlar == null)
            {
                pro.BunaBakanlar = 0;
            }
            if (pro.CokSatanlar == null)
            {
                pro.CokSatanlar = 0;
            }
            if (pro.Discount == null)
            {
                pro.Discount = 0;
            }
            if (pro.Keywords == null)
            {
                pro.Keywords = "";
            }
            if (pro.Notes == null)
            {
                pro.Notes = "";
            }
            if (pro.OneCikanlar == null)
            {
                pro.OneCikanlar = 0;
            }
            if (pro.statusID == null)
            {
                pro.statusID = 0;
            }

            pro.Active = true;
            pro.PhotoPath = fileuploader.FileName; //resmin ismini tabloya yazar

            //resim dosyasını fiziksel olarak /Content/resimler klasörüne kopyalamam lazım
            if (fileuploader != null)
            {
                string path = Path.Combine(Server.MapPath("~/Content/resimler"), Path.GetFileName(fileuploader.FileName));
                fileuploader.SaveAs(path);
            }

            try
            {
                tbl_Products prdfind = db.tbl_Products.FirstOrDefault(p=>p.ProductName.ToLower().Trim()==pro.ProductName.ToLower().Trim());


                if (prdfind == null)
                {
                    db.tbl_Products.Add(pro);
                    db.SaveChanges();
                    Session["Mesaj"] = pro.ProductName + " ürünü başarıyla kaydedildi.";
                }
                else
                {
                    Session["Mesaj"] = pro.ProductName + "Bu ürün daha önce girilmiş.";
                }
                
            }
            catch (Exception)
            {
                Session["Mesaj"] = "HATA " + pro.ProductName + " ürünü kaydedilemedi.";
            }
            return RedirectToAction("UrunGiris"); // [HttpGet] e gider, oradan .cshtml sayfasına veri gönderiliyor
        }


        public ActionResult UrunListele()
        {
            List<vw_tum_urunler> urunler = cu.tum_urunler_getir();
            return View(urunler);
        }

        public ActionResult UrunGuncelle(int id)
        {
            List<tbl_Categories> catList = db.tbl_Categories.ToList();
            ViewData["CategoryList"] = catList.Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });

            List<tbl_Suppliers> supList = db.tbl_Suppliers.ToList();
            ViewData["SupplierList"] = supList.Select(c => new SelectListItem { Text = c.BrandName, Value = c.SupplierID.ToString() });

            List<tbl_Status> statList = db.tbl_Status.ToList();
            ViewData["StatusList"] = statList.Select(c => new SelectListItem { Text = c.StatusName, Value = c.statusID.ToString() });

            vw_tum_urunler urn = db.vw_tum_urunler.FirstOrDefault(u => u.ProductID == id);

            return View(urn);
        }


        [HttpPost]
        public ActionResult UrunGuncelle(tbl_Products prd, HttpPostedFileBase fileuploader)
        {
            vw_tum_urunler urn = db.vw_tum_urunler.FirstOrDefault(u => u.ProductID == prd.ProductID);
            prd.AddDate = urn.AddDate;
            prd.OneCikanlar = urn.OneCikanlar;
            prd.CokSatanlar = urn.CokSatanlar;

            if (fileuploader != null)
            {
                //ürün resmini fiziksel olarak ~/Content/resimler altına kopyalamalıyım
                string path = Path.Combine(Server.MapPath("~/Content/resimler"), Path.GetFileName(fileuploader.FileName));
                fileuploader.SaveAs(path);

                //resmin ismini tabloya yazar
                prd.PhotoPath = fileuploader.FileName;
            }
            else
            {
                //veritabanndaki eski kayıtlara ulaştım
                prd.PhotoPath = urn.PhotoPath;
            }

            try
            {
                //  db.tbl_Products.Add(prd);
                db.Entry(prd).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Session["Mesaj"] = prd.ProductName + " ürünü başarıyla güncellendi.";
            }
            catch (Exception)
            {
                Session["Mesaj"] = "HATA " + prd.ProductName + " ürünü güncellenemedi.";
            }

            return RedirectToAction("UrunListele");
        }

        public ActionResult Cikis()
        {
            Session.Abandon();
            Session.Remove("email");
            return RedirectToAction("Index","Home");
        }


    }
}