﻿using Invoice.Model;
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
            document = new PdfDocument();
            document.Pages.PageAdded += Pages_PageAdded;
            var section = document.Sections.Add();         
            currentPage = document.Pages.Add();
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
            PdfFont contentFont= new PdfTrueTypeFont(standardFontStream,8);
            boldFont = new PdfTrueTypeFont(boldFontStream,10);
            PdfFont boldTextFont = new PdfTrueTypeFont(boldFontStream,9);
            RectangleF bounds = new RectangleF(0, 0, currentPage.GetClientSize().Width, 50);
            PdfGraphics graphics = currentPage.Graphics;
            FileStream stream = new FileStream(@"../../../Assets/logo.png", FileMode.Open);
            stream.Position = 0;
            PdfImage icon = new PdfBitmap(stream);
            PointF iconLocation= new PointF(29,54);
            SizeF iconSize = new SizeF(45, 38);
            graphics.DrawImage(icon,iconLocation,iconSize);

            float leftBottom = iconLocation.Y + iconSize.Height;
            var headerText = new PdfTextElement(model.SellerAddress.Name,boldTextFont);
            headerText.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
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

            headerText = new PdfTextElement(address, contentFont);
            headerText.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            result = headerText.Draw(currentPage, new PointF(clientSize.Width - 25, result.Bounds.Bottom+1));

            float bottomValue=0;
            if(leftBottom>result.Bounds.Bottom)
                bottomValue=leftBottom;
            else
                bottomValue=result.Bounds.Bottom;

            float y = bounds.Bottom + 46.56f;
            string text = "INVOICE";
            SizeF textSize = boldFont.MeasureString(text);
            float lineWidthPosition = (clientSize.Width - 54 - textSize.Width - 20) / 2;

            currentPage.Graphics.DrawLine(new PdfPen(PdfBrushes.LightGray, 0.5f), new PointF(29, bottomValue + 10), new PointF(lineWidthPosition + 29, bottomValue + 10));
           
            currentPage.Graphics.DrawString(text, boldFont, PdfBrushes.Black, new PointF(lineWidthPosition + 39,bottomValue+10-(textSize.Height/2))) ;
                       
            currentPage.Graphics.DrawLine(new PdfPen(PdfBrushes.LightGray, 0.5f), new PointF(lineWidthPosition+textSize.Width+49,bottomValue+10), new PointF(clientSize.Width-25,bottomValue+10));

            bottom = bottomValue + 10;
            return result;
        }

        public PdfLayoutResult ComposeAddress(RectangleF bounds)
        {
            contentFont = new PdfTrueTypeFont(standardFontStream, 8);
            boldFont = new PdfTrueTypeFont(boldFontStream, 9);
            float y = bottom+ 38;   
            
            //From address
            currentPage.Graphics.DrawString("Bill To", contentFont, PdfBrushes.Black, new PointF(29.15f, y-8.2f)); 
            
            var headerText = new PdfTextElement(model.CustomerAddress.Name, boldFont);
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
            PdfLayoutResult addressLayoutresult = headerText.Draw(currentPage, new PointF(29, result.Bounds.Bottom+1));

            PdfGridCellStyle rowCellStyle = new PdfGridCellStyle();
            rowCellStyle.Borders.Right = PdfPens.LightGray;
            rowCellStyle.Borders.Left = PdfPens.LightGray;
            rowCellStyle.Borders.Top = PdfPens.LightGray;
            rowCellStyle.Borders.Bottom = PdfPens.LightGray;
            //For address
            PdfGrid pdfGrid = new PdfGrid();
            pdfGrid.Style.Font = contentFont;
            PdfGridRow row1 = pdfGrid.Rows.Add();            
            PdfGridRow row2 = pdfGrid.Rows.Add();            
            PdfGridRow row3 = pdfGrid.Rows.Add();            
            PdfGridRow row4 = pdfGrid.Rows.Add();  
            pdfGrid.Columns.Add(2);
            pdfGrid.Columns[0].Width = 95;
            pdfGrid.Columns[1].Width = 95;
            pdfGrid.Rows[0].Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            pdfGrid.Rows[0].Cells[0].Value = "Invoice#";            
            pdfGrid.Rows[0].Cells[1].Value = $"{"  " +model.InvoiceNumber}";
            pdfGrid.Rows[0].Cells[1].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

            pdfGrid.Rows[1].Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            pdfGrid.Rows[1].Cells[0].Value = "Inovice Date";
            pdfGrid.Rows[1].Cells[1].Value = $"{"  " + model.IssueDate}";
            pdfGrid.Rows[1].Cells[1].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

            pdfGrid.Rows[1].Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            pdfGrid.Rows[2].Cells[0].Value = "Due Date";
            pdfGrid.Rows[2].Cells[1].Value = $"{"  " + model.DueDate}";
            pdfGrid.Rows[2].Cells[1].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

            pdfGrid.Rows[3].Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            pdfGrid.Rows[3].Cells[0].Value = "Ref#";
            pdfGrid.Rows[3].Cells[1].Value = $"{"  " + model.RefNumber}";
            pdfGrid.Rows[3].Cells[1].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            for (int i = 0; i < pdfGrid.Rows.Count; i++)
            {
                pdfGrid.Rows[i].Cells[0].Style.Borders.All = new PdfPen(Color.FromArgb(255, 236, 231, 231));               
                pdfGrid.Rows[i].Cells[1].Style.Borders.All = new PdfPen(Color.FromArgb(255, 236, 231, 231));               
            }

            for (int i = 0; i <= 3; i++)
                pdfGrid.Rows[i].Cells[0].Style.BackgroundBrush = new PdfSolidBrush(Color.FromArgb(255, 236, 255, 212));
            pdfGrid.Style.Font = contentFont;

            PdfGridStyle gridStyle = new PdfGridStyle();
            gridStyle.CellPadding = new PdfPaddings(5, 5, 5, 3);
            pdfGrid.Style = gridStyle;

            float x = (clientSize.Width/2)+45;
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            layoutFormat.Layout = PdfLayoutType.Paginate;
            PdfLayoutResult tableLayoutResult = pdfGrid.Draw(currentPage, x,y,clientSize.Width-50,layoutFormat);

            if (addressLayoutresult.Bounds.Bottom > tableLayoutResult.Bounds.Bottom)
                result = addressLayoutresult;
            else
                result= tableLayoutResult;
            return result;
        }

        public PdfLayoutResult ComposeTable(RectangleF prevBounds)
        {
            contentFont = new PdfTrueTypeFont(standardFontStream, 8);

            PdfGrid grid = new PdfGrid();
            grid.Style.Font = contentFont; 
            grid.Columns.Add(6);
            grid.Columns[0].Width = 25;
            grid.Columns[1].Width = 150;
            grid.Columns[2].Width = 53;
            grid.Columns[3].Width = 53;
            grid.Columns[4].Width = 52.91f;
            grid.Headers.Add(1);

            PdfStringFormat stringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);

            PdfGridRow header = grid.Headers[0];
            header.Cells[0].Value = "#";
            header.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center);
            header.Cells[1].Value = "Item & description";
            header.Cells[2].Value = "Qty";
            header.Cells[2].StringFormat = stringFormat;
            header.Cells[3].Value= "Rate($)";
            header.Cells[3].StringFormat = stringFormat;
            header.Cells[4].Value = "Discoun(%)";
            header.Cells[4].StringFormat = stringFormat;
            header.Cells[5].Value = "Amount($)";
            header.Cells[5].StringFormat = stringFormat;     
            
            PdfGridCellStyle cellStyle = new PdfGridCellStyle();
            cellStyle.Borders.All= new PdfPen(Color.FromArgb(255, 236, 231, 231));          
            for (int j = 0; j < grid.Headers[0].Cells.Count; j++)
            {
                grid.Headers[0].Cells[j].Style = cellStyle;
                PdfGridCell cell = header.Cells[j];
                cell.Style.BackgroundBrush = new PdfSolidBrush(Color.FromArgb(255, 236, 255, 212));
            }

            decimal sum = 0;
            int index = 1;
            foreach (var item in model.Items)
            {
                PdfGridRow row = grid.Rows.Add();
                row.Cells[0].Value = $"{(model.Items.IndexOf(item) + 1)}";
                row.Cells[0].StringFormat= new PdfStringFormat(PdfTextAlignment.Center);
                row.Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

                row.Cells[1].Value = item.Name[index-1];
                row.Cells[1].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

                row.Cells[2].Value = $"{item.Qty+" "}";
                row.Cells[2].StringFormat = stringFormat;
                row.Cells[2].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

                row.Cells[3].Value = $"{item.Rate}";
                row.Cells[3].StringFormat = stringFormat;
                row.Cells[3].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

                row.Cells[4].Value = $"{item.Discount}";
                row.Cells[4].StringFormat = stringFormat;
                row.Cells[4].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

                decimal amount = item.Rate * item.Qty * (item.Discount / 100);
                row.Cells[5].Value = String.Format("{0:0.##}",amount);
                row.Cells[5].StringFormat = stringFormat;
                row.Cells[5].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
                for (int i = 0; i < row.Cells.Count; i++) 
                {
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
            PdfGridStyle gridStyle = new PdfGridStyle();
            gridStyle.CellPadding = new PdfPaddings(5, 5, 5, 3);
            grid.Style = gridStyle;

            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            layoutFormat.Layout = PdfLayoutType.Paginate;
            PdfLayoutResult result = grid.Draw(currentPage, 30, prevBounds.Bottom +52,clientSize.Width-50,layoutFormat);
            float resultWidth=result.Bounds.Width;
            if (pageCount > 1)
                resultWidth = clientSize.Width - 50;               
            PdfTextElement element = new PdfTextElement("Thanks for your business", contentFont);
            element.StringFormat = new PdfStringFormat(PdfTextAlignment.Left);
            element.Draw(currentPage, new RectangleF(29, result.Bounds.Bottom + 23, resultWidth, result.Bounds.Height));

            element = new PdfTextElement("Sub Total", contentFont);
            SizeF fontSize = contentFont.MeasureString(element.Text);
            float y = result.Bounds.Bottom;
            result =element.Draw(currentPage, new RectangleF(result.Bounds.Width-(contentFont.Size*9)-10, result.Bounds.Bottom + 23, resultWidth, result.Bounds.Height));
            float totalWidth = result.Bounds.X;
            
            
            var totalPrice = $"{Math.Round(sum, 2)}";
            element = new PdfTextElement(totalPrice, contentFont);
            element.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            result = element.Draw(currentPage, new RectangleF(30, y + 23,result.Bounds.Width, result.Bounds.Height));

            
            y = result.Bounds.Bottom+15;
            element = new PdfTextElement("Sample Tax1 (4.70%)", contentFont);
            SizeF size = contentFont.MeasureString(element.Text);
            if (y > clientSize.Height)
            {
                y = y - clientSize.Height;
                document.Pages.Add();
            }
            result =element.Draw(currentPage, new RectangleF(totalWidth - size.Width+ fontSize.Width,y, result.Bounds.Width, result.Bounds.Height));
            float firstTaxWidth = result.Bounds.X;
            OrderItem tax=new OrderItem();
            decimal sampletax1 = sum * (tax.SampleTax1 / 100);
            element = new PdfTextElement(String.Format("{0:0.##}",sampletax1), contentFont);
            element.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            result = element.Draw(currentPage, new RectangleF(30, y, result.Bounds.Width, result.Bounds.Height));

        
            y = result.Bounds.Bottom+15;
            if (y > clientSize.Height)
            {
                y = y - clientSize.Height;
                document.Pages.Add();
            }
            element = new PdfTextElement("Sample Tax2 (7.0%)", contentFont);
            size = contentFont.MeasureString(element.Text);
            result = element.Draw(currentPage, new RectangleF(totalWidth - size.Width+ fontSize.Width,y, result.Bounds.Width, result.Bounds.Height));
            float secondTaxWidth = result.Bounds.X;
            decimal sampletax2 = sum * (tax.SampleTax2 / 100);
            element = new PdfTextElement(String.Format("{0:0.##}",sampletax2), contentFont);
            element.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            result = element.Draw(currentPage, new RectangleF(30, y, result.Bounds.Width, result.Bounds.Height));

            PdfPen pen = new PdfPen(PdfBrushes.LightGray, 0.5f);
            float width= totalWidth < firstTaxWidth ? totalWidth : firstTaxWidth < secondTaxWidth ? firstTaxWidth : secondTaxWidth;
            currentPage.Graphics.DrawLine(pen, new PointF(width, result.Bounds.Bottom + 10), new PointF(result.Bounds.Right, result.Bounds.Bottom + 10));
           
            boldFont = new PdfTrueTypeFont(boldFontStream, 9);
            y = result.Bounds.Bottom+9;
            if (y > clientSize.Height)
            {
                y = y - clientSize.Height;
                document.Pages.Add();
            }
            element = new PdfTextElement("Total",boldFont);
            SizeF textSize = boldFont.MeasureString(element.Text);
            float x = totalWidth - textSize.Width + fontSize.Width;
            y = y + 9;
            element.Draw(currentPage, new RectangleF(x,y, result.Bounds.Width, textSize.Height));
            decimal total = sampletax1+sampletax2 + sum;
            totalPrice = $"$ {String.Format("{0:0.##}",total)}";

            element = new PdfTextElement(totalPrice, boldFont );
            textSize = boldFont.MeasureString(element.Text);
            element.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            result = element.Draw(currentPage, new RectangleF(30, y, result.Bounds.Width, textSize.Height));
           
            currentPage.Graphics.DrawLine(pen, new PointF(width, result.Bounds.Bottom + 9), new PointF(result.Bounds.Right, result.Bounds.Bottom + 10));
            y = result.Bounds.Bottom + 130;
            if (y > clientSize.Height)
            {
                y = y - clientSize.Height;
                document.Pages.Add();
            }
            element = new PdfTextElement("Terms & Condition", contentFont);
            result = element.Draw(currentPage, new RectangleF(30, y, result.Bounds.Width, element.Font.Height));
            y = result.Bounds.Bottom + 2;
            if (y > clientSize.Height)
            {
                y = y - clientSize.Height;
                document.Pages.Add();
            }
            element = new PdfTextElement("You can either make online payment or pay cash at the time of delivery", contentFont);
            result = element.Draw(currentPage, new RectangleF(30,y, result.Bounds.Width,result.Bounds.Height));

            currentPage.Graphics.DrawLine(pen, new PointF(30,clientSize.Height), new PointF(clientSize.Width - 23, clientSize.Height));
            return result;
        }

    }

}
