using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using ListItem = System.Tuple<string, string, bool, object>;

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

        // http://jsfiddle.net/uHfLL/
        private string ConstructHtml(object model) 
        {
            var rootItems = this.ToListItems(model);
            var rootUL = this.UL(rootItems);
            rootUL.Remove(0, 4);
            rootUL = "<ul id=\"#tpinspector\">" + rootUL;
            return rootUL;
        }

        private string LI(ListItem item)
        {
            var sb = new StringBuilder("<li>");

            sb.AppendFormat("<a title=\"{0}\" href=\"#\">{1}</a>", item.Item2, item.Item1);
            if (item.Item3)
            {
                // non terminal
                sb.Append(UL(item.Item4 as IEnumerable<ListItem>));
            }
            else
            {
                //terminal
                sb.AppendFormat("<code>{0}</code>", item.Item4 as string);
            }

            sb.Append("</li>");
            return sb.ToString();
        }

        private string UL(IEnumerable<ListItem> items)
        {
            var sb = new StringBuilder("<ul>");
            foreach (var item in items) {
                sb.Append(LI(item));
            }
            sb.Append("</ul>");
            return sb.ToString();
        }

        private IEnumerable<ListItem> ToListItems(object model)
        {
            var results = new List<ListItem>();

            foreach (var property in model.GetType().GetProperties())
            {
                var propertyName = property.Name;
                var propertyType = property.PropertyType.Name;
                var propertyValue = property.GetValue(model, null);
                var isTerminal = this.IsTerminal(propertyValue);

                if (isTerminal)
                {
                    results.Add(new ListItem(propertyName, propertyType, false, TerminalString(propertyValue)));
                }
                else
                {
                    results.Add(new ListItem(propertyName, propertyType, true, ToListItems(propertyValue)));
                }
            }

            return results;
        }

        private bool IsTerminal(object o)
        {
            return
                o == null ||
                o is string ||
                o.GetType().IsValueType;
        }

        private string TerminalString(object o)
        {
            if (o == null) return "null";
            else return o.ToString();
        }
    }

    internal sealed class RowItem
    {
        public IEnumerable<RowItem> Children { get; set; }

        public string DisplayHover { get; set; }

        public string DisplayName { get; set; }

        public string DisplayValue { get; set; }
    }
}
