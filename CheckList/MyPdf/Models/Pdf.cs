using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MyPdf.Models
{
    public sealed class Pdf
    {
        private static readonly string Path = $@"F:\Уроки C#\MikhnoRuslan\it_academy_finalProject\CheckList\MyPdf";
        private static int _count = 1;

        public static FileStream Create((string id, string color, int numberOfSeats, int task) tuple,
            Tuple<ushort, ushort, ushort, float>[] characteristics,
            string login)
        {
            using var document = new Document();
            using var stream = new FileStream($@"{Path}\PDF\Checklist {tuple.id}.pdf", FileMode.Append);
            using (PdfWriter.GetInstance(document, stream))
            {
                document.Open();
                var name = new Paragraph($"Checklist {tuple.id}", new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.BOLD));
                name.Alignment = Element.ALIGN_CENTER;
                document.Add(name);

                AddCharacteristics(document, Constants.Task, tuple.task, 14);
                AddCharacteristics(document, Constants.Color, tuple.color, 14);
                AddCharacteristics(document, Constants.NumberOfPackages, tuple.numberOfSeats, 14);

                AddTableCharacteristics(document, characteristics);

                AddMessage(document);

                GetSignature(document, login);
                document.Close();
            }

            return stream;
        }

        private static void AddCharacteristics<T>(Document document, string characteristic, T item, int sizeFont)
        {
            var str = new Paragraph($"{characteristic} {item}", new Font(Font.FontFamily.TIMES_ROMAN, sizeFont, Font.NORMAL));
            str.Alignment = Element.ALIGN_LEFT;
            document.Add(str);
        }

        private static void AddTableCharacteristics(Document document, Tuple<ushort, ushort, ushort, float>[] characteristics)
        {
            AddCharacteristics(document,"", "\n", 14);
            PdfPTable table = new PdfPTable(5);
            table.HorizontalAlignment = Element.ALIGN_LEFT;

            var colWidth = new[] {7f, 12f, 12f, 12f, 12f};
            table.SetWidths(colWidth);

            PdfPCell title = new PdfPCell(new Phrase($"Data package", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLDITALIC)));
            title.HorizontalAlignment = Element.ALIGN_CENTER;
            title.Colspan = 5;
            
            table.AddCell(title);

            PdfPCell package = new PdfPCell(new Phrase($"Pack", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            table.AddCell(package);

            PdfPCell length = new PdfPCell(new Phrase($"Length (mm)", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            table.AddCell(length);

            PdfPCell width = new PdfPCell(new Phrase($"Width (mm)", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            table.AddCell(width);

            PdfPCell height = new PdfPCell(new Phrase($"Height (mm)", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            table.AddCell(height);

            PdfPCell weight = new PdfPCell(new Phrase($"Weight (mm)", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            table.AddCell(weight);

            for (int i = 0; i < characteristics.Length; i++)
            {
                table.AddCell($"{i + 1}");
                table.AddCell(characteristics[i].Item1.ToString());
                table.AddCell(characteristics[i].Item2.ToString());
                table.AddCell(characteristics[i].Item3.ToString());
                table.AddCell(characteristics[i].Item4.ToString());
            }

            document.Add(table);
        }

        public static void AddMessage(Document document)
        {
            var table = new PdfPTable(4);
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            var colWidth = new[] { 25f, 10f, 10f, 10f };
            table.SetWidths(colWidth);

            var title = new PdfPCell(new Phrase($"Post-assembly notes", 
                new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLDITALIC)));

            title.HorizontalAlignment = Element.ALIGN_CENTER;
            title.Colspan = 5;
            table.AddCell(title);

            var mes = new PdfPCell(new Phrase($"Message", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            table.AddCell(mes);

            var foto = new PdfPCell(new Phrase($"Foto", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            table.AddCell(foto);

            var error = new PdfPCell(new Phrase($"Error", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            table.AddCell(error);

            var @fixed = new PdfPCell(new Phrase($"Fixed", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD)));
            table.AddCell(@fixed);

            Console.WriteLine($"\nEnter post-assembly notes:");
            ConsoleKeyInfo key;
            do
            {
                Console.Write($"\tEnter:");
                PdfPCell cell = new PdfPCell(new Phrase($"{GetMessage()}\n", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL)));
                title.HorizontalAlignment = Element.ALIGN_LEFT;
                title.Rowspan = 2;
                table.AddCell(cell);

                cell = new PdfPCell(GetImages(), true);
                table.AddCell(cell);

                table.AddCell("");
                table.AddCell("");

                Console.WriteLine($"\nDo you want to enter another message (Y/N)?");
                key = Console.ReadKey();
                Console.WriteLine();
            } while (key.Key != ConsoleKey.N);

            document.Add(table);
        }

        private static string GetMessage()
        {
            return Console.ReadLine();
        }

        private static Image GetImages()
        {
            ConsoleKeyInfo key;
            Image image = null;
            do
            {
                Console.WriteLine($"\nDo you want to add image (Y/N)?");
                key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key == ConsoleKey.Y)
                    image = Image.GetInstance($@"{Path}\Image\{_count++}.jpg");

            } while (key.Key.Equals(ConsoleKey.N));

            return image;
        }

        private static void GetSignature(Document document, string login)
        {
            var table = new PdfPTable(2);
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            var colWidth = new[] { 27f, 27f};
            table.SetWidths(colWidth);

            var empty = new PdfPCell(new Phrase($"\n", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL)));
            empty.Border = 0;

            table.AddCell(empty);
            table.AddCell(empty);

            DateTime now = DateTime.Now;
            var cellTime = new PdfPCell(new Phrase($"{now:g}", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL)));
            cellTime.Border = 0;
            table.AddCell(cellTime);

            var cellUser = new PdfPCell(new Phrase($"{login}", new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.NORMAL)));
            cellUser.Border = 0;
            table.AddCell(cellUser);

            document.Add(table);
        }
    }
}
