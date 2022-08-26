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
    internal class BoardingPassDocument
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
            PdfDocument document = new PdfDocument();
            document.PageSettings.Orientation = PdfPageOrientation.Landscape;
            document.PageSettings.Size = new SizeF(700, 288);
            document.PageSettings.Margins.All = 0;
            currentPage = document.Pages.Add();
            
            //Left Side Boarding Pass
            RectangleF rectangle = new RectangleF(0,0,700,38);
            currentPage.Graphics.DrawRectangle(PdfBrushes.DarkBlue, rectangle);
            FileStream imageStream= new FileStream(@"../../../Assets/Image/images.png", FileMode.Open, FileAccess.Read);
            PdfBitmap image = new PdfBitmap(imageStream);
            currentPage.Graphics.DrawImage(image,(700/2)-50,0, 20, 38);
            PdfFont titleFont = new PdfTrueTypeFont(headerfontStream, 15);
            PdfStringFormat stringFormat = new PdfStringFormat();
            stringFormat.Alignment = PdfTextAlignment.Right;
            currentPage.Graphics.DrawString("BOARDING PASS", titleFont, PdfBrushes.White, new PointF(500-20, 8), stringFormat);
            float y = rectangle.Height + 20;

            //Airlines
            RectangleF rect = new RectangleF(0, 0, 64, 288);
            currentPage.Graphics.DrawRectangle(PdfBrushes.Red, rect);
            PdfFont leftFont = new PdfTrueTypeFont(headerfontStream, 15);
            PdfStringFormat format = new PdfStringFormat();
            stringFormat.Alignment = PdfTextAlignment.Center;
            stringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            //currentPage.Graphics.TranslateTransform(0, 288/ 2);
            //currentPage.Graphics.RotateTransform(-90);
            currentPage.Graphics.DrawString("Your Airlines", leftFont, PdfBrushes.White, new Point(0, 288 / 2), format);
            //currentPage.Graphics.RotateTransform(90);
            float x = rect.Width + 25;

            PdfLayoutResult result = FormDetails(x,y);
            
            document.Save(stream);
            document.Close(true);
            
        }
        public PdfLayoutResult FormDetails(float x, float y)
        {
            FileStream titlefontStream=new FileStream(@"../../../Assets/Fonts/OpenSans-Light.ttf", FileMode.Open, FileAccess.Read);
            PdfFont titleFont = new PdfTrueTypeFont(titlefontStream, 12);
            
            FileStream contentfontStream = new FileStream(@"../../../Assets/Fonts/OpenSans-Bold.ttf", FileMode.Open, FileAccess.Read);
            PdfFont contentFont = new PdfTrueTypeFont(contentfontStream, 12);

            //Left side:
            //Passenger Name
            var headerText = new PdfTextElement("Passenger Name", titleFont);
            var result = headerText.Draw(currentPage, new Point((int)x,(int) y));
            result = new PdfTextElement($" {model.PassengerName}", contentFont).Draw(currentPage, new PointF(x, result.Bounds.Bottom + 2));

            //From
            var from = new PdfTextElement("From", titleFont);
            result = from.Draw(currentPage, new Point((int)x,(int)result.Bounds.Bottom+15));
            float flightY = result.Bounds.Y;
            float flightX=result.Bounds.Right;
            result = new PdfTextElement($" {model.From}", contentFont).Draw(currentPage, new PointF(x, result.Bounds.Bottom + 2));

            PdfFont ChangepdfFont = new PdfTrueTypeFont(contentfontStream, 18);
            //Flight
            var flight = new PdfTextElement("Flight", titleFont);
            PdfLayoutResult flightResult = flight.Draw(currentPage, new Point((int)flightX+120, (int)flightY));
            float dateY = flightResult.Bounds.Y;
            float dateR = flightResult.Bounds.Right;
            flightResult = new PdfTextElement($"{model.Flight}", ChangepdfFont).Draw(currentPage, new PointF((int)flightX + 120, flightY + 15));
            float barcodeStartPoint = flightResult.Bounds.X;

            //Date
            var date = new PdfTextElement("Date", titleFont);
            PdfLayoutResult dateResult = date.Draw(currentPage, new Point((int)dateR + 100, (int)dateY));
            dateResult = new PdfTextElement($"{model.Date}", ChangepdfFont).Draw(currentPage, new PointF((int)dateR + 100, dateY + 15));
            float datebottom = dateResult.Bounds.Bottom;

            //To
            var destination = new PdfTextElement("To", titleFont);
            result = destination.Draw(currentPage, new Point((int)x, (int)result.Bounds.Bottom + 15));
            result = new PdfTextElement($"{model.To}", contentFont).Draw(currentPage, new PointF(x, result.Bounds.Bottom + 2));
            float toright = result.Bounds.Bottom;

            //Time
            var time = new PdfTextElement("Time", titleFont);
            PdfLayoutResult timeResult = time.Draw(currentPage, new Point((int)dateR + 100,(int) datebottom+10));           
            timeResult = new PdfTextElement($"{model.Time}", ChangepdfFont, PdfBrushes.Red).Draw(currentPage, new PointF((int)dateR + 100, timeResult.Bounds.Bottom + 2));
            float timebottom = timeResult.Bounds.Bottom;

            //Gate
            var gate = new PdfTextElement("Gate", titleFont);
            result = gate.Draw(currentPage, new Point((int)x, (int)result.Bounds.Bottom + 15));
            float gateX = result.Bounds.Right + 30;
            result = new PdfTextElement($"{model.Gate}", ChangepdfFont).Draw(currentPage, new PointF(x, result.Bounds.Bottom));           
            

            //Seat
            var seat = new PdfTextElement("Seat", titleFont);
            PdfLayoutResult seatresult = seat.Draw(currentPage, new Point((int)gateX, (int)toright+15));
            seatresult = new PdfTextElement($"{model.Seat}", ChangepdfFont).Draw(currentPage, new PointF((int)seatresult.Bounds.X, (int)seatresult.Bounds.Bottom));

            FileStream FooterStream = new FileStream(@"../../../Assets/Fonts/Open Sans Light.ttf", FileMode.Open, FileAccess.Read);
            PdfFont footerFont = new PdfTrueTypeFont(FooterStream, 8);
            var footerText = new PdfTextElement("*Gate Closes 30 Minutes Before Departure", footerFont, PdfBrushes.Red);
            result = footerText.Draw(currentPage, new Point((int)x,(int) result.Bounds.Bottom + 18));

            //Barcode
            PdfCode39Barcode barcode = new PdfCode39Barcode();
            barcode.Text = "CODE39$";
            barcode.Size = new SizeF(dateResult.Bounds.Right-240, 30);
            barcode.TextDisplayLocation = TextLocation.None;
            barcode.Draw(currentPage, new PointF(barcodeStartPoint, (int)timebottom+18));

            //DashLine
            var pen = new PdfPen(Color.Gray, 1);
            pen.DashStyle = PdfDashStyle.Dash;
            pen.DashOffset = 0.5f;
            currentPage.Graphics.DrawLine(pen, 500, 0, 500, currentPage.GetClientSize().Height);


            //Right side Boarding Pass:
            PdfFont righttitleFont = new PdfTrueTypeFont(headerfontStream, 15);
            currentPage.Graphics.DrawString("BOARDING PASS", righttitleFont, PdfBrushes.White, new PointF(500 + 30, 8));

            //PassengerName
            var rightPassengerName = new PdfTextElement("Passenger Name", titleFont);
            result = rightPassengerName.Draw(currentPage, new Point(500 + 20, (int)y));
            result = new PdfTextElement($" {model.PassengerName}", contentFont).Draw(currentPage, new PointF(500 + 20, result.Bounds.Bottom + 2));

            //From
            var rightFrom = new PdfTextElement("From", titleFont);
            result = rightFrom.Draw(currentPage, new Point(500 + 20,(int) result.Bounds.Bottom+15));           
            result = new PdfTextElement($" {model.From}", contentFont).Draw(currentPage, new PointF(500 + 20,(int)result.Bounds.Bottom +2));

            //To
            var rightTo = new PdfTextElement("To", titleFont);
            result = rightTo.Draw(currentPage, new Point(500 + 20, (int)result.Bounds.Bottom + 15));         
            result = new PdfTextElement($"{model.To}", contentFont).Draw(currentPage, new PointF(500 + 20, (int)result.Bounds.Bottom + 2));
            float tobottom = result.Bounds.Bottom;

            //Flight
            var rightFlight = new PdfTextElement("Flight", titleFont);
            result = rightFlight.Draw(currentPage, new Point(500 + 20, (int)result.Bounds.Bottom + 10));           
            result = new PdfTextElement($"{model.Flight}", contentFont).Draw(currentPage, new PointF(500 + 20, (int)result.Bounds.Bottom + 2));
            float flightRight = result.Bounds.Right;
            float rightflightbottom = result.Bounds.Bottom + 10;

            //Date
            var rightDate = new PdfTextElement("Date", titleFont);
            PdfLayoutResult dateresult = rightDate.Draw(currentPage, new Point((int)flightRight + 50, (int)tobottom +10));
            float dateLeft = dateresult.Bounds.Left ;
            dateresult = new PdfTextElement($"{model.Date}", contentFont).Draw(currentPage, new PointF((int)flightRight +50, (int)dateresult.Bounds.Bottom + 2));
            float datecontentBttom = dateresult.Bounds.Bottom;

            //Gate
            var rightGate = new PdfTextElement("Gate", titleFont);
            result = rightGate.Draw(currentPage, new Point(500 + 20, (int)result.Bounds.Bottom + 10));
            float gateRight = result.Bounds.Right;
            result = new PdfTextElement($"{model.Gate}", contentFont).Draw(currentPage, new PointF(500 + 20, (int)result.Bounds.Bottom + 2));

            //Seat
            var rightseat = new PdfTextElement("Seat", titleFont);
            PdfLayoutResult seatResult = rightseat.Draw(currentPage, new Point((int)gateRight + 20, (int)rightflightbottom));
            seatResult = new PdfTextElement($"{model.Seat}", contentFont).Draw(currentPage, new PointF((int)gateRight + 20, seatResult.Bounds.Bottom + 2));

            //Time
            var righttime = new PdfTextElement("Time", titleFont);
            PdfLayoutResult righttimeResult = righttime.Draw(currentPage, new Point((int)dateLeft, (int)datecontentBttom+10));   
            righttimeResult = new PdfTextElement($"{model.Time}", contentFont).Draw(currentPage, new PointF((int)dateLeft , righttimeResult.Bounds.Bottom + 2));

            return result;
            
        }
    }
}
