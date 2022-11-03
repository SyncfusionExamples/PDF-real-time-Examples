using HospitalDocument.Model;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalDocument
{
    internal class DischargeDocument
    {
        #region Fields
        DischargeModel model;
        float rectanglePadding = 40;
        float padding = 50;
        int smallTextMargin = 10;
        SizeF clientSize;
        FileStream fontStream;
        PdfFont titleFont;
        PdfFont titleInnerFonts;
        PdfFont textTopicFonts;
        PdfFont textFont;
        PdfFont addressFont;
        PdfFont textFontTerms;
        PdfColor color;
        PdfBrush brush;
        PdfBrush lineBrush;
        PdfPage currentPage;
        int alignment = 50;
        float xPosition = 325.5f;
        #endregion
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DischargeDocument"/> class.
        /// </summary>
        /// <param name="DischargeModel">The Discharge model details.</param>
        public DischargeDocument(DischargeModel model)
        {
            this.model = model;
            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTrueTypeFont class.
            fontStream = new FileStream(@"../../../Assets/Fonts/OpenSans-Regular.ttf", FileMode.Open, FileAccess.Read);
            titleFont = new PdfTrueTypeFont(fontStream, 20f, PdfFontStyle.Bold);
            addressFont = new PdfTrueTypeFont(fontStream, 12f, PdfFontStyle.Regular);
            titleInnerFonts = new PdfTrueTypeFont(fontStream, 12f, PdfFontStyle.Bold);
            textTopicFonts = new PdfTrueTypeFont(fontStream, 12f, PdfFontStyle.Bold);
            textFont = new PdfTrueTypeFont(fontStream, 12f, PdfFontStyle.Regular);
            textFontTerms = new PdfTrueTypeFont(fontStream, 6f, PdfFontStyle.Regular);
            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfColor class.
            color = new PdfColor(217, 217, 217);
            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfSolidBrush class.
            brush = new PdfSolidBrush(color);
            lineBrush = new PdfSolidBrush(Color.Black);
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
            FileStream imageStream = new FileStream(@"../../../Assets/Image/Logo.png", FileMode.Open);
            PdfImage image = new PdfBitmap(imageStream);
            PdfBrush drawTopics = new PdfSolidBrush(Color.White);
            PdfColor color = new PdfColor(53, 67, 168);
            PdfBrush drawRectangle = new PdfSolidBrush(color);
            //Draws the rectangle at the specified location and with the specified size.
            header.Graphics.DrawRectangle(drawRectangle, headerBounds);
            //Draws the specified Image at the specified location and with the specified size.
            header.Graphics.DrawImage(image, new PointF(35, 40), new SizeF(60, 60));
            //Draws the text at the specified location and with the specified size.
            header.Graphics.DrawString("JAMES HOSPITAL", titleFont, drawTopics, new RectangleF(120, 51.5f, 0, 0));
            header.Graphics.DrawString("123 Dokato, St Church,", addressFont, drawTopics, new RectangleF(420, 38.5f, 0, 0));
            header.Graphics.DrawString("New York, 182916", addressFont, drawTopics, new RectangleF(420, 53.5f, 0, 0));
            header.Graphics.DrawString("Phone: 456-6780-21", addressFont, drawTopics, new RectangleF(420, 68.5f, 0, 0));
            header.Graphics.DrawString("jameshospital.com", addressFont, drawTopics, new RectangleF(420, 83.5f, 0, 0));
            document.Template.Top = header;
            //Initializes a new instance of the Syncfusion.Drawing.RectangleF class.
            RectangleF footerBounds = new RectangleF(0, 0, 595, 20.75f);
            PdfBrush drawString = new PdfSolidBrush(Color.Black);
            //Added the footer of the page
            PdfPageTemplateElement footer = new PdfPageTemplateElement(footerBounds);
            PdfPageNumberField pageNumber = new PdfPageNumberField(textFontTerms, drawString);
            PdfPageCountField count = new PdfPageCountField(textFontTerms, drawString);
            PdfCompositeField compositeField = new PdfCompositeField(textFontTerms, drawString, "Page {0} of {1}", pageNumber, count);
            compositeField.Bounds = footer.Bounds;
            //Draws the rectangle at the specified location and with the specified size.
            footer.Graphics.DrawRectangle(drawRectangle, new RectangleF(0, 0, 595, 20.75f)); ;
            //Draws the page number at the specified location and with the specified size.
            compositeField.Draw(footer.Graphics, new PointF(520, -20));
            document.Template.Bottom = footer;

            var result = ComposePatient();
            result = ComposeDischargeDetailsStart(result.Bounds);
            result = ComposeDischargeDetailsEnd(result.Bounds);
            //Save the documents
            document.Save(stream);
            document.Close(true);
            imageStream.Dispose();
        }
        /// <summary>
        /// Compose the Compose Patient
        /// </summary>
        /// <param name="bounds">The rectangle bounds.</param>
        /// <returns>Pdf Layout Result</returns>
        public PdfLayoutResult ComposePatient()
        {
            //Initializes a new instance of the Syncfusion.Drawing.RectangleF class.
            RectangleF bound = new RectangleF(337.93f, 10, 135.07f, 11.56f);
            float y = bound.Bottom + 20;
            RectangleF halfBounds = new RectangleF(smallTextMargin, smallTextMargin, clientSize.Width / 2, (clientSize.Height / 2));
            //Draws the title at the specified location and with the specified size.
            var result = new PdfTextElement("DISCHARGE SUMMARY", titleInnerFonts).Draw(currentPage, new RectangleF(180+ padding, y-20, clientSize.Width, 0));
            //Draws the line at the specified location and with the specified size.
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(rectanglePadding, y-25, 500, 1));
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(rectanglePadding, result.Bounds.Bottom+5, 500, 1));
            //Draws the parties details at the specified location and with the specified size.
            result = new PdfTextElement("Patient UID :"+" "+model.PatientDetails.PatientUID.ToString(), textTopicFonts).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 14, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Admission No :"+" "+ model.AdmissionDetails.AdmissionNo.ToString(), textTopicFonts).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Name :" + " " + model.PatientDetails.Name, textFont).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 2, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Admission Date :"+" " + model.AdmissionDetails.AdmissionDate.ToString(), textFont).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Age :" + " "+model.PatientDetails.Age, textFont).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 2, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Discharge Date :"+" "+model.AdmissionDetails.DischargeDate, textFont).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width - alignment, halfBounds.Height));

            result = new PdfTextElement("Address :"+" "+model.PatientDetails.Address, textFont).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 2, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Primary Treating Consultant’s Details:", textTopicFonts).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 14, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Discharge Status:", textTopicFonts).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement(model.ConsultantDetails.DoctorName, textFont).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 2, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement(model.AdmissionDetails.DischargeStatus, textFont).Draw(currentPage, new RectangleF(xPosition, result.Bounds.Y, halfBounds.Width - alignment, halfBounds.Height));
            result = new PdfTextElement("Speciality :"+" "+model.ConsultantDetails.Speciality, textFont).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 2, halfBounds.Width - alignment, halfBounds.Height));
            return result;
        }

        /// <summary>
        /// Compose the lease period details
        /// </summary>
        /// <param name="bounds">The rectangle bounds.</param>
        /// <returns>Pdf Layout Result</returns>
        public PdfLayoutResult ComposeDischargeDetailsStart(RectangleF bounds)
        {
            //Get the y position in bounds
            float y = bounds.Bottom + 18.11f;
            //Initialize the half page bounds
            RectangleF Bounds = new RectangleF(smallTextMargin, smallTextMargin, clientSize.Width, (clientSize.Height));
            //Draws the line at the specified location and with the specified size.
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(rectanglePadding, y, 500, 1));
            //Draws the lease period details at the specified location and with the specified size.
            var result = new PdfTextElement("Final Diagnosis at the time of Admission:", textTopicFonts).Draw(currentPage, new RectangleF(padding, y+14, clientSize.Width, 0));
            result = new PdfTextElement(model.DischargeDetails.FinalDiagnosis, textFont).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 2, Bounds.Width - alignment, Bounds.Height));
            result = new PdfTextElement("Key findings, on physical examination at the time of admission:", textTopicFonts).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 14, Bounds.Width - alignment, Bounds.Height));
            result = new PdfTextElement("BP(mmHg):"+" "+model.DischargeDetails.PhysicalExamination.BP+","+ "Pulse(/min):"+" "+model.DischargeDetails.PhysicalExamination.Pulse.ToString(), textFont).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 2, Bounds.Width - alignment, Bounds.Height));
            result = new PdfTextElement("General Appearance:", textTopicFonts).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 14, Bounds.Width - alignment, Bounds.Height));
            result = new PdfTextElement("Head/Eyes/Nose/Throat/Neck, Heart, Chest/Lung, Abdomen, Skin, Extreminities/Spine and Neurological Examination:"+" "+model.DischargeDetails.GeneralAppearance.Disease, textFont).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 2, Bounds.Width - alignment, Bounds.Height));
            return result;
        }
        /// <summary>
        /// Compose the Discharge Details2
        /// </summary>
        /// <param name="bounds">The rectangle bounds.</param>
        /// <returns>Pdf Layout Result</returns>
        public PdfLayoutResult ComposeDischargeDetailsEnd(RectangleF bounds)
        {
            //Get the y position in bounds
            float y = bounds.Bottom + 18.11f;
            //Initialize the half page bounds
            RectangleF Bounds = new RectangleF(smallTextMargin, smallTextMargin, clientSize.Width, (clientSize.Height));
            //Draws the line at the specified location and with the specified size.
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(rectanglePadding, y, 500, 1));
            //Draws the lease period details at the specified location and with the specified size.
            var result = new PdfTextElement("Summary of the Key Investigations during Hospitalization:", textTopicFonts).Draw(currentPage, new RectangleF(padding, y + 14, clientSize.Width, 0));
            result = new PdfTextElement(model.DischargeDetails.Investigation, textFont).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 2, Bounds.Width - alignment, Bounds.Height));
            result = new PdfTextElement("Discharge Medication:", textTopicFonts).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 14, Bounds.Width - alignment, Bounds.Height));
            result = new PdfTextElement(model.DischargeDetails.Medication, textFont).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 2, Bounds.Width - alignment, Bounds.Height));
            result = new PdfTextElement("Discharge Instructions:", textTopicFonts).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 14, Bounds.Width - alignment, Bounds.Height));
            result = new PdfTextElement(model.DischargeDetails.Instructions, textFont).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 2, Bounds.Width - alignment, Bounds.Height));
            result = new PdfTextElement("Patient/Attendant:", textTopicFonts).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 14, Bounds.Width - alignment, Bounds.Height));
            result = new PdfTextElement(model.DischargeDetails.Patient, textFont).Draw(currentPage, new RectangleF(padding, result.Bounds.Bottom + 2, Bounds.Width - alignment, Bounds.Height));
            result = new PdfTextElement("Name      :"+" "+model.SignatureDetails.Name, textTopicFonts).Draw(currentPage, new RectangleF(380+padding, result.Bounds.Bottom + 16, Bounds.Width - alignment, Bounds.Height));
            currentPage.Graphics.DrawRectangle(lineBrush, new RectangleF(440+padding, result.Bounds.Bottom, 60, 1));
            result = new PdfTextElement("Signature :",textTopicFonts).Draw(currentPage, new RectangleF(380+padding, result.Bounds.Bottom + 2, Bounds.Width - alignment, Bounds.Height));
            currentPage.Graphics.DrawImage(model.SignatureDetails.Signature,new PointF(440 + padding, result.Bounds.Bottom-14), new SizeF(50, 15));
            currentPage.Graphics.DrawRectangle(lineBrush, new RectangleF(440 + padding, result.Bounds.Bottom,60, 1));
            return result;
        }
        #endregion
    }
}
