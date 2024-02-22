using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaloonReservation.Data;
using SaloonReservation.Models;
using NToastNotify;
using Microsoft.AspNetCore.Localization;
using SaloonReservation.Migrations.Salon;
using Microsoft.EntityFrameworkCore;

namespace SaloonReservation.Areas.Admin.Pages.Configurations.ManageArea
{
    public class IndexModel : PageModel
    {
        private SalonContext _context;
        private ApplicationDbContext _appContext;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;


        public string url { get; set; }


        [BindProperty]
        public SaloonReservation.Models.Area Newarea { get; set; }


        public List<Country> Countries = new List<Country>();
        public List<City> Cities = new List<City>();

        public Area areaObj { get; set; }

        public IndexModel(SalonContext context, IWebHostEnvironment hostEnvironment,
                                            IToastNotification toastNotification, ApplicationDbContext appContext)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;
            Newarea = new Area();
            areaObj = new Area();
            _appContext = appContext;
        }
        public void OnGet()
        {
            Countries = _context.Countries.ToList();
            Cities = _context.Cities.ToList();
            url = $"{this.Request.Scheme}://{this.Request.Host}";
        }

        public IActionResult OnGetSingleAreaForEdit(int AreaId)
        {
            Newarea = _context.Areas.Where(c => c.AreaId == AreaId).FirstOrDefault();

            return new JsonResult(Newarea);
        }

        public IActionResult OnPostEditArea(int AreaId)
        {
            try
            {
                var model = _context.Areas.Where(c => c.AreaId == AreaId) .FirstOrDefault();
                if (model == null)
                {
                    _toastNotification.AddErrorToastMessage("Area Object Not Found");

                    return Redirect("/Admin/Configurations/ManageArea/Index");
                }


                model.AreaTlAr = Newarea.AreaTlAr;
                model.AreaTlEn = Newarea.AreaTlEn;
                model.AreaIsActive = Newarea.AreaIsActive;
                model.AreaOrderIndex = Newarea.AreaOrderIndex;
                model.CityId = Newarea.CityId;



                var UpdatedArea = _context.Areas.Attach(model);

                UpdatedArea.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                _context.SaveChanges();

                _toastNotification.AddSuccessToastMessage("Area Edited Successfully");


            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went Error");

            }
            return Redirect("/Admin/Configurations/ManageArea/Index");
        }
        public IActionResult OnGetSingleAreaForView(int AreaId)
        {
            var Result = _context.Areas.Include(e => e.City).ThenInclude(e => e.Country).Where(c => c.AreaId == AreaId).Select(i => new
            {
                AreaId = i.AreaId,
                AreaIsActive = i.AreaIsActive,
                AreaOrderIndex = i.AreaOrderIndex,
                AreaTlAr = i.AreaTlAr,
                AreaTlEn = i.AreaTlEn,
                CityTlAr = i.City.CityTlAr,
                CityTlEn = i.City.CityTlEn,
                CountryTlAr = i.City.Country.CountryTlAr,
                CountryTlEn = i.City.Country.CountryTlEn,


            }).FirstOrDefault();

            return new JsonResult(Result);
        }
        public IActionResult OnGetSingleAreaForDelete(int AreaId)
        {
            var Result = _context.Areas.Where(c => c.AreaId == AreaId).Select(i => new
            {
                AreaId = i.AreaId,


            }).FirstOrDefault();

            return new JsonResult(Result);
        }
        public async Task<IActionResult> OnPostAddArea()
        {

            try
            {

                _context.Areas.Add(Newarea);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Area Added Successfully");
            }
            catch (Exception e)
            {

                //_toastNotification.AddErrorToastMessage("Something went wrong");
                _toastNotification.AddErrorToastMessage(e.Message);
            }
            return Redirect("/Admin/Configurations/ManageArea/Index");
        }
        public async Task<IActionResult> OnPostDeleteArea(int AreaId)
        {
            try
            {
                areaObj = _context.Areas.Where(e => e.AreaId == AreaId).FirstOrDefault();
                if (areaObj != null)
                {
                    var usersInArea = await _context.Customers.Where(u => u.AreaId == AreaId).ToListAsync();

                    foreach (var user in usersInArea)
                    {
                        user.AreaId = null;
                    }
                    _context.Areas.Remove(areaObj);

                    _context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Area Deleted Successfully");
                }

            }
            catch (Exception)

            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }

            return Redirect("/Admin/Configurations/ManageArea/Index");
        }
    }
}
