using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongo.Repos
{
    public interface IRepository<E>
    {
        Task<E> Add(E item);

        Task<bool> Delete(Guid id);

        Task<bool> Edit(E item);

        Task<IEnumerable<E>> GetAll();

        Task<E> GetById(Guid id);
    }
}
