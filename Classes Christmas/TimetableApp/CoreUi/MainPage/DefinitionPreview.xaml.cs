using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class DefinitionPreview : UserControl
    {

        public delegate void OnOpenHandler(Course model);
        public event OnOpenHandler OnOpen;

       
        public DefinitionPreview(Course model)
        {
            this.InitializeComponent();
            this.Loaded += (a, b) => { if (model.Modules.Count == 0) (this.Parent as StackPanel).Children.Remove(this); };
            foreach (Module m in model.Modules)
            {
                foreach (Definition def in m.Definitions) defs.Add(def);
            }

            

            name.Text = model.Name;
            header.Background = new SolidColorBrush(model.TileColor);
            edit.Tapped += (a, b) => OnOpen(model);
            pick_random_definition(model);
            refresh.Tapped += (a, b) => pick_random_definition(model);
        }

        //this method will return a random definition to preview
        List<Definition> defs = new List<Definition>();
        Random rnd = new Random();//random generator
        void pick_random_definition(Course model)
        {
            //only there are any definitions
            if (defs.Count > 0)
            {
                Definition def = defs[rnd.Next(0, defs.Count)];
                title.Text = def.Caption;
                content.Text = def.Content;
            }
            else
            {
                title.Text = "";
                content.Text = "None of the modules have any definitions yet.\n Try adding some by tapping the pencil icon on the top right of this box";
            }
        }
    }
}
