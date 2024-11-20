namespace CRC.Api.Service;

public interface IService<T, Request, Response, ListResponse> where T : class where Request : class where Response : class where ListResponse : class
{
    
    Task<ListResponse> GetAllAsync(int pageNumber, int pageSize);
    
    Task<Response> GetByIdAsync(int id);
    
    Task<Response> AddAsync(Request request);
    
    Task<Response> UpdateAsync(int id, Request request);
    
    Task<Response> DeleteAsync(int id);
    
    Response MapToResponse(T entity);
    
    ListResponse MapToListResponse(IEnumerable<T> entities, int pageNumber, int pageSize);
    
    T MapToEntity(Request request);
    
}