using EmployeeCertificate.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeCertificate
{
    /// <summary>
    /// Certificate details 
    /// </summary>
    internal class CertificateDataSource
    {
        /// <summary>
        /// Get the certificate details
        /// </summary>
        /// <returns>Certificate details</returns>
        public static CertificateModel GetCertificateDetails()
        {
            return new CertificateModel
            {
                EmployeeName = "John Smith",
                ManagerSign = "../../../Assets/Sign.png", 
                DateOfSign = DateTime.Now.ToString("dd MMM yyyy"),
            };
        }
    }
}
