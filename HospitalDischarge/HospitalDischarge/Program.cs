using HospitalDocument;
using HospitalDocument.Model;

Console.WriteLine("sample.pdf");
FileStream fs = new FileStream("DischargeHospital.pdf", FileMode.Create);
DischargeModel model = DischargeDataSource.GetDischargeModel();
DischargeDocument document = new DischargeDocument(model);
document.GeneratePdf(fs);
fs.Close();
