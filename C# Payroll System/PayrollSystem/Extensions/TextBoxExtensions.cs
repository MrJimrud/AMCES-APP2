using System;
using System.Drawing;
using System.Windows.Forms;

namespace PayrollSystem.Extensions
{
    public static class TextBoxExtensions
    {
        public static void SetPlaceholderText(this TextBox textBox, string placeholderText)
        {
            textBox.Text = placeholderText;
            textBox.ForeColor = Color.Gray;
            
            textBox.GotFocus += (s, e) => {
                if (textBox.Text == placeholderText)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };
            
            textBox.LostFocus += (s, e) => {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholderText;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }
    }
}