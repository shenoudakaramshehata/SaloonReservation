
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SaloonReservation.Data;
using SaloonReservation.Models;
using Microsoft.AspNetCore.Localization;

namespace SaloonReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]

    public class ItemImagesController : Controller
    {
        private SalonContext _context;

        public ItemImagesController(SalonContext context) {
            _context = context;
        }

        

        [HttpGet]
        public async Task<object> GetImagesForItem([FromQuery] int id)
        {
            var productimages = _context.BarberImages.Where(p => p.BarberId == id).Select(i => new {
                i.BarberId,
                i.pic,
                i.BarberImageId,
                i.picDescription
            });

            return productimages;
        }



        [HttpPost]
        public async Task<int> RemoveImageById([FromQuery] int id)
        {
            var itemPic = _context.BarberImages.FirstOrDefault(p => p.BarberImageId == id);
            _context.BarberImages.Remove(itemPic);
            _context.SaveChanges();

            return id;
        }

    }
}