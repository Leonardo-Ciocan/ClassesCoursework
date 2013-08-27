using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class StructurePreview : UserControl
    {
        public delegate void OnOpenHandler(Course model);
        public event OnOpenHandler OnOpen;
        public StructurePreview(Course model)
        {
            this.InitializeComponent();
            this.Loaded += (a, b) => { if (model.Modules.Count == 0) (this.Parent as StackPanel).Children.Remove(this); };

            name.Text = model.Name;
            edit.Tapped += (a, b) => OnOpen(model);
            root.Background = new SolidColorBrush(model.TileColor);
            int total =0 , s = 0, r = 0; //s is the number studied and r is the number revised
            //iterate through every topic and increment accordingly
            foreach (Module mod in model.Modules)
            {
                foreach (StructureBlock block in mod.StructureBlocks)
                {
                    foreach (Topic tp in block.Topics)
                    {
                        total++;
                        if (tp.Studied) s++;
                        if (tp.Revised) r++;
                    }
                }
            }
            this.Loaded += (a, b) =>
            {
                topics.Text = total.ToString() + " topics";
                //if studied >0
                if (s != 0)
                {
                    studied.Text = (s * 100 / total).ToString() + "%";
                    Color c = model.TileColor;
                    c.R = (byte)(c.R * 0.8);//dime the color a bit for nice contrast
                    c.B = (byte)(c.B * 0.8);
                    c.G = (byte)(c.G * 0.8);
                    studied.Foreground = new SolidColorBrush(c);
                    //draw the arc to symbolize the percentage studied , the angle is the same as the ratio of studied to total
                    studiedPie.DrawWithAngle((double)s / total * 360 , model.TileColor);

                }
                if (r != 0)
                {
                    revised.Foreground = new SolidColorBrush(model.TileColor);
                    revised.Text = (r * 100 / total).ToString() + "%";
                    studiedPie.DrawInnerWithAngle(((double)r / total) * 360 , model.TileColor);
                }


            };
        }
    }
}
