using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TimetableApp
{
    /// <summary>
    /// This class can be inherited by any item that needs to show both a presentation and editing view.
    /// </summary>
    public class EditableItem : UserControl
    {
        List<FrameworkElement> Static = new List<FrameworkElement>();
        List<FrameworkElement> Editable = new List<FrameworkElement>();


        /// <summary>
        /// Set all the items that will be shown in edit mode.
        /// </summary>
        /// <param name="list">The list of editable items.</param>
        public void SetEditableItems(params FrameworkElement[] list)
        {
            Editable.Clear();
            foreach (FrameworkElement e in list)
            {
                Editable.Add(e);
            }
        }

        /// <summary>
        /// Set all the items that will be shown when not in edit mode.
        /// </summary>
        /// <param name="list">The list of static items.</param>
        public void SetStaticItems(params FrameworkElement[] list)
        {
            Static.Clear();
            foreach (FrameworkElement e in list)
            {
                Static.Add(e);
            }
        }




        bool edit = false;
        public bool isEditable
        {
            get { return edit; }
            set
            {
                edit = value;
                foreach (FrameworkElement element in Static)
                {
                    element.Visibility = (edit) ? Visibility.Collapsed : Visibility.Visible;
                }
                foreach (FrameworkElement element in Editable)
                {
                    element.Visibility = (edit) ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Set the current presentation mode.
        /// </summary>
        public virtual void setEditable(bool isEditable)
        {
            //fade out elements that should not be shown based on the current editing mode.
            foreach (FrameworkElement element in Static)
            {
                if (isEditable)
                {
                    
                    AnimationHelper.FadeTo(element, 0, 0.5, true).Completed += (a, b) => element.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                else
                {
                    AnimationHelper.FadeTo(element, 1, 0.5, true).Completed += (a, b) => element.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }

            }
            foreach (FrameworkElement element in Editable)
            {
                if (isEditable)
                {
                    AnimationHelper.FadeTo(element, 1, 0.5, true).Completed += (a, b) => element.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                else
                {
                    AnimationHelper.FadeTo(element, 0, 0.5, true).Completed += (a, b) => element.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
            }
        }
    }
}