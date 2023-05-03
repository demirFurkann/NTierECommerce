﻿ using Project.BLL.DesignPatterns.SingletonPattern;
using Project.BLL.Repositories.IntRep;
using Project.DAL.Context;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Repositories.BaseRep
{
	public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
	{
		MyContext _db;
		public BaseRepository()
		{
			_db = DBTool.DbInstance;
		}
		void Save()
		{
			_db.SaveChanges();
		}
		public void Add(T item)
		{
			_db.Set<T>().Add(item);
			Save();
		}

		public void AddRange(List<T> list)
		{
			_db.Set<T>().AddRange(list);
			Save();
		}

		public bool Any(Expression<Func<T, bool>> exp)
		{
			return _db.Set<T>().Any(exp);
		}

		public void Delete(T item)
		{
			item.Status = ENTITIES.Enums.DataStatus.Deleted;
			item.DeletedDate = DateTime.Now;
			Save();
		}

		public void DeleteRange(List<T> list)
		{
			foreach (T item in list) Delete(item);
			Save();
	
		}

		public void DestroyRange(List<T> list)
		{
			foreach (T item in list) Delete(item);
			Save();
		}

		public void Destroy(T item)
		{
			_db.Set<T>().Remove(item);
			Save();
		}

		public T Find(int id)
		{
			return _db.Set<T>().Find(id);
		}

		public T FirstOrDefault(Expression<Func<T, bool>> exp)
		{
			return _db.Set<T>().FirstOrDefault(exp);
		}

		public List<T> GetActives()
		{
			return Where(x => x.Status != ENTITIES.Enums.DataStatus.Deleted);
		}

		public List<T> GetAll()
		{
			return _db.Set<T>().ToList();
		}

		public List<T> GetFirstDatas(int number)
		{
			return _db.Set<T>().OrderBy(x => x.CreatedDate).Take(number).ToList();
		}

		public List<T> GetLastDatas(int number)
		{
			return _db.Set<T>().OrderByDescending(x => x.CreatedDate).Take(number).ToList();
		}

		public List<T> GetModifeds()
		{
			return Where(x => x.Status == ENTITIES.Enums.DataStatus.Updated);
		}

		public List<T> GetPassives()
		{
			return Where(x => x.Status == ENTITIES.Enums.DataStatus.Deleted);
		}

		public object Select(Expression<Func<T, object>> exp)
		{
			return _db.Set<T>().Select(exp).ToList();
		}

		public IQueryable<X> Select<X>(Expression<Func<T, X>> exp)
		{
			return _db.Set<T>().Select(exp);
		}

		public void Update(T item)
		{
			item.Status = ENTITIES.Enums.DataStatus.Updated;
			item.ModifiedDate = DateTime.Now;
			T tobeUpdated = Find(item.ID);
			_db.Entry(tobeUpdated).CurrentValues.SetValues(item);
			Save();
		}

		public void UpdateRange(List<T> list)
		{
			foreach(T item in list) Update(item);
			Save();
		}

		public List<T> Where(Expression<Func<T, bool>> exp)
		{
			return _db.Set<T>().Where(exp).ToList();
		}
	}
}
