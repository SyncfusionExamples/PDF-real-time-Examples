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
        public decimal Rate { get; set; }
        public int Qty { get; set; }
        public int Discount { get; set; }
        public decimal Amount { get; set; }
        public float SampleTax1 { get; set; }
        public float SampleTax2 { get; set; }
        public List<string> Name
        {
            get
            {
                return GetItemValue();
            }
            set
            {

            }
        }
        public static List<string> item = new List<string>(10);

        public static List<string> GetItemValue()
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
