using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Models.CustomTools
{
	public static class ImageUploader
	{
		public static string UploadImage(string serverPath, HttpPostedFileBase file,string name)
		{
			if(file != null)
			{
				Guid uniqueName = Guid.NewGuid();

				string[] fileArray = file.FileName.Split('.');

				string extension = fileArray[fileArray.Length -1].ToLower();

				string fileName = $"{uniqueName}.{name}.{extension}";

				if(extension == "jpg" || extension == "jpeg" || extension == "gif" || extension == "png")
				{
					if (File.Exists(HttpContext.Current.Server.MapPath(serverPath + fileName)))
					{
						return "1";
					}
					else
					{
						string filePath = HttpContext.Current.Server.MapPath(serverPath + fileName);
						file.SaveAs(filePath);
						return $"{serverPath} {fileName}";
					}
				}
				else
				{
					return "2";
				}
			}
			return "3";
		}

		public static void DeleteImage(string imagePath)
		{
			if(File.Exists(imagePath))
			{
				File.Delete(imagePath);
			}
		}
	}
}