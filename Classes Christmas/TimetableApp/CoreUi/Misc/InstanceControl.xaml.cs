using System;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TimetableApp
{
    public sealed partial class InstanceControl : UserControl
    {
        public bool isValid
        {
            set { error_flag.Visibility = (value) ? Visibility.Collapsed : Visibility.Visible; }
            get { return (error_flag.Visibility == Windows.UI.Xaml.Visibility.Collapsed) ? true : false; }
        }

        Instance self;
        StudentDataBundle selfParent;
        public InstanceControl(Instance instance , StudentDataBundle studentDataBundle)
        {
            this.InitializeComponent();
            this.Loaded += InstanceControl_Loaded;
            self = instance;
            selfParent = studentDataBundle;
        }

        public delegate void InstanceChangedHandler();
        public event InstanceChangedHandler InstanceChanged;
        async void changedEvent(object sender, SelectionChangedEventArgs e)
        {
            

            self.StartTime.Hours = int.Parse(((start1.SelectedItem as ComboBoxItem).Content as string));
            self.StartTime.Minutes = int.Parse(((start2.SelectedItem as ComboBoxItem).Content as string));

            self.EndTime.Hours = int.Parse(((end1.SelectedItem as ComboBoxItem).Content as string));
            self.EndTime.Minutes = int.Parse(((end2.SelectedItem as ComboBoxItem).Content as string));
            
            self.Day = day.SelectedIndex;
            try
            {
                InstanceChanged();
            }
            catch { }

            if (int.Parse((start1.SelectedValue as ComboBoxItem).Content as string) * 60 + int.Parse((start2.SelectedValue as ComboBoxItem).Content as string) >= int.Parse((end1.SelectedValue as ComboBoxItem).Content as string) * 60 + int.Parse((end2.SelectedValue as ComboBoxItem).Content as string))
            {
                MessageDialog error_dialog = new MessageDialog("The class start time must be before the end time", "We have a problem , Watson");
                error_dialog.Commands.Add(new UICommand("Okay"));
                await error_dialog.ShowAsync();
                error_flag.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                error_flag.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }

            TimeOverlapData result = CoreTime.isOverlapping(self, selfParent);
            if (result.isOverlapping)
            {
                MessageDialog error_dialog = new MessageDialog("Classes cannot overlap.Please choose another time.", "This class time overlaps with your " + result.OverlappingEventName + " class");
                error_dialog.Commands.Add(new UICommand("Okay"));
                await error_dialog.ShowAsync();
                isValid = false;
            }
            else
            {
                isValid = true;
            }
        }
        void InstanceControl_Loaded(object sender, RoutedEventArgs e)
        {
            start1.Items.Clear();
            start2.Items.Clear();
            end1.Items.Clear();
            end2.Items.Clear();
            for (int x = 1; x <= 24; x++)
            {
                start1.Items.Add(new ComboBoxItem { Content = x.ToString() });
                end1.Items.Add(new ComboBoxItem { Content = x.ToString() });
            }
            for (int x = 0; x <= 45; x += 15)
            {
                start2.Items.Add(new ComboBoxItem { Content = (x==0)? x.ToString() + "0" : x.ToString() });
                end2.Items.Add(new ComboBoxItem { Content = (x == 0) ? x.ToString() + "0" : x.ToString() });
            }
            start1.SelectedIndex = 7;
            end1.SelectedIndex = 8;

            start2.SelectedIndex = 0;
            end2.SelectedIndex = 0;

            day.SelectedIndex = self.Day;
            

            day.SelectedIndex = self.Day;
            start1.SelectedIndex = self.StartTime.Hours - 1;
            start2.SelectedIndex = self.StartTime.Minutes / 15;
            end1.SelectedIndex = self.EndTime.Hours - 1;
            end2.SelectedIndex = self.EndTime.Minutes / 15;

            start1.SelectionChanged += changedEvent;
            start2.SelectionChanged += changedEvent;
            end1.SelectionChanged += changedEvent;
            end2.SelectionChanged += changedEvent;
            day.SelectionChanged += changedEvent;
        }

        public delegate void DeletedHandler(InstanceControl control , Instance instance);
        public event DeletedHandler OnDelete;
        private void delete_clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            OnDelete(this,self);
        }
    }
}
