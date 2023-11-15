using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Alg_Lab_5.M
{
    public class ManagerPages
    {
        private LinkedList<Page> pages = new LinkedList<Page>();
        private LinkedListNode<Page> currentPage;

        public ManagerPages(List<Page> pages)
        {
            foreach (Page page in pages)
            {
                this.pages.AddLast(page);
            }
            currentPage = this.pages.First;
        }

        public Page NextPage()
        {
            currentPage = currentPage.Next;
            return currentPage.Value;
        }

        public Page LastPage()
        {
            currentPage = currentPage.Previous;
            return currentPage.Value;
        }

        public bool CanNext()
        {
            return currentPage.Next != null;
        }

        public bool CanUndo()
        {
            return currentPage.Previous != null;
        }

        public Page GetCurrentPage()
        {
            return currentPage.Value;
        }
    }
}
