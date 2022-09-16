using LeaseAgreement;
using LeaseAgreement.Models;

Console.WriteLine("sample.pdf");
FileStream fs = new FileStream("MonthtoMonth.pdf", FileMode.Create);
LeaseModel model = LeaseDataSource.GetMonthLeaseDetails();
LeaseDocument document = new LeaseDocument(model);
document.GeneratePdf(fs);
fs.Close();
