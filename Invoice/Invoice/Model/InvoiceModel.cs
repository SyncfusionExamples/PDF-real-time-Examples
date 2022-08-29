using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Model
{
    public class InvoiceModel
    {
        public string InvoiceNumber { get; set; }
        public int RefNumber { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly DueDate { get; set; }

        public Address SellerAddress { get; set; }
        public Address CustomerAddress { get; set; }

        public List<OrderItem> Items { get; set; }
        

    }

    public class OrderItem
    {
        decimal tax1 = 4.7M, tax2 = 7.0m;

        public decimal Rate { get; set; }
        public int Qty { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public decimal SampleTax1
        { 
            get { return tax1; }
            set { tax1 = value; }
        }
        public decimal SampleTax2
        {
            get { return tax2; }
            set { tax1 = value; }
        }
        public List<string>Name { get; set; }

    }

    public class Address
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
