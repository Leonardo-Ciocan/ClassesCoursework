
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class TopicControl : EditableItem
    {
        public delegate void ChangedEventHandler();
        public event ChangedEventHandler OnChange;

        public delegate void DeletedEventHandler(TopicControl topicC);
        public event DeletedEventHandler OnDelete;

        public Topic self;
        public TopicControl(Topic topic)
        {
            this.InitializeComponent();
            SetEditableItems(_titleEdit , deleteButton);
            SetStaticItems(_title , _revised , _studied);
            self = topic;
            _title.Text = topic.Name;
            _titleEdit.Text = topic.Name;
            _revised.IsChecked = topic.Revised;
            _studied.IsChecked = topic.Studied;

            _revised.Tapped += (e, ev) =>
            {
                self.Revised = _revised.IsChecked.Value;
                //OnChange();
            };
            _studied.Tapped += (e, ev) =>
            {
                self.Studied = _studied.IsChecked.Value;
                //OnChange();
            };

            _titleEdit.TextChanged += (e, ev) =>
            {
                Validator.isStringValid(
                    target: _titleEdit.Text, scope: "title", allowDigits: false, allowEmpty: true, max: Validator.TopicTitleMax,
                    autoFix: true, fixTarget: _titleEdit, autoNotify: true);
                _title.Text = _titleEdit.Text;
                self.Name = _titleEdit.Text;
                //OnChange();
            };

            deleteButton.Click += (e, ev) =>
            {
                OnDelete(this);
            };
        }
    }
}
