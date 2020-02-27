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
        public static int CreateUser(string firstName, string lastName, string emailAddress, int phoneNumber, string password)
        {
            UserModel data = new UserModel
            {
               
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                PhoneNumber = phoneNumber,
                Password = password
            };

            string sql = @"insert into dbo.Users(FirstName, LastName, EmailAddress, PhoneNumber, Password)
                            values (@FirstName, @LastName, @EmailAddress, @PhoneNumber, @Password);";

            return SqlDataAccess.SaveData(sql, data);
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

       
        public static int SaveEditedProduct(int? id, string productName, string description, int price)
        {
            ProductModel data = new ProductModel
            {   
                ProductId = id,
                ProductName = productName,
                Description = description,
                ProductPrice = price,

            };

            string sql = @"update dbo.Products set ProductName=@ProductName, Description=@Description,
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
