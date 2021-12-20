using ClosedXML.Excel;
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
        private readonly string outputFilePath = @"output.xlsx";

        public Model() => CheckOutputFileOnStart(outputFilePath);

        public bool isUnsuccessfulFileCreating { get; private set; } = false;

        public bool isUnsuccessfulWrite { get; private set; } = false;

        public bool wasFileDeletedOnStoring { get; private set; } = false;

        public void StoreWords(string kanji, string hiragana, string translation)
        {
            if (DoesOutputFileExistOnStoring(outputFilePath))
                WriteToFile(outputFilePath, kanji, hiragana, translation);

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

            void CreateFile(string filePath)
            {
                using (var workbook = new XLWorkbook())
                {
                    workbook.Worksheets.Add();
                    var worksheet = workbook.Worksheets.First();

                    WriteCell("A1", "Kanji");
                    WriteCell("B1", "Hiragana");
                    WriteCell("C1", "Translation");

                    workbook.SaveAs(filePath);

                    void WriteCell(string cell, string value)
                    {
                        int column;

                        switch (cell[0])
                        {
                            case 'A':
                                column = 1;
                                break;
                            case 'B':
                                column = 2;
                                break;
                            case 'C':
                                column = 3;
                                break;
                            default:
                                column = 4;
                                break;
                        }

                        worksheet.Cell(cell).Value = value;
                        worksheet.Column(column).AdjustToContents();
                    }
                }

                //catch (IOException e)
                //{
                //    isUnsuccessfulFileCreating = true;
                //}
            }
        }

        private void WriteToFile(string filePath, string kanji, string hiragana, string translation)
        {
            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheets.First();

                int currentRow = worksheet.LastRowUsed().RowNumber() + 1;

                WriteCell($"A{currentRow}", kanji);
                WriteCell($"B{currentRow}", hiragana);
                WriteCell($"C{currentRow}", translation);

                workbook.Save();

                void WriteCell(string cell, string value)
                {
                    int column;

                    switch (cell[0])
                    {
                        case 'A':
                            column = 1;
                            break;
                        case 'B':
                            column = 2;
                            break;
                        case 'C':
                            column = 3;
                            break;
                        default:
                            column = 4;
                            break;
                    }

                    worksheet.Cell(cell).Value = value;
                    worksheet.Column(column).AdjustToContents();
                }
            }

            //catch (IOException e)
            //{
            //    isUnsuccessfulFileCreating = true;
            //}
        }
    }
}
