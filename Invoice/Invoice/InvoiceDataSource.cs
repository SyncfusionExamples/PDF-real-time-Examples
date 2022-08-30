using Invoice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Invoice
{
    public static class InvoiceDocumentDataSource
    {
        private static Random Random = new Random();
        public static List<string> item = new List<string>(20);

        /// <summary>
        /// Add the item name
        /// </summary>
        public static List<string>  GetItemValue()
        {
            item.Add("Brochure Design");
            item.Add("Web Design Packages(Template) - Basic");
            item.Add("Print Ad - Basic - Color 1.00");
            item.Add("Brochure Design");
            item.Add("Web Design Packages(Template) - Basic");
            item.Add("Print Ad - Basic - Color 1.00");
            item.Add("Brochure Design");
            item.Add("Web Design Packages(Template) - Basic");
            item.Add("Print Ad - Basic - Color 1.00");
            item.Add("Print Ad - Basic - Color 1.00");
            return item;
        }
        /// <summary>
        /// Get the Invoice details
        /// </summary>
        public static InvoiceModel GetInvoiceDetails()
        {
            var items = Enumerable
                .Range(1, 5)
                .Select(i => GenerateRandomOrderItem())
                .ToList();

            return new InvoiceModel
            {
                InvoiceNumber = "INV-17",//Random.Next(1_000, 10_000),
                RefNumber = 321014,//Random.Next(1_000, 10_000),
                IssueDate = DateOnly.FromDateTime(new DateTime(2016,02,24)),
                DueDate = DateOnly.FromDateTime(new DateTime(2016, 02, 24) + TimeSpan.FromDays(14)),

                SellerAddress = GenerateSellerAddress(),
                CustomerAddress = GenerateCustomerAddress(),

                Items = items,               
            };
        }
        /// <summary>
        /// Generate the random order item
        /// </summary>
        private static OrderItem GenerateRandomOrderItem()
        {

            return new OrderItem
            {
                Name=GetItemValue(),    
                Rate = (decimal)Math.Round(Random.NextDouble() * 100, 2),
                Qty = Random.Next(1, 50),
                Discount = Random.Next(1, 100),           
                
            };
            
        }
        /// <summary>
        /// Set the seller address
        /// </summary>
        private static Address GenerateSellerAddress()
        {
            return new Address
            {
                Name = "ZyLKer",
                Street = "7455 Drew Court",
                City = "White City",
                State = "",
                Email = "",
                Phone = "KS 66872",
            };
        }
        /// <summary>
        /// Set the customer address
        /// </summary>
        private static Address GenerateCustomerAddress()
        {
            return new Address
            {
                Name = "Jeff J.Ritchie",
                Street = "4799 Highland View Drive",
                City = "Sacramento",
                State = "illumna kosari",
                Email = "sales@tempora.com",
                Phone = "CA 95815"
            };
        }
    }
}
