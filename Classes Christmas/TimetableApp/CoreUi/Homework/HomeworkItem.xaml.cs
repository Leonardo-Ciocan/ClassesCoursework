using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class HomeworkItem : UserControl
    {
        public HomeworkTask self;
        public HomeworkItem(HomeworkTask task , Windows.UI.Color c, UIElementCollection container = null, Module module = null)
        {
            this.InitializeComponent();
            self = task;

            //initial values
            root.Background = new SolidColorBrush(c);
            _description.Text = self.Notes;
            completedSwitch.IsOn = self.Completed;
            dateButton.Content = CoreTime.FormatTime(task.DueOn);



            deleteButton.Tapped += (e, ev) =>
            {
                //OnDelete(this);
                container.Remove(this);
                module.Homeworks.Remove(self);
            };
            completedSwitch.Toggled += (e, ev) =>
            {
                self.Completed = completedSwitch.IsOn;
               // OnChange();
            };
            _description.TextChanged += (e, ev) =>
            {
                Validator.isStringValid(
                    target: _description.Text, scope: "description", allowDigits: true, allowEmpty: true, max: Validator.HomeworkDescriptionMax,
                    autoFix: true, fixTarget: _description, autoNotify: true);
                self.Notes = _description.Text;
                //OnChange();
            };

            dateButton.Click += (e, ev) =>
            {
                introDate.Begin();
            };
            datePicker.BackPressed += (time) =>
            {
                exitDate.Begin();
                dateButton.Content = CoreTime.FormatTime(time);
                self.DueOn = time;
                //OnChange();
            };
        }
    }
}
