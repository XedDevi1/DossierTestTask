using MainTestTask.Dto;
using MainTestTask.Models;

namespace MainTestTask.Services.Interfaces
{
    public interface IDossierService
    {
        Task<List<DossierDto>> GetDossiers();
        Task DeleteDossier(int id);
        Task AddDossierAfter(int id, CreateDossierDto createDossierDto);
        Task AddDossierBefore(int id, CreateDossierDto createDossierDto);
        Task AddDossierChild(int id, CreateDossierDto dossierDto);
        Task UpdateDossier(int id, UpdateDossierDto updateDossierDto);

    }
}
