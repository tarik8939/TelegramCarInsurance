using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramCarInsurance.Domain.Static
{
    /// <summary>
    /// Static class with all error's types
    /// </summary>
    public static class StaticErrors
    {
        public static string DefaultError => "{0} oops, something went wrong";
        public static string UnsupportedTypeMessage => "{0} sorry, but I don't support {1} type of message";
        public static string NotExistingCommand => "{0} sorry, but I don't support {1} command";
        public static string UnsupportedTypeDocument => "{0} sorry, but I don't support the type of documents what you sent. What does it {1} mean?";
        public static string UnknownTypeDocument => "{0} sorry, but I don't understand which document you sent. Could you please send it again with the specified document type?";
        public static string NotUploadedDocument => "{0} sorry, but you didn't upload document with personal data for {1}";
        public static string DoNotHaveDocument => "{0} sorry, but i don't have data about your {1}, try upload it again";
        public static string NotConfirmedData => "{0} sorry, but you didn't confirm your personal data, please press Confirm button";
        public static string GeneratePolicyError => "{0} sorry, but i can't generate your insurance policy. Please, try again later";
        public static string GenerateAnswerError => "{0} sorry, but i can't generate answer for your question. Please, try again later";
        public static string ExtractDataError => "{0} sorry, but i can't extract data from your documents. Please, try again later";

    }
}
