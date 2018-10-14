using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpress.Models.PageViews
{
    public class ControllerView
    {
        public ControllerView() { }


        public ControllerView(string controllerName, string homePage, string alternatePage)
        {
            ControllerName = controllerName;
            HomePage = homePage;
            AlternatePage = alternatePage;
        }

        public string ControllerName { get; set; }

        public string HomePage { get; set; }

        public string AlternatePage { get; set; }
    }
}
