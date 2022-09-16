using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaseAgreement.Models
{
    public class LeaseModel
    {
        public string Title { get; set; }

        public Parties Parties { get; set; }

        public LeasePeriod LeasePeriod { get; set; }

        public OtherOccupants OtherOccupants1 { get; set; }

        public OtherOccupants OtherOccupants2 { get; set; }

        public AgreementSignature LessorSignature { get; set; }

        public AgreementSignature LesseeSignature { get; set; }

        public TermsofLease TermsofLease { get; set; }
    }
    public class Parties
    {
        public string Title { get; set; }
        public string Lessor { get; set; }
        public string Lessee { get; set; }
        public string LeaseTerm { get; set; }

        public PropertyDetails PropertyDetails { get; set; }

        public DateTime PaymentPeriod { get; set; }

    }
    public class PropertyDetails
    {
        public string PlatNo { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int PinCode { get; set; }
    }

    public class LeasePeriod
    {
        public string Title { get; set; }
        public string LeaseFrom { get; set; }
        public string LeaseUntil { get; set; }

        public float SecurityDebitAmount { get; set; }
        public float MonthlyLeaseAmount { get; set; }
    }
    public class OtherOccupants
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string RelationShip { get; set; }
        public DateTime DateofBirth { get; set; }
    }

    public class AgreementSignature
    {
        public string Signature { get; set; }
    }
    public class TermsofLease
    {
        public string Title { get; set; }
        public string UseandOccupancy { get; set; }
        public string AssignmentandSublease { get; set; }
        public string Rent { get; set; }
        public string Abandonment { get; set; }
        public string NecessaryExpenses { get; set; }
        public string JudicialAction { get; set; }
        public string ImprovementPremises { get; set; }
        public string Pets { get; set; }

    }
}
