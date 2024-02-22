using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NToastNotify;
using SaloonReservation.Data;
using SaloonReservation.DataTables;
using SaloonReservation.Models;
using static Azure.Core.HttpHeader;
using static System.Net.Mime.MediaTypeNames;

namespace SaloonReservation.Areas.Admin.Pages.Configurations.Barber
{
    public class IndexModel : PageModel
    {
        private SalonContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public string url { get; set; }

        [BindProperty]
        public Models.Barber barber { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;

        //public List<Event> EventList = new List<Event>();

        public Models.Barber barberObj { get; set; }

        public IndexModel(
            SalonContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment hostEnvironment,

            IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;
            barber = new Models.Barber();
            barberObj = new Models.Barber();
            _userManager = userManager;
        }

        [BindProperty]
        public DataTablesRequest DataTablesRequest { get; set; }

        public async Task<JsonResult> OnPostAsync()
        {
            var recordsTotal = _context.Barbers.Count();

            var customersQuery = _context.Barbers.AsQueryable();
            var searchText = DataTablesRequest.Search?.Value?.ToUpper();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                customersQuery = customersQuery.Where(s =>
                    s.FullName.ToUpper().Contains(searchText) ||
                    s.Phone.ToUpper().Contains(searchText)
                //s.Email.ToUpper().Contains(searchText)
                );
            }

            var recordsFiltered = customersQuery.Count();
            var data = await customersQuery.Where(e => e.IsActive == true)
                .ToListAsync();

            return new JsonResult(new
            {
                draw = DataTablesRequest.Draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = data
            });
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAddBarberItem(IFormFile file, IFormFileCollection formFiles, List<string> ImageDescription)
        {

           
            try
            {
                if (file != null)
                {
                    string folder = "Images/Barber/";
                    barber.Image = UploadImage(folder, file);
                }

                List<BarberImage> itemImagesList = new List<BarberImage>();


                if (formFiles.Count != 0)
                {
                    for (var i = 0; i < formFiles.Count; i++)
                    {
                        var item = formFiles[i];
                        var itemImageObj = new BarberImage();
                        string folder = "Images/Barber/";
                        itemImageObj.pic = UploadImage(folder, item);
                        itemImageObj.picDescription = ImageDescription != null && ImageDescription.Count > i ? ImageDescription[i] : null;

                        itemImagesList.Add(itemImageObj);


                    }
                    barber.BarberImages = itemImagesList;
                }

                _context.Barbers.Add(barber);
                var user = new ApplicationUser
                {
                    UserName = barber.Email,
                    Email = barber.Email,
                    FullName = barber.FullName,
                    IsActive = barber.IsActive,

                };
                var result = await _userManager.CreateAsync(user, barber.Password);

                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user, "Barber");
                    _context.SaveChanges();

                }
                _context.SaveChanges();

                _toastNotification.AddSuccessToastMessage("Barber Added Successfully");

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            return Redirect("/Admin/Configurations/Barber/Index");
        }

        public IActionResult OnGetSingleBarberForEdit(int BarberId)
        {
            barber = _context.Barbers
        .Include(e => e.OffWeekDay)
        .Where(c => c.BarberId == BarberId)
        .FirstOrDefault();
            return new JsonResult(barber);

        }

