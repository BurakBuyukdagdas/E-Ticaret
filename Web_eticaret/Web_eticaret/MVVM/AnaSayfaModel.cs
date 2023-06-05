using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using Web_eticaret.Models;

namespace Web_eticaret.MVVM
{
    public class AnaSayfaModel
    {

        public List<vw_urunler> Sliderurunler { get; set; }
        public vw_urunler gununurunu { get; set; }
        public List<vw_urunler> yeniurunler { get; set; }
        public List<vw_urunler> ozelurunler { get; set; }
        public List<vw_urunler> yildizliurunler { get; set; }
        public List<vw_urunler> firsaturunler { get; set; }
        public List<vw_urunler> onecikanlar { get; set; }
        public List<vw_urunler> coksatanlar { get; set; }
        public List<vw_urunler> indirimliurunler { get; set; }

        public List<vw_aktif_urunler> bunabaktilar { get; set; }
        public vw_urun_detaylari urun { get; set; }
        public List<vw_aktif_urunler> aynikategorili_urunler { get; set; }
        public List<vw_aktif_urunler> aynimarkali_urunler { get; set; }
        public List<tbl_Messages> mesajlar { get; set; }

    }
}