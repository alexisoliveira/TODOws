using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcessoDados;
using System.Collections;
using System.Data;
using ToDoWSWCFData;

namespace ToDoWSWCF
{
    public static class DAO
    {
        #region ObterTarefas
        /// <summary>
        /// Obtém as tarefas do usuário
        /// </summary>
        /// <param name="cdUsuario"></param>
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
        /// <summary>
        /// Efetuar o login do usuário
        /// </summary>
        /// <param name="nrTelefone"></param>
        /// <param name="dsSenha"></param>
        public static DataTable ObterUsuario(string nrTelefone, string dsSenha)
        {
            Hashtable htParametro = new Hashtable();
            htParametro["@nr_telefone"] = nrTelefone;
            htParametro["@ds_senha"] = Util.GetMD5Hash(dsSenha);

            string sql = @"select * from tb_usuario (nolock) where nr_telefone = @nr_telefone and ds_senha = @ds_senha";

            return FabricaConexao.ExecuteQuery(sql, htParametro);
        }
        #endregion

        #region InserirUsuario
        /// <summary>
        /// Efetua o cadastro de um usuário
        /// </summary>
        /// <param name="nrTelefone"></param>
        /// <param name="dsSenha"></param>
        public static void InserirUsuario(string nrTelefone, string dsSenha, string dsEmail)
        {
            Hashtable htParametro = new Hashtable();
            htParametro["@nr_telefone"] = nrTelefone;
            htParametro["@ds_senha"] = Util.GetMD5Hash(dsSenha);
            htParametro["@ds_email"] = dsEmail;

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
    }
}