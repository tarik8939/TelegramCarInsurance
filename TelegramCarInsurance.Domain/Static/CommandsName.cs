using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramCarInsurance.Domain.Static
{
    public static class CommandsName
    {
        public static string StartCommand => "/start";
        public static string ErrorCommand => "Error";
        public static string GeneratePolicyCommand => "Yes, generate Insurance Policy Issuance";
        public static string GeneratePriceQuotationCommand => "Generate Price Quotation";
        public static string PriceDisagreeCommand => "No, disagree";
        public static string ScanPassportCommand => "/passport";
        public static string ScanVehiclePlateCommand => "/vehicle";
        public static string WatchDataCommand => "Watch data";
        public static string ConfirmDataCommand => "Confirm";
    }
}
