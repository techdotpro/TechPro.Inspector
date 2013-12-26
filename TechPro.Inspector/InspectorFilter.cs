using System.IO;
using System.Reflection;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace TechPro.Inspector
{
    public sealed class InspectorFilter : ActionFilterAttribute
    {
        public Assembly SelfAssembly {
            get {
                return Assembly.GetExecutingAssembly();
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult == null) return;

            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var model = filterContext.Controller.ViewData.Model;
                filterContext.HttpContext.Response.Write(this.GetCSS());
                filterContext.HttpContext.Response.Write(this.ConstructHtml(model));
                filterContext.HttpContext.Response.Write(this.GetJS());
            }

            base.OnActionExecuted(filterContext);
        }

        private string GetCSS()
        {
            using (var rStream = SelfAssembly.GetManifestResourceStream("TechPro.Inspector.Inspector.min.css"))
            using (var reader = new StreamReader(rStream))
            {
                return string.Format("<style>{0}</style>", reader.ReadToEnd());
            }
        }

        private string GetJS()
        {
            using (var rStream = SelfAssembly.GetManifestResourceStream("TechPro.Inspector.Inspector.min.js"))
            using (var reader = new StreamReader(rStream))
            {
                return string.Format("<script type=\"text/javascript\">{0}</script>", reader.ReadToEnd());
            }
        }

        private string ConstructHtml(object model) 
        {
            var serializerSettings = new JsonSerializerSettings()
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                Formatting = Formatting.None,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatString = "yyyy/MM/dd HH:mm:ss",
                TypeNameHandling = TypeNameHandling.None,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(model, Formatting.None, serializerSettings);
            return string.Format(@"<div id='tpinspector' data-json='{0}'>
    <div>TP Inspector <a title='Fold In'>◈</a></div>
    <div></div>
</div>", json);
        }
    }
}