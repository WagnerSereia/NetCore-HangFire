using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCore_HangFire.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore_HangFire.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly ICollection<Tarefa> tarefas;

        private readonly ILogger<Tarefa> _logger;

        public TarefaController(ILogger<Tarefa> logger)
        {
            _logger = logger;
            tarefas = new List<Tarefa>();
            tarefas.Add(new Tarefa("Fire and Forget, dispara e esquece", DateTime.Now, Tarefa.tipo.DisparaEEsquece));
            tarefas.Add(new Tarefa("Delayed, com atraso de 30 segundos", DateTime.Now, Tarefa.tipo.Atrasada));
            tarefas.Add(new Tarefa("Recorrente, disparada a cada minuto", DateTime.Now, Tarefa.tipo.Recorrente));
            tarefas.Add(new Tarefa("Enfileirada 1 após outra - tarefa 1", DateTime.Now, Tarefa.tipo.Encadeada));
            tarefas.Add(new Tarefa("Enfileirada 1 após outra - tarefa 2", DateTime.Now, Tarefa.tipo.Encadeada));
        }

        [HttpPost("DispararTarefaUnica")]
        public void DispararTarefaUnica()
        {
            var tarefa = tarefas.Where(t => t.Tipo == Tarefa.tipo.DisparaEEsquece).First();
            BackgroundJob.Enqueue(() => Console.WriteLine($"Mensagem: {tarefa.Mensagem}, Data agendada: {tarefa.DataAgendamento}, Data Atual: {DateTime.Now}"));
        }

        [HttpPost("DispararTarefaComAtraso")]
        public void DispararTarefaComAtraso()
        {
            var tarefa = tarefas.Where(t => t.Tipo == Tarefa.tipo.Atrasada).First();
            BackgroundJob.Schedule(() => Console.WriteLine($"Mensagem: {tarefa.Mensagem}, Data agendada: {tarefa.DataAgendamento}, Data Atual: {DateTime.Now}"), TimeSpan.FromSeconds(30));
        }

        [HttpPost("DispararTarefaRecorrente")]
        public void DispararTarefaRecorrente()
        {
            var tarefa = tarefas.Where(t => t.Tipo == Tarefa.tipo.Recorrente).First();
            RecurringJob.AddOrUpdate(() => Console.WriteLine($"Mensagem: {tarefa.Mensagem}, Data agendada: {tarefa.DataAgendamento}, Data Atual: {DateTime.Now}"), Cron.Minutely);
        }

        [HttpPost("DispararTarefaEncadeada")]
        public void DispararTarefaEncadeada()
        {
            var id = ExecutarTarefa1();

            var tarefa2 = tarefas.Where(t => t.Tipo == Tarefa.tipo.Encadeada && t.Mensagem.Contains("2")).First();
            BackgroundJob.ContinueWith(id, () => Console.WriteLine($"Mensagem: {tarefa2.Mensagem}, Data agendada: {tarefa2.DataAgendamento}, Data Atual: {DateTime.Now}"));
        }

        private string ExecutarTarefa1()
        {
            var tarefa1 = tarefas.Where(t => t.Tipo == Tarefa.tipo.Encadeada).First();
            var id = BackgroundJob.Enqueue(() => Console.WriteLine($"Mensagem: {tarefa1.Mensagem}, Data agendada: {tarefa1.DataAgendamento}, Data Atual: {DateTime.Now}"));

            Task.Delay(5000);
            Console.WriteLine("Termino da tarefa 1");
            
            return id;
        }

        [HttpPost("DispararTarefaComErro")]
        public void DispararTarefaComErro()
        {            
            BackgroundJob.Enqueue(() => TarefaComErro());
        }
        [HttpPost("TarefaComErro-NAOCHAMAR")]
        public async Task TarefaComErro()
        {
            await Task.Run(() =>
            {
                Console.WriteLine($"Mensagem única por com erro de execução, Data Atual: {DateTime.Now}");
                throw new ArgumentException("Falha proposital para avaliar a qtd de retentativas");
            });
        }
    }
}
