using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Web_eticaret.Models
{
    public class connection
    {

        public static SqlConnection baglanti
        {
            //("Server=.;Database=NORTHWND;User Id=sa;Password=as;") login şifre var
            //("Server=.;Database=NORTHWND;Trusted_Connection=True;"); login şifre yok
            get
            {
                SqlConnection sqlcon = new SqlConnection("Server=.;Database=burak_eticaret;Trusted_Connection=True;");
                return sqlcon;
            }
        }

    }
}