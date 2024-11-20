using CRC.Domain.Entities;

namespace CRC.Api.Repository;

public interface IMoradorBonusRepository
{
    Task<IEnumerable<MoradorBonus>> GetAllAsync(int pageNumber, int pageSize);
    Task<MoradorBonus> GetByIdAsync(int id);
    Task<MoradorBonus> AddAsync(MoradorBonus entity);
    Task<MoradorBonus> UpdateAsync(MoradorBonus entity);
    Task<MoradorBonus> DeleteAsync(MoradorBonus entity);
    Task<IEnumerable<MoradorBonus>> GetByMoradorIdAsync(int id);
    Task<IEnumerable<MoradorBonus>> GetByBonusIdAsync(int id);
    Task<MoradorBonus> GetByMoradorIdAndBonusIdAsync(int idMorador, int idBonus);
    Task<int> GetAvaliableByIdAsync(int idBonus);
}