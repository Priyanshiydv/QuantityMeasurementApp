using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    // service class to compare two feet values
    public class QuantityMeasurementService
    {
        // method to check equality of two feet objects
        public bool AreEqual(Feet feet1, Feet feet2)
        {
            // handle null safety
            if (feet1 == null || feet2 == null)
                return false;

            // use Equals method from Feet class
            return feet1.Equals(feet2);
        }
    }
}