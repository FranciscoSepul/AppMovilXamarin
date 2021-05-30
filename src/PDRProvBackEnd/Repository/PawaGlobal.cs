using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PDRProvBackEnd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PDRProvBackEnd.Contexts;

namespace PDRProvBackEnd.Repository
{
    public class PDRGlobal<T> : IPDRGlobal<T> where T : class,IEntityGlobal
    {
        protected PDRContext context;
        public PDRContext PDRContext
        {
            get
            {
                return context;
            }
        }

        public PDRContext PawaContext => throw new NotImplementedException();

        private DbSet<T> entities;
         public PDRGlobal(PDRContext context)
        {
            this.context = context;
            entities = this.context.Set<T>();
        }
        public IQueryable<T> GetAllAndDisabled()
        {
            return entities;
        }
        public IQueryable<T> GetAll()
        {
            return GetAll().Where<T>(w=>w.Disabled == false);
        }
        //public async Task<T> GetByIdAsync(Guid id)
        //{
        //    return await GetAll().AsNoTracking().SingleOrDefaultAsync(s => s.id == id);
        //}
        public async Task<int> InsertAsync(T entityglobal)
        {
            if (entityglobal == null) throw new ArgumentNullException("entityglobal");

            await entities.AddAsync(entityglobal);
            return await context.SaveChangesAsync();
        }
        public async Task<int> UpdateAsync(T entityglobal)
        {
            if (entityglobal == null) throw new ArgumentNullException("entityglobal");
            entities.Update(entityglobal);
            return await context.SaveChangesAsync();
        }
        public async Task<int> DeleteAsync(Guid id)
        {
            T entityglobal = entities.SingleOrDefault(s => s.Id == id);
            entities.Remove(entityglobal);
            return await context.SaveChangesAsync();
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                    context = null;
                }
            }
        }
        #endregion
    }
}
