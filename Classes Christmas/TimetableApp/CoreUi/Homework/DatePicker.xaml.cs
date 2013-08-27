using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class DatePicker : UserControl
    {
        public DatePicker()
        {
            this.InitializeComponent();
        }

        private bool _showFutureOnly;
        public bool ShowFutureOnly
        {
            get { return _showFutureOnly; }
            set { 
                _showFutureOnly = value;
                Draw();
            }
        }

        public void Draw()
        {
            //foreach
        }

    }
}
