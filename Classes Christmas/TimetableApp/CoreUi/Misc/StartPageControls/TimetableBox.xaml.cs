using System;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class TimetableBox : UserControl
    {
        StudentDataBundle self;
        public TimetableBox(StudentDataBundle table)
        {
            this.InitializeComponent();
            _name.Text = table.Name;
            _description.Text = table.Description;
            _nameLabel.Text = table.Name;
            self=table;
            preview.FillWith(self);
        }

        public delegate void DeletedEventHandler();
        public event DeletedEventHandler OnDeleted;

        public delegate void OpenHandler(StudentDataBundle table);
        public event OpenHandler OnOpen;

        private void edit_clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            AnimationHelper.FadeTo(front, 0, 0.5, true).Completed += (a, b) =>
            {
                back.Visibility = Windows.UI.Xaml.Visibility.Visible;
                AnimationHelper.FadeTo(back, 1, 0.5, true).Completed += (c, d) =>
                {
                    front.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                };
            };
        }

        private async void save_clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            bool rename = _name.Text != self.Name && _name.Text != "";
            self.Description = _description.Text;
            //self.Name = _name.Text;
            ValidatorResponse TitleErrors = Validator.isStringValid(
                target: _name.Text, scope: "title", allowDigits: true, allowEmpty: false, max: Validator.TimetableTitleMax, fileName: true);

            ValidatorResponse DescriptionErrors = Validator.isStringValid(
                target: _description.Text, scope: "description", allowDigits: true, allowEmpty: true, max: Validator.TimetableDescriptionMax);


            if (TitleErrors.isValid && DescriptionErrors.isValid)
            {
                if (await CoreData.EditTimetable(self, rename: rename, newName: _name.Text))
                {
                    _nameLabel.Text = _name.Text;
                    ToFront();
                }
                preview.FillWith(self);
            }
            else
            {
                string errorMessage = TitleErrors.Errors.Aggregate("Cannot save timetable because of the following errors:\n", (current, s) => current + (s + "\n"));
                errorMessage = DescriptionErrors.Errors.Aggregate(errorMessage, (current, s) => current + (s + "\n"));

                var dialog = new MessageDialog(content: errorMessage
                                                          , title: "An error occured");
                await dialog.ShowAsync();
            }
        }

        private async void delete_clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var dialog = new MessageDialog("This action cannot be undone.", "Are you sure you want to delete " + self.Name);
            var delete = new UICommand("Delete", (com) => {
                
            });
            var cancel = new UICommand("Cancel");
            dialog.Commands.Add(delete);
            dialog.Commands.Add(cancel);
            var response =  await dialog.ShowAsync();
            if (response.Label == "Delete")
            {
                if (await CoreData.DeleteTimetable(self))
                {
                    OnDeleted();
                }
                else
                {
                    var diag = new MessageDialog("Try to restart the app.", "This timetable could not be deleted");
                    await diag.ShowAsync();

                }
            }
            
        }
        
        private void back_clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ToFront();
            
        }

        private void open_clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            OnOpen(self);
        }

        private async void export_clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await CoreData.ExportTimetable(self);
        }

        private void ToFront()
        {
            AnimationHelper.FadeTo(back, 0, 0.5, true).Completed += (a, b) =>
            {
                front.Visibility = Windows.UI.Xaml.Visibility.Visible;
                AnimationHelper.FadeTo(front, 1, 0.5, true).Completed += (c, d) =>
                {
                    back.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                };
            };
        }
    }
}
