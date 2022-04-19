using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


            string fileName = @"C:\Users\Выймова Елена\Documents\MergeFilesStream.pdf";

            var tempFiles = Path.GetTempFileName().Replace(".tmp", ".pdf");

            MergeFiles(docs, tempFiles);

            return tempFiles;

            //using (var fileStream = File.OpenWrite(fileName))
            //{
            // объединение pdf файлов
            //MergeFiles(docs, fileStream);

            //return fileStream;
            //}

            //return @"C:\Users\Выймова Елена\Documents\MergeFiles.pdf";
        }

        static string SaveDoc(string destFileName)
        {
            DocumentModel documentDoc = DocumentModel.Load(destFileName);
            string newName = @"C:\Users\Выймова Елена\Documents\OutputDocMerge.pdf";
            documentDoc.Save(newName);
            return newName;
        }

        static void MergeFiles(List<string> fileNames, string tempFiles)
        {
            using (PdfDocument documentPdf = new PdfDocument())
            {
                foreach (var fileName in fileNames)
                    using (var source = PdfDocument.Load(fileName))
                        documentPdf.Pages.Kids.AddClone(source.Pages);

                documentPdf.Save(tempFiles);
                //documentPdf.Save(@"C:\Users\Выймова Елена\Documents\MergeFiles.pdf");
            }
        }
    }
}
