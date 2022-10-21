using JobOfferLetter;
using JobOfferLetter.Model;

FileStream fs = new FileStream("JobOfferLetter.pdf", FileMode.Create);
JobOfferLetterModel model = DataSourceJobOfferLetterDocument.GetDetails();
JobOfferLetterDocument document = new JobOfferLetterDocument(model);
document.GeneratePdf(fs);
fs.Close();
