namespace TransitLine.Tools
{
    public class PaymentHelper
    {
        public static decimal CalculatePayment(float distance, int numberUnits, decimal cargoWeight)
        {
            decimal baseRate = 0.02m;
            decimal distanceCharge = (decimal)distance * 0.03m;
            decimal unitCharge = numberUnits * 1m;
            decimal weightCharge = cargoWeight * 0.7m;

            decimal totalPayment = baseRate + distanceCharge + unitCharge + weightCharge;
            return totalPayment;
        }

    }
}
