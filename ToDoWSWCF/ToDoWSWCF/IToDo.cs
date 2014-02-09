using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ToDoWSWCF;

namespace ToDoWSWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IToDo" in both code and config file together.
    [ServiceContract]
    public interface IToDo
    {
       /*
        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json, UriTemplate = "SaveTarefa?idLocal={idLocal}&descricao={descricao}&observacao{observacao}&data={data}&notificar={notificar}")]
        Tarefa[] SaveTarefa(int idLocal, string descricao, string observacao, DateTime data, bool notificar);
        */

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "gettarefa")]
        List<Tarefa> GetTarefas();

        /*
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "sincronizar/{value}")]
        bool Sincronizar(List<Tarefa> tarefa);
         */

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "efetuarlogin/{login};{senha}")]
        bool EfetuarLogin(string login, string senha);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "verificarSessao")]
        bool VerificarSessao();
    }

    /*
    [DataContract(Name = "Tarefa")]
    public class Tarefa
    {
        [DataMember(Name = "IdServer")]
        public int IdServer { set; get; }

        [DataMember(Name = "IdLocal")]
        public int IdLocal { set; get; }

        [DataMember(Name = "Descricao")]
        public string Descricao { set; get; }

        [DataMember(Name = "Observacao")]
        public string Observacao { set; get; }

        [DataMember(Name = "Data")]
        public DateTime Data { set; get; }

        [DataMember(Name = "Notificar")]
        public bool Notificar { set; get; }
    }
    */
    
}
