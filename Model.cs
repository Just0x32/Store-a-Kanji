using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store_a_Kanji
{
    class Model
    {
        private readonly string outputFilePath = @"output.txt";

        public Model()
        {

        }

        public bool isSuccessfulWrite { get; private set; } = false;

        public void StoreWords(string kanji, string hiragana, string translate)
        {

        }

        private void CheckOutputFile()
        {

        }

        private void WriteToFile(string kanji, string hiragana, string translate)
        {

        }
    }
}
