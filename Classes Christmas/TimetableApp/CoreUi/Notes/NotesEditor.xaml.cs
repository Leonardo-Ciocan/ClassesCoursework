using System;
using Windows.UI.StartScreen;
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
    public sealed partial class NotesEditor : Page
    {
        public NotesEditor()
        {
            this.InitializeComponent();
        }

        Course self;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            self = (e.Parameter as object[])[1] as Course;

            title.Foreground = new SolidColorBrush(self.TileColor);
            pin.Foreground = new SolidColorBrush(self.TileColor);
            back.Foreground = new SolidColorBrush(self.TileColor);
            add.Foreground = new SolidColorBrush(self.TileColor);

            title.Text = self.Name + " notes";

            bool isPinned = false;
            foreach (SecondaryTile tl in await SecondaryTile.FindAllForPackageAsync())
            {
                if (tl.TileId == self.Name.Replace(" ", "") + "notes")
                {
                    pin.Icon = "";
                    isPinned = true;
                }
            }
            pin.Tapped += async (a, b) =>
            {
                Uri logo = new Uri("ms-appx:///Assets/TileLogo.png");
                SecondaryTile tile = new SecondaryTile();
                tile.BackgroundColor = self.TileColor;
                tile.TileId = self.Name.Replace(" ", "") + "notes";
                tile.Logo = logo;
                tile.ForegroundText = ForegroundText.Light;
                tile.DisplayName = "NotesEditor for " + self.Name;
                tile.ShortName = self.Name + " notes";
                tile.TileOptions = TileOptions.ShowNameOnLogo;
                tile.Arguments = "notes," + ((e.Parameter as object[])[0] as StudentDataBundle).Name + "," + self.Name;
                if (!isPinned)
                {
                    isPinned = await tile.RequestCreateAsync();
                    pin.Icon = "";
                }
                else
                {
                    isPinned = !await tile.RequestDeleteAsync();
                    pin.Icon = "";
                }
            };


            back.Tapped += async (a, b) =>
            {
                await CoreData.SaveTimetable((e.Parameter as object[])[0] as StudentDataBundle);
                Frame.Navigate(typeof(MainPage), (e.Parameter as object[])[0]);
            };

            moduleHolder.fill(self, typeof(NotesEditor));
            moduleHolder.OnChosen += (module) => refresh();
            refresh();



            add.Tapped += (q, b) =>
            {
                var note = new Note { Name = "Click edit to enter a name" };
                moduleHolder.Chosen.Notes.Add(note);
                refresh();
            };

            holder.SelectionChanged += (a, b) =>
                                           {
                                               
                                               if(NotesHolder.Items.Count>0)NotesHolder.SelectedIndex = holder.SelectedIndex;
                                           };
        }

        void refresh()
        {
            holder.Items.Clear();
            NotesHolder.Items.Clear();
            foreach (Note note in moduleHolder.Chosen.Notes)
            {
                var nb = new NoteBox(note, self.TileColor);
                nb.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                holder.Items.Add(new FlipViewItem { VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top, Content = nb });


                
                var item = new ListViewItem
                               {
                                   Content = note.Name,
                                   Tag = holder.Items.Count - 1,
                                   FontSize = 15,
                                   Style = (Style) Resources["ItemViewST"]
                               };
                NotesHolder.Items.Add(item);
                item.Tapped += (a, b) =>
                                   {
                                       holder.SelectedIndex = (int)item.Tag;
                                   };
                NotesHolder.SelectedIndex = 0;
            }

        }
    }
}
