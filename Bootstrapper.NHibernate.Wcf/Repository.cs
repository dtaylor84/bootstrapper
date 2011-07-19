using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bootstrap.NHibernate.Wcf
{
    public class Repository<T> : IIntKeyedRepository<T> where T : class
    {
        public ISession Session { get; set; }

        public Repository(ISession session)
        {
            Session = session;
        }

        #region IRepository<T>

        public virtual T Add(T item)
        {
            Session.Save(item);
            return item;
        }

        public virtual IEnumerable<T> Add(IEnumerable<T> items)
        {
            return items.Select(Add).ToList();
        }

        public virtual T Update(T item)
        {
            Session.Update(item);
            return item;
        }

        public virtual IEnumerable<T> Update(IEnumerable<T> items)
        {
            return items.Select(Update).ToList();
        }

        public virtual bool Delete(T item)
        {
            Session.Delete(item);
            return true;
        }

        public virtual bool Delete(IEnumerable<T> items)
        {
            foreach (var item in items)
                Delete(item);

            return true;
        }

        public void ApplyChanges()
        {
            Session.Flush();
        }

        #endregion IRepository<T>

        #region IIntKeyedRepository<T>

        public T FindBy(int id)
        {
            return Session.Get<T>(id);
        }

        #endregion IIntKeyedRepository<T>

        #region IReadOnlyRepository<T>

        public virtual IQueryable<T> All()
        {
            return Session.Query<T>();
        }

        public T FindBy(Expression<Func<T, bool>> expression)
        {
            return FilterBy(expression).Single();
        }

        public IQueryable<T> FilterBy(Expression<Func<T, bool>> expression)
        {
            return All().Where(expression).AsQueryable();
        }

        #endregion IReadOnlyRepository<T>
    }
}