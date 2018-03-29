using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Mylist.Item
{
    public class Item : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string title
        {
            get { return this.Title; }
            set { this.Title = value; INotifyPropertyChanged("title"); }
        }
        private string Title;
        public string description
        {
            get { return this.Description; }
            set { this.Description = value; INotifyPropertyChanged("description"); }
        }
        private string Description;
        public string id;
        public DateTimeOffset date
        {
            get { return this.Date; }
            set { this.Date = value; INotifyPropertyChanged("date"); }
        }
        private DateTimeOffset Date;
        public ImageSource imgSrc
        {
            get { return this.ImgSrc; }
            set { this.ImgSrc = value; INotifyPropertyChanged("imgSrc"); }
        }
        private ImageSource ImgSrc;
        public Visibility linevisible { get { return this.lineVisible; }
            set { this.lineVisible = value; INotifyPropertyChanged("linevisible"); } }
        private Visibility lineVisible;
        private bool? Ischecked;
        public bool? ischecked
        {
            get { return this.Ischecked; }
            set { this.Ischecked = value;INotifyPropertyChanged("ischecked"); }
        }
        public Item(string t, string des, DateTimeOffset date, ImageSource i)
        {
            this.title = t;
            this.description = des;
            this.Date = date;
            this.ImgSrc = i;
            this.id = Guid.NewGuid().ToString();
            this.lineVisible = Visibility.Collapsed;
            this.Ischecked = false;
        }
        public void INotifyPropertyChanged(string propertyName)
        {
            //Debug.WriteLine(propertyName);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

namespace Mylist.ViewModel
{
    public class ViewModel
    {
        private ObservableCollection<Mylist.Item.Item> allitems = new ObservableCollection<Mylist.Item.Item>();
        public ObservableCollection<Mylist.Item.Item> AllItems { get { return this.allitems; } }
        public Item.Item NowPick;
        public void AddNewItem(string title, string des, DateTimeOffset date, ImageSource i)
        {
            Mylist.Item.Item newItem = new Mylist.Item.Item(title, des, date, i);
            allitems.Add(newItem);
            NowPick = null;
        }
        public void SetLineVisibility(string id)
        {
            foreach (var i in allitems)
            {
                if (i.id == id)
                {
                    if (i.linevisible == Visibility.Visible)
                    {
                        i.linevisible = Visibility.Collapsed;
                        i.ischecked = false;
                    }

                    else
                    {
                        i.linevisible = Visibility.Visible;
                        i.ischecked = true;
                    }
                }
            }
        }
        public void DeleteItem(Item.Item item)
        {
            allitems.Remove(item);
        }
        public void Update(Item.Item item)
        {
            foreach(var i in allitems)
            {
                if (i.id == item.id)
                {
                    i.title = item.title;
                    i.description = item.description;
                    i.date = item.date;
                    i.imgSrc = item.imgSrc;
                }
                    
            }
        }
        public Item.Item FindEleById(string id)
        {
            foreach(var i in allitems)
            {
                if (i.id == id)
                    return i;
            }
            return null;
        }
        public void DeleteItems()
        {
            foreach(var i in allitems.ToList())
            {
                if (i == NowPick)
                    continue;
                else if (i.ischecked == true)
                    allitems.Remove(i);
            }
        }
    }
}

namespace Mylist
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    Mylist.ViewModel.ViewModel viewModel = new ViewModel.ViewModel();
                    rootFrame.Navigate(typeof(MainPage), viewModel);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
    }
}
