namespace PRM.Domain.Products.Rents.Dtos
{
    public class RentFinishedDto : Rent
    {
        public decimal ValueToPay { get; set; }
    }
}