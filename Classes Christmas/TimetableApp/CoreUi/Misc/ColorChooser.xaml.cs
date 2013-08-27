using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    /// <summary>
    /// Used to choose the color of the class.
    /// </summary>
    public sealed partial class ColorChooser : UserControl
    {
        /// <summary>
        /// This event is triggered when the user presses a rectangle other than the selected one.
        /// </summary>
        /// <param name="newColor">The new color selected.</param>
        public delegate void ColorChangedHandler(Color newColor);
        public event ColorChangedHandler ColorChanged;
        public ColorChooser()
        {
            this.InitializeComponent();
            foreach (Rectangle r in root.Children)
            {
                AnimationHelper.AddPointerAnimation(r);
                r.Tapped += (e, ev) =>
                {
                    foreach (Rectangle rr in root.Children)
                    {
                        AnimationHelper.SmoothMove(rr, 30, 0.5);
                    }
                    AnimationHelper.SmoothMove(e as FrameworkElement, 10, 0.5);
                    SelectedRectangle = e as Rectangle;
                    ColorChanged(SelectedColor);
                };
            }
        }
        /// <summary>
        /// Points to the rectangle currently selected by the user (or defaulted by the constructor)
        /// </summary>
        public Rectangle SelectedRectangle;

        /// <summary>
        /// Property for setting and returning the current color
        /// </summary>
        public Color SelectedColor
        {
            get { return (SelectedRectangle.Fill as SolidColorBrush).Color; }
            set
            {
                foreach (Rectangle r in root.Children)
                {
                    if ((r.Fill as SolidColorBrush).Color == value)
                    {
                        AnimationHelper.SmoothMove(r, 10, 0.5);
                    }
                    else
                    {
                        AnimationHelper.SmoothMove(r, 30, 0.5);
                    }
                }
            }
        }
    }
}
