using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DoAnWeb.Pages
{
    public class RegisterPageModel : PageModel
    {
        private readonly ILogger<RegisterPageModel> _logger;
        public RegisterPageModel(ILogger<RegisterPageModel> logger)
        {
            _logger = logger;
        }
        [BindProperty]
        public string tendangnhap { get; set; }

        [BindProperty]
        public string matkhau { get; set; }

        [BindProperty]
        public string hoten { get; set; }

        [BindProperty]
        public string sodt { get; set; }

        [BindProperty]
        public string diachi { get; set; }

        [BindProperty]
        public string email { get; set; }
        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
                // Thêm tài khoản mới
                InsertUser(tendangnhap, matkhau, hoten, sodt, diachi, email);
                return RedirectToPage("/LoginPage");

        }
        public void InsertUser(string tendangnhap, string matkhau, string hoten, string sodt, string diachi, string email)
        {
            using (SqlConnection connection = new SqlConnection(Constring.Conn))
            {
                connection.Open();
                string query = "INSERT INTO NGUOIDUNG (TENDANGNHAP, MATKHAU, HOVATEN, SDT, DIACHI, EMAIL, QUYEN_HAN) " +
                               "VALUES (@TenDangNhap, @MatKhau, @HoVaTen, @SDT, @DiaChi, @Email, @QuyenHan)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TenDangNhap", tendangnhap);
                    command.Parameters.AddWithValue("@MatKhau", matkhau);
                    command.Parameters.AddWithValue("@HoVaTen", hoten);
                    command.Parameters.AddWithValue("@SDT", sodt);
                    command.Parameters.AddWithValue("@DiaChi", diachi);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@QuyenHan", "KHACHHANG");

                    command.ExecuteNonQuery(); // Thực thi truy vấn để thêm người dùng mới
                }
            }
        }
    }
}
