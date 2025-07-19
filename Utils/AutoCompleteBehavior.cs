using MagazynAM.MVVM.ViewModel;
using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows.Input;

namespace MagazynAM.Utils {
    public class AutoCompleteBehavior : Behavior<TextBox> {

        protected override void OnAttached() {
            AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
            AssociatedObject.TextChanged += OnTextChanged;
        }

        protected override void OnDetaching() {
            AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
            AssociatedObject.TextChanged -= OnTextChanged;
        }

        //Helper variables
        private int _prevTypedTextLength = 0;
        private int _prevTextLength = 0;

        private void OnTextChanged(object sender, TextChangedEventArgs e) {
            if (AssociatedObject.DataContext is DialogViewModel vm) {

                //Get textbox
                var tb = AssociatedObject;

                //If text is inserted not typed we have to leave it alone
                if (!tb.IsFocused || Keyboard.FocusedElement != tb)
                    return;

                int caretPos = tb.CaretIndex;
                string typedText = tb.Text.Substring(0, caretPos);
                string suggestion = vm.SuggestedSupplierText ?? "";

                //Desensitize textbox to text changes for that moment code is altering text
                tb.TextChanged -= OnTextChanged;

                //Here we are checking if IF we pressed backspace (first cond.) and the text before was a suggestion (second cond.)
                bool shouldRestore = typedText.Length <= _prevTypedTextLength
                                     && _prevTextLength != _prevTypedTextLength;

                //If there was sugg and we typed backspace (see above) then move the caret one char back and reapply the suggestion, now we are free to type further
                if (shouldRestore && typedText.Length > 1) {
                    caretPos = --tb.CaretIndex;
                    tb.Text = suggestion;
                    tb.SelectionStart = caretPos;
                    tb.SelectionLength = suggestion.Length - caretPos;
                    _prevTypedTextLength = typedText.Length - 1;
                }
                //If there is one letter left we have to clear this
                else if (shouldRestore && typedText.Length <= 1) {
                    tb.Text = "";
                    _prevTypedTextLength = 0;
                }
                else {
                    _prevTypedTextLength = typedText.Length;

                    //If there is suitable suggestion - insert it and select
                    if (!string.IsNullOrEmpty(typedText) && suggestion.StartsWith(typedText, StringComparison.InvariantCultureIgnoreCase) && suggestion.Length > typedText.Length) {
                        tb.Text = suggestion;
                        tb.SelectionStart = typedText.Length;
                        tb.SelectionLength = suggestion.Length - typedText.Length;
                    }

                    //Or clear
                    if (typedText == "")
                        tb.Text = "";
                }

                //And go back to reacting to text change
                _prevTextLength = tb.Text.Length;
                tb.TextChanged += OnTextChanged;

            }
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e) {
            //If enter is pressed, we accept the suggestion
            if (e.Key == Key.Enter && AssociatedObject.DataContext is DialogViewModel vm) {
                var tb = AssociatedObject;

                vm.SuggestedSupplierText = tb.Text;
                tb.SelectionLength = 0;
                tb.CaretIndex = tb.Text.Length;

                e.Handled = true;
            }
        }
    }
}

