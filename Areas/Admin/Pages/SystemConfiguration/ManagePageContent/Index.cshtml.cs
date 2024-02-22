using SaloonReservation.Data;
using SaloonReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace SaloonReservation.Areas.Admin.Pages.SystemConfiguration.ManagePageContent
{
    public class IndexModel : PageModel
    {

        private SalonContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;


        [BindProperty]
        public PageContent PageContent { get; set; }
        public List<PageContent> pageContents { set; get; }


        public IndexModel(SalonContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }
        public ActionResult OnGet()
        {
            pageContents = _context.PageContents.ToList();

            return Page();

        }

    }
}
