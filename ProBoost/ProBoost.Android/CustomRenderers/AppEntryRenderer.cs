using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ProBoost.CustomControl;
using ProBoost.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AppEntry),typeof(AppEntryRenderer))]
namespace ProBoost.Droid.CustomRenderers
{
    public class AppEntryRenderer:EntryRenderer
    {
        public AppEntryRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null) return;
            Control.Background = new  ColorDrawable(Android.Graphics.Color.Transparent);
        }
    }
}