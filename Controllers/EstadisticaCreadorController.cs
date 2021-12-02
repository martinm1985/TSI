using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Crud.Services;
using Crud.Models;
using Crud.DTOs;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Crud.Data;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadisticaCreadorController : ControllerBase
    {
        private readonly EstadisticaCreadorService _estadisticaCreador;
        private readonly  IIdentityService _identityService;

        public EstadisticaCreadorController(EstadisticaCreadorService estadisticaService, IIdentityService identityService)
        {
                        _identityService = identityService;

            _estadisticaCreador = estadisticaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<EstadisticaCreador>>> Get() 
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);
            if (user==null){
                return Unauthorized();
            }

            return  _estadisticaCreador.Get(user.Id);
        }

        [HttpGet("{id:length(24)}", Name = "GetEstadisticaCreador")]
        public ActionResult<List<EstadisticaCreador>> Get(string id)
        {
            var est = _estadisticaCreador.Get(id);

            if (est == null)
            {
                return NotFound();
            }

            return est;
        }

        [HttpPost]
        public ActionResult<EstadisticaCreador> Create(EstadisticaCreador est)
        {
            _estadisticaCreador.Create(est);

            return CreatedAtRoute("GetEstadisticaCreador", new { id = est.Id.ToString() }, est);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, EstadisticaCreador estIn)
        {
            var est = _estadisticaCreador.Get(id);

            if (est == null)
            {
                return NotFound();
            }

            _estadisticaCreador.Update(id, estIn);

            return NoContent();
        }

        // [HttpDelete("{id:length(24)}")]
        // public IActionResult Delete(string id)
        // {
        //     var est = _estadisticaCreador.Get(id);

        //     if (est == null)
        //     {
        //         return NotFound();
        //     }

        //     _estadisticaCreador.Remove(est.Id);

        //     return NoContent();
        // }

    }
}
