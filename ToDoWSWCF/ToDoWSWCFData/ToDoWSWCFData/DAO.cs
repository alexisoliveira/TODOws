using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;
using ToDoWSWCFData;

namespace ToDoWSWCFData
{
    public static class DAO
    {
        #region ObterTarefas
        public static List<Tarefa> ObterTarefas(int cdUsuario)
        {
            Hashtable htParametro = new Hashtable();
            htParametro["@id_usuario"] = cdUsuario;

            string sql = @"select * from tb_tarefa (nolock) where id_usuario = @id_usuario";

            DataTable dtt = FabricaConexao.ExecuteQuery(sql, htParametro);

            List<Tarefa> retorno = new List<Tarefa>();
            foreach (DataRow dr in dtt.Rows)
            {
                Tarefa t = new Tarefa();
                t.IdServer = Convert.ToInt32(dr["id_server"]);
                t.IdLocal = Convert.ToInt32(dr["id_local"]);
                //t.IdUsuario = Convert.ToInt32(dr["id_usuario"]);
                t.Descricao = dr["descricao"].ToString();
                t.Observacao = dr["observacao"].ToString();
                t.Data = Convert.ToDateTime(dr["data"]);
                t.Notificar = Convert.ToBoolean(dr["notificar"]);

                retorno.Add(t);
            }

            return retorno;
        }
        #endregion

        #region ObterUsuario
        public static DataTable ObterUsuario(string nrTelefone, string dsSenha)
        {
            Hashtable htParametro = new Hashtable();
            htParametro["@nr_telefone"] = nrTelefone;
            htParametro["@ds_senha"] = Util.GetMD5Hash(dsSenha);

            string sql = @"select * from tb_usuario (nolock) where nr_telefone = @nr_telefone and ds_senha = @ds_senha";

            return FabricaConexao.ExecuteQuery(sql, htParametro);
        }
        #endregion

        #region ObterUsuario
        public static DataTable ObterUsuario(string nrTelefone)
        {
            Hashtable htParametro = new Hashtable();
            htParametro["@nr_telefone"] = nrTelefone;

            string sql = @"select * from tb_usuario (nolock) where nr_telefone = @nr_telefone";

            return FabricaConexao.ExecuteQuery(sql, htParametro);
        }
        #endregion

        #region CadastrarUsuario
        public static void CadastrarUsuario(string nrTelefone, string dsSenha, string dsEmail)
        {
            Hashtable htParametro = new Hashtable();

            if (nrTelefone != null)
            {
                htParametro["@nr_telefone"] = nrTelefone;
            }
            else
            {
                htParametro["@nr_telefone"] = string.Empty;
            }

            if (dsSenha != null)
            {
                htParametro["@ds_senha"] = Util.GetMD5Hash(dsSenha);
            }
            else
            {
                htParametro["@ds_senha"] = string.Empty;
            }

            if (dsEmail != null)
            {
                htParametro["@ds_email"] = dsEmail;
            }
            else
            {
                htParametro["@ds_email"] = string.Empty;
            }

            string sql = @"
                            insert into tb_usuario
                            (
	                            ds_email
	                            ,nr_telefone
	                            ,ds_senha
                            )
                            values
                            (
	                            @ds_email
	                            ,@nr_telefone
	                            ,@ds_senha
                            )";

            FabricaConexao.ExecuteNonQuery(sql, htParametro, false);
        }
        #endregion

        #region AtualizarUsuario
        public static void AtualizarUsuario(string nrTelefone, string dsSenha, string dsEmail)
        {
            Hashtable htParametro = new Hashtable();
            if (nrTelefone != null)
            {
                htParametro["@nr_telefone"] = nrTelefone;
            }
            else
            {
                htParametro["@nr_telefone"] = string.Empty;
            }

            if (dsSenha != null)
            {
                htParametro["@ds_senha"] = Util.GetMD5Hash(dsSenha);
            }
            else
            {
                htParametro["@ds_senha"] = string.Empty;
            }

            if (dsEmail != null)
            {
                htParametro["@ds_email"] = dsEmail;
            }
            else
            {
                htParametro["@ds_email"] = string.Empty;
            }

            string sql = @"
                            update tb_usuario
                            set 
                                ds_email = @ds_email,
                                ds_senha = @ds_senha
                            where nr_telefone = @nr_telefone
                            ";

            FabricaConexao.ExecuteNonQuery(sql, htParametro, false);
        }
        #endregion
    }
}