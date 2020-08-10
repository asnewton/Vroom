using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vroom.AppDbContext;
using vroom.Models;

namespace vroom.Controllers
{
    [Authorize(Roles ="Admin,Executive")]
     public class MakeController : Controller
    {
        private readonly VroomDbContext _db;

        public MakeController(VroomDbContext db)
        {
            _db = db;
        }
    
        public IActionResult Index()
        {
            var model = _db.Makes.ToList();
            return View(model);
         }

        public IActionResult Create(Make model)
        {
            if (ModelState.IsValid)
            {
                _db.Makes.Add(model);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            var model = _db.Makes.Find(Id);
            if(model != null)
            {
                _db.Makes.Remove(model);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult Edit(int Id)
        {
            var model = _db.Makes.Find(Id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Make model)
        {
            if (ModelState.IsValid)
            {
                _db.Makes.Update(model);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
        }

    }
}