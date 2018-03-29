using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Mylist
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
       private FileOpenPicker openPicker;
        /*
        private ItemList curList;
        private Line[] arrline;
        private CheckBox[] arrcheckbox;
        private Image[] arrimage;
        private TextBlock[] arrTextBlock;
        private Canvas[] arrcanvas;*/
        public Mylist.ViewModel.ViewModel ItemViewModel;
        public ObservableCollection<Item.Item> set = new ObservableCollection<Item.Item>();
        public MainPage()
        {
            openPicker = new FileOpenPicker();
            ItemViewModel = new Mylist.ViewModel.ViewModel();
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
            AppViewBackButtonVisibility.Collapsed;
            ApplicationView.GetForCurrentView().TryResizeView(new Size(1100, 600));
            this.InitializeComponent();
            create.Click += deleteItem;
            cancel.Click += updateItem;
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
            if(isTitleWritten == false)
            {
                var dialog = new MessageDialog("标题或者描述为空");
                await dialog.ShowAsync();
                return;
            }
            else if(isDateLegal == false)
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
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ItemViewModel = (ViewModel.ViewModel)e.Parameter;
        }
        private void PageJump(object sender, RoutedEventArgs e)
        {
            
            double curWidth = Window.Current.Bounds.Width;
            list.SelectedItem = null;
            ItemViewModel.NowPick = null;
            list.SelectedItem = null;
            create.Content = "create";
            cancel.Content = "cancel";
            clear();
            if (curWidth > 800.0)
            {
                return;
            }
            Frame root = Window.Current.Content as Frame;
            root.Navigate(typeof(NewPage), ItemViewModel);
        }
        private void ImageSizeChange(object sender, RangeBaseValueChangedEventArgs e)
        {
            img.Width = 300 * (slider.Value / 100 * 0.1 + 1);
            img.Height = 200 * (slider.Value / 100 * 0.1 + 1);
        }
        private void LineVisibility(object sender, RoutedEventArgs e)
        {
            var i = (CheckBox)sender;
            ItemViewModel.SetLineVisibility(i.Name);
        }

        private void ViewDetail(object sender, SelectionChangedEventArgs e)
        {
            double curWidth = Window.Current.Bounds.Width;
            var i = (ListView)sender;
            var j = (Item.Item)i.SelectedItem;
            ItemViewModel.NowPick = j;
            if (list.SelectedItem == null)
                return;
            if (curWidth < 800)
            {
                Frame root = Window.Current.Content as Frame;
                root.Navigate(typeof(NewPage), ItemViewModel);
            }
            
            clear();

            titleContent.Text = j.title;
            desContent.Text = j.description;
            img.Source = j.imgSrc;
            datepick.Date = j.date;
            create.Content = "delete";
            cancel.Content = "update";
        }
        private void deleteItem(object sender, RoutedEventArgs e)
        {
            if (ItemViewModel.NowPick == null)
                return;
            ItemViewModel.DeleteItem(ItemViewModel.NowPick);
            clear();
            create.Content = "create";
            cancel.Content = "cancel";
            ItemViewModel.NowPick = null;
            list.SelectedItem = null;
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
            if (TitleDesFlag == false)
            {
                var dialog = new MessageDialog("标题或者描述为空");
                await dialog.ShowAsync();
                return;
            }
            else if (DateFlag == false)
            {
                var dialog = new MessageDialog("日期不合法");
                await dialog.ShowAsync();
                return;
            }
            ItemViewModel.Update(ItemViewModel.NowPick);
            clear();
            list.SelectedItem = null;
            create.Content = "create";
            cancel.Content = "cancel";
        }
        private void PickNDel(object sender, RoutedEventArgs e)
        {
            var i = (MenuFlyoutItem)sender;
            ItemViewModel.NowPick = ItemViewModel.FindEleById(i.DataContext.ToString());
            deleteItem(sender, e);
        }
        private void Pick(object sender, RoutedEventArgs e)
        {
            var i = (MenuFlyoutItem)sender;
            double curWidth = Window.Current.Bounds.Width;
            ItemViewModel.NowPick = ItemViewModel.FindEleById(i.DataContext.ToString());
            if (curWidth < 800)
            {
                Frame root = Window.Current.Content as Frame;
                root.Navigate(typeof(NewPage), ItemViewModel);
            }

            clear();

            titleContent.Text = ItemViewModel.NowPick.title;
            desContent.Text = ItemViewModel.NowPick.description;
            img.Source = ItemViewModel.NowPick.imgSrc;
            datepick.Date = ItemViewModel.NowPick.date;
            create.Content = "delete";
            cancel.Content = "update";
        }
        private void removeall(object sender, RoutedEventArgs e)
        {
            ItemViewModel.DeleteItems();
            if (ItemViewModel.NowPick != null &&ItemViewModel.NowPick.ischecked == true)
                deleteItem(sender, e);
        }
    }
}
