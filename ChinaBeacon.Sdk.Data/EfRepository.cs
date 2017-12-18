using System;
using System.Linq;
using System.Threading.Tasks;
using ChinaBeacon.Sdk.Core;
using ChinaBeacon.Sdk.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace ChinaBeacon.Sdk.Data
{
    /// <summary>
    /// Entity Framework repository
    /// </summary>
    public partial class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IDbContext _context;
        private DbSet<T> _entities;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public EfRepository(IDbContext context)
        {
            this._context = context;
        }

        public T GetById(object id)
        {
            return this.Entities.Find(id);
        }

        public void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.Entities.Add(entity);

            //this._context.SaveChanges();
        }

        public async Task InsertAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.Entities.Add(entity);

            //await this._context.SaveChangesAsync();

        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

           // this._context.SaveChanges();

        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            //await this._context.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.Entities.Remove(entity);
            //this._context.SaveChanges();
        }

        public async Task DeleteAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.Entities.Remove(entity);

            //await this._context.SaveChangesAsync();
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        private DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }
    }
}
