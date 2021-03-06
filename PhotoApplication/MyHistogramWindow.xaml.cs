﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhotoApplication
{
    /// <summary>
    /// Interaction logic for myHistogramWindow.xaml
    /// </summary>
    public partial class MyHistogramWindow : Window
    {
        private BitmapSource mySourceBitmap;
        private byte[] pixelData;
        private long[] amount;
        private long maxValue;
        public MyHistogramWindow()
        {
            InitializeComponent();
            amount = new long[256];
        }
        public void setMyPixelData(byte[] pd, BitmapSource src)
        {
            pixelData = pd;
            mySourceBitmap = src;
            countPixels();
        }
        private void countPixels()
        {
            int rawStride = (mySourceBitmap.PixelWidth * 32 + 7) / 8;
            for (int y = 0; y < mySourceBitmap.PixelHeight; y++)
            {
                int yIndex = y * rawStride;
                for (int x = 0; x < rawStride; x += 4)
                {
                    int max = Math.Max(pixelData[x + yIndex + 2], Math.Max(pixelData[x + yIndex + 1], pixelData[x + yIndex]));
                    amount[max]++;
                }
            }
            lookForMaxValue();
        }
        private void lookForMaxValue()
        {
            maxValue = -1;
            for(int i = 0; i < 256; i++)
            {
                if (amount[i] > maxValue)
                    maxValue = amount[i];
            }
        }
        public void drawHistogram()
        {
            double w = ((double)maxValue / 200);
            w = 1.0 / w;
            for (int i = 1; i < 255; i++)
            {
                Line line = new Line();
                line.Stroke = Brushes.Black;
                line.X1 = (2*i)-1;
                line.X2 = 2*i;
                line.Y1 = 200.0 - (amount[i - 1] * w);
                line.Y2 = 200.0 - (amount[i] * w);
                line.StrokeThickness = 2;
                histogramImage.Children.Add(line);
                
                
                line = new Line();
                line.Stroke = Brushes.Gray;
                line.X1 = (2 * i) - 1;
                line.X2 = (2 * i) - 1;
                line.Y1 = 200.0 - (amount[i - 1] * w);
                line.Y2 = 200;
                line.StrokeThickness = 1;
                histogramImage.Children.Add(line);
                
                line = new Line();
                line.Stroke = Brushes.Gray;
                line.X1 = 2 * i;
                line.X2 = 2 * i;
                line.Y1 = 200;
                line.Y2 = 200.0 - (amount[i] * w);
                line.StrokeThickness = 1;
                histogramImage.Children.Add(line);
              

            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.isHistogramOpen = false;
        }
    }
}
