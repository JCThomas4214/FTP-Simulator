using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects.DataClasses;
using System.Linq;
using Stardome.Infrastructure.Repository;
using System.Linq.Expressions;

namespace Stardome.Repositories
{
    public abstract class BaseContentRepository<TObject> : IObjectRepository<TObject> where TObject : class
    {
        private readonly DbContext context;

        public DbContext ObjectContext { get { return context; } }

        protected BaseContentRepository(DbContext ctx)
        {
            context = ctx;
        }

        public abstract TObject GetById(object id);

        public abstract DbSet<TObject> GetObjectSet();

        public virtual TObject Create()
        {
            throw new NotImplementedException();
        }

        public virtual TType CreateObject<TType>() where TType : EntityObject
        {
            throw new NotImplementedException();
        }

        public TObject SingleOrDefault(Expression<Func<TObject, bool>> criteria)
        {
            return GetObjectSet().SingleOrDefault(criteria);
        }

        public TObject FindOne(Expression<Func<TObject, bool>> criteria)
        {
            return GetObjectSet().FirstOrDefault(criteria);
        }

        public IEnumerable<TObject> Find(Expression<Func<TObject, bool>> criteria)
        {
            return GetObjectSet().Where(criteria);
        }

        public IEnumerable<TObject> Find(Expression<Func<TObject, bool>> criteria, params string[] includes)
        {
            DbSet<TObject> objects = GetObjectSet();
            foreach (string include in includes)
            {
                objects.Include(include);
            }
            return objects.Where(criteria);
        }

        public void Delete(TObject obj)
        {
            if (obj != null)
            {
                GetObjectSet().Remove(obj);
                context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException("obj");
            }

        }

        public void Delete(Expression<Func<TObject, bool>> criteria)
        {
            var recordsToDelete = Find(criteria);
            foreach (var item in recordsToDelete)
            {
                Delete(item);
            }
        }

        public void Save(TObject obj)
        {
            context.SaveChanges();
        }

        public bool Any(Expression<Func<TObject, bool>> criteria)
        {
            return GetObjectSet().Any(criteria);
        }

        public virtual void Audit(TObject obj, bool added)
        {

        }
    }
}