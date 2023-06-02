using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace QuanLyKhachSanClient.Models
{
    public partial class Dichvu
    {
        public Dichvu()
        {
            Chitietdichvus = new HashSet<Chitietdichvu>();
        }
        public string Id { get; set; }
        public string TenDichVu { get; set; } = null!;
        public int DonGia { get; set; }

        public virtual ICollection<Chitietdichvu> Chitietdichvus { get; set; }
    }
}
