using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2020
{
    class Library
    {
        private List<int> books;
        private List<int> procBooks;
        private int sigupTime;
        private int performance;
        private bool processed;
        private bool signuping;
        private bool processing;
        private int remDays;
        private int id;
        
        public int SigupTime { get => sigupTime; set => sigupTime = value; }
        public int Performance { get => performance; set => performance = value; }
        internal List<int> Books { get => books; set => books = value; }
        public bool Processed { get => processed; set => processed = value; }
        public bool Signuping { get => signuping; set => signuping = value; }
        public int RemDays { get => remDays; set => remDays = value; }
        public bool Processing { get => processing; set => processing = value; }
        public List<int> ProcBooks { get => procBooks; set => procBooks = value; }
        public int Id { get => id; set => id = value; }

        public Library(int books, int sigupTime, int performance, int id)
        {
            this.Books = new List<int>();
            this.ProcBooks = new List<int>();
            this.sigupTime = sigupTime;
            this.performance = performance;
            this.processed = false;
            this.signuping = false;
            this.remDays = sigupTime;
            this.processing = false;
            this.id = id;
        }
    }
}
