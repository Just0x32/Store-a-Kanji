using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS0168

namespace Store_a_Kanji
{
    class Model
    {
        private readonly string outputFilePath = @"output.txt";

        private StreamWriter streamWriter;

        public Model() => CheckOutputFileOnStart(outputFilePath);

        public bool isUnsuccessfulFileCreating { get; private set; } = false;

        public bool isUnsuccessfulWrite { get; private set; } = false;

        public bool wasFileDeletedOnStoring { get; private set; } = false;

        public void StoreWords(string kanji, string hiragana, string translate)
        {
            if (DoesOutputFileExistOnStoring(outputFilePath))
                WriteToFile(outputFilePath, kanji, hiragana, translate);

            bool DoesOutputFileExistOnStoring(string path)
            {
                if (File.Exists(path))
                {
                    return true;
                }
                else
                {
                    wasFileDeletedOnStoring = true;
                    return false;
                }
            }
        }

        private void CheckOutputFileOnStart(string path)
        {
            if (!File.Exists(path))
                CreateFile(path);

            void CreateFile(string path)
            {
                try
                {
                    using (File.Create(path)) { };
                }
                catch (IOException e)
                {
                    isUnsuccessfulFileCreating = true;
                }
            }
        }

        private void WriteToFile(string path, string kanji, string hiragana, string translate)
        {
            try
            {
                streamWriter = new StreamWriter(path, true);
                streamWriter.WriteLine(kanji + "*" + hiragana + "*" + translate);
            }
            catch (IOException e)
            {
                isUnsuccessfulWrite = true;
            }
            finally
            {
                streamWriter?.Dispose();
            }
        }
    }
}
