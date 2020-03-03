using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CatalogOfProducts2.Models;
using DataLibrary;
using DataLibrary.Validation;
using static DataLibrary.BusinessLogic.DataProcessor;

namespace CatalogOfProducts2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var lastNProduct = LoadLastNProducts(3);
            List<ProductModel> lastNlist = new List<ProductModel>();

            foreach (var row in lastNProduct)
            {
                var extractedImage = LoadImage(row.ProductId);
                lastNlist.Add(new ProductModel
                {
                    ProductId = row.ProductId,
                    ProductName = row.ProductName,
                    ImagePath = extractedImage.ImagePath
                }
                );
            }

           /* var dbMenu = LoadMenu();
            List<MenuHandler> MenuList = new List<MenuHandler>();
            foreach (var row in dbMenu)
            {
                MenuList.Add(new MenuHandler
                {
                    MenuId = row.MenuId,
                    MenuText = row.MenuText,
                    ParentId = row.ParentId,
                    ControllerName = row.ControllerName,
                    ActionName = row.ActionName

                });

            }

            Session["MenuList"] = MenuList;*/

            return View(lastNlist);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult AddProduct()
        {
            
            var categories = LoadCategories();
            List<ProductModel> categoryList = new List<ProductModel>();

            foreach(var row in categories)
            {
                categoryList.Add(new ProductModel{ 
                    CategoryId = row.CategoryId,
                    Category = row.CategoryName
                   }
                );
            }
            ViewBag.list = categoryList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize]
        public ActionResult AddProduct(ProductModel model, HttpPostedFileBase file)
        {       
            if (ModelState.IsValid)
            {
                ValidateFileAttribute fileValidator = new ValidateFileAttribute();
                if (fileValidator.IsValid(file))
                {   
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/") + file.FileName);
                    var filePath = file.FileName;
                    DateTime date = DateTime.Now;
                    date.ToString("dd/MM/yyyy");

                    int CategoryId = Int32.Parse(model.Category);
                    int productId = CreateProduct(model.ProductName,CategoryId, model.Description, model.ProductPrice, date);

                    SaveImagePath(filePath, productId);
                    ModelState.Clear();
                }
                
                return RedirectToAction("ViewProducts");
            }
            else { 
                return View();
            }

        }

        public ActionResult AddCategories()
        {
            var extractedCategories = LoadCategories();
            List<ProductCategoryModel> listOfCategories = new List<ProductCategoryModel>();

            foreach(var row in extractedCategories)
            {
                listOfCategories.Add(new ProductCategoryModel
                {   
                    CategoryId = row.CategoryId,
                    CategoryName = row.CategoryName
                });
            }

            ViewBag.ListOfCategories = listOfCategories;

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public ActionResult AddCategories(ProductCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                CreateCategory(model.CategoryName);
                ModelState.Clear();
            }
            else
            {
                return View();
            }

            return View();
        }
        

        public ActionResult ViewProducts(string searchString, string sortOrder)
        {
            ViewBag.ProductName = String.IsNullOrEmpty(sortOrder) ? "productName_desc" : "";
            ViewBag.Category = sortOrder == "Category" ? "category_asc" : "Category";
            ViewBag.Price = sortOrder == "Price" ? "price_asc" : "Price";
            var data = LoadProducts();

            List<ProductModel> products = new List<ProductModel>();
            
            foreach (var row in data)
            {
                var extractedImage = LoadImage(row.ProductId);
                var extractedCategory = LoadCategory(row.CategoryId);
                if (extractedImage == null)
                {
                    break;
                }
                else
                {
                    products.Add(new ProductModel
                    {
                        ProductId = row.ProductId,
                        ProductName = row.ProductName,
                        Category = extractedCategory.CategoryName,
                        Description = row.Description,
                        ProductPrice = row.ProductPrice,
                        ImagePath = extractedImage.ImagePath

                    });
                }

            }
            if (!String.IsNullOrEmpty(searchString))
            {
                var answer = products.Where(s => s.ProductName.Contains(searchString)
                                       || s.Category.Contains(searchString));

                return View(answer.ToList());
            }
            
            switch (sortOrder)
            {
                case "productName_desc":
                    var answer1 =products.OrderByDescending(s => s.ProductName);
                    return View(answer1.ToList());    
                    

                case "category_asc":
                    var answer2 = products.OrderBy(s => s.Category);
                    return View(answer2.ToList());

                case "price_asc":
                    var answer3 = products.OrderBy(s => s.ProductPrice);
                    return View(answer3.ToList());

                default:
                   var answer = products.OrderBy(s => s.ProductName);
                    return View(answer.ToList());
            }

        }

        public ActionResult ProductDetails()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ProductDetails(int? id)
        {
            if (id == null)
            {
               return  RedirectToAction("Index");
            }

            var extractedData = LoadProduct(id);
            var extractedImage = LoadImage(id);
            var extractedCategory = LoadCategory(extractedData.CategoryId);

            ProductModel product = new ProductModel
            {

                ProductId = id,
                ProductName = extractedData.ProductName,
                Category = extractedCategory.CategoryName,
                Description = extractedData.Description,
                ProductPrice = extractedData.ProductPrice,
                ImagePath = extractedImage.ImagePath

            };

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            var extractedData = LoadProduct(id);
            var extractedCategory = LoadCategory(extractedData.CategoryId);
            ProductModel product = new ProductModel
            {
                ProductId = id,
                ProductName = extractedData.ProductName,
                Category = extractedCategory.CategoryName,
                Description = extractedData.Description,
                ProductPrice = extractedData.ProductPrice

            };

            return View(product);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit(ProductModel model, int? id)
        {
            SaveEditedProduct(id, model.ProductName, model.Description, model.ProductPrice);

            return RedirectToAction("ViewProducts");

        }
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var extractedData = LoadProduct(id);
            var extractedCategory = LoadCategory(extractedData.CategoryId);
            ProductModel product = new ProductModel
            {
                ProductId = id,
                ProductName = extractedData.ProductName,
                Category = extractedCategory.CategoryName,
                Description = extractedData.Description,
                ProductPrice = extractedData.ProductPrice

            };

            return View(product);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {

            string fileName = LoadImage(id).ImagePath;
            System.IO.File.Delete(Request.PhysicalApplicationPath + "Images/" + fileName);

            DeleteImage(id);

            DeleteProduct(id);

            return RedirectToAction("ViewProducts");
        }


        public ActionResult AddToCompareList(int? id)
        {
            if (id == null)
            {
                RedirectToAction("Index");
            }
            else
            {
                var extractedData = LoadProduct(id);
                ProductModel product = new ProductModel
                {
                    ProductId = id,
                    ProductName = extractedData.ProductName,
                    Description = extractedData.Description,
                    ProductPrice = extractedData.ProductPrice

                };

                List<ProductModel> comparedProducts = null;

                if (TempData["CompareList"] == null)
                {
                    comparedProducts = new List<ProductModel>();
                    comparedProducts.Add(product);
                }
                else
                {
                    comparedProducts = (List<ProductModel>)TempData["CompareList"];
                    if (comparedProducts != null && comparedProducts.Count < 3 && comparedProducts.Contains(product)==false)
                    {
                        comparedProducts.Add(product);
                        
                    }
                }
                TempData["CompareList"] = comparedProducts;
                TempData.Keep();

                return RedirectToAction("ProductDetails/" + id);
            }

            return RedirectToAction("Index");

        }

        public ActionResult ShowCompareList()
        {
            
            List<ProductModel> comparedProducts = TempData["CompareList"] as List<ProductModel>;
            TempData.Keep();    
            if (ModelState.IsValid)
            {
                return View(comparedProducts);
            }      
            return View();
        }

       
        public ActionResult RemoveFromCompareList(int? id)
        {
            List<ProductModel> comparedProducts = TempData["CompareList"] as List<ProductModel>;
            ProductModel removedProduct = comparedProducts.Find(x => x.ProductId == id);


            if (comparedProducts != null&&comparedProducts.Count>0)
            {
                comparedProducts.Remove(removedProduct);
                
            }
            return RedirectToAction("ShowCompareList");

        }

    }

}