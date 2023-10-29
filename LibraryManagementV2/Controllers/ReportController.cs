using iTextSharp.text.pdf;
using iTextSharp.text;
using LibraryManagementV2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagementV2.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        private LibraryEntities db = new LibraryEntities();
        public async Task<ActionResult> Index()
        {
            var targetDate = new DateTime(2015, 10, 1);
            var targetDate2 = new DateTime(2015, 12, 31);
            var top10Books = await db.books
         .Join(db.borrows, b => b.bookId, br => br.bookId, (b, br) => new { b, br })
         .Where(t => t.br.takenDate >= targetDate && t.br.takenDate <= targetDate2)
         .GroupBy(c => c.b.bookId)
         .Select(g => new BookReportModel
         {
             BookId = g.Key,
             BookName = g.FirstOrDefault().b.name, // Assuming you have a property for book name.
             BorrowCount = g.Count(),
         })
         .OrderByDescending(b => b.BorrowCount)
         .Take(10)
         .ToListAsync();


            return View(top10Books);
        }

        [HttpPost]
        public ActionResult DownloadReport(string fileName, string fileType, string capturedChartImage)
        {
            if (capturedChartImage != null)
            {
                // Split the capturedChartImage and get the second part (base64 data).
                string[] parts = capturedChartImage.Split(',');
                if (parts.Length > 1)
                {
                    var chartImageBytes = Convert.FromBase64String(parts[1]);

                    // Define the folder path where you want to save the reports
                    string folderPath = Server.MapPath("~/Documents/");

                    // Create the folder if it doesn't exist
                    Directory.CreateDirectory(folderPath);

                    // Set the file path for the saved report
                    string filePath = Path.Combine(folderPath, fileName + "." + fileType);

                    if (fileType.ToLower() == "pdf")
                    {
                        // Save PDF files using iTextSharp or another PDF library
                        // Replace the code below with code to create and save a PDF file
                        // Example code using iTextSharp:
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            // Create a new document
                            Document document = new Document();
                            PdfWriter writer = PdfWriter.GetInstance(document, fs);
                            document.Open();

                            // Add content to the PDF (you'll need to implement this part)
                            // For example: document.Add(new Paragraph("Hello, World!"));

                            document.Close();
                        }
                    }
                    else
                    {
                        // For other file types (e.g., images), write the image bytes to the file
                        System.IO.File.WriteAllBytes(filePath, chartImageBytes);
                    }

                    return RedirectToAction("Index");
                }
            }

            // Handle the case where capturedChartImage is null or doesn't contain the expected data.
            return HttpNotFound(); // You can return an appropriate result in case of an error.
        }

        public ActionResult DocumentArchive()
        {
            var documentDirectory = Server.MapPath("~/Documents/");
            var files = Directory.GetFiles(documentDirectory).Select(Path.GetFileName).ToList();
            return View(files);
        }

        [HttpPost]
        public ActionResult DownloadDocument(string fileName)
        {
            string path = Server.MapPath("~/Documents/") + fileName;
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public ActionResult DeleteDocument(string fileName)
        {
            string path = Server.MapPath("~/Documents/") + fileName;
            System.IO.File.Delete(path);

            // Redirect back to the document archive after deletion.
            return RedirectToAction("DocumentArchive");
        }


        public ActionResult DeleteFile(string fileName)
        {
            string path = Server.MapPath("~/Documents/") + fileName;
            byte[] bytes = System.IO.File.ReadAllBytes(path);


            System.IO.File.Delete(path);

            return RedirectToAction("Index");
        }

        public FileResult DisplayFile(string fileName)
        {
            string path = Server.MapPath("~/Documents/") + fileName;
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
