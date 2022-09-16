using Invoice.Model;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System;
using System.Data;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Invoice
{
    public class InvoiceDocument
    {

        InvoiceModel model;
        SizeF clientSize;
        //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTrueTypeFont class.
        private FileStream standardFontStream = new FileStream("../../../Assets/OpenSans-Regular.ttf", FileMode.Open, FileAccess.Read);
        PdfFont contentFont = null;
        PdfPage currentPage;
        PdfDocument document;
        float bottomValue = 0;
        int pageCount = 1;

        public InvoiceDocument(InvoiceModel model)
        {
            this.model = model;
        }

        public void GeneratePdf(Stream stream)
        {
            //Create a new PDF document.
            document = new PdfDocument();
            //Represents the method that executes on a PdfDocument when a new page is created.
            document.Pages.PageAdded += Pages_PageAdded;
            //Add a page to the document.
            currentPage = document.Pages.Add();
            //Get the PDF page size reduced by page margins and page template dimensions.
            clientSize = currentPage.GetClientSize();
            PdfLayoutResult result;

            result = ComposeHeader();

            result = ComposeAddress(result.Bounds);

            result = ComposeTable(result.Bounds);

            document.Save(stream);
            document.Close(true);
        }

        private void Pages_PageAdded(object sender, PageAddedEventArgs args)
        {
            this.currentPage = args.Page;
            this.pageCount++;
        }

        public PdfLayoutResult ComposeHeader()
        {
            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTrueTypeFont class.
            PdfFont contentFont = new PdfTrueTypeFont(standardFontStream, 8);       
            
            //Initializes a new instance of the Syncfusion.Drawing.RectangleF class.
            RectangleF bounds = new RectangleF(0, 0, currentPage.GetClientSize().Width, 50);
            // It represents a graphics of the page.
            PdfGraphics graphics = currentPage.Graphics;
            FileStream stream = new FileStream(@"../../../Assets/icon.png", FileMode.Open);
            stream.Position = 0;
            //Creates new PdfBitmap instance.
            PdfImage icon = new PdfBitmap(stream);
            PointF iconLocation = new PointF(14, 13);
            SizeF iconSize = new SizeF(40, 40);
            //Draws the specified Image at the specified location and with the specified size.
            graphics.DrawImage(icon, iconLocation, iconSize);

            float leftBottom = iconLocation.Y + iconSize.Height;
            /*Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTextElement class
            with the text and Syncfusion.Pdf.Graphics.PdfFont.*/
            contentFont = new PdfTrueTypeFont(standardFontStream, 20,PdfFontStyle.Bold);
            var headerText = new PdfTextElement("INVOICE",contentFont, new PdfSolidBrush(Color.FromArgb(1, 53, 67, 168)));
            //Gets or sets the Syncfusion.Pdf.Graphics.PdfStringFormat that will be used  to set the string format
            headerText.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            //Draws the element on the page with the specified page and Syncfusion.Drawing.PointF structure
            var result = headerText.Draw(currentPage, new PointF(clientSize.Width - 25, iconLocation.Y+10));
            return result;
        }

        public PdfLayoutResult ComposeAddress(RectangleF bounds)
        {
            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTrueTypeFont class.
            contentFont = new PdfTrueTypeFont(standardFontStream, 10);
            float y = bounds.Bottom + 38;

            //draw the text
            currentPage.Graphics.DrawString("To", contentFont, PdfBrushes.Black, new PointF(14, y - 9));

            /*Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTextElement class
            with the text and Syncfusion.Pdf.Graphics.PdfFont.*/
            contentFont = new PdfTrueTypeFont(standardFontStream, 12,PdfFontStyle.Bold);

            var headerText = new PdfTextElement(model.CustomerAddress.Name, contentFont);
            //Draws the bill to element on the page with the specified page and Syncfusion.Drawing.PointF structure
            var result = headerText.Draw(currentPage, new PointF(14, y + 4));

            string address = String.Empty;
            if (!string.IsNullOrEmpty(model.CustomerAddress.Street))
                address = model.CustomerAddress.Street + "\n";
            if (!string.IsNullOrEmpty(model.CustomerAddress.City))
                address += model.CustomerAddress.City + "\n";
            if (!string.IsNullOrEmpty(model.CustomerAddress.State))
                address += model.CustomerAddress.State + "\n";
            if (!string.IsNullOrEmpty(model.CustomerAddress.Email))
                address += model.CustomerAddress.Email + "\n";
            if (!string.IsNullOrEmpty(model.CustomerAddress.Phone))
                address +="Phone: "+ model.CustomerAddress.Phone + "\n";
            contentFont = new PdfTrueTypeFont(standardFontStream, 10);
            headerText = new PdfTextElement(address, contentFont);            
            //Draws the address element on the page with the specified page and Syncfusion.Drawing.PointF structure
            PdfLayoutResult addressLayoutresult = headerText.Draw(currentPage, new PointF(14, result.Bounds.Bottom + 3));
            contentFont = new PdfTrueTypeFont(standardFontStream, 10,PdfFontStyle.Bold);
            headerText = new PdfTextElement("Invoice No. " + model.InvoiceNumber, contentFont);
            headerText.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            PdfLayoutResult layoutresult = headerText.Draw(currentPage, new PointF(clientSize.Width - 25, y - 8.2f));

            contentFont = new PdfTrueTypeFont(standardFontStream, 10);
            //Initializes a new instance of the Syncfusion.Pdf.Grid.PdfGrid class.
            PdfGrid pdfGrid = new PdfGrid();
            pdfGrid.Style.Font = contentFont;
            //Add new row to the grid.
            PdfGridRow row1 = pdfGrid.Rows.Add();            
            PdfGridRow row2 = pdfGrid.Rows.Add();            
            PdfGridRow row3 = pdfGrid.Rows.Add();            
            PdfGridRow row4 = pdfGrid.Rows.Add();
            //Adds the number of specified column count.
            pdfGrid.Columns.Add(2);
            //sets the width of the Syncfusion.Pdf.Grid.PdfGridColumn.
            pdfGrid.Columns[0].Width = 60;
            pdfGrid.Columns[1].Width = 95;
            

            //sets the vertical text alignment.
            pdfGrid.Rows[0].Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            //sets the value of the cell.
            pdfGrid.Rows[0].Cells[0].Value = "Inovice Date";
            //sets the value of the cell.
            pdfGrid.Rows[0].Cells[1].Value = $"{": " + model.IssueDate}";
            //sets the vertical text alignment.
            pdfGrid.Rows[0].Cells[1].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

            //sets the vertical text alignment.
            pdfGrid.Rows[1].Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            //sets the value of the cell.
            pdfGrid.Rows[1].Cells[0].Value = "Due Date";
            //sets the value of the cell.
            pdfGrid.Rows[1].Cells[1].Value = $"{": " + model.DueDate}";
            //sets the vertical text alignment.
            pdfGrid.Rows[1].Cells[1].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

            //sets the vertical text alignment.
            pdfGrid.Rows[2].Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            //sets the value of the cell.
            pdfGrid.Rows[2].Cells[0].Value = "Account No";
            //sets the value of the cell.
            pdfGrid.Rows[2].Cells[1].Value = $"{": " + model.RefNumber}";
            //sets the vertical text alignment.
            pdfGrid.Rows[2].Cells[1].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            for (int i = 0; i < pdfGrid.Rows.Count; i++)
            {
                //sets the border of the Syncfusion.Pdf.Grid.PdfGridCell.
                pdfGrid.Rows[i].Cells[0].Style.Borders.All = PdfPens.Transparent; 
                pdfGrid.Rows[i].Cells[1].Style.Borders.All = PdfPens.Transparent;               
            }
           
            pdfGrid.Style.Font = contentFont;

            //Initializes a new instance of the Syncfusion.Pdf.Grid.PdfGridStyle class.
            PdfGridStyle gridStyle = new PdfGridStyle();
            //sets the cell padding.
            gridStyle.CellPadding = new PdfPaddings(1, 1, 1, 1);
            //sets the grid style.
            pdfGrid.Style = gridStyle;

            //divide the page width
            float x = (clientSize.Width/2)+128;
            //Initializes a new instance of the Syncfusion.Pdf.Grid.PdfGridLayoutFormat class.
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            //sets layout type of the element.
            layoutFormat.Layout = PdfLayoutType.Paginate;
            //Draws the PdfGrid in the specified PdfPage.
           PdfLayoutResult tableLayoutResult = pdfGrid.Draw(currentPage, x, result.Bounds.Bottom + 3, clientSize.Width-30,layoutFormat);

            if (addressLayoutresult.Bounds.Bottom > tableLayoutResult.Bounds.Bottom)
                bottomValue = addressLayoutresult.Bounds.Bottom;
            else
                bottomValue = tableLayoutResult.Bounds.Bottom;
            return result;
        }

        public PdfLayoutResult ComposeTable(RectangleF prevBounds)
        {
            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTrueTypeFont class.
            contentFont = new PdfTrueTypeFont(standardFontStream, 10,PdfFontStyle.Regular);

            //Initializes a new instance of the Syncfusion.Pdf.Grid.PdfGrid class.
            PdfGrid grid = new PdfGrid();
            grid.Style.Font = contentFont;
            //Adds the number of specified count.
            grid.Columns.Add(4);
            //sets the width of the Syncfusion.Pdf.Grid.PdfGridColumn.
            grid.Columns[1].Width = 70;
            grid.Columns[2].Width = 70;
            grid.Columns[3].Width = 70;
            // Add rows to the header at run time.
            grid.Headers.Add(1);

            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfStringFormat class with horizontal and vertical alignment
            PdfStringFormat stringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);

            //Gets the headers collection from the PdfGrid.[Read - Only]
            PdfGridRow header = grid.Headers[0];
            //sets the value of the cell.

            //sets the value of the cell.
            header.Cells[0].Value = "Item & description";
            header.Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            //sets the value of the cell.
            header.Cells[1].Value = "Hrs/Qty";
            //set the string format
            header.Cells[1].StringFormat = stringFormat;
            //sets the value of the cell.
            header.Cells[2].Value = "Rate($)";
            //set the string format
            header.Cells[2].StringFormat = stringFormat;

            header.Cells[3].Value = "Amount($)";
            //set the string format
            header.Cells[3].StringFormat = stringFormat;

            PdfGridCellStyle cellStyle = new PdfGridCellStyle();
            cellStyle.Borders.Right = PdfPens.Transparent;
            cellStyle.Borders.Left = PdfPens.Transparent;
            cellStyle.Borders.Top = PdfPens.Transparent;
            cellStyle.Borders.Bottom = PdfPens.Transparent;

            for (int j = 0; j < grid.Headers[0].Cells.Count; j++)
            {
                //Apply the style to header
                grid.Headers[0].Cells[j].Style = cellStyle;
                //Apply the trasnparent color to hide the background 
                grid.Headers[0].Cells[j].Style.TextBrush = PdfBrushes.White;
            }
          
            //Initializes a new instance of the PdfGridCellStyle class.                  
            for (int j = 0; j < grid.Headers[0].Cells.Count; j++)
            {
                //sets the cell style
                grid.Headers[0].Cells[j].Style = cellStyle;
                //Gets the cells from the selected row.
                PdfGridCell cell = header.Cells[j];
                //sets the background brush. 
                cell.Style.BackgroundBrush = new PdfSolidBrush(Color.FromArgb(1, 53, 67, 168));
            }

            decimal sum = 0;
            int index = 0;
            foreach (var item in model.Items)
            {
                //Add new row to the grid.
                PdfGridRow row = grid.Rows.Add();
                //sets the value of the cell.
                row.Cells[0].Value = item.Name[index];
                //sets the vertical text alignment.
                row.Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

                //sets the value of the cell.
                row.Cells[1].Value = $"{item.Qty + " "}";
                //sets the string format.
                row.Cells[1].StringFormat = stringFormat;
                //sets the vertical text alignment.
                row.Cells[1].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

                //sets the value of the cell.
                row.Cells[2].Value = $"{item.Rate}";
                //sets the string format.
                row.Cells[2].StringFormat = stringFormat;
                //sets the vertical text alignment.
                row.Cells[2].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

                //calculate the amount 
                decimal amount = item.Rate * item.Qty;
                //sets the value of the cell.
                row.Cells[3].Value = String.Format("{0:0.##}", amount);
                //sets the string format.
                row.Cells[3].StringFormat = stringFormat;
                //sets the vertical text alignment.
                row.Cells[3].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
                sum += amount;
                index++;
            }
            grid.ApplyBuiltinStyle(PdfGridBuiltinStyle.PlainTable3);
            //Initializes a new instance of the Syncfusion.Pdf.Grid.PdfGridStyle class.
            PdfGridStyle gridStyle = new PdfGridStyle();
            //sets the cell padding
            gridStyle.CellPadding = new PdfPaddings(5, 5, 5, 5);
            //sets the grid style
            grid.Style = gridStyle;

            //Initializes a new instance of the Syncfusion.Pdf.Grid.PdfGridLayoutFormat class.
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            //sets layout type of the element.
            layoutFormat.Layout = PdfLayoutType.Paginate;
            //Draws the PdfGrid in the specified PdfPage.
            PdfLayoutResult result = grid.Draw(currentPage, 14, bottomValue + 30, clientSize.Width - 35, layoutFormat);
            float resultWidth = result.Bounds.Width;
            if (pageCount > 1)
                resultWidth = clientSize.Width - 50;
            //Initializes a new instance of the PdfTextElement class with the text and PdfFont
            PdfTextElement element = new PdfTextElement("Sub Total", contentFont);
            //Measures a string by using this font.
            SizeF fontSize = contentFont.MeasureString(element.Text);
            float y = result.Bounds.Bottom;
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(result.Bounds.Width - (contentFont.Size * 9) - 30, result.Bounds.Bottom + 22, resultWidth, result.Bounds.Height));
            float totalWidth = result.Bounds.X;

            //get the sub total amount
            var totalPrice = $"{Math.Round(sum, 2)}";
            //Initializes a new instance of the PdfTextElement class with the text and PdfFont
            element = new PdfTextElement(totalPrice, contentFont);
            element.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(30, y + 23, result.Bounds.Width, result.Bounds.Height));


            y = result.Bounds.Bottom + 10;
            //Initializes a new instance of the PdfTextElement class with the text and PdfFont
            element = new PdfTextElement("Val Tax1 (12.5%)", contentFont);
            //Measures a string by using this font.
            SizeF size = contentFont.MeasureString(element.Text);
            if (y > clientSize.Height)
            {
                y = y - clientSize.Height;
                document.Pages.Add();
            }
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(totalWidth - size.Width + fontSize.Width, y, result.Bounds.Width, result.Bounds.Height));
            float firstTaxWidth = result.Bounds.X;
            OrderItem tax = new OrderItem();
            //calculate the tax value
            decimal sampletax1 = sum * (tax.SampleTax1 / 100);
            element = new PdfTextElement(String.Format("{0:0.##}", sampletax1), contentFont);
            element.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(30, y, result.Bounds.Width, result.Bounds.Height));
            SizeF fontsize = contentFont.MeasureString(element.Text);
            float width = size.Width + fontsize.Width;                    
            y = result.Bounds.Bottom + 9;
            if (y > clientSize.Height)
            {
                y = y - clientSize.Height;
                document.Pages.Add();
            }

            //draw the rectangle
            currentPage.Graphics.DrawRectangle(new PdfSolidBrush(Color.FromArgb(255, 239, 242, 255)), new RectangleF(totalWidth - size.Width + fontSize.Width, result.Bounds.Bottom + 10,width+63, 9 + 18));

            contentFont = new PdfTrueTypeFont(standardFontStream, 10, PdfFontStyle.Bold);
            //Initializes a new instance of the PdfTextElement class with the text and PdfFont
            element = new PdfTextElement("Total", contentFont);
            SizeF textSize = contentFont.MeasureString(element.Text);
            float x = totalWidth - textSize.Width + fontSize.Width;
            y = y + 9;
            //Draws the element on the page with the specified page and RectangleF structure
            element.Draw(currentPage, new RectangleF(x, y, result.Bounds.Width, textSize.Height));
            decimal total = sampletax1 + sum;
            totalPrice = $"$ {String.Format("{0:0.##}", total)}";

            contentFont = new PdfTrueTypeFont(standardFontStream, 10, PdfFontStyle.Bold);
            //Initializes a new instance of the PdfTextElement class with the text and PdfFont
            element = new PdfTextElement(totalPrice, contentFont);
            textSize = contentFont.MeasureString(element.Text);
            //sets the string format
            element.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(30, y, result.Bounds.Width, textSize.Height));

            //draw the line
            y = result.Bounds.Bottom + 45;
            if (y > clientSize.Height)
            {
                y = y - clientSize.Height;
                document.Pages.Add();
            }
            contentFont = new PdfTrueTypeFont(standardFontStream, 10,PdfFontStyle.Regular);
            //Initializes a new instance of the PdfTextElement class with the text and PdfFont
            element = new PdfTextElement("Payment Method", contentFont);
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(30, y, result.Bounds.Width, element.Font.Height));
            y = result.Bounds.Bottom + 2;

            contentFont = new PdfTrueTypeFont(standardFontStream,8);
            element = new PdfTextElement("Paypal : payments @websitename.com", contentFont);
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(30, y, result.Bounds.Width + 10, element.Font.Height));
            y = result.Bounds.Bottom + 2;
            if (y > clientSize.Height)
            {
                y = y - clientSize.Height;
                document.Pages.Add();
            }
            //Initializes a new instance of the PdfTextElement class with the text and PdfFont
            element = new PdfTextElement("Card payment we accept : Visa,Mastercar,Payoneer", contentFont);
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(30, y, result.Bounds.Width, result.Bounds.Height));

            //draw the line            
            return result;
        }

    }

}

