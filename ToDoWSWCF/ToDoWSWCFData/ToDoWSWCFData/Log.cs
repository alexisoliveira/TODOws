using System;
using System.IO;

namespace ToDoWSWCFData
{
    public static class Log
    {
        public static void registrarEvento(string texto)
        {
            string nomeArquivo = @"c:\logs\log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            StreamWriter writer = new StreamWriter(nomeArquivo + " - " + DateTime.Now.ToString("dd/MM/yyyy hh:ss"), true);
            writer.WriteLine(texto);
            writer.Close();
        }
    }
}