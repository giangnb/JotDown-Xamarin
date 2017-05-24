﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JotDown
{
    //[XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class Main : TabbedPage
    {
        public Main()
        {
            InitializeComponent();
            if (Device.OS == TargetPlatform.Android)
            {
                Title = "JotDown";
            }
        }
    }
}