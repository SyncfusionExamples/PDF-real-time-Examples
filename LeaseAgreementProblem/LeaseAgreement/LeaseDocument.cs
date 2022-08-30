using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using Syncfusion.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaseAgreement.Models;

namespace LeaseAgreement
{
    /// <summary>
    /// Lease data source class
    /// </summary>
    internal class LeaseDocument
    {
        #region Fields
        LeaseModel model;
        int margin = 20;
        int Padding = 5;
        int smallTextMargin = 10;
        int largeTextMargin = 30;
        SizeF clientSize;
        PdfStandardFont contentFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
        PdfStandardFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
        PdfPage currentPage;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LeaseDocument"/> class.
        /// </summary>
        /// <param name="LeaseModel">The lease model details.</param>
        public LeaseDocument(LeaseModel model)
        {
            this.model = model;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generate the PDF document
        /// </summary>
        /// <param name="Stream">The file stream.</param>
        public void GeneratePdf(Stream stream)
        {
            PdfDocument document = new PdfDocument();
            document.Pages.PageAdded += Pages_PageAdded;
            currentPage = document.Pages.Add();
            clientSize = currentPage.GetClientSize();
            RectangleF bounds = new RectangleF(0, 0, clientSize.Width, 50);
            PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);
            FileStream stream1 = new FileStream(@"..\..\..\Data\logo.png", FileMode.Open);
            PdfImage image = new PdfBitmap(stream1);
            PdfBrush drawString = new PdfSolidBrush(Color.Black);
            int logoWidth = (int)(clientSize.Width / 2) + 90;
            PdfBrush drawRectangle = new PdfSolidBrush(Color.LightGreen);

            header.Graphics.DrawRectangle(drawRectangle, bounds);
            header.Graphics.DrawImage(image, new PointF(logoWidth, 0), new SizeF(100, 50));
            header.Graphics.DrawString("Month to Month Lease Agreement", titleFont, drawString, new RectangleF(20, 20, clientSize.Width, 0));
            header.Graphics.DrawString("ABCD Company", titleFont, drawString, new RectangleF(logoWidth+10, 35, clientSize.Width, 0));

            document.Template.Top = header;

            PdfPageTemplateElement footer = new PdfPageTemplateElement(bounds);
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 7);
            PdfPageNumberField pageNumber = new PdfPageNumberField(font, drawString);
            PdfPageCountField count = new PdfPageCountField(font, drawString);
            PdfCompositeField compositeField = new PdfCompositeField(font, drawString, "Page {0} of {1}", pageNumber, count);
            compositeField.Bounds = footer.Bounds;
            compositeField.Draw(footer.Graphics, new PointF(470, 40));
            document.Template.Bottom = footer;

            var result = ComposeDate();
            result = ComposeParties(result.Bounds);
            result = ComposeLeasePeriod(result.Bounds);
            result = ComposeOtherOccupants(result.Bounds);
            result = ComposeTermsofLease(result.Bounds);
            document.Save(stream);
            document.Close(true);
        }
        /// <summary>
        /// Generate the pages
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The Page Added EventArgs.</param>
        private void Pages_PageAdded(object sender, PageAddedEventArgs args)
        {
            this.currentPage = args.Page;
        }
        /// <summary>
        /// Compose the date
        /// </summary>
        public PdfLayoutResult ComposeDate()
        {
            SizeF fontSize = contentFont.MeasureString(model.FormDate);
            float sizeX = clientSize.Width - fontSize.Width;
            var result = new PdfTextElement(model.FormDate, contentFont).Draw(currentPage, new PointF(sizeX, 10));
            return result;
        }
        /// <summary>
        /// Compose the parties details
        /// </summary>
        /// <param name="bounds">The rectangle bounds.</param>
        /// <returns>Pdf Layout Result</returns>
        public PdfLayoutResult ComposeParties(RectangleF bounds)
        {
            float y = bounds.Bottom + margin;
            float x = (clientSize.Width / 2) ;
            RectangleF halfBounds = new RectangleF(smallTextMargin, smallTextMargin, (clientSize.Width / 2), (clientSize.Height / 2));
            PdfBrush brush = PdfBrushes.LightGreen;
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(0, y, clientSize.Width, 20));


