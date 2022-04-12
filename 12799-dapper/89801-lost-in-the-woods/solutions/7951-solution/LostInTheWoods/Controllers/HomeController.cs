using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LostInTheWoods.Factories;
using LostInTheWoods.Models;

namespace LostInTheWoods.Controllers
{
    public class HomeController : Controller
    {
        private TrailFactory trailFactory;
        public HomeController(TrailFactory factoryService)
        {
            trailFactory = factoryService;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            var allTrails = trailFactory.GetTrails();
            return View(allTrails);
        }
        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            var trail = trailFactory.GetTrailById(id);
            if(trail == null)
                return RedirectToAction("Index");
            return View(trail);
        }
        [HttpGet("new")]
        public IActionResult New()
        {
            return View();
        }
        [HttpPost("create")]
        public IActionResult Create(Trail newTrail)
        {
            if(ModelState.IsValid)
            {
                trailFactory.Create(newTrail);
                return RedirectToAction("Index");
            }
            return View("New");

        }
    }
}
