using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SloperMobile.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NetworkErrorPage : ContentPage
    {
        public NetworkErrorPage()
        {
            InitializeComponent();
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                //DisplayAlert("Connectivity Changed", "IsConnected: " + args.IsConnected.ToString(), "OK");
            };

        }
    }
}
