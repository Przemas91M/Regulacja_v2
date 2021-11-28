using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Regulacja_v2
{
    public class PDF
    {
        public static void UtworzPDF()
        {
            Document doc = new Document();
            string path = Directory.GetCurrentDirectory().ToString();
            PdfWriter.GetInstance(doc, new FileStream(path + "\\Doc1.pdf", FileMode.Create));
            doc.Open();
            doc.Add(new Paragraph("HELLO WORLD"));
            doc.Close();
        }
    }
}
