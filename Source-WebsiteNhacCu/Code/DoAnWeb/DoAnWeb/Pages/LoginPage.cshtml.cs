using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Data.SqlClient;

namespace DoAnWeb.Pages
{
    public class LoginPageModel : PageModel
    {
        [BindProperty]
        public Credential credential { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            using (SqlConnection connection = new SqlConnection(Constring.Conn))
            {
                connection.Open();

                // Truy vấn SQL để kiểm tra thông tin đăng nhập
                string query = "SELECT QUYEN_HAN FROM NGUOIDUNG WHERE TENDANGNHAP = @TenDangNhap AND MATKHAU = @MatKhau";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TenDangNhap", credential.UserName);
                    command.Parameters.AddWithValue("@MatKhau", credential.Password);

                    // Thực hiện truy vấn và lấy giá trị quyền hạn từ cơ sở dữ liệu
                    object roleObject = command.ExecuteScalar();

                    if (roleObject != null)
                    {
                        string role = roleObject.ToString();
                        Console.WriteLine(role);
                        List<Claim> lst = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, credential.UserName),
                            new Claim(ClaimTypes.Name, credential.UserName),
                            new Claim(ClaimTypes.Role, role), // Thêm claim cho vai trò (role)
                        };
                        ClaimsIdentity ci = new ClaimsIdentity(lst,
                            Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme
                            );
                        ClaimsPrincipal cp = new ClaimsPrincipal(ci);
                        await HttpContext.SignInAsync(cp);
                        if (role != "QUANTRI")
                        {
                            return RedirectToPage("/HomePage");
                        }
                        else
                        {
                            return Redirect("/AdminPage");
                        }
                        
                    }
                    else
                    {
                        return RedirectToPage("/LoginPage");
                    }
                }
            }
        }
        public class Credential
        {
            [Required]
            public string UserName { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
