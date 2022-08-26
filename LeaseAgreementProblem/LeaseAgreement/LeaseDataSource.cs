﻿using LeaseAgreement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaseAgreement
{
    internal class LeaseDataSource
    {
        public static LeaseModel GetMonthLeaseDetails()
        {
            return new LeaseModel
            {
                Title = "Month to Month Lease Agreement",
                CompanyName = "ABZ Company",
                FormDate = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                Parties = GeneratePartiesDetails(),
                LeasePeriod = GenerateLeasePeriod(),
                OtherOccupants1 = GenerateOtherOccupants1(),
                OtherOccupants2 = GenerateOtherOccupants2(),
                LesseeSignature = GenerateAgreementSignature1(),
                LessorSignature = GenerateAgreementSignature2(),
                TermsofLease = GenerateTermsofLeaseDetails(),
            };
        }
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
        private static PropertyDetails GeneratePropertyDetails()
        {
            return new PropertyDetails
            {
                PlatNo = "64432 Clarendon Alley,",
                Street = "9929 Almo PaWashingto,",
                City = "District o,",
                PinCode = 20062,
            };
        }

        private static LeasePeriod GenerateLeasePeriod()
        {
            return new LeasePeriod
            {
                Title = "Lease Period",
                LeaseFrom = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                LeaseUntil = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                MonthlyLeaseAmount = 1200.00f,
                SecurityDebitAmount = 890.00f,
            };
        }

        private static OtherOccupants GenerateOtherOccupants1()
        {
            return new OtherOccupants
            {
                Title = "Names of Other Occupants 1",
                Name = "Dannel Kreuzer",
                DateofBirth =DateTime.UtcNow.ToString("dddd,2000,08,02"),
                RelationShip = "Intege",
            };
        }
        private static OtherOccupants GenerateOtherOccupants2()
        {
            return new OtherOccupants
            {
                Title = "Names of Other Occupants 2",
                Name = "Raja",
                DateofBirth = DateTime.UtcNow.ToString("dddd,1998,08,02"),
                RelationShip = "Maecen",
            };
        }
        private static AgreementSignature GenerateAgreementSignature1()
        {
            return new AgreementSignature
            {
                Signature = " ",
            };
        }

        private static AgreementSignature GenerateAgreementSignature2()
        {
            return new AgreementSignature
            {
                Signature = " ",
            };
        }
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
