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
            var dbMenu = LoadMenu();
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

            Session["MenuList"] = MenuList;

            return View(MenuList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult SignUp()
        {
            ViewBag.Message = "Sign Up";

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(UserModel model)
        {
            if (ModelState.IsValid)
            {
                CreateUser(model.FirstName, model.LastName, model.EmailAddress, model.PhoneNumber, model.Password);

                return RedirectToAction("Index");
            }

            return View();
        }
        
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

                }

                return RedirectToAction("ViewProducts");               
            }


            return View();
        }

        public ActionResult ViewProducts(string searchString)
        {
            ViewBag.Message = "Employee Sign Up";
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


            return View(products);
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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

        [HttpPost]
        public ActionResult Edit(ProductModel model, int? id)
        {
            SaveEditedProduct(id, model.ProductName, model.Description, model.ProductPrice);

            return RedirectToAction("ViewProducts");

        }

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
                    if (comparedProducts != null && comparedProducts.Count < 3)
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

        //TODO dodelat mazani z listu
        public ActionResult RemoveFromCompareList(ProductModel product, List<ProductModel> compareList)
        {
            if (compareList.Contains(product))
            {
                compareList.Remove(product);
            }

            return RedirectToAction("ShowCompareList");
        }


    }

}