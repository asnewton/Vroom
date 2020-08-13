using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vroom.AppDbContext;
using vroom.Controllers.Resources;
using vroom.Models;
using vroom.Models.ViewModels;

namespace vroom.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class ModelController : Controller
    {
        private readonly VroomDbContext _db;
        private readonly IMapper _mapper;

        [BindProperty]
        public ModelViewModel ModelVM { get; set; }
        public ModelController(VroomDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            ModelVM = new ModelViewModel()
            {
                Makes = _db.Makes.ToList(),
                Model = new Model()
            };
        }
       

        public IActionResult Index()
        {
            var model = _db.Models.Include(m => m.Make);
            return View(model);
        }

        public IActionResult Create()
        {
            return View(ModelVM);
        }

        [HttpPost, ActionName("Create")]
        public IActionResult CreatePost()
        {
            if (ModelState.IsValid)
            {
                _db.Models.Add(ModelVM.Model);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
            return View(ModelVM);
            }
        }

        public IActionResult Edit(int Id)
        {
            ModelVM.Model = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == Id);
            if(ModelVM.Model == null)
            {
                return NotFound();
            }
            return View(ModelVM);
        }

        [HttpPost, ActionName("Edit")]
        public IActionResult EditPost()
        {
            if (ModelState.IsValid)
            {
                _db.Models.Update(ModelVM.Model);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(ModelVM);
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Model model = _db.Models.Find(Id);
            if(model == null)
            {
                return NotFound();
            }
            _db.Models.Remove(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpGet("api/models/{MakeId}")]
        public IEnumerable<Model> Models(int MakeId)
        {
            return _db.Models.ToList().Where(m => m.MakeID == MakeId);
        }
        
        [AllowAnonymous]
        [HttpGet("api/models")]
        public IEnumerable<ModelResources> Models()
        {
            // without automapper
            var models =  _db.Models.ToList();
            //var modelResources = models.Select(m => new ModelResources()
            //{
            //    Id = m.Id,
            //    Name=m.Name
            //}).ToList();

            // with automapper
            //var config = new MapperConfiguration(mc => mc.CreateMap<Model, ModelResources>());
            //var mapper = new Mapper(config);
            //var modelResources = mapper.Map<List<Model>, List<ModelResources>>(models);

            // automapper globally
            var modelResources = _mapper.Map<List<Model>, List<ModelResources>>(models);

            return modelResources;
        }


    }
}