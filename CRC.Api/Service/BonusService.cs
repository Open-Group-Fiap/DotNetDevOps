using CRC.Api.Models.Request;
using CRC.Api.Models.Response;
using CRC.Api.Repository;
using CRC.Api.Service;
using CRC.Domain.Entities;

namespace CRC.Api.Resource;

public class BonusService : IService<Bonus, BonusRequest,BonusResponse, BonusListResponse>
{
    
    private BonusRepository _repo;
    private MoradorBonusRepository _moradorBonusRepository;

    public BonusService(BonusRepository repo, MoradorBonusRepository moradorBonusRepository)
    {
        _repo = repo;
        _moradorBonusRepository = moradorBonusRepository;
    }
    
    public async Task<BonusListResponse> GetAllAsync(int pageNumber, int pageSize)
    {
        var bonus = await _repo.GetAllAsync(pageNumber, pageSize);
        return MapToListResponse(bonus, pageNumber, pageSize);
    }

    public async Task<BonusResponse> GetByIdAsync(int id)
    {
        var bonus = await _repo.GetByIdAsync(id);
        
        if (bonus == null)
        {
            return null;
        }
        
        return MapToResponse(bonus);
    }

    public async Task<BonusResponse> AddAsync(BonusRequest request)
    {
        var bonus = MapToEntity(request);
        bonus = await _repo.AddAsync(bonus);
        
        bonus = await _repo.GetByIdAsync(bonus.Id);
        
        return MapToResponse(bonus);
    }

    public async Task<BonusResponse> UpdateAsync(int id, BonusRequest request)
    {
        var bonus = await _repo.GetByIdAsync(id);
        
        if (bonus == null)
        {
            return null;
        }
        
        bonus.Nome = request.Nome;
        bonus.Descricao = request.Descricao;
        bonus.Custo = request.Custo;
        bonus.QtdMax = request.QtdMax;
        
        bonus = await _repo.UpdateAsync(bonus);
        
        return MapToResponse(bonus);
    }

    public async Task<BonusResponse> DeleteAsync(int id)
    {
        var bonus = await _repo.GetByIdAsync(id);
        
        if (bonus == null)
        {
            return null;
        }
        
        bonus = await _repo.DeleteAsync(bonus);
        
        return MapToResponse(bonus);
    }
    
    public async Task<BonusListResponse> GetByCondominioIdAsync(int idCondominio, int pageNumber, int pageSize)
    {
        var bonus = await _repo.GetByCondominioIdAsync(idCondominio);
        return MapToListResponse(bonus, pageNumber, pageSize);
    }
    
    public async Task<IEnumerable<BonusCalculateResponse>> GetAvaliableByCondominioIdAsync(int idCondominio)
    {
        var bonus = await _repo.GetByCondominioIdAsync(idCondominio);
        var bonusResponse = new List<BonusCalculateResponse>();
        
        for (int i = 0; i < bonus.Count(); i++)
        {
            var MoradorBonusQtd = await _moradorBonusRepository.GetAvaliableByIdAsync(bonus.ElementAt(i).Id);
            var qtd = bonus.ElementAt(i).QtdMax -  MoradorBonusQtd;
            bonusResponse.Add(MapToCalculateResponse(bonus.ElementAt(i), qtd));
        }
        
        return bonusResponse;
    }
    
    public async Task<BonusCalculateResponse> GetAvaliableBonusAsync(int idBonus)
    {
        var bonus = await _repo.GetByIdAsync(idBonus);
        
        if (bonus == null)
        {
            return null;
        }
        
        var MoradorBonusQtd = await _moradorBonusRepository.GetAvaliableByIdAsync(idBonus);
        var qtd = bonus.QtdMax -  MoradorBonusQtd;
        
        return MapToCalculateResponse(bonus, qtd);
    }
    
    public BonusCalculateResponse MapToCalculateResponse(Bonus entity, int qtd)
    {
        return new BonusCalculateResponse(
            entity.Id,
            entity.Nome,
            entity.Descricao,
            entity.Custo,
            qtd
        );
    }

    public BonusResponse MapToResponse(Bonus entity)
    {
        return new BonusResponse(
            entity.Id,
            new CondominioResponse(
                entity.Condominio.Id,
                entity.Condominio.Nome,
                entity.Condominio.Endereco
            ),
            entity.Nome,
            entity.Descricao,
            entity.Custo,
            entity.QtdMax
        );
    }

    public BonusListResponse MapToListResponse(IEnumerable<Bonus> entities, int pageNumber, int pageSize)
    {
        var bonusList = entities.Select(b => MapToResponse(b));
        
        return new BonusListResponse(
            pageNumber,
            pageSize,
            bonusList.Count(),
            bonusList
        );
    }

    public Bonus MapToEntity(BonusRequest request)
    {
        return new Bonus{
            IdCondominio = request.IdCondominio,
            Nome = request.Nome,
            Descricao = request.Descricao,
            Custo = request.Custo,
            QtdMax = request.QtdMax
        };
    }
}