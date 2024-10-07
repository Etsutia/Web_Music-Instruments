using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DoAnWeb.Pages
{
    public class InfoModel : PageModel
    {
        [BindProperty]
        public string tendangnhap { get; set; }

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
            using (SqlConnection connection = new SqlConnection(Constring.Conn))
            {
                connection.Open();

                // Truy vấn SQL để kiểm tra thông tin đăng nhập
                string query = $"SELECT TENDANGNHAP, HOVATEN, SDT, EMAIL, DIACHI FROM NGUOIDUNG WHERE TENDANGNHAP = '{User.Identity.Name}' ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            {
                                tendangnhap = reader.GetString(0);
                                hoten = reader.GetString(1);
                                sodt = reader.GetString(2);
                                email = reader.GetString(3);
                                diachi = reader.GetString(4);
                            };
                        }
                    }

                }
            }
        }
    }
}
