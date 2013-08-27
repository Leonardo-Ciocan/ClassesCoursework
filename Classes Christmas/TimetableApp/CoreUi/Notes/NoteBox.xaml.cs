using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class NoteBox : EditableItem
    {
        Note self;
        Color color;
        public NoteBox(Note note , Color col)
        {
            
            this.InitializeComponent();
            color = col;
            header.Background = new SolidColorBrush(col);
            add.Foreground = new SolidColorBrush(Colors.White);
            edit.Foreground = new SolidColorBrush(Colors.White);
            self = note;

            this.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
            edit.Tapped += (a, b) =>
            {
                if ((edit.Foreground as SolidColorBrush).Color == Windows.UI.Colors.Black)
                {
                    foreach (EditableItem box in holder.Children) box.setEditable(false);
                    edit.Foreground = new SolidColorBrush(col);
                }
                else
                {
                    foreach (EditableItem box in holder.Children) box.setEditable(true);
                    edit.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                }
                //this.Height = holder.ActualHeight + 68;
            };

            Title.Text = self.Name;
            TitleEdit.Text = self.Name;

            TitleEdit.TextChanged += (a, b) =>
                                         {
                                             Title.Text = TitleEdit.Text;
                                             self.Name = TitleEdit.Text;
                                         };

            add.Tapped += (a, b) =>
            {
                NoteFragment frag = new NoteFragment { Content = "Turn on editing to add text" };
                self.Fragments.Add(frag);
                refresh();
            };

            this.Loaded += (c, d) =>
            {
                refresh();
                //holder.SizeChanged += (a, b) => this.Height = holder.ActualHeight + 68;
            };

            
        }

        void refresh()
        {
            holder.Children.Clear();
            bool shade = true;
            foreach (NoteFragment fragment in self.Fragments)
            {
                NoteFragmentControl cont = new NoteFragmentControl(fragment, holder.Children, self , color,shade);
                holder.Children.Add(cont);
                shade = !shade;
            }
            //this.Height = holder.ActualHeight + 68;
        }
    }
}
