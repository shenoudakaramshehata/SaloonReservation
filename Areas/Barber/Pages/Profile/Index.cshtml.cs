using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using SaloonReservation.Data;
using SaloonReservation.Models;
using Microsoft.AspNetCore.Identity;
using SaloonReservation.ViewModels;

namespace SaloonReservation.Areas.Barber.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private SalonContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ProfileVm profileVm { get; set; }
        public IndexModel(SalonContext context, ApplicationDbContext db, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;
            _signInManager = signInManager;
            _userManager = userManager;
            profileVm = new ProfileVm();
            _db = db;
        }
        public async Task<IActionResult> OnGet()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Login");
            }
            profileVm.FullName = user.FullName;
            profileVm.Mobile = user.PhoneNumber;
            profileVm.Pic = user.UserPic;
            profileVm.Email = user.Email;
            profileVm.UserId = user.Id;

            return Page();
        }
        public async Task<IActionResult> OnGetSingleUserForEdit()
        {
            var user = await _userManager.GetUserAsync(User);
            return new JsonResult(user);
        }
        public async Task<IActionResult> OnPostEditUserProfile(IFormFile Editfile)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Redirect("/Login");
                }


                if (Editfile != null)
                {
                    string folder = "Images/Profile/";
                    user.UserPic = UploadImage(folder, Editfile);
                }
                else
                {
                    user.UserPic = profileVm.Pic;
                }
                user.PhoneNumber = profileVm.Mobile;
                user.FullName = profileVm.FullName;
                var UpdatedUser = _db.Users.Attach(user);
                UpdatedUser.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                var barber=_context.Barbers.Where(e => e.Email == user.Email).FirstOrDefault();
                barber.Phone = profileVm.Mobile;
                var UpdatedBarber = _context.Barbers.Attach(barber);
                UpdatedBarber.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.SaveChanges();
                _context.SaveChanges();
                
                _toastNotification.AddSuccessToastMessage("User Edited Successfully");


            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went Error");

            }
            return Redirect("/Barber/Profile/Index");
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
