using CRC.Api.Models.Request;
using CRC.Api.Models.Response;
using CRC.Api.Repository;
using CRC.Api.Utils;
using CRC.Data;
using CRC.Domain.Entities;

namespace CRC.Api.Service;

public class MoradorService : IService<Morador, MoradorRequest, MoradorResponse, MoradorListResponse>
{
    private MoradorRepository _repo;
    
    private AuthRepository _authRepo;
    
    private readonly CrcDbContext _context;
    
    public MoradorService(MoradorRepository repo, AuthRepository authRepo, CrcDbContext context)
    {
        _repo = repo;
        _authRepo = authRepo;
        _context = context;
    }
    
    public async Task<MoradorListResponse> GetAllAsync(int pageNumber, int pageSize)
    {
        var moradores = await _repo.GetAllAsync(pageNumber, pageSize);
        return MapToListResponse(moradores, pageNumber, pageSize);
    }

    public async Task<MoradorResponse> GetByIdAsync(int id)
    {
        var morador = await _repo.GetByIdAsync(id);
        
        if (morador == null)
        {
            return null;
        }
        
        return MapToResponse(morador);
    }

    public async Task<MoradorResponse> AddAsync(MoradorRequest request)
    {
        
            var auth = new Auth()
            {
                Email = request.Email,
                HashSenha = UtilsService.QuickHash(request.Senha)
            };

            var authDb = await _authRepo.AddAsync(auth);

            var morador = MapToEntity(request, authDb);
            morador = await _repo.AddAsync(morador);
            
            morador = await _repo.GetByIdAsync(morador.Id);
            
            return MapToResponse(morador);
        
        
    }
    
    public async Task<MoradorResponse> UpdateAsync(int id, MoradorRequest request)
    {
        
            var morador = await _repo.GetByIdAsync(id);
            if (morador == null)
            {
                return null;
            }

            var auth = await _authRepo.GetByIdAsync(morador.IdAuth);
            
            auth.Email = request.Email;
            auth.HashSenha = UtilsService.QuickHash(request.Senha);

            var authDb = await _authRepo.UpdateAsync(auth);
            
            morador.Nome = request.Nome;
            morador.IdCondominio = request.IdCondominio;
            morador.IdAuth = authDb.Id;
            morador.Cpf = request.Cpf;
            morador.QtdMoradores = request.QtdMoradores;
            morador.IdentificadorRes = request.IdentificadorRes;

            var updatedMorador = await _repo.UpdateAsync(morador);
            
            updatedMorador = await _repo.GetByIdAsync(updatedMorador.Id);

            return MapToResponse(updatedMorador);
       
        
    }
    
    public async Task<MoradorResponse> DeleteAsync(int id)
    {
        var morador = await _repo.GetByIdAsync(id);
        
        if (morador == null)
        {
            return null;
        }
        
        var auth = await _authRepo.GetByIdAsync(morador.IdAuth);

        
        await _authRepo.DeleteAsync(auth);
        return MapToResponse(morador);
    }
    
    public async Task<MoradorResponse> GetByEmailAsync(string email)
    {
        var morador = await _repo.GetByEmailAsync(email);
        
        if (morador == null)
        {
            return null;
        }
        
        return MapToResponse(morador);
    }
    
    public async Task<MoradorResponse> GetByCpfAsync(string cpf)
    {
        var morador = await _repo.GetByCpfAsync(cpf);
        
        if (morador == null)
        {
            return null;
        }
        
        return MapToResponse(morador);
    }
    
    public MoradorResponse MapToResponse(Morador morador)
    {
        return new MoradorResponse(
            morador.Id,
            morador.Condominio != null ?new CondominioResponse(
                morador.Condominio.Id,
                morador.Condominio.Nome,
                morador.Condominio.Endereco
            ) : null,
            morador.Auth != null ? new AuthResponse(
                morador.Auth.Id,
                morador.Auth.Email,
                morador.Auth.HashSenha
                ): null,
            morador.Cpf,
            morador.Nome,
            morador.Pontos,
            morador.QtdMoradores,
            morador.IdentificadorRes
        );
    }
    
    //override
    public Morador MapToEntity(MoradorRequest request, Auth auth)
    {
        return new Morador
        {
            IdCondominio = request.IdCondominio,
            Nome = request.Nome,
            IdAuth = auth.Id,
            Cpf = request.Cpf,
            QtdMoradores = request.QtdMoradores,
            IdentificadorRes = request.IdentificadorRes,
            Pontos = 0
        };
    }
    
    public MoradorListResponse MapToListResponse(IEnumerable<Morador> entities, int pageNumber, int pageSize)
    {
        var list = entities.Select(m => MapToResponse(m)).ToList();
        return new MoradorListResponse( pageNumber, pageSize, list.Count, list);
    }

    public Morador MapToEntity(MoradorRequest entity)
    {
        throw new NotImplementedException();
    }
    
}