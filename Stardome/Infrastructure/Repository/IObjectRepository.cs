using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Stardome.Infrastructure.Repository
{
    /// <summary>
    /// Repository
    /// </summary>
    public interface IObjectRepository<TObject> where TObject : class
    {
        /// <summary>
        /// Gets the object by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TObject GetById(object id);

        /// <summary>
        /// Gets the object by criteria
        /// </summary>
        /// <remarks>
        /// Will throw exception if multiple objects were found with matching criteria
        /// </remarks>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TObject SingleOrDefault(Expression<Func<TObject, bool>> criteria);

        /// <summary>
        /// Gets the first matching object by expression
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TObject FindOne(Expression<Func<TObject, bool>> criteria);

        /// <summary>
        /// Gets matching objects by criteria
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IEnumerable<TObject> Find(Expression<Func<TObject, bool>> criteria);
        IEnumerable<TObject> Find(Expression<Func<TObject, bool>> criteria, params string[] includes);

        /// <summary>
        /// Deletes the objects 
        /// </summary>
        /// <param name="obj"></param>
        void Delete(TObject obj);

        /// <summary>
        /// Deletes one or more objects by criteria
        /// </summary>
        /// <param name="criteria"></param>
        void Delete(Expression<Func<TObject, bool>> criteria);

        /// <summary>
        /// Saves (Add/Update) the object
        /// </summary>
        /// <param name="obj"></param>
        void Save(TObject obj);


        TObject Create();

    }
}