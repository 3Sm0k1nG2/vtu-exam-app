using Microsoft.AspNetCore.Mvc;
using AppRestaurant.Data;
using AppRestaurant.Models;
using AppRestaurant.Models.Forms;
using AppRestaurant.Services;

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

        [HttpPost]
        public IActionResult Create([FromForm] DishFormDataModel formData)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return NotFound();

            new DishService().Create(formData);

            return Redirect("/menu");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return NotFound();

            DishModel? model = new DishService().GetOne(id);

            HttpContext.Session.Remove("updateFormId");

            if (model == null)
            {
                return Redirect("/menu");
            }

            HttpContext.Session.SetInt32("updateFormId", model.Id);

            return View("Update", model);
        }


        [HttpPost]
        public IActionResult Update([FromForm] DishFormDataModel formData)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return NotFound();

            int? nullTest = HttpContext.Session.GetInt32("updateFormId");

            if (nullTest == null)
                return NotFound();

            int dishId = (int)nullTest;

            HttpContext.Session.Remove("updateFormId");

            new DishService().Update(dishId, formData);

            return Redirect("/menu");
        }
    }
}
