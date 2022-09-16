using LeaseAgreement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaseAgreement
{
    /// <summary>
    /// Lease data source class
    /// </summary>
    internal class LeaseDataSource
    {
        /// <summary>
        /// Get month lease details
        /// </summary>
        /// <returns>lease model details</returns>
        public static LeaseModel GetMonthLeaseDetails()
        {
            return new LeaseModel
            {
                Title = "Month to Month Lease Agreement",
                Parties = GeneratePartiesDetails(),
                LeasePeriod = GenerateLeasePeriod(),
                OtherOccupants1 = GenerateOtherOccupants1(),
                OtherOccupants2 = GenerateOtherOccupants2(),
                LesseeSignature = GenerateAgreementSignature1(),
                LessorSignature = GenerateAgreementSignature2(),
                TermsofLease = GenerateTermsofLeaseDetails(),
            };
        }
        /// <summary>
        /// Generate the parties details
        /// </summary>
        /// <returns>Parties details</returns>
        private static Parties GeneratePartiesDetails()
        {
            return new Parties
            {
                Title = "This Agreement is Executed By and Between the Parties:",
                Lessor = "Elvis Peckham",
                Lessee = "Torrin Adrienne",
                LeaseTerm = "Integer",
                PaymentPeriod = new DateTime(2018, 07, 25),
                PropertyDetails = GeneratePropertyDetails(),
            };
        }
        /// <summary>
        /// Generate the property details
        /// </summary>
        /// <returns>Property details</returns>
        private static PropertyDetails GeneratePropertyDetails()
        {
            return new PropertyDetails
            {
                PlatNo = "54432 Clarendon alley",
                Street = "9929 Almo PaWashingto,",
                City = "District o,",
                PinCode = 20062,
            };
        }
        /// <summary>
        /// Generate the lease period details
        /// </summary>
        /// <returns>lease period details</returns>
        private static LeasePeriod GenerateLeasePeriod()
        {
            return new LeasePeriod
            {
                Title = "Lease Period",
                LeaseFrom = DateTime.Now.ToString("dddd, dd MMMM 2000"),
                LeaseUntil = DateTime.Now.ToString("dddd, dd MMMM 2016"),
                MonthlyLeaseAmount = 1200.00f,
                SecurityDebitAmount = 890.00f,
            };
        }
        /// <summary>
        /// Generate the other occupants details
        /// </summary>
        /// <returns>Other occupants details</returns>
        private static OtherOccupants GenerateOtherOccupants1()
        {
            return new OtherOccupants
            {
                Title = "Names of Other Occupant 1",
                Name = "Dannel Kreuzer",
                DateofBirth =DateTime.Now,
                RelationShip = "Intege",
            };
        }
        /// <summary>
        /// Generate the other occupants details
        /// </summary>
        /// <returns>Other occupants details</returns>
        private static OtherOccupants GenerateOtherOccupants2()
        {
            return new OtherOccupants
            {
                Title = "Names of Other Occupant 2",
                Name = "Raja",
                DateofBirth = DateTime.Now,
                RelationShip = "Maecen",
            };
        }
        /// <summary>
        /// Generate the Agreement signature
        /// </summary>
        /// <returns>agreement signature</returns>
        private static AgreementSignature GenerateAgreementSignature1()
        {
            return new AgreementSignature
            {
                Signature = " ",
            };
        }
        /// <summary>
        /// Generate the Agreement signature
        /// </summary>
        /// <returns>agreement signature</returns>
        private static AgreementSignature GenerateAgreementSignature2()
        {
            return new AgreementSignature
            {
                Signature = " ",
            };
        }
        /// <summary>
        /// Generate the terms of lease details
        /// </summary>
        /// <returns>Terma of lease details</returns>
        private static TermsofLease GenerateTermsofLeaseDetails()
        {
            string json = File.ReadAllText(@"..\..\..\Data\StringofData.json");
            TermsofLease Data = JsonConvert.DeserializeObject<TermsofLease>(json);
            return new TermsofLease
            {
                Title = "Terms of Lease",
                Abandonment = Data.Abandonment,
                AssignmentandSublease = Data.AssignmentandSublease,
                JudicialAction = Data.JudicialAction,
                ImprovementPremises = Data.ImprovementPremises,
                Rent = Data.Rent,
                NecessaryExpenses = Data.NecessaryExpenses,
                Pets = Data.Pets,
                UseandOccupancy = Data.UseandOccupancy,
            };
        }

    }
}
