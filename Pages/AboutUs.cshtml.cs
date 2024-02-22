using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Localization;
using SaloonReservation.Data;
using SaloonReservation.ViewModels;

namespace SaloonReservation.Pages
{
    public class AboutUsModel : PageModel
    {
        
        public IRequestCultureFeature locale;
        public string BrowserCulture;
        private SalonContext _context;


        public string ContentAr { get; set; }

        public string ContentEn { get; set; }

		public string phone { get; set; }

		public AboutUsModel(SalonContext context)
        {
            _context = context;

        }


        public void OnGet()
        {
            locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            BrowserCulture = locale.RequestCulture.UICulture.ToString();
            var pageContent = _context.PageContents.FirstOrDefault(p => p.PageContentId == 1);
            if (pageContent != null)
            {
                ContentAr = pageContent.ContentAr;
                ContentEn = pageContent.ContentEn;

            }
			phone = _context.SoicialMidiaLinks.Where(e => e.SoicialMidiaLinkId == 1).FirstOrDefault().WhatsApp;
		}
    }
}
