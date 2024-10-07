using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DoAnWeb.Pages
{
    public class ThanhToanThanhCongModel : PageModel
    {
        public string ResponseCode { get; set; }
        public void OnGet([FromQuery(Name = "responseCode")] string responseCode)
        {
            ResponseCode = responseCode;

        }
        
    }
}
