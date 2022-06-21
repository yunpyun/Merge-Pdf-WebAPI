using System;
using System.Collections.Generic;
using System.IO;
using GemBox.Document;
using GemBox.Pdf;

namespace MergePdfLib
{
    public class Merge
    {
        public string MergeDocs(List<string> docs)
        {
            // лицензирование GemBox
            GemBox.Document.ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            GemBox.Pdf.ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            // конвертация из исходного массива doc файлов в pdf
            for (int i = 0; i < docs.Count; i++)
            {
                if (docs[i].EndsWith(".doc") || docs[i].EndsWith(".docx"))
                {
                    docs[i] = SaveDoc(docs[i]);
                }
            }

            string tempFiles = MergeFiles(docs);

            return tempFiles;
        }

        static string SaveDoc(string destFileName)
        {
            DocumentModel documentDoc = DocumentModel.Load(destFileName);
            string newNameTemp = Path.GetTempFileName().Replace(".tmp", ".pdf");
            documentDoc.Save(newNameTemp);
            return newNameTemp;
        }

        static string MergeFiles(List<string> fileNames)
        {
            string newNameFilesTemp = Path.GetTempFileName().Replace(".tmp", ".pdf");

            using (PdfDocument documentPdf = new PdfDocument())
            {
                foreach (var fileName in fileNames)
                    using (var source = PdfDocument.Load(fileName))
                        documentPdf.Pages.Kids.AddClone(source.Pages);

                documentPdf.Save(newNameFilesTemp);
            }

            return newNameFilesTemp;
        }

        public int CountPages(string file)
        {
            GemBox.Pdf.ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            int counter = 0;

            using (var document = PdfDocument.Load(file))
            {
                foreach (var page in document.Pages)
                {
                    counter++;
                }
            }

            return counter;
        }
    }
}
