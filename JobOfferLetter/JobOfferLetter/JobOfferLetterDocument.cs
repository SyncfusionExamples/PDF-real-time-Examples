using JobOfferLetter.Model;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using Syncfusion.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Pdf.Graphics;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace JobOfferLetter
{
    public class JobOfferLetterDocument
    {
        JobOfferLetterModel model;
        PdfPage currentPage;
        FileStream fontStream;
        SizeF clientSize;
        /// <summary>
        /// Initializes a new instance of the <see cref="JobOfferLetterDocument"/> class.
        /// </summary>
        /// <param name="JobOfferLetterModel">The JobOfferLetterModel details.</param>
        public JobOfferLetterDocument(JobOfferLetterModel model)
        {
            this.model = model;
            fontStream = new FileStream(@"../../../Assets/Font/OpenSans-Regular.ttf", FileMode.Open, FileAccess.Read);
        }
        /// <summary>
        /// Generate the PDF document
        /// </summary>
        /// <param name="Stream">The file stream.</param>
        public void GeneratePdf(Stream stream)
        {
            
            //Create a new PDF document.
            PdfDocument document = new PdfDocument();
            //Set the margin of the page.
            document.PageSettings.Margins.All = 0;
            //Add a page to the document.
            currentPage = document.Pages.Add();
            //Get the PDF page size reduced by page margins and page template dimensions.
            clientSize = currentPage.GetClientSize(); 

            //Initializes a new instance of the RectangleF class.
            RectangleF headerBounds = new RectangleF(0, 0, 595, 170);
            //Create a header and draw the image.
            PdfPageTemplateElement header = new PdfPageTemplateElement(headerBounds);
            FileStream imageStream = new FileStream(@"../../../Assets/Image/logo.png", FileMode.Open, FileAccess.Read);
            //Load the image from the stream.
            PdfBitmap image = new PdfBitmap(imageStream);
            //Draws the header rectangle at the specified location and with the specified size.
            header.Graphics.DrawRectangle(PdfBrushes.DarkBlue, headerBounds);
            //Draws the specified Image at the specified location and with the specified size.
            header.Graphics.DrawImage(image, new PointF(40, 40), new SizeF(70, 70));
            PdfFont headerFont = new PdfTrueTypeFont(fontStream, 25f, PdfFontStyle.Bold);
            //Draws the text string at the specified location and size with the specified brush and font objects.
            header.Graphics.DrawString("AMAZE \nFOX", headerFont, new PdfSolidBrush(Color.White), new RectangleF(120, 45f, 0, 0));
            //Added the header of the page.
            document.Template.Top = header;


            string address = String.Empty;
            if (!string.IsNullOrEmpty(model.CustomerAddress.Street))
                address = model.CustomerAddress.Street + "\n";
            if (!string.IsNullOrEmpty(model.CustomerAddress.City))
                address += model.CustomerAddress.City + "\n";
            if (!string.IsNullOrEmpty(model.CustomerAddress.Phone))
                address += "Phone: "+ model.CustomerAddress.Phone + "\n";
            if (!string.IsNullOrEmpty(model.CustomerAddress.Website))
                address += model.CustomerAddress.Website + "\n";
            //Initializes a new instance of the PdfTrueTypeFont class.
            PdfFont contentFont = new PdfTrueTypeFont(fontStream, 12f, PdfFontStyle.Bold);
            //Draws the text string at the specified location and size with the specified brush and font objects.
            header.Graphics.DrawString(address, contentFont, new PdfSolidBrush(Color.White), new RectangleF(clientSize.Width - 170, 40, 0, 0));

            //Initializes a new instance of the PdfTextElement class with the text and PdfFont.
            var headerText = new PdfTextElement("Dear John Smith,", new PdfTrueTypeFont(fontStream, 14, PdfFontStyle.Bold));
            //Draws the element on the page with the specified page and PointF structure.
            PdfLayoutResult result =headerText.Draw(currentPage, new PointF(50, 40));
            result = new PdfTextElement(model.OfferLetterContent.BodyofContent, new PdfTrueTypeFont(fontStream, 14)).Draw(currentPage, new RectangleF(result.Bounds.X, result.Bounds.Bottom + 20, 595 - 100, 0));
            //Initializes a new instance of the PdfTextElement class with the text and PdfFont.
            headerText = new PdfTextElement("Sincerely, \nAmazeFox PVT LTD \nMicKin \n(Managing Director)", new PdfTrueTypeFont(fontStream, 14, PdfFontStyle.Bold));
            //Draws the element on the page with the specified page and PointF structure
            result = headerText.Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom+40));
           

            RectangleF footerBounds = new RectangleF(0, 0, 595, 40f);
            //Added the footer template of the page.
            PdfPageTemplateElement footer = new PdfPageTemplateElement(footerBounds);
            //Draws the rectangle at the specified location and with the specified size.
            footer.Graphics.DrawRectangle(PdfBrushes.DarkBlue, new RectangleF(0, 0, 595, 40f));
            //Draws the text string at the specified location and size with the specified brush and font objects.
            footer.Graphics.DrawString("AMAZE FOX PVT LTD", contentFont, new PdfSolidBrush(Color.White), new RectangleF(50, 10, 0, 0));
            footer.Graphics.DrawString("amazefoxsite.com", contentFont, new PdfSolidBrush(Color.White), new RectangleF(475, 10, 0, 0));
            document.Template.Bottom = footer;

            //Save and close the document.
            document.Save(stream);
            document.Close(true);
        }
    }
}
