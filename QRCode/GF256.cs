namespace QRCode;

public class GF256
{
    private const uint PrimitivePolynomial = 0x11D; // Порождающий полином

    public static uint Add(uint a, uint b)
    {
        return a ^ b;
    }

    public static uint Multiply(uint a, uint b)
    {
        uint result = 0;
        for (var i = 0; i < 8; i++)
            if ((b & (1U << i)) != 0)
                result ^= a << i;
        
        return result;
    }

    public static uint Divide(uint dividend, uint divisor)
    {
        uint quotient = 0;
        uint remainder = dividend;
        
        for (int i = 7; i >= 0; i--)
        {
            if (remainder >= divisor)
            {
                remainder -= divisor;
                quotient |= 1U << i;
            }
        }
        
        return quotient;
    }

    public static uint Power(uint baseValue, int exponent)
    {
        uint result = 1;
        for (int i = 0; i < Math.Abs(exponent); i++)
        {
            result = Multiply(result, baseValue);
        }
        return exponent > 0 ? result : Divide(result, PrimitivePolynomial);
    }
}