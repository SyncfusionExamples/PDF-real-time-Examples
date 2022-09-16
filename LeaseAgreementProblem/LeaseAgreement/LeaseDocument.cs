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
    public class LeaseDocument
    {
        LeaseModel model;
        int margin = 20;
        float Padding = 40;
        int smallTextMargin = 10;
        int largeTextMargin = 15;
        SizeF clientSize;
        static FileStream streamRegular = new FileStream(@"E:\111-New_Feature\LeaseAgreementProblem\LeaseAgreement\open-sans-cufonfonts\OpenSans-Regular.ttf", FileMode.Open, FileAccess.Read);
        PdfFont titleFont = new PdfTrueTypeFont(streamRegular, 20f, PdfFontStyle.Bold);
        PdfFont titleFonts = new PdfTrueTypeFont(streamRegular, 12f, PdfFontStyle.Bold);
        PdfFont textFonts = new PdfTrueTypeFont(streamRegular, 10f, PdfFontStyle.Bold);
        PdfFont textFont = new PdfTrueTypeFont(streamRegular, 10f, PdfFontStyle.Regular);
        PdfFont textFontTerms = new PdfTrueTypeFont(streamRegular, 8f, PdfFontStyle.Regular);
        static PdfColor color = new PdfColor(217, 217, 217);
        PdfBrush drawLine = new PdfSolidBrush(color);
        PdfPage currentPage;
        int alignment = 50;
        float xPosition = 325.5f;
        public LeaseDocument(LeaseModel model)
        {
            this.model = model;
        }

        public void GeneratePdf(Stream stream)
        {
            
            PdfDocument document = new PdfDocument();
            document.Pages.PageAdded += Pages_PageAdded;
            document.PageSettings.Margins.All = 0;
            currentPage = document.Pages.Add();
            clientSize = currentPage.GetClientSize();
            RectangleF headerBounds = new RectangleF(0, 0, 595, 130);
            PdfPageTemplateElement header = new PdfPageTemplateElement(headerBounds);
            FileStream stream1 = new FileStream(@"..\..\..\Data\Logo.png", FileMode.Open);
            PdfImage image = new PdfBitmap(stream1);
            PdfBrush drawTopics = new PdfSolidBrush(Color.White);
            PdfColor color = new PdfColor(53,67,168);
            PdfBrush drawRectangle = new PdfSolidBrush(color);

            header.Graphics.DrawRectangle(drawRectangle, headerBounds);
            header.Graphics.DrawImage(image, new PointF(35, 40), new SizeF(60,60));
            header.Graphics.DrawString("MONTH TO MONTH LEASE AGREEMENT", titleFont, drawTopics, new RectangleF(120, 51.5f,0 ,0));
            document.Template.Top = header;

            RectangleF footerBounds = new RectangleF(0,0 , 595, 20.75f);
            PdfBrush drawString = new PdfSolidBrush(Color.Black);
            PdfPageTemplateElement footer = new PdfPageTemplateElement(footerBounds);
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 8);
            PdfPageNumberField pageNumber = new PdfPageNumberField(font, drawString);
            PdfPageCountField count = new PdfPageCountField(font, drawString);
            PdfCompositeField compositeField = new PdfCompositeField(font, drawString, "Page {0} of {1}", pageNumber, count);
            compositeField.Bounds = footer.Bounds;
            footer.Graphics.DrawRectangle(drawRectangle, new RectangleF(0, 0, 595, 20.75f));;
            compositeField.Draw(footer.Graphics, new PointF(520, -20));
            document.Template.Bottom = footer;


            var result = ComposeParties();
            result = ComposeLeasePeriod(result.Bounds);
            result = ComposeOtherOccupants(result.Bounds);
            result = ComposeTermsofLease(result.Bounds);
            document.Save(stream);
            document.Close(true);
        }
        private void Pages_PageAdded(object sender, PageAddedEventArgs args)
        {
            this.currentPage = args.Page;
        }
        public PdfLayoutResult ComposeParties()
        {
            RectangleF bound = new RectangleF(337.93f, 10, 135.07f, 11.56f);
            float y = bound.Bottom + margin;
            RectangleF halfBounds = new RectangleF(smallTextMargin, smallTextMargin,clientSize.Width/2 , (clientSize.Height / 2));

            var result = new PdfTextElement(model.Parties.Title, titleFonts).Draw(currentPage, new RectangleF(Padding, y,clientSize.Width,0));
            currentPage.Graphics.DrawRectangle(drawLine, new RectangleF(Padding, result.Bounds.Bottom+8, 430, 1));
            result = new PdfTextElement("Lessor / Landlord", textFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 19, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Lessee / Tenant", textFonts).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement(model.Parties.Lessor, textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 2, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement(model.Parties.Lessee, textFont).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Property subject to lease:", textFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 16, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Lease Term", textFonts).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width - alignment, halfBounds.Height));


            string address = model.Parties.PropertyDetails.PlatNo + model.Parties.PropertyDetails.Street + model.Parties.PropertyDetails.City + model.Parties.PropertyDetails.PinCode;
            result = new PdfTextElement(address, textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 2, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement(model.Parties.LeaseTerm, textFont).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Date payment period on every month", textFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 26, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement($"{model.Parties.PaymentPeriod:d}", textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 2, halfBounds.Width - alignment, halfBounds.Height));
            return result;
        }
        public PdfLayoutResult ComposeLeasePeriod(RectangleF bounds)
        {
            float y = bounds.Bottom + 40.11f; 
            RectangleF halfBounds = new RectangleF(smallTextMargin, smallTextMargin, clientSize.Width / 2, (clientSize.Height / 2));

            var result = new PdfTextElement(model.LeasePeriod.Title, titleFonts).Draw(currentPage, new RectangleF(Padding, y, clientSize.Width, 0));
            currentPage.Graphics.DrawRectangle(drawLine, new RectangleF(Padding, result.Bounds.Bottom + 8, 430, 1));
            result = new PdfTextElement("Lease From", textFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 19, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Lease Until", textFonts).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement(model.LeasePeriod.LeaseFrom, textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 2, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement(model.LeasePeriod.LeaseUntil, textFont).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));


            result = new PdfTextElement("Security Deposit Amount", textFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 16, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("Monthly Lease Amount", textFonts).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("$ " + model.LeasePeriod.SecurityDebitAmount.ToString(), textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 2, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("$ " + model.LeasePeriod.MonthlyLeaseAmount.ToString(), textFont).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            return result;
        }
        public PdfLayoutResult ComposeOtherOccupants(RectangleF bounds)
        {
            float y = bounds.Bottom + 41;
            float x = 325.5f;
            RectangleF halfBounds = new RectangleF(10, 10, (clientSize.Width / 2), (clientSize.Height / 2));


            var result = new PdfTextElement("Names of other occupants", titleFonts).Draw(currentPage, new RectangleF(Padding, y,clientSize.Width,0));
            currentPage.Graphics.DrawRectangle(drawLine, new RectangleF(Padding, result.Bounds.Bottom + 8, 430, 1));
            result = new PdfTextElement("Name of other occupant 1", textFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 19, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("Name of other occupant 2", textFonts).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants1.Name, textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 2, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants2.Name, textFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("Relationship", textFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 16, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("Relationship", textFonts).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants1.RelationShip, textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 2, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants2.RelationShip, textFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));


            result = new PdfTextElement("Date of Birth", textFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 16, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("Date of Birth", textFonts).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants1.DateofBirth.ToString("dddd, dd MMMM 2010") , textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 2, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants2.DateofBirth.ToString("dddd, dd MMMM 2015"), textFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("Lessor Signature", textFonts).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom + 67, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement("Lessee Signature", textFonts).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement(model.LessorSignature.Signature, textFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom+ largeTextMargin, halfBounds.Width- alignment, halfBounds.Height));
            result = new PdfTextElement(model.LesseeSignature.Signature, textFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width- alignment, halfBounds.Height));
            return result;
        }
        public PdfLayoutResult ComposeTermsofLease(RectangleF bounds)
        {
            float y = bounds.Bottom+80;
            float x = (clientSize.Width / 2)+20;
            float padding = 35;
            float width = (clientSize.Width / 2) - 30;
            RectangleF rect = new RectangleF(10, 10, (clientSize.Width / 2), (clientSize.Height / 2));
            var result = new PdfTextElement("", titleFonts).Draw(currentPage, new RectangleF(0, y, clientSize.Width, 0));
            result = new PdfTextElement(model.TermsofLease.Title, titleFonts).Draw(currentPage, new RectangleF(width, result.Bounds.Bottom + 10, clientSize.Width,0));

            var leftsideResult = new PdfTextElement("Use and Occupancy", textFonts).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom+ 30, rect.Width - alignment, rect.Height));
            var rightsideResult = new PdfTextElement("Assignment and Sublease", textFonts).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width - alignment, rect.Height));
            leftsideResult = new PdfTextElement(model.TermsofLease.UseandOccupancy, textFontTerms).Draw(currentPage, new RectangleF(padding, leftsideResult.Bounds.Bottom + 8, rect.Width - alignment, rect.Height));
            rightsideResult = new PdfTextElement(model.TermsofLease.AssignmentandSublease, textFontTerms).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width - alignment, rect.Height));

            if(rightsideResult.Bounds.Bottom>leftsideResult.Bounds.Bottom)
            {
                leftsideResult = rightsideResult;
            }
            leftsideResult = new PdfTextElement("Rent", textFonts).Draw(currentPage, new RectangleF(padding, leftsideResult.Bounds.Bottom + largeTextMargin, rect.Width- alignment, rect.Height));
            rightsideResult = new PdfTextElement("Abandonment", textFonts).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width- alignment, rect.Height));
            leftsideResult = new PdfTextElement(model.TermsofLease.Rent, textFontTerms).Draw(currentPage, new RectangleF(padding, leftsideResult.Bounds.Bottom + 8, rect.Width - alignment, rect.Height));
            rightsideResult = new PdfTextElement(model.TermsofLease.Abandonment, textFontTerms).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width - alignment, rect.Height));

            if (rightsideResult.Bounds.Bottom > leftsideResult.Bounds.Bottom)
            {
                leftsideResult = rightsideResult;
            }
            leftsideResult = new PdfTextElement("Necessary Expenses", textFonts).Draw(currentPage, new RectangleF(padding, leftsideResult.Bounds.Bottom + largeTextMargin, rect.Width- alignment, rect.Height));
            rightsideResult = new PdfTextElement("Judicial Action", textFonts).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width- alignment, rect.Height));
            leftsideResult = new PdfTextElement(model.TermsofLease.NecessaryExpenses, textFontTerms).Draw(currentPage, new RectangleF(padding, leftsideResult.Bounds.Bottom + 8, rect.Width - alignment, rect.Height));
            rightsideResult = new PdfTextElement(model.TermsofLease.JudicialAction, textFontTerms).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width- alignment, rect.Height));

            if (rightsideResult.Bounds.Bottom > leftsideResult.Bounds.Bottom)
            {
                leftsideResult = rightsideResult;
            }
            leftsideResult = new PdfTextElement("Improvements to the Premises", textFonts).Draw(currentPage, new RectangleF(padding, leftsideResult.Bounds.Bottom + largeTextMargin, rect.Width- alignment, rect.Height));
            rightsideResult = new PdfTextElement("Pets", textFonts).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width- alignment, rect.Height));
            leftsideResult = new PdfTextElement(model.TermsofLease.ImprovementPremises, textFontTerms).Draw(currentPage, new RectangleF(padding, leftsideResult.Bounds.Bottom + 8, rect.Width - alignment, rect.Height));
            rightsideResult = new PdfTextElement(model.TermsofLease.Pets, textFontTerms).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width- alignment, rect.Height));

            return result;
        }
    }
}
