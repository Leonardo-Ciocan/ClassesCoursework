using System;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TimetableApp
{
    public sealed partial class ModuleBox : EditableItem
    {
        public Module self;
        public ModuleBox(Module module , Color col , UIElementCollection container=null , Course model=null)
        {
            this.InitializeComponent();
            SetEditableItems(moduleNameEdit);
            SetStaticItems(moduleName);
            self = module;
            deleteButton.Click += async (e, ev) =>
            {
                //OnDelete(this);
                MessageDialog diag = new MessageDialog("This action cannot be undone", "Are you sure you want to delete " + model.Name);
                diag.Commands.Add(new UICommand("Delete", (d) =>
                {
                    container.Remove(this);
                    model.Modules.Remove(self);
                }));
                diag.Commands.Add(new UICommand("Cancel"));
                await diag.ShowAsync();
                
            };
            moduleNameEdit.TextChanged += (e, ev) =>
            {
                Validator.isStringValid(
                    target: moduleNameEdit.Text, scope: "name", allowDigits: false, allowEmpty: false, max: Validator.EventTeacherMax,
                    autoFix: true, fixTarget: moduleNameEdit, autoNotify: true);
                moduleName.Text = moduleNameEdit.Text;
                self.Name = moduleNameEdit.Text;
               // OnChange();
            };
            this.Loaded += (e, ev) =>
            {
                moduleName.Text = module.Name;
                moduleNameEdit.Text = module.Name;
                root.Background = new SolidColorBrush(col);
            };
        }
    }
}
