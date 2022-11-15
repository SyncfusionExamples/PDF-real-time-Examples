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
    /// Lease data documentation class
    /// </summary>
    public class LeaseDocument
    {
        #region Fields
        LeaseModel model;
        int margin = 20;
        float Padding = 40;
        int smallTextMargin = 10;
        int largeTextMargin = 15;
        SizeF clientSize;
        FileStream fontStream;
        PdfFont titleFont;
        PdfFont titleInnerFonts;
        PdfFont textTopicFonts;
        PdfFont textFont;
        PdfFont textFontTerms ;
        PdfColor color;
        PdfBrush brush;
        PdfPage currentPage;
        int alignment = 50;
        float xPosition = 325.5f;
        #endregion
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LeaseDocument"/> class.
        /// </summary>
        /// <param name="LeaseModel">The lease model details.</param>
        public LeaseDocument(LeaseModel model)
        {
            this.model = model;
            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTrueTypeFont class.
            fontStream = new FileStream(@"../../../Data/OpenSans-Regular.ttf", FileMode.Open, FileAccess.Read);
            titleFont = new PdfTrueTypeFont(fontStream, 20f, PdfFontStyle.Bold);
            titleInnerFonts = new PdfTrueTypeFont(fontStream, 12f, PdfFontStyle.Bold);
            textTopicFonts = new PdfTrueTypeFont(fontStream, 10f, PdfFontStyle.Bold);
            textFont = new PdfTrueTypeFont(fontStream, 10f, PdfFontStyle.Regular);
            textFontTerms = new PdfTrueTypeFont(fontStream, 8f, PdfFontStyle.Regular);
            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfColor class.
            color = new PdfColor(217, 217, 217);
            brush = new PdfSolidBrush(color);
        }
        #endregion
        #region Methods
        /// <summary>
        /// Generate the PDF document
        /// </summary>
        /// <param name="Stream">The file stream.</param>
        public void GeneratePdf(Stream stream)
        {
            //Create a new PDF document.
            PdfDocument document = new PdfDocument();
            //Represents the method that executes on a PdfDocument when a new page is created.
            document.Pages.PageAdded += Pages_PageAdded;
            //Set all margin is zero
            document.PageSettings.Margins.All = 0;
            //Add a page to the document.
            currentPage = document.Pages.Add();
            //Get the PDF page size reduced by page margins and page template dimensions.
            clientSize = currentPage.GetClientSize();
            //Initializes a new instance of the Syncfusion.Drawing.RectangleF class.
            RectangleF headerBounds = new RectangleF(0, 0, 595, 130);
            //Added the header of the page
            PdfPageTemplateElement header = new PdfPageTemplateElement(headerBounds);
            //Load the image as stream.
            FileStream imageStream = new FileStream(@"../../../Data/Logo.png", FileMode.Open);
            PdfImage image = new PdfBitmap(imageStream);
            PdfBrush drawTopics = new PdfSolidBrush(Color.White);
            PdfColor color = new PdfColor(53,67,168);
            PdfBrush drawRectangle = new PdfSolidBrush(color);
            //Draws the rectangle at the specified location and with the specified size.
            header.Graphics.DrawRectangle(drawRectangle, headerBounds);
            //Draws the specified Image at the specified location and with the specified size.
            header.Graphics.DrawImage(image, new PointF(35, 40), new SizeF(60,60));
            //Draws the text at the specified location and with the specified size.
            header.Graphics.DrawString("MONTH TO MONTH LEASE AGREEMENT", titleFont, drawTopics, new RectangleF(120, 51.5f,0 ,0));
            document.Template.Top = header;
            //Initializes a new instance of the Syncfusion.Drawing.RectangleF class.
            RectangleF footerBounds = new RectangleF(0,0 , 595, 20.75f);
            PdfBrush drawString = new PdfSolidBrush(Color.Black);
            //Added the footer of the page
            PdfPageTemplateElement footer = new PdfPageTemplateElement(footerBounds);
            PdfPageNumberField pageNumber = new PdfPageNumberField(textFontTerms, drawString);
            PdfPageCountField count = new PdfPageCountField(textFontTerms, drawString);
            PdfCompositeField compositeField = new PdfCompositeField(textFontTerms, drawString, "Page {0} of {1}", pageNumber, count);
            compositeField.Bounds = footer.Bounds;
            //Draws the rectangle at the specified location and with the specified size.
            footer.Graphics.DrawRectangle(drawRectangle, new RectangleF(0, 0, 595, 20.75f));;
            //Draws the page number at the specified location and with the specified size.
            compositeField.Draw(footer.Graphics, new PointF(520, -20));
            document.Template.Bottom = footer;

            var result = ComposeParties();
            result = ComposeLeasePeriod(result.Bounds);
            result = ComposeOtherOccupants(result.Bounds);
            result = ComposeTermsofLease(result.Bounds);
            //Save the documents
            document.Save(stream);
            document.Close(true);
            imageStream.Dispose();
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
        /// Compose the parties details
        /// </summary>
        /// <param name="bounds">The rectangle bounds.</param>
        /// <returns>Pdf Layout Result</returns>
        public PdfLayoutResult ComposeParties()
        {
            //Initializes a new instance of the Syncfusion.Drawing.RectangleF class.
            RectangleF bound = new RectangleF(337.93f, 10, 135.07f, 11.56f);
            float y = bound.Bottom + margin;
            RectangleF halfBounds = new RectangleF(smallTextMargin, smallTextMargin,clientSize.Width/2 , (clientSize.Height / 2));
            //Draws the title at the specified location and with the specified size.
            var result = new PdfTextElement(model.Parties.Title, titleInnerFonts).Draw(currentPage, new RectangleF(Padding, y,clientSize.Width,0));
            //Draws the line at the specified location and with the specified size.
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(Padding, result.Bounds.Bottom+8, 430, 1));
            //Draws the parties details at the specified location and with the specified size.
            result = new PdfTextElement("Lessor/Landlord", textTopicFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 19, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Lessee/Tenant", textTopicFonts).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement(model.Parties.Lessor, textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 2, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement(model.Parties.Lessee, textFont).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Property subject to lease:", textTopicFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 16, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Lease Term", textTopicFonts).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width - alignment, halfBounds.Height));

            string address = model.Parties.PropertyDetails.PlatNo + ", " +model.Parties.PropertyDetails.Street + ", \n" + model.Parties.PropertyDetails.City + ", " + model.Parties.PropertyDetails.PinCode;
            result = new PdfTextElement(address, textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 2, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement(model.Parties.LeaseTerm, textFont).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Date payment period on every month", textTopicFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 26, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement($"{model.Parties.PaymentPeriod:d}", textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 2, halfBounds.Width - alignment, halfBounds.Height));
            return result;
        }
        /// <summary>
        /// Compose the lease period details
        /// </summary>
        /// <param name="bounds">The rectangle bounds.</param>
        /// <returns>Pdf Layout Result</returns>
        public PdfLayoutResult ComposeLeasePeriod(RectangleF bounds)
        {
            //Get the y position in bounds
            float y = bounds.Bottom + 40.11f; 
            //Initialize the half page bounds
            RectangleF halfBounds = new RectangleF(smallTextMargin, smallTextMargin, clientSize.Width / 2, (clientSize.Height / 2));
            //Draws the title at the specified location and with the specified size.
            var result = new PdfTextElement(model.LeasePeriod.Title, titleInnerFonts).Draw(currentPage, new RectangleF(Padding, y, clientSize.Width, 0));
            //Draws the line at the specified location and with the specified size.
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(Padding, result.Bounds.Bottom + 8, 430, 1));
            //Draws the lease period details at the specified location and with the specified size.
            result = new PdfTextElement("Lease From", textTopicFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 19, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Lease Until", textTopicFonts).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement(model.LeasePeriod.LeaseFrom, textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 2, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement(model.LeasePeriod.LeaseUntil, textFont).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));

            result = new PdfTextElement("Security Deposit Amount", textTopicFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 16, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("Monthly Lease Amount", textTopicFonts).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("$" + model.LeasePeriod.SecurityDebitAmount.ToString(), textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 2, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("$" + model.LeasePeriod.MonthlyLeaseAmount.ToString(), textFont).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            return result;
        }
        /// <summary>
        /// Compose the other occupants details
        /// </summary>
        /// <param name="bounds">The rectangle bounds.</param>
        /// <returns>Pdf Layout Result</returns>
        public PdfLayoutResult ComposeOtherOccupants(RectangleF bounds)
        {
            //Get the y position in bounds
            float y = bounds.Bottom + 41;
            //Initialize the half page bounds
            RectangleF halfBounds = new RectangleF(10, 10, (clientSize.Width / 2), (clientSize.Height / 2));

            //Draws the title at the specified location and with the specified size.
            var result = new PdfTextElement("Names of other occupants", titleInnerFonts).Draw(currentPage, new RectangleF(Padding, y,clientSize.Width,0));
            //Draws the line at the specified location and with the specified size.
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(Padding, result.Bounds.Bottom + 8, 430, 1));
            //Draws the other occupants details at the specified location and with the specified size.
            result = new PdfTextElement("Name of other occupant 1", textTopicFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 19, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("Name of other occupant 2", textTopicFonts).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants1.Name, textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 2, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants2.Name, textFont).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("Relationship", textTopicFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 16, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("Relationship", textTopicFonts).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants1.RelationShip, textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 2, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants2.RelationShip, textFont).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));


            result = new PdfTextElement("Date of Birth", textTopicFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 16, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("Date of Birth", textTopicFonts).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants1.DateofBirth.ToString("dddd, 02 MMMM 1990") , textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 2, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants2.DateofBirth.ToString("dddd, 16 MMMM 1992"), textFont).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("Lessor Signature", textTopicFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 67, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("Lessee Signature", textTopicFonts).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement(model.LessorSignature.Signature, textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom+ largeTextMargin, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement(model.LesseeSignature.Signature, textFont).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            return result;
        }
        /// <summary>
        /// Compose the terms of lease details
        /// </summary>
        /// <param name="bounds">The rectangle bounds.</param>
        /// <returns>Pdf Layout Result</returns>
        public PdfLayoutResult ComposeTermsofLease(RectangleF bounds)
        {
            //Get the y position in bounds
            float y = bounds.Bottom+80;
            //Get the x position bounds
            float x = (clientSize.Width / 2)+20;
            float padding = 35;
            int textGap = 8;
            float width = (clientSize.Width / 2) - 30;
            //Initialize the half page bounds
            RectangleF rect = new RectangleF(10, 10, (clientSize.Width / 2), (clientSize.Height / 2));
            var result = new PdfTextElement("", titleInnerFonts).Draw(currentPage, new RectangleF(0, y, clientSize.Width, 0));
            //Draws the title at the specified location and with the specified size.
            result = new PdfTextElement(model.TermsofLease.Title, titleInnerFonts).Draw(currentPage, new RectangleF(width, result.Bounds.Bottom + 10, clientSize.Width,0));
            //Draws the terms of lease details at the specified location and with the specified size.
            var leftsideResult = new PdfTextElement("Use and Occupancy", textTopicFonts).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom+ 30, rect.Width - alignment, rect.Height));
            var rightsideResult = new PdfTextElement("Assignment and Sublease", textTopicFonts).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width - alignment, rect.Height));
            //Draws the leftside content at the specified location and with the specified size.
            leftsideResult = new PdfTextElement(model.TermsofLease.UseandOccupancy, textFontTerms).Draw(currentPage, new RectangleF(padding, leftsideResult.Bounds.Bottom + textGap, rect.Width - alignment, rect.Height));
            //Draws the Rightside content at the specified location and with the specified size.
            rightsideResult = new PdfTextElement(model.TermsofLease.AssignmentandSublease, textFontTerms).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width - alignment, rect.Height));
            //Assign the highest bottom bound values
            if(rightsideResult.Bounds.Bottom>leftsideResult.Bounds.Bottom)
            {
                leftsideResult = rightsideResult;
            }
            leftsideResult = new PdfTextElement("Rent", textTopicFonts).Draw(currentPage, new RectangleF(padding, leftsideResult.Bounds.Bottom + largeTextMargin, rect.Width- alignment, rect.Height));
            rightsideResult = new PdfTextElement("Abandonment", textTopicFonts).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width- alignment, rect.Height));
            leftsideResult = new PdfTextElement(model.TermsofLease.Rent, textFontTerms).Draw(currentPage, new RectangleF(padding, leftsideResult.Bounds.Bottom + textGap, rect.Width - alignment, rect.Height));
            rightsideResult = new PdfTextElement(model.TermsofLease.Abandonment, textFontTerms).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width - alignment, rect.Height));
            //Assign the highest bottom bound values
            if (rightsideResult.Bounds.Bottom > leftsideResult.Bounds.Bottom)
            {
                leftsideResult = rightsideResult;
            }
            leftsideResult = new PdfTextElement("Necessary Expenses", textTopicFonts).Draw(currentPage, new RectangleF(padding, leftsideResult.Bounds.Bottom + largeTextMargin, rect.Width- alignment, rect.Height));
            rightsideResult = new PdfTextElement("Judicial Action", textTopicFonts).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width- alignment, rect.Height));
            leftsideResult = new PdfTextElement(model.TermsofLease.NecessaryExpenses, textFontTerms).Draw(currentPage, new RectangleF(padding, leftsideResult.Bounds.Bottom + textGap, rect.Width - alignment, rect.Height));
            rightsideResult = new PdfTextElement(model.TermsofLease.JudicialAction, textFontTerms).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width- alignment, rect.Height));
            //Assign the highest bottom bound values
            if (rightsideResult.Bounds.Bottom > leftsideResult.Bounds.Bottom)
            {
                leftsideResult = rightsideResult;
            }
            leftsideResult = new PdfTextElement("Improvements to the Premises", textTopicFonts).Draw(currentPage, new RectangleF(padding, leftsideResult.Bounds.Bottom + largeTextMargin, rect.Width- alignment, rect.Height));
            rightsideResult = new PdfTextElement("Pets", textTopicFonts).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width- alignment, rect.Height));
            leftsideResult = new PdfTextElement(model.TermsofLease.ImprovementPremises, textFontTerms).Draw(currentPage, new RectangleF(padding, leftsideResult.Bounds.Bottom + textGap, rect.Width - alignment, rect.Height));
            rightsideResult = new PdfTextElement(model.TermsofLease.Pets, textFontTerms).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width- alignment, rect.Height));

            return result;
        }
        #endregion
    }
}
