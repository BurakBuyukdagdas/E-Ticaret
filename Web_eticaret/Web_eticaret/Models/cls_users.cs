using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Web_eticaret.Models
{
    public class cls_users
    {
        public static bool uyeekle(tbl_Users usr)
        {
            using(burak_eticaretEntities db = new burak_eticaretEntities())
            {
                try
                {
                    usr.Active = true;
                    usr.IsAdmin = false;
                    usr.Password=MD5Sifrele(usr.Password);
                    db.tbl_Users.Add(usr);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
        }

        public static string MD5Sifrele(string deger)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] btr = Encoding.UTF8.GetBytes(deger);
            btr = md5.ComputeHash(btr);

            StringBuilder sb = new StringBuilder();

            foreach (byte item in btr)
            {
                sb.Append(item.ToString("x2").ToLower());
            }

            return sb.ToString();
        }


        public static string uyekontrol(tbl_Users usr)
        {
            using (burak_eticaretEntities db = new burak_eticaretEntities())
            {
                string cevap = "";
                try
                {
                    string md5sifre = MD5Sifrele(usr.Password);
                    tbl_Users us = db.tbl_Users.FirstOrDefault(u=>u.Email == usr.Email && u.Password == md5sifre);
                    if (us==null)
                    {
                        cevap = "Yanlış"; // Böyle bir kullanıcı Yok
                    }
                    else
                    //Kullanıcı Admin mi yoksa müşteri mi kontrol edip panele yönlendirir. 
                    {
                        if(us.IsAdmin==true)
                        {
                            cevap = "admin";
                        }
                        else
                        {
                            cevap = us.Email;
                        }
                    }
                }
                catch (Exception)
                {

                    return "HATA";
                }
                return cevap;
            }
        }


        public static tbl_Users uye_bilgileri_getir(string email)
        {
            using (burak_eticaretEntities db = new burak_eticaretEntities())
            {
                tbl_Users usr = db.tbl_Users.FirstOrDefault(u => u.Email == email);
                return usr;
            }
        }








    }
}