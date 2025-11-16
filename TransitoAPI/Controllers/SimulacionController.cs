using Microsoft.AspNetCore.Mvc;
using TransitoAPI.Models;
using TransitoAPI.Services.Simulacion;

namespace TransitoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimulacionController : ControllerBase
    {
        private readonly SimuladorService _simulador;
        public SimulacionController(
            SimuladorService simulador)
        {
            _simulador = simulador;
        }



        [HttpPost("iniciar")]
        public IActionResult IniciarSimulacion([FromBody] SimulacionRequest request)
        {
            _simulador.Iniciar(
                request.Cabinas,
                request.IntervaloMs
            );

            return Ok(new { mensaje = "Simulación iniciada" });
        }

        [HttpPost("detener")]
        public IActionResult DetenerSimulacion()
        {
            _simulador.Detener();
            return Ok(new { mensaje = "Simulación estándar detenida" });
        }

        [HttpGet("estado")]
        public IActionResult Estado()
        {
            return Ok(new
            {
                corriendo = _simulador.EstaCorriendo
            });
        }


        [HttpPost("iniciar-personalizada")]
        public IActionResult IniciarPersonalizada([FromBody] SimulacionPersonalizadaRequest req)
        {
            _simulador.Iniciar(
                req.Cabinas,
                req.IntervaloMs,
                req.CantidadVehiculos,
                req.ProbAuto,
                req.ProbMoto,
                req.ProbCamion,
                req.ProbBus
            );

            return Ok(new { mensaje = "Simulación personalizada iniciada" });
        }

        [HttpPost("detener-personalizada")]
        public IActionResult DetenerSimulacionPersonalizada()
        {
            _simulador.Detener();
            return Ok(new { mensaje = "Simulación personalizada detenida" });
        }

        [HttpGet("estado-personalizada")]
        public IActionResult EstadoPersonalizada()
        {
            return Ok(new
            {
                corriendo = _simulador.EstaCorriendo
            });
        }
    }
}
