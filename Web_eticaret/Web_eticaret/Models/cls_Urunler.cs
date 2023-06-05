using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_eticaret.Models
{
    public class cls_Urunler
    {
        burak_eticaretEntities db = new burak_eticaretEntities();

        public List<vw_urunler> urunleri_getir(string hangiurunler,int anasayfaadet,string hangisayfa,int altsayfaadet,int sayfano)
        {
            List<vw_urunler> urunler=null;

            if (hangiurunler== "slider")
            {
                 urunler = db.vw_urunler.Where(u => u.statusID == 2).OrderBy(u => u.ProductName).Take(anasayfaadet).ToList();
            }

            else if (hangiurunler== "yeni")
            {
                if (hangisayfa == "anasayfa")
                {
                    //Home index burası
                    urunler = db.vw_urunler.OrderByDescending(u => u.AddDate).Take(anasayfaadet).ToList();
                }
                else
                {
                    //Burası alt sayfa mı Hangi sayfa olduğunu bulacak
                    if (hangisayfa=="altsayfa")
                    {
                        urunler = db.vw_urunler.OrderByDescending(u => u.AddDate).Take(altsayfaadet).ToList();
                    }
                    else
                    {
                        urunler = db.vw_urunler.OrderByDescending(u => u.AddDate).Skip(sayfano * altsayfaadet).Take(altsayfaadet).ToList();
                    }
                    
                }
               
            }


            else if (hangiurunler == "indirimli")
            {
                if (hangisayfa == "anasayfa")
                {
                    //Home index burası
                    urunler = db.vw_urunler.OrderByDescending(u => u.AddDate).Take(anasayfaadet).ToList();
                }
                else
                {
                    //Burası alt sayfa mı Hangi sayfa olduğunu bulacak
                    if (hangisayfa == "altsayfa")
                    {
                        urunler = db.vw_urunler.OrderByDescending(u => u.Discount).Take(altsayfaadet).ToList();
                    }
                    else
                    {
                        urunler = db.vw_urunler.OrderByDescending(u => u.Discount).Skip(sayfano * altsayfaadet).Take(altsayfaadet).ToList();
                    }

                }

            }


            else if (hangiurunler == "Özel")
            {
                if (hangisayfa == "anasayfa")
                {
                    //Home index burası
                    urunler = db.vw_urunler.Where(u => u.statusID == 3).OrderBy(s=>s.ProductName).Take(anasayfaadet).ToList();
                }
                else
                {
                    //Burası alt sayfa mı Hangi sayfa olduğunu bulacak
                    if (hangisayfa == "altsayfa")
                    {
                        urunler = db.vw_urunler.Where(u => u.statusID == 3).OrderBy(s => s.ProductName).Take(altsayfaadet).ToList();
                    }
                    else
                    {
                        urunler = db.vw_urunler.Where(u => u.statusID == 3).OrderBy(s => s.ProductName).Skip(sayfano * altsayfaadet).Take(altsayfaadet).ToList();
                    }
                }
            }



            else if (hangiurunler == "öneçıkanlar")
            {
                if (hangisayfa == "anasayfa")
                {
                    //Home index burası
                    urunler = db.vw_urunler.OrderByDescending(u => u.OneCikanlar).Take(anasayfaadet).ToList();
                }
                else
                {
                    //Burası alt sayfa mı Hangi sayfa olduğunu bulacak
                    if (hangisayfa == "altsayfa")
                    {
                        urunler = db.vw_urunler.OrderByDescending(u => u.OneCikanlar).Take(altsayfaadet).ToList();
                    }
                    else
                    {
                        urunler = db.vw_urunler.OrderByDescending(u => u.OneCikanlar).Skip(sayfano * altsayfaadet).Take(altsayfaadet).ToList();
                    }
                }
            }


            else if (hangiurunler == "yıldız")
            {
                urunler = db.vw_urunler.Where(u => u.statusID == 4).Take(anasayfaadet).ToList();
            }

            else if (hangiurunler == "fırsat")
            {
                urunler = db.vw_urunler.Where(u => u.statusID == 5).Take(anasayfaadet).ToList();
            }

           

            else if (hangiurunler == "çoksatanlar")
            {
                urunler = db.vw_urunler.OrderByDescending(u => u.CokSatanlar).Take(anasayfaadet).ToList();
            }


            else
            {
                //VAR
            }

            return urunler; 
        }

           public static vw_urunler gunun_urununu_getir()
        {
            using(burak_eticaretEntities db2 = new burak_eticaretEntities()) //using yazılma nedeni static olması Metod içinde yazılır.
            {
                vw_urunler urun = db2.vw_urunler.FirstOrDefault(s=>s.statusID == 1);
                return urun;
            }
        }



        public static void one_cikan_degerini_arttir(int id)
        {
            using (burak_eticaretEntities db2 = new burak_eticaretEntities())
            {
                tbl_Products prd = db2.tbl_Products.FirstOrDefault(p => p.ProductID == id);
                prd.OneCikanlar = prd.OneCikanlar + 1;
                db2.SaveChanges();
            }
        }


        public static vw_urun_detaylari urun_detayini_getir(int? id)
        {
            using (burak_eticaretEntities db = new burak_eticaretEntities())
            {
                vw_urun_detaylari urunBilgisi = db.vw_urun_detaylari.FirstOrDefault(g => g.ProductID == id);
                return urunBilgisi;
            }
        }


        public static List<vw_aktif_urunler> buna_bakan_getir(vw_urun_detaylari urun, string hangiurunler)
        {
            using (burak_eticaretEntities db = new burak_eticaretEntities())
            {
                List<vw_aktif_urunler> bunabaktilar = null;

                if (hangiurunler == "bunabakan")
                {
                    bunabaktilar = db.vw_aktif_urunler.Where(b => b.BunaBakanlar == urun.BunaBakanlar && b.ProductID != urun.ProductID).ToList();
                }
                else if (hangiurunler == "aynikategori")
                {
                    bunabaktilar = db.vw_aktif_urunler.Where(b => b.CategoryID == urun.CategoryID && b.ProductID != urun.ProductID).OrderBy(b => b.ProductName).Take(4).ToList();
                }
                else
                {
                    bunabaktilar = db.vw_aktif_urunler.Where(b => b.SupplierID == urun.SupplierID && b.ProductID != urun.ProductID).OrderBy(b => b.ProductName).Take(4).ToList();
                }
                return bunabaktilar;
            }
        }



        public static List<vw_arama> arama_getir(string id)
        {
            List<vw_arama> arama = new List<vw_arama>();
            using (burak_eticaretEntities db2 = new burak_eticaretEntities())
            {
                arama = db2.vw_arama.Where(p => p.ARAMAISMI.Contains(id)).ToList();
                return arama;
            }
        }


        public List<vw_tum_urunler> tum_urunler_getir()
        {
            List<vw_tum_urunler> urunler = db.vw_tum_urunler.OrderByDescending(p => p.Active).ThenBy(p => p.ProductName).ToList();
            return urunler;
        }


        public static List<tbl_Messages> mesaj_getir(int? ProductID)
        {
            using (burak_eticaretEntities db = new burak_eticaretEntities())
            {
                List<tbl_Messages> msg = db.tbl_Messages.Where(u => u.ProductID == ProductID).OrderByDescending(u => u.MesajID).ToList();
                return msg;
            }
        }

        public static bool mesajekle(tbl_Messages msg)
        {
            using (burak_eticaretEntities db = new burak_eticaretEntities())
            {
                try
                {
                    db.tbl_Messages.Add(msg);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

    }
}