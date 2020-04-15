using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace PeopleList.Filters
{
    public class CultureAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
           
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string cultureName;
            var cultureCookie = filterContext.HttpContext.Request.Cookies["lang"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = "ru";

            var cultures = new List<string>(ConfigurationManager.AppSettings["langs"].Split(','));
            if (!cultures.Contains(cultureName))
            {
                cultureName = "ru";
            }
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
        }
    }
}