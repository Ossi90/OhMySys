namespace OhMySys.Common.Helpers;


public class StringSizeConverter
{
    /// <summary>
    /// This method converts a size in bytes to a human-readable string.
    /// </summary>
    /// <param name="sizesGb"></param>
    /// <returns></returns>
    public static string ConvertBytesToReadableString(double[] sizesGb)
    {
        const double mbToGb = 1024.0;
        const double gbToTb = 1024.0;

        bool allInSameCategory = true;
        string category = "GB";

        foreach (var size in sizesGb)
        {
            if (size < 1)
            {
                category = "MB";
            }
            else if (size >= gbToTb)
            {
                category = "TB";
            }

            if ((category == "MB" && size >= 1) ||
                (category == "TB" && size < gbToTb) ||
                (category == "GB" && (size < 1 || size >= gbToTb)))
            {
                allInSameCategory = false;
                break;
            }
        }

        var outputString = string.Empty;
        foreach (var size in sizesGb)
        {
            double convertedSize;
            string unit;

            if (allInSameCategory)
            {
                switch (category)
                {
                    case "MB":
                        convertedSize = size * mbToGb;
                        unit = "MB";
                        break;

                    case "TB":
                        convertedSize = size / gbToTb;
                        unit = "TB";
                        break;

                    default:
                        convertedSize = size;
                        unit = "GB";
                        break;
                }
                outputString += $"{convertedSize:0.0} / ";
            }
            else
            {
                if (size < 1)
                {
                    convertedSize = size * mbToGb;
                    unit = "MB";
                }
                else if (size >= gbToTb)
                {
                    convertedSize = size / gbToTb;
                    unit = "TB";
                }
                else
                {
                    convertedSize = size;
                    unit = "GB";
                }
                outputString += $"{convertedSize:0.0} {unit} / ";
            }
        }

        if (outputString.EndsWith(" / "))
        {
            outputString = outputString.Substring(0, outputString.Length - 3);
        }

        if (allInSameCategory)
        {
            outputString += $" {category}";
        }

        return outputString;
    }
}