using System;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class NewTimetableBox : UserControl
    {
        public NewTimetableBox()
        {
            this.InitializeComponent();
        }

        public delegate void CreateHandler();
        public event CreateHandler OnCreate;

        private void add_tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            //toBack.Begin();
            AnimationHelper.FadeTo(front, 0,0.5, true).Completed += (a, b) =>
                {
                    back.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    AnimationHelper.FadeTo(back,1, 0.5, true).Completed += (c, d) =>
                        {
                            front.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        };
                };
        }

        private void back_clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //toFront.Begin();
            AnimationHelper.FadeTo(front, 0, 0.5, true).Completed += (a, b) =>
            {
                front.Visibility = Windows.UI.Xaml.Visibility.Visible;
                AnimationHelper.FadeTo(front, 1, 0.5, true).Completed += (c, d) =>
                {
                    back.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                };
            };
        }

        private async void create_clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            
            ValidatorResponse TitleErrors = Validator.isStringValid(
                target: _name.Text, scope: "title", allowDigits: true, allowEmpty: false, max: Validator.TimetableTitleMax , fileName:true);

            ValidatorResponse DescriptionErrors = Validator.isStringValid(
                target: _description.Text, scope: "description", allowDigits: true, allowEmpty: true, max: Validator.TimetableDescriptionMax);

            if (!TitleErrors.isValid || !DescriptionErrors.isValid)
            {
                string errorMessage = TitleErrors.Errors.Aggregate("Cannot create timetable because of the following errors:\n", (current, s) => current + (s + "\n"));
                errorMessage = DescriptionErrors.Errors.Aggregate(errorMessage, (current, s) => current + (s + "\n"));

                var dialog = new MessageDialog(content: errorMessage
                                                          , title: "An error occured");
                await dialog.ShowAsync();
            }
            else
            {
                var tb = new StudentDataBundle { Name = _name.Text, Description = _description.Text };
                if (await CoreData.CreateTimetable(tb))
                {
                    // CoreData.Timetables.Add(tb);
                    OnCreate();
                    toFront.Begin();
                    _name.Text = "";
                    _description.Text = "";
                }
                else
                {
                    var diag = new MessageDialog("There was an error creating it");
                    await diag.ShowAsync();
                }
            }
        }

        private async void import_clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (await CoreData.ImportTimetable())
            {
                OnCreate();
            }
            else
            {
                var diag = new MessageDialog("The file might be corrupted or you might have another timetable with the same name.Please check there isn't another timetable with the same name", "There was a problem");
                await diag.ShowAsync();
            }
        }
    }
}
