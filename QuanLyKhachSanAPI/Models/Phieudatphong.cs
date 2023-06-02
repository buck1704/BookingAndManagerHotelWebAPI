using System;
using System.Collections.Generic;

namespace QuanLyKhachSanAPI.Models
{
    public partial class Phieudatphong
    {
        public Phieudatphong()
        {
            Phieudichvus = new HashSet<Phieudichvu>();
        }

        public int MaDp { get; set; }
        public int IdKh { get; set; }
        public int Idphong { get; set; }
        public string PhuongThucThanhToan { get; set; } = null!;
        public DateTime NgayDen { get; set; }
        public DateTime NgayDi { get; set; }
        public DateTime NgayTt { get; set; }

        public virtual Quanlytaikhoan IdKhNavigation { get; set; } = null!;
        public virtual Chitietphong IdphongNavigation { get; set; } = null!;
        public virtual ICollection<Phieudichvu> Phieudichvus { get; set; }
    }
}
