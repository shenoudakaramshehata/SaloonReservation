using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaloonReservation.Data;
using SaloonReservation.Models;
using NToastNotify;
using Microsoft.AspNetCore.Localization;
using SaloonReservation.Migrations.Salon;
using Microsoft.EntityFrameworkCore;

namespace SaloonReservation.Areas.Admin.Pages.Configurations.ManageCity
{
    public class IndexModel : PageModel
    {
        private SalonContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;


        public string url { get; set; }


        [BindProperty]
        public City City { get; set; }


        public List<Country> Countries = new List<Country>();

        public City cityObj { get; set; }

        public IndexModel(SalonContext context, IWebHostEnvironment hostEnvironment,
                                            IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;
            City = new City();
            cityObj = new City();
        }
        public void OnGet()
        {
            Countries = _context.Countries.ToList();
            url = $"{this.Request.Scheme}://{this.Request.Host}";
        }

        public IActionResult OnGetSingleCityForEdit(int CityId)
        {
            City = _context.Cities.Where(c => c.CityId == CityId).FirstOrDefault();

            return new JsonResult(City);
        }

        public IActionResult OnPostEditCity(int CityId)
        {
            try
            {
                var model = _context.Cities
                                    .Where(c => c.CityId == CityId)
                                    .FirstOrDefault();
                if (model == null)
                {
                    _toastNotification.AddErrorToastMessage("City Object Not Found");

                    return Redirect("/Admin/Configurations/ManageCity/Index");
                }


                model.CityTlAr = City.CityTlAr;
                model.CityTlEn = City.CityTlEn;
                model.CityIsActive = City.CityIsActive;
                model.CityOrderIndex = City.CityOrderIndex;
                model.CountryId = City.CountryId;



                var UpdatedCity = _context.Cities.Attach(model);

                UpdatedCity.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                _context.SaveChanges();

                _toastNotification.AddSuccessToastMessage("City Edited Successfully");


            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went Error");

            }
            return Redirect("/Admin/Configurations/ManageCity/Index");
        }
        public IActionResult OnGetSingleCityForView(int CityId)
        {
            var Result = _context.Cities.Where(c => c.CityId == CityId).Include(e=>e.Country).Select(i => new
            {
                CityId = i.CityId,
                CityTlEn = i.CityTlEn,
                CityTlAr = i.CityTlAr,
                CityOrderIndex = i.CityOrderIndex,
                CityIsActive = i.CityIsActive,
                CountryTlAr = i.Country.CountryTlAr,
                CountryTlEn = i.Country.CountryTlEn,


            }).FirstOrDefault();

            return new JsonResult(Result);
        }
        public IActionResult OnGetSingleCityForDelete(int CityId)
        {
            var Result = _context.Cities.Where(c => c.CityId == CityId).Select(i => new
            {
                CityId = i.CityId,
                CityTlEn = i.CityTlEn,
                CityTlAr = i.CityTlAr,

            }).FirstOrDefault();

            return new JsonResult(Result);
        }
        public async Task<IActionResult> OnPostAddCity()
        {

            try
            {

                _context.Cities.Add(City);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("City Added Successfully");
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            return Redirect("/Admin/Configurations/ManageCity/Index");
        }
        public async Task<IActionResult> OnPostDeleteCity(int CityId)
        {
            try
            {

                cityObj = _context.Cities.Where(e => e.CityId == CityId).FirstOrDefault();
                if (cityObj != null)
                {
                    var Areas = _context.Areas.Where(e => e.CityId == CityId).ToList();
                    if (Areas != null)
                    {
                        _context.Areas.RemoveRange(Areas);
                    }

                    var areas = _context.Areas.Where(e => e.CityId == CityId).ToList();
                    if (areas != null)
                    {
                            foreach (var a in areas)
                            {
                                var usersInArea = await _context.Customers.Where(u => u.AreaId == a.AreaId).ToListAsync();

                                foreach (var user in usersInArea)
                                {
                                    user.AreaId = null;
                                }
                            }


                        

                        _context.Areas.RemoveRange(areas);
                    }
                    _context.Cities.Remove(cityObj);
                    _context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("City Deleted Successfully");
                }

            }
            catch (Exception)

            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }

            return Redirect("/Admin/Configurations/ManageCity/Index");
        }
    }
}
