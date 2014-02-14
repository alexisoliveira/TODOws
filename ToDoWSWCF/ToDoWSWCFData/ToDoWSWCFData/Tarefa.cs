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
        public int Id { set; get; }
        public string Nome { set; get; }
        public string Observacao { set; get; }
        public string DataFinalizacao { set; get; }
        public bool Notificar { set; get; }
        public bool Status { set; get; }
        public int Id_usuario { set; get; }
    }
}