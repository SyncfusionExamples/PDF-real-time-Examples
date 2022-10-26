using PatientMedicalRecord;
using PatientMedicalRecord.Model;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.Diagnostics;
using System.IO;

namespace PatientMedicalRecord
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create file stream to save the PDF document
            FileStream fileStream = new FileStream("MedicalRecord.pdf", FileMode.Create);

            //Get the record details to generate the PDF document
            PatientRecordModel recordModel = PatientDataSource.GetMedicalRecordDetails();
            PatientRecordDocument document = new PatientRecordDocument(recordModel);
            document.GeneratePDF(fileStream);

            //Close the file stream
            fileStream.Close();
        }
    }
}