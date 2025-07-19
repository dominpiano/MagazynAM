using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace MagazynAM.Utils {
    public static class DataGridDeselectBehavior {

        public static readonly DependencyProperty EnableDeselectOnClickOutsideProperty =
            DependencyProperty.RegisterAttached(
                "EnableDeselectOnClickOutside",
                typeof(bool),
                typeof(DataGridDeselectBehavior),
                new PropertyMetadata(false, OnEnableDeselectChanged));

        public static bool GetEnableDeselectOnClickOutside(DependencyObject obj) =>
            (bool)obj.GetValue(EnableDeselectOnClickOutsideProperty);

        public static void SetEnableDeselectOnClickOutside(DependencyObject obj, bool value) =>
            obj.SetValue(EnableDeselectOnClickOutsideProperty, value);

        //Basically whenever and wherever mouse is pressed we detect if user clicked inside or outside the dataGrid
        private static void OnEnableDeselectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is DataGrid dataGrid) {
                if ((bool)e.NewValue) {
                    dataGrid.Loaded += (s, args) => {
                        var window = Window.GetWindow(dataGrid);
                        if (window != null) {
                            window.PreviewMouseDown += (s2, e2) => {
                                if (!IsClickInside(dataGrid, e2) && !IsClickOnButton(e2)) {
                                    //Deselecting item and clearing focus on everything
                                    dataGrid.SelectedItem = null;
                                    Keyboard.ClearFocus();
                                }
                            };
                        }
                    };
                }
            }
        }

        private static bool IsClickInside(UIElement element, MouseButtonEventArgs e) {
            Point p = e.GetPosition(element);
            return p.X >= 0 && p.X <= element.RenderSize.Width &&
                   p.Y >= 0 && p.Y <= element.RenderSize.Height;
        }

        private static bool IsClickOnButton(MouseButtonEventArgs e) {
            var source = e.OriginalSource as DependencyObject;
            while (source != null) {
                //When delete and edit button are clicked, don't clear selection
                if (source is ButtonBase && (string)(source as ButtonBase).Tag != "AddHomeRound")
                    return true;
                source = VisualTreeHelper.GetParent(source);
            }

            return false;
        }
    }
}
