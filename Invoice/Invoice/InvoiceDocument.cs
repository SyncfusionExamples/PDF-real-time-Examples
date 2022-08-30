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
        FileStream standardFontStream = new FileStream("../../../Assets/calibri-regular.ttf", FileMode.Open, FileAccess.Read);
        FileStream boldFontStream = new FileStream("../../../Assets/calibri-bold.ttf", FileMode.Open, FileAccess.Read);
        PdfFont contentFont = null;
        PdfFont boldFont = null;
        PdfPage currentPage;
        PdfDocument document;
        float bottom = 0;
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
            PdfFont contentFont = new PdfTrueTypeFont(standardFontStream,8);
            boldFont = new PdfTrueTypeFont(boldFontStream,10);
            PdfFont boldTextFont = new PdfTrueTypeFont(boldFontStream,9);
            //Initializes a new instance of the Syncfusion.Drawing.RectangleF class.
            RectangleF bounds = new RectangleF(0, 0, currentPage.GetClientSize().Width, 50);
            // It represents a graphics of the page.
            PdfGraphics graphics = currentPage.Graphics;
            FileStream stream = new FileStream(@"../../../Assets/logo.png", FileMode.Open);
            stream.Position = 0;
            //Creates new PdfBitmap instance.
            PdfImage icon = new PdfBitmap(stream);
            PointF iconLocation= new PointF(29,54);
            SizeF iconSize = new SizeF(45, 38);
            //Draws the specified Image at the specified location and with the specified size.
            graphics.DrawImage(icon,iconLocation,iconSize);

            float leftBottom = iconLocation.Y + iconSize.Height;
            /*Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTextElement class
            with the text and Syncfusion.Pdf.Graphics.PdfFont.*/
            var headerText = new PdfTextElement(model.SellerAddress.Name,boldTextFont);
            //Gets or sets the Syncfusion.Pdf.Graphics.PdfStringFormat that will be used  to set the string format
            headerText.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            //Draws the element on the page with the specified page and Syncfusion.Drawing.PointF structure
            var result = headerText.Draw(currentPage, new PointF(clientSize.Width-25,iconLocation.Y));

            string address = String.Empty;
            if (!string.IsNullOrEmpty(model.SellerAddress.Street))
                address = model.SellerAddress.Street + "\n";
            if (!string.IsNullOrEmpty(model.SellerAddress.City))
                address+= model.SellerAddress.City + "\n";
            if (!string.IsNullOrEmpty(model.SellerAddress.State))
                address+= model.SellerAddress.State + "\n";
            if (!string.IsNullOrEmpty(model.SellerAddress.Email))
                address+= model.SellerAddress.Email + "\n";
            if (!string.IsNullOrEmpty(model.SellerAddress.Phone))
                address+= model.SellerAddress.Phone + "\n";

            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTextElement class with the text and Syncfusion.Pdf.Graphics.PdfFont.
            headerText = new PdfTextElement(address, contentFont);
            //Gets or sets the Syncfusion.Pdf.Graphics.PdfStringFormat that will be used to set the string format
            headerText.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            //Draws the address element on the page with the specified page and Syncfusion.Drawing.PointF structure
            result = headerText.Draw(currentPage, new PointF(clientSize.Width - 25, result.Bounds.Bottom+1));

            float bottomValue=0;
            if(leftBottom>result.Bounds.Bottom)
                bottomValue=leftBottom;
            else
                bottomValue=result.Bounds.Bottom;

            float y = bounds.Bottom + 46.56f;
            string text = "INVOICE";
            //Measures a string by using this font.
            SizeF textSize = boldFont.MeasureString(text);
            float lineWidthPosition = (clientSize.Width - 54 - textSize.Width - 20) / 2;

            //Draw the line
            currentPage.Graphics.DrawLine(new PdfPen(PdfBrushes.LightGray, 0.5f), new PointF(29, bottomValue + 10), new PointF(lineWidthPosition + 29, bottomValue + 10));

            //Draw the invoice text
            currentPage.Graphics.DrawString(text, boldFont, PdfBrushes.Black, new PointF(lineWidthPosition + 39,bottomValue+10-(textSize.Height/2))) ;
               
            //Draw the header
            currentPage.Graphics.DrawLine(new PdfPen(PdfBrushes.LightGray, 0.5f), new PointF(lineWidthPosition+textSize.Width+49,bottomValue+10), new PointF(clientSize.Width-25,bottomValue+10));

            bottom = bottomValue + 10;
            return result;
        }

        public PdfLayoutResult ComposeAddress(RectangleF bounds)
        {
            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTrueTypeFont class.
            contentFont = new PdfTrueTypeFont(standardFontStream, 8);
            boldFont = new PdfTrueTypeFont(boldFontStream, 9);
            float y = bottom+ 38;   
            
            //draw the text
            currentPage.Graphics.DrawString("Bill To", contentFont, PdfBrushes.Black, new PointF(29.15f, y-8.2f));

            /*Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTextElement class
            with the text and Syncfusion.Pdf.Graphics.PdfFont.*/
            var headerText = new PdfTextElement(model.CustomerAddress.Name, boldFont);
            //Draws the bill to element on the page with the specified page and Syncfusion.Drawing.PointF structure
            var result = headerText.Draw(currentPage, new PointF(29, y + 4));

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
                address += model.CustomerAddress.Phone + "\n";
            headerText = new PdfTextElement(address,contentFont);
            //Draws the address element on the page with the specified page and Syncfusion.Drawing.PointF structure
            PdfLayoutResult addressLayoutresult = headerText.Draw(currentPage, new PointF(29, result.Bounds.Bottom+1));

            //Initializes a new instance of the Syncfusion.Pdf.Grid.PdfGridCellStyle class.
            PdfGridCellStyle rowCellStyle = new PdfGridCellStyle();
            //sets the direction of the border of the Syncfusion.Pdf.Grid.PdfGridCell.
            rowCellStyle.Borders.Right = PdfPens.LightGray;
            rowCellStyle.Borders.Left = PdfPens.LightGray;
            rowCellStyle.Borders.Top = PdfPens.LightGray;
            rowCellStyle.Borders.Bottom = PdfPens.LightGray;
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
            pdfGrid.Columns[0].Width = 95;
            pdfGrid.Columns[1].Width = 95;
            //sets the vertical text alignment.
            pdfGrid.Rows[0].Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            //sets the value of the cell.
            pdfGrid.Rows[0].Cells[0].Value = "Invoice#";
            //sets the value of the cell.
            pdfGrid.Rows[0].Cells[1].Value = $"{"  " +model.InvoiceNumber}";
            //sets the vertical text alignment.
            pdfGrid.Rows[0].Cells[1].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

            //sets the vertical text alignment.
            pdfGrid.Rows[1].Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            //sets the value of the cell.
            pdfGrid.Rows[1].Cells[0].Value = "Inovice Date";
            //sets the value of the cell.
            pdfGrid.Rows[1].Cells[1].Value = $"{"  " + model.IssueDate}";
            //sets the vertical text alignment.
            pdfGrid.Rows[1].Cells[1].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

            //sets the vertical text alignment.
            pdfGrid.Rows[1].Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            //sets the value of the cell.
            pdfGrid.Rows[2].Cells[0].Value = "Due Date";
            //sets the value of the cell.
            pdfGrid.Rows[2].Cells[1].Value = $"{"  " + model.DueDate}";
            //sets the vertical text alignment.
            pdfGrid.Rows[2].Cells[1].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

            //sets the vertical text alignment.
            pdfGrid.Rows[3].Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            //sets the value of the cell.
            pdfGrid.Rows[3].Cells[0].Value = "Ref#";
            //sets the value of the cell.
            pdfGrid.Rows[3].Cells[1].Value = $"{"  " + model.RefNumber}";
            //sets the vertical text alignment.
            pdfGrid.Rows[3].Cells[1].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            for (int i = 0; i < pdfGrid.Rows.Count; i++)
            {
                //sets the border of the Syncfusion.Pdf.Grid.PdfGridCell.
                pdfGrid.Rows[i].Cells[0].Style.Borders.All = new PdfPen(Color.FromArgb(255, 236, 231, 231));               
                pdfGrid.Rows[i].Cells[1].Style.Borders.All = new PdfPen(Color.FromArgb(255, 236, 231, 231));               
            }

            for (int i = 0; i <= 3; i++)
                //sets the background brush for cell 
                pdfGrid.Rows[i].Cells[0].Style.BackgroundBrush = new PdfSolidBrush(Color.FromArgb(255, 236, 255, 212));
            pdfGrid.Style.Font = contentFont;

            //Initializes a new instance of the Syncfusion.Pdf.Grid.PdfGridStyle class.
            PdfGridStyle gridStyle = new PdfGridStyle();
            //sets the cell padding.
            gridStyle.CellPadding = new PdfPaddings(5, 5, 5, 3);
            //sets the grid style.
            pdfGrid.Style = gridStyle;

            //divide the page width
            float x = (clientSize.Width/2)+45;
            //Initializes a new instance of the Syncfusion.Pdf.Grid.PdfGridLayoutFormat class.
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            //sets layout type of the element.
            layoutFormat.Layout = PdfLayoutType.Paginate;
            //Draws the PdfGrid in the specified PdfPage.
            PdfLayoutResult tableLayoutResult = pdfGrid.Draw(currentPage, x,y,clientSize.Width-50,layoutFormat);

            if (addressLayoutresult.Bounds.Bottom > tableLayoutResult.Bounds.Bottom)
                result = addressLayoutresult;
            else
                result= tableLayoutResult;
            return result;
        }

        public PdfLayoutResult ComposeTable(RectangleF prevBounds)
        {
            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfTrueTypeFont class.
            contentFont = new PdfTrueTypeFont(standardFontStream, 8);

            //Initializes a new instance of the Syncfusion.Pdf.Grid.PdfGrid class.
            PdfGrid grid = new PdfGrid();
            grid.Style.Font = contentFont;
            //Adds the number of specified count.
            grid.Columns.Add(6);
            //sets the width of the Syncfusion.Pdf.Grid.PdfGridColumn.
            grid.Columns[0].Width = 25;
            grid.Columns[1].Width = 150;
            grid.Columns[2].Width = 53;
            grid.Columns[3].Width = 53;
            grid.Columns[4].Width = 52.91f;
            // Add rows to the header at run time.
            grid.Headers.Add(1);

            //Initializes a new instance of the Syncfusion.Pdf.Graphics.PdfStringFormat class with horizontal and vertical alignment
            PdfStringFormat stringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);

            //Gets the headers collection from the PdfGrid.[Read - Only]
            PdfGridRow header = grid.Headers[0];
            //sets the value of the cell.
            header.Cells[0].Value = "#";
            //set the string format
            header.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center);
            //sets the value of the cell.
            header.Cells[1].Value = "Item & description";
            //sets the value of the cell.
            header.Cells[2].Value = "Qty";
            //set the string format
            header.Cells[2].StringFormat = stringFormat;
            //sets the value of the cell.
            header.Cells[3].Value= "Rate($)";
            //set the string format
            header.Cells[3].StringFormat = stringFormat;
            //sets the value of the cell.
            header.Cells[4].Value = "Discoun(%)";
            //set the string format
            header.Cells[4].StringFormat = stringFormat;
            //sets the value of the cell.
            header.Cells[5].Value = "Amount($)";
            //set the string format
            header.Cells[5].StringFormat = stringFormat;

            //Initializes a new instance of the PdfGridCellStyle class.
            PdfGridCellStyle cellStyle = new PdfGridCellStyle();
            cellStyle.Borders.All= new PdfPen(Color.FromArgb(255, 236, 231, 231));          
            for (int j = 0; j < grid.Headers[0].Cells.Count; j++)
            {
                //sets the cell style
                grid.Headers[0].Cells[j].Style = cellStyle;
                //Gets the cells from the selected row.
                PdfGridCell cell = header.Cells[j];
                //sets the background brush.
                cell.Style.BackgroundBrush = new PdfSolidBrush(Color.FromArgb(255, 236, 255, 212));
            }

            decimal sum = 0;
            int index = 1;
            foreach (var item in model.Items)
            {
                //Add new row to the grid.
                PdfGridRow row = grid.Rows.Add();
                //sets the value of the cell.
                row.Cells[0].Value = $"{(model.Items.IndexOf(item) + 1)}";
                //sets the string format.
                row.Cells[0].StringFormat= new PdfStringFormat(PdfTextAlignment.Center);
                //sets the vertical text alignment.
                row.Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

                //sets the value of the cell.
                row.Cells[1].Value = item.Name[index-1];
                //sets the vertical text alignment.
                row.Cells[1].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

                //sets the value of the cell.
                row.Cells[2].Value = $"{item.Qty+" "}";
                //sets the string format.
                row.Cells[2].StringFormat = stringFormat;
                //sets the vertical text alignment.
                row.Cells[2].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

                //sets the value of the cell.
                row.Cells[3].Value = $"{item.Rate}";
                //sets the string format.
                row.Cells[3].StringFormat = stringFormat;
                //sets the vertical text alignment.
                row.Cells[3].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

                //sets the value of the cell.
                row.Cells[4].Value = $"{item.Discount}";
                //sets the string format.
                row.Cells[4].StringFormat = stringFormat;
                //sets the vertical text alignment.
                row.Cells[4].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

                //calculate the amount 
                decimal amount = item.Rate * item.Qty * (item.Discount / 100);
                //sets the value of the cell.
                row.Cells[5].Value = String.Format("{0:0.##}",amount);
                //sets the string format.
                row.Cells[5].StringFormat = stringFormat;
                //sets the vertical text alignment.
                row.Cells[5].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
                for (int i = 0; i < row.Cells.Count; i++) 
                {
                    //sets the border color direction of the Syncfusion.Pdf.Grid.PdfGridCell.
                    row.Cells[i].Style.Borders.Right = new PdfPen(Color.FromArgb(255, 236, 231, 231));
                    row.Cells[i].Style.Borders.Left = new PdfPen(Color.FromArgb(255, 236, 231, 231));
                    row.Cells[i].Style.Borders.Top = PdfPens.Transparent;
                    if (model.Items.Count != index)
                        row.Cells[i].Style.Borders.Bottom = PdfPens.Transparent;
                    else
                        row.Cells[i].Style.Borders.Bottom = new PdfPen(Color.FromArgb(255, 236, 231, 231));
                }             
                sum += amount;
                index++;
            }
            //Initializes a new instance of the Syncfusion.Pdf.Grid.PdfGridStyle class.
            PdfGridStyle gridStyle = new PdfGridStyle();
            //sets the cell padding
            gridStyle.CellPadding = new PdfPaddings(5, 5, 5, 3);
            //sets the grid style
            grid.Style = gridStyle;

            //Initializes a new instance of the Syncfusion.Pdf.Grid.PdfGridLayoutFormat class.
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            //sets layout type of the element.
            layoutFormat.Layout = PdfLayoutType.Paginate;
            //Draws the PdfGrid in the specified PdfPage.
            PdfLayoutResult result = grid.Draw(currentPage, 30, prevBounds.Bottom +52,clientSize.Width-50,layoutFormat);
            float resultWidth=result.Bounds.Width;
            if (pageCount > 1)
                resultWidth = clientSize.Width - 50;
            //Initializes a new instance of the PdfTextElement class with the text and PdfFont
            PdfTextElement element = new PdfTextElement("Thanks for your business", contentFont);
            element.StringFormat = new PdfStringFormat(PdfTextAlignment.Left);
            //Draws the element on the page with the specified page and RectangleF structure
            element.Draw(currentPage, new RectangleF(29, result.Bounds.Bottom + 23, resultWidth, result.Bounds.Height));

            //Initializes a new instance of the PdfTextElement class with the text and PdfFont
            element = new PdfTextElement("Sub Total", contentFont);
            //Measures a string by using this font.
            SizeF fontSize = contentFont.MeasureString(element.Text);
            float y = result.Bounds.Bottom;
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(result.Bounds.Width-(contentFont.Size*9)-10, result.Bounds.Bottom + 23, resultWidth, result.Bounds.Height));
            float totalWidth = result.Bounds.X;
            
            //get the sub total amount
            var totalPrice = $"{Math.Round(sum, 2)}";
            //Initializes a new instance of the PdfTextElement class with the text and PdfFont
            element = new PdfTextElement(totalPrice, contentFont);
            element.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(30, y + 23,result.Bounds.Width, result.Bounds.Height));

            
            y = result.Bounds.Bottom+15;
            //Initializes a new instance of the PdfTextElement class with the text and PdfFont
            element = new PdfTextElement("Sample Tax1 (4.70%)", contentFont);
            //Measures a string by using this font.
            SizeF size = contentFont.MeasureString(element.Text);
            if (y > clientSize.Height)
            {
                y = y - clientSize.Height;
                document.Pages.Add();
            }
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(totalWidth - size.Width+ fontSize.Width,y, result.Bounds.Width, result.Bounds.Height));
            float firstTaxWidth = result.Bounds.X;
            OrderItem tax=new OrderItem();
            //calculate the tax value
            decimal sampletax1 = sum * (tax.SampleTax1 / 100);
            element = new PdfTextElement(String.Format("{0:0.##}",sampletax1), contentFont);
            element.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(30, y, result.Bounds.Width, result.Bounds.Height));

        
            y = result.Bounds.Bottom+15;
            if (y > clientSize.Height)
            {
                y = y - clientSize.Height;
                document.Pages.Add();
            }
            //Initializes a new instance of the PdfTextElement class with the text and PdfFont
            element = new PdfTextElement("Sample Tax2 (7.0%)", contentFont);
            //Measures a string by using this font.
            size = contentFont.MeasureString(element.Text);
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(totalWidth - size.Width+ fontSize.Width,y, result.Bounds.Width, result.Bounds.Height));
            float secondTaxWidth = result.Bounds.X;
            //calculate the tax value
            decimal sampletax2 = sum * (tax.SampleTax2 / 100);
            element = new PdfTextElement(String.Format("{0:0.##}",sampletax2), contentFont);
            //sets the string format
            element.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(30, y, result.Bounds.Width, result.Bounds.Height));

            PdfPen pen = new PdfPen(PdfBrushes.LightGray, 0.5f);
            //find the maximum width of text elements
            float width= totalWidth < firstTaxWidth ? totalWidth : firstTaxWidth < secondTaxWidth ? firstTaxWidth : secondTaxWidth;
            //draw the line
            currentPage.Graphics.DrawLine(pen, new PointF(width, result.Bounds.Bottom + 10), new PointF(result.Bounds.Right, result.Bounds.Bottom + 10));
           
            boldFont = new PdfTrueTypeFont(boldFontStream, 9);
            y = result.Bounds.Bottom+9;
            if (y > clientSize.Height)
            {
                y = y - clientSize.Height;
                document.Pages.Add();
            }
            //Initializes a new instance of the PdfTextElement class with the text and PdfFont
            element = new PdfTextElement("Total",boldFont);
            SizeF textSize = boldFont.MeasureString(element.Text);
            float x = totalWidth - textSize.Width + fontSize.Width;
            y = y + 9;
            //Draws the element on the page with the specified page and RectangleF structure
            element.Draw(currentPage, new RectangleF(x,y, result.Bounds.Width, textSize.Height));
            decimal total = sampletax1+sampletax2 + sum;
            totalPrice = $"$ {String.Format("{0:0.##}",total)}";

            //Initializes a new instance of the PdfTextElement class with the text and PdfFont
            element = new PdfTextElement(totalPrice, boldFont );
            textSize = boldFont.MeasureString(element.Text);
            //sets the string format
            element.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(30, y, result.Bounds.Width, textSize.Height));

            //draw the line
            currentPage.Graphics.DrawLine(pen, new PointF(width, result.Bounds.Bottom + 9), new PointF(result.Bounds.Right, result.Bounds.Bottom + 10));
            y = result.Bounds.Bottom + 130;
            if (y > clientSize.Height)
            {
                y = y - clientSize.Height;
                document.Pages.Add();
            }
            //Initializes a new instance of the PdfTextElement class with the text and PdfFont
            element = new PdfTextElement("Terms & Condition", contentFont);
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(30, y, result.Bounds.Width, element.Font.Height));
            y = result.Bounds.Bottom + 2;
            if (y > clientSize.Height)
            {
                y = y - clientSize.Height;
                document.Pages.Add();
            }
            //Initializes a new instance of the PdfTextElement class with the text and PdfFont
            element = new PdfTextElement("You can either make online payment or pay cash at the time of delivery", contentFont);
            //Draws the element on the page with the specified page and RectangleF structure
            result = element.Draw(currentPage, new RectangleF(30,y, result.Bounds.Width,result.Bounds.Height));

            //draw the line
            currentPage.Graphics.DrawLine(pen, new PointF(30,clientSize.Height), new PointF(clientSize.Width - 23, clientSize.Height));
            return result;
        }

    }

}

