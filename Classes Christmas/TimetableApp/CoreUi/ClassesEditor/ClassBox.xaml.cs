using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TimetableApp
{
    public sealed partial class ClassBox : UserControl
    {
        Course self;//reference to the model of this class box
        StudentDataBundle selfTB;//reference to parent StudentDataBundle for deletion

        public ClassBox(
            Course model, UIElementCollection parent, StudentDataBundle tb)
        {
            this.InitializeComponent();
            self = model;
            selfTB = tb;

            //setting initial values
            name.Text = model.Name;
            teacher.Text = model.Teacher;
            room.Text = model.Class;
            notes.Text = model.Notes;
            color.SelectedColor = model.TileColor;
            border.BorderBrush = new SolidColorBrush(model.TileColor);

            //lambdas to handle changes
            color.ColorChanged += (col) =>
            {
                model.TileColor = col;
                border.BorderBrush = new SolidColorBrush(model.TileColor);
                deleteBtn.Foreground = new SolidColorBrush(model.TileColor);
            };

            //when text changes , perform a validation check/fix
            name.TextChanged += (a, b) =>
            {
                Validator.isStringValid(
                    target: name.Text, scope: "name", allowDigits: true, allowEmpty: false, max: Validator.EventTitleMax,
                    autoFix: true, fixTarget: name, autoNotify: true);
                model.Name = name.Text;
            };

            notes.TextChanged += (a, b) =>
                {
                    Validator.isStringValid(
                    target: notes.Text, scope: "note", allowDigits: true, allowEmpty: true, max: Validator.EventNotesMax,
                    autoFix: true, fixTarget: notes, autoNotify: true);
                    model.Notes = notes.Text;
                };
            room.TextChanged += (a, b) =>
            {
                Validator.isStringValid(
                    target: room.Text, scope: "room name", allowDigits: true, allowEmpty: false, max: Validator.EventClassMax,
                    autoFix: true, fixTarget: room, autoNotify: true);
                model.Class = room.Text;
            };
            teacher.TextChanged += (a, b) =>
            {
                Validator.isStringValid(
                    target: teacher.Text, scope: "teacher name", allowDigits: false, allowEmpty: false, max: Validator.EventTeacherMax,
                    autoFix: true, fixTarget: teacher, autoNotify: true);
                model.Teacher = teacher.Text;
            };

            //lambdas to handle tap events (delete/add)
            deleteBtn.Tapped += async (a, b) =>
            {
                MessageDialog diag = new MessageDialog("This action cannot be undone", "Are you sure you want to delete " + model.Name);
                diag.Commands.Add(new UICommand("Delete", (d) =>
                {
                    parent.Remove(this);
                    tb.Courses.Remove(model);
                }));
                diag.Commands.Add(new UICommand("Cancel"));
                await diag.ShowAsync();

            };
            


            add.Tapped += (a, b) =>
            {
                Instance inst = new Instance { Day = 0, StartTime = new Time { Hours = 9, Minutes = 0 }, EndTime = new Time { Hours = 10, Minutes = 0 }, EventName = self.Name };
                self.Instances.Add(inst);
                refresh();
            };
            
            refresh();
        }

        //this method recreates the content of the holder after an update to the data source
        void refresh()
        {
            holder.Children.Clear();
            foreach (Instance c in self.Instances)
            {
                InstanceControl cont = new InstanceControl(c, selfTB);
                holder.Children.Add(cont);
                cont.OnDelete += (a, b) =>
                {
                    holder.Children.Remove(a);
                    self.Instances.Remove(b);
                };
            }
        }
    }
}
