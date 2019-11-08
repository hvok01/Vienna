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
    public class EventoController : ControllerBase
    {

        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public EventoController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }

        // GET: api/Evento
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                var eventosByDuenio = contexto.Evento
                .Include(d => d.DuenioEvento)
                .Include(s => s.Sala)
                .Where(x => x.EstadoEvento == 1 && x.DuenioEvento.Correo == usuario && x.IdDuenioEvento == x.DuenioEvento.IdDuenioEvento);
                return Ok(eventosByDuenio);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Evento/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                var contratoById = contexto.Evento
                .Include(d => d.DuenioEvento)
                .Include(s => s.Sala)
                .Where(x => x.EstadoEvento == 1 && x.DuenioEvento.Correo == usuario && x.IdEvento == id);
                return Ok(contratoById);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{nombre}")]
        public async Task<IActionResult> Get(string nombre)
        {
            try
            {
                //trae la cantidad de asistenes de un evento por el nombre del evento.

                var existe = contexto.Evento.FirstOrDefault(x => x.Nombre == nombre);

                if(existe != null)
                {
                    var usuario = User.Identity.Name;
                    var eventoByNombre = contexto.Evento
                    .FirstOrDefault(x => x.EstadoEvento == 1 && x.DuenioEvento.Correo == usuario
                    && x.DuenioEvento.IdDuenioEvento == x.IdDuenioEvento && x.Nombre == nombre);

                    var asistentes = eventoByNombre.CantidadAsistentes;

                    return Ok(eventoByNombre);

                } else
                {
                    return BadRequest("No se encontró un evento con ese nombre");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/Evento
        [HttpPost("{NombreSala}")]
        public async Task<IActionResult> Post(Evento entidad, string nombreSala)
        {
            try
            {
                //Metodo post crea un evento nuevo del propietario y establece un nuevo contrato.

                var fechaOcupada = contexto.Evento.FirstOrDefault(x => x.InicioEvento >= entidad.InicioEvento
                && x.FinEvento <= entidad.FinEvento);

                var salaByNombre = contexto.Sala.FirstOrDefault(x => x.EstadoSala == 1 && x.Nombre == nombreSala);

                var usuario = contexto.DuenioEvento.FirstOrDefault(x => x.Correo == User.Identity.Name);

                if (ModelState.IsValid && fechaOcupada == null)
                {
                    entidad.IdSala = salaByNombre.IdSala;
                    entidad.IdDuenioEvento = usuario.IdDuenioEvento;
                    entidad.EstadoEvento = 1;
                    entidad.CantidadAsistentes = 0;
                    contexto.Evento.Add(entidad);
                    contexto.SaveChanges();
                    var entidadContrato = new Contrato();
                    entidadContrato.EstadoContrato = 1;
                    entidadContrato.IdEvento = entidad.IdEvento;
                    entidadContrato.Pagado = 0;
                    entidadContrato.PrecioFinal = 0;
                    contexto.Contrato.Add(entidadContrato);
                    contexto.SaveChanges();
                    return CreatedAtAction(nameof(Get), new { id = entidad.IdEvento }, entidad);
                    
                }
                else
                {
                    return BadRequest("No es posible realizar un evento en esta fecha.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT: api/Evento/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Evento entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    entidad.IdEvento = id;
                    contexto.Evento.Update(entidad);
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
                var entidad = contexto.Evento.FirstOrDefault(x => x.EstadoEvento == 1 && x.IdEvento == id);

                if (entidad != null)
                {
                    entidad.EstadoEvento = 0;
                    contexto.Evento.Update(entidad);
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
