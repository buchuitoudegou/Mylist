﻿#pragma checksum "C:\Users\75654\Desktop\Mylist\Mylist\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "46DD77898D9B768CF973D897B7182774"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mylist
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    global::Windows.UI.Xaml.Controls.Grid element1 = (global::Windows.UI.Xaml.Controls.Grid)(target);
                    #line 10 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Grid)element1).SizeChanged += this.Grid_SizeChanged;
                    #line default
                }
                break;
            case 2:
                {
                    this.list = (global::Windows.UI.Xaml.Controls.ScrollViewer)(target);
                }
                break;
            case 3:
                {
                    this.MessageBox = (global::Windows.UI.Xaml.Controls.ScrollViewer)(target);
                }
                break;
            case 4:
                {
                    this.img = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 5:
                {
                    this.slider = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    #line 22 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Slider)this.slider).ValueChanged += this.ImageSizeChange;
                    #line default
                }
                break;
            case 6:
                {
                    global::Windows.UI.Xaml.Controls.Button element6 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 23 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element6).Click += this.uploadPicture;
                    #line default
                }
                break;
            case 7:
                {
                    this.datepick = (global::Windows.UI.Xaml.Controls.DatePicker)(target);
                }
                break;
            case 8:
                {
                    this.create = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 34 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.create).Click += this.Add;
                    #line default
                }
                break;
            case 9:
                {
                    this.cancel = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 35 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.cancel).Click += this.clearAll;
                    #line default
                }
                break;
            case 10:
                {
                    this.des = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 11:
                {
                    this.desContent = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 12:
                {
                    this.title = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 13:
                {
                    this.titleContent = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 14:
                {
                    this.container = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 15:
                {
                    this.btmCmdBar = (global::Windows.UI.Xaml.Controls.CommandBar)(target);
                }
                break;
            case 16:
                {
                    this.AddButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 56 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.AddButton).Click += this.PageJump;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
