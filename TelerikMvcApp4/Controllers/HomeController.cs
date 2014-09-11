using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace TelerikMvcApp4.Controllers
{
    public class CarViewModel
    {

        public int Id { get; set; }

        public string Name { get; set; }

        [UIHint("ClientCategory")]
        [Required]
        public string Category
        {
            get;
            set;
        }
    }







    public class HomeController : Controller
    {
        CarsDbContext db;
        public HomeController()
        {
            db = new CarsDbContext();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cars_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json((from c in db.Cars
                         select new CarViewModel
                         {
                             Id = c.Id,
                             Name = c.Name,
                             Category = c.Category.Name
                         }).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }


        public ActionResult Categories()
        {
            var l = from c in db.Categories.ToList()
                    select c.Name;

            return Json(l, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult Cars_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CarViewModel> cars)
        {
            var results = new List<CarViewModel>();
            foreach (var car in cars)
            {
                if (car != null && ModelState.IsValid)
                {
                    var newCar = new Car { Name = car.Name };
                    var category = db.Categories.Where(cat => cat.Name == car.Category).SingleOrDefault();
                    if (category == null)
                    {
                        category = new Category { Name = car.Category };
                    }
                    newCar.Category = category;
                    db.Cars.Add(newCar);
                    db.SaveChanges();
                    car.Id = newCar.Id;
                    results.Add(car);
                }
            }
            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Cars_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CarViewModel> cars)
        {
            foreach (var car in cars)
            {
                if (car != null && ModelState.IsValid)
                {
                    var dbCar = db.Cars.Where(c => c.Id == car.Id).SingleOrDefault();
                    if (dbCar != null)
                    {                        
                        var category = db.Categories.Where(cat => cat.Name == car.Category).SingleOrDefault();
                        if (category == null)
                        {
                            category = new Category { Name = car.Category };
                        }
                        dbCar.Name = car.Name;
                        dbCar.Category = category;
                    }

                }
            }
            db.SaveChanges();
            return Json(cars.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Cars_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CarViewModel> cars)
        {
            foreach (var car in cars)
            {
                var dbCar = db.Cars.Where(c => c.Id == car.Id).SingleOrDefault();
                db.Cars.Remove(dbCar);

            }
            db.SaveChanges();
            return Json(cars.ToDataSourceResult(request));
        }
    }
}
