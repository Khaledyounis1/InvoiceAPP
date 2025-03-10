namespace InvoiceApp.Infrastructure.Abstracts
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T Entity);

        void Update(T Entity);
        void Save();

        IQueryable<T> GetAll();
        T GetById(int id);

        void Delete(int id);
    }
}
