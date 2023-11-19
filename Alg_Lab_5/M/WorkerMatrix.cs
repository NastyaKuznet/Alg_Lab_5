using Alg_Lab_5.M.FolderGraph;
using Alg_Lab_5.VM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Alg_Lab_5.M
{
    public class WorkerMatrix
    {
        public DataTable CreateMatrix(LinkedList<NodeGraph> nodes)
        {
            DataTable tempTable = new DataTable();
            AddColumns(nodes, tempTable);
            AddRow(nodes, tempTable);
            return tempTable;
        }

        private void AddColumns(LinkedList<NodeGraph> nodes, DataTable tempTable)
        {
            tempTable.Columns.Add("=");
            foreach (NodeGraph node in nodes)
            {
                tempTable.Columns.Add(node.Name);
            }
        }

        private void AddRow(LinkedList<NodeGraph> nodes, DataTable tempTable)
        {
            //foreach (NodeGraph node in nodes)
            //{
            //    DataRow newRow = tempTable.NewRow();
            //    int ind = 0;
            //    //foreach (string cell in row.ItemArray)
            //    //{
            //    //    newRow[ind] = cell;
            //    //    ind++;
            //    //}
            //    newRow[ind] = 0;
            //    tempTable.Rows.Add(newRow);
            //}
            foreach (NodeGraph node in nodes)
            {
                DataRow addRow = tempTable.NewRow();
                addRow[0] = node.Name;
                //for (int i = 1; i < tempTable.Columns.Count; i++)
                //{
                //    addRow[i] = 0;
                //}
                tempTable.Rows.Add(addRow);
            }
        }
    }
}
