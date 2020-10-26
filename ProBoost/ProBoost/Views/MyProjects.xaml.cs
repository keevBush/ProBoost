﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProBoost.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyProjects : ContentPage
    {
        public MyProjects()
        {
            InitializeComponent();
        }

        public MyProjects(string key)
        {
            InitializeComponent();
            BindingContext = new ViewModels.MyProjectsViewModel(key);
        }
    }
}