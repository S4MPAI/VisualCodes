namespace Barcode.Extensions;

public static class LongExtensions
{
    public static IEnumerable<int> GetDigits(this long number)
    {
        while (number > 0)
        {
            var digit = (int)(number % 10);
            number /= 10;
            
            yield return digit;
        }
    }
}