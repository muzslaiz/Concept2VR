﻿#pragma checksum "..\..\Rowing.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "AFF6E46DF791C6A0C661C90F2EEC1960A3EEB1AF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using C2Test2;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace C2Test2 {
    
    
    /// <summary>
    /// Rowing
    /// </summary>
    public partial class Rowing : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\Rowing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MediaElement VideoControl;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\Rowing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbl_speedrate;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\Rowing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbl_speed;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\Rowing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label wdistance;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\Rowing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_speedUp;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\Rowing.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_progress;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/C2Test2;component/rowing.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Rowing.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.VideoControl = ((System.Windows.Controls.MediaElement)(target));
            return;
            case 2:
            this.lbl_speedrate = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.lbl_speed = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.wdistance = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.btn_speedUp = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\Rowing.xaml"
            this.btn_speedUp.Click += new System.Windows.RoutedEventHandler(this.btn_speedUp_Click_1);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btn_progress = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\Rowing.xaml"
            this.btn_progress.Click += new System.Windows.RoutedEventHandler(this.btn_progress_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

