﻿using Project.VM.PureVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Areas.Admin.Data.AdminPageVMs
{
	public class AdminAddUpdateProductPageVM
	{
		public AdminProductVM Product { get; set; }

		public List<AdminCategoryVM> Categories { get; set; }
	}
}