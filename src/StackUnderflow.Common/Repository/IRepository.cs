using System.Collections.Generic;

namespace StackUnderflow.Common.Repository
{
    public interface IRepository<T>
    {
        T Find(object id);
        T Single(object id);
        IList<T> GetAll();
    }
}