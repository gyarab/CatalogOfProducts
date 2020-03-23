using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CatalogOfProducts2.Models
{
    public partial class MenuHandler 
    {
        public int MenuId { get; set; }
        public string MenuText { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool IsChecked { get; set; }
        public List<MenuHandler> Menus { get; set; }
        public int? CategoryId { get; set; }


    }
}