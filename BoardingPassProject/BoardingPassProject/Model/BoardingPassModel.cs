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
        //Get or set the Passenger Name
        public string PassengerName { get; set; }
        //Get or set the source cities
        public string From { get; set; }
        //Get or set the destination cities
        public string To { get; set; }
        //Get or set the flight
        public string Flight { get; set; }
        //Get or set the date
        public string Date { get; set; }
        //Get or set the time
        public string Time { get; set; }
        //Get or set the gate
        public string Gate { get; set; }
        //Get or set the seat
        public string Seat { get; set; }
        
    }
}
