using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SaloonReservation.Data;
using SaloonReservation.DataTables;
using SaloonReservation.Models;
using System.Linq.Dynamic.Core;

namespace SaloonReservation.Areas.Admin.Pages.Configurations.ManageServices
{
    public class IndexModel : PageModel
    {
        private SalonContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public string url { get; set; }

        [BindProperty]
        public Service Service { get; set; }
        public Service ServiceObj { get; set; }

        public IndexModel(SalonContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;
            Service = new Service();
            ServiceObj = new Service();
        }
        public void OnGet()
        {

            url = $"{this.Request.Scheme}://{this.Request.Host}";

            ViewData["selectGenderList"] = new SelectList(_context.Genders.ToList(), nameof(Gender.GenderId), nameof(Gender.GenderTLEn));


        }

        [BindProperty]
        public DataTablesRequest DataTablesRequest { get; set; }

        public async Task<JsonResult> OnPostAsync()
        {
            var recordsTotal = _context.Services.Count();

            var customersQuery = _context.Services.AsQueryable();
            var searchText = DataTablesRequest.Search?.Value?.ToUpper();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                customersQuery = customersQuery.Where(s =>
                    s.serviceTlEn.ToUpper().Contains(searchText) ||
                    s.serviceTlAr.ToUpper().Contains(searchText) ||
                    s.Duration.ToString().Contains(searchText) 
                );
            }

            var recordsFiltered = customersQuery.Count();

            var skip = DataTablesRequest.Start;
            var take = DataTablesRequest.Length;
            var data = await customersQuery
                .ToListAsync();

            return new JsonResult(new
            {
                draw = DataTablesRequest.Draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = data
            });
        }
        public IActionResult OnGetSingleServiceForEdit(int ServiceId)
        {
            Service = _context.Services.Where(c => c.ServiceId == ServiceId).Include(a => a.Gender).FirstOrDefault();

            return new JsonResult(Service);
        }

        public async Task<IActionResult> OnPostEditService(int ServiceId)
        {
            try
            {
                var model = _context.Services.Where(c => c.ServiceId == ServiceId).FirstOrDefault();
                if (model == null)
                {
                    _toastNotification.AddErrorToastMessage("Service Not Found");

                    return Redirect("/Admin/Configurations/ManageServices/Index");
                }

                model.Duration = Service.Duration;
                model.serviceTlAr = Service.serviceTlAr;
                model.serviceTlEn = Service.serviceTlEn;
                model.OneKidPrice = Service.OneKidPrice;
                model.MoreKidsPrice = Service.MoreKidsPrice;
                model.GenderId = Service.GenderId;

                var UpdatedService = _context.Services.Attach(model);

                UpdatedService.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                _context.SaveChanges();

                _toastNotification.AddSuccessToastMessage("Service Edited successfully");

                return Redirect("/Admin/Configurations/ManageServices/Index");

            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went Error");

            }
            return Redirect("/Admin/Configurations/ManageServices/Index");
        }


        public IActionResult OnGetSingleServiceForView(int ServiceId)
        {
            var Result = _context.Services.Where(c => c.ServiceId == ServiceId).Include(a => a.Gender).FirstOrDefault();
            return new JsonResult(Result);
        }


        public IActionResult OnGetSingleServiceForDelete(int ServiceId)
        {
            Service = _context.Services.Where(c => c.ServiceId == ServiceId).FirstOrDefault();
            return new JsonResult(Service);
        }

        public async Task<IActionResult> OnPostDeleteService(int ServiceId)
        {
            try
            {
                Service ServiceObj = _context.Services.Where(e => e.ServiceId == ServiceId).FirstOrDefault();


                if (ServiceObj != null)
                {


                    _context.Services.Remove(ServiceObj);

                    await _context.SaveChangesAsync();

                    _toastNotification.AddSuccessToastMessage("Service Deleted successfully");


                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong Try Again");
                }
            }
            catch (Exception)

            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }

            return Redirect("/Admin/Configurations/ManageServices/Index");
        }



    }
}
