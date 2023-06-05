using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Web_eticaret.Models
{
    public class cls_siparisler
    {

        public int ProductID { get; set; }
        public int adet { get; set; }
        public string sepet { get; set; } //10=1&20=1&30=1
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public int Kdv { get; set; }
        public string PhotoPath { get; set; }
        public string tcno_vergino { get; set; }

        burak_eticaretEntities db = new burak_eticaretEntities();

        public bool sepete_ekle(string scid)
        {
            bool varmi = true;
            string[] sepetdizi = sepet.Split('&');
            //10=5  sepetdizi[0]
            //20=2  sepetdizi[1]
            //30=1  sepetdizi[2]
            for (int i = 0; i < sepetdizi.Length; i++)
            {
                string[] sepetdizi2 = sepetdizi[i].Split('=');
                //sepetdizi2[0] = 30 , sepetdizi2[1] = 1 
                if (sepetdizi2[0] == scid)
                {
                    //bu ürün sepette daha önce eklenmiştir
                    varmi = false;
                }
            }
            return varmi;
        }


        //projede g üst kösede sepetim üstüne gidince verileri getirecek olan metod
        public List<cls_siparisler> sepetiyazdir()
        {
            List<cls_siparisler> sip = new List<cls_siparisler>();
            string[] sepetdizi = sepet.Split('&');

            if (sepetdizi[0] != "")
            {
                for (int i = 0; i < sepetdizi.Length; i++)
                {
                    string[] sepetdizi2 = sepetdizi[i].Split('=');
                    int sepetid = Convert.ToInt32(sepetdizi2[0]);

                    tbl_Products prd = db.tbl_Products.FirstOrDefault(p => p.ProductID == sepetid);

                    cls_siparisler pr = new cls_siparisler();
                    pr.ProductID = prd.ProductID;
                    pr.adet = Convert.ToInt32(sepetdizi2[1]);
                    pr.Price = Convert.ToDecimal(prd.Price);
                    pr.ProductName = prd.ProductName;
                    pr.Kdv = Convert.ToInt32(prd.Kdv);
                    pr.PhotoPath = prd.PhotoPath;
                    sip.Add(pr);
                }
            }
            return sip;
        }


        //sepet sayfamda ürün sildigimde bu metod calısacak
        public void sepetten_sil(string scid)
        {
            string[] sepetdizi = sepet.Split('&');
            string yenisepet = "";
            int count = 1;

            for (int i = 0; i < sepetdizi.Length; i++)
            {
                string[] sepetdizi2 = sepetdizi[i].Split('=');
                if (count == 1)
                {
                    //ilk kayıtta & isareti olmayacak
                    if (sepetdizi2[0] != scid)
                    {
                        yenisepet += sepetdizi2[0] + "=" + Convert.ToInt32(sepetdizi2[1]);
                        count++;
                    }
                }
                else
                {
                    //count > 1
                    if (sepetdizi2[0] != scid)
                    {
                        yenisepet += "&" + sepetdizi2[0] + "=" + Convert.ToInt32(sepetdizi2[1]);
                    }
                }
            }
            sepet = yenisepet;
        }


        public string cookie_sepetini_siparis_tablosuna_yaz(string email)
        {
            List<cls_siparisler> sip = sepetiyazdir();

            string OrderGroupGUID = DateTime.Now.ToString().Replace(":", "").Replace(" ", "").Replace(".", "");

            foreach (var item in sip)
            {
                tbl_Orders ord = new tbl_Orders();
                ord.orderDate = DateTime.Now;
                ord.orderGroupGUID = OrderGroupGUID;
                ord.UserID = db.tbl_Users.FirstOrDefault(u => u.Email == email).UserID;
                ord.ProductID = item.ProductID;
                ord.quantity = item.adet;
                db.tbl_Orders.Add(ord);
                db.SaveChanges();
            }
            return OrderGroupGUID;
        }


        public List<vw_siparislerim> siparislerimi_getir(string email)
        {
            int UserID = db.tbl_Users.FirstOrDefault(u => u.Email == email).UserID;

            List<vw_siparislerim> sipler = db.vw_siparislerim.Where(s => s.UserID == UserID).ToList();
            return sipler;
        }


        public List<cls_siparisler> detayli_sorgu_urunleri_getir(string sorgu)
        {
            List<cls_siparisler> urunler = new List<cls_siparisler>();
            SqlConnection sqlcon = connection.baglanti;
            SqlCommand sqlcmd = new SqlCommand(sorgu, sqlcon);
            sqlcon.Open();
            SqlDataReader sdr = sqlcmd.ExecuteReader();
            while (sdr.Read())
            {
                cls_siparisler p = new cls_siparisler();
                p.ProductID = Convert.ToInt32(sdr["ProductID"]);
                p.ProductName = sdr["ProductName"].ToString();
                p.Price = Convert.ToDecimal(sdr["Price"]);
                p.PhotoPath = sdr["PhotoPath"].ToString();
                urunler.Add(p);
            }
            sqlcon.Close();
            return urunler;
        }


    }
}