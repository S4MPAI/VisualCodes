// See https://aka.ms/new-console-template for more information

using System.Drawing;
using System.Drawing.Imaging;
using Barcode;

var visualizer = new BarcodeVisualizer(new BarcodeEan13Converter());
var number = long.Parse(Console.ReadLine());
var bitMap = visualizer.Visualize(number);
bitMap.Save("barcode.png", ImageFormat.Png);