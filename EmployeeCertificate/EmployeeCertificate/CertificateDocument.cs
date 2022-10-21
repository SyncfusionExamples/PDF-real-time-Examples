using EmployeeCertificate.Model;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EmployeeCertificate
{
    /// <summary>
    /// Certificate document class 
    /// </summary>
    public class CertificateDocument
    {
        CertificateModel model;
        PdfPage currentPage;
        SizeF clientSize;
        PdfFont titleFont;
        PdfFont textFont;
        PdfFont nameFont;
        PdfFont contentFont;
        PdfFont footerFont;
        float padding;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateDocument"/> class
        /// </summary>
        /// <param name="model">The certificate model details</param>
        public CertificateDocument(CertificateModel model)
        {
            this.model = model;

            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTrueTypeFont class
            FileStream titleFontStream = new FileStream("../../../Assets/OLD.ttf", FileMode.Open, FileAccess.Read);
            //Set the truetype font 
            titleFont = new PdfTrueTypeFont(titleFontStream, 47f, PdfFontStyle.Bold);

            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTrueTypeFont class
            FileStream textFontStream = new FileStream("../../../Assets/Calibri.ttf", FileMode.Open, FileAccess.Read);
            //Set the truetype font 
            textFont = new PdfTrueTypeFont(textFontStream, 17);

            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTrueTypeFont class
            FileStream nameFontStream = new FileStream("../../../Assets/SCRIPTBL.ttf", FileMode.Open, FileAccess.Read);
            //Set the truetype font 
            nameFont = new PdfTrueTypeFont(nameFontStream, 45);

            //Set the truetype font 
            contentFont = new PdfTrueTypeFont(textFontStream, 17, PdfFontStyle.Italic&PdfFontStyle.Bold);

            //Set the truetype font 
            footerFont = new PdfTrueTypeFont(textFontStream, 12);

            padding = 30;
        }
        #endregion
        #region method
        /// <summary>
        /// Generate the PDF document
        /// </summary>
        /// <param name="stream">The file stream</param>
        public void GeneratePDF(Stream stream)
        {
            //Create a new PDF document 
            PdfDocument document = new PdfDocument();

            //Represents the method that executes on a PdfDocument when a new page is created
            document.Pages.PageAdded += Pages_PageAdded;

            //Set all margin is 0
            document.PageSettings.Margins.All = 0;

            //Add a page to the document
            currentPage = document.Pages.Add();

            //Get the PDF page size reduced by page margins and page template dimensions
            clientSize = currentPage.GetClientSize();

            //Draw margin
            PdfBrush brush = new PdfSolidBrush(new PdfColor(209, 209, 224));
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(0, 0, clientSize.Width, clientSize.Height));
            currentPage.Graphics.DrawRectangle(new PdfPen(new PdfColor(122, 0, 204)), new RectangleF(15, 15, clientSize.Width-30, clientSize.Height-30));

            //Draw and fill rectangle       
            currentPage.Graphics.DrawRectangle(PdfBrushes.WhiteSmoke, new RectangleF(30, 30, clientSize.Width-60, clientSize.Height-60));

            //Initializes a new instance of the image class
            FileStream imageStream = new FileStream("../../../Assets/EmpImage1.png", FileMode.Open, FileAccess.Read);

            //Draw image
            PdfBitmap image = new PdfBitmap(imageStream);
            currentPage.Graphics.DrawImage(image, new RectangleF(180, 80, 200, 80));

            //Draw title text 
            string text = "Certificate of Appreciation";
            RectangleF bounds = GetBounds(text, 200, titleFont, clientSize.Width);
            PdfLayoutResult result = new PdfTextElement(text, titleFont).Draw(currentPage, bounds);

            //Initializes a new instance of the image class
            imageStream = new FileStream("../../../Assets/EmpImage.png", FileMode.Open, FileAccess.Read);

            //Draw image
            image = new PdfBitmap(imageStream);
            currentPage.Graphics.DrawImage(image, new RectangleF(100, result.Bounds.Bottom + 70, 400, 70));

            //Draw title text 
            text = "This certificate is awarded to";
            bounds = GetBounds(text, result.Bounds.Bottom + 170, textFont, clientSize.Width);
            result = new PdfTextElement(text, textFont).Draw(currentPage, bounds);

            //Draw name text 
            text = model.EmployeeName;
            bounds = GetBounds(text, result.Bounds.Bottom, nameFont, clientSize.Width);
            result = new PdfTextElement(text, nameFont, PdfBrushes.DarkBlue).Draw(currentPage, bounds);

            //Draw content text 
            text = "For your stellar performance for the month of December";
            bounds = GetBounds(text, result.Bounds.Bottom + 10, contentFont, clientSize.Width);
            result = new PdfTextElement(text, contentFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, bounds);

            //Draw content text 
            text = "Thank you for always showing us the right way with your attention to details. We feel lucky to have some one like you amidst us. Cheers!";
            bounds = GetBounds(text, result.Bounds.Bottom + 50, textFont, 370);
            PdfTextElement textElement = new PdfTextElement(text, textFont);
            textElement.StringFormat = new PdfStringFormat(PdfTextAlignment.Center);
            result = textElement.Draw(currentPage, new RectangleF(bounds.X, bounds.Y, 370, bounds.Height));

            //Initializes a new instance of the image class
            imageStream = new FileStream(model.ManagerSign, FileMode.Open, FileAccess.Read);
            image = new PdfBitmap(imageStream);
            //Draw image
            currentPage.Graphics.DrawImage(image, new RectangleF(110, clientSize.Height - 130, 70, 70));
            //Draw line
            currentPage.Graphics.DrawLine(new PdfPen(Color.Black), new PointF(100,clientSize.Height-80), new PointF(170,clientSize.Height-80));
            //Draw manager text 
            new PdfTextElement("Manager", footerFont).Draw(currentPage, new PointF(115, clientSize.Height - 75));

            //Draw line
            currentPage.Graphics.DrawLine(new PdfPen(Color.Black), new PointF(450, clientSize.Height - 80), new PointF(520, clientSize.Height - 80));
            //Draw manager text 
            new PdfTextElement("Date:", footerFont).Draw(currentPage, new PointF(475, clientSize.Height - 75));
            //Draw date
            new PdfTextElement(model.DateOfSign, footerFont).Draw(currentPage, new PointF(455, clientSize.Height-100));

            //Save the document
            document.Save(stream);

            //Close the document
            document.Close(true);
        }
        #endregion

        /// <summary>
        /// Generate the pages 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">Page added event arguments</param>
        private void Pages_PageAdded(object sender, PageAddedEventArgs args)
        {
            this.currentPage = args.Page;
        }
        /// <summary>
        /// Get the bounds of the text which could be placed center of document
        /// </summary>
        /// <param name="text"></param>
        /// <param name="height"></param>
        /// <param name="font"></param>
        /// <param name="textWidth"></param>
        /// <returns>bounds</returns>
        private RectangleF GetBounds(string text, float height, PdfFont font, float textWidth)
        {
            //Measure size of text 
            SizeF size = font.MeasureString(text, textWidth);

            //Get the starting position of text
            float remainingWidth = clientSize.Width - size.Width;
            float width = remainingWidth / 2;

            //Generate bounds to draw text 
            RectangleF bounds = new RectangleF(width, height, size.Width, size.Height);

            return bounds;
        }
    }
}
