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
        

        /// <summary>
        /// Add the item name
        /// </summary>
        
        /// <summary>
        /// Get the Invoice details
        /// </summary>
        public static InvoiceModel GetInvoiceDetails()
        {
            var items = Enumerable
                .Range(1, 8)
                .Select(i => GenerateRandomOrderItem())
                .ToList();

            return new InvoiceModel
            {
                InvoiceNumber = "#23698720",
                RefNumber = 321014,
                IssueDate = DateOnly.FromDateTime(new DateTime(2016,02,24)),
                DueDate = DateOnly.FromDateTime(new DateTime(2016, 02, 24)),

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
                Name = "John Smith",
                Street = "398 W Broadway, New York,",
                City = "North Dakota, 10012",
                State = "",
                Email = "",
                Phone = "(646) 392-7868"
            };
        }
    }
}
