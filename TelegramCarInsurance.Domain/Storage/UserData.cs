﻿using System;
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
    public class UserData
    {
        public LicensePlateV1Document LicensePlateDocument { get; set; }
        public PassportV1Document PassportDocument { get; set; }
        /// <summary>
        /// Is all user's data confirmed
        /// </summary>
        public bool IsDataConfirmed { get; set; }

        /// <summary>
        /// Is user agreed with price
        /// </summary>
        public bool IsPriceConfirmed { get; set; }

        public UserData(LicensePlateV1Document licensePlateDocument, PassportV1Document passportDocument)
        {
            LicensePlateDocument = licensePlateDocument;
            PassportDocument = passportDocument;
        }

        /// <summary>
        /// Method that checks whether the user has downloaded all the information
        /// </summary>
        /// <returns>bool</returns>
        public bool IsDataFilled()
        {
            return LicensePlateDocument != null && PassportDocument != null;
        }

        /// <summary>
        /// Method that confirms the data
        /// </summary>
        public void ConfirmData()
        {
            IsDataConfirmed = true;
        }

        /// <summary>
        /// Method that confirms the price
        /// </summary>
        public void ConfirmPrice()
        {
            IsPriceConfirmed = true;
        }
    }
}
