using Microsoft.AspNetCore.Mvc;
using AppRestaurant.Data;
using AppRestaurant.Models;
using AppRestaurant.Models.Forms;

namespace AppRestaurant.Controllers.Menu
{
    public class DishController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return NotFound();

            DishModel? model = new DishModel();

            return View("Create", model);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return NotFound();

            DishesDb db = new DishesDb();
            DishModel? model = db.GetOne(id);

            if (model == null)
            {
                return Redirect("/menu");
            }

            return View("Update", model);
        }

        [HttpPost]
        public IActionResult Create([FromForm] DishFormDataModel formData)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return NotFound();

            DishesDb db = new DishesDb();
            DishModel model = new DishModel(formData);
            db.Create(model);

            return Redirect("/menu");
        }

        [HttpPost]
        public IActionResult Update([FromForm] DishFormDataModel formData)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return NotFound();

            DishesDb db = new DishesDb();
            DishModel model = new DishModel(formData);
            db.Update(formData.Id, model);

            return Redirect("/menu");
        }
    }
}
