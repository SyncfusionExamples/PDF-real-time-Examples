using PatientMedicalRecord.Model;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PatientMedicalRecord
{
    /// <summary>
    /// Patient medical record document generation class
    /// </summary>
    public class PatientRecordDocument
    {
        PatientRecordModel model;
        FileStream fontStream;
        PdfPage currentPage;
        SizeF clientSize;
        PdfTrueTypeFont textFont;
        PdfLayoutResult result;
        float dataWidth;
        PdfTrueTypeFont titleFont;
        RectangleF rectangleBounds;
        float leftPadding;
        PdfLayoutResult result1;
        float titlePadding;
        float dataHeight;
        float rightPadding;
        PdfColor color;

        public PatientRecordDocument(PatientRecordModel model)
        {
            this.model = model;

            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTrueTypeFont class
            fontStream = new FileStream("../../../Assets/phagspa.ttf", FileMode.Open, FileAccess.Read);

            //Set the truetype font 
            textFont = new PdfTrueTypeFont(fontStream, 12f);
            titleFont = new PdfTrueTypeFont(fontStream, 14f);
        }

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

            //Add header and footer to PDF document
            document.Template.Top = AddHeader();
            document.Template.Bottom = AddFooter();

            //Call the method to collect patient details, emergency details and medical information 
            GetPatientDetails();
            GetEmergencyDetails();
            GetMedicalInformation();

            //Save the document
            document.Save(stream);

            //Close the document
            document.Close(true);
        }
        /// <summary>
        /// Generate the pages
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">Page added event arguments</param>
        private void Pages_PageAdded(object sender, PageAddedEventArgs args)
        {
            this.currentPage = args.Page;
        }

        #region Header
        /// <summary>
        /// Add header to PDF document
        /// </summary>
        /// <param name="document"></param>
        public PdfPageTemplateElement AddHeader()
        {
            //Initializes a new instance of the Syncfusion.Drawing.RectangleF class
            RectangleF headerBounds = new RectangleF(0, 0, clientSize.Width, 90);

            //Added the header of the page
            PdfPageTemplateElement header = new PdfPageTemplateElement(headerBounds);

            //Load the image as stream
            FileStream imageStream = new FileStream(@"../../../Assets/Logo.png", FileMode.Open);
            PdfImage image = new PdfBitmap(imageStream);

            //Draws the specified Image at the specified location and with the specified size.
            header.Graphics.DrawImage(image, new PointF(30, 40), new SizeF(300, 38));

            return header;
        }
        #endregion
        #region footer 
        /// <summary>
        /// Add footer to PDF document 
        /// </summary>
        /// <returns>footer</returns>
        public PdfPageTemplateElement AddFooter()
        {
            //Initializes a new instance of the Syncfusion.Drawing.RectangleF class
            RectangleF footerBounds = new RectangleF(0, 0, 595, 20.75f);

            //Added the footer of the page
            PdfPageTemplateElement footer = new PdfPageTemplateElement(footerBounds);

            //Set the color to footer
            color = new PdfColor(38, 115, 77);
            PdfBrush brush = new PdfSolidBrush(color);

            //Draw rectangle in footer
            footer.Graphics.DrawRectangle(brush, footerBounds);

            return footer;
        }

        #endregion
        /// <summary>
        /// Get patient details and draw it in PDF document 
        /// </summary>
        public void GetPatientDetails()
        {
            leftPadding = 60;
            float dataPosition = 125;
            dataWidth = clientSize.Width / 4;
            float rightPadding = clientSize.Width - 200;

            #region Rectangle 

            //Initializes a new instance of the Syncfusion.Drawing.RectangleF class
            rectangleBounds = new RectangleF(30, 20, clientSize.Width-50, 180);

            //Draw rectangle
            currentPage.Graphics.DrawRectangle(PdfBrushes.WhiteSmoke, rectangleBounds);

            #endregion

            #region LeftData

            //Add patient details at the specified location and specified size 
            result = new PdfTextElement("Patient information", textFont, new PdfPen(Color.Black, 0.7f)).Draw(currentPage, new RectangleF(rectangleBounds.X+30, rectangleBounds.Y+20, dataWidth, rectangleBounds.Height));

            //Add patient name at the specified location and specified size 
            result1 = new PdfTextElement("Name        :", textFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, new RectangleF(leftPadding, result.Bounds.Bottom + 7, dataWidth, rectangleBounds.Height));
            new PdfTextElement(model.Patient.Name, textFont).Draw(currentPage, new RectangleF(dataPosition, result.Bounds.Bottom + 7, dataWidth, rectangleBounds.Height));

            //Add patient phone number at the specified location and specified size 
            result = new PdfTextElement("Phone       :", textFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, new RectangleF(leftPadding, result1.Bounds.Bottom + 7, dataWidth, rectangleBounds.Height));
            new PdfTextElement(model.Patient.PhoneNo, textFont).Draw(currentPage, new RectangleF(dataPosition, result1.Bounds.Bottom + 7, dataWidth, rectangleBounds.Height));

            //Add patient address at the specified location and specified size 
            new PdfTextElement("Address    :", textFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, new RectangleF(leftPadding, result.Bounds.Bottom + 7, dataWidth, rectangleBounds.Height));
            string patientAddress = model.Patient.PatientAddress.PlotNo + " " + model.Patient.PatientAddress.StreetName1;
            result = new PdfTextElement(patientAddress, textFont).Draw(currentPage, new RectangleF(dataPosition, result.Bounds.Bottom + 7, dataWidth, rectangleBounds.Height));
            patientAddress = model.Patient.PatientAddress.StreetName2 + ", " + model.Patient.PatientAddress.City + ", " + model.Patient.PatientAddress.PinCode;
            result = new PdfTextElement(patientAddress, textFont).Draw(currentPage, new RectangleF(leftPadding, result.Bounds.Bottom, dataWidth, rectangleBounds.Height));
            result = new PdfTextElement(model.Patient.PatientAddress.Country, textFont).Draw(currentPage, new RectangleF(leftPadding, result.Bounds.Bottom, dataWidth, rectangleBounds.Height));

            #endregion

            #region RightSideData

            //Add patient birth date at the specified location and specified size 
            result = new PdfTextElement("Birth Date:", textFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, new RectangleF(rightPadding, rectangleBounds.Y + 20, dataWidth, rectangleBounds.Height));
            result = new PdfTextElement(model.Patient.BirthDate, textFont).Draw(currentPage, new RectangleF(rightPadding, result.Bounds.Bottom + 3, dataWidth, rectangleBounds.Height));

            //Add patient weight at the specified location and specified size 
            result = new PdfTextElement("Weight:", textFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, new RectangleF(rightPadding, result.Bounds.Bottom + 20, dataWidth, rectangleBounds.Height));
            result = new PdfTextElement(model.Patient.Weight.ToString(), textFont).Draw(currentPage, new RectangleF(rightPadding, result.Bounds.Bottom + 3, dataWidth, rectangleBounds.Height));

            //Add patient height at the specified location and specified size 
            result = new PdfTextElement("Height:", textFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, new RectangleF(rightPadding, result.Bounds.Bottom + 20, dataWidth, rectangleBounds.Height));
            result = new PdfTextElement(model.Patient.Height.ToString(), textFont).Draw(currentPage, new RectangleF(rightPadding, result.Bounds.Bottom + 3, dataWidth, rectangleBounds.Height));

            #endregion
        } 
        /// <summary>
        /// Get emergency details and draw it in PDF document 
        /// </summary>
        public void GetEmergencyDetails()
        {
            //Get the title padding position 
            titlePadding = 30;

            //Get half height of inserted data
            dataHeight = clientSize.Height / 2;

            //Get the padding of right-side aligned text
            rightPadding = clientSize.Width - 240;

            #region Title 

            //Draw tile of emergency details 
            result = new PdfTextElement("In Case of Emergency", titleFont, new PdfPen(Color.Black, 0.7f)).Draw(currentPage, new RectangleF(titlePadding, result.Bounds.Bottom + 60, dataWidth, dataHeight));

            #endregion

            #region line

            //Draw line to PDF page
            currentPage.Graphics.DrawLine(new PdfPen(color), titlePadding, result.Bounds.Bottom + 10, rectangleBounds.Width + titlePadding, result.Bounds.Bottom + 10);

            #endregion

            #region Data

            //Add emergency address at the specified location and specified size 
            result1 = new PdfTextElement("Address", textFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, new RectangleF(rightPadding, result.Bounds.Bottom + 35, dataWidth, rectangleBounds.Height));

            //Add emergency contant name at the specified location and specified size 
            result = new PdfTextElement("Full Name", textFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, new RectangleF(leftPadding, result.Bounds.Bottom + 35, dataWidth, rectangleBounds.Height));
            result = new PdfTextElement(model.EmergencyDetails.FullName, textFont).Draw(currentPage, new RectangleF(leftPadding, result.Bounds.Bottom, dataWidth, rectangleBounds.Height));

            //Add emergency number at the specified location and specified size 
            result = new PdfTextElement("Number", textFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, new RectangleF(leftPadding, result.Bounds.Bottom + 20, dataWidth, rectangleBounds.Height));
            result = new PdfTextElement(model.EmergencyDetails.Number, textFont).Draw(currentPage, new RectangleF(leftPadding, result.Bounds.Bottom, dataWidth, rectangleBounds.Height));

            string address = model.EmergencyDetails.EmergencyAddress.PlotNo + ",\n" + model.EmergencyDetails.EmergencyAddress.StreetName + ", " + model.EmergencyDetails.EmergencyAddress.PinCode + ", " + model.EmergencyDetails.EmergencyAddress.Country;
            result1 = new PdfTextElement(address, textFont).Draw(currentPage, new RectangleF(rightPadding, result1.Bounds.Bottom, dataWidth, rectangleBounds.Height));

            #endregion
        }

        public void GetMedicalInformation()
        {
            //Get the 3/4 width of the PDF page to insert data 
            float halfwidth = clientSize.Width / 3;

            #region Title 
            //Add title for medical informatino 
            result = new PdfTextElement("Medical Information", titleFont, new PdfPen(Color.Black, 0.7f)).Draw(currentPage, new RectangleF(titlePadding, result.Bounds.Bottom + 40, dataWidth, dataHeight));
            #endregion

            #region Line
            //Draw line to PDF page
            currentPage.Graphics.DrawLine(new PdfPen(Color.DarkGreen), titlePadding, result.Bounds.Bottom + 10, rectangleBounds.Width + titlePadding, result.Bounds.Bottom + 10);
            #endregion

            #region Details 
            result1 = new PdfTextElement("Phone Number", textFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, new RectangleF(rightPadding, result.Bounds.Bottom + 35, halfwidth, rectangleBounds.Height));

            //Add clinic name at the specified location and specified size 
            result = new PdfTextElement("Name of physician or clinic/hospital", textFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, new RectangleF(leftPadding, result.Bounds.Bottom + 35, halfwidth, rectangleBounds.Height));
            result = new PdfTextElement(model.MedicalInformation.ClinicName, textFont).Draw(currentPage, new RectangleF(leftPadding, result.Bounds.Bottom, halfwidth, rectangleBounds.Height));

            //Add immunizations details at the specified location and specified size 
            result = new PdfTextElement("Is the camp up-to-date on all immunizations?", textFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, new RectangleF(leftPadding, result.Bounds.Bottom + 20, halfwidth, rectangleBounds.Height));
            result = new PdfTextElement(model.MedicalInformation.Immunizations, textFont).Draw(currentPage, new RectangleF(leftPadding, result.Bounds.Bottom, halfwidth, rectangleBounds.Height));

            //Add medical problems at the specified location and specified size 
            result = new PdfTextElement("List any medical problems (asthma, seizures, etc.)", textFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, new RectangleF(leftPadding, result.Bounds.Bottom + 20, clientSize.Width, rectangleBounds.Height));
            result = new PdfTextElement(model.MedicalInformation.MedicalProblem, textFont).Draw(currentPage, new RectangleF(leftPadding, result.Bounds.Bottom, halfwidth, rectangleBounds.Height));

            //Add address at the specified location and specified size 
            result1 = new PdfTextElement(model.MedicalInformation.PhoneNumber.ToString(), textFont).Draw(currentPage, new RectangleF(rightPadding, result1.Bounds.Bottom, halfwidth, rectangleBounds.Height));
            string address = model.MedicalInformation.Address.StreetName + ", " + model.MedicalInformation.Address.Landmark + ", " + model.MedicalInformation.Address.PinCode;
            result1 = new PdfTextElement("Address", textFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, new RectangleF(rightPadding, result1.Bounds.Bottom + 20, halfwidth, rectangleBounds.Height));
            result1 = new PdfTextElement(address, textFont).Draw(currentPage, new RectangleF(rightPadding, result1.Bounds.Bottom, halfwidth, rectangleBounds.Height));

            //Add policy number and medical insurance details at the specified location and specified size 
            result1 = new PdfTextElement("Policy number", textFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, new RectangleF(rightPadding, result.Bounds.Bottom + 20, halfwidth, rectangleBounds.Height));
            result = new PdfTextElement("If any medical insurance", textFont, new PdfPen(Color.Black, 0.5f)).Draw(currentPage, new RectangleF(leftPadding, result.Bounds.Bottom + 20, halfwidth, rectangleBounds.Height));        
            result = new PdfTextElement(model.MedicalInformation.MedicalInsurance, textFont).Draw(currentPage, new RectangleF(leftPadding, result.Bounds.Bottom, halfwidth, rectangleBounds.Height));
            result1 = new PdfTextElement(model.MedicalInformation.PolicyNumber.ToString(), textFont).Draw(currentPage, new RectangleF(rightPadding, result1.Bounds.Bottom, halfwidth, rectangleBounds.Height));
            #endregion
        }
    }
}
