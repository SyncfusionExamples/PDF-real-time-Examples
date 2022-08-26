using BoardingPassProject;
using BoardingPassProject.Model;


FileStream fs = new FileStream("BoardingPass.pdf", FileMode.Create);
BoardingPassModel model = DataSourceBoardingPassDocument.GetDetails();
BoardingPassDocument document = new BoardingPassDocument(model);
document.GeneratePdf(fs);
fs.Close();
