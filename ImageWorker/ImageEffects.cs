using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageWorker;

public class ImageEffects
{
    public static void Brightness(WriteableBitmap resultBitmap, float brightnessFactor)
    {
        byte[] pixels = new byte[resultBitmap.PixelWidth * resultBitmap.PixelHeight * 4]; // Assuming 32 bits per pixel (4 bytes)

        resultBitmap.CopyPixels(pixels, resultBitmap.PixelWidth * 4, 0);

        for (int i = 0; i < pixels.Length; i += 4)
        {
            byte b = pixels[i];
            byte g = pixels[i + 1];
            byte r = pixels[i + 2];
            byte a = pixels[i + 3];

            // Применение яркости к каждому каналу
            r = (byte)Math.Min(255, r * brightnessFactor);
            g = (byte)Math.Min(255, g * brightnessFactor);
            b = (byte)Math.Min(255, b * brightnessFactor);

            pixels[i] = b;
            pixels[i + 1] = g;
            pixels[i + 2] = r;
        }

        resultBitmap.WritePixels(new Int32Rect(0, 0, resultBitmap.PixelWidth, resultBitmap.PixelHeight), pixels, resultBitmap.PixelWidth * 4, 0);
    }
    
    public static int[,] BlurTexture(int[,] texture, int kernelSize)
    {
        int width = texture.GetLength(1);
        int height = texture.GetLength(0);

        int[,] blurredTexture = new int[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                blurredTexture[y, x] = CalculateBlurredValue(texture, x, y, kernelSize);
            }
        }

        return blurredTexture;
    }

    public static int CalculateBlurredValue(int[,] texture, int x, int y, int kernelSize)
    {
        int width = texture.GetLength(1);
        int height = texture.GetLength(0);

        int sum = 0;
        int count = 0;

        for (int i = -kernelSize / 2; i <= kernelSize / 2; i++)
        {
            for (int j = -kernelSize / 2; j <= kernelSize / 2; j++)
            {
                int newX = x + j;
                int newY = y + i;

                if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                {
                    sum += texture[newY, newX];
                    count++;
                }
            }
        }

        return sum / count;
    }
    
    public static int[,] GetIntArrayFromBitmapSource(BitmapSource bitmapSource)
    {
        int width = bitmapSource.PixelWidth;
        int height = bitmapSource.PixelHeight;

        int[,] resultArray = new int[height, width];
        // var result = BitmapSource.Create(width, height, 1d, 1d, new PixelFormat(), new BitmapPalette(bitmapSource, 32), resultArray); 

        // Буфер для получения цветов пикселей
        byte[] pixelBuffer = new byte[width * height * 4]; // 4 байта на пиксель (ARGB)

        // Заполняем буфер изображения данными
        bitmapSource.CopyPixels(pixelBuffer, width * 4, 0);

        // Извлекаем данные из буфера
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = (y * width + x) * 4; // каждый пиксель состоит из 4 байт (ARGB)

                // Получаем цвета из буфера и формируем цвет в формате int
                int color = (pixelBuffer[index + 3] << 24) | (pixelBuffer[index] << 16) | (pixelBuffer[index + 1] << 8) | pixelBuffer[index + 2];

                resultArray[y, x] = color;
            }
        }

        return resultArray;
    }
}