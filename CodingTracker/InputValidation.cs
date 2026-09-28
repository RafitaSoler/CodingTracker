using System.Globalization;

namespace CodingTracker
{
    internal static class InputValidation
    {
        /// <summary>
        /// Checks if date is valid
        /// </summary>
        /// <param name="dateInput"></param>
        /// <returns></returns>
        internal static bool ValidateDateTime(string dateInput, out DateTime dateValidated)
        {
            bool validDate = false;
            if (DateTime.TryParseExact(dateInput, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValidated))
            {
                validDate = true;
            }
            return validDate;
        }

        internal static bool ValidateEndTime(DateTime startTime, DateTime endTime)
        {
            return (endTime - startTime).TotalSeconds >= 0;
        }
    }
}
