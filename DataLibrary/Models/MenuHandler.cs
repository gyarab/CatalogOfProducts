using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class MenuHandler
    {
        public int MenuId { get; set; }
        public string MenuText { get; set; }
        public int ParentId { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int? CategoryId { get; set; }
    }
}
