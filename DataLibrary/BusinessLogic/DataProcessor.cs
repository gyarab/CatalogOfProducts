using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public class DataProcessor
    {   
        public static List<MenuHandler>LoadMenu()
        {
            string sql =@"select* from dbo.Menu";

            return SqlDataAccess.LoadData<MenuHandler>(sql);
        }       

        public static List<ProductModel> LoadProducts()
       {
            string sql = @"select Id_Product as ProductId, ProductName, CategoryId, Description, ProductPrice from dbo.Products";
           
           return SqlDataAccess.LoadData<ProductModel>(sql);
       }

        public static ProductModel LoadProduct(int? id)
        {
            string sql = @"select * from dbo.Products where Id_Product = @Id";
            return SqlDataAccess.LoadSingleDataById<ProductModel>(sql, id);
        }
        
        public static ImageModel LoadImage(int? id)
        {
            string sql =@"select * from dbo.ProductImages where Id_Product = @Id";
            return SqlDataAccess.LoadSingleDataById<ImageModel>(sql, id);
        }

        public static List<ProductCategoryModel> LoadCategories()
        {
            string sql = @"select CategoryId, CategoryName from dbo.Category";
            return SqlDataAccess.LoadData<ProductCategoryModel>(sql);
        }

        public static ProductCategoryModel LoadCategory(int? id)
        {
            string sql = @"select * from dbo.Category where CategoryId = @Id";
            return SqlDataAccess.LoadSingleDataById<ProductCategoryModel>(sql, id);
        }

        public static List<ProductModel> LoadLastNProducts(int n)
        {
            string sql = @"select top "+n+ " Id_Product as ProductId, ProductName from dbo.Products order by Id_Product desc";
            return SqlDataAccess.LoadData<ProductModel>(sql);
        }

        public static List<ProductModel> LoadProductsByCategory(int? id)
        {
            string sql = @"select  Id_Product as ProductId, ProductName, CategoryId, Description, ProductPrice from dbo.Products where CategoryId =" + id;
            return SqlDataAccess.LoadData<ProductModel>(sql);
        }

        public static int CreateProduct(string productName, int categoryId, string description, int price,DateTime date)
        {
            ProductModel data = new ProductModel
            {
                ProductName = productName,
                CategoryId = categoryId,
                Description = description,
                ProductPrice = price,               
                Date = date
            };

            string sql = @"insert into dbo.Products(ProductName, CategoryId, Description, ProductPrice, Published)
                            values (@ProductName, @CategoryId, @Description, @ProductPrice, @Date); SELECT CAST(SCOPE_IDENTITY() as int)";

            return SqlDataAccess.SaveDataGiveID(sql, data);
        }

        public static int CreateCategory(string categoryName)
        {
            ProductCategoryModel data = new ProductCategoryModel
            {
                CategoryName = categoryName
            };

            string sql = @"insert into dbo.Category(CategoryName) values (@CategoryName); SELECT CAST(SCOPE_IDENTITY() as int)";

            return SqlDataAccess.SaveDataGiveID(sql, data);
        }

        public static int CreateCategoryMenuRow(string menuText,int? categoryId )
        {
            string actionName = "ShowProductsByCategory";
            int parentId = 1;
            string controllerName = "Home";

            MenuHandler data = new MenuHandler
            {
                MenuText = menuText,
                ParentId = parentId,
                ControllerName = controllerName,
                ActionName = actionName,
                CategoryId = categoryId

            };

            string sql = @"insert into dbo.Menu(MenuText, ParentId, ControllerName, ActionName, CategoryId)
                            values (@MenuText, @ParentId, @ControllerName, @ActionName, @CategoryId)";
            return SqlDataAccess.SaveData<MenuHandler>(sql,data);
        }

        public static int SaveImagePath(string imagePath, int productId)
        {
            
            ImageModel data = new ImageModel
            {
              ImagePath = imagePath,
              ProductId = productId  
            };

            string sql = @"insert into dbo.ProductImages(ImagePath,Id_product)
                            values (@ImagePath,@ProductId);";

            return SqlDataAccess.SaveData(sql, data);
        }

       
        public static int SaveEditedProduct(int? id, string productName,int categoryId, string description, int price)
        {
            ProductModel data = new ProductModel
            {   
                ProductId = id,
                ProductName = productName,
                CategoryId = categoryId,
                Description = description,
                ProductPrice = price,

            };

            string sql = @"update dbo.Products set ProductName=@ProductName,CategoryId=@CategoryId, Description=@Description,
            ProductPrice=@ProductPrice where Id_Product=@ProductId ";
            return SqlDataAccess.SaveData(sql, data);
        }

        public static int SaveNewCategoryName(string categoryName)
        {
            ProductCategoryModel data = new ProductCategoryModel
            {
                CategoryName=categoryName
            };

            string sql = @"update dbo.Category CategoryName = @CategoryName";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int DeleteProduct(int id)
        {
            string sql = "delete from dbo.Products where Id_Product = "+id;

            return SqlDataAccess.DeleteData(sql);
        }

        public static int DeleteImage(int id)
        {
            string sql = "delete from dbo.ProductImages where Id_Product = " + id;

            return SqlDataAccess.DeleteData(sql);
        }

        public static int DeleteCategory(int id)
        {
            string sql = "delete from dbo.Category where CategoryId = " + id;

            return SqlDataAccess.DeleteData(sql);
        }
        
    }
}
