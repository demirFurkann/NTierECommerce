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
using System.Net.Http;
using System.Threading.Tasks;
using Project.COMMON.Tools;

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
                CategoryName = x.Category != null ? x.Category.CategoryName : "",
                ImagePath = x.ImagePath,
                Status = x.Status.ToString(),

            }).ToPagedList(page ?? 1, 9) : _prodRep.Where(x => x.CategoryID == categoryID && x.Status != ENTITIES.Enums.DataStatus.Deleted).Select(x => new ProductVM
            {
                ID = x.ID,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                CategoryName = x.Category != null ? x.Category.CategoryName : "",
                ImagePath = x.ImagePath,
                CategoryID = x.CategoryID,
                Status = x.Status.ToString(),


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

            //return RedirectToAction("ShoppingList");

            return Redirect(Request.UrlReferrer.ToString());

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

        public ActionResult ConfirmOrder()
        {
            AppUser currentUser;
            if (Session["member"] != null)
            {
                currentUser = Session["member"] as AppUser;
            }
        
            return View();
        }
        //http://localhost:58476/api/Payment/ReceivePayment
        //PaymentRequestModel

        [HttpPost]
        public ActionResult ConfirmOrder(OrderPageVM ovm)
        {
            bool sonuc;
            Cart sepet = Session["scart"] as Cart;
            ovm.Order.TotalPrice = ovm.PaymentRM.ShoppingPrice = sepet.TotalPrice;

            #region APISection
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:58476/api/");
                Task<HttpResponseMessage> postTask = client.PostAsJsonAsync("Payment/ReceivePayment", ovm.PaymentRM);

                HttpResponseMessage result;
                try
                {
                    result = postTask.Result;
                }
                catch (Exception ex)
                {
                    TempData["baglantiRed"] = "Banka baglantıyı reddetti";
                    return RedirectToAction("ShoppingList");
                }

                if (result.IsSuccessStatusCode) sonuc = true;
                else sonuc = false;

                if (sonuc)
                {
                    if (Session["member"] != null)
                    {
                        AppUser kullanici = Session["member"] as AppUser;
                        ovm.Order.AppUserID = kullanici.ID;

                    }

                    _ordRep.Add(ovm.Order); //OrderRepository bu noktada Order'i eklerken onun ID'sini olusturur...

                    foreach (CartItem item in sepet.Sepetim)
                    {
                        OrderDetail od = new OrderDetail();
                        od.OrderID = ovm.Order.ID;
                        od.ProductID = item.ID;
                        od.TotalPrice = item.SubTotal;
                        od.Quantity = item.Amount;
                        _ordDetRep.Add(od);

                        //Stoktan da düsürelim
                        Product stoktanDusurulecek = _prodRep.Find(item.ID);
                        stoktanDusurulecek.UnitsInStock -= item.Amount;
                        _prodRep.Update(stoktanDusurulecek);

                        //Algoritma  Ödevi : Eger stoktan düsürüldügünde stokta kalmayacak bir şekilde item varsa onun Amount'ı Sepette asılamayacak bir hale gelsin
                    }

                    TempData["odeme"] = "Siparişiniz bize ulasmıstır...Tesekkür ederiz";

                    if (Session["member"] != null)
                        MailService.Send(ovm.Order.AppUser.Email, body: $"Siparişiniz basarıyla alındı{ovm.Order.TotalPrice}"); //Kullanıcıya aldıgı ürünleri de Mail yoluyla gönderin...
                    else MailService.Send(ovm.Order.NonMemberEmail, body: $"Siparişiniz basarıyla alındı{ovm.Order.TotalPrice}");

                    Session.Remove("scart");
                    return RedirectToAction("ShoppingList");
                }

                else
                {
                    Task<string> s = result.Content.ReadAsStringAsync();
                    TempData["sorun"] = s;
                    return RedirectToAction("ShoppingList");
                }

            }



            #endregion
        }

    }
}
