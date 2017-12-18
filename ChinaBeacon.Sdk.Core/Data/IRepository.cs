using System.Linq;
using System.Threading.Tasks;

namespace ChinaBeacon.Sdk.Core.Data
{
    /// <summary>
    /// Repository
    /// </summary>
    public partial interface IRepository<T> where T : BaseEntity
    {
        T GetById(object id);
        void Insert(T entity);
        Task InsertAsync(T entity);
        void Update(T entity);
        Task UpdateAsync(T entity);
        void Delete(T entity);
        Task DeleteAsync(T entity);
        IQueryable<T> Table { get; }
    }
}
