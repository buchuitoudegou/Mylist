using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Mylist
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    
    public sealed partial class NewPage : Page
    {
        private ItemList curList;
        private FileOpenPicker openPicker;
        public NewPage()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
            AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            this.InitializeComponent();
            curList = new ItemList();
            openPicker = new FileOpenPicker();
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            curList = (ItemList)e.Parameter;
        }
        public async void PictureOpening()
        {
            StorageFile FilePicture = await openPicker.PickSingleFileAsync();
            BitmapImage bitmap = new BitmapImage();
            try
            {
                using (var stream = await FilePicture.OpenAsync(FileAccessMode.ReadWrite))
                {
                    bitmap.SetSource(stream);
                }
                img.Source = bitmap;
            }
            catch (Exception i)
            {
                var dialog = new MessageDialog(i.ToString());
                await dialog.ShowAsync();
            }
        }
        private void uploadPicture(object sender, RoutedEventArgs e)
        {
            PictureOpening();
        }
        private void clearAll(object sender, RoutedEventArgs e)
        {
            clear();
        }
        private void clear()
        {
            desContent.Text = "";
            titleContent.Text = "";
            datepick.Date = DateTime.Now;
            img.Width = 300;
            img.Height = 200;
            slider.Value = 0;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(img.BaseUri, "/Assets/StoreLogo.png");
            img.Source = bitmapImage;
        }
        private bool DateJudge(int year, int month, int day)
        {
            int curYear = DateTime.Now.Year;
            int curMonth = DateTime.Now.Month;
            int curDay = DateTime.Now.Day;
            if (year > curYear)
                return true;
            else if (year == curYear && month > curMonth)
                return true;
            else if (year == curYear && month == curMonth && day >= curDay)
                return true;
            else return false;
        }
        private bool IsTitleDesWritten()
        {
            if (titleContent.Text != "" && desContent.Text != "")
                return true;
            else return false;
        }
        private async void Add(object sender, RoutedEventArgs e)
        {
            int year = datepick.Date.Year;
            int month = datepick.Date.Month;
            int day = datepick.Date.Day;
            bool isTitleWritten = IsTitleDesWritten();
            bool isDateLegal = DateJudge(year, month, day);
            if (isTitleWritten == false)
            {
                var dialog = new MessageDialog("标题或者描述为空");
                await dialog.ShowAsync();
                return;
            }
            else if (isDateLegal == false)
            {
                var dialog = new MessageDialog("日期不合法");
                await dialog.ShowAsync();
                return;
            }
            else
            {
                Item newItem = new Item();
                newItem.date = datepick.Date;
                newItem.title = titleContent.Text;
                newItem.description = desContent.Text;
                newItem.image = img.Source;
                newItem.valid = true;
                newItem.scale = slider.Value;
                //CreateNewItem(newItem, curList.GetItemNum());
                curList.Add(newItem);
                clear();
                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame == null)
                    return;
                rootFrame.Navigate(typeof(MainPage), curList);
            }
        }
        private void App_BackRequested(object sender,
   Windows.UI.Core.BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;

            // Navigate back if possible, and if the event has not 
            // already been handled .
            rootFrame.Navigate(typeof(MainPage), curList);
        }

        private void ImageSizeChange(object sender, RangeBaseValueChangedEventArgs e)
        {
            img.Width = 300 * (slider.Value / 100 * 0.1 + 1);
            img.Height = 200 * (slider.Value / 100 * 0.1 + 1);
        }
    }
    
}
