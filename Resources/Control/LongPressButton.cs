using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VendingMachine.Resources.Control
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:VendingMachine.Resources.Control"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:VendingMachine.Resources.Control;assembly=VendingMachine.Resources.Control"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:LongPressButton/>
    ///
    /// </summary>
    public class LongPressButton : Button
    {
        public static readonly DependencyProperty DelayElapsedProperty =
         DependencyProperty.Register("DelayElapsed", typeof(double), typeof(LongPressButton), new PropertyMetadata(0d));

        public static readonly DependencyProperty DelayMillisecondsProperty =
                DependencyProperty.Register("DelayMilliseconds", typeof(int), typeof(LongPressButton), new PropertyMetadata(3000));

        public double DelayElapsed
        {
            get { return (double)this.GetValue(DelayElapsedProperty); }
            set { this.SetValue(DelayElapsedProperty, value); }
        }

        public int DelayMilliseconds
        {
            get { return (int)this.GetValue(DelayMillisecondsProperty); }
            set { this.SetValue(DelayMillisecondsProperty, value); }
        }


        private void BeginDelay()
        {
            var _animation = new DoubleAnimationUsingKeyFrames() { FillBehavior = FillBehavior.Stop };
            _animation.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)), new CubicEase() { EasingMode = EasingMode.EaseIn }));
            _animation.KeyFrames.Add(new EasingDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(this.DelayMilliseconds)), new CubicEase() { EasingMode = EasingMode.EaseIn }));
            _animation.Completed += (o, e) =>
            {
                this.DelayElapsed = 0d;
                this.Command.Execute(this.CommandParameter);    // Replace with whatever action you want to perform
            };

            this.BeginAnimation(DelayElapsedProperty, _animation);
        }

        private void CancelDelay()
        {
            // Cancel animation
            this.BeginAnimation(DelayElapsedProperty, null);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.BeginDelay();
        }


        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            this.CancelDelay();
        }
    }
}
