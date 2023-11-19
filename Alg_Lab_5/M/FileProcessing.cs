using Alg_Lab_5.M.FolderGraph;
using System;
using System.Collections.Generic;
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
    }
}
