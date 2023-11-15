using AutoMapper;
using MainTestTask.Dto;
using MainTestTask.Exceptions;
using MainTestTask.Models;
using MainTestTask.Persistence;
using MainTestTask.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MainTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DossierController : ControllerBase
    {
        private readonly IDossierService _dossierService;

        public DossierController(IDossierService dossierService)
        {
            _dossierService = dossierService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DossierDto>>> GetDossiers()
        {
            var dossierDtos = await _dossierService.GetDossiers();
            return Ok(dossierDtos);
        }

        //[HttpPost]
        //public async Task<ActionResult<Dossier>> PostDossier(CreateDossierDto createDossierDto)
        //{
        //    Dossier dossier = new Dossier
        //    {
        //        ParentId = createDossierDto.ParentId,
        //        SectionCode = createDossierDto.SectionCode,
        //        Name = createDossierDto.Name
        //    };

        //    if (dossier.ParentId.HasValue)
        //    {
        //        var parent = await _context.Dossiers.FindAsync(dossier.ParentId.Value);
        //        if (parent == null)
        //        {
        //            return NotFound("Родительское досье не найдено");
        //        }

        //        // Назначаем OrderNumber на основе уже существующих элементов в той же ветке
        //        var siblings = _context.Dossiers.Where(d => d.ParentId == dossier.ParentId);
        //        dossier.OrderNumber = siblings.Any() ? siblings.Max(d => d.OrderNumber) + 1 : 0;
        //    }
        //    else
        //    {
        //        // Если это корневой элемент, то OrderNumber будет 0
        //        dossier.OrderNumber = 0;
        //    }

        //    _context.Dossiers.Add(dossier);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDossier(int id)
    {
        try
        {
            await _dossierService.DeleteDossier(id);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

        [HttpPost("{id}/After")]
        public async Task<IActionResult> AddDossierAfter(int id, CreateDossierDto createDossierDto)
        {
            try
            {
                await _dossierService.AddDossierAfter(id, createDossierDto);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{id}/Before")]
        public async Task<IActionResult> AddDossierBefore(int id, CreateDossierDto createDossierDto)
        {
            try
            {
                await _dossierService.AddDossierBefore(id, createDossierDto);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{id}/Child")]
        public async Task<ActionResult<Dossier>> PostDossierChild(int id, CreateDossierDto dossierDto)
        {
            try
            {
                await _dossierService.AddDossierChild(id, dossierDto);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/Update")]
        public async Task<IActionResult> UpdateDossier(int id, UpdateDossierDto updateDossierDto)
        {
            try
            {
                await _dossierService.UpdateDossier(id, updateDossierDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}