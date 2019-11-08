using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Vienna.Models;

namespace Vienna.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EventoInvitadoController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public EventoInvitadoController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }

        // GET: api/EventoInvitado
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                var eventosInvitados = contexto.EventoInvitado.ToList();
                return Ok(eventosInvitados);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/EventoInvitado/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                var eventosInvitadosById = contexto.EventoInvitado.FirstOrDefault(x => x.IdInvitado == id);
                return Ok(eventosInvitadosById);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/EventoInvitado
        [HttpPost]
        public async Task<IActionResult> Post(EventoInvitado entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contexto.EventoInvitado.Add(entidad);
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

        // PUT: api/EventoInvitado/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoInvitado entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    entidad.IdInvitado = id;
                    contexto.EventoInvitado.Update(entidad);
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
                var entidad = contexto.EventoInvitado.FirstOrDefault(x => x.IdInvitado == id);

                if (entidad != null)
                {
                    entidad.EstadoRelacion = 0;
                    contexto.EventoInvitado.Update(entidad);
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
