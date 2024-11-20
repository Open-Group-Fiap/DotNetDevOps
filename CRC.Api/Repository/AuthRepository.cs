using CRC.Data;
using CRC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRC.Api.Repository;

public class AuthRepository
{
    private readonly CrcDbContext _context;
    private readonly DbSet<Auth> _db;
    
    public AuthRepository(CrcDbContext db)
    {
        this._context = db;
        this._db = db.Set<Auth>();
    }
    
   public async Task<Auth> GetAuth(Auth entity)
   {
       return _db.FirstOrDefault(x => x.Email == entity.Email && x.HashSenha == entity.HashSenha);
   }
   
   public async Task<Auth> AddAsync(Auth entity)
   {
       await _db.AddAsync(entity);
       await _context.SaveChangesAsync();
       return entity;
   }
   
   public async Task<Auth> UpdateAsync(Auth entity)
   {
       _db.Update(entity);
       await _context.SaveChangesAsync();
       return entity;
   }
   
    public async Task<Auth> GetByIdAsync(int id)
    {
         return await _db.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<Auth> DeleteAsync(Auth entity)
    {
        _db.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}