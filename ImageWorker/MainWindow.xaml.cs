using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace ImageWorker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonChooseFile_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения (*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|Все файлы (*.*)|*.*",
                Title = "Выберите изображение"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));
                    imagePreview.Source = bitmapImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при открытии изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ButtonProcess_OnClick(object sender, RoutedEventArgs e)
        {
            var texture = ImageEffects.GetIntArrayFromBitmapSource(imagePreview.Source as BitmapSource);
            int[,] blurredTexture = ImageEffects.BlurTexture(texture, 3); // Размер ядра: 3x3
            var originSource = imagePreview.Source as BitmapSource;
            var resultBitmap = new WriteableBitmap(originSource);
            ImageEffects.Brightness(resultBitmap, 1.5f);
            resultImage.Source = resultBitmap;
            // resultImage.Source = blurredTexture;
        }
    }
}