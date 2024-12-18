using System.Collections;
using System.Drawing;

namespace Barcode;

public class BarcodeVisualizer(BarcodeEan13Converter converter)
{
    private readonly BitArray _separator = new([false, true, false, true, false]);
    private readonly BitArray _border = new([true, false, true]);
    private const int BarSize = 60;

    public Bitmap Visualize(long number)
    {
        var bits = converter.ConvertToBarcode(number);
        var bitmap = new Bitmap(7 * 12 + 5 + 3 + 3, BarSize);
        bits.Insert(6, _separator);
        bits.Insert(0, _border);
        bits.Add(_border);
        

        InsertBitsOnBitmap(bitmap, bits);
        
        return bitmap;
    }

    private void InsertBitsOnBitmap(Bitmap bitmap, List<BitArray> bits)
    {
        var bitsCount = bits[0].Length;
        var readBits = 0;
        foreach (var digit in bits)
        {
            for (var j = 0; j < digit.Count; j++)
            {
                var color = digit[j] ? Color.Black : Color.White;
                
                for (var k = 0; k < BarSize; k++)
                    bitmap.SetPixel(readBits + j, k, color);
            }
            
            readBits += digit.Count;
        }
    }
}