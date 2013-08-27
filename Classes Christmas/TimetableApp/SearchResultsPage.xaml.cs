using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TimetableApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchResultsPage : Page
    {
        public SearchResultsPage()
        {
            this.InitializeComponent();
        }

        string query;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (await CoreData.Initialize())
            {
                //iterate through the folder and get all timetables
                if (await CoreData.GetTimetables())
                {
                    
                }
            }
            else
            {
                //if something happened then suggest a fix for the user
                MessageDialog dialog = new MessageDialog("There was a problem initializing the data storage.\nTry closing and reopening the app.", "Oops");
                await dialog.ShowAsync();
            }

            back.Tapped += (a, b) => Frame.Navigate(typeof(StartPage));
            query = e.Parameter as string;
            header.Text = "Search results for " + '"' + query + '"';
            foreach (StudentDataBundle tb in CoreData.Timetables)
            {
                ComboBoxItem item = new ComboBoxItem { Content = tb.Name , Tag = tb };
                timetables.Items.Add(item);
            }

            timetables.SelectedIndex = 0;

            foreach (Course model in ((timetables.SelectedItem as FrameworkElement).Tag as StudentDataBundle).Courses)
            {
                ComboBoxItem item = new ComboBoxItem { Content = model.Name, Tag = model, Foreground = new SolidColorBrush(model.TileColor) };
                classes.Items.Add(item);
            }
            classes.SelectedIndex = 0;


            timetables.SelectionChanged += (a, b) =>
            {
                classes.Items.Clear();
                foreach (Course model in ((timetables.SelectedItem as FrameworkElement).Tag as StudentDataBundle).Courses)
                {
                    ComboBoxItem item = new ComboBoxItem { Content = model.Name , Tag = model , Foreground = new SolidColorBrush(model.TileColor)};
                    classes.Items.Add(item);
                }
                if(classes.Items.Count>0)classes.SelectedIndex = 0;
                Search();
            };



            foreach (Module module in ((classes.SelectedItem as FrameworkElement).Tag as Course).Modules)
            {
                ComboBoxItem item = new ComboBoxItem { Content = module.Name, Tag = module };
                modules.Items.Add(item);
            }
            if (modules.Items.Count > 0) modules.SelectedIndex = 0;

            classes.SelectionChanged += (a, b) =>
            {
                modules.Items.Clear();
                foreach (Module module in ((classes.SelectedItem as FrameworkElement).Tag as Course).Modules)
                {
                    ComboBoxItem item = new ComboBoxItem { Content = module.Name, Tag = module};
                    modules.Items.Add(item);
                }
                if(modules.Items.Count>0) modules.SelectedIndex = 0;
                Search();
            };

            target.SelectionChanged += (a, b) => Search();
            everywhere.Tapped += (a, b) =>
            {
                foreach (FrameworkElement elem in filters.Children)
                {
                    if (elem != in_timetable && elem != timetables)
                    {
                        elem.Visibility = (everywhere.IsChecked.Value) ? Visibility.Collapsed : Visibility.Visible;
                    }
                }
                Search();
            };
        }

        bool canSearch()
        {
            return false;
        }


        void Search()
        {
            holder.Children.Clear();
            if (target.SelectedIndex == 0 || everywhere.IsChecked.Value)
            {
                var targets = new List<DefinitionControl>();
                if (everywhere.IsChecked.Value)
                {
                    foreach (Course model in (get(timetables).Tag as StudentDataBundle).Courses) foreach (Module m in model.Modules)
                        {
                            foreach (Definition def in m.Definitions)
                            {
                                if (def.Caption.Contains(query) || def.Content.Contains(query))
                                {
                                   targets.Add(new DefinitionControl(def, model.TileColor, holder.Children, m));
                                }
                            }
                        };
                }
                else
                {
                    foreach (Definition def in (get(modules).Tag as Module).Definitions)
                    {
                        if (def.Caption.Contains(query) || def.Content.Contains(query))
                        {
                            targets.Add(new DefinitionControl(def, (get(classes).Tag as Course).TileColor, holder.Children, (get(modules).Tag as Module)));
                        }
                    }
                }

                foreach (DefinitionControl m in targets)
                {
                   holder.Children.Add(m);
                }
            }

            if (target.SelectedIndex == 1 || everywhere.IsChecked.Value)
            {
                var targets = new List<HomeworkItem>();

                if (everywhere.IsChecked.Value)
                {
                    targets.AddRange(from model in (get(timetables).Tag as StudentDataBundle).Courses 
                                     from m in model.Modules from def in m.Homeworks 
                                     where def.Notes.Contains(query) 
                                     select new HomeworkItem(def, model.TileColor, holder.Children, m));
                }
                else
                {
                    foreach (HomeworkTask def in (get(modules).Tag as Module).Homeworks)
                    {
                        if (def.Notes.Contains(query))
                        {
                            targets.Add(new HomeworkItem(def, (get(classes).Tag as Course).TileColor, holder.Children, (get(modules).Tag as Module)));
                        }
                    }
                }

                foreach (HomeworkItem m in targets)
                {
                    holder.Children.Add(m);
                }
            }

            if (target.SelectedIndex == 2 || everywhere.IsChecked.Value)
            {
                List<StructureBlockControl> targets = new List<StructureBlockControl>();

                if (everywhere.IsChecked.Value)
                {
                    foreach (Course model in (get(timetables).Tag as StudentDataBundle).Courses) foreach (Module m in model.Modules)
                        {
                            foreach (StructureBlock def in m.StructureBlocks)
                            {
                                bool found = false;
                                foreach (Topic tp in def.Topics) if (tp.Name.Contains(query)) found = true;

                                if (def.Name.Contains(query) || found)
                                {
                                    targets.Add(new StructureBlockControl(def, model.TileColor, holder.Children, m));
                                }
                            }
                        };
                }
                else
                {
                    foreach (StructureBlock def in (get(modules).Tag as Module).StructureBlocks)
                    {
                            bool found = false;
                            foreach (Topic tp in def.Topics) if (tp.Name.Contains(query)) found = true;

                            if (def.Name.Contains(query) || found)
                            {
                                targets.Add(new StructureBlockControl(def, (get(classes).Tag as Course).TileColor, holder.Children, (get(modules).Tag as Module)));
                            }
                        
                    }
                }

                foreach (StructureBlockControl m in targets)
                {
                    holder.Children.Add(m);
                }
            }
        }

        ComboBoxItem get(ComboBox box)
        {
            return box.SelectedItem as ComboBoxItem;
        }
    }
}
