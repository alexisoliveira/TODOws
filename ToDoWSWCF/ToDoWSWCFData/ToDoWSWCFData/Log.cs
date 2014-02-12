using System;
using System.IO;

namespace ToDoWSWCFData
{
    public static class Log
    {
        public static void registrarEvento(string texto)
        {
            string nomeArquivo = @"c:\logs\log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            StreamWriter writer = new StreamWriter(nomeArquivo, true);
            writer.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm") + " - " + texto);
            writer.Close();
        }
    }
}