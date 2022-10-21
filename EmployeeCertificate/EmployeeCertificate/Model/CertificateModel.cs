using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeCertificate.Model
{
    /// <summary>
    /// Employee details model class
    /// </summary>
    public class CertificateModel
    {
        /// <summary>
        /// Get or set the employee name
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// Get or set the manger sign
        /// </summary>
        public string ManagerSign { get; set; }

        /// <summary>
        /// Get or set the date of sign
        /// </summary>
        public string DateOfSign { get; set; }
    }
}
