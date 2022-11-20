using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HortimexB2B.Web.Pages.Basket
{
    [Authorize]
    public class CheckoutSuccessModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
