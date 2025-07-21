using System.Linq.Expressions;

namespace Yaban.Web.Application.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);

    // Sorgunun sızdığı kısım burası. IQueryable<T> dönüyoruz.
    IQueryable<T> GetAll();

    // Where koşulunu doğrudan repository'de uygulamak için bir alternatif
    IQueryable<T> Where(Expression<Func<T, bool>> expression);

    Task AddAsync(T entity);

    void Update(T entity);

    void Remove(T entity);
}
