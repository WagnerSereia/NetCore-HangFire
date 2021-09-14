using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore_HangFire.Entities
{
    public class Tarefa
    {        
        public Tarefa(string mensagem, DateTime dataAgendamento, tipo tipo)
        {
            this.id = Guid.NewGuid();
            Mensagem = mensagem;
            DataAgendamento = dataAgendamento;
            Tipo = tipo;
        }

        public Guid id { get; private set; }
        public string Mensagem { get; private set; }
        public DateTime DataAgendamento { get; private set; }
        public tipo Tipo { get; private set; }

        public enum tipo 
        {
            DisparaEEsquece=0,
            Atrasada=1,
            Recorrente=2,
            Encadeada=3
        };
    }
}
