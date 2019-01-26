﻿using System.Windows.Controls;

namespace Xvpn.UI.LocationModule
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class LocationView : UserControl
    {
        public LocationView(ILocationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}