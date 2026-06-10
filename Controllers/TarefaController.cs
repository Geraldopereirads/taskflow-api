using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var tarafa = await _context.Tarefas.FindAsync(id);
            if (tarafa == null)
            {
                return NotFound();
            }
            return Ok(tarafa);
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var tarefa = await _context.Tarefas.ToListAsync();
            return Ok(tarefa);
        }


        [HttpGet("titulo")]
        public async Task<IActionResult> ObterPorTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return BadRequest("Título inválido.");

            var tarefas = await _context.Tarefas
                .Where(x => x.Titulo != null &&
                            x.Titulo.ToLower().Contains(titulo.ToLower()))
                .ToListAsync();


            return Ok(tarefas);
        }




        [HttpGet("Data")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }



        [HttpGet("Status")]
        public async Task<IActionResult> ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefa = await _context.Tarefas.Where(x => x.Status == status).ToListAsync();
            return Ok(tarefa);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Tarefa tarefa)
        {


            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = await _context.Tarefas.FindAsync(id);

            if (tarefaBanco == null)
                return NotFound();  

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _context.Tarefas.Update(tarefaBanco);
            await _context.SaveChangesAsync();

            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var tarefaBanco = await _context.Tarefas.FindAsync(id);

            if (tarefaBanco == null)
                return NotFound();

            _context.Tarefas.Remove(tarefaBanco);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
