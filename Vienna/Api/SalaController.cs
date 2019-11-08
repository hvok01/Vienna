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
    public class SalaController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public SalaController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }
        // GET: api/Sala
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                var salas = contexto.Sala.ToList();
                return Ok(salas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Sala/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var salaById = contexto.Sala.FirstOrDefault(x => x.IdSala == id);
                return Ok(salaById);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/Sala
        [HttpPost]
        public async Task<IActionResult> Post(Sala entidad)
        {
            try
            {
                //,
                //
                if (ModelState.IsValid)
                {
                    contexto.Sala.Add(entidad);
                    contexto.SaveChanges();
                    return CreatedAtAction(nameof(Get), new { id = entidad.IdSala }, entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT: api/Sala/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Sala entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    entidad.IdSala = id;
                    contexto.Sala.Update(entidad);
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
                var entidad = contexto.Sala.FirstOrDefault(x => x.EstadoSala == 1 && x.IdSala == id);

                if (entidad != null)
                {
                    entidad.EstadoSala = 0;
                    contexto.Sala.Update(entidad);
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
