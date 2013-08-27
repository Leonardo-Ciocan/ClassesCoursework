/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class NoteEditor : UserControl
    {
        public NoteEditor()
        {
            this.InitializeComponent();
            this.Loaded += NoteEditor_Loaded;
        }

        public delegate void BackHandler();
        public event BackHandler OnBack;

        void NoteEditor_Loaded(object sender, RoutedEventArgs e)
        {
            editableSwitch.Toggled += (_e, _ev) =>
            {
                propertiesHolder.Opacity = (!editableSwitch.IsOn) ? 1 : 0.5;
                propertiesHolder.IsHitTestVisible = !editableSwitch.IsOn;
                foreach (NoteFragmentControl cont in fragmentHolder.Children)
                {
                    cont.setEditable(editableSwitch.IsOn);
                    cont.SelectorVisible = false;
                }
            };

            addButton.Tapped += (_e, _ev) =>
            {
                NoteFragment fragment = new NoteFragment();
                self.Fragments.Add(fragment);
                NoteFragmentControl cont = new NoteFragmentControl(fragment, fragmentHolder.Children, self , (leftSide.Background as SolidColorBrush).Color);
                cont.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
                cont.Tapped += (__e, __ev) =>
                {
                    ItemSelected(__e as NoteFragmentControl);
                };
                fragmentHolder.Children.Add(cont);
            };

            backButton.Tapped += (_e, _ev) =>
            {
                OnBack();
            };

            bold.Tapped += (_e, _ev) =>
            {
                currentFragment.Bold = bold.IsChecked.Value;
                currentFragmentControl.Update();
            };

            italic.Tapped += (_e, _ev) =>
            {
                currentFragment.Italic = italic.IsChecked.Value;
                currentFragmentControl.Update();
            };

            center.Tapped += (_e, _ev) =>
            {
                currentFragment.Centered = center.IsChecked.Value;
                currentFragmentControl.Update();
            };

            sizeSlider.ValueChanged += (_e, _ev) =>
            {
                currentFragment.FontSize = sizeSlider.Value;
                currentFragmentControl.Update();
            };

            colorPicker.ColorChanged += (col) =>
            {
                currentFragment.BackColor = col;
                currentFragmentControl.Update();
            };

            colorPicker2.ColorChanged += (col) =>
            {
                currentFragment.FrontColor = col;
                currentFragmentControl.Update();
            };
        }

        Module currentModule;
        Note self;
        NoteFragment currentFragment;
        NoteFragmentControl currentFragmentControl;
        public void FillWith(Note note , Module module , Windows.UI.Color color)
        {
            if (module != currentModule) fillNotes(module);
            currentModule = module;
            self = note;
            fragmentHolder.Children.Clear();
            name.Text = note.Name;
            description.Text = note.Description;
            foreach (NoteFragment fragment in note.Fragments)
            {
                NoteFragmentControl cont = new NoteFragmentControl(fragment, fragmentHolder.Children, self,color);
                cont.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
                fragmentHolder.Children.Add(cont);
                cont.Tapped += (__e, __ev) =>
                {
                    ItemSelected(__e as NoteFragmentControl);
                };
            }
            leftSide.Background = new SolidColorBrush(color);
            rightSide.Background = new SolidColorBrush(color);
            
        }

        public void fillNotes(Module module)
        {
            notesHolder.Children.Clear();
            foreach (Note note in module.NotesEditor)
            {
                NoteControl cont = new NoteControl(note,notesHolder.Children,module);
                cont.Width = 257;
                cont.Margin = new Thickness(0, 0, 0, 7);
                notesHolder.Children.Add(cont);
                cont.OnEdit += (nt) =>
                {
                    FillWith(nt, module, (leftSide.Background as SolidColorBrush).Color);
                };
            }
        }

        public void ItemSelected(NoteFragmentControl cont)
        {
            currentFragment = cont.self;
            currentFragmentControl= cont;
            foreach (NoteFragmentControl nfc in fragmentHolder.Children)
            {
               nfc.SelectorVisible = false;
            }
            cont.SelectorVisible = true;
            sizeSlider.Value = currentFragment.FontSize;
            bold.IsChecked = (currentFragment.Bold);
            italic.IsChecked = currentFragment.Italic;
            center.IsChecked = currentFragment.Centered;
        }
    }
}*/
