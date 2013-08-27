using System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TimetableApp
{
    public sealed partial class ModuleListItem : UserControl
    {
        public Module self;
        Color col;
        public ModuleListItem(Module module , Type context , Color c)
        {
            this.InitializeComponent();
            col = c;
            self = module;
            name.Text = module.Name;
            this.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            this.Loaded += (a, b) => this.Width = name.ActualWidth;
            if (context == typeof(TopicsEditor))
            {
                count.Text = module.StructureBlocks.Count.ToString();
            }
            if (context == typeof(NotesEditor))
            {
                count.Text = module.Notes.Count.ToString();
            }
            if (context == typeof(DefinitionsEditor))
            {
                count.Text = module.Definitions.Count.ToString();
            }
            if (context == typeof(HomeworkEditor))
            {
                count.Text = module.Homeworks.Count.ToString();
            }
            setSelected(false);
        }

        public void setSelected(bool selected)
        {
            name.Foreground = new SolidColorBrush((selected) ? col : Colors.DimGray);
        }
    }
}
