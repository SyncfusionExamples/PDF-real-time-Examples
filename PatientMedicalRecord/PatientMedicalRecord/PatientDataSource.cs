using PatientMedicalRecord.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PatientMedicalRecord
{
    /// <summary>
    /// Patient data source class
    /// </summary>
    internal class PatientDataSource
    { 
        /// <summary>
        /// Get the patient medical details
        /// </summary>
        /// <returns>Patient medical details</returns>
        public static PatientRecordModel GetMedicalRecordDetails()
        {
            return new PatientRecordModel
            {
                Patient = GetPatientDetails(), 
                EmergencyDetails = GetEmergencyDetails(),
                MedicalInformation = GetMedicalInformation(),
            };
        }

        /// <summary>
        /// Generate patient details 
        /// </summary>
        /// <returns>Patient details</returns>
        private static Patient GetPatientDetails()
        {
            return new Patient
            {
                Name = "John Smith",
                PhoneNo = "(706) 296-9894",
                PatientAddress = GetPatientAddress(),
                BirthDate = "February 8th 1984",
                Weight = 68.89f,
                Height = 162f,
            };
        }

        /// <summary>
        /// Generate the patient address
        /// </summary>
        /// <returns>Patient address</returns>
        private static PatientAddress GetPatientAddress()
        {
            return new PatientAddress
            {
                PlotNo = 11815,
                StreetName1 = "RollyStreet",
                StreetName2 = "Atrens",
                City = "Georgia",
                PinCode = 345691,
                Country = "United States",
            };
        }

        /// <summary>
        /// Generate the emergency details for contact 
        /// </summary>
        /// <returns>Emergency details</returns>
        public static EmergencyDetails GetEmergencyDetails()
        {
            return new EmergencyDetails
            {
                FullName = "James K",
                Number = "8942-89-2193",
                EmergencyAddress = GetEmergencyAddress(),
            };
        }
        /// <summary>
        /// Generate the emergency address for patient
        /// </summary>
        /// <returns>Emergency address</returns>
        public static EmergencyAddress GetEmergencyAddress()
        {
            return new EmergencyAddress
            {
                PlotNo = "9080 Bolgor",
                StreetName = "Fosfal road", 
                PinCode = 8989214, 
                Country = "United States",
            };
        }

        /// <summary>
        /// Generate the patient medical information
        /// </summary>
        /// <returns>Patient medical information</returns>
        public static MedicalInformation GetMedicalInformation()
        {
            return new MedicalInformation
            {
                ClinicName = "Alosius Hospital",
                Immunizations = "Yes",
                MedicalProblem = "Asthma, Low Pressure, Diabetes",
                MedicalInsurance = "Yes, ITF Co",
                PhoneNumber = "(1111111) 182182-1212",
                Address = GetAddress(),
                PolicyNumber = "8888-9912-12333",
            };
        }

        /// <summary>
        /// Generate address
        /// </summary>
        /// <returns>Address</returns>
        public static Address GetAddress()
        {
            return new Address
            {
                StreetName = "Doe Street",
                Landmark = "Near ombos", 
                PinCode = 481919,
            };
        }
    }
}
