using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ToDoWSWCF
{
    public class SessionState
    {
        public SessionState() { }
        public int idUsuario { set; get; }
        public string SessionId { get; set; }
    }
}