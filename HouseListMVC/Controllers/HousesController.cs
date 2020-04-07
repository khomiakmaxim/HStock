using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseListMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseListMVC.Controllers
{    
    public class HousesController : Controller
    {
        [BindProperty]
        public House House { get; set; }

        private readonly ApplicationDbContext _db;

        public HousesController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Upsert(int? id)
        {   if (User.IsInRole("Buyer"))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                House = new House();
                House.AppUserId = int.Parse(User.Identity.GetUserId());
                if (id == null)
                {
                    return View(House);
                }
                House = _db.Houses.Where(p => p.AppUserId == int.Parse(User.Identity.GetUserId())).FirstOrDefault(u => u.Id == id);
                
                if (House == null)
                {
                    return NotFound();
                }
                return View(House);
            }

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (User.IsInRole("Seller"))
            {
                if (ModelState.IsValid)
                {
                    House.AppUserId = int.Parse(User.Identity.GetUserId());
                    if (House.Id == 0)
                    {
                        _db.Houses.Add(House);
                    }
                    else
                    {
                        _db.Houses.Update(House);
                    }
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(House);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        #region API Calls        
        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            if (User.IsInRole("Buyer"))
            {
                return Json(new { data = await _db.Houses.ToListAsync() });
            }
            else 
            {                       
                return Json(new { data = await _db.Houses.Where(p => p.AppUserId == int.Parse(User.Identity.GetUserId())).ToListAsync() });
            }
            
        }
                
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var houseFromDb = await _db.Houses.FirstOrDefaultAsync(u => u.Id == id);
            if (User.IsInRole("Seller") && houseFromDb.AppUserId == int.Parse(User.Identity.GetUserId()))
            {                
                if (houseFromDb == null)
                {
                    return Json(new { success = false, message = "Error while Deleting" });
                }

                _db.Houses.Remove(houseFromDb);
                await _db.SaveChangesAsync();
                return Json(new { succes = true, message = "Delete successful" });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }   
        #endregion
    }
}