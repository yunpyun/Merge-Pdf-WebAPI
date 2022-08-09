using System;
using System.Collections.Generic;
using System.IO;
using GemBox.Document;
using GemBox.Pdf;

namespace MergePdfLib
{
    public class Merge
    {
        /// <summary>
        /// Функция, вызываемая из api. Проверяет, есть ли doc-файлы и возвращает путь с объединенным pdf-файлом
        /// </summary>
        /// <param name="docs">Список путей до входных файлов для объединения</param>
        /// <returns>Путь до объединенного pdf-файла</returns>
        public string MergeDocs(List<string> docs)
        {
            // лицензирование GemBox
            string doc_key = Environment.GetEnvironmentVariable("GEMBOX_ELMA365_KEY_DOC") != null
                ? Environment.GetEnvironmentVariable("GEMBOX_ELMA365_KEY_DOC")
                : "FREE-LIMITED-KEY";

            string pdf_key = Environment.GetEnvironmentVariable("GEMBOX_ELMA365_KEY_PDF") != null
                ? Environment.GetEnvironmentVariable("GEMBOX_ELMA365_KEY_PDF")
                : "FREE-LIMITED-KEY";

            GemBox.Document.ComponentInfo.SetLicense(doc_key);
            GemBox.Pdf.ComponentInfo.SetLicense(pdf_key);

            // конвертация из исходного массива doc-файлов в pdf-файлы
            for (int i = 0; i < docs.Count; i++)
            {
                if (docs[i].EndsWith(".doc") || docs[i].EndsWith(".docx"))
                {
                    docs[i] = ConvertDocToPdf(docs[i]);
                }
            }

            // объединение массива файлов в один файл и получение его пути
            string tempFiles = MergeFiles(docs);

            return tempFiles;
        }

        /// <summary>
        /// Функция для конвертации doc-файлов в pdf
        /// </summary>
        /// <param name="destFileName">Путь до файла, который нужно конвертировать</param>
        /// <returns>Путь до pdf-файла, сконвертированного из doc</returns>
        static string ConvertDocToPdf(string destFileName)
        {
            // загрузка doc-файла
            DocumentModel documentDoc = DocumentModel.Load(destFileName);
            // формирование пути для pdf-файла
            string newNameTemp = Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
            // конвертация doc в pdf
            documentDoc.Save(newNameTemp);

            // удаление из временной папки уже ненужного doc-файла
            FileInfo fileInf = new FileInfo(destFileName);
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }

            return newNameTemp;
        }

        /// <summary>
        /// Функция для объединения pdf-файлов в один
        /// </summary>
        /// <param name="fileNames">Список путей до файлов, которые нужно объединить</param>
        /// <returns>Путь до объединенного pdf-файла</returns>
        static string MergeFiles(List<string> fileNames)
        {
            // формирование пути для сохранения итогового файла
            string newNameFilesTemp = Path.GetTempPath() + Guid.NewGuid().ToString() + "-MergedPdf" + ".pdf";

            // объединение pdf-файлов в один документ
            using (PdfDocument documentPdf = new PdfDocument())
            {
                foreach (var fileName in fileNames)
                {
                    using var source = PdfDocument.Load(fileName);
                    documentPdf.Pages.Kids.AddClone(source.Pages);
                }

                documentPdf.Save(newNameFilesTemp);
            }

            return newNameFilesTemp;
        }

        /// <summary>
        /// Функция, вызываемая из api. Для подсчета страниц в документе
        /// </summary>
        /// <param name="file">Путь до pdf-файла, в котором нужно посчитать страницы</param>
        /// <returns>Количество страниц в файле</returns>
        public int CountPages(string file)
        {
            string pdf_key = Environment.GetEnvironmentVariable("GEMBOX_ELMA365_KEY_PDF") != null 
                ? Environment.GetEnvironmentVariable("GEMBOX_ELMA365_KEY_PDF") 
                : "FREE-LIMITED-KEY";

            GemBox.Pdf.ComponentInfo.SetLicense(pdf_key);

            // загрузка pdf-документа
            using var document = PdfDocument.Load(file);
            return document.Pages.Count;
        }
    }
}
