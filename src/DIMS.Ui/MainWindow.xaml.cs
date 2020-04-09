﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DIM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool MenuClosed = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (MenuClosed)
            {
                var openMenu = (Storyboard)Button.FindResource("OpenMenu");
                openMenu.Begin();
            }
            else
            {
                var closeMenu = (Storyboard)Button.FindResource("CloseMenu");
                closeMenu.Begin();
            }

            MenuClosed = !MenuClosed;
        }
    }
}
