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
        FileStream headerFontStream = new FileStream(@"../../../Assets/Fonts/OpenSans-Regular.ttf", FileMode.Open, FileAccess.Read);
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
            document.PageSettings.Size = new SizeF(568, 228);
            //Set the margin of the page.
            document.PageSettings.Margins.All = 0;
            //Add a page to the document.
            currentPage = document.Pages.Add();

            //Set the bounds for rectangle.
            RectangleF rectangle = new RectangleF(0,0,568,36);
            //Draw the rectangle on PDF document.
            currentPage.Graphics.DrawRectangle(new PdfSolidBrush(Color.FromArgb(1, 0, 87, 255)), rectangle);
            float y = rectangle.Height;
            //Create font.
            PdfFont titleFont = new PdfTrueTypeFont(headerFontStream, 10, PdfFontStyle.Bold);
            //Create a BOARDING PASS text element with the text and font.
            var headerText = new PdfTextElement("BOARDING PASS", titleFont, PdfBrushes.White);
            //Set the format for string.
            headerText.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult boardingResult = headerText.Draw(currentPage, new Point(420 - 25, 12));
            float barcodeStartPoint = boardingResult.Bounds.X;
            FileStream imageStream = new FileStream(@"../../../Assets/Image/logo.png", FileMode.Open, FileAccess.Read);
            //Load the image from the stream.
            PdfBitmap image = new PdfBitmap(imageStream);
            //Draw the image.
            currentPage.Graphics.DrawImage(image, 30, 7, 23, 23);
            float airTextStart =image.Width + 15;
            //Create a text element with the AIRWAY text and font.
            var airWayText = new PdfTextElement("AIRWAY", titleFont, PdfBrushes.White);
            //Set the format for string.
            airWayText.StringFormat = new PdfStringFormat(PdfTextAlignment.Left);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult airResult = airWayText.Draw(currentPage, new RectangleF(airTextStart, 13, 43, 14));


            //Initializes a new instance of the PdfPen class with color and width of the pen
            var pen = new PdfPen(Color.FromArgb(1,197,197, 197), 1);
            //Gets or sets the dash style of the pen. 
            pen.DashStyle = PdfDashStyle.Dash;
            //Gets or sets the dash offset of the pen. 
            pen.DashOffset = 1f;
            //Draw the DashLine.
            currentPage.Graphics.DrawLine(pen, 420, 0, 420, currentPage.GetClientSize().Height);

            PdfLayoutResult result = FormDetails(30, y);

            //Drawing Code39 barcode.
            PdfQRBarcode barcode = new PdfQRBarcode();
            //Set the barcode text.
            barcode.Text = $"{model.Gate}+{model.Seat}+{model.Flight}";
            //Setting size of the barcode.
            barcode.Size = new SizeF(80, 80);
            //Printing barcode on to the Pdf.
            barcode.Draw(currentPage, new PointF(barcodeStartPoint, y+14));

            document.Save(stream);
            document.Close(true);
            
        }
        public PdfLayoutResult FormDetails(float x, float y)
        {
            //Create font.
            PdfFont titleFont = new PdfTrueTypeFont(headerFontStream, 10);
            PdfFont contentFont = new PdfTrueTypeFont(headerFontStream, 10, PdfFontStyle.Bold);
           
            //Left side:
            //Create a text element with the Passenger Name text and font.
            var passengerNameText = new PdfTextElement("Passenger Name", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            var result = passengerNameText.Draw(currentPage, new PointF(x,y+14));
            //Initializes a new instance of the PdfTextElement class with the PassengerName text and PdfFont & draw
            result = new PdfTextElement($"{model.PassengerName}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 2));
            

            //Create a text element with the From text and font.
            var fromText = new PdfTextElement("From", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            result = fromText.Draw(currentPage, new PointF(result.Bounds.X,result.Bounds.Bottom+5));
            //Initializes a new instance of the PdfTextElement class with the from text and PdfFont and & draw
            result = new PdfTextElement($"{model.From}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 2));


            //Create a text element with the To text and font.
            var destinationText = new PdfTextElement("To", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            result = destinationText.Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 5));
            //Initializes a new instance of the PdfTextElement class with the To text and PdfFont & draw
            result = new PdfTextElement($"{model.To}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 2));
           
            float gateRectY = result.Bounds.Bottom + 20;
            RectangleF rectangle = new RectangleF(0, gateRectY, 419, 50);
            //Draw the rectangle on PDF document.
            currentPage.Graphics.DrawRectangle(new PdfSolidBrush(Color.FromArgb(1,226, 242, 255)), rectangle);
            float gateY = rectangle.Y+10;

            //Create a text element with the gate text and font.
            var gate = new PdfTextElement("Gate", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            result = gate.Draw(currentPage, new PointF(result.Bounds.X, gateY));
            float seatX = result.Bounds.Right + 30;
            //Initializes a new instance of the PdfTextElement class with the gate text and PdfFont & draw
            result = new PdfTextElement($"{model.Gate}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom));


            //Create a text element with the seat text and font.
            var seat = new PdfTextElement("Seat", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult leftSeatResult = seat.Draw(currentPage, new PointF(seatX, gateY));
            float flightX = leftSeatResult.Bounds.Right + 30; 
            //Initializes a new instance of the PdfTextElement class with the seat text and PdfFont & draw
            leftSeatResult = new PdfTextElement($"{model.Seat}", contentFont).Draw(currentPage, new PointF(leftSeatResult.Bounds.X, leftSeatResult.Bounds.Bottom));


            //Create a text element with the Flight text and font.
            var flight = new PdfTextElement("Flight", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult flightResult = flight.Draw(currentPage, new PointF(flightX, gateY));
            float dateR = flightResult.Bounds.Right + 34;
            //Initializes a new instance of the PdfTextElement class with the flight text and PdfFont & draw
            flightResult = new PdfTextElement($"{model.Flight}", contentFont).Draw(currentPage, new PointF(flightResult.Bounds.X, flightResult.Bounds.Bottom));


            //Create a text element with the date text and font.
            var date = new PdfTextElement("Date", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult dateResult = date.Draw(currentPage, new PointF(dateR, gateY));
            //Initializes a new instance of the PdfTextElement class with the date text and PdfFont & draw
            dateResult = new PdfTextElement($"{model.Date}", contentFont).Draw(currentPage, new PointF(dateResult.Bounds.X, dateResult.Bounds.Bottom));
            //float timeY = dateResult.Bounds.Bottom;
            float timeR = dateResult.Bounds.Right +30;

            //Create a text element with the time text and font.
            var time = new PdfTextElement("Time", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult timeResult = time.Draw(currentPage, new PointF(timeR, gateY));
            //Initializes a new instance of the PdfTextElement class with the time text and PdfFont & draw
            timeResult = new PdfTextElement($"{model.Time}", contentFont).Draw(currentPage, new PointF(timeResult.Bounds.X, timeResult.Bounds.Bottom));

            //BottomRectangle:
            RectangleF bottomRectangle = new RectangleF(0, 214, 568, 14);
            //Draw the rectangle on PDF document.
            currentPage.Graphics.DrawRectangle(new PdfSolidBrush(Color.FromArgb(1, 0, 87, 255)), bottomRectangle);


            //Right side:
            //Draw the BoardingPass text string.
            currentPage.Graphics.DrawString("BOARDING PASS", contentFont, PdfBrushes.White, new PointF(420 + 16, 12));


            //Initializes a new instance of the PdfTextElement class with the PassengerName text and PdfFont
            var rightPassengerName = new PdfTextElement("Passenger Name", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            result = rightPassengerName.Draw(currentPage, new PointF(420 + 16, y+10));
            //Initializes a new instance of the PdfTextElement class with the passenger name text and PdfFont & draw
            result = new PdfTextElement($"{model.PassengerName}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom));


            //Create a text element with the from text and font.
            var rightFrom = new PdfTextElement("From", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            result = rightFrom.Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 4));
            //Initializes a new instance of the PdfTextElement class with the from text and PdfFont & draw
            result = new PdfTextElement($"{model.From}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom));


            //Create a text element with the To text and font.
            var rightTo = new PdfTextElement("To", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            result = rightTo.Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 4));
            //Initializes a new instance of the PdfTextElement class with the To text and PdfFont & draw
            result = new PdfTextElement($"{model.To}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom));

            //Create a text element with the gate text and font.
            var rightGate = new PdfTextElement("Gate", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            result = rightGate.Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 6));
            float gateRight = result.Bounds.Right + 17;
            float seatY = result.Bounds.Y;
            //Initializes a new instance of the PdfTextElement class with the gate text and PdfFont & draw
            result = new PdfTextElement($"{model.Gate}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom));


            //Create a text element with the seat text and font.
            var rightSeat = new PdfTextElement("Seat", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult seatResult = rightSeat.Draw(currentPage, new PointF(gateRight, seatY));
            float seatRightX = seatResult.Bounds.Right + 17;
            //Initializes a new instance of the PdfTextElement class with the seat text and PdfFont & draw
            seatResult = new PdfTextElement($"{model.Seat}", contentFont).Draw(currentPage, new PointF(seatResult.Bounds.X, seatResult.Bounds.Bottom));

            //Create a text element with the Flight text and font.
            var rightFlight = new PdfTextElement("Flight", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult rightflightResult = rightFlight.Draw(currentPage, new PointF(seatRightX, seatY));
            //Initializes a new instance of the PdfTextElement class with the Flight text and PdfFont & draw
            rightflightResult = new PdfTextElement($"{model.Flight}", contentFont).Draw(currentPage, new PointF(rightflightResult.Bounds.X, rightflightResult.Bounds.Bottom));
            

            //Create a text element with the date text and font.
            var rightDate = new PdfTextElement("Date", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult rightDateResult = rightDate.Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 6));
            float flightBottom = rightDateResult.Bounds.Y;
            //Initializes a new instance of the PdfTextElement class with the date text and PdfFont & draw
            rightDateResult = new PdfTextElement($"{model.Date}", contentFont).Draw(currentPage, new PointF(rightDateResult.Bounds.X, rightDateResult.Bounds.Bottom));


            //Create a text element with the time text and font.
            var rightTime = new PdfTextElement("Time", titleFont);
            //Draws the element on the page with the specified page and PointF structure
            PdfLayoutResult rightTimeResult = rightTime.Draw(currentPage, new PointF(seatRightX, flightBottom));
            //Initializes a new instance of the PdfTextElement class with the time text and PdfFont & draw
            rightTimeResult = new PdfTextElement($"{model.Time}", contentFont).Draw(currentPage, new PointF(rightTimeResult.Bounds.X, rightTimeResult.Bounds.Bottom ));

            return result;
            
        }
    }
}
