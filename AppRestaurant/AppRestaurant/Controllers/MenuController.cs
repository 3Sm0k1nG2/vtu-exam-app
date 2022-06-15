using Microsoft.AspNetCore.Mvc;
using AppRestaurant.Data;
using AppRestaurant.Models;
using System.Dynamic;

namespace AppRestaurant.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index([FromQuery] string query)
        {
            dynamic model = new ExpandoObject();
            DishesDb db = new DishesDb();
            List<DishModel>? models;

            if (query != null)
            {
                models = db.GetAll(query);
                model.prevQuery = query;
            }
            else
            {
                models = db.GetAll();
                model.prevQuery = "";
            }

            model.models = models;

            if (models.Count == 0)
                model.models = null;

            return View(model);
        }

        public IActionResult Details(int id)
        {
            DishesDb db = new DishesDb();
            DishModel? model = db.GetOne(id);

            if (model == null)
            {
                return Redirect("/menu");
            }

            return View(model);
        }
    }
}
