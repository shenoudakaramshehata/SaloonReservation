using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaloonReservation.Data;
using SaloonReservation.Models;
using NToastNotify;
using Microsoft.AspNetCore.Localization;
using SaloonReservation.Migrations.Salon;
using Microsoft.EntityFrameworkCore;

namespace SaloonReservation.Areas.Admin.Pages.Configurations.MangeCountry
{
    public class IndexModel : PageModel
    {
        private SalonContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;


        public string url { get; set; }


        [BindProperty]
        public Country country { get; set; }


        public List<Country> Countries = new List<Country>();

        public Country countryObj { get; set; }

        public IndexModel(SalonContext context, IWebHostEnvironment hostEnvironment,
                                            IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;
            country = new Country();
            countryObj = new Country();
        }
        public void OnGet()
        {
            Countries = _context.Countries.ToList();
            url = $"{this.Request.Scheme}://{this.Request.Host}";
        }

        public IActionResult OnGetSingleCountryForEdit(int CountryId)
        {
            country = _context.Countries.Where(c => c.CountryId == CountryId).FirstOrDefault();

            return new JsonResult(country);
        }

        public IActionResult OnPostEditCountry(int CountryId)
        {
            try
            {
                var model = _context.Countries
                                    .Where(c => c.CountryId == CountryId)
                                    .FirstOrDefault();
                if (model == null)
                {
                    _toastNotification.AddErrorToastMessage("Country Object Not Found");

                    return Redirect("/Admin/Configurations/MangeCountry/Index");
                }


                model.CountryTlAr = country.CountryTlAr;
                model.CountryTlEn = country.CountryTlEn;
                model.CountryIsActive = country.CountryIsActive;
                model.CountryOrderIndex = country.CountryOrderIndex;
             


                var UpdatedCountry = _context.Countries.Attach(model);

                UpdatedCountry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                _context.SaveChanges();

                _toastNotification.AddSuccessToastMessage("Country Edited successfully");


            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went Error");

            }
            return Redirect("/Admin/Configurations/MangeCountry/Index");
        }
        public IActionResult OnGetSingleCountryForView(int CountryId)
        {
            var Result = _context.Countries.Where(c => c.CountryId == CountryId).Select(i => new
            {
                CountryId = i.CountryId,
                CountryTlEn = i.CountryTlEn,
                CountryTlAr = i.CountryTlAr,
                CountryOrderIndex = i.CountryOrderIndex,
                CountryIsActive = i.CountryIsActive,
       

            }).FirstOrDefault();

            return new JsonResult(Result);
        }
        public IActionResult OnGetSingleCountryForDelete(int CountryId)
        {
            var Result = _context.Countries.Where(c => c.CountryId == CountryId).Select(i => new
            {
                CountryId = i.CountryId,
                CountryTlEn = i.CountryTlEn,
                CountryTlAr = i.CountryTlAr,
                CountryOrderIndex = i.CountryOrderIndex,
                CountryIsActive = i.CountryIsActive,


            }).FirstOrDefault();

            return new JsonResult(Result);
        }
        public async Task<IActionResult> OnPostAddCountry()
        {
            
            try
            {
               
                _context.Countries.Add(country);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Country Added Successfully");
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            return Redirect("/Admin/Configurations/MangeCountry/Index");
        }
        public async Task<IActionResult> OnPostDeleteCountry(int CountryId)
        {
            try
            {
                countryObj = _context.Countries.Where(e => e.CountryId == CountryId).FirstOrDefault();
                if (country != null)
                {
                   
                        var Cities = _context.Cities.Where(e => e.CountryId == CountryId).ToList();
                        if (Cities != null)
                        {
                        var city = await _context.Cities.Where(u => u.CountryId == CountryId).ToListAsync();
                        foreach (var c in city)
                        {
                            var usersInCity = await _context.Customers.Where(u => u.CityId == c.CityId).ToListAsync();

                            foreach (var user in usersInCity)
                            {
                                user.CityId = null;
                            }
                        }
                        foreach (var C in Cities)
                        {
                            var area = await _context.Areas.Where(u => u.CityId == C.CityId).ToListAsync();
                            foreach (var a in area)
                            {
                                var usersInArea = await _context.Customers.Where(u => u.AreaId == a.AreaId).ToListAsync();

                                foreach (var user in usersInArea)
                                {
                                    user.AreaId = null;
                                }
                            }

                            _context.Areas.RemoveRange(area);

                        }
                       
                        _context.Cities.RemoveRange(Cities);
                        }
                        _context.Countries.Remove(countryObj);
                        _context.SaveChanges();

                    _toastNotification.AddSuccessToastMessage("Country Deleted Successfully");
                }

            }
            catch (Exception)

            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }

            return Redirect("/Admin/Configurations/MangeCountry/Index");
        }
    }
}
