using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;
using ToDoWSWCFData;
using System.Data.SqlClient;

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
                t.Id = Convert.ToInt32(dr["id"]);
                t.Id_usuario = Convert.ToInt32(dr["id_usuario"]);
                t.Nome = dr["descricao"].ToString();
                t.Observacao = dr["observacao"].ToString();
                t.DataFinalizacao = dr["data"].ToString();
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

        #region SincronizarTarefas
        public static bool SincronizarTarefas(List<Tarefa> listaTarefa)
        {
            SqlConnection conn = FabricaConexao.AbrirConexao();
            SqlTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                foreach (Tarefa t in listaTarefa)
                {
                    /*
                        Verificar se a tarefa existe na base do servidor pelo idLocal da tarefa e o id_usuario
                        Caso exista atualizar a tarefa
                        Caso não exista deverá ser feita inclusão da tarefa
                    */
                    if (ObterTarefa(t.Id_usuario, t.Id).Rows.Count > 0)
                    {
                        AlterarTarefa(t, trans);
                    }
                    else
                    {
                        InserirTarefa(t, trans);
                    }
                }
                trans.Commit();

            }
            catch(Exception ex)
            {
                trans.Rollback();
                FabricaConexao.FecharConexao(conn);
                throw new Exception(ex.Message);
            }

            FabricaConexao.FecharConexao(conn);
            return true;
        }
        #endregion

        #region InserirTarefa
        public static void InserirTarefa(Tarefa f, SqlTransaction trans)
        {

            Hashtable htParametro = new Hashtable();

            if (f.Id_usuario != null)
            {
                htParametro["@id_usuario"] = f.Id_usuario;
            }
            else
            {
                htParametro["@id_usuario"] = string.Empty;
            }

            if (f.Id != null)
            {
                htParametro["@id_local"] = f.Id;
            }
            else
            {
                htParametro["@id_local"] = string.Empty;
            }

            if (f.Notificar != null)
            {
                htParametro["@notificar"] = f.Notificar;
            }
            else
            {
                htParametro["@notificar"] = string.Empty;
            }

            if (f.DataFinalizacao != null)
            {
                htParametro["@data"] = f.DataFinalizacao;
            }
            else
            {
                htParametro["@data"] = string.Empty;
            }

            if (f.Observacao != null)
            {
                htParametro["@observacao"] = f.Observacao;
            }
            else
            {
                htParametro["@observacao"] = string.Empty;
            }

            if (f.Nome != null)
            {
                htParametro["@descricao"] = f.Nome;
            }
            else
            {
                htParametro["@descricao"] = string.Empty;
            }

            string sql = @"
                            insert into tb_tarefa
                             (
	                             id_local
	                            ,id_usuario
	                            ,descricao
	                            ,observacao
	                            ,data
	                            ,notificar
                            )
                            values
                            (
	                             @id_local
	                            ,@id_usuario
	                            ,@descricao
	                            ,@observacao
	                            ,@data
	                            ,@notificar
                            )
                            ";
            FabricaConexao.ExecuteNonQuery(sql, htParametro, trans);
        }
        #endregion

        #region AlterarTarefa
        public static void AlterarTarefa(Tarefa f, SqlTransaction trans)
        {
            Hashtable htParametro = new Hashtable();

            if (f.Id_usuario != null)
            {
                htParametro["@id_usuario"] = f.Id_usuario;
            }
            else
            {
                htParametro["@id_usuario"] = string.Empty;
            }

            if (f.Id != null)
            {
                htParametro["@id_local"] = f.Id;
            }
            else
            {
                htParametro["@id_local"] = string.Empty;
            }

            if (f.Notificar != null)
            {
                htParametro["@notificar"] = f.Notificar;
            }
            else
            {
                htParametro["@notificar"] = string.Empty;
            }

            if (f.DataFinalizacao != null)
            {
                htParametro["@data"] = f.DataFinalizacao;
            }
            else
            {
                htParametro["@data"] = string.Empty;
            }

            if (f.Observacao != null)
            {
                htParametro["@observacao"] = f.Observacao;
            }
            else
            {
                htParametro["@observacao"] = string.Empty;
            }

            if (f.Nome != null)
            {
                htParametro["@descricao"] = f.Nome;
            }
            else
            {
                htParametro["@descricao"] = string.Empty;
            }

            string sql = @"
                           update tb_tarefa
                            set
	                             descricao = @descricao
	                            ,observacao = @observacao
	                            ,data = @data
	                            ,notificar = @notificar
                            where 
                             id_local = @id_local
                             and id_usuario = @id_usuario
                            ";
            FabricaConexao.ExecuteNonQuery(sql, htParametro, trans);
        }
        #endregion

        #region ObterTarefa
        public static DataTable ObterTarefa(int cdUsuario, int idLocal)
        {
            Hashtable htParametro = new Hashtable();
            htParametro["@id_usuario"] = cdUsuario;
            htParametro["@idLocal"] = idLocal;

            string sql = @"select * from tb_tarefa (nolock) where id_usuario = @id_usuario and id = @idLocal";

            DataTable dtt = FabricaConexao.ExecuteQuery(sql, htParametro);

            return dtt;
        }
        #endregion
    }
}