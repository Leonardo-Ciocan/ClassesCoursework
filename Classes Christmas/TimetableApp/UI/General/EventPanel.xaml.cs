using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TimetableApp
{
    public sealed partial class EventPanel : UserControl
    {

        EventControl cont = new EventControl();
        public TimetableView Preview;
        public EventPanel()
        {
            this.InitializeComponent();
            list.SelectionChanged += list_SelectionChanged;

            _class.TextChanged += textChangedHandler;
            _teacher.TextChanged += textChangedHandler;
            _title.TextChanged += textChangedHandler;
            _notes.TextChanged += textChangedHandler;
            preview.Children.Add(cont);
           
            noSelectionGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
            SelectionGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            colorChooser.ColorChanged += (col) =>
            {
                temp.TileColor = col;
                cont.FillWith(temp);
                ShouldSave = true;
            };
        }

        
        void textChangedHandler(object sender, TextChangedEventArgs e)
        {
            temp.Class = _class.Text;
            temp.Teacher = _teacher.Text;
            temp.Name = _title.Text;
            temp.Notes = _notes.Text;
            cont.FillWith(temp);
            ShouldSave = true;
            //if(sender == 
        }

        
        Course temp = new Course();
        Course actual;
        void list_SelectionChanged(ref Course model , bool no_selection)
        {
            if (!no_selection)
            {
                noSelectionGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                SelectionGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                actual = model;
                temp.Clone(model);
                clear();
                _class.Text = model.Class;
                _teacher.Text = model.Teacher;
                _notes.Text = model.Notes;
                _title.Text = model.Name;
                colorChooser.SelectedColor = model.TileColor;
                for (int x = 0; x < temp.Instances.Count; x++)
                {
                    Instance target = temp.Instances[x];
                    InstanceControl cc = new InstanceControl(target,self);
                    holder.Children.Add(cc);
                    cc.InstanceChanged += () =>
                    {
                        ShouldSave = true;
                    };
                    cc.OnDelete += (sender,instance) =>
                    {
                        temp.Instances.Remove(instance);
                        holder.Children.Remove(sender);
                        ShouldSave = true;
                    };
                }
            }
            else
            {
                SelectionGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                noSelectionGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            ShouldSave = false;
        }

        void clear()
        {
            _class.Text = "";
            _teacher.Text = "";
            _title.Text = "";
            _notes.Text = "";
            holder.Children.Clear();
            ShouldSave = true;
        }

        public delegate void BackPressedHandler(object sender);
        public event BackPressedHandler BackPressed;
        private void back_btn_pressed(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            BackPressed(this);
        }

        public StudentDataBundle self;
        public void FillWith(ref StudentDataBundle studentDataBundle)
        {
            list.FillWith(ref studentDataBundle);
            self = studentDataBundle;
        }

        private void add_instance(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Instance instance = new Instance { Day = 0, StartTime = new Time { Hours = 9, Minutes = 0 }, EndTime = new Time { Hours = 10, Minutes = 0 } ,EventName=temp.Name};
            temp.Instances.Add(instance);
            var control = new InstanceControl(instance,self);
            holder.Children.Add(control);
            control.InstanceChanged += () =>
            {
                ShouldSave = true;
            };
            control.OnDelete += (instanceElement,ins) =>
            {
                temp.Instances.Remove(ins);
                holder.Children.Remove(instanceElement);
                ShouldSave = true;
            };
            
        }

        public void UpdateTimetable(){
            List<Course> models = new List<Course>();
            foreach (EventListItem m in list.Children)
            {
                models.Add(m.self);
            }
            models.Remove(actual);
            models.Add(temp);
            Preview.FillWith(new StudentDataBundle { Courses = models });
        }
        public void checkInstace()
        {

        }

        private bool shouldSave = false;
        private bool ShouldSave
        {
            get { return shouldSave; }
            set
            {
                shouldSave = value;
                saveButton.Background = new SolidColorBrush((shouldSave) ? Color.FromArgb(255, 0, 104, 255) : Color.FromArgb(178, 182, 182, 182));
                saveButton.Content = (shouldSave) ? "Save" : "Done";
                saveButton.IsEnabled = (shouldSave) ? true : false;
                foreach (InstanceControl instance in holder.Children)
                {
                    if (!instance.isValid)
                    {
                        saveButton.Background = new SolidColorBrush(Colors.DarkRed);
                        saveButton.Content = "Can't save";
                    }
                }
                UpdateTimetable();
            }
        }
        private void save_all_clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //shouldSave = !shouldSave;
            //saveButton.Background = new SolidColorBrush((shouldSave) ? Color.FromArgb(255, 0, 104, 255) : Color.FromArgb(178, 182, 182, 182));
            //saveButton.Content = (shouldSave) ? "Save" : "Done";

            if (((string)saveButton.Content) != "Can't save")
            {
                actual.Clone(temp);
                list.Selected.Update();
                ShouldSave = false;
            }
        }

    }
}
