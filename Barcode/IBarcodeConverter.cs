using System.Collections;

namespace Barcode;

public interface IBarcodeConverter
{
    public List<BitArray> ConvertToBarcode(long value);
}