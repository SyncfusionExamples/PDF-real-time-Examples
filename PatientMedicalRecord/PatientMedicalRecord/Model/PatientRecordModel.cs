using System;
using System.Collections.Generic;
using System.Text;

namespace PatientMedicalRecord.Model
{
    /// <summary>
    /// Patient medical record model class
    /// </summary>
    public class PatientRecordModel
    {
        /// <summary>
        /// Get or set the patient details 
        /// </summary>
        public Patient Patient { get; set; }

        /// <summary>
        /// Get or set the emergency details 
        /// </summary>
        public EmergencyDetails EmergencyDetails { get; set; }

        /// <summary>
        /// Get or set the medical information
        /// </summary>
        public MedicalInformation MedicalInformation { get; set; }
    }
    /// <summary>
    /// Patient class
    /// </summary>
    public class Patient
    {
        /// <summary>
        /// Get or set patient name 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set phone number 
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// Get or set the address 
        /// </summary>
        public PatientAddress PatientAddress { get; set; }

        /// <summary>
        /// Get or set the birth date 
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// Get or set the weight
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// Get or set the height
        /// </summary>
        public float Height { get; set; }
    }

    /// <summary>
    /// Address class 
    /// </summary>
    public class PatientAddress
    {
        /// <summary>
        /// Get or set door number
        /// </summary>
        public int PlotNo { get; set; }

        /// <summary>
        /// Get or set the street name
        /// </summary>
        public string StreetName1 { get; set; }

        /// <summary>
        /// Get or set the street name
        /// </summary>
        public string StreetName2 { get; set; }

        /// <summary>
        /// Get or set the city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Get or set the pincode
        /// </summary>
        public int PinCode { get; set; }

        /// <summary>
        /// Get or set the country name
        /// </summary>
        public string Country { get; set; }
    }
    /// <summary>
    /// Emergency class 
    /// </summary>
    public class EmergencyDetails
    {
        /// <summary>
        /// Get or set the full name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Get or set the phone number 
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EmergencyAddress EmergencyAddress { get; set; }
    }
    /// <summary>
    /// Emergency address class
    /// </summary>
    public class EmergencyAddress
    {
        /// <summary>
        /// Get or set door number
        /// </summary>
        public string PlotNo { get; set; }

        /// <summary>
        /// Get or set the street name
        /// </summary>
        public string StreetName { get; set; }
        /// <summary>
        /// Get or set the city name
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Get or set the pincode
        /// </summary>
        public string PinCode { get; set; }

        /// <summary>
        /// Get or set the country name
        /// </summary>
        public string Country { get; set; }
    }
    /// <summary>
    /// Medical information class
    /// </summary>
    public class MedicalInformation
    {
        /// <summary>
        /// Get or set the clinic name
        /// </summary>
        public string ClinicName { get; set; }

        /// <summary>
        /// Get or set the immunizations 
        /// </summary>
        public string Immunizations { get; set; }

        /// <summary>
        /// Get or set the medical problems 
        /// </summary>
        public string MedicalProblem { get; set; }

        /// <summary>
        /// Get or set the medical insurance 
        /// </summary>
        public string MedicalInsurance { get; set; }

        /// <summary>
        /// Get or set the phone number 
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Get or set the address
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Get or set the policy number 
        /// </summary>
        public string PolicyNumber { get; set; }
    }
    /// <summary>
    /// Address class
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Get or set the street name
        /// </summary>
        public string StreetName { get; set; }

        /// <summary>
        /// Get or set the landmark
        /// </summary>
        public string Landmark { get; set; }

        /// <summary>
        /// Get or set the city 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Get or set the pincode
        /// </summary>
        public string PinCode { get; set; }
    }
}
