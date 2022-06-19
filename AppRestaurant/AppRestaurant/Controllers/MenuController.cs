using Microsoft.AspNetCore.Mvc;
using AppRestaurant.Data;
using AppRestaurant.Models;
using System.Dynamic;
using AppRestaurant.Services;

namespace AppRestaurant.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index([FromQuery] string query)
        {
            dynamic model = new ExpandoObject();
            DishService dishService = new DishService();
            List<DishModel>? models;

            if (query != null)
            {
                models = dishService.GetAll(query);
                model.prevQuery = query;
            }
            else
            {
                models = dishService.GetAll();
                model.prevQuery = "";
            }

            model.models = models;

            if (models.Count == 0)
                model.models = null;

            return View(model);
        }

        public IActionResult Details(int id)
        {
            DishModel? model = new DishService().GetOne(id);

            if (model == null)
            {
                return Redirect("/menu");
            }

            return View(model);
        }
    }
}
