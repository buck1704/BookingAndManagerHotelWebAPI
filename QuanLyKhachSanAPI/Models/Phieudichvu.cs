using System;
using System.Collections.Generic;

namespace QuanLyKhachSanAPI.Models
{
    public partial class Phieudichvu
    {
        public Phieudichvu()
        {
            Chitietdichvus = new HashSet<Chitietdichvu>();
        }

        public int Mpdv { get; set; }
        public int MaDp { get; set; }
        public int TongTien { get; set; }

        public virtual Phieudatphong MaDpNavigation { get; set; } = null!;
        public virtual ICollection<Chitietdichvu> Chitietdichvus { get; set; }
    }
}
