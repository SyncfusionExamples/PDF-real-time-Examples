
using Invoice;
using Invoice.Model;

FileStream fs = new FileStream("Invoice.pdf", FileMode.Create);
InvoiceModel model = InvoiceDocumentDataSource.GetInvoiceDetails();
InvoiceDocument document = new InvoiceDocument(model);
document.GeneratePdf(fs);
fs.Close();

