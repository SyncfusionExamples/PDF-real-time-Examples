##### Example: BoardingPass

# Purpose
The BoardingPass project is an example of the boarding pass generation. The example demonstrates a simple one-page document that includes a text, images. This example also demonstrates the usage of margins.

The example source is available in [repo](https://github.com/SyncfusionExamples/PDF-real-time-Examples/BoardingPass).

# Prerequisites
1) **Visual Studio 2017** or above is installed.
   To install a community version of Visual Studio use the following link: https://visualstudio.microsoft.com/vs/community/
   Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.

2) **.NET Core SDK 2.1** or above is installed.
   To install the framework use the following link: https://dotnet.microsoft.com/download

# Description

### Image
Image of the logo of the airplane is located in the **Assets/Image/images.png*.

### Output file
The example creates the file **BoardingPass.pdf** in the output **bin/(debug|release)/net6.0** folder.

# Writing the source code

#### 1. Create new console application.
1.1.	Run Visual Studio  
1.2.	File -> Create -> Console Application (.Net Core)

#### 2. Modify class Program.
2.1. Set the path to the output PDF-file In the Program.cs:

```c#
     FileStream fs = new FileStream("BoardingPass.pdf", FileMode.Create);
```
2.2. Call the **GetDetails()** method  for the boarding pass PDF generation 

```c#
   BoardingPassModel model = DataSourceBoardingPassDocument.GetDetails();
   BoardingPassDocument document = new BoardingPassDocument(model);
   document.GeneratePdf(fs);
   fs.Close();
```
#### 3. Create class for BoardingPassModel to get and set boarding pass data.

```c#
    public class BoardingPassModel
    {
        public string PassengerName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Flight { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Gate { get; set; }
        public string Seat { get; set; }
        
    }
```

#### 4. Create class for boarding pass data source to get and set boarding pass details.

```c#
     public static class DataSourceBoardingPassDocument
     {
        public static BoardingPassModel GetDetails()
        {
           var dateTime = DateTime.Now;
            
           return new BoardingPassModel
            {
               PassengerName = "JOHN SMITH",
               From = "Moscow",
               To = "San Francisco",
               Flight = "85SKL",
               Date= dateTime.ToString("dd MMM yyyy"),
               Time = dateTime.ToString("t"),
               Gate = "08",
               Seat = "15B",
            };
        }
    }
```	

#### 5. Create the BoardingPassDocument class which will contain the method to build the document structure 

```c#
public class BoardingPassDocument
```
5.1. Created a new document and set the document settings

```c#
            //Create a new PDF document.
            PdfDocument document = new PdfDocument();
            //Change a page orientation to landscape.
            document.PageSettings.Orientation = PdfPageOrientation.Landscape;
            //set the page size.
            document.PageSettings.Size = new SizeF(700, 288);
            //Set the margin is 0.
            document.PageSettings.Margins.All = 0;
            //Add a page to the document.
            currentPage = document.Pages.Add();
```
5.2. Add the header text and image

```c#
            //Left Side Boarding Pass and image
            RectangleF rectangle = new RectangleF(0,0,700,38);
            currentPage.Graphics.DrawRectangle(PdfBrushes.DarkBlue, rectangle);
            PdfFont titleFont = new PdfTrueTypeFont(headerfontStream, 15);
            var headerText = new PdfTextElement("BOARDING PASS", titleFont, PdfBrushes.White);
            headerText.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            PdfLayoutResult boardingResult = headerText.Draw(currentPage, new Point(500 - 40, 8));
            float imageX = boardingResult.Bounds.X-50;
            FileStream imageStream = new FileStream(@"../../../Assets/Image/images.png", FileMode.Open, FileAccess.Read);
            PdfBitmap image = new PdfBitmap(imageStream);            
            currentPage.Graphics.DrawImage(image, imageX, 0, 30, 38);
            float y = rectangle.Height + 20;

```
5.3. Create FormDetails() method 

