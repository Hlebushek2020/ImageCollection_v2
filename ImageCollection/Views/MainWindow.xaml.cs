using ImageCollection.Interfaces;
using ImageCollection.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageCollection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_viewModel.CollectionsManager != null)
            {
                ICollection toCollection = _viewModel.CollectionsManager.HotkeyManager.GetCollectionByHotkeys(e.KeyboardDevice.Modifiers, e.Key);
                if (toCollection != null && _viewModel.SelectedCollection != null)
                {
                    int currentIndex = listBox_CollectionItems.SelectedIndex;
                    _viewModel.CollectionsManager.ToCollection(_viewModel.SelectedCollection, toCollection);
                    listBox_CollectionItems.Items.MoveCurrentToPosition(Math.Min(currentIndex, listBox_CollectionItems.Items.Count - 1));
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _viewModel.SelectedCollection?.StopInitPreviewImages(true);
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e) => PreparationOfImageDisplayElement();

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => PreparationOfImageDisplayElement();

        private void PreparationOfImageDisplayElement()
        {
            if (image_Image.Source != null && image_Image.Width != 0)
            {
                double actualWidth = ((Grid)image_Image.Parent).ActualWidth;
                double actualHeight = ((Grid)image_Image.Parent).ActualHeight;
                double pixelWidth = ((BitmapImage)image_Image.Source).PixelWidth;
                double pixelHeight = ((BitmapImage)image_Image.Source).PixelHeight;
                if (pixelWidth < actualWidth && pixelHeight < actualHeight)
                {
                    image_Image.Height = pixelHeight;
                    image_Image.Width = pixelWidth;
                    image_Image.Stretch = Stretch.Fill;
                }
                else
                {
                    image_Image.Height = double.NaN;
                    image_Image.Width = double.NaN;
                    image_Image.Stretch = Stretch.Uniform;
                }
            }
        }

        private void ListBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                if (e.Key == Key.Up)
                {
                    if (listBox_CollectionItems.SelectedIndex > 0)
                        listBox_CollectionItems.Items.MoveCurrentToPrevious();
                }
                else if (e.Key == Key.Down)
                {
                    if (listBox_CollectionItems.SelectedIndex != listBox_CollectionItems.Items.Count - 1)
                        listBox_CollectionItems.Items.MoveCurrentToNext();
                }
                listBox_CollectionItems.ScrollIntoView(listBox_CollectionItems.SelectedItem);
                e.Handled = true;
            }
        }
    }
}