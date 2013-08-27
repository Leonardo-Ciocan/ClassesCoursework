using System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class SimpleEventItem : UserControl
    {
        public Course self;
        public SimpleEventItem(Course model)
        {
            this.InitializeComponent();
            self = model;
            root.SizeChanged += (e, ev) =>
            {
                this.Height = root.ActualHeight+20;
            };
            this.Loaded += (e, ev) =>
            {
                _name.Text = self.Name;
                _description.Text = self.Notes;
                if(String.IsNullOrWhiteSpace(_description.Text))
                    _description.Text = "";
            };
            AnimationHelper.AddPointerAnimation(this);
            visualRoot.Background = new SolidColorBrush(Color.FromArgb(0,0,0,0));
            _name.Foreground = new SolidColorBrush(self.TileColor);
            _description.Foreground = new SolidColorBrush(self.TileColor);
            selectedColor = self.TileColor;
        }

        public Color selectedColor = Colors.LightCyan;
        public bool Selected
        {
            get { return ((visualRoot.Background as SolidColorBrush).Color.A == 0) ? false : true; }
            set
            {
                visualRoot.Background = new SolidColorBrush((value) ? selectedColor : Color.FromArgb(0, 0, 0, 0));
                _name.Foreground = new SolidColorBrush((value) ? Colors.White : selectedColor);
                _description.Foreground = new SolidColorBrush((value) ? Colors.White : selectedColor);
            }
        }
    }
}
