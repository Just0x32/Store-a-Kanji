using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

namespace Store_a_Kanji
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string onAppStartLanguage;
        private readonly int defaultLanguageId;

        private List<string> availableLanguages = new List<string>() { "reserved for ja-JP" };
        private readonly int availableLanguagesCount;

        public MainWindow()
        {
            InitializeComponent();

            onAppStartLanguage = InputLanguageManager.Current.CurrentInputLanguage.Name.ToString();
            CurrentLanguage = onAppStartLanguage;

            GetAvailableLanguages();
            availableLanguagesCount = availableLanguages.Count();

            ValidateLanguages();

            if (IsJapaneseAvailable)
            {
                defaultLanguageId = GetDefaultLanguageId();

                CurrentLanguageId = defaultLanguageId;
                CurrentLanguage = availableLanguages[defaultLanguageId];
                SetTranslateButtonText(CurrentLanguage);

                //Debug
                {
                    var sb = new StringBuilder();
                    sb.Append("defaultLanguageId = " + defaultLanguageId + Environment.NewLine);
                    sb.Append("CurrentLanguageId = " + CurrentLanguageId + Environment.NewLine);
                    sb.Append("CurrentLanguage = " + CurrentLanguage + Environment.NewLine);
                    for (int i = 0; i < availableLanguagesCount; i++)
                    {
                        sb.Append(i + ". " + availableLanguages[i] + Environment.NewLine);
                    }
                    MessageBox.Show(sb.ToString());
                }
            }
        }

        private bool IsJapaneseAvailable { get; set; } = false;

        private int CurrentLanguageId { get; set; }

        public string CurrentLanguage { get; private set; }

        private void ValidateLanguages()
        {
            if (!IsJapaneseAvailable)
            {
                MessageBox.Show("\"ja-JP\" keyboard language is not available!");
                CloseApp();
            }

            if (availableLanguagesCount < 2)
            {
                MessageBox.Show("Only Japanese keyboard language is available!");
                CloseApp();
            }
        }

        private void GetAvailableLanguages()
        {
            IEnumerable availableLanguages = InputLanguageManager.Current.AvailableInputLanguages;
            string language;

            foreach (var item in availableLanguages)
            {
                language = item.ToString();

                if (language.Contains("en-") || language.Contains("ru-"))
                {
                    this.availableLanguages.Add(language);
                }
                else if (language == "ja-JP")
                {
                    this.availableLanguages.RemoveAt(0);
                    this.availableLanguages.Insert(0, language);

                    IsJapaneseAvailable = true;
                }
            }
        }

        private int GetDefaultLanguageId()
        {
            int i = 0;

            foreach (var item in availableLanguages)
            {
                if (item == onAppStartLanguage)
                    break;

                i++;
            }

            if (i < 1)
                i = 1;

            return i;
        }

        private void TranslateButtonClick(object sender, RoutedEventArgs e)
        {
            ChangeCurrentLanguage();

            ChangeTranslateTextBoxLanguage(CurrentLanguage);
            SetTranslateButtonText(CurrentLanguage);
        }

        private void SetTranslateButtonText(string language)
        {
            if (language.Contains("en-"))
            {
                TranslateButton.Content = "Translate";
            }
            else if (language.Contains("ru-"))
            {
                TranslateButton.Content = "Перевод";
            }
        }

        private void ChangeCurrentLanguage()
        {
            CurrentLanguageId = (CurrentLanguageId < availableLanguagesCount - 1) ? CurrentLanguageId + 1 : 1;
            CurrentLanguage = availableLanguages[CurrentLanguageId];
        }

        public void JapaneseTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            InputLanguageManager.SetInputLanguage((TextBox)sender, new CultureInfo(availableLanguages[0]));
            RefreshFocusedElement();
        }

        public void TranslateTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            InputLanguageManager.SetInputLanguage((TextBox)sender, new CultureInfo(CurrentLanguage));
            RefreshFocusedElement();
        }

        private void ChangeTranslateTextBoxLanguage(string language)
        {
            InputLanguageManager.SetInputLanguage(TranslateTextBox, new CultureInfo(language));
            RefreshFocusedElement();
        }

        private void RefreshFocusedElement()
        {
            UIElement element;
            element = Keyboard.FocusedElement as UIElement;
            Keyboard.ClearFocus();
            element.Focus();
        }

        private void CloseApp() => Application.Current.Shutdown();
    }
}
