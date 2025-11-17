namespace TransitoAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TransitoAPI.Models;
    using TransitoAPI.Rabbit;
    using TransitoAPI.Services.Interfaces;

    [ApiController]
    [Route("api/[controller]")]
    public class TollController : ControllerBase
    {
        private readonly ITollService _service;

        public TollController(ITollService service)
        {
            _service = service;
        }

        [HttpGet("test-rabbit")]
        public IActionResult TestRabbit([FromServices] RabbitMqPublisher pub)
        {
            pub.PublishLanePassage(new
            {
                event_id = Guid.NewGuid(),
                toll_id = "TEST-TOLL",
                lane_id = "L-01",
                timestamp_utc = DateTime.UtcNow.ToString("o"),
                plate_raw = "TEST123",
                vehicle_class_hint = "car",
                source = "unit-test"
            });

            return Ok("Evento enviado al bus de datos.");
        }


        [HttpGet("cabinas")]
        public async Task<IActionResult> ObtenerCabinas()
        {
            return Ok(await _service.ObtenerCabinasAsync());
        }

        [HttpPost("cabinas")]
        public async Task<IActionResult> CrearCabina([FromBody] CabinaCrearDto dto)
        {
            return Ok(await _service.CrearCabinaAsync(dto));
        }

        [HttpPut("cabinas/{id}")]
        public async Task<IActionResult> ActualizarCabina(Guid id, [FromBody] CabinaActualizarDto dto)
        {
            var cabina = await _service.ActualizarCabinaAsync(id, dto);
            if (cabina == null) return NotFound();
            return Ok(cabina);
        }

        [HttpPatch("cabinas/{id}")]
        public async Task<IActionResult> ParchearCabina(Guid id, [FromBody] object dto)
        {
            var cabina = await _service.ParchearCabinaAsync(id, dto);
            if (cabina == null) return NotFound();
            return Ok(cabina);
        }

        [HttpDelete("cabinas/{gate_id}")]
        public async Task<IActionResult> EliminarCabina(Guid gate_id)
        {
            bool ok = await _service.EliminarCabinaAsync(gate_id);
            return ok ? Ok() : NotFound();
        }



        [HttpGet("transitos")]
        public async Task<IActionResult> ObtenerTransitos()
        {
            return Ok(await _service.ObtenerTransitosAsync());
        }

        [HttpPost("transitos")]
        public async Task<IActionResult> CrearTransito([FromBody] TransitoCrearDto dto)
        {
            
            return Ok(await _service.CrearTransitoAsync(dto));
        }

        [HttpPut("transitos/{id}")]
        public async Task<IActionResult> ActualizarTransito(Guid id, [FromBody] TransitoActualizarDto dto)
        {
            var transito = await _service.ActualizarTransitoAsync(id, dto);
            return transito == null ? NotFound() : Ok(transito);
        }

        [HttpPatch("transitos/{id}")]
        public async Task<IActionResult> ParchearTransito(Guid id, [FromBody] object dto)
        {
            var transito = await _service.ParchearTransitoAsync(id, dto);
            return transito == null ? NotFound() : Ok(transito);
        }

        [HttpDelete("transitos/{id}")]
        public async Task<IActionResult> EliminarTransito(Guid id)
        {
            bool ok = await _service.EliminarTransitoAsync(id);
            return ok ? Ok() : NotFound();
        }
        [HttpGet("estadisticas")]
        public async Task<IActionResult> ObtenerEstadisticas()
        {
            var stats = await _service.ObtenerEstadisticasAsync();
            return Ok(stats);
        }
    }

}
