using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.ServiceModel.Activation;
using System.Data;
using System.IO;
using System.Collections.Specialized;


namespace ToDoWSWCF
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ToDo : IToDo
    {
        #region EfetuarLogin
        /// <summary>
        /// Este método é responsável por realizar o login do usuário e criar uma sessão para que seja possível 
        /// a utilização das demais funções do webservice.
        /// </summary>
        /// <remarks>
        /// Criado por: Alexis de A. Oliveira
        /// Data: 07/02/2014
        /// </remarks>
        /// <param name="usuario"></param>
        /// <param name="senha"></param>
        /// <returns></returns>        
        public bool EfetuarLogin(string login, string senha)
        {
            SessionState state = (SessionState)HttpContext.Current.Session["SessionState"];
            if (state == null)
            {

                string nrTelefone = login;
                string dsDenha = senha;

                /*
                string nrTelefone = "7999999999";
                string dsDenha = "1312313";
                 */

                DataTable dttUsuario = new DAO().ObterUsuario(nrTelefone, dsDenha);
                if (dttUsuario.Rows.Count > 0)
                {
                    state = new SessionState();
                    state.idUsuario = Convert.ToInt32(dttUsuario.Rows[0]["id_usuario"]);
                    state.SessionId = HttpContext.Current.Session.SessionID;
                    HttpContext.Current.Session["SessionState"] = state;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                HttpContext.Current.Session.Clear();
                return false;
            }
        }
        #endregion

        #region GetTarefas
        public List<Tarefa> GetTarefas()
        {
            if (HttpContext.Current.Session["SessionState"] == null)
            {
                return null;
            }
            else
            {
                SessionState state = (SessionState)HttpContext.Current.Session["SessionState"];
                return new DAO().ObterTarefas(state.idUsuario);
            }
        }
        #endregion

        #region Sincronizar
        /// <summary>
        /// Este método receberá a lista de tarefas com o FL_SINCRONIZADO = 0
        /// e o WS deverá verificar o FL_OPERACAO para realizar as seguintes operações:
        ///     FL_OPERACAO = 'E' => excluir dados da base do servidor
        ///     FL_OPERACAO = 'A' => alterar dados na base do servidor
        ///     FL_OPERACAO = 'I' => incluir dados na base do servidor             
        /// </summary>
        /// <remarks>
        /// Criado por: Alexis de A. Oliveira
        /// Data: 07/02/2014
        /// </remarks>
        /// <param name="tarefa"></param>
        /// <returns></returns>
        public bool Sincronizar(List<Tarefa> tarefa)
        {
            if (HttpContext.Current.Session["SessionState"] == null)
            {
                return false;
            }
            else
            {
                try
                {

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        #endregion

        #region VerificarSessao
        /// <summary>
        /// Verifica se o usuário possui uma sessão ativa
        /// </summary>
        /// <remarks>
        /// Criado por: Alexis de A. Oliveira
        /// Data: 07/02/2014
        /// </remarks>
        /// <returns></returns>
        public bool VerificarSessao()
        {
            if (HttpContext.Current.Session["SessionState"] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}