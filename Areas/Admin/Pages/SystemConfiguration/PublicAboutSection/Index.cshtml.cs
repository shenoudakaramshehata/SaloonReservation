using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using SaloonReservation.Data;
using SaloonReservation.Models;

namespace SaloonReservation.Areas.Admin.Pages.SystemConfiguration.PublicAboutSection
{
    public class IndexModel : PageModel
    {
        private SalonContext _context;

        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;
        public string url { get; set; }
        [BindProperty]
        public PublicSection publicSection { get; set; }
        public List<PublicSection> publicSections = new List<PublicSection>();

        public IRequestCultureFeature locale;
        public string BrowserCulture;

        public IndexModel(SalonContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;
            publicSection = new PublicSection();
        }
        public void OnGet()
        {
            locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            BrowserCulture = locale.RequestCulture.UICulture.ToString();
            url = $"{this.Request.Scheme}://{this.Request.Host}";
            publicSections = _context.PublicSections.ToList();

        }
        public async Task<IActionResult> OnGetSinglePublicSectionForEdit(int Id)
        {
            publicSection = _context.PublicSections.Where(c => c.Id == Id).FirstOrDefault();

            return new JsonResult(publicSection);

        }
        public async Task<IActionResult> OnGetSinglePublicSectionForView(int Id)
        {
            var publicSection = _context.PublicSections.Where(c => c.Id == Id).FirstOrDefault();

            return new JsonResult(publicSection);

        }
        
        public async Task<IActionResult> OnPostEditPublicSection(int Id, IFormFile Editfile)
        {
            try
            {
                var model = _context.PublicSections.Where(c => c.Id == Id).FirstOrDefault();
                if (model == null)
                {
                    return Redirect("/Admin/SystemConfiguration/PublicAboutSection/Index");
                }


                if (Editfile != null)
                {
                   

                    string folder = "Images/PublicSlider/";
                    model.Image = await UploadImage(folder, Editfile);
                }
                else
                {
                    model.Image = publicSection.Image;
                }


                model.TitleEn = publicSection.TitleEn;
                model.TitleAr = publicSection.TitleAr;
                model.DescritpionAr = publicSection.DescritpionAr;
                model.DescritpionEn = publicSection.DescritpionEn;

                var UpdatedSection = _context.PublicSections.Attach(model);
                UpdatedSection.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Public About Section Edited successfully");


            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage("Something went Error");
            }
            return Redirect("/Admin/SystemConfiguration/PublicAboutSection/Index");

        }

        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_hostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return folderPath;
        }
    }
}