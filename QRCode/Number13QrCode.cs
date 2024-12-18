using System.Drawing;
using CodeHelpers.Extensions;
using ZXing.Common;

namespace QRCode;

public class Number13QrCode : INumberQrCode
{
    public const int CodeLength = 13;
    public const int MaxGroupLength = 3;
    public static readonly BitArray CodingType = GetCodingType();
    public const int VersionTypeBitsLength = 10;

    private static BitArray GetCodingType()
    {
        var result = new BitArray();
        result.appendBits(1, 4);
        
        return result;
    }

    public Bitmap Encode(long number)
    {
        var numberGroups = ConvertNumberToNumberGroups(number);

        var  numberInBits = new BitArray();

        for (var i = 0; i < numberGroups.Count; i++)
        {
            if (i == numberGroups.Count - 1)
                numberInBits.appendBits(numberGroups[i], 4);
            
            numberInBits.appendBits(numberGroups[i], 10);
        }

        return null;
    }

    private static List<int> ConvertNumberToNumberGroups(long number)
    {
        var digits = number.GetDigits().Take(13).ToList();
        digits.AddRange(Enumerable.Repeat(0, CodeLength - digits.Count));
        digits.Reverse();
        return digits.Select((x, i) => (value: x, pos: i))
            .GroupBy(x => x.pos / 3, x => x.value)
            .Select(x =>
            {
                var count = x.Count();
                return x.Aggregate(0, (number, digit) => number + digit * count--);
            })
            .ToList();
    }
}