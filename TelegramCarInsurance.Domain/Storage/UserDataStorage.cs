using Mindee.Product.Eu.LicensePlate;
using Mindee.Product.Passport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramCarInsurance.Domain.Storage
{
    /// <summary>
    /// Data structure that will store information about the extracted user data 
    /// </summary>
    public class UserDataStorage
    {
        private Dictionary<long, UserData> UserCollection = new Dictionary<long, UserData>();

        /// <summary>
        /// Method that returns information about the user by ChatId
        /// </summary>
        /// <param name="key">ChatId</param>
        /// <returns>UserData</returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        public UserData GetData(long key)
        {
            try
            {
                return UserCollection[key];

            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("{0} sorry, but i don't have any information about your documents. Please, try to upload them again");
            }
        }

        /// <summary>
        /// Method that adds user information to the structure
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddData(long key, LicensePlateV1Document value)
        {
            try
            {
                var row = UserCollection[key];
                UserCollection[key] = new UserData(
                    value, row.PassportDocument);
            }
            catch (KeyNotFoundException e)
            {
                UserCollection.Add(key, new UserData(
                    value, null));
            }
        }

        /// <summary>
        /// Method that adds user information to the structure
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddData(long key, PassportV1Document value)
        {
            try
            {
                var row = UserCollection[key];

                UserCollection[key] = new UserData(
                    row.LicensePlateDocument, value);
            }
            catch (KeyNotFoundException e)
            {
                UserCollection.Add(key, new UserData(
                    null, value));
            }
        }
    }
}
