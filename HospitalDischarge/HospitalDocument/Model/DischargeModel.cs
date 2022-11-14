using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalDocument.Model
{
    internal class DischargeModel
    {
        /// <summary>
        /// Get or set the Patient Details
        /// </summary>
        public PatientDetails PatientDetails { get; set; }

        /// <summary>
        /// Get or set the Admission Details
        /// </summary>
        public AdmissionDetails AdmissionDetails { get; set; }

        /// <summary>
        /// Get or set the Consultant Details
        /// </summary>
        public ConsultantDetails ConsultantDetails { get; set; }

        /// <summary>
        /// Get or set the Discharge Details
        /// </summary>
        public DischargeDetails DischargeDetails { get; set; }

        /// <summary>
        /// Get or set the SignatureDetails
        /// </summary>
        public SignatureDetails SignatureDetails { get; set; }

    }
    /// <summary>
    /// PatientDetails class
    /// </summary>
    public class PatientDetails
    {
        /// <summary>
        /// Get or set the PatientUID
        /// </summary>
        public int PatientUID { get; set; }

        /// <summary>
        /// Get or set the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set the Age
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Get or set the Address
        /// </summary>
        public string Address { get; set; }
    }
    /// <summary>
    /// AdmissionDetails class
    /// </summary>
    public class AdmissionDetails
    {
        /// <summary>
        /// Get or set the AdmissionNo
        /// </summary>
        public int AdmissionNo { get; set; }

        /// <summary>
        /// Get or set the AdmissionDate
        /// </summary>
        public string AdmissionDate { get; set; }

        /// <summary>
        /// Get or set the DischargeDate
        /// </summary>
        public string DischargeDate { get; set; }

        /// <summary>
        /// Get or set the DischargeStatus
        /// </summary>
        public string DischargeStatus { get; set; }
    }
    /// <summary>
    /// ConsultantDetails class
    /// </summary>
    public class ConsultantDetails
    {
        /// <summary>
        /// Get or set the DoctorName
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// Get or set the Speciality
        /// </summary>
        public string Speciality { get; set; }
    }
    /// <summary>
    /// DischargeDetails class
    /// </summary>
    public class DischargeDetails
    {
        /// <summary>
        /// Get or set the FinalDiagnosis
        /// </summary>
        public string FinalDiagnosis { get; set; }

        /// <summary>
        /// Get or set the PhysicalExamination
        /// </summary>
        public PhysicalExamination PhysicalExamination { get; set; }

        /// <summary>
        /// Get or set the GeneralAppearance
        /// </summary>
        public GeneralAppearance GeneralAppearance { get; set; }

        /// <summary>
        /// Get or set the Investigation
        /// </summary>
        public string Investigation { get; set; }
        /// <summary>
        /// Get or set the Medication
        /// </summary>
        public string Medication { get; set; }

        /// <summary>
        /// Get or set the Instructions
        /// </summary>
        public string Instructions { get; set; }

        /// <summary>
        /// Get or set the Patient
        /// </summary>
        public string Patient { get; set; }
    }
    /// <summary>
    /// SignatureDetails class
    /// </summary>
    public class SignatureDetails
    {
        /// <summary>
        /// Get or set the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set the Signature
        /// </summary>
        public PdfBitmap Signature { get; set; }
    }
    /// <summary>
    /// physicalExamination class
    /// </summary>
    public class PhysicalExamination
    {
        /// <summary>
        /// Get or set the BP
        /// </summary>
        public string BP { get; set; }

        /// <summary>
        /// Get or set the Pulse
        /// </summary>
        public int Pulse { get; set; }
    }
    /// <summary>
    /// GeneralAppearance class
    /// </summary>
    public class GeneralAppearance
    {
        /// <summary>
        /// Get or set the Disease
        /// </summary>
        public string Disease { get; set; }
    }
}
