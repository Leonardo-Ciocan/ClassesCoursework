using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace TimetableApp
{
    public class AnimationHelper
    {
        public enum SmoothMoveType{
            Left, Top ,Both
        }

        #region Opacity Animations
        /// <summary>
        /// Fade out an element to 0 opacity.
        /// </summary>
        /// <param name="target">The element to fade</param>
        /// <param name="duration">The time it should take for the animation to complete</param>
        /// <param name="autoStart">Whether the animation should start imidiately after the call or will be handled using the return value.</param>
        /// <param name="autoReverse">Whether the animation should loop back to the original opacity.</param>
        /// <returns></returns>
        public static Storyboard FadeTo(FrameworkElement target,double targetValue, double duration, bool autoStart = true, bool autoReverse = false)
        {
            //create a storyboard to handle the animation
            Storyboard sb = new Storyboard();
            //a double animation to animate opacity (of type double)
            DoubleAnimation dt = new DoubleAnimation();
            dt.Duration = new Duration(TimeSpan.FromSeconds(duration));
            dt.From = target.Opacity;
            dt.To = targetValue ;
            //set the property to Opacity and the target to target
            Storyboard.SetTargetProperty(dt, "Opacity");
            Storyboard.SetTarget(sb, target);
            //add the double animation to the storyboard
            sb.Children.Add(dt);
            sb.AutoReverse = autoReverse;
            if (autoStart) sb.Begin();
            return sb;//return the storyboard so additional operations can be chained (i.e complete events)
        }
        #endregion

        /// <summary>
        /// This will move an element to a specific location in a Canvas with a smooth animation
        /// </summary>
        /// <param name="target">The element to be moved</param>
        /// <param name="to">The Y coordonate of the wanted position</param>
        /// <param name="duration">Time for the movement to complete</param>
        /// <param name="autoStart">Whether the animation should immidiately start</param>
        /// <param name="autoReverse">Whether the animation should loop to its starting values</param>
        /// <returns></returns>
        public static Storyboard SmoothMove(FrameworkElement target, double to, double duration, bool autoStart = true, bool autoReverse = false)
        {
            Storyboard sb = new Storyboard();

                DoubleAnimation dy = new DoubleAnimation();
                dy.Duration = new Duration(TimeSpan.FromSeconds(duration));
                dy.From = Canvas.GetTop(target);
                dy.To = to;
                Storyboard.SetTargetProperty(dy, "(Canvas.Top)");
                sb.Children.Add(dy);

                Storyboard.SetTarget(sb, target);
           
            sb.AutoReverse = autoReverse;
            if (autoStart) sb.Begin();
            return sb;
        }

        /// <summary>
        /// This method allows to bind a small reaction animation to an element. The element will move when pressed to show it responds to touch.
        /// </summary>
        /// <param name="element">The element to add the animation to</param>
        public static void AddPointerAnimation(FrameworkElement element)
        {
            //animation to shrink the element when mouse is pressed
            Storyboard anim_down = new Storyboard();
            Storyboard.SetTarget(anim_down, element);
            anim_down.Children.Add(new PointerDownThemeAnimation());

            //animation to enlarge to original size when mouse is not pressed
            Storyboard anim_up = new Storyboard();
            Storyboard.SetTarget(anim_up,element);
            anim_up.Children.Add(new PointerUpThemeAnimation());

            bool down = false;
            //when the pointer enters the animation should be shrinking unless the mouse is not pressed.
            element.PointerEntered += (e, ev) =>
            {
                if (down)
                    anim_down.Begin();
            };

            //if mouse button is down then begin shrinking animation and set down to true
            element.PointerPressed += (e, ev) =>
            {
                anim_down.Begin();
                down = true;
            };

            //if the pointer leaves the bounds of the element the released event will not be detected. Therefore stop the nimation and reverse value of down.
            element.PointerExited += (e, ev) =>
            {
                anim_up.Begin();
                down = false;
            };

            //if the mouse button is released resize and set down to false.
            element.PointerReleased += (e, ev) =>
            {
                anim_up.Begin();
                down = false;
            };
        }
    }
}
