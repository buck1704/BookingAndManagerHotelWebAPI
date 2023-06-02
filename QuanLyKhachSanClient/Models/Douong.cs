namespace QuanLyKhachSanClient.Models
{
    public partial class DouongViewModel
    {
        
        public int Id { get; set; }
        public string TenDoUong { get; set; } = null!;
        public string Gia { get; set; } = null!;
        public string Img { get; set; } = null!;
    }

    public partial class DouongModel
    {
        public int Id { get; set; }
        public string TenDoUong { get; set; } = null!;
        public string Gia { get; set; } = null!;
        public IFormFile Img { get; set; } = null!;
    }
}
