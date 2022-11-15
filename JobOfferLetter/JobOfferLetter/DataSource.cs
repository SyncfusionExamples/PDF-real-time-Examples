using JobOfferLetter.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JobOfferLetter
{
    public static class DataSourceJobOfferLetterDocument
    {
        /// <summary>
        /// Get the JobOfferLetter details
        /// </summary>    
        public static JobOfferLetterModel GetDetails()
        {
            return new JobOfferLetterModel
            {
                CustomerAddress = GenerateCustomerAddress(),
                OfferLetterContent = GenerateOfferLetterContentDetails(),
            };
        }
        /// <summary>
        /// Set the customer address
        /// </summary>
        private static Address GenerateCustomerAddress()
        {
            return new Address
            {
                Street = "123 Dokato, St Church,",
                City = "New York, NY 182916",               
                Phone = "456-6780-21",
                Website = "amazefoxsite.com"
            };
        }
        /// <summary>
        /// Generate the OfferLetterContentDetails
        /// </summary>
        /// <returns>OfferLetterContent</returns>
        private static OfferLetterContent GenerateOfferLetterContentDetails()
        {
            string json = File.ReadAllText(@"../../../Assets/StringofData.json");
            OfferLetterContent data = JsonConvert.DeserializeObject<OfferLetterContent>(json);
            return new OfferLetterContent
            {
                BodyofContent = data.BodyofContent,
            };
        }
    }
}
