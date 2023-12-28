namespace TransitLine.Services
{
    public class DistanceConverterService
    {
        public static (float, string) ConvertDistance(float distance, string unitOfMeasurement)
        {
            float convertedDistance;
            string convertedUnit;

            switch (unitOfMeasurement.ToLower())
            {
                case "feet":
                    convertedDistance = ConvertKilometersToFeet(distance);
                    convertedUnit = "feet";
                    break;
                case "miles":
                    convertedDistance = ConvertKilometersToMiles(distance);
                    convertedUnit = "miles";
                    break;
                case "kilometers":
                    convertedDistance = distance;
                    convertedUnit = "kilometers";
                    break;
                default:
                    throw new ArgumentException("Invalid unit of measurement");
            }

            return (convertedDistance, convertedUnit);
        }

        private static float ConvertKilometersToMiles(float kilometers)
        {
            return kilometers / 1.60934f;
        }

        private static float ConvertKilometersToFeet(float kilometers)
        {
            return kilometers * 3280.84f;
        }
    }
}