The Passenger details in left and side of the ticket is added in this method
```c#
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
            float flightR=result.Bounds.Right+130;
            result = new PdfTextElement($" {model.From}", contentFont).Draw(currentPage, new PointF(x, result.Bounds.Bottom + 2));

            PdfFont changeContentFont = new PdfTrueTypeFont(contentfontStream, 18);
            //Flight
            var flight = new PdfTextElement("Flight", titleFont);
            PdfLayoutResult flightResult = flight.Draw(currentPage, new Point((int)flightR, (int)flightY));
            float dateY = flightResult.Bounds.Y;
            float dateR = flightResult.Bounds.Right +100 ;
            flightResult = new PdfTextElement($"{model.Flight}", changeContentFont).Draw(currentPage, new PointF(flightResult.Bounds.X, flightResult.Bounds.Bottom));
            float barcodeStartPoint = flightResult.Bounds.X;

            //Date
            var date = new PdfTextElement("Date", titleFont);
            PdfLayoutResult dateResult = date.Draw(currentPage, new Point((int)dateR, (int)dateY));
            dateResult = new PdfTextElement($"{model.Date}", changeContentFont).Draw(currentPage, new PointF(dateResult.Bounds.X /*dateR*/, dateResult.Bounds.Bottom));
            float timeY = dateResult.Bounds.Bottom;

            //To
            var destination = new PdfTextElement("To", titleFont);
            result = destination.Draw(currentPage, new Point((int)x, (int)result.Bounds.Bottom + 15));
            result = new PdfTextElement($"{model.To}", contentFont).Draw(currentPage, new PointF(x, result.Bounds.Bottom + 2));

            //Time
            var time = new PdfTextElement("Time", titleFont);
            PdfLayoutResult timeResult = time.Draw(currentPage, new Point((int)dateR,(int)timeY + 10));           
            timeResult = new PdfTextElement($"{model.Time}", changeContentFont, PdfBrushes.Red).Draw(currentPage, new PointF(timeResult.Bounds.X, timeResult.Bounds.Bottom));
            float barcodeEndPoint = timeResult.Bounds.Bottom;

            //Gate
            var gate = new PdfTextElement("Gate", titleFont);
            result = gate.Draw(currentPage, new Point((int)x, (int)result.Bounds.Bottom + 15));
            float seatY = result.Bounds.Y;
            float seatX = result.Bounds.Right + 30;
            result = new PdfTextElement($"{model.Gate}", changeContentFont).Draw(currentPage, new PointF(x, result.Bounds.Bottom));           
            

            //Seat
            var seat = new PdfTextElement("Seat", titleFont);
            PdfLayoutResult leftSeatResult = seat.Draw(currentPage, new Point((int)seatX, (int)seatY));
            leftSeatResult = new PdfTextElement($"{model.Seat}", changeContentFont).Draw(currentPage, new PointF(leftSeatResult.Bounds.X, leftSeatResult.Bounds.Bottom));


            PdfFont footerFont = new PdfTrueTypeFont(titlefontStream, 8);
            var footerText = new PdfTextElement("*Gate Closes 30 Minutes Before Departure", footerFont, PdfBrushes.Red);
            result = footerText.Draw(currentPage, new Point((int)x,(int) result.Bounds.Bottom + 18));

            //Barcode
            PdfCode39Barcode barcode = new PdfCode39Barcode();
            barcode.Text = "CODE39$";
            barcode.Size = new SizeF(dateResult.Bounds.Right/2, 30);
            barcode.TextDisplayLocation = TextLocation.None;
            barcode.Draw(currentPage, new PointF(barcodeStartPoint, barcodeEndPoint + 18));

            //DashLine
            var pen = new PdfPen(Color.Gray, 1);
            pen.DashStyle = PdfDashStyle.Dash;
            pen.DashOffset = 0.5f;
            currentPage.Graphics.DrawLine(pen, 500, 0, 500, currentPage.GetClientSize().Height);



            //Right side Boarding Pass:
            PdfFont rightTitleFont = new PdfTrueTypeFont(headerfontStream, 15);
            currentPage.Graphics.DrawString("BOARDING PASS", rightTitleFont, PdfBrushes.White, new PointF(500 + 30, 8));

            //PassengerName
            var rightPassengerName = new PdfTextElement("Passenger Name", titleFont);
            result = rightPassengerName.Draw(currentPage, new Point(500 + 20, (int)y));
            result = new PdfTextElement($" {model.PassengerName}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 2));

            //From
            var rightFrom = new PdfTextElement("From", titleFont);
            result = rightFrom.Draw(currentPage, new Point((int)result.Bounds.X, (int) result.Bounds.Bottom+15));           
            result = new PdfTextElement($" {model.From}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom +2));

            //To
            var rightTo = new PdfTextElement("To", titleFont);
            result = rightTo.Draw(currentPage, new Point((int)result.Bounds.X, (int)result.Bounds.Bottom + 15));         
            result = new PdfTextElement($"{model.To}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 2));
            float dateRight = result.Bounds.Right+2;

            //Flight
            var rightFlight = new PdfTextElement("Flight", titleFont);
            result = rightFlight.Draw(currentPage, new Point((int)result.Bounds.X, (int)result.Bounds.Bottom + 10));
            float flightBottom = result.Bounds.Y;
            result = new PdfTextElement($"{model.Flight}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 2));

            //Date
            var rightDate = new PdfTextElement("Date", titleFont);
            PdfLayoutResult rightDateResult = rightDate.Draw(currentPage, new Point((int)dateRight, (int)flightBottom));
            float dateLeft = rightDateResult.Bounds.Left ;
            rightDateResult = new PdfTextElement($"{model.Date}", contentFont).Draw(currentPage, new PointF(rightDateResult.Bounds.X, rightDateResult.Bounds.Bottom + 2));

            //Gate
            var rightGate = new PdfTextElement("Gate", titleFont);
            result = rightGate.Draw(currentPage, new Point((int)result.Bounds.X, (int)result.Bounds.Bottom + 10));
            float gateRight = result.Bounds.Right+20;
            float gateY =result.Bounds.Y;
            result = new PdfTextElement($"{model.Gate}", contentFont).Draw(currentPage, new PointF(result.Bounds.X, result.Bounds.Bottom + 2));

            //Seat
            var rightSeat = new PdfTextElement("Seat", titleFont);
            PdfLayoutResult seatResult = rightSeat.Draw(currentPage, new Point((int)gateRight,(int) gateY));
            seatResult = new PdfTextElement($"{model.Seat}", contentFont).Draw(currentPage, new PointF(seatResult.Bounds.X, seatResult.Bounds.Bottom + 2));

            //Time
            var rightTime = new PdfTextElement("Time", titleFont);
            PdfLayoutResult rightTimeResult = rightTime.Draw(currentPage, new Point((int)dateLeft, (int)gateY));   
            rightTimeResult = new PdfTextElement($"{model.Time}", contentFont).Draw(currentPage, new PointF(rightTimeResult.Bounds.X , rightTimeResult.Bounds.Bottom + 2));

            return result;
            
        }
```
5.4. Draw the rotated (Your Airlines) text 

```c#
//Airlines 
            RectangleF rect = new RectangleF(0, 0, 64, 288);
            currentPage.Graphics.DrawRectangle(PdfBrushes.Red, rect);
            float x = rect.Width + 25;
   
            PdfFont leftFont = new PdfTrueTypeFont(headerfontStream, 15);
            PdfStringFormat format = new PdfStringFormat();
            format.Alignment = PdfTextAlignment.Center;
            format.LineAlignment = PdfVerticalAlignment.Middle;
            currentPage.Graphics.TranslateTransform(64/2, 288/2);
            currentPage.Graphics.RotateTransform(-90);
            currentPage.Graphics.DrawString("Your Airlines", leftFont, PdfBrushes.White, new Point(0, 0), format);

```

#### 6. The generated **PDF file** must look as shown below:

The resulting BoardingPass.pdf document can be accessed [here](https://github.com/SyncfusionExamples/PDF-real-time-Examples/blob/BoardingPassProject/results/BoardingPass.pdf).





	 
