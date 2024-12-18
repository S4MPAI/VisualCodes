using System.Collections;
using CodeHelpers.Extensions;

namespace Barcode;

public class BarcodeEan13Converter : IBarcodeConverter
{
    public const int MaxCodeLength = 13;

    private static readonly Dictionary<int, string> FirstDigitCoding = new()
    {
        { 0, "LLLLLL" },
        { 1, "LLGLGG" },
        { 2, "LLGGLG" },
        { 3, "LLGGGL" },
        { 4, "LGLLGG" },
        { 5, "LGGLLG" },
        { 6, "LGGGLL" },
        { 7, "LGLGLG" },
        { 8, "LGLGGL" },
        { 9, "LGGLGL" }
    };

    private static readonly Dictionary<char, Dictionary<int, BitArray>> DigitsCoding = GetDigitsCoding();

    private static Dictionary<char,Dictionary<int,BitArray>> GetDigitsCoding()
    {
        var lTypeDigitsCoding = GetLTypeDigitsCoding();
        var rTypeDigitsCoding = GetRTypeDigitsCodingFromLTypeCoding(lTypeDigitsCoding);
        var gTypeDigitsCoding = GetGTypeDigitsCodingFromRTypeCoding(rTypeDigitsCoding);
        
        return new Dictionary<char, Dictionary<int, BitArray>>
        {
            { DigitCodeType.LType, lTypeDigitsCoding },
            { DigitCodeType.RType, rTypeDigitsCoding },
            { DigitCodeType.GType, gTypeDigitsCoding }
        };
    }
    
    private static Dictionary<int,BitArray> GetLTypeDigitsCoding()
    {
        var dictionary = new Dictionary<int, BitArray>();
        var digitsCodes = new[]
        {
            "0001101",
            "0011001",
            "0010011",
            "0111101",
            "0100011",
            "0110001",
            "0101111",
            "0111011",
            "0110111",
            "0001011"
        };

        for (var i = 0; i < digitsCodes.Length; i++)
            dictionary[i] = digitsCodes[i].ToBitArray();
        
        return dictionary;
    }
    
    private static Dictionary<int, BitArray> GetRTypeDigitsCodingFromLTypeCoding(Dictionary<int, BitArray> lTypeDigitsCoding) => 
        lTypeDigitsCoding
            .ToDictionary(
                digitCode => digitCode.Key, 
                digitCode => new BitArray(digitCode.Value).Not());
    
    private static Dictionary<int, BitArray> GetGTypeDigitsCodingFromRTypeCoding(Dictionary<int, BitArray> rTypeDigitsCoding)
    {
        var result = new Dictionary<int, BitArray>();

        foreach (var (key, rTypeCode) in rTypeDigitsCoding)
        {
            var bits = new bool[rTypeCode.Length];
            
            rTypeCode.CopyTo(bits, 0);
            Array.Reverse(bits);
            
            result.Add(key, new BitArray(bits));
        }
        
        return result;
    }

    public List<BitArray> ConvertToBarcode(long value)
    {
        var digits = value.GetDigits().ToList();
        digits.AddRange(Enumerable.Repeat(0, MaxCodeLength - digits.Count));
        digits.Reverse();
        
        var groupsCodes = FirstDigitCoding[digits[0]] + "RRRRRR";
        return CodeDigitsGroup(groupsCodes, digits.Skip(1).ToList());
    }

    private static List<BitArray> CodeDigitsGroup(string groupCode, IReadOnlyList<int> digits)
    {
        var result = new List<BitArray>();
        
        if (groupCode.Length > digits.Count)
            throw new ArgumentException("Group code length must be equal digits");

        for (var i = 0; i < groupCode.Length; i++)
        {
            var currentDigitCodeType = groupCode[i];
            var currentDigit = digits[i];
            var code = DigitsCoding[currentDigitCodeType][currentDigit];
            
            result.Add(new BitArray(code));
        }
        
        return result;
    }
}