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
    
    public class Usuario
    {
        public int IdUsuario { set; get; }
        public string Email { set; get; }
        public string Telefone { set; get; }
        public string Senha { set; get; }
    }
}