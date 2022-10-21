using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JobOfferLetter.Model
{
    public class JobOfferLetterModel
    {
        //Get or set the CustomerAddress
        public Address CustomerAddress { get; set; }

        //Get or set the OfferLetterContent
        public OfferLetterContent OfferLetterContent { get; set; }
    }
    /// <summary>
    /// Address details class
    /// </summary>
    public class Address
    {        
        //Get or set the Street
        public string Street { get; set; }

        //Get or set the City
        public string City { get; set; }

        //Get or set the Phone
        public string Phone { get; set; }

        //Get or set the Website
        public string Website { get; set; }
    }
    /// <summary>
    /// OfferLetterContent class
    /// </summary>
    public class OfferLetterContent
    {
        //Get or set the BodyofContent
        public string BodyofContent { get; set; }
    }
}
