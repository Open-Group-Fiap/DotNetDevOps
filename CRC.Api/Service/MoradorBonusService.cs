﻿using CRC.Api.Models.Request;
using CRC.Api.Models.Response;
using CRC.Api.Repository;
using CRC.Domain.Entities;

namespace CRC.Api.Service;

public class MoradorBonusService : IService<MoradorBonus, MoradorBonusRequest, MoradorBonusResponse, MoradorBonusListResponse>
{
    private MoradorBonusRepository _repo;
    private MoradorRepository _moradorRepo;
    private BonusRepository _bonusRepo;
    
    public MoradorBonusService(MoradorBonusRepository repo, MoradorRepository moradorRepo, BonusRepository bonusRepo)
    {
        _repo = repo;
        _moradorRepo = moradorRepo;
        _bonusRepo = bonusRepo;
    }
    
    
    public async Task<MoradorBonusListResponse> GetAllAsync(int pageNumber, int pageSize)
    {
        var moradorBonus = await _repo.GetAllAsync(pageNumber, pageSize);
        return MapToListResponse(moradorBonus, pageNumber, pageSize);
    }

    public async Task<MoradorBonusResponse> GetByIdAsync(int id)
    {
        var moradorBonus = await _repo.GetByIdAsync(id);
        
        if (moradorBonus == null)
        {
            return null;
        }
        
        return MapToResponse(moradorBonus);
    }

    public async Task<MoradorBonusResponse> AddAsync(MoradorBonusRequest request)
    {
        var moradorBonus = MapToEntity(request);


        moradorBonus = await _repo.AddAsync(moradorBonus);

        var morador = await _moradorRepo.GetByIdAsync(moradorBonus.IdMorador);
        var bonus = await _bonusRepo.GetByIdAsync(moradorBonus.IdBonus);

        decimal pontos = (decimal) morador.Pontos - request.Qtd * bonus.Custo;

        morador.Pontos = (int)Math.Round(pontos);

        _moradorRepo.UpdateAsync(morador);

        moradorBonus = await _repo.GetByIdAsync(moradorBonus.Id);
        
        return MapToResponse(moradorBonus);
    }

    public async Task<MoradorBonusResponse> UpdateAsync(int id, MoradorBonusRequest request)
    {
        var moradorBonus = await _repo.GetByIdAsync(id);
        
        if (moradorBonus == null)
        {
            return null;
        }

        var qtdAntes = moradorBonus.Qtd;
        
        moradorBonus.IdMorador = request.IdMorador;
        moradorBonus.IdBonus = request.IdBonus;
        moradorBonus.Qtd = request.Qtd;
        
        await _repo.UpdateAsync(moradorBonus);

        var morador = await _moradorRepo.GetByIdAsync(moradorBonus.IdMorador);
        var bonus = await _bonusRepo.GetByIdAsync(moradorBonus.IdBonus);

        decimal pontos = ((decimal)morador.Pontos - request.Qtd * bonus.Custo) + qtdAntes*bonus.Custo;

        morador.Pontos = (int)Math.Round(pontos);

        _moradorRepo.UpdateAsync(morador);

        return MapToResponse(moradorBonus);
    }

    public async Task<MoradorBonusResponse> DeleteAsync(int id)
    {
        var moradorBonus = await _repo.GetByIdAsync(id);
        
        if (moradorBonus == null)
        {
            return null;
        }
        
        await _repo.DeleteAsync(moradorBonus);
        
        return MapToResponse(moradorBonus);
    }
    
    public async Task<IEnumerable<MoradorBonusResponse>> GetByMoradorIdAsync(int id)
    {
        var moradorBonus = await _repo.GetByMoradorIdAsync(id);
        
        return moradorBonus.Select(mb => MapToResponse(mb));
    }
    
    public async Task<IEnumerable<MoradorBonusResponse>> GetByBonusIdAsync(int id)
    {
        var moradorBonus = await _repo.GetByBonusIdAsync(id);
        
        return moradorBonus.Select(mb => MapToResponse(mb));
    }
    
    public async Task<MoradorBonusResponse> GetByMoradorIdAndBonusIdAsync(int idMorador, int idBonus)
    {
        var moradorBonus = await _repo.GetByMoradorIdAndBonusIdAsync(idMorador, idBonus);
        
        if (moradorBonus == null)
        {
            return null;
        }

        return MapToResponse(moradorBonus);
    }
    

    public MoradorBonusResponse MapToResponse(MoradorBonus entity)
    {
        return new MoradorBonusResponse(
            entity.Id,
            entity.IdMorador,
            entity.IdBonus,
            entity.Qtd
        );
    }

    public MoradorBonusListResponse MapToListResponse(IEnumerable<MoradorBonus> entities, int pageNumber, int pageSize)
    {
        var list = entities.Select(mb => MapToResponse(mb));
        
        return new MoradorBonusListResponse(
            pageSize,
            pageNumber, 
            list.Count(),
            list
        );
    }

    public MoradorBonus MapToEntity(MoradorBonusRequest request)
    {
        return new MoradorBonus
        {
            IdMorador = request.IdMorador,
            IdBonus = request.IdBonus,
            Qtd = request.Qtd
        };
    }
}