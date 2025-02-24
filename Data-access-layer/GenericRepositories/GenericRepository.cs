using System.Linq;
using InvoiceApi.Models;

namespace InvoiceAPI.GenericRepositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly Appdbcontext context;

        public GenericRepository(Appdbcontext context)
        {
            this.context = context;
        }

        public void Add(T Entity)
        {
            context.Set<T>().Add(Entity);
        }

        public void Delete(int id)
        {
            T Entity = context.Set<T>().Find(id);
            context.Set<T>().Remove(Entity);
        }

        public IQueryable<T> GetAll()
        {
            return context.Set<T>().AsQueryable();
        }

        public T GetById(int id)
        {
            return context.Set<T>().Find(id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(T Entity)
        {
            context.Set<T>().Update(Entity);
        }
    }
}
