using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardingPassProject.Model;
using Syncfusion.Drawing;
using Syncfusion.Pdf.Barcode;
using Syncfusion.Pdf.Interactive;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace BoardingPassProject
{
    public class BoardingPassDocument
    {
        BoardingPassModel model;
        PdfPage currentPage;
        FileStream headerfontStream = new FileStream(@"../../../Assets/Fonts/OpenSans-ExtraBold.ttf", FileMode.Open, FileAccess.Read);
        public BoardingPassDocument(BoardingPassModel model)
        {
            this.model = model;
        }

        public void GeneratePdf(Stream stream)
        {
            //Create a new PDF document.
            PdfDocument document = new PdfDocument();
            //Change a page orientation to landscape.
            document.PageSettings.Orientation = PdfPageOrientation.Landscape;
            //set the page size.
            document.PageSettings.Size = new SizeF(700, 288);
            //Set the margin of the page.
            document.PageSettings.Margins.All = 0;
            //Add a page to the document.
            currentPage = document.Pages.Add();

            //Set the bounds for rectangle.
            RectangleF rectangle = new RectangleF(0,0,700,38);
            //Draw the rectangle on PDF document.
            currentPage.Graphics.DrawRectangle(PdfBrushes.DarkBlue, rectangle);
            //Create font.
            PdfFont titleFont = new PdfTrueTypeFont(headerfontStream, 15);
            //Create a BOARDING PASS text element with the text and font.
            var headerText = new PdfTextElement("BOARDING PASS", titleFont, PdfBrushes.White);
            //Set the format for string.
            headerText.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult boardingResult = headerText.Draw(currentPage, new Point(500 - 40, 8));
            float imageX = boardingResult.Bounds.X-50;
            FileStream imageStream = new FileStream(@"../../../Assets/Image/images.png", FileMode.Open, FileAccess.Read);
            //Load the image from the stream.
            PdfBitmap image = new PdfBitmap(imageStream);
            //Draw the image.
            currentPage.Graphics.DrawImage(image, imageX, 0, 30, 38);
            float y = rectangle.Height + 20;

            //Set the bounds for rectangle.
            RectangleF rect = new RectangleF(0, 0, 64, 288);
            //Draw the rectangle on PDF document.
            currentPage.Graphics.DrawRectangle(PdfBrushes.Red, rect);
            float x = rect.Width + 25;

            PdfLayoutResult result = FormDetails(x,y);

            //Create font.
            PdfFont leftFont = new PdfTrueTypeFont(headerfontStream, 15);
            //Set the format for string.
            PdfStringFormat format = new PdfStringFormat();
            //Set the alignment.
            format.Alignment = PdfTextAlignment.Center;
            //Set the line alignment.
            format.LineAlignment = PdfVerticalAlignment.Middle;
            //Translate the coordinate system’s to where you want draw the text position.
            currentPage.Graphics.TranslateTransform(64/2, 288/2);
            //Rotate the coordinate system’s.
            currentPage.Graphics.RotateTransform(-90);
            //Draw the Your Airlines string at the origin with brush and font.
            currentPage.Graphics.DrawString("Your Airlines", leftFont, PdfBrushes.White, new Point(0, 0), format);


            document.Save(stream);
            document.Close(true);
            
        }
        public PdfLayoutResult FormDetails(float x, float y)
        {
            FileStream titlefontStream=new FileStream(@"../../../Assets/Fonts/OpenSans-Light.ttf", FileMode.Open, FileAccess.Read);
            //Create font.
            PdfFont titleFont = new PdfTrueTypeFont(titlefontStream, 12);
            
            FileStream contentFontStream = new FileStream(@"../../../Assets/Fonts/OpenSans-Bold.ttf", FileMode.Open, FileAccess.Read);
            //Create font.
            PdfFont contentFont = new PdfTrueTypeFont(contentFontStream, 12);

            //Left side:
            //Create a text element with the Passenger Name text and font.
            var headerText = new PdfTextElement("Passenger Name", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            var result = headerText.Draw(currentPage, new PointF(x,y));
            //Initializes a new instance of the PdfTextElement class with the PassengerName text and PdfFont & draw
            result = new PdfTextElement($" {model.PassengerName}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 2));


            //Create a text element with the From text and font.
            var from = new PdfTextElement("From", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            result = from.Draw(currentPage, new PointF(result.Bounds.X,result.Bounds.Bottom+15));
            float flightY = result.Bounds.Y;
            float flightR = result.Bounds.Right+130;
            //Initializes a new instance of the PdfTextElement class with the from text and PdfFont and & draw
            result = new PdfTextElement($" {model.From}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 2));


            //Create font.
            PdfFont changeContentFont = new PdfTrueTypeFont(contentFontStream, 18);

            //Create a text element with the Flight text and font.
            var flight = new PdfTextElement("Flight", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult flightResult = flight.Draw(currentPage, new PointF(flightR,flightY));
            float dateY = flightResult.Bounds.Y;
            float dateR = flightResult.Bounds.Right +100 ;
            //Initializes a new instance of the PdfTextElement class with the flight text and PdfFont & draw
            flightResult = new PdfTextElement($"{model.Flight}", changeContentFont).Draw(currentPage, new PointF(flightResult.Bounds.X, flightResult.Bounds.Bottom));
            float barcodeStartPoint = flightResult.Bounds.X;


            //Create a text element with the date text and font.
            var date = new PdfTextElement("Date", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult dateResult = date.Draw(currentPage, new PointF(dateR,dateY));
            //Initializes a new instance of the PdfTextElement class with the date text and PdfFont & draw
            dateResult = new PdfTextElement($"{model.Date}", changeContentFont).Draw(currentPage, new PointF(dateResult.Bounds.X, dateResult.Bounds.Bottom));
            float timeY = dateResult.Bounds.Bottom;


            //Create a text element with the To text and font.
            var destination = new PdfTextElement("To", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            result = destination.Draw(currentPage, new PointF(result.Bounds.X,result.Bounds.Bottom + 15));
            //Initializes a new instance of the PdfTextElement class with the To text and PdfFont & draw
            result = new PdfTextElement($"{model.To}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 2));


            //Create a text element with the time text and font.
            var time = new PdfTextElement("Time", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult timeResult = time.Draw(currentPage, new PointF(dateR,timeY + 10));
            //Initializes a new instance of the PdfTextElement class with the time text and PdfFont & draw
            timeResult = new PdfTextElement($"{model.Time}", changeContentFont, PdfBrushes.Red).Draw(currentPage, new PointF(timeResult.Bounds.X, timeResult.Bounds.Bottom));
            float barcodeEndPoint = timeResult.Bounds.Bottom;


            //Create a text element with the gate text and font.
            var gate = new PdfTextElement("Gate", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            result = gate.Draw(currentPage, new PointF(result.Bounds.X,result.Bounds.Bottom + 15));
            float seatY = result.Bounds.Y;
            float seatX = result.Bounds.Right + 30;
            //Initializes a new instance of the PdfTextElement class with the gate text and PdfFont & draw
            result = new PdfTextElement($"{model.Gate}", changeContentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom));


            //Create a text element with the seat text and font.
            var seat = new PdfTextElement("Seat", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult leftSeatResult = seat.Draw(currentPage, new PointF(seatX,seatY));
            //Initializes a new instance of the PdfTextElement class with the seat text and PdfFont & draw
            leftSeatResult = new PdfTextElement($"{model.Seat}", changeContentFont).Draw(currentPage, new PointF(leftSeatResult.Bounds.X, leftSeatResult.Bounds.Bottom));


            //Create font.
            PdfFont footerFont = new PdfTrueTypeFont(titlefontStream, 8);
            //Create a  text element with the text and font.
            var footerText = new PdfTextElement("*Gate Closes 30 Minutes Before Departure", footerFont, PdfBrushes.Red);
            //Draws the element on the page with the specified page and PointF structure
            result = footerText.Draw(currentPage, new PointF(result.Bounds.X,result.Bounds.Bottom + 18));


            //Drawing Code39 barcode.
            PdfCode39Barcode barcode = new PdfCode39Barcode();
            //Set the barcode text.
            barcode.Text = "CODE39$";
            //Setting size of the barcode.
            barcode.Size = new SizeF(dateResult.Bounds.Right/2, 30);
            //Set the TextDisplayLocation.
            barcode.TextDisplayLocation = TextLocation.None;
            //Printing barcode on to the Pdf.
            barcode.Draw(currentPage, new PointF(barcodeStartPoint, barcodeEndPoint + 18));


            //Initializes a new instance of the PdfPen class with color and width of the pen
            var pen = new PdfPen(Color.Gray, 1);
            //Gets or sets the dash style of the pen. 
            pen.DashStyle = PdfDashStyle.Dash;
            //Gets or sets the dash offset of the pen. 
            pen.DashOffset = 0.5f;
            //Draw the DashLine.
            currentPage.Graphics.DrawLine(pen, 500, 0, 500, currentPage.GetClientSize().Height);


            //Right side:
            //Create font.
            PdfFont rightTitleFont = new PdfTrueTypeFont(headerfontStream, 15);
            //Draw the BoardingPass text string.
            currentPage.Graphics.DrawString("BOARDING PASS", rightTitleFont, PdfBrushes.White, new PointF(500 + 30, 8));


            //Initializes a new instance of the PdfTextElement class with the PassengerName text and PdfFont
            var rightPassengerName = new PdfTextElement("Passenger Name", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            result = rightPassengerName.Draw(currentPage, new PointF(500 + 20, y));
            //Initializes a new instance of the PdfTextElement class with the passenger name text and PdfFont & draw
            result = new PdfTextElement($" {model.PassengerName}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 2));


            //Create a text element with the from text and font.
            var rightFrom = new PdfTextElement("From", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            result = rightFrom.Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom+15));
            //Initializes a new instance of the PdfTextElement class with the from text and PdfFont & draw
            result = new PdfTextElement($" {model.From}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom +2));


            //Create a text element with the To text and font.
            var rightTo = new PdfTextElement("To", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            result = rightTo.Draw(currentPage, new PointF(result.Bounds.X,result.Bounds.Bottom + 15));
            //Initializes a new instance of the PdfTextElement class with the To text and PdfFont & draw
            result = new PdfTextElement($"{model.To}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 2));
            float dateRight = result.Bounds.Right+2;


            //Create a text element with the Flight text and font.
            var rightFlight = new PdfTextElement("Flight", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            result = rightFlight.Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 10));
            float flightBottom = result.Bounds.Y;
            //Initializes a new instance of the PdfTextElement class with the Flight text and PdfFont & draw
            result = new PdfTextElement($"{model.Flight}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 2));


            //Create a text element with the date text and font.
            var rightDate = new PdfTextElement("Date", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult rightDateResult = rightDate.Draw(currentPage, new PointF(dateRight,flightBottom));
            float dateLeft = rightDateResult.Bounds.Left ;
            //Initializes a new instance of the PdfTextElement class with the date text and PdfFont & draw
            rightDateResult = new PdfTextElement($"{model.Date}", contentFont).Draw(currentPage, new PointF(rightDateResult.Bounds.X, rightDateResult.Bounds.Bottom + 2));


            //Create a text element with the gate text and font.
            var rightGate = new PdfTextElement("Gate", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            result = rightGate.Draw(currentPage, new PointF(result.Bounds.X,result.Bounds.Bottom + 10));
            float gateRight = result.Bounds.Right+20;
            float gateY = result.Bounds.Y;
            //Initializes a new instance of the PdfTextElement class with the gate text and PdfFont & draw
            result = new PdfTextElement($"{model.Gate}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 2));


            //Create a text element with the seat text and font.
            var rightSeat = new PdfTextElement("Seat", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult seatResult = rightSeat.Draw(currentPage, new PointF(gateRight,gateY));
            //Initializes a new instance of the PdfTextElement class with the seat text and PdfFont & draw
            seatResult = new PdfTextElement($"{model.Seat}", contentFont).Draw(currentPage, new PointF(seatResult.Bounds.X, seatResult.Bounds.Bottom + 2));


            //Create a text element with the time text and font.
            var rightTime = new PdfTextElement("Time", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult rightTimeResult = rightTime.Draw(currentPage, new PointF(dateLeft,gateY));
            //Initializes a new instance of the PdfTextElement class with the time text and PdfFont & draw
            rightTimeResult = new PdfTextElement($"{model.Time}", contentFont).Draw(currentPage, new PointF(rightTimeResult.Bounds.X , rightTimeResult.Bounds.Bottom + 2));

            return result;
            
        }
    }
}
