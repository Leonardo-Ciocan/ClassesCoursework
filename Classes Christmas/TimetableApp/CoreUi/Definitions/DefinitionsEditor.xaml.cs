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
    public sealed partial class DefinitionsEditor : Page
    {
        public DefinitionsEditor()
        {
            this.InitializeComponent();
            
        }

        Course self;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            self = (e.Parameter as object[])[1] as Course;//keep for reference

            //set default values
            title.Foreground = new SolidColorBrush(self.TileColor);
            back.Foreground = new SolidColorBrush(self.TileColor);
            add.Foreground = new SolidColorBrush(self.TileColor);
            edit.Foreground = new SolidColorBrush(self.TileColor);
            pin.Foreground = new SolidColorBrush(self.TileColor);
            title.Text = self.Name + " definitions";

            //Handle pinning to start screen
             bool isPinned = false;
             foreach(SecondaryTile tl in  await SecondaryTile.FindAllForPackageAsync()){
                if (tl.TileId == self.Name.Replace(" ", "") + "definitions")
                {
                    pin.Icon = "";
                    isPinned=true;
                }
            }
           
            pin.Tapped += async (a, b) =>
            {
                Uri logo = new Uri("ms-appx:///Assets/TileLogo.png");//icon
                SecondaryTile tile = new SecondaryTile();
                tile.BackgroundColor = self.TileColor;//course color
                tile.TileId = self.Name.Replace(" ", "") + "definitions";//unique id
                tile.Logo = logo;
                tile.ForegroundText = ForegroundText.Light;
                tile.DisplayName = "DefinitionsEditor for " + self.Name;
                tile.ShortName = self.Name + " definitions";
                tile.TileOptions = TileOptions.ShowNameOnLogo;
                tile.Arguments = "definitions," + ((e.Parameter as object[])[0] as StudentDataBundle).Name + "," + self.Name;//used to direct to appropiate page
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



            back.Tapped += async(a, b) =>
            {
                await CoreData.SaveTimetable((e.Parameter as object[])[0] as StudentDataBundle);
                Frame.Navigate(typeof(MainPage), (e.Parameter as object[])[0]);
            };

            //this will enable filtering by module
            moduleHolder.fill(self,typeof(DefinitionsEditor));
            moduleHolder.OnChosen += (module) =>
            {
                refresh();
            };
            refresh();

            

            add.Tapped += (q, b) =>
            {
                Definition def = new Definition { Caption="Title here" , Content="Definition or explanation goes here!" };
                moduleHolder.Chosen.Definitions.Add(def);
                refresh();
            };

            //change the editable mode of each box
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

        //redraw all boxes
        void refresh()
        {
            if (moduleHolder.Chosen != null)
            {
                holder.Children.Clear();
                foreach (Definition def in moduleHolder.Chosen.Definitions)
                {        
                    DefinitionControl dc = new DefinitionControl(def,self.TileColor, holder.Children, moduleHolder.Chosen);
                    holder.Children.Add(dc);
                }
                if (holder.Children.Count == 0) holder.Children.Add(new Image { HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center , VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top, Width = 600 , Height = 600 , Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/empty.png"))});
            }
        }
    }
}
