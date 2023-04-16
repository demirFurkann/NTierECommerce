using Project.BLL.Repositories.ConcRep;
using Project.ENTITIES.Models;
using Project.MVCUI.Areas.Admin.Data.AdminPageVMs;
using Project.MVCUI.Models.CustomTools;
using Project.VM.PureVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;




namespace Project.MVCUI.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        CategoryRepository _cRep;
        ProductRepository _pRep;

        public ProductController()
        {
            _pRep = new ProductRepository();
            _cRep = new CategoryRepository();
        }

        private List<AdminCategoryVM> GetCategoriesVM()
        {
            return _cRep.Select(x => new AdminCategoryVM
            {
                ID = x.ID,
                CategoryName = x.CategoryName,
                Description = x.Description

            }).ToList();
        }

        private List<AdminProductVM> GetProductsVM()
        {
            return _pRep.Select(x => new AdminProductVM
            {
                ID = x.ID,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                CategoryName = x.Category.CategoryName,
                ImagePath = x.ImagePath,

                Status = x.Status.ToString()

            }).ToList();
        }

        public ActionResult ListProducts(int? id)
        {
            List<AdminProductVM> products = GetProductsVM();
            AdminProductListPageVM apvm = new AdminProductListPageVM
            {
                Products = products,
            };
            _pRep.GetAll();
            return View(apvm);

        }
        public ActionResult AddProduct()
        {
            List<AdminCategoryVM> categories = GetCategoriesVM();
            AdminAddUpdateProductPageVM apvm = new AdminAddUpdateProductPageVM
            {
                Categories = categories,
            };

            return View(apvm);
        }
        [HttpPost]

        public ActionResult AddProduct(AdminProductVM product, HttpPostedFileBase image, string fileName)
        {
       
            Product p = new Product
            {
                ProductName = product.ProductName,
                ID = product.ID,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                CategoryID = product.CategoryID,
                ImagePath = product.ImagePath = ImageUploader.UploadImage("/Pictures/", image, fileName)
            };

            _pRep.Add(p);
            return RedirectToAction("ListProducts");
        }

        public ActionResult UpdateProduct(int id)
        {
            List<AdminCategoryVM> categories = GetCategoriesVM();
            AdminAddUpdateProductPageVM apvm = new AdminAddUpdateProductPageVM
            {
                Categories = categories,
                Product = _pRep.Where(x => x.ID == id).Select(x => new AdminProductVM
                {
                    ID = x.ID,
                    ProductName = x.ProductName,
                    UnitPrice = x.UnitPrice,
                    UnitsInStock = x.UnitsInStock,
                    CategoryID = x.CategoryID,
                    ImagePath = x.ImagePath,
                }).FirstOrDefault()
            };
            return View(apvm);
        }
        [HttpPost]

        public ActionResult UpdateProduct(AdminProductVM product, HttpPostedFileBase image, string fileName)
        {
            Product toBeUpdated = _pRep.Find(product.ID);
            toBeUpdated.ProductName = product.ProductName;
            toBeUpdated.UnitPrice = product.UnitPrice;
            toBeUpdated.UnitsInStock = product.UnitsInStock;
            toBeUpdated.CategoryID = product.CategoryID;
            toBeUpdated.ImagePath = product.ImagePath;


            if (toBeUpdated == null)
            {
                toBeUpdated.ImagePath = ImageUploader.UploadImage("/Pictures/", image, fileName);
            }



            //if (image != null)
            //{
            //	if (product.ImagePath != null)
            //	{
            //		ImageUploader.DeleteImage(product.ImagePath);
            //	}
            //	string imagePath = ImageUploader.UploadImage("/Pictures/", image, fileName);
            //	product.ImagePath = imagePath;
            //}
            //else if (removeImage != null)
            //{
            //	if (product.ImagePath != null)
            //	{
            //		ImageUploader.DeleteImage(product.ImagePath);
            //		product.ImagePath = null;
            //	}
            //}


            _pRep.Update(toBeUpdated);
            return RedirectToAction("ListProducts");
        }

        public ActionResult DeleteProduct(int id)
        {
            _pRep.Delete(_pRep.Find(id));

            return RedirectToAction("ListProducts");
        }

    }



}
