using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vienna.Models;

namespace Vienna.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContratoController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public ContratoController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }
        // GET: api/Contrato
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                var contratosByDuenioEvento = contexto.Contrato
                .Include(e => e.Evento)
                .ThenInclude(s => s.Sala)
                .Include(z => z.Evento)
                .ThenInclude(d => d.DuenioEvento)
                .Where(x => x.EstadoContrato == 1 && x.Evento.DuenioEvento.Correo == usuario && x.Evento.DuenioEvento.IdDuenioEvento == x.Evento.IdDuenioEvento);
                return Ok(contratosByDuenioEvento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Contrato/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                var contratoById = contexto.Contrato
                .Include(e => e.Evento)
                .ThenInclude(s => s.Sala)
                .Include(z => z.Evento)
                .ThenInclude(d => d.DuenioEvento)
                .Where(x => x.EstadoContrato == 1 && x.Evento.DuenioEvento.Correo == usuario && x.IdContrato == id && x.Evento.DuenioEvento.IdDuenioEvento == x.Evento.IdDuenioEvento);
                return Ok(contratoById);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/Contrato
        [HttpPost]
        public async Task<IActionResult> Post(Contrato entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contexto.Contrato.Add(entidad);
                    contexto.SaveChanges();
                    return CreatedAtAction(nameof(Get), new { id = entidad.IdContrato }, entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT: api/Contrato/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Contrato entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    entidad.IdContrato = id;
                    contexto.Contrato.Update(entidad);
                    contexto.SaveChanges();
                    return Ok(entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entidad = contexto.Contrato.FirstOrDefault(x => x.EstadoContrato == 1 && x.IdContrato == id);

                if (entidad != null)
                {
                    entidad.EstadoContrato = 0;
                    contexto.Contrato.Update(entidad);
                    contexto.SaveChanges();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
