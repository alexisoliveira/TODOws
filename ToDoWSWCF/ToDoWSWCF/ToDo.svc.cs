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
using System.Web.Script.Serialization;


namespace ToDoWSWCF
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ToDo : IToDo
    {
        #region EfetuarLogin
        public bool EfetuarLogin(Stream stream)
        {
            try
            {
                Log.registrarEvento("INICIO - METODO EFETUAR LOGIN");
                SessionState state = (SessionState)HttpContext.Current.Session["SessionState"];

                NameValueCollection coll = HttpUtility.ParseQueryString(new StreamReader(stream).ReadToEnd());
                string usuario = coll["usuario"];
                Usuario usuarioObj = new JavaScriptSerializer().Deserialize<Usuario>(usuario);

                DataTable dttUsuario = DAO.ObterUsuario(usuarioObj.Telefone, usuarioObj.Senha);
                if (dttUsuario.Rows.Count > 0)
                {
                    state = new SessionState();
                    state.idUsuario = Convert.ToInt32(dttUsuario.Rows[0]["id_usuario"]);
                    state.SessionId = HttpContext.Current.Session.SessionID;
                    HttpContext.Current.Session["SessionState"] = state;

                    Log.registrarEvento("Login efetuado com sucesso: "+ state.SessionId);
                    Log.registrarEvento("FIM - METODO EFETUAR LOGIN");
                    return true;

                }
                else
                {
                    Log.registrarEvento("Nao foi possivel realizar o login");
                    HttpContext.Current.Session.Clear();
                }
                Log.registrarEvento("FIM - METODO EFETUAR LOGIN");
                return false;
            }
            catch (Exception ex)
            {
                Log.registrarEvento("ERRO: " + ex.Message);
                HttpContext.Current.Session.Clear();
                return false;
            }
        }
        #endregion

        #region GetTarefas
        public List<Tarefa> GetTarefas()
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
                    List<Tarefa> tarefa = DAO.ObterTarefas(state.idUsuario);

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
        public bool Sincronizar(Stream stream)
        {
            try
            {
                Log.registrarEvento("INICIO - METODO Sincronizar");
                NameValueCollection coll = HttpUtility.ParseQueryString(new StreamReader(stream).ReadToEnd());
                string listaTarefa = coll["listaTarefa"];
                string usuario = coll["usuario"];
                List<Tarefa> listaTarefaObj = new JavaScriptSerializer().Deserialize<List<Tarefa>>(listaTarefa);
                Usuario usuarioObj = new JavaScriptSerializer().Deserialize<Usuario>(usuario);

                bool resultado = DAO.SincronizarTarefas(listaTarefaObj, usuarioObj);

                if (resultado)
                {
                    Log.registrarEvento("Sincronização realizada com sucesso. Qtd= " + listaTarefaObj.Count);
                }

                Log.registrarEvento("FIM - METODO Sincronizar");
                return resultado;
            }
            catch (Exception ex)
            {
                Log.registrarEvento("ERRO: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region VerificarSessao
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

        #region CadastrarAtualizarUsuario
        public bool CadastrarAtualizarUsuario(Stream stream)
        {
            try
            {
                Log.registrarEvento("INICIO - METODO CADASTRAR/ATUALIZAR USUARIO");
                NameValueCollection coll = HttpUtility.ParseQueryString(new StreamReader(stream).ReadToEnd());
                string usuario = coll["usuario"];
                Usuario usuarioObj = new JavaScriptSerializer().Deserialize<Usuario>(usuario);

                DataTable dttUsuario = DAO.ObterUsuario(usuarioObj.Telefone);
                if (dttUsuario.Rows.Count > 0)//Atualizar
                {
                    DAO.AtualizarUsuario(usuarioObj.Telefone, usuarioObj.Senha, usuarioObj.Email);
                }
                else //Cadastrar
                {
                    DAO.CadastrarUsuario(usuarioObj.Telefone, usuarioObj.Senha, usuarioObj.Email);
                }

                Log.registrarEvento("FIM - METODO CADASTRAR/ATUALIZAR USUARIO");
                return true;
            }
            catch (Exception ex)
            {
                Log.registrarEvento("ERRO: " + ex.Message);
                return false;
            }
        }
        #endregion
    }
}