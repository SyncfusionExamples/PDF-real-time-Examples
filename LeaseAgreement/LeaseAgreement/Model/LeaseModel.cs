using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaseAgreement.Models
{
    /// <summary>
    /// LeaseModel class.
    /// </summary>
    public class LeaseModel
    {
        /// <summary>
        /// Get or set the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get or set the parties
        /// </summary>
        public Parties Parties { get; set; }

        /// <summary>
        /// Get or set the lease period
        /// </summary>
        public LeasePeriod LeasePeriod { get; set; }

        /// <summary>
        /// Get or set the other occupants
        /// </summary>
        public OtherOccupants OtherOccupants1 { get; set; }

        /// <summary>
        /// Get or set the other occupants
        /// </summary>
        public OtherOccupants OtherOccupants2 { get; set; }

        /// <summary>
        /// Get or set the lessor signature
        /// </summary>
        public AgreementSignature LessorSignature { get; set; }

        /// <summary>
        /// Get or set the lessor signature
        /// </summary>
        public AgreementSignature LesseeSignature { get; set; }

        /// <summary>
        /// Get or set the Terms of Lease
        /// </summary>
        public TermsofLease TermsofLease { get; set; }
    }
    /// <summary>
    /// Parties class
    /// </summary>
    public class Parties
    {
        /// <summary>
        /// Get or set the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get or set the lessor
        /// </summary>
        public string Lessor { get; set; }

        /// <summary>
        /// Get or set the Lessee
        /// </summary>
        public string Lessee { get; set; }

        /// <summary>
        /// Get or set the lease term
        /// </summary>
        public string LeaseTerm { get; set; }

        /// <summary>
        /// Get or set the property details
        /// </summary>
        public PropertyDetails PropertyDetails { get; set; }


        /// <summary>
        /// Get or set the payment period
        /// </summary>
        public DateTime PaymentPeriod { get; set; }

    }
    /// <summary>
    /// Property details class
    /// </summary>
    public class PropertyDetails
    {
        /// <summary>
        /// Get or set the plat number
        /// </summary>
        public string PlatNo { get; set; }

        /// <summary>
        /// Get or set the street
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Get or set the city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Get or set the pincode
        /// </summary>
        public string PinCode { get; set; }
    }

    /// <summary>
    /// Lease period class
    /// </summary>
    public class LeasePeriod
    {
        /// <summary>
        /// Get or set the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get or set the lease form
        /// </summary>
        public string LeaseFrom { get; set; }

        /// <summary>
        /// Get or set the lease until
        /// </summary>
        public string LeaseUntil { get; set; }

        /// <summary>
        /// Get or set the security debit amount
        /// </summary>
        public float SecurityDebitAmount { get; set; }

        /// <summary>
        /// Get or set the monthly lease amount
        /// </summary>
        public float MonthlyLeaseAmount { get; set; }
    }

    /// <summary>
    /// Other occupants class
    /// </summary>
    public class OtherOccupants
    {
        /// <summary>
        /// Get or set the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get or set the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set the relationship
        /// </summary>
        public string RelationShip { get; set; }

        /// <summary>
        /// Get or set the date of birth
        /// </summary>
        public DateTime DateofBirth { get; set; }
    }

    /// <summary>
    /// Agreement signature class
    /// </summary>
    public class AgreementSignature
    {
        /// <summary>
        /// Get or set the signature
        /// </summary>
        public string Signature { get; set; }
    }

    /// <summary>
    /// Terms of lease class
    /// </summary>
    public class TermsofLease
    {
        /// <summary>
        /// Get or set the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get or set the Use and Occupancy
        /// </summary>
        public string UseandOccupancy { get; set; }

        /// <summary>
        /// Get or set the Assignment and Sublease
        /// </summary>
        public string AssignmentandSublease { get; set; }

        /// <summary>
        /// Get or set the Rent
        /// </summary>
        public string Rent { get; set; }

        /// <summary>
        /// Get or set the Abandonment
        /// </summary>
        public string Abandonment { get; set; }

        /// <summary>
        /// Get or set the Necessary Expenses
        /// </summary>
        public string NecessaryExpenses { get; set; }

        /// <summary>
        /// Get or set the Judicial Action
        /// </summary>
        public string JudicialAction { get; set; }

        /// <summary>
        /// Get or set the Improvements to the Premises
        /// </summary>
        public string ImprovementPremises { get; set; }

        /// <summary>
        /// Get or set the Pets
        /// </summary>
        public string Pets { get; set; }

    }
}
