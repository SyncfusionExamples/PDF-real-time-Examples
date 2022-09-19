using LeaseAgreement;
using LeaseAgreement.Models;

Console.WriteLine("SampleProject.pdf");
FileStream fs = new FileStream("LeaseAgreement.pdf", FileMode.Create);
LeaseModel model = LeaseDataSource.GetMonthLeaseDetails();
LeaseDocument document = new LeaseDocument(model);
document.GeneratePdf(fs);
fs.Close();
