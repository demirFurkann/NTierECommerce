using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
	public class Category:BaseEntity
	{
		public Category()
		{
			Products = new List<Product>(); // Bu ifade ,MyInit class'ında henüfz EF tetiklenmeden yani işlemlerimizi saf bir sekılde Ram'de baslandıgında Bu Category Class'ının Products isimli propertys'si null gelmesin diye yapılmiştir
		}
		public string CategoryName { get; set; }

		public string Description { get; set; }

		//Relational Properties

		public virtual List<Product> Products { get; set; }

	}
}
