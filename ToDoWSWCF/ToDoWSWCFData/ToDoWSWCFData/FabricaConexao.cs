using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AcessoDados
{
    public class FabricaConexao
    {
        private static SqlConnection  AbrirConexao()
        {
            string conexao = String.Format(@"Server=5258ab4c-e9a4-4d26-b163-a2c9012a3ec7.sqlserver.sequelizer.com;Database=db5258ab4ce9a44d26b163a2c9012a3ec7;User ID=jntszfuasqubjhrk;Password=r8yrwk7pqdLxvbGMH3hxaDtJZULnxnfGeursCkijFDA2HxMQekABksyVNXzJGSYc;");     
            SqlConnection conn = new SqlConnection(conexao);

            conn.Open();

            return conn;
        }

        private static void FecharConexao(SqlConnection conn)
        {
            conn.Close();
        }

        public static DataTable ExecuteQuery(string sql, Hashtable htParametro)
        {

            SqlConnection conn = AbrirConexao();
            SqlCommand command = new SqlCommand(sql, conn);

            if (htParametro != null)
            {
                IDictionaryEnumerator dic = htParametro.GetEnumerator();
                while (dic.MoveNext())
                {
                    command.Parameters.Add(dic.Key.ToString(), dic.Value.ToString());
                }
            }

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            
            
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            FecharConexao(conn);

            return dt;
        }

        public static void ExecuteNonQuery(string sql, Hashtable htParametro, bool usarTransacao)
        {
            SqlConnection conn = AbrirConexao();
            SqlTransaction trans = null;
            try
            {
                
                if (usarTransacao)
                {
                    trans = conn.BeginTransaction();
                }
                SqlCommand command = new SqlCommand(sql, conn, trans);

                if (htParametro != null)
                {
                    IDictionaryEnumerator dic = htParametro.GetEnumerator();
                    while (dic.MoveNext())
                    {
                        command.Parameters.Add(dic.Key.ToString(), dic.Value.ToString());
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                command.ExecuteNonQuery();
                if (usarTransacao)
                {
                    trans.Commit();
                }
            }
            catch
            {
                trans.Rollback();
                FecharConexao(conn);
                throw new Exception();
            }

            FecharConexao(conn);

        }
    }
}
