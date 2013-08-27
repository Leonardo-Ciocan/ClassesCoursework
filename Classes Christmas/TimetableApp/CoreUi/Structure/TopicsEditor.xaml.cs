using System;
using Windows.UI.StartScreen;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TimetableApp
{

    public sealed partial class TopicsEditor : Page
    {
        public TopicsEditor()
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
            edit.Foreground = new SolidColorBrush(self.TileColor);
            pin.Foreground = new SolidColorBrush(self.TileColor);

            title.Text = self.Name + " definitions";

            bool isPinned = false;
            foreach (SecondaryTile tl in await SecondaryTile.FindAllForPackageAsync())
            {
                if (tl.TileId == self.Name.Replace(" ", "") + "topics")
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
                tile.TileId = self.Name.Replace(" ", "") + "topics";
                tile.Logo = logo;
                tile.ForegroundText = ForegroundText.Light;
                tile.DisplayName = "Topics for " + self.Name;
                tile.ShortName = self.Name + " topics";
                tile.TileOptions = TileOptions.ShowNameOnLogo;
                tile.Arguments = "topics," + ((e.Parameter as object[])[0] as StudentDataBundle).Name + "," + self.Name;
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

            moduleHolder.fill(self,typeof(TopicsEditor));
            moduleHolder.OnChosen += (module) =>
            {
                refresh();
            };
            refresh();



            add.Tapped += (q, b) =>
            {
                StructureBlock bl = new StructureBlock();
                moduleHolder.Chosen.StructureBlocks.Add(bl);
                refresh();
            };

            edit.Tapped += (a, b) =>
            {
                if ((edit.Foreground as SolidColorBrush).Color == Windows.UI.Colors.Black)
                {
                    foreach (EditableItem box in holder.Children) box.setEditable(false);
                    edit.Foreground = new SolidColorBrush(self.TileColor);
                }
                else
                {
                    foreach (EditableItem box in holder.Children) box.setEditable(true);
                    edit.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                }
            };
        }

        void refresh()
        { 
            if (moduleHolder.Chosen != null)
            {
                holder.Children.Clear();
                foreach (StructureBlock def in moduleHolder.Chosen.StructureBlocks)
                {
                    StructureBlockControl dc = new StructureBlockControl(def, self.TileColor, holder.Children, moduleHolder.Chosen);
                    holder.Children.Add(dc);
                }
                if (holder.Children.Count == 0) holder.Children.Add(new Image { HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center, VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top, Width = 600, Height = 600, Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/empty.png")) });

            }
        }
    }
}
