using System.Collections;
using Barcode;
using Barcode.Extensions;
using FluentAssertions;

namespace BarcodeTests;

[TestFixture]
public class BarcodeEan13ConverterTests
{
    [Test]
    public void ConvertToBarcode_ShouldReturnListOfBits()
    {
        var ean13 = new BarcodeEan13Converter();
        var number = 1111111111111;
        
        var actualResult = ean13.ConvertToBarcode(number);
        var expectedResult = new List<BitArray>
        {
            "0011001".ToBitArray(),
            "0011001".ToBitArray(),
            "0110011".ToBitArray(),
            "0011001".ToBitArray(),
            "0110011".ToBitArray(),
            "0110011".ToBitArray(),
            
            "1100110".ToBitArray(),
            "1100110".ToBitArray(),
            "1100110".ToBitArray(),
            "1100110".ToBitArray(),
            "1100110".ToBitArray(),
            "1100110".ToBitArray(),
        };
        
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}