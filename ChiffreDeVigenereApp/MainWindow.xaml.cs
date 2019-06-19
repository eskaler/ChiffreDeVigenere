using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChiffreDeVigenereApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {         

        public MainWindow()
        {
            InitializeComponent();
            
            //проверка текста, вставленного в поле ключа из буфера обмена
            DataObject.AddPastingHandler(tbKey, OnPaste); 

        }

        private void BAbout_Click(object sender, RoutedEventArgs e)
        {
            Window window = new AboutWindow();
            window.ShowDialog();
        }

        private void BTransform_Click(object sender, RoutedEventArgs e)
        {
            Vigenere.SetAlphabet((int)cbAlphabet.SelectedIndex); //установка алфавита
            if ((bool)rbEncode.IsChecked)
                tbOutput.Text = Vigenere.Encode(tbInput.Text, tbKey.Text);  //шифрование
            else
                tbOutput.Text = Vigenere.Decode(tbInput.Text, tbKey.Text);  //дешифрование
        }

        

        private void CbAlphabet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbInput.Text = "";
            tbKey.Text = "";
            tbOutput.Text = "";
        }

        /// <summary>
        /// Проверка и очистка вводимого текста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string regexString = (int)cbAlphabet.SelectedIndex == 0 ? "[^А-ЯЁ]" : "[^A-Z]";
            Regex regex = new Regex(regexString);
            e.Handled = regex.IsMatch(e.Text.ToUpper());
        }

        bool modifiedPaste = false;
        /// <summary>
        /// Проверка и очистка текста, вставляемого из буфера обмена
        /// </summary>
        /// <param name="sender">TextBox получивший данные из буфера обмена</param>
        /// <param name="e"></param>
        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {

            if (modifiedPaste == false)
            {
                modifiedPaste = true;
                var tb = sender as TextBox;

                var isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
                if (!isText) return;

                var text = (e.SourceDataObject.GetData(DataFormats.UnicodeText) as string).ToUpper();


                var alphabet = (int)cbAlphabet.SelectedIndex == 0 ? "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ " : "ABCDEFGHIJKLMNOPQRSTUVWXYZ ";

                var newText = string.Empty;

                foreach(var item in text)
                {
                    if(alphabet.IndexOf(item) > -1)
                    {
                        newText += item;
                    }
                }

                e.CancelCommand();
                Clipboard.SetData(DataFormats.UnicodeText, newText);
                ApplicationCommands.Paste.Execute(newText, tb);
            }
            else
                modifiedPaste = false;
        }
    }
}
