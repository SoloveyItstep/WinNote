using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Notepad;

namespace EventLibrsries
{
    public class Notepad_M_
    {
        public static RichTextBox RichTextBoxElement { get; set; }
        public static ComboBox ComboBox_FontStyle { get; set; }
        public static ComboBox ComboBox_FontSize { get; set; }
        public static Button Button_Color { get; set; }
        public static Button Button_BoldFontFamily { get; set; }
        public static Button Button_ItalicFontFamily { get; set; }
        public static Button Button_Underlining { get; set; }


        public Notepad_M_()
        {
            NotepadTools notepad = new NotepadTools();
            notepad.GetFontFamilysList(ComboBox_FontStyle, RichTextBoxElement);
            notepad.GetFontSizesList(ComboBox_FontSize, RichTextBoxElement);
            notepad.FontColorSize(RichTextBoxElement, ComboBox_FontStyle, ComboBox_FontSize, Button_Color, Button_BoldFontFamily, Button_ItalicFontFamily, Button_Underlining);
            notepad.FontStyleText(RichTextBoxElement, Button_BoldFontFamily, Button_ItalicFontFamily, Button_Underlining);
            notepad.ImplementationButtonsColor(RichTextBoxElement, Button_Color);
        }
    }
}
