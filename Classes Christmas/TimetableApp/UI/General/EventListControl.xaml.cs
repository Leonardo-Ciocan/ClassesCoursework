using System;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class EventListControl : UserControl
    {
        public delegate void SelectionChangedHandler(ref Course model , bool no_selection);
        public event SelectionChangedHandler SelectionChanged;

        public EventListControl()
        {
            this.InitializeComponent();
        }

        StudentDataBundle currentTarget;
        public void FillWith(ref StudentDataBundle tb)
        {
            root.Children.Clear();
            currentTarget = tb;
            for(int x =0; x< tb.Courses.Count;x++)
            {
                Course temp = tb.Courses[x];
                addEvent(temp);
            }
        }

        public EventListItem Selected;
        public void addEvent(Course model)
        {
            EventListItem item = new EventListItem(ref model);
            root.Children.Add(item);
            item.Tapped += (e, ev) =>
            {
                Selected = e as EventListItem;
                //indicator.Background = new SolidColorBrush((e as EventListItem).self.TileColor);
                bool selected = (e as EventListItem).isSelected;
                SelectionChanged(ref (e as EventListItem).self , selected);
                foreach (EventListItem lsi in root.Children)
                {
                    lsi.SetState(visible: false);
                }

                if (!selected)
                {
                    (e as EventListItem).SetState(visible: true);
                }
                deleteButton.IsEnabled = !selected;
            };
            AnimationHelper.AddPointerAnimation(item);
        }

        private async void delete_tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            PopupMenu menu = new PopupMenu();
            menu.Commands.Add(new UICommand("I am sure I want to delete this", (command) => {
                root.Children.Remove(Selected);
                currentTarget.Courses.Remove(Selected.self);
                SelectionChanged(ref Selected.self ,no_selection:true);
            }));
            Point pt = (sender as FrameworkElement).TransformToVisual(null).TransformPoint(new Point());
            await menu.ShowForSelectionAsync(new Rect(pt, new Size((sender as FrameworkElement).ActualWidth, (sender as FrameworkElement).ActualHeight)));
        }


        private void add_clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Course model = new Course();
            model.Name = "New Class";
            currentTarget.Courses.Add(model);
            FillWith(ref currentTarget);
        }

        public UIElementCollection Children
        {
            get { return root.Children; }
        }
    }
}
