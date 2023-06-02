using System;
using System.Collections.Generic;

namespace QuanLyKhachSanAPI.Models
{
    public partial class Chitietphong
    {
        public Chitietphong()
        {
            Phieudatphongs = new HashSet<Phieudatphong>();
        }

        public int Id { get; set; }
        public int? IdPhong { get; set; }
        public string TenPhong { get; set; } = null!;
        public int NguoiMax { get; set; }
        public string LoaiGiuong { get; set; } = null!;
        public int GiaPhong { get; set; }
        public int TinhTrang { get; set; }
        public string? Img { get; set; }
        public string DienTich { get; set; } = null!;
        public string TamNhin { get; set; } = null!;
        public string Mota { get; set; } = null!;

        public virtual Phong? IdPhongNavigation { get; set; }
        public virtual ICollection<Phieudatphong> Phieudatphongs { get; set; }
    }
}
