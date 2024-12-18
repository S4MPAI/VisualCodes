using System.Drawing;

namespace QRCode;

public interface INumberQrCode
{
    public Bitmap Encode(long number);
}