namespace TechPro.InspectorMVC.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using TechPro.InspectorMVC.Models;
    using AttributeRouting.Web.Mvc;
    using System;

    [HandleError]
    public class HomeController : Controller
    {
        public static ICollection<PersonModel> Database = new List<PersonModel>();

        #region GET:/

        [GET("/")]
        public ActionResult Index(int pg = 1)
        {
            ViewBag.Total = Database.Count;
            var model = Database
                .OrderBy(person => person.Name.Split(' ')[1])       // order by surname
                .Skip((pg - 1) * 10)
                .Take(10);

            return View("List", model);
        }

        #endregion

        #region GET:/{id}/

        [GET("/{uuid:int}", AppendTrailingSlash = true)]
        public ActionResult Details(int uuid)
        {
            var model = Database.Single(person => person.UUID == uuid);
            return View("Details", model);
        }

        #endregion

        #region GET:/{id}/edit

        [GET("/{uuid:int}/edit/")]
        public ActionResult Edit(int uuid)
        {
            var model = Database.SingleOrDefault(p => p.UUID == uuid);
            if (model == null) {
                return new HttpNotFoundResult();
            }
            else
            {
                return View("Edit", model);
            }
        }

        #endregion

    }
}