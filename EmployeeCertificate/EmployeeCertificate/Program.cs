using EmployeeCertificate.Model;
using System;
using System.IO;

namespace EmployeeCertificate
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create file stream to save the PDF document
            FileStream fileStream = new FileStream("EmployeeCertificate.pdf", FileMode.Create);

            //Get the certificate details to generate the PDF document 
            CertificateModel model = CertificateDataSource.GetCertificateDetails();
            CertificateDocument document = new CertificateDocument(model);
            document.GeneratePDF(fileStream);

            //Close the stream
            fileStream.Close();
        }
    }
}
