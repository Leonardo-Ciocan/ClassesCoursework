using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class StructureBlockControl : EditableItem
    {
        public delegate void ChangedEventHandler();
        public event ChangedEventHandler OnChange;

        public delegate void DeletedEventHandler(StructureBlockControl structure);
        public event DeletedEventHandler OnDelete;

        public StructureBlock self;
        public StructureBlockControl(StructureBlock block,Windows.UI.Color c, UIElementCollection container = null, Module module = null)
        {
            this.InitializeComponent();
            root.Background = new SolidColorBrush(c);
            SetEditableItems(nameEdit);
            SetStaticItems(name);
            self = block;
            name.Text = self.Name;
            nameEdit.Text = self.Name;
            foreach (Topic topic in block.Topics)
            {
                TopicControl cont = new TopicControl(topic);
                topicsHolder.Children.Add(cont);
                cont.OnChange += () => OnChange();
                cont.OnDelete += (tpC) =>
                {
                    self.Topics.Remove(tpC.self);
                    topicsHolder.Children.Remove(tpC);
                   // OnChange();
                };
            }
            addButton.Tapped += (e, ev) =>
            {
                Topic topic = new Topic();
                TopicControl cont = new TopicControl(topic);
                if (isEditable) cont.setEditable(true);
                topicsHolder.Children.Add(cont);
                self.Topics.Add(topic);

                //cont.OnChange += () => ;
                cont.OnDelete += (tpC) =>
                {
                    self.Topics.Remove(tpC.self);
                    topicsHolder.Children.Remove(tpC);
                    //OnChange();
                };
            };
            deleteButton.Tapped += (e, ev) =>
            {
                //OnDelete(this);
                container.Remove(this);
                module.StructureBlocks.Remove(self);
            };
            nameEdit.TextChanged += (e, ev) =>
            {
                Validator.isStringValid(
                    target: nameEdit.Text, scope: "name", allowDigits: false, allowEmpty: true, max: Validator.StructureBlockNameMax,
                    autoFix: true, fixTarget: nameEdit, autoNotify: true);
                name.Text = nameEdit.Text;
                self.Name = nameEdit.Text;
            };
        }

        public override void setEditable(bool isEditable)
        {
            foreach (TopicControl cont in topicsHolder.Children)
            {
                cont.isEditable = isEditable;
            }
            base.setEditable(isEditable);
        }
    }
}
