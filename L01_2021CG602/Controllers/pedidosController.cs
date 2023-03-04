using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using L01_2021CG602.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2021CG602.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class pedidosController : ControllerBase
    {
        private readonly restauranteContext _restauranteContext;

        public pedidosController(restauranteContext restauranteContext)
        {
            _restauranteContext= restauranteContext;
        }

        [HttpGet]
        [Route("GetAllPedidos")]
        public IActionResult GetAllPedidos()
        {
            List<pedidos> listadoPedidos = (from e in _restauranteContext.pedidos select e).ToList();

            if (listadoPedidos.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoPedidos);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult CrearPedido([FromBody] pedidos pedido)
        {
            try
            {
                _restauranteContext.pedidos.Add(pedido);
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
        public IActionResult ActualizarPedido(int id, [FromBody] pedidos pedidoModificar)
        {
            pedidos? pedidoActual = (from e in _restauranteContext.pedidos
                                     where e.pedidoId == id
                                     select e).FirstOrDefault();
            if (pedidoActual == null)
            {
                return NotFound();
            }

            //pedidoActual.motoristaId = pedidoModificar.motoristaId;
            pedidoActual.clienteId = pedidoModificar.clienteId;
            pedidoActual.platoId = pedidoModificar.platoId;
            pedidoActual.cantidad = pedidoModificar.cantidad;
            pedidoActual.precio = pedidoModificar.precio;



            _restauranteContext.Entry(pedidoActual).State = EntityState.Modified;
            _restauranteContext.SaveChanges();

            return Ok(pedidoModificar);

        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarPedido(int id)
        {
            pedidos? pedido = (from e in _restauranteContext.pedidos
                               where e.pedidoId == id
                               select e).FirstOrDefault();
            if (pedido == null)
                return NotFound();

            _restauranteContext.pedidos.Attach(pedido);
            _restauranteContext.pedidos.Remove(pedido);
            _restauranteContext.SaveChanges();

            return Ok(pedido);
        }

        [HttpGet]
        [Route("GetPedidoByCliente/{clienteId}")]
        public IActionResult GetPedidoByCliente(int clienteId)
        {
            pedidos? pedido = (from e in _restauranteContext.pedidos
                               where e.clienteId == clienteId
                               select e).FirstOrDefault();
            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }

        [HttpGet]
        [Route("GetPedidoByMotorista/{motoristaId}")]
        public IActionResult GetPedidoByMotorista(int motoristaId)
        {
            pedidos? pedido = (from e in _restauranteContext.pedidos
                               where e.motoristaId == motoristaId
                               select e).FirstOrDefault();
            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }


    }
}
