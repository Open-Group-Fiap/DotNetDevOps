using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRC.Api.Repository;

public interface IRepository<T> where T : class
{
    // generic method to get all records
    Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize);
    
    // generic method to get a record by id
    Task<T>  GetByIdAsync(int id);
    
    // generic method to add a record
    Task<T> AddAsync(T entity);
    
    // generic method to update a record
    Task<T> UpdateAsync(T entity);
    
    // generic method to delete a record
    Task<T> DeleteAsync(T entity);
    
}