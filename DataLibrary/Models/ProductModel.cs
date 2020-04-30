using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class ProductModel
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int ProductPrice { get; set; }
        public DateTime Date { get; set; }
        public string UrlLink { get; set; }

    }
}
