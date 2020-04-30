using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CatalogOfProducts2.Models
{
    public class ProductModel
    {
        public int? ProductId { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Product name required.")]
        public string ProductName { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description required.")]
        public string Description { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category required.")]
        public string Category { get; set; }

        [Display(Name = "Price")]
        
        [Required(ErrorMessage = "Price required.")]
        public int ProductPrice { get; set; }

        [Display(Name = "Image")]    
        public HttpPostedFileBase File { get; set; }
        [Display(Name = "Link")]
        public string UrlLink { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }
        public SelectList CategoryList { get; set; }

    }
}