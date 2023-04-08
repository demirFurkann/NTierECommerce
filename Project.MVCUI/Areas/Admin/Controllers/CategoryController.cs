using Project.BLL.Repositories.ConcRep;
using Project.ENTITIES.Models;
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
            if(id == null)
            {
                categories = _cRep.Select(x => new AdminCategoryVM
                {
					ID = x.ID,
					CategoryName = x.CategoryName,
					DeletedDate = x.DeletedDate,
					CreatedDate = x.CreatedDate,
					Description = x.Description,
					ModifiedDate = x.ModifiedDate,
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
                    Status = x.Status.ToString(),
                }).ToList();
            }
            AdminCategoryListPageVM aclpvm = new AdminCategoryListPageVM
            {
                Categories = categories,
            };
            return View(aclpvm);
        }

        public ActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCategory(AdminCategoryVM item)
        {
            Category c = new Category
            {
                CategoryName = item.CategoryName,
                Description = item.Description,
                ID = item.ID,
            };
            _cRep.Add(c);
            return RedirectToAction("ListCategories");
        }

        public ActionResult UpdateCategory(int id )
        {
            Category selected = _cRep.Find(id);

            AdminAddUpdateCategoryPageVM acpvm = new AdminAddUpdateCategoryPageVM
            {
                Category = new AdminCategoryVM
                {
                    ID = selected.ID,
                    CategoryName = selected.CategoryName,
                    Description = selected.Description
                }
            };
            return View(acpvm);
        }

        [HttpPost]

        public ActionResult UpdateCategory(AdminCategoryVM category)
        {
            Category toBeUpdated = _cRep.Find(category.ID);

            toBeUpdated.CategoryName = category.CategoryName;
            toBeUpdated.Description = category.Description;
            _cRep.Update(toBeUpdated);
            return RedirectToAction("ListCategories");
        }

        public ActionResult DeleteCategory(int id)
        {
            _cRep.Delete(_cRep.Find(id));
            return RedirectToAction("ListCategories");
        }
    }

}