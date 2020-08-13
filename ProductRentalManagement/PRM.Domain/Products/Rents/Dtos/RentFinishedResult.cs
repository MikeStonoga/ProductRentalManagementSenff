namespace PRM.Domain.Products.Rents.Dtos
{
    public class RentFinishedResult : Rent
    {
        public decimal ValueToPay { get; set; }
    }
}