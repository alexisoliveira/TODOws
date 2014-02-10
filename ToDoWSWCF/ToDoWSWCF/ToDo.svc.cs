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
using ToDoWSWCFData;


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

                DataTable dttUsuario = DAO.ObterUsuario(nrTelefone, dsDenha);
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
        public Tarefa[] GetTarefas()
        {
            try
            {
                Log.registrarEvento("INICIO - METODO GET TAREFA");
                if (HttpContext.Current.Session["SessionState"] == null)
                {
                    Log.registrarEvento("SESSAO INVALIDA");
                    return null;
                }
                else
                {
                    SessionState state = (SessionState)HttpContext.Current.Session["SessionState"];
                    Tarefa[] tarefa = DAO.ObterTarefas(state.idUsuario).ToArray();

                    Log.registrarEvento("FIM - METODO GET TAREFA");
                    return tarefa;
                }
            }
            catch (Exception ex)
            {
                Log.registrarEvento("ERRO: " + ex.Message);
                return null;
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
        public bool Sincronizar(Stream stream)
        {
            if (HttpContext.Current.Session["SessionState"] == null)
            {
                return false;
            }
            else
            {
                try
                {
                    NameValueCollection coll = HttpUtility.ParseQueryString(new StreamReader(stream).ReadToEnd());
                    string nome = coll["nome"];
                    string email = coll["email"];
                    string idade = coll["idade"];

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

        #region InserirUsuario
        /// <summary>
        /// Método responsável por inserir usuário no servidor
        /// </summary>
        /// <remarks>
        /// Criado por: Alexis de A. Oliveira
        /// Data: 09/02/2014
        /// </remarks>
        /// <returns></returns>
        public bool CadastrarUsuario(Usuario usuario)
        {
            try
            {
                Log.registrarEvento("INICIO - METODO INSERIR USUARIO");
                
                /*
                NameValueCollection coll = HttpUtility.ParseQueryString(new StreamReader(stream).ReadToEnd());
                string nrTelefone = coll["nrTelefone"];

                Log.registrarEvento("PARAM"+ nrTelefone);

                string dsSenha = coll["dsSenha"];

                Log.registrarEvento("PARAM" + dsSenha);

                string dsEmail = coll["dsEmail"];

                Log.registrarEvento("PARAM" + dsEmail);
                DAO.InserirUsuario(nrTelefone, dsSenha, dsEmail);

                Log.registrarEvento("FIM - METODO INSERIR USUARIO");
                 */ 
                return true;
            }
            catch(Exception ex)
            {
                Log.registrarEvento("ERRO: " + ex.Message);
                return false;
            }
        }
        #endregion
    }
}