﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Exolutio.SupportingClasses;

namespace Exolutio.WPFClient
{
    /// <summary>
    /// Interaction logic for ExceptionWindow.xaml
    /// </summary>
    public partial class ExceptionWindow : Window
    {
        private Exception exception; 

        public ExceptionWindow()
        {
            InitializeComponent();
        }

        public ExceptionWindow(Exception exception)
            : this()
        {
            this.exception = exception;

            ExolutioException xe = exception as ExolutioException;
			
            tbExMsg.Content = exception.Message;
			if (xe != null)
			{
				tbExInner.Content = String.Empty;
				expander1.Visibility = Visibility.Collapsed;
                if (xe.ExceptionTitle != null)
                {
                    textBlock1.Content = xe.ExceptionTitle;
                }
                if (!string.IsNullOrEmpty(xe.ExceptionTitle))
                {
                    this.Title = xe.ExceptionTitle;
                }
				button1.Content = "Ok";
				button2.Visibility = Visibility.Collapsed;
                if (xe is Exolutio.Controller.Commands.ExolutioCommandException)
                {
                    if (string.IsNullOrEmpty(xe.ExceptionTitle))
                    {
                        textBlock1.Content = "Operation can not continue.";
                    }
                    Exolutio.Controller.Commands.ExolutioCommandException xec = ((Exolutio.Controller.Commands.ExolutioCommandException)xe);
                    if (xec.Command != null && xec.Command.ErrorDescription != null)
                    {
                        tbExInner.Content = xec.Command.ErrorDescription;
                    }
                }

                if (exception.InnerException != null)
                {
                    tbExInner.Content = "Inner exception: " + exception.InnerException.Message;
                    expander1.Visibility = Visibility.Visible;
                }
			}
			else
			{
				if (exception.InnerException != null)
				{
					tbExInner.Content = "Inner exception: " + exception.InnerException.Message;
				}
				else
				{
					tbExInner.Content = String.Empty;
				}
			}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void expander1_Expanded(object sender, RoutedEventArgs e)
        {
            if (expander1.IsExpanded)
            {
                tbExStack.Content = exception.StackTrace;
            }
            else
            {
                tbExStack.Content = String.Empty;
            }
        }
    }
}
