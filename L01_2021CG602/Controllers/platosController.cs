using L01_2021CG602.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2021CG602.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class platosController : ControllerBase
    {
        private readonly restauranteContext _restauranteContext;
        public platosController(restauranteContext restauranteContext)
        {
            _restauranteContext = restauranteContext;
        }

        [HttpGet]
        [Route("GetAllPlatos")]
        public IActionResult GetAllPlatos()
        {
            List<platos> listadoPlatos = (from e in _restauranteContext.platos select e).ToList();

            if (listadoPlatos.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoPlatos);
        }

        [HttpPost]
        [Route("AddPlato")]
        public IActionResult CrearPlato([FromBody] platos plato)
        {
            try
            {
                _restauranteContext.platos.Add(plato);
                _restauranteContext.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarPlato(int id, [FromBody] platos platoModificar)
        {
            platos? platoActual = (from e in _restauranteContext.platos
                                     where e.platoId == id
                                     select e).FirstOrDefault();
            if (platoActual == null)
            {
                return NotFound();
            }

            //platoActual.platoId = platoModificar.platoId;
            platoActual.nombrePlato = platoModificar.nombrePlato;
            platoActual.precio = platoModificar.precio;


            _restauranteContext.Entry(platoActual).State = EntityState.Modified;
            _restauranteContext.SaveChanges();

            return Ok(platoModificar);

        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarPlato(int id)
        {
            platos? plato = (from e in _restauranteContext.platos
                               where e.platoId == id
                               select e).FirstOrDefault();
            if (plato == null)
                return NotFound();

            _restauranteContext.platos.Attach(plato);
            _restauranteContext.platos.Remove(plato);
            _restauranteContext.SaveChanges();

            return Ok(plato);
        }

        [HttpGet]
        [Route("GetPlatoMenorQue/{precio}")]
        public IActionResult GetPedidoByMotorista(decimal precio)
        {
            List<platos> listadoPlatos = (from e in _restauranteContext.platos
                               where e.precio < precio
                               select e).ToList();
            if (listadoPlatos.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoPlatos);

        }
    }
}
