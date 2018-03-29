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
        //private ItemList curList;
        private FileOpenPicker openPicker;
        public Mylist.ViewModel.ViewModel ItemViewModel;
        public NewPage()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
            AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            this.InitializeComponent();
            openPicker = new FileOpenPicker();
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            ItemViewModel = new ViewModel.ViewModel();
        }
        private void App_BackRequested(object sender,Windows.UI.Core.BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            ItemViewModel.NowPick = null;
            if (rootFrame == null)
                return;

            // Navigate back if possible, and if the event has not 
            // already been handled .
            rootFrame.Navigate(typeof(MainPage), ItemViewModel);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ItemViewModel = (ViewModel.ViewModel)e.Parameter;
            if(ItemViewModel.NowPick != null)
            {
                titleContent.Text = ItemViewModel.NowPick.title;
                desContent.Text = ItemViewModel.NowPick.description;
                img.Source = ItemViewModel.NowPick.imgSrc;
                datepick.Date = ItemViewModel.NowPick.date;
                create.Content = "delete";
                create.Click -= Add;
                create.Click += deleteItem;
                cancel.Content = "update";
                cancel.Click -= clearAll;
                cancel.Click += updateItem;
            }
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
            if (ItemViewModel.NowPick != null)
                return;
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
                ItemViewModel.AddNewItem(titleContent.Text, desContent.Text, datepick.Date, img.Source);
                clear();
                var dialog = new MessageDialog("创建成功");
                await dialog.ShowAsync();
                Frame root = Window.Current.Content as Frame;
                root.Navigate(typeof(MainPage), ItemViewModel);
            }
        }
        private void ImageSizeChange(object sender, RangeBaseValueChangedEventArgs e)
        {
            img.Width = 300 * (slider.Value / 100 * 0.1 + 1);
            img.Height = 200 * (slider.Value / 100 * 0.1 + 1);
        }
        private void clearAll(object sender, RoutedEventArgs e)
        {
            if (ItemViewModel.NowPick != null)
                return;
            clear();
        }
        private void clear()
        {
            desContent.Text = "";
            titleContent.Text = "";
            datepick.Date = DateTime.Now;
            BitmapImage bitmapImage = new BitmapImage();
            slider.Value = 0;
            bitmapImage.UriSource = new Uri(img.BaseUri, "/Assets/StoreLogo.png");
            img.Source = bitmapImage;
        }
        private void deleteItem(object sender, RoutedEventArgs e)
        {
            if (ItemViewModel.NowPick == null)
                return;
            ItemViewModel.DeleteItem(ItemViewModel.NowPick);
            clear();
            Frame root = Window.Current.Content as Frame;
            root.Navigate(typeof(MainPage), ItemViewModel);
        }
        private async void updateItem(object sender, RoutedEventArgs e)
        {
            if (ItemViewModel.NowPick == null)
                return;
            ItemViewModel.NowPick.title = titleContent.Text;
            ItemViewModel.NowPick.description = desContent.Text;
            ItemViewModel.NowPick.imgSrc = img.Source;
            ItemViewModel.NowPick.date = datepick.Date;
            bool TitleDesFlag = IsTitleDesWritten();
            bool DateFlag = DateJudge(datepick.Date.Year, datepick.Date.Month, datepick.Date.Day);
            if(TitleDesFlag == false)
            {
                var dialog = new MessageDialog("标题或者描述为空");
                await dialog.ShowAsync();
                return;
            }
            else if(DateFlag == false)
            {
                var dialog = new MessageDialog("日期不合法");
                await dialog.ShowAsync();
                return;
            }
            ItemViewModel.Update(ItemViewModel.NowPick);
            clear();
            ItemViewModel.NowPick = null;
            Frame root = Window.Current.Content as Frame;
            root.Navigate(typeof(MainPage), ItemViewModel);
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
    }
    
}
