using System;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class ModuleList : UserControl
    {
        public ModuleList()
        {
            this.InitializeComponent();
            
        }

        public delegate void ChosenEventHandler(object target);
        public event ChosenEventHandler OnChosen;

        public Module Chosen;

        public void fill(Course model , Type context)
        {
            foreach (Module mod in model.Modules)
            {
                var c = new ModuleListItem(mod, context,model.TileColor);
                holder.Children.Add(c);
                c.Tapped += (a, b) =>
                {
                    Chosen = (a as ModuleListItem).self;
                    foreach (ModuleListItem item in holder.Children) item.setSelected(false);
                    OnChosen((a as ModuleListItem).self);
                    (a as ModuleListItem).setSelected(true);
                   
                };
            }
            (holder.Children[0] as ModuleListItem).setSelected(true);
            Chosen = (holder.Children[0] as ModuleListItem).self;
        }


    }
}
