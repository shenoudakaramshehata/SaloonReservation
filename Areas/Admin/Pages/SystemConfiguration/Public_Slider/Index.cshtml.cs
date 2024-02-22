using SaloonReservation.Data;
using SaloonReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Identity;

namespace SaloonReservation.Areas.Admin.Pages.SystemConfiguration.Public_Slider
{
    public class IndexModel : PageModel
    {
        private SalonContext _context;

        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;
        public string url { get; set; }
        [BindProperty]
        public PublicSlider publicSlider { get; set; }
        public List<PublicSlider> publicSliders  = new List<PublicSlider>();

        public IRequestCultureFeature locale;
        public string BrowserCulture;

        public IndexModel(SalonContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;
            publicSlider = new PublicSlider();
        }
        public void OnGet()
        {
            locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            BrowserCulture = locale.RequestCulture.UICulture.ToString();
            url = $"{this.Request.Scheme}://{this.Request.Host}";
            publicSliders = _context.PublicSliders.ToList();

        }
        public async Task<IActionResult> OnGetSinglePublicSliderForEdit(int PublicSliderId)
        {
            publicSlider = _context.PublicSliders.Where(c => c.PublicSliderId == PublicSliderId).FirstOrDefault();

            return new JsonResult(publicSlider);

        }
        public async Task<IActionResult> OnPostEditPublicSlider(int PublicSliderId, IFormFile Editfile)
        {
            try
            {
                var model = _context.PublicSliders.Where(c => c.PublicSliderId == PublicSliderId).FirstOrDefault();
                if (model == null)
                {
                    return Redirect("/Admin/SystemConfiguration/Public_Slider/Index");
                }


                if (Editfile != null)
                {
                    //if (model.Background != null)
                    //{
                    //    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, model.Background);
                    //    if (System.IO.File.Exists(ImagePath))
                    //    {
                    //        System.IO.File.Delete(ImagePath);
                    //    }
                    //}

                    string folder = "Images/PublicSlider/";
                    model.Background = await UploadImage(folder, Editfile);
                }
                else
                {
                    model.Background = publicSlider.Background;
                }


                model.IsImage = publicSlider.IsImage;
                model.Title1Ar = publicSlider.Title1Ar;
                model.Title1En = publicSlider.Title1En;
                model.Title2Ar = publicSlider.Title2Ar;
                model.Title2En = publicSlider.Title2En;
                model.DescriptionAr = publicSlider.DescriptionAr;
                model.DescriptionEn = publicSlider.DescriptionEn;

                var UpdatedBanner = _context.PublicSliders.Attach(model);
                UpdatedBanner.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("public Slider Edited successfully");


            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage("Something went Error");
                return Redirect("/Admin/SystemConfiguration/Public_Slider/Index");
            }
            return Redirect("/Admin/SystemConfiguration/Public_Slider/Index");

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