            var result = new PdfTextElement(model.Parties.Title, titleFont).Draw(currentPage, new RectangleF(Padding, y+ Padding, clientSize.Width, 0));
            result = new PdfTextElement("Lessor / Landlord", titleFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Lessee / Tenant", titleFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.Parties.Lessor, contentFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.Parties.Lessee, contentFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Property subject to lease:", titleFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Lease Term", titleFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));


            string address = model.Parties.PropertyDetails.PlatNo + " " + model.Parties.PropertyDetails.Street + "\n" + model.Parties.PropertyDetails.City + " " + model.Parties.PropertyDetails.PinCode;
            result = new PdfTextElement(address, contentFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + 10, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.Parties.LeaseTerm, contentFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Date payment period on every month", titleFont).Draw(currentPage, new RectangleF(x, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement($"{model.Parties.PaymentPeriod:d}", contentFont).Draw(currentPage, new RectangleF(x, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            return result;
        }
        /// <summary>
        /// Compose the lease period details
        /// </summary>
        /// <param name="bounds">The rectangle bounds.</param>
        /// <returns>Pdf Layout Result</returns>
        public PdfLayoutResult ComposeLeasePeriod(RectangleF bounds)
        {
            float y = bounds.Bottom + margin;
            float secondHalf = (clientSize.Width / 4);
            float thirdHalf = (clientSize.Width)/2; 
            float fourthHalf = (clientSize.Width /2)+100;  
            PdfBrush brush = PdfBrushes.LightGreen;
            RectangleF quaterBounds = new RectangleF(10, 10, (clientSize.Width / 4), (clientSize.Height / 4));
            
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(0, y, clientSize.Width , 20));
            var result = new PdfTextElement(model.LeasePeriod.Title, titleFont).Draw(currentPage, new RectangleF(Padding, y+ Padding, clientSize.Width, 0));
            result = new PdfTextElement("Lease From", titleFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, quaterBounds.Width/1.5f, quaterBounds.Height));
            result = new PdfTextElement("Lease Until", titleFont).Draw(currentPage, new RectangleF(thirdHalf, result.Bounds.Y, quaterBounds.Width/1.5f, quaterBounds.Height));
            result = new PdfTextElement(model.LeasePeriod.LeaseFrom, contentFont).Draw(currentPage, new RectangleF(secondHalf, result.Bounds.Y, quaterBounds.Width/1.5f, quaterBounds.Height));
            result = new PdfTextElement(model.LeasePeriod.LeaseUntil, contentFont).Draw(currentPage, new RectangleF(fourthHalf, result.Bounds.Y, quaterBounds.Width/1.5f, quaterBounds.Height));
            result = new PdfTextElement("Security Deposit Amount", titleFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, quaterBounds.Width/1.5f, quaterBounds.Height));
            result = new PdfTextElement("$ " + model.LeasePeriod.SecurityDebitAmount.ToString(), contentFont).Draw(currentPage, new RectangleF(secondHalf, result.Bounds.Y, quaterBounds.Width, quaterBounds.Height));
            result = new PdfTextElement("Monthly Lease Amount", titleFont).Draw(currentPage, new RectangleF(thirdHalf, result.Bounds.Y, quaterBounds.Width/1.5f, quaterBounds.Height));
            result = new PdfTextElement("$ " + model.LeasePeriod.MonthlyLeaseAmount.ToString(), contentFont).Draw(currentPage, new RectangleF(fourthHalf, result.Bounds.Y, quaterBounds.Width, quaterBounds.Height));
            return result;
        }
        /// <summary>
        /// Compose the other occupants details
        /// </summary>
        /// <param name="bounds">The rectangle bounds.</param>
        /// <returns>Pdf Layout Result</returns>
        public PdfLayoutResult ComposeOtherOccupants(RectangleF bounds)
        {
            float y = bounds.Bottom + margin;
            float x = (clientSize.Width / 2);
            RectangleF halfBounds = new RectangleF(10, 10, (clientSize.Width / 2), (clientSize.Height / 2));
            PdfBrush brush = PdfBrushes.LightGreen;
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(0, y, clientSize.Width - 20, 20));


            var result = new PdfTextElement(model.OtherOccupants1.Title, titleFont).Draw(currentPage, new PointF(Padding, y+ Padding));
            result = new PdfTextElement("Name of other occupant 1", titleFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width,halfBounds.Height));
            result = new PdfTextElement("Name of other occupant 2", titleFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants1.Name, contentFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants2.Name, contentFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Relationship", titleFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Relationship", titleFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants1.RelationShip, contentFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants2.RelationShip, contentFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));


            result = new PdfTextElement("Date of Birth", titleFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Date of Birth", titleFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants1.DateofBirth, contentFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants2.DateofBirth, contentFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Lessor Signature", titleFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + 100, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Lessee Signature", titleFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.LessorSignature.Signature, contentFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom+ largeTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.LesseeSignature.Signature, contentFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("-----------------------------------------------------------------page break--------------------------------------------------------------", contentFont).Draw(currentPage, new RectangleF(20, result.Bounds.Bottom+30, clientSize.Width, 0));
            result = new PdfTextElement(" ", contentFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + largeTextMargin, clientSize.Width, 0));
            return result;
        }
        /// <summary>
        /// Compose the terms of lease details
        /// </summary>
        /// <param name="bounds">The rectangle bounds.</param>
        /// <returns>Pdf Layout Result</returns>
        public PdfLayoutResult ComposeTermsofLease(RectangleF bounds)
        {
            float y = bounds.Bottom;
            float x = (clientSize.Width / 2) ;
            float width = (clientSize.Width / 2) - 40;
            PdfBrush brush = PdfBrushes.LightGreen;
            RectangleF rect = new RectangleF(10, 10, (clientSize.Width / 2), (clientSize.Height / 2));
            var result = new PdfTextElement(model.TermsofLease.Title, titleFont).Draw(currentPage, new RectangleF(width, y,0,0));

            currentPage.Graphics.DrawRectangle(brush, new RectangleF(0, result.Bounds.Bottom+ 10, rect.Width - 10, 20));
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(x, result.Bounds.Bottom+ 10, rect.Width - 10, 20));
            var leftsideResult = new PdfTextElement("Use and Occupancy", titleFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom+ smallTextMargin + Padding, rect.Width - 25, rect.Height));
            var rightsideResult = new PdfTextElement("Assignment and Sublease", titleFont).Draw(currentPage, new RectangleF(x+ Padding, leftsideResult.Bounds.Y, rect.Width - 10, rect.Height));
            leftsideResult = new PdfTextElement(model.TermsofLease.UseandOccupancy, contentFont).Draw(currentPage, new RectangleF(0, leftsideResult.Bounds.Bottom + smallTextMargin, rect.Width - 10, rect.Height));
            rightsideResult = new PdfTextElement(model.TermsofLease.AssignmentandSublease, contentFont).Draw(currentPage, new RectangleF(x+5, leftsideResult.Bounds.Y, rect.Width - 10, rect.Height));

            if(rightsideResult.Bounds.Bottom>leftsideResult.Bounds.Bottom)
            {
                leftsideResult = rightsideResult;
            }

            currentPage.Graphics.DrawRectangle(brush, new RectangleF(0, leftsideResult.Bounds.Bottom +30, rect.Width - 10, 20));
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(x, leftsideResult.Bounds.Bottom +30, rect.Width - 10, 20));
            leftsideResult = new PdfTextElement("Rent", titleFont).Draw(currentPage, new RectangleF(Padding, leftsideResult.Bounds.Bottom + largeTextMargin + Padding, rect.Width, rect.Height));
            rightsideResult = new PdfTextElement("Abandonment", titleFont).Draw(currentPage, new RectangleF(x + Padding, leftsideResult.Bounds.Y, rect.Width, rect.Height));
            leftsideResult = new PdfTextElement(model.TermsofLease.Rent, contentFont).Draw(currentPage, new RectangleF(0, leftsideResult.Bounds.Bottom + smallTextMargin, rect.Width - 10, rect.Height));
            rightsideResult = new PdfTextElement(model.TermsofLease.Abandonment, contentFont).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width - 10, rect.Height));

            if (rightsideResult.Bounds.Bottom > leftsideResult.Bounds.Bottom)
            {
                leftsideResult = rightsideResult;
            }

            currentPage.Graphics.DrawRectangle(brush, new RectangleF(0, leftsideResult.Bounds.Bottom + 30, rect.Width - 10, 20));
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(x, leftsideResult.Bounds.Bottom + 30, rect.Width - 10, 20));
            leftsideResult = new PdfTextElement("Necessary Expenses", titleFont).Draw(currentPage, new RectangleF(Padding, leftsideResult.Bounds.Bottom + largeTextMargin + Padding, rect.Width, rect.Height));
            rightsideResult = new PdfTextElement("Judicial Action", titleFont).Draw(currentPage, new RectangleF(x + Padding, leftsideResult.Bounds.Y, rect.Width, rect.Height));
            leftsideResult = new PdfTextElement(model.TermsofLease.NecessaryExpenses, contentFont).Draw(currentPage, new RectangleF(0, leftsideResult.Bounds.Bottom + smallTextMargin, rect.Width - 10, rect.Height));
            rightsideResult = new PdfTextElement(model.TermsofLease.JudicialAction, contentFont).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width - 10, rect.Height));

            if (rightsideResult.Bounds.Bottom > leftsideResult.Bounds.Bottom)
            {
                leftsideResult = rightsideResult;
            }

            currentPage.Graphics.DrawRectangle(brush, new RectangleF(0, leftsideResult.Bounds.Bottom + 30, rect.Width - 10, 20));
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(x, leftsideResult.Bounds.Bottom + 30, rect.Width - 10, 20));
            leftsideResult = new PdfTextElement("Improvements to the Premises", titleFont).Draw(currentPage, new RectangleF(Padding, leftsideResult.Bounds.Bottom + largeTextMargin + Padding, rect.Width, rect.Height));
            rightsideResult = new PdfTextElement("Pets", titleFont).Draw(currentPage, new RectangleF(x + Padding, leftsideResult.Bounds.Y, rect.Width, rect.Height));
            leftsideResult = new PdfTextElement(model.TermsofLease.ImprovementPremises, contentFont).Draw(currentPage, new RectangleF(0, leftsideResult.Bounds.Bottom + smallTextMargin, rect.Width - 10, rect.Height));
            rightsideResult = new PdfTextElement(model.TermsofLease.Pets, contentFont).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width - 10, rect.Height));

            return result;
        }
        #endregion
    }
}
