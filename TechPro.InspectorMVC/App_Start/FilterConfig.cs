﻿using System.Web;
using System.Web.Mvc;
using TechPro.Inspector;
namespace TechPro.InspectorMVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new InspectorFilter());
        }
    }
}
