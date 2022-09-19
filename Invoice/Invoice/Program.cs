
using Invoice;
using Invoice.Model;

Console.WriteLine("sample.pdf");
FileStream fs = new FileStream("invoice.pdf", FileMode.Create);
InvoiceModel model = InvoiceDocumentDataSource.GetInvoiceDetails();
InvoiceDocument document = new InvoiceDocument(model);
document.GeneratePdf(fs);
fs.Close();

