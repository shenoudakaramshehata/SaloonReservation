
using SaloonReservation.Data;
using SaloonReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace SaloonReservation.Areas.CRM.Pages.SystemConfiguration.SocialLinks
{

    #nullable disable
    public class IndexModel : PageModel
    {
        private SalonContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;


        public string url { get; set; }


        [BindProperty]
        public SoicialMidiaLink socialMediaLink { get; set; }


        public List<SoicialMidiaLink> socialMediaLinksList = new List<SoicialMidiaLink>();
        
        public SoicialMidiaLink socialMediaObj { get; set; }

        public IndexModel(SalonContext context, IWebHostEnvironment hostEnvironment, 
                                            IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;
            socialMediaLink = new SoicialMidiaLink();
            socialMediaObj = new SoicialMidiaLink();
        }
        public void OnGet()
        {
            socialMediaLinksList = _context.SoicialMidiaLinks.ToList();
            url = $"{this.Request.Scheme}://{this.Request.Host}";
        }

        public IActionResult OnGetSingleSocialForEdit(int SocialMediaLinkId)
        {
            socialMediaLink = _context.SoicialMidiaLinks.Where(c => c.SoicialMidiaLinkId == SocialMediaLinkId).FirstOrDefault();
            
            return new JsonResult(socialMediaLink);
        }

        public IActionResult OnPostEditSocial(int SoicialMidiaLinkId)
        {
            try
            {
                var model = _context.SoicialMidiaLinks
                                    .Where(c => c.SoicialMidiaLinkId == SoicialMidiaLinkId)
                                    .FirstOrDefault();
                if (model == null)
                {
                    _toastNotification.AddErrorToastMessage("Social Object Not Found");
                 
                    return Redirect("/Admin/SystemConfiguration/SocialLinks/Index");
                }


                model.Facebook = socialMediaLink.Facebook;
                model.Twitter = socialMediaLink.Twitter;
                model.Instgram = socialMediaLink.Instgram;
                model.LinkedIn = socialMediaLink.LinkedIn;
                model.WhatsApp = socialMediaLink.WhatsApp;
                model.Youtube = socialMediaLink.Youtube;
               

                var UpdatedSocialLinks = _context.SoicialMidiaLinks.Attach(model);

                UpdatedSocialLinks.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                _context.SaveChanges();

                _toastNotification.AddSuccessToastMessage("Links Edited successfully");


            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went Error");
               
            }
            return Redirect("/Admin/SystemConfiguration/SocialLinks/Index");
        }

    }
}
