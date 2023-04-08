using Project.VM.PureVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Models.PageVMs
{
	public class RegisterNowPageVM
	{
		public AppUserVM AppUser{ get; set; }

		public AppUserProfileVM profile { get; set; }
	}
}