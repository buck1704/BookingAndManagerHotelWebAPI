using System;
using System.Collections.Generic;

namespace QuanLyKhachSanClient.Models
{
    public partial class Quanlytaikhoan
    {
        public Quanlytaikhoan()
        {
            Phieudatphongs = new HashSet<Phieudatphong>();
        }

        public int Id { get; set; }
        public string HoTen { get; set; } = null!;
        public string Sdt { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Cmnd { get; set; } = null!;
        public string PassWord { get; set; } = null!;

        public virtual ICollection<Phieudatphong> Phieudatphongs { get; set; }
    }
}
