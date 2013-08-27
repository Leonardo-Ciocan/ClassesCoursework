using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class HomeworkPreview : UserControl
    {
        public delegate void OnOpenHandler(Course model);
        public event OnOpenHandler OnOpen;

        public HomeworkPreview(Course model)
        {
            this.InitializeComponent();
            this.Loaded += (a, b) => { if (model.Modules.Count == 0) (this.Parent as StackPanel).Children.Remove(this); };
            name.Text = model.Name;
            header.Background = new SolidColorBrush(model.TileColor);
            edit.Tapped += (a, b) => OnOpen(model);

            List<HomeworkData> data = new List<HomeworkData>();//create a list of homework with the module name attached
            foreach (Module m in model.Modules) foreach (HomeworkTask tk in m.Homeworks)
                {
                    data.Add(new HomeworkData { homework = tk, ModuleName = m.Name });
                }

            //perform bubble sort to list them from the ones more urgent
            while (!isOrdered(data))
            {
                for (int i = 0; i < data.Count -1; i++)
                {
                    if (data[i].homework.DueOn > data[i+1].homework.DueOn)
                    {
                        swap(data, i, i + 1);
                    }
                }
            }

            //10 homeworks is enough for the preview:
            for (int i = 0; i < ((data.Count>10 ) ? 10: data.Count); i++)
            {
                HomeworkPreviewComponent cont = new HomeworkPreviewComponent(data[i].homework, data[i].ModuleName , model.TileColor);
                holder.Children.Add(cont);
            }
        }

        //handy struct to associate homework with the module
        struct HomeworkData
        {
            public HomeworkTask homework;
            public string ModuleName;
        }

        //this will return true when the homeworks are sorted from closest
        bool isOrdered(List<HomeworkData> hw)
        {
            for (int i = 0; i < hw.Count -1 ; i++)
            {
                var current = hw[i];
                var next = hw[i+1];
                if (current.homework.DueOn > next.homework.DueOn) return false;
            }
            return true;
        }

        //swaps two indexes using a temporary value
        void swap(List<HomeworkData> hw, int a, int b)
        {
            HomeworkData temp = hw[a];
            hw[a] = hw[b];
            hw[b] = temp;    
        }
    }
}
