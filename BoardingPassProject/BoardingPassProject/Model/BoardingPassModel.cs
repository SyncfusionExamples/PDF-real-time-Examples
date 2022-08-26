using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardingPassProject.Model
{
    public class BoardingPassModel
    {
        public string PassengerName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Flight { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Gate { get; set; }
        public string Seat { get; set; }
        
    }
}
