using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;

namespace DoAnWeb.Pages
{
    public class TrongModel : PageModel
    {
        public DataTable Trong { get; set; }
        public string productID { get; set; }
        public string Gia { get; set; }
        public void TimKiem1(string query)
        {
            using (SqlConnection con = new SqlConnection(Constring.Conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    Trong = new DataTable();
                    adapter.Fill(Trong);
                }
            }
        }
        public void OnGet()
        {
            productID = "";
            productID = Request.Query["ProductID"];
            Gia = Request.Query["Gia"];
            Console.WriteLine(productID);
            if (productID != null)
            {
                using (SqlConnection con = new SqlConnection(Constring.Conn))
                {
                    con.Open();
                    string query1 = @"INSERT INTO GIOHANG (TENDANGNHAP, MASANPHAM,SOLUONG, GIATIEN) VALUES (@Tendangnhap, @Masanpham, 1, @Gia)";
                    using (SqlCommand cmd = new SqlCommand(query1, con))
                    {
                        cmd.Parameters.AddWithValue("@Tendangnhap", User.Identity.Name);
                        cmd.Parameters.AddWithValue("@Masanpham", productID);
                        cmd.Parameters.AddWithValue("@Gia", int.Parse(Gia));
                        Console.WriteLine(productID);
                        // Thực thi truy vấn SQL
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine(1);
                        }
                        else
                        {
                        }
                    }
                }

            }
            if (Trong != null)
            {
                Trong.Clear();
            }
            string query = @"SELECT * FROM SANPHAM INNER JOIN LOAISANPHAM ON SANPHAM.MALOAISANPHAM = LOAISANPHAM.MALOAISANPHAM WHERE TENLOAISANPHAM = N'Trống'";
            TimKiem1(query);
        }
    }
}
