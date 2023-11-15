using AutoMapper;
using MainTestTask.Dto;
using MainTestTask.Exceptions;
using MainTestTask.Models;
using MainTestTask.Persistence;
using MainTestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MainTestTask.Services
{
    public class DossierService : IDossierService
    {
        private readonly DossierDbContext _context;
        private readonly IMapper _mapper;

        public DossierService(DossierDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DossierDto>> GetDossiers()
        {
            var dossiers = await _context.Dossiers.ToListAsync();
            var dossierDict = dossiers.ToDictionary(d => d.Id);
            var rootDossiers = new List<Dossier>();

            foreach (var dossier in dossiers)
            {
                if (dossier.ParentId == null)
                {
                    rootDossiers.Add(dossier);
                }
                else if (dossierDict.ContainsKey(dossier.ParentId.Value))
                {
                    var parent = dossierDict[dossier.ParentId.Value];
                    if (parent.Children == null)
                    {
                        parent.Children = new List<Dossier>();
                    }
                    parent.Children.Add(dossier);
                }
            }

            rootDossiers = rootDossiers.OrderBy(d => d.OrderNumber).ToList();
            foreach (var dossier in rootDossiers)
            {
                if (dossier.Children != null)
                {
                    dossier.Children = dossier.Children.OrderBy(d => d.OrderNumber).ToList();
                }
            }

            var dossierDtos = _mapper.Map<List<DossierDto>>(rootDossiers);

            return dossierDtos;
        }

        public async Task DeleteDossier(int id)
        {
            var dossier = await _context.Dossiers.FindAsync(id);
            if (dossier == null)
            {
                throw new NotFoundException("Dossier not found");
            }

            var siblings = await _context.Dossiers.Where(d => d.ParentId == dossier.ParentId && d.OrderNumber > dossier.OrderNumber).ToListAsync();
            foreach (var sibling in siblings)
            {
                sibling.OrderNumber--;
            }

            await DeleteChildrenAsync(dossier);

            _context.Dossiers.Remove(dossier);
            await _context.SaveChangesAsync();
        }

        private async Task DeleteChildrenAsync(Dossier parent)
        {
            var children = await _context.Dossiers.Where(d => d.ParentId == parent.Id).ToListAsync();
            foreach (var child in children)
            {
                await DeleteChildrenAsync(child);
                _context.Dossiers.Remove(child);
            }
            await _context.SaveChangesAsync();
        }

        public async Task AddDossierAfter(int id, CreateDossierDto createDossierDto)
        {
            var currentDossier = _context.Dossiers.FirstOrDefault(d => d.Id == id);
            if (currentDossier == null)
            {
                throw new NotFoundException("Dossier not found");
            }

            var newDossier = new Dossier
            {
                ParentId = currentDossier.ParentId,
                OrderNumber = currentDossier.OrderNumber + 1,
                SectionCode = createDossierDto.SectionCode,
                Name = createDossierDto.Name
            };
            _context.Dossiers.Add(newDossier);

            var subsequentDossiers = _context.Dossiers.Where(d => d.ParentId == currentDossier.ParentId && d.OrderNumber > currentDossier.OrderNumber).ToList();
            foreach (var dossier in subsequentDossiers)
            {
                dossier.OrderNumber++;
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddDossierBefore(int id, CreateDossierDto createDossierDto)
        {
            var currentDossier = _context.Dossiers.FirstOrDefault(d => d.Id == id);
            if (currentDossier == null)
            {
                throw new NotFoundException("Dossier not found");
            }

            var newDossier = new Dossier
            {
                ParentId = currentDossier.ParentId,
                OrderNumber = currentDossier.OrderNumber,
                SectionCode = createDossierDto.SectionCode,
                Name = createDossierDto.Name
            };
            _context.Dossiers.Add(newDossier);

            var subsequentDossiers = _context.Dossiers.Where(d => d.ParentId == currentDossier.ParentId && d.OrderNumber >= currentDossier.OrderNumber).ToList();
            foreach (var dossier in subsequentDossiers)
            {
                dossier.OrderNumber++;
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddDossierChild(int id, CreateDossierDto dossierDto)
        {
            var parent = await _context.Dossiers
                .OrderByDescending(d => d.Id)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (parent == null)
            {
                throw new NotFoundException("Parent dossier not found");
            }

            int orderNumber;
            if (_context.Dossiers.Any(d => d.ParentId == parent.Id))
            {
                orderNumber = _context.Dossiers.Where(d => d.ParentId == parent.Id).Max(d => d.OrderNumber) + 1;
            }
            else
            {
                orderNumber = 0;
            }

            var newDossier = new Dossier
            {
                ParentId = parent.Id,
                OrderNumber = orderNumber,
                SectionCode = dossierDto.SectionCode,
                Name = dossierDto.Name
            };
            _context.Dossiers.Add(newDossier);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateDossier(int id, UpdateDossierDto updateDossierDto)
        {
            var dossier = await _context.Dossiers.FindAsync(id);
            if (dossier == null)
            {
                throw new KeyNotFoundException("Dossier not found");
            }

            dossier.SectionCode = updateDossierDto.SectionCode;
            dossier.Name = updateDossierDto.Name;

            await _context.SaveChangesAsync();
        }

    }
}
