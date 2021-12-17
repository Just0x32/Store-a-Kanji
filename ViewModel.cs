using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store_a_Kanji
{
    class ViewModel
    {
        private Model model;

        public ViewModel() => model = new Model();

        public bool isSuccessfulWrite { get => model.isSuccessfulWrite; }

        public void StoreWords(string kanji, string hiragana, string translate) => model.StoreWords(kanji, hiragana, translate);
    }
}
