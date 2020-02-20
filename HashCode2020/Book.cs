using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2020
{
    class Book
    {
        private int score;

        public int Score { get => score; set => score = value; }

        public Book(int score)
        {
            this.score = score;
        }
    }
}
