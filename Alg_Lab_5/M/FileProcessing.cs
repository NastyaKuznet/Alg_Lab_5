using Alg_Lab_5.M.FolderGraph;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Alg_Lab_5.M
{
    public class FileProcessing
    {
        public void SaveGraph(string pathFolder, Graph graph)
        {
            string json = JsonSerializer.Serialize(graph);
            File.WriteAllText(pathFolder + "\\structGraph.json", json);
        }

        public Graph OpenGraph(string pathFolder) 
        {
            Graph graph = new Graph();
            foreach (string nameFile in Directory.EnumerateFiles(pathFolder))
            {
                if(nameFile.Contains("structGraph.json"))
                {
                    string content = File.ReadAllText(nameFile);
                    graph = JsonSerializer.Deserialize<Graph>(content);
                }
            }
            return graph;
        }

        public void SaveMatrix(string pathFolder, DataTable dataTable)
        {
            StringBuilder contentFile = new StringBuilder();
            StringBuilder str = new StringBuilder();
            foreach(DataColumn column in dataTable.Columns)
            {
                str.Append(column.ColumnName + ";");
            }
            contentFile.AppendLine(str.ToString());
            str.Clear();
            foreach(DataRow row in dataTable.Rows) 
            {
                for(int i = 0; i < row.ItemArray.Length; i++)
                {
                    str.Append(row[i].ToString() + ";");
                }
                contentFile.AppendLine(str.ToString());
                str.Clear();
            }
            File.WriteAllText(pathFolder + "\\MatrixAdjacency.csv", contentFile.ToString());
        }
    }
}
