using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoWSWCFData
{
    /// <summary>
    /// Criado por Alexis de A. oliveira
    /// Data: 07/02/2014
    /// </summary>
    public class Tarefa
    {
        public int IdServer { set; get; }
        public int IdLocal { set; get; }
        public string Descricao { set; get; }
        public string Observacao { set; get; }
        public DateTime Data { set; get; }
        public bool Notificar { set; get; }
    }
}