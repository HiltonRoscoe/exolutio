﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Exolutio.ViewToolkit
{
    /// <summary>
    /// Interaction logic for ExolutioCanvasWithZoomer.xaml
    /// </summary>
    public partial class ExolutioCanvasWithZoomer : UserControl
    {
        public ExolutioCanvasWithZoomer()
        {
            InitializeComponent();

            zoomer.PropertyChanged+=Zoomer_PropertyChanged;
            scrollViewer.ScrollChanged += new ScrollChangedEventHandler(scrollViewer_ScrollChanged);
        }

        void scrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ExolutioCanvasWithZoomer_SizeChanged(null, null);
        }

        private void Zoomer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            scaleTransform.ScaleX = zoomer.ScaleX;
            scaleTransform.ScaleY = zoomer.ScaleY;
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseButtonEventArgs eventArgs = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton);
            eventArgs.RoutedEvent = ExolutioCanvas.MouseDownEvent;
            if (e.OriginalSource == scrollViewer)
            {
                eventArgs.Source = ExolutioCanvas;
            }
            else
            {
                eventArgs.Source = e.OriginalSource;
            }
            ExolutioCanvas.ExolutioCanvas_MouseDown(ExolutioCanvas, eventArgs);
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs eventArgs = new MouseEventArgs(e.MouseDevice, e.Timestamp);
            eventArgs.RoutedEvent = ExolutioCanvas.MouseMoveEvent;
            if (e.OriginalSource == scrollViewer)
            {
                eventArgs.Source = ExolutioCanvas;
            }
            else
            {
                eventArgs.Source = e.OriginalSource;
            }
            ExolutioCanvas.ExolutioCanvas_MouseMove(ExolutioCanvas, e);
            ShowHideZoomer(e);

        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseButtonEventArgs eventArgs = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton);
            eventArgs.RoutedEvent = ExolutioCanvas.MouseUpEvent;
            if (e.OriginalSource == scrollViewer)
            {
                eventArgs.Source = ExolutioCanvas;
            }
            else
            {
                eventArgs.Source = e.OriginalSource;
            }
            ExolutioCanvas.ExolutioCanvas_MouseUp(ExolutioCanvas, eventArgs);
        }

        private void ShowHideZoomer(MouseEventArgs e)
        {
            Point position = e.GetPosition(this);

            double comparedHeight = this.ActualHeight;

            if (zoomer.Visibility != System.Windows.Visibility.Visible
                && comparedHeight - position.Y < 50)
            {
                zoomer.Visibility = System.Windows.Visibility.Visible;
            }

            if (zoomer.Visibility == System.Windows.Visibility.Visible
                && comparedHeight - position.Y >= 50)
            {
                zoomer.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void ExolutioCanvasWithZoomer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            scrollViewer.Width = this.ActualWidth;
            scrollViewer.Height = this.ActualHeight;

            const int height = 0;
            const int scrollbarSize = 20;

            if (scrollViewer.ComputedHorizontalScrollBarVisibility == System.Windows.Visibility.Visible)
            {
                Canvas.SetBottom(zoomer, height + scrollbarSize);
            }
            else
            {
                Canvas.SetBottom(zoomer, height);
            }

            if (scrollViewer.ComputedVerticalScrollBarVisibility == System.Windows.Visibility.Visible)
            {
                zoomer.Width = Math.Max(this.ActualWidth - scrollbarSize, 0);
            }
            else
            {
                zoomer.Width = this.ActualWidth;
            }
        }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            
        }
    }
}
