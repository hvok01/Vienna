using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Vienna.Models;

namespace Vienna.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DuenioEventoController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public DuenioEventoController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }

        // GET: api/DuenioEvento
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                return Ok(contexto.DuenioEvento.SingleOrDefault(x => x.Correo == usuario));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/DuenioEvento/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(contexto.DuenioEvento.SingleOrDefault(x => x.IdDuenioEvento == id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/DuenioEvento
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(DuenioEvento entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    entidad.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                password: entidad.Clave,
                                                salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                                                prf: KeyDerivationPrf.HMACSHA1,
                                                iterationCount: 1000,
                                                numBytesRequested: 256 / 8));
                    contexto.DuenioEvento.Add(entidad);
                    contexto.SaveChanges();
                    return CreatedAtAction(nameof(Get), new { id = entidad.IdDuenioEvento }, entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT: api/DuenioEvento/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, DuenioEvento entidad)
        {
            try
            {
                //edita solo el propie logeado
                if (ModelState.IsValid && contexto.DuenioEvento.AsNoTracking().SingleOrDefault(e => e.IdDuenioEvento == id && e.Correo == User.Identity.Name) != null)
                {
                    entidad.IdDuenioEvento = id;
                    entidad.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                password: entidad.Clave,
                                                salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                                                prf: KeyDerivationPrf.HMACSHA1,
                                                iterationCount: 1000,
                                                numBytesRequested: 256 / 8));
                    contexto.DuenioEvento.Update(entidad);
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
                //Borra solo el DuenioEvento logeado
                var entidad = contexto.DuenioEvento.FirstOrDefault(e => e.IdDuenioEvento == id && e.Correo == User.Identity.Name);
                if (entidad != null)
                {
                    entidad.EstadoDuenio = 0;
                    contexto.DuenioEvento.Update(entidad);
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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(DuenioEvento entidad)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: entidad.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                var p = contexto.DuenioEvento.FirstOrDefault(x => x.Correo == entidad.Correo);
                if (p == null || p.Clave != hashed)
                {
                    return BadRequest("Nombre de usuario o clave incorrecta");
                }
                else
                {
                    var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, p.Correo),
                        new Claim("FullName", p.Nombre + " " + p.Apellido),
                        new Claim(ClaimTypes.Role, "DuenioEvento"),
                    };

                    var token = new JwtSecurityToken(
                        issuer: config["TokenAuthentication:Issuer"],
                        audience: config["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: credenciales
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
