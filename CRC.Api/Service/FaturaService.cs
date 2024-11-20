using CRC.Api.Models.Request;
using CRC.Api.Models.Response;
using CRC.Api.Repository;
using CRC.Domain.Entities;

namespace CRC.Api.Service;

public class FaturaService : IService<Fatura, FaturaRequest, FaturaResponse, FaturaListResponse>
{
    
    private FaturaRepository _repo;
    private MoradorRepository _moradorRepo;

    public FaturaService(FaturaRepository repo, MoradorRepository moradorRepo)
    {
        _repo = repo;
        _moradorRepo = moradorRepo;
    }

    
    public async Task<FaturaListResponse> GetAllAsync(int pageNumber, int pageSize)
    {
        var faturas = await _repo.GetAllAsync(pageNumber, pageSize);
        return MapToListResponse(faturas, pageNumber, pageSize);
    }

    public async Task<FaturaResponse> GetByIdAsync(int id)
    {
        var fatura = await _repo.GetByIdAsync(id);
        if (fatura == null)
        {
            return null;
        }
        return MapToResponse(fatura);
    }

    public async Task<FaturaResponse> AddAsync(FaturaRequest request)
    {
        
        var fatura = MapToEntity(request);

        var morador = await _moradorRepo.GetByIdAsync(fatura.IdMorador);
        
        decimal pontos = ((decimal) morador.QtdMoradores / fatura.QtdConsumida) * 1000;
        var pontosRedondo = (int) Math.Round(pontos);
        
        morador.Pontos += pontosRedondo;
        await _moradorRepo.UpdateAsync(morador);
        
        var addedFatura = await _repo.AddAsync(fatura);
        return MapToResponse(addedFatura);
    }

    public async Task<FaturaResponse> UpdateAsync(int id, FaturaRequest request)
    {
        var fatura = await _repo.GetByIdAsync(id);
        if (fatura == null)
        {
            return null;
        }
        fatura.DtGeracao = request.DtGeracao;
        fatura.IdMorador = request.IdMorador;
        fatura.QtdConsumida = request.QtdConsumida;
        
        var updatedFatura = await _repo.UpdateAsync(fatura);
        return MapToResponse(updatedFatura);
    }

    public async Task<FaturaResponse> DeleteAsync(int id)
    {
        var fatura = await _repo.GetByIdAsync(id);
        if (fatura == null)
        {
            return null;
        }
        await _repo.DeleteAsync(fatura);
        return MapToResponse(fatura);
    }
    
    public async Task<FaturaListResponse> GetByMoradorAsync(int idMorador, int pageNumber, int pageSize)
    {
        var faturas = await _repo.GetByMoradorAsync(idMorador, pageNumber, pageSize);
        return MapToListResponse(faturas, pageNumber, pageSize);
    }

    public FaturaResponse MapToResponse(Fatura entity)
    {
        return new FaturaResponse(
            entity.Id,
            entity.IdMorador,
            entity.QtdConsumida,
            entity.DtGeracao
        );
    }

    public FaturaListResponse MapToListResponse(IEnumerable<Fatura> entities, int pageNumber, int pageSize)
    {
        var faturaList = entities.Select(f => MapToResponse(f));
        return new FaturaListResponse( pageNumber, pageSize, faturaList.Count(), faturaList);
    }

    public  Fatura MapToEntity(FaturaRequest request)
    {
        return new Fatura {
            IdMorador = request.IdMorador,
            QtdConsumida = request.QtdConsumida,
            DtGeracao = request.DtGeracao
        };
    }
}
