﻿

#pragma checksum "D:\tfs\TFS\Interfaces\Interfaces\EventConnection\EventConnection\EventConnection.WindowsPhone\Controls\MenuControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "632EA5B20F2077EA30076180ECA54ECD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EventConnection.Controls
{
    partial class MenuControl : global::Windows.UI.Xaml.Controls.UserControl, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 21 "..\..\Controls\MenuControl.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ToList_OnClick;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 35 "..\..\Controls\MenuControl.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ToChat_OnClick;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 47 "..\..\Controls\MenuControl.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ToReview_OnClick;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


