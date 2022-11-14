using HospitalDocument.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalDocument
{
    internal class DischargeDataSource
    {
        /// <summary>
        /// Generate the Discharge Model
        /// </summary>
        /// <returns>Discharge Model</returns>
        public static DischargeModel GetDischargeModel()
        {
            return new DischargeModel
            {
                PatientDetails = GeneratePatientDetails(),
                AdmissionDetails = GenerateAdmissionDetails(),
                ConsultantDetails = GenerateConsultantDetails(),
                DischargeDetails = GenerateDischargeDetails(),
                SignatureDetails = GenerateSignatureDetails(),
            };
        }
        /// <summary>
        /// Generate the Patient Details
        /// </summary>
        /// <returns>Patient Details</returns>
        private static PatientDetails GeneratePatientDetails()
        {
            return new PatientDetails
            {
                PatientUID = 58,
                Name = "Laura Hase",
                Age = 40,
                Address = "New York, 1823467",
            };
        }
        /// <summary>
        /// Generate the Admission Details
        /// </summary>
        /// <returns>Admission Details</returns>
        private static AdmissionDetails GenerateAdmissionDetails()
        {
            return new AdmissionDetails
            {
                AdmissionNo = 1221000,
                AdmissionDate = "09 - Aug - 22, 1:30 PM",
                DischargeDate = "26 - Aug - 22, 4:30 PM",
                DischargeStatus = "Discharged",
            };
        }
        /// <summary>
        /// Generate the Consultant Details
        /// </summary>
        /// <returns>Consultant Details</returns>
        private static ConsultantDetails GenerateConsultantDetails()
        {
            return new ConsultantDetails
            {
                DoctorName = "Dr. John Mc",
                Speciality = "Infectious Diseases Practitioner",
            };
        }
        /// <summary>
        /// Generate the Discharge Details
        /// </summary>
        /// <returns>Discharge Details</returns>
        private static DischargeDetails GenerateDischargeDetails()
        {
            return new DischargeDetails
            {
                FinalDiagnosis = "Fracture at Wrist and Hand Level.",
                Investigation = "Hemogram, X-Ray",
                PhysicalExamination = GeneratePhysicalExamination(),
                GeneralAppearance = GenerateGeneralAppearance(),
                Medication = "Acetaminophen and Cephalosporins",
                Instructions = "No activity restrictions, regular diet and follow up in two - three weeks with regular physician.",
                Patient = "I have understood the Instructions given about the Medication Dosage and Discharge.",
            };
        }
        /// <summary>
        /// Generate the PhysicalExamination
        /// </summary>
        /// <returns>PhysicalExamination</returns>
        private static PhysicalExamination GeneratePhysicalExamination()
        {
            return new PhysicalExamination
            {
                BP = "122-80",
                Pulse = 72,
            };
        }
        /// <summary>
        /// Generate the GeneralAppearance
        /// </summary>
        /// <returns>GeneralAppearance</returns>
        private static GeneralAppearance GenerateGeneralAppearance()
        {
            return new GeneralAppearance
            {
                Disease = "NAD",
            };
        }
        /// <summary>
        /// Generate the Signature Details
        /// </summary>
        /// <returns>Signature Details</returns>
        private static SignatureDetails GenerateSignatureDetails()
        {
            FileStream signature = new FileStream(@"../../../Assets/Image/Signature.png", FileMode.Open,FileAccess.Read);
            return new SignatureDetails
            {
                Name = "Robson",
                Signature = new Syncfusion.Pdf.Graphics.PdfBitmap(signature),
            };
        }
    }
}
