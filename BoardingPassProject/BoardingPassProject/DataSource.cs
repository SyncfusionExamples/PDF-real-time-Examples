using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardingPassProject.Model;
using Syncfusion.Drawing;
using Syncfusion.Pdf.Graphics;

namespace BoardingPassProject
{
    public static class DataSourceBoardingPassDocument
    {
        /// <summary>
        /// Get the BoardingPass details
        /// </summary>    
        public static BoardingPassModel GetDetails()
        {
            var dateTime = DateTime.Now;
            
            return new BoardingPassModel
            {
                PassengerName = "JOHN SMITH",
                From = "Moscow",
                To = "San Francisco",
                Flight = "85SKL",
                Date= dateTime.ToString("dd MMM yyyy"),
                Time = dateTime.ToString("t"),
                Gate = "08",
                Seat = "15B",
            };
        }
    }
}
