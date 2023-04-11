using PagedList;
using Project.BLL.Repositories.ConcRep;
using Project.ENTITIES.Enums;
using Project.MVCUI.Models.ShoppingTools;
using Project.VM.PureVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using Project.ENTITIES.Models;
using Project.MVCUI.Models.PageVMs;
using System.Web.Management;

namespace Project.MVCUI.Controllers
{
    public class ShoppingController : Controller
    {
        OrderRepository _ordRep;
        ProductRepository _prodRep;
        CategoryRepository _catRep;
        OrderDetailRepository _ordDetRep;
        public ShoppingController()
        {
            _prodRep = new ProductRepository();
            _ordDetRep = new OrderDetailRepository();
            _catRep = new CategoryRepository();
            _ordRep = new OrderRepository();
        }

        public ActionResult ShoppingList(int? page, int? categoryID)
        {
            IPagedList<ProductVM> products = categoryID == null ? _prodRep.Where(x => x.Status != ENTITIES.Enums.DataStatus.Deleted).Select(x => new ProductVM
            {
                ID = x.ID,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                CategoryName = x.Category.CategoryName,
                ImagePath = x.ImagePath,
            }).ToPagedList(page ?? 1, 9) : _prodRep.Where(x => x.CategoryID == categoryID && x.Status != ENTITIES.Enums.DataStatus.Deleted).Select(x => new ProductVM
            {
                ID = x.ID,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                CategoryName = x.Category.CategoryName,
                ImagePath = x.ImagePath,
                CategoryID = x.CategoryID

            }).ToPagedList(page ?? 1, 9);
            PaginationVM pavm = new PaginationVM
            {
                PagedProducts = products,

                Categories = _catRep.Select(x => new CategoryVM
                {
                    ID = x.ID,
                    CategoryName = x.CategoryName,
                    Description = x.Description,
                }).ToList()
            };

            if (categoryID != null)
            {
                TempData["catID"] = categoryID;
            }

            return View(pavm);
        }

        public ActionResult AddToCart(int id)
        {
            Cart c = Session["scart"] == null ? new Cart() : Session["scart"] as Cart;

            Product add = _prodRep.Find(id);

            CartItem ci = new CartItem
            {
                ID = add.ID,
                Name = add.ProductName,
                Price = add.UnitPrice,
                ImagePath = add.ImagePath,
            };

            c.SepeteEkle(ci);
            Session["scart"] = c;
            return RedirectToAction("ShoppingList");
        }

        public ActionResult CartPage()
        {
            if (Session["scart"] != null)
            {
                Cart c = Session["scart"] as Cart;
                CartPageVM cpvm = new CartPageVM
                {
                    Cart = c,
                };
                return View(cpvm);
            }
            ViewBag.SepetBos = "Sepetinizde Ürün Bulunamamktadır";
            return RedirectToAction("ShoppingList");
        }

        public ActionResult DeleteFromCart(int id)
        {
            if (Session["scart"] != null)
            {
                Cart c = Session["scart"] as Cart;
                c.SepettenCikar(id);
                if (c.Sepetim.Count == 0)
                {
                    Session.Remove("scart");
                    TempData["sepetBos"] = "Sepetnizdeki Tüm Ürünler Çıkarılmıştır";
                    return RedirectToAction("ShoppingList");
                }
            }
            return RedirectToAction("CartPage");
        }
    }
}
