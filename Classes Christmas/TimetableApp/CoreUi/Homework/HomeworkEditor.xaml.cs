using System;
using Windows.UI.StartScreen;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TimetableApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeworkEditor : Page
    {
        public HomeworkEditor()
        {
            this.InitializeComponent();
        }

        Course self;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            self = (e.Parameter as object[])[1] as Course;

            title.Foreground = new SolidColorBrush(self.TileColor);
            back.Foreground = new SolidColorBrush(self.TileColor);
            add.Foreground = new SolidColorBrush(self.TileColor);

            title.Text = self.Name + " homework";

            bool isPinned = false;
            foreach (SecondaryTile tl in await SecondaryTile.FindAllForPackageAsync())
            {
                if (tl.TileId == self.Name.Replace(" ", "") + "homework")
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
                tile.TileId = self.Name.Replace(" ", "") + "homework";
                tile.Logo = logo;
                tile.ForegroundText = ForegroundText.Light;
                tile.DisplayName = "HomeworkEditor for " + self.Name;
                tile.ShortName = self.Name + " homework";
                tile.TileOptions = TileOptions.ShowNameOnLogo;
                tile.Arguments = "homework," + ((e.Parameter as object[])[0] as StudentDataBundle).Name + "," + self.Name;
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

            moduleHolder.fill(self,typeof(HomeworkEditor));
            moduleHolder.OnChosen += (module) =>
            {
                refresh();
            };
            refresh();



            add.Tapped += (q, b) =>
            {
                HomeworkTask bl = new HomeworkTask();
                moduleHolder.Chosen.Homeworks.Add(bl);
                refresh();
            };
        }

        void refresh()
        {
            if (moduleHolder.Chosen != null)
            {
                holder.Children.Clear();
                foreach (HomeworkTask def in moduleHolder.Chosen.Homeworks)
                {
                    HomeworkItem dc = new HomeworkItem(def, self.TileColor, holder.Children, moduleHolder.Chosen);
                    holder.Children.Add(dc);
                }
                if (holder.Children.Count == 0) holder.Children.Add(new Image { HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center, VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top, Width = 600, Height = 600, Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/empty.png")) });

            }
        }
    }
}
