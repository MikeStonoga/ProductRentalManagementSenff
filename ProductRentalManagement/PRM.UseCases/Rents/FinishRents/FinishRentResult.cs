namespace PRM.UseCases.Rents.FinishRents
{
    public class FinishRentResult
    {
        public decimal ValueToPay { get; }
        
        public FinishRentResult()
        {
            ValueToPay = 0;
        }

        public FinishRentResult(decimal valueToPay)
        {
            ValueToPay = valueToPay;
        }
    }
}