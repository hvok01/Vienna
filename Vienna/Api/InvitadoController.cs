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
    public class InvitadoController : ControllerBase
    {

        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public InvitadoController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }

        // GET: api/Invitado
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                var invitadosByDuenio = contexto.Invitado
                .Include(e => e.EventoInvitado)
                .ThenInclude(e => e.Evento)
                .ThenInclude(z => z.DuenioEvento)
                .Where(x => x.EstadoInvitado == 1);
                return Ok(invitadosByDuenio);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Invitado/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                var invitadosById = contexto.Invitado
                .Include(e => e.EventoInvitado)
                .ThenInclude(e => e.Evento)
                .ThenInclude(z => z.DuenioEvento)
                .Where(x => x.EstadoInvitado == 1);
                return Ok(invitadosById);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/Invitado
        [HttpPost]
        public async Task<IActionResult> Post(Invitado entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contexto.Invitado.Add(entidad);
                    contexto.SaveChanges();
                    return CreatedAtAction(nameof(Get), new { id = entidad.IdInvitado }, entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT: api/Invitado/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Invitado entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    entidad.IdInvitado = id;
                    contexto.Invitado.Update(entidad);
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
                var entidad = contexto.Invitado.FirstOrDefault(x => x.EstadoInvitado == 1 && x.IdInvitado == id);

                if (entidad != null)
                {
                    entidad.EstadoInvitado = 0;
                    contexto.Invitado.Update(entidad);
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
