using PRM.Domain.Products.Rents;
using PRM.InterfaceAdapters.Presenters.BaseCore.Dtos;

namespace PRM.InterfaceAdapters.Presenters.Products.Rents
{
    public class RentView : FullAuditedEntityView<Rent>
    {
        public string ProductId { get; set; }
        public string UserId { get; set; }
        public string DailyPrice { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DailyLateFee { get; set; }
        public string CurrentRentPaymentValue { get; set; }
        public string PriceWithoutLateFee { get; set; }
        public string RentDays { get; set; }
        public string LateFee { get; set; }
        public bool IsLate { get; set; }
        public string LateDays { get; set; }
    }
}