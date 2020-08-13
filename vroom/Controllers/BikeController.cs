using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vroom.AppDbContext;
using vroom.Models;
using vroom.Models.ViewModels;
using System.IO;
 using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using cloudscribe.Pagination.Models;

namespace vroom.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class BikeController : Controller
    {
        private readonly VroomDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [BindProperty]
        public BikeViewModel BikeVM { get; set; }
        public BikeController(VroomDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            BikeVM = new BikeViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = _db.Models.ToList(),
                Bike = new Bike()
            };
        }         

        [AllowAnonymous]
        public IActionResult Index(string searchString, string sortOrder,int pageNumber=1, int pageSize=2)
        {
            ViewBag.currentSortOrder = sortOrder;
            ViewBag.currentSearchString = searchString;
            ViewBag.PriceSortParam = String.IsNullOrEmpty(sortOrder) ? "sord_desc" : "";
            int ExcludeRecords = (pageNumber * pageSize) - pageSize;
            var bikes = from b in _db.Bikes.Include(m => m.Make).Include(m => m.Model) select b;
            var bikesCount = bikes.Count();
            if (!String.IsNullOrEmpty(searchString))
            {
                bikes = bikes.Where(x => x.Make.Name.Contains(searchString));
                bikesCount = bikes.Count();
            }
            switch (sortOrder)
            {
                case "sord_desc":
                   bikes= bikes.OrderByDescending(x => x.Price);
                    break;
                default:
                    bikes = bikes.OrderBy(x => x.Price);
                    break;
            }
            var model = bikes.Skip(ExcludeRecords).Take(pageSize);
            var result = new PagedResult<Bike>
            {
                Data = model.AsNoTracking().ToList(),
                TotalItems = bikesCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return View(result);
        }

        public IActionResult Create()
        {
            return View(BikeVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost()
        {
            if (!ModelState.IsValid)
            {
                BikeVM.Makes = _db.Makes.ToList();
                BikeVM.Models = _db.Models.ToList();
                return View(BikeVM);
            }
            _db.Bikes.Add(BikeVM.Bike);
            _db.SaveChanges();
            // Get id of current saved obj
            var BikeId = BikeVM.Bike.Id;
            string wwwrootPath = _hostingEnvironment.WebRootPath;
            //Get uploaded file
            var files = HttpContext.Request.Form.Files;
            var SavedBike = _db.Bikes.Find(BikeId);
            if(files.Count != 0)
            {
                //make path for file
                var ImagePath = @"images\bike\";
                var Extention = Path.GetExtension(files[0].FileName);
                var RelativeImagePath = ImagePath + BikeId + Extention;
                var AbsImagePath = Path.Combine(wwwrootPath, RelativeImagePath);
                //upload file on server
                using (var fileStream = new FileStream(AbsImagePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                //set imagepath to database
                SavedBike.ImagePath = RelativeImagePath;
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int Id)
        {
            BikeVM.Bike = _db.Bikes.Include(m => m.Make).Include(m=>m.Model).SingleOrDefault(m => m.Id == Id);
            if (BikeVM.Bike == null)
            {
                return NotFound();
            }
            return View(BikeVM);
        }

        [HttpPost, ActionName("Edit")]
        public IActionResult EditPost()
        {
            if (ModelState.IsValid)
            {
                _db.Bikes.Update(BikeVM.Bike);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(BikeVM);
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Bike model = _db.Bikes.Find(Id);
            if (model == null)
            {
                return NotFound();
            }
            _db.Bikes.Remove(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


    }
}