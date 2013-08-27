using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class DefinitionControl : EditableItem
    {


        public Definition self;
        public DefinitionControl(Definition def, Windows.UI.Color c, object container = null, Module module = null)
        {
            this.InitializeComponent();
            //set default values
            root.Background = new SolidColorBrush(c);
            self = def;
            SetEditableItems(definitionEdit, titleEdit);
            SetStaticItems(definition, title);
            title.Text = def.Caption;
            titleEdit.Text = def.Caption;
            definition.Text = def.Content;
            definitionEdit.Text = def.Content;

            //lambdas for change events
            titleEdit.TextChanged += (e, ev) =>
            {
                Validator.isStringValid(
                    target: titleEdit.Text, scope: "name of the definition", allowDigits: true, allowEmpty: true, max: Validator.DefinitionNameMax,
                    autoFix: true, fixTarget: titleEdit, autoNotify: true);
                title.Text = titleEdit.Text;
                self.Caption = titleEdit.Text;
                //OnChange();
            };
            definitionEdit.TextChanged += (e, ev) =>
            {
                Validator.isStringValid(
                    target: definitionEdit.Text, scope: "definition", allowDigits: true, allowEmpty: true, max: Validator.DefinitionDescriptionMax,
                    autoFix: true, fixTarget: definitionEdit, autoNotify: true);
                definition.Text = definitionEdit.Text;
                self.Content = definitionEdit.Text;
               // OnChange();
            };
            
            //internal lambda to remove itself from data source
            deleteButton.Click += (e, ev) =>
            {
                //OnDelete(this);
                if (container.GetType() == typeof(UIElementCollection))
                {
                    (container as UIElementCollection).Remove(this);
                }
                else
                {
                    (container as ItemCollection).Remove(this);
                }
                module.Definitions.Remove(self);
            };
        }
    }
}
