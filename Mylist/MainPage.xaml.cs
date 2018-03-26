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
        private ItemList curList;
        private Line[] arrline;
        private CheckBox[] arrcheckbox;
        private Image[] arrimage;
        private TextBlock[] arrTextBlock;
        private Canvas[] arrcanvas;
        private int NowPick;
        public MainPage()
        {
            openPicker = new FileOpenPicker();
            curList = new ItemList();
            arrline = new Line[200];
            arrcheckbox = new CheckBox[200];
            arrimage = new Image[200];
            arrTextBlock = new TextBlock[200];
            arrcanvas = new Canvas[200];
            NowPick = -1;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
            AppViewBackButtonVisibility.Collapsed;
            ApplicationView.GetForCurrentView().TryResizeView(new Size(1100, 600));
            this.InitializeComponent();
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
                Item newItem = new Item();
                newItem.date = datepick.Date;
                newItem.title = titleContent.Text;
                newItem.description = desContent.Text;
                newItem.image = img.Source;
                newItem.valid = true;
                newItem.scale = slider.Value;
                CreateNewItem(newItem, curList.GetItemNum());
                curList.Add(newItem);
                clear();
                var dialog = new MessageDialog("创建成功");
                await dialog.ShowAsync();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            curList = (ItemList)e.Parameter;
            int num = curList.GetItemNum();
            for (int i = 0; i < num; ++i)
                CreateNewItem(curList.FindItemByIndex(i), i);
        }

        private void CreateNewItem(Item newItemMessage, int number)
        {
            if (newItemMessage.valid == false)
                return;
            var newCanvas = new Canvas();
            newCanvas.Height = 40;
            newCanvas.Name = "Canvas" + number.ToString();
            arrcanvas[number] = newCanvas;

            var newRelativePanel = new RelativePanel();

            var newCheckBox = new CheckBox();
            newCheckBox.Name = "CheckBox" + number.ToString();
            newCheckBox.Checked += LineShow;
            newCheckBox.Unchecked += LineHide;
            arrcheckbox[number] = newCheckBox;

            var newImage = new Image();
            newImage.Width = 50;
            newImage.Height = 40;
            newImage.Source = newItemMessage.image;
            newImage.Margin = new Thickness(30, 0, 0, 0);
            arrimage[number] = newImage;

            var newTextBlock = new TextBlock();
            newTextBlock.Text = newItemMessage.title;
            newTextBlock.Margin = new Thickness(90, 10, 0, 0);
            arrTextBlock[number] = newTextBlock;

            var newLine = new Line();
            newLine.X1 = 80;
            newLine.Y1 = 20;
            newLine.X2 = 500;
            newLine.Y2 = 20;
            newLine.Stroke = new SolidColorBrush(Windows.UI.Colors.Black);
            newLine.Visibility = Visibility.Collapsed;
            newLine.Name = "Line" + number.ToString();
            arrline[number] = newLine;

            container.Children.Add(newCanvas);
            newCanvas.Children.Add(newRelativePanel);
            newRelativePanel.Children.Add(newCheckBox);
            newRelativePanel.Children.Add(newImage);
            newRelativePanel.Children.Add(newTextBlock);
            newCanvas.Children.Add(newLine);
        }
        private void LineShow(object sender, RoutedEventArgs e)
        {
            if (NowPick != -1)
                LineHide(arrcheckbox[NowPick], e);
            CheckBox curcheckbox = (CheckBox)sender;
            int num = -1;
            int strlength = curcheckbox.Name.ToString().Length;
            int.TryParse(curcheckbox.Name.ToString().Substring(8, strlength - 8), out num);
            arrline[num].Visibility = Visibility.Visible;
            NowPick = num;
            LoadMessage(num);
            create.Visibility = Visibility.Collapsed;
            cancel.Content = "delete";
            cancel.Click += DeleteCurItem;
        }
        private async void DeleteCurItem(object sender, RoutedEventArgs e)
        {
            DeleteItem(NowPick);
            var dialog = new MessageDialog("删除成功");
            await dialog.ShowAsync();

        }
        private void LineHide(object sender, RoutedEventArgs e)
        {
            CheckBox curcheckbox = (CheckBox)sender;
            curcheckbox.IsChecked = false;
            int num = -1;
            int strlength = curcheckbox.Name.ToString().Length;
            int.TryParse(curcheckbox.Name.ToString().Substring(8, strlength - 8), out num);
            arrline[num].Visibility = Visibility.Collapsed;
            NowPick = -1;

            titleContent.Text = "";
            desContent.Text = "";
            BitmapImage DefaultImage = new BitmapImage();
            DefaultImage.UriSource = new Uri(img.BaseUri, "/Assets/StoreLogo.png");
            img.Source = DefaultImage;
            datepick.Date = DateTime.Now;
            create.Visibility = Visibility.Visible;
            img.Width = 300;
            img.Height = 200;
            slider.Value = 0;
            cancel.Content = "cancel";
            cancel.Click -= DeleteCurItem;
        }
        private void DeleteItem(int index)
        {
            arrcanvas[index].Visibility = Visibility.Collapsed;
            curList.DeleteItem(index);
            cancel.Content = "cancel";
            create.Visibility = Visibility.Visible;
        }
        private void LoadMessage(int number)
        {
            Item curItem = curList.FindItemByIndex(number);
            titleContent.Text = curItem.title;
            desContent.Text = curItem.description;
            img.Source = curItem.image;
            datepick.Date = curItem.date;
            img.Width = 300 * (curItem.scale / 100 * 0.1 + 1);
            img.Height = 200 * (curItem.scale / 100 * 0.1 + 1);
            slider.Value = curItem.scale;
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double curWidth = Window.Current.Bounds.Width;
            int itemNum = curList.GetItemNum();
            if(curWidth < 600.0)
            {
                for (int i = 0; i < itemNum; ++i)
                {
                    try
                    {
                        arrimage[i].Visibility = Visibility.Collapsed;
                        arrTextBlock[i].Margin = new Thickness(30, 10, 0, 0);
                        arrline[i].X1 = 30;
                    }
                    catch(Exception j)
                    {
                        var str = j.ToString();
                        str = str + "1";
                    }
                }

            }
            else
            {
                for (int i = 0; i < itemNum; ++i)
                {
                    try
                    {
                        arrimage[i].Visibility = Visibility.Visible;
                        arrTextBlock[i].Margin = new Thickness(90, 10, 0, 0);
                        arrline[i].X1 = 80;
                    }
                    catch(Exception j)
                    {
                        var str = j.ToString();
                        str += "1";
                    }
                }
            }
        }

        private void PageJump(object sender, RoutedEventArgs e)
        {
            double curWidth = Window.Current.Bounds.Width;
            if (curWidth > 800.0)
                return;
            Frame root = Window.Current.Content as Frame;
            root.Navigate(typeof(NewPage), curList);
        }
        private void ImageSizeChange(object sender, RangeBaseValueChangedEventArgs e)
        {
            img.Width = 300 * (slider.Value / 100 * 0.1 + 1);
            img.Height = 200 * (slider.Value / 100 * 0.1 + 1);
        }
    }
}
