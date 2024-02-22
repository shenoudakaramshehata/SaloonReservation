using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Localization;
using SaloonReservation.Data;

namespace SaloonReservation.Pages
{
    public class TermsModel : PageModel
    {
		private readonly SalonContext _context;
		public IRequestCultureFeature locale;
		public string BrowserCulture;
		public string pageTitleEn { get; set; }
		public string pageTitleAr { get; set; }
		public string ContentAr { get; set; }

		public string ContentEn { get; set; }
		public TermsModel(SalonContext context)
		{
			_context = context;
		}

		public void OnGet()
		{
			locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
			BrowserCulture = locale.RequestCulture.UICulture.ToString();
			var pageContent = _context.PageContents.FirstOrDefault(p => p.PageContentId == 3);
			if (pageContent != null)
			{
				ContentAr = pageContent.ContentAr;
				ContentEn = pageContent.ContentEn;
				pageTitleAr = pageContent.PageTitleAr;
				pageTitleEn = pageContent.PageTitleEn;
			}
		}
	}
}
