using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Model
{
    public class InvoiceModel
    {
        //Get or set the invoice number
        public string InvoiceNumber { get; set; }

        //Get or set the Reference number
        public int RefNumber { get; set; }

        //Get or set the Issue date
        public DateOnly IssueDate { get; set; }

        //Get or set the Due date
        public DateOnly DueDate { get; set; }

        //Get or set the seller address
        public Address SellerAddress { get; set; }

        //Get or set the customer address
        public Address CustomerAddress { get; set; }

        //Get or set the order items
        public List<OrderItem> Items { get; set; }

       
        

    }

    public class OrderItem
    {
        decimal tax1 = 4.7M, tax2 = 7.0m;

        //Get or set the item rate
        public decimal Rate { get; set; }

        //Get or set the item quantity
        public int Qty { get; set; }

        //Get or set the item discount
        public decimal Discount { get; set; }

        //Get or set the item amount
        public decimal Amount { get; set; }

        //Get or set the item sampletax1
        public decimal SampleTax1
        { 
            get { return tax1; }
            set { tax1 = value; }
        }

        //Get or set the item sampletax2
        public decimal SampleTax2
        {
            get { return tax2; }
            set { tax1 = value; }
        }

        public List<string> name = GetItemValue();
        public static List<string> item = new List<string>(20);
        //Get or set the item name
        public List<string> Name
        {
            get
            {
                return name;
            }
            set { name = value; }
        }
        public static List<string> GetItemValue()
        {
            item.Add("API Development");
            item.Add("Desktop Software Development");
            item.Add("Web Design");
            item.Add("Marketing Design");
            item.Add("Search Engines Optimization");
            item.Add("Print Ad - Basic - Color 1.00");
            item.Add("Site admin development");
            item.Add("Web Design Packages(Template) - Basic");
            item.Add("Redesign a service site");
            item.Add("Print Ad - Basic - Color 1.00");
            return item;
        }

    }

    public class Address
    {
        //Get or se the name
        public string Name { get; set; }

        //Get or se the street
        public string Street { get; set; }

        //Get or se the city
        public string City { get; set; }

        //Get or se the state
        public string State { get; set; }

        //Get or se the email
        public string Email { get; set; }

        //Get or se the phone
        public string Phone { get; set; }
    }
}
