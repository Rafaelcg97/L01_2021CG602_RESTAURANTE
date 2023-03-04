using L01_2021CG602.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2021CG602.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class motoristasController : ControllerBase
    {
        private readonly restauranteContext _restauranteContext;
        public motoristasController(restauranteContext restauranteContext)
        {
            _restauranteContext = restauranteContext;
        }

        [HttpGet]
        [Route("GetAllMotoristas")]
        public IActionResult Get()
        {
            List<motoristas> listadoMotoristas = (from e in _restauranteContext.motoristas select e).ToList();

            if (listadoMotoristas.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoMotoristas);
        }

        [HttpPost]
        [Route("AddMotorista")]
        public IActionResult CreatMotorista([FromBody] motoristas motorista)
        {
            try
            {
                _restauranteContext.motoristas.Add(motorista);
                _restauranteContext.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("actualizarMotorista/{id}")]
        public IActionResult ActualizarMotorista(int id, [FromBody] motoristas motoristaModificar)
        {
            motoristas? motoristaActual = (from e in _restauranteContext.motoristas
                                   where e.motoristaId == id
                                   select e).FirstOrDefault();
            if (motoristaActual == null)
            {
                return NotFound();
            }

            //motoristaActual.motoristaId = motoristaModificar.motoristaId;
            motoristaActual.nombreMotorista = motoristaModificar.nombreMotorista;


            _restauranteContext.Entry(motoristaActual).State = EntityState.Modified;
            _restauranteContext.SaveChanges();

            return Ok(motoristaModificar);

        }

        [HttpDelete]
        [Route("eliminarMotorista/{id}")]
        public IActionResult EliminarMotorista(int id)
        {
            motoristas? motorista = (from e in _restauranteContext.motoristas
                             where e.motoristaId == id
                             select e).FirstOrDefault();
            if (motorista == null)
                return NotFound();

            _restauranteContext.motoristas.Attach(motorista);
            _restauranteContext.motoristas.Remove(motorista);
            _restauranteContext.SaveChanges();

            return Ok(motorista);
        }

        [HttpGet]
        [Route("GetMotoristaByNombre/{nombre}")]
        public IActionResult GetMotoristaByNombre(string nombre)
        {
            motoristas? motorista = (from e in _restauranteContext.motoristas
                                     where e.nombreMotorista.Contains(nombre)
                                     select e).FirstOrDefault();

            if (motorista == null)
                return NotFound();

            return Ok(motorista);
        }
    }
}
