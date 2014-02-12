using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ToDoWSWCFData;

namespace ToDoWSWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IToDo" in both code and config file together.
    [ServiceContract]
    public interface IToDo
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "gettarefa")]
        List<Tarefa> GetTarefas();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "sincronizar")]
        bool Sincronizar(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "efetuarlogin")]
        bool EfetuarLogin(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "verificarsessao")]
        bool VerificarSessao();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "cadastraratualizarusuario")]
        bool CadastrarAtualizarUsuario(Stream stream);

    }
}
