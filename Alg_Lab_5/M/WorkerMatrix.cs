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
            Drawer drawer = new Drawer();
            foreach (NodeGraph node in nodes)
            {
                DataRow addRow = tempTable.NewRow();
                addRow[0] = node.Name;
                foreach(Edge edge in node.Edges)
                {
                    if (edge.Type.Equals(TypeEdge.Base) && edge.Weight == 0)
                    {
                        NodeGraph node1 = drawer.FindNodeInTouch(nodes, edge.FirstPosX, edge.FirstPosY);
                        NodeGraph node2 = drawer.FindNodeInTouch(nodes, edge.SecondPosX, edge.SecondPosY);
                        if (node1.Equals(node))
                        {
                            addRow[node2.Name] = 1;
                        }
                        else if (node2.Equals(node))
                        {
                            addRow[node1.Name] = 1;
                        }
                    }
                    else if (edge.Type.Equals(TypeEdge.Base) && edge.Weight != 0)
                    {
                        NodeGraph node1 = drawer.FindNodeInTouch(nodes, edge.FirstPosX, edge.FirstPosY);
                        NodeGraph node2 = drawer.FindNodeInTouch(nodes, edge.SecondPosX, edge.SecondPosY);
                        if (node1.Equals(node))
                        {
                            addRow[node2.Name] = edge.Weight;
                        }
                        else if (node2.Equals(node))
                        {
                            addRow[node1.Name] = edge.Weight;
                        }
                    }
                    else if (edge.Type.Equals(TypeEdge.Directed) && edge.Weight == 0)
                    {
                        NodeGraph node1 = drawer.FindNodeInTouch(nodes, edge.FirstPosX, edge.FirstPosY);
                        NodeGraph node2 = drawer.FindNodeInTouch(nodes, edge.SecondPosX, edge.SecondPosY);
                        if (node1.Equals(node))
                        {
                            addRow[node2.Name] = 1;
                        }
                    }
                    else if (edge.Type.Equals(TypeEdge.Directed) && edge.Weight != 0)
                    {
                        NodeGraph node1 = drawer.FindNodeInTouch(nodes, edge.FirstPosX, edge.FirstPosY);
                        NodeGraph node2 = drawer.FindNodeInTouch(nodes, edge.SecondPosX, edge.SecondPosY);
                        if (node1.Equals(node))
                        {
                            addRow[node2.Name] = edge.Weight;
                        }
                    }
                }
                for(int i = 0; i < addRow.ItemArray.Length; i++)
                {
                    if (!int.TryParse(addRow[i].ToString(), out int trash))
                    {
                        addRow[i] = 0;
                    }
                }
                tempTable.Rows.Add(addRow);
            }
        }
    }
}
