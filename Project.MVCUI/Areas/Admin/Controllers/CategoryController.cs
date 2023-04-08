using Project.BLL.Repositories.ConcRep;
using Project.MVCUI.Areas.Admin.Data.AdminPageVMs;
using Project.VM.PureVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        CategoryRepository _cRep;
        public CategoryController()
        {
            _cRep = new CategoryRepository();
        }
        public ActionResult ListCategories(int? id)
        {
            List<AdminCategoryVM> categories;
            if (id == null)
            {
                categories = _cRep.Select(x => new AdminCategoryVM
                {
                    ID = x.ID,
                    CategoryName = x.CategoryName,
                    Description = x.Description,
                    DeletedDate = x.DeletedDate,
                    ModifiedDate = x.ModifiedDate,
                    CreatedDate = x.CreatedDate,
                    Status = x.Status.ToString(),
                }).ToList();
                
            }
            else
            {
                categories = _cRep.Select(x => new AdminCategoryVM
                {
                    ID = x.ID,
                    CategoryName = x.CategoryName,
                    DeletedDate = x.DeletedDate,
                    CreatedDate = x.CreatedDate,
                    Description = x.Description,
                    ModifiedDate = x.ModifiedDate,
                    Status = x.Status.ToString()
                }).ToList();
            }
            AdminCategoryListPageVM aclpvm = new AdminCategoryListPageVM
            {
                Categories = categories,
            };
            return View(aclpvm);
        }
    }
}