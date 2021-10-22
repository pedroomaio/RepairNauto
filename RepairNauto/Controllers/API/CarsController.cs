using AutoRepair.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AutoRepair.Controllers.API
{

    [Route("api/[controller]/{id}")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CarsController : Controller
    {


        private readonly IInspecionRepository _inspecionRepository;

        public CarsController(IInspecionRepository inspecionRepository)
        {
            _inspecionRepository = inspecionRepository;
        }


        [HttpGet]
        
        public IActionResult GetCars(int id)
        {
            return Ok(_inspecionRepository.GetAllWithCars(id));
        }
    }
}
