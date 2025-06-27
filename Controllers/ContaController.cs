using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using myapp.Models;
using myapp.Services;

namespace myapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaController : ControllerBase
    {
        private readonly ContaService _contaService;

        public ContaController(ContaService contaService)
        {
            _contaService = contaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conta>>> Get()
        {
            var contas = await _contaService.GetAllContasAsync();
            return Ok(contas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Conta>> Get(int id)
        {
            var conta = await _contaService.GetContaByIdAsync(id);
            if (conta == null) // Changed from _contaService.GetById
            {
                return NotFound();
            }
            return Ok(conta);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Conta conta)
        {
            await _contaService.AddContaAsync(conta);
            return CreatedAtAction(nameof(Get), new { id = conta.Id }, conta);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Conta conta)
        {
            if (id != conta.Id)
            {
                return BadRequest();
            }

            await _contaService.UpdateContaAsync(conta);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _contaService.DeleteContaAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/saque")]
        public async Task<ActionResult> Saque(int id, [FromBody] decimal valor)
        {
            var result = await _contaService.SaqueAsync(id, valor);
            if (!result)
            {
                return BadRequest("Saldo insuficiente ou valor inválido.");
            }
            return Ok("Saque realizado com sucesso.");
        }

        [HttpPost("{id}/deposito")]
        public async Task<ActionResult> Deposito(int id, [FromBody] decimal valor)
        {
            var result = await _contaService.DepositoAsync(id, valor);
            if (!result)
            {
                return BadRequest("Valor de depósito inválido.");
            }
            return Ok("Depósito realizado com sucesso.");
        }

        [HttpPost("transferencia")]
        public async Task<ActionResult> Transferencia([FromBody] TransferenciaRequest request)
        {
            var result = await _contaService.TransferenciaAsync(request.ContaOrigemId, request.ContaDestinoId, request.Valor);
            if (!result)
            {
                return BadRequest("Transferência não realizada. Verifique as contas e o saldo.");
            }
            return Ok("Transferência realizada com sucesso.");
        }
    }
}