﻿#pragma checksum "C:\Users\75654\Desktop\Mylist\Mylist\NewPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A873098D36954E799D00028A2819B514"
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
    partial class NewPage : 
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
                    this.MessageBox = (global::Windows.UI.Xaml.Controls.ScrollViewer)(target);
                }
                break;
            case 2:
                {
                    this.img = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 3:
                {
                    this.slider = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    #line 21 "..\..\..\NewPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Slider)this.slider).ValueChanged += this.ImageSizeChange;
                    #line default
                }
                break;
            case 4:
                {
                    global::Windows.UI.Xaml.Controls.Button element4 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 22 "..\..\..\NewPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element4).Click += this.uploadPicture;
                    #line default
                }
                break;
            case 5:
                {
                    this.datepick = (global::Windows.UI.Xaml.Controls.DatePicker)(target);
                }
                break;
            case 6:
                {
                    this.create = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 33 "..\..\..\NewPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.create).Click += this.Add;
                    #line default
                }
                break;
            case 7:
                {
                    this.cancel = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 34 "..\..\..\NewPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.cancel).Click += this.clearAll;
                    #line default
                }
                break;
            case 8:
                {
                    this.des = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 9:
                {
                    this.desContent = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 10:
                {
                    this.title = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 11:
                {
                    this.titleContent = (global::Windows.UI.Xaml.Controls.TextBox)(target);
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

