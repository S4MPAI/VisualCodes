using System.Collections;

namespace Barcode.Extensions;

public static class StringExtensions
{
    public static BitArray ToBitArray(this string value)
    {
        var result = new List<bool>();

        foreach (var ch in value)
        {
            if (!char.IsDigit(ch) || ch is not '1' and not '0')
                throw new ArgumentException("String is not a bits");
            
            result.Add(ch == '1');
        }
        
        return new BitArray(result.ToArray());
    }
}