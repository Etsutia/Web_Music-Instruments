using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using DoAnWeb.ThanhToan;

namespace DoAnWeb.Pages
{
    public class CartModel : PageModel
    {
        public List<string> tenSanPhamList;
        public DataTable GioHang { get; set; }
        public void TimKiem1(string query)
        {
            using (SqlConnection con = new SqlConnection(Constring.Conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    GioHang = new DataTable();
                    adapter.Fill(GioHang);
                }
            }
        }

        public void GetTenSP(DataTable GioHang)
        {
            tenSanPhamList = new List<string>();

            string query = @"SELECT TENSANPHAM FROM SANPHAM WHERE MASANPHAM = @maSP";

            using (SqlConnection con = new SqlConnection(Constring.Conn))
            {
                con.Open();

                for (int i = 0; i < GioHang.Rows.Count; i++)
                {
                    string maSP = GioHang.Rows[i]["MASANPHAM"].ToString();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@maSP", maSP);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string tenSP = reader["TENSANPHAM"].ToString();
                                tenSanPhamList.Add(tenSP);
                            }
                        }
                    }
                }
            }

        }
        public double GetGia()
        {
            double totalPrice = 0;
            string query = @"SELECT SUM(GIATIEN) AS TotalPrice FROM GIOHANG";

            using (SqlConnection con = new SqlConnection(Constring.Conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Thực thi truy vấn và đọc kết quả
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        totalPrice = Convert.ToDouble(result);
                    }
                }
            }

            return totalPrice;
        }
        public double Gia { get; set; }
        public void OnGet()
        {
            if (GioHang != null)
            {
                GioHang.Clear();
            }
            string query = $@"SELECT * FROM GIOHANG WHERE TENDANGNHAP = N'{User.Identity.Name}'";
            TimKiem1(query);
            GetTenSP(GioHang);
            Gia = GetGia();
            PaymentInformationModel.Amount = Gia;
        }
    }
}
