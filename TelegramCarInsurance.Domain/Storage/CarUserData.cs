using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mindee.Product.Eu.LicensePlate;
using Mindee.Product.Passport;

namespace TelegramCarInsurance.Domain.Storage
{
    /// <summary>
    /// Class that will store information about the extracted user data 
    /// </summary>
    public class CarUserData
    {
        public LicensePlateV1Document LicensePlateDocument { get; set; }
        public PassportV1Document PassportDocument { get; set; }
        public bool IsConfirmed { get; set; }

        public CarUserData(LicensePlateV1Document licensePlateDocument, PassportV1Document passportDocument)
        {
            LicensePlateDocument = licensePlateDocument;
            PassportDocument = passportDocument;
            IsConfirmed = false;
        }

        /// <summary>
        /// Function that checks whether the user has downloaded all the information
        /// </summary>
        /// <returns>bool</returns>
        public bool IsDataFilled()
        {
            return LicensePlateDocument != null && PassportDocument != null;
        }

        public void ConfirmData()
        {
            IsConfirmed = true;
        }
    }


}