        public IActionResult OnGetSingleBarberForView(int BarberId)
        {
            barber = _context.Barbers
       .Include(e => e.OffWeekDay)
       .Where(c => c.BarberId == BarberId)
       .FirstOrDefault();
            return new JsonResult(barber);
        }
        public async Task<IActionResult> OnPostEditBarber(int BarberId, IFormFile Editfile, IFormFileCollection Editfilepond, List<string> picDescriptions)
        {
            

            try
            {
                var model = _context.Barbers.Where(c => c.BarberId == BarberId).FirstOrDefault();
                if (model == null)
                {
                    return Redirect("../Error");
                }

                if (Editfile != null)
                {


                    string folder = "Images/Barber/";

                    model.Image = UploadImage(folder, Editfile);
                }
                else
                {
                    model.Image = barber.Image;
                }
                // Delete the old main image, if it exists
                //var oldMainImagePath = Path.Combine(uploadFolder, model.Image);
                //if (System.IO.File.Exists(oldMainImagePath))
                //{
                //    System.IO.File.Delete(oldMainImagePath);
                //}

                var ImagesOfItemdes = _context.BarberImages.Where(i => i.BarberId == BarberId).ToList();
                for (int i = 0; i < ImagesOfItemdes.Count && i < picDescriptions.Count; i++)
                {
                    ImagesOfItemdes[i].picDescription = picDescriptions[i];
                }
                //foreach (var existingImage in ImagesOfItemdes)
                //{
                //    var matchingDescription = picDescriptions.FirstOrDefault(d => d != null && d.StartsWith(existingImage.BarberImageId.ToString()));

                //    if (matchingDescription != null)
                //    {
                //        // Extract the description part from the string
                //        existingImage.picDescription = matchingDescription.Substring(existingImage.BarberImageId.ToString().Length);
                //    }
                //}
                List<BarberImage> EditItemImagesList = new List<BarberImage>();

                if (Editfilepond.Count != 0)
                {

                    var ImagesOfItem = _context.BarberImages.Where(i => i.BarberId == BarberId).ToList();
                    //_context.RemoveRange(ImagesOfItem);

                    for (var i = 0; i < Editfilepond.Count; i++)
                    {
                        var item = Editfilepond[i];

                        var itemImageObj = new BarberImage();
                        string folder = "Images/Barber/";
                        itemImageObj.pic = UploadImage(folder, item);
                        itemImageObj.BarberId = BarberId;
                        //itemImageObj.picDescription = picDescriptions != null && picDescriptions.Count > i ? picDescriptions[i] : null;
                        EditItemImagesList.Add(itemImageObj);


                    }
                    _context.BarberImages.AddRange(EditItemImagesList);
                }



                // Update other properties of the model based on the form data
                model.IsActive = barber.IsActive;
                model.FullName = barber.FullName;
                model.Phone = barber.Phone;
                model.Description = barber.Description;
                model.OffWeekDayId = barber.OffWeekDayId;

                _context.Attach(model).State = EntityState.Modified;
                //_context.AttachRange(model.BarberImages).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                //_toastNotification.AddSuccessToastMessage("Barber Edited successfully");
            }
            catch (DbUpdateConcurrencyException)
            {
                //_toastNotification.AddErrorToastMessage("Something went wrong");
            }

            return Redirect("/admin/configurations/barber/Index");
        }

        public IActionResult OnGetSingleBarberForDelete(int BarberId)
        {
            barber = _context.Barbers.Where(c => c.BarberId == BarberId).FirstOrDefault();
            return new JsonResult(barber);

        }

        public async Task<IActionResult> OnPostDeleteBarber(Models.Barber barber)
        {
            try
            {
                Models.Barber _assetDocument = _context.Barbers.Where(e => e.BarberId == barber.BarberId).FirstOrDefault();
                var imgList = _context.BarberImages.Where(c => c.BarberId == barber.BarberId).ToList();

                var ImagePathv = Path.Combine(_hostEnvironment.WebRootPath, "/" + _assetDocument.Image);


                if (_assetDocument != null)
                {
                    if (imgList.Count > 0)
                    {
                        _context.BarberImages.RemoveRange(imgList);

                    }

                    _context.SaveChanges();
                    _context.Barbers.Remove(_assetDocument);
                    await _context.SaveChangesAsync();

                    if (System.IO.File.Exists(ImagePathv))
                    {
                        System.IO.File.Delete(ImagePathv);
                    }
                    _context.SaveChanges();

                    var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                    var BrowserCulture = locale.RequestCulture.UICulture.ToString();
                    if (BrowserCulture == "en-US")

                        _toastNotification.AddSuccessToastMessage("Item Request Deleted successfully");

                    else
                        _toastNotification.AddSuccessToastMessage("تم مسح العنصر  بنجاح");
                }


            }
            catch (Exception)

            {
                var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var BrowserCulture = locale.RequestCulture.UICulture.ToString();
                if (BrowserCulture == "en-US")
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                else
                    _toastNotification.AddErrorToastMessage("حدث شئ خاطئ");
                return Redirect("/Admin/Configurations/ManageItem/Index");

            }

            return Redirect("/Admin/Configurations/Barber/Index");
        }
        private string UploadImage(string folderPath, IFormFile file)
        {
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_hostEnvironment.WebRootPath, folderPath);

            file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return folderPath;
        }

    }
}
