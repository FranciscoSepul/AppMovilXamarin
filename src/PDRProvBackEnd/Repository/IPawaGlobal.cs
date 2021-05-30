using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PDRProvBackEnd.Models;

namespace PDRProvBackEnd.Repository
{
    public interface IPDRGlobal<T> : IDisposable
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAllAndDisabled();
        //Task<T> GetByIdAsync(Guid id);
        Task<int> InsertAsync(T entityglobal);
        Task<int> UpdateAsync(T entityglobal);
        Task<int> DeleteAsync(Guid id);
        Contexts.PDRContext PDRContext {get;}
    }
}
