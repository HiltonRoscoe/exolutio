﻿#pragma checksum "D:\Programování\EvoX\View\Commands\Project\ServerProjectsWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "43747CDC4F2B6FB49D9FFE9317142F09"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SilverFlow.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace EvoX.View.Commands.Project {
    
    
    public partial class ServerProjectsWindow : SilverFlow.Controls.FloatingWindow {
        
        internal System.Windows.Controls.Button bOK;
        
        internal System.Windows.Controls.Button bCancel;
        
        internal System.Windows.Controls.ListBox lbProjects;
        
        internal System.Windows.Controls.Label lTitle;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/EvoX.View;component/Commands/Project/ServerProjectsWindow.xaml", System.UriKind.Relative));
            this.bOK = ((System.Windows.Controls.Button)(this.FindName("bOK")));
            this.bCancel = ((System.Windows.Controls.Button)(this.FindName("bCancel")));
            this.lbProjects = ((System.Windows.Controls.ListBox)(this.FindName("lbProjects")));
            this.lTitle = ((System.Windows.Controls.Label)(this.FindName("lTitle")));
        }
    }
}

