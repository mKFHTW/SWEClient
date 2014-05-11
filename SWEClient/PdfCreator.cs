using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.fonts;
namespace SWEClient
{
    class PdfCreator
    {
        public static void Write(Models.Rechnung Rechnung)
        {
            string path = @"rechnung.pdf";
            FileStream stream = new FileStream(path, FileMode.Create);
            double zwischensumme = 0;

            // step 1
            using (Document document = new Document(iTextSharp.text.PageSize.A4))
            {
                // step 2
                PdfWriter.GetInstance(document, stream);
                // step 3
                document.Open();
                // step 4
                Font font1 = new Font(Font.FontFamily.TIMES_ROMAN, 24f);
                Font font2 = new Font(Font.FontFamily.TIMES_ROMAN, 10f);

                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                Paragraph paragraphKundID = new Paragraph("Kundennummer: "+ Rechnung.KundenID, font2);
                paragraphKundID.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraphKundID);
                Paragraph paragraphKundName = new Paragraph("Kundenname: "+ Rechnung.Kundenname, font2);
                paragraphKundName.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraphKundName);

                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                Paragraph paragraphUeberschrift = new Paragraph("Rechnung", font1);
                paragraphUeberschrift.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraphUeberschrift);

                Paragraph paragraphRechDate = new Paragraph("Datum: "+ Rechnung.Datum.ToShortDateString(), font2);
                paragraphRechDate.Alignment = Element.ALIGN_RIGHT;
                document.Add(paragraphRechDate);
                Paragraph paragraphFaelligDate = new Paragraph("Fälligkeitsdatum: "+ Rechnung.Due.ToShortDateString(), font2);
                paragraphFaelligDate.Alignment = Element.ALIGN_RIGHT;
                document.Add(paragraphFaelligDate);
                Paragraph paragraphRechNum = new Paragraph("Rechnungsnummer: " + Rechnung.ID, font2);
                paragraphRechNum.Alignment = Element.ALIGN_RIGHT;
                //paragraphRechNum.Leading = 40f;

                document.Add(paragraphRechNum);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);


                PdfPTable tab = new PdfPTable(4);
                /*dfPCell cell = new PdfPCell(new Phrase("Rechnung",
                                     new Font(Font.FontFamily.HELVETICA, 22F)));
                 cell.Colspan = 3;
                 cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                 //Style
                 // cell.BorderColor = new BaseColor(System.Drawing.Color.Red);
                 cell.Border = 0; // | Rectangle.TOP_BORDER;
                 //cell.BorderWidthBottom = 3f;
                 tab.AddCell(cell);*/
                //row 1
                tab.AddCell(new PdfPCell(new Phrase("Anzahl", font2))); ;
                tab.AddCell(new PdfPCell(new Phrase("Artikel", font2)));
                tab.AddCell(new PdfPCell(new Phrase("Einzelpreis", font2)));
                tab.AddCell(new PdfPCell(new Phrase("Gesammtpreis", font2)));

                PdfPCell cell = new PdfPCell(new Phrase("   "));
                cell.Colspan = 4;
                cell.Border = 0;
                tab.AddCell(cell);

                foreach (var item in Rechnung.Zeilen)
                {
                    tab.AddCell(new PdfPCell(new Phrase(item.Stk.ToString(), font2))); 
                    tab.AddCell(new PdfPCell(new Phrase(item.Artikel, font2)));
                    tab.AddCell(new PdfPCell(new Phrase(item.Preis.ToString(), font2)));
                    tab.AddCell(new PdfPCell(new Phrase((item.Preis*item.Stk).ToString(), font2)));
                    zwischensumme+= (item.Preis * item.Stk);
                }

                cell = new PdfPCell(new Phrase("   "));
                cell.Colspan = 4;
                cell.Border = 0;
                tab.AddCell(cell);

                tab.AddCell(new PdfPCell(new Phrase("Zwischensumme", font2)));
                tab.AddCell("   ");
                tab.AddCell("   ");
                tab.AddCell(new PdfPCell(new Phrase(zwischensumme.ToString(), font2)));

                tab.AddCell(new PdfPCell(new Phrase("Mehrwertsteuer", font2)));
                tab.AddCell("   ");
                tab.AddCell(new PdfPCell(new Phrase("20%", font2)));
                tab.AddCell(new PdfPCell(new Phrase((zwischensumme*0.2).ToString(), font2)));

                cell = new PdfPCell(new Phrase("   "));
                cell.Colspan = 4;
                cell.Border = 0;
                tab.AddCell(cell);

                tab.AddCell(new PdfPCell(new Phrase("Gesamtbetrag", font2)));
                tab.AddCell("   ");
                tab.AddCell("   ");
                tab.AddCell(new PdfPCell(new Phrase(((zwischensumme * 0.2)+zwischensumme).ToString(), font2)));

                float[] columnWidths = new float[] { 13f, 40f, 9f, 11f };
                tab.SetWidths(columnWidths);
                document.Add(tab);

                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);

                Paragraph paragraphKommentar = new Paragraph("Kommentar: " + Rechnung.Kommentar, font2);
                paragraphKommentar.Alignment = Element.ALIGN_LEFT;
                paragraphKommentar.Leading = 40f;
                document.Add(paragraphKommentar);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                Paragraph paragraphNachricht = new Paragraph("Nachricht: " + Rechnung.Nachricht, font2);
                paragraphNachricht.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraphNachricht);


                document.Close();
                stream.Close();
                System.Diagnostics.Process.Start(path);
            }
        }
    }
}
