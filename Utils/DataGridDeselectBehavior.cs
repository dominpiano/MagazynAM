using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace MagazynekAM.Utils {
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

        private static void OnEnableDeselectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is DataGrid dataGrid) {
                if ((bool)e.NewValue) {
                    dataGrid.Loaded += (s, args) => {
                        var window = Window.GetWindow(dataGrid);

                        if (window != null) {
                            window.PreviewMouseDown += (s2, e2) => {
                                if (!IsClickInside(dataGrid, e2) && !IsClickOnButton(e2)) {
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
                if (source is ButtonBase && (string)(source as ButtonBase).Tag != "AddHomeRound") // Button, ToggleButton, etc.
                    return true;

                source = VisualTreeHelper.GetParent(source);
            }

            return false;
        }
    }
}
