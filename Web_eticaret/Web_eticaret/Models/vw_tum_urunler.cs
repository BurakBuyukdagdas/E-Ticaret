//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web_eticaret.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vw_tum_urunler
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<long> Stok { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<int> statusID { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
        public string Keywords { get; set; }
        public Nullable<int> Kdv { get; set; }
        public Nullable<int> OneCikanlar { get; set; }
        public Nullable<int> CokSatanlar { get; set; }
        public Nullable<int> BunaBakanlar { get; set; }
        public string Notes { get; set; }
        public string PhotoPath { get; set; }
        public Nullable<bool> Active { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
    }
}
