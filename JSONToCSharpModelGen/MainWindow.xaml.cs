using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
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
using System.Xml;

namespace JSONToCSharpModelGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string windowName = "C# model generator";
        public MainWindow()
        {
            InitializeComponent();
            Title = windowName;
            ClassStyleDropdown.Items.Clear();
            ClassStyleDropdown.ItemsSource = Enum.GetValues(typeof(CSharpClassStyle)).Cast<CSharpClassStyle>();
            ClassStyleDropdown.SelectedIndex = 0;
        }


        private async Task<string> Generate(string json, CSharpGeneratorSettings settings,string name = "name")
        {
            var schema = JsonSchema.FromSampleJson(json);
            schema.AllowAdditionalProperties = false;
            schema.Title = name;
            
            string temp=schema.ToJson();
            string temp2 = schema.ToString();
            var generator = new CSharpGenerator(schema, settings);

            var code = generator.GenerateFile();
            return code;
        }
        private async void ParseAndWork()
        {
            string collection = "System.Collections.Generic.List";
            Title = "Parsing - " + windowName;
            try
            {
                CSharpGeneratorSettings settings = new CSharpGeneratorSettings()
                {
                    ClassStyle = (CSharpClassStyle)ClassStyleDropdown.SelectedItem,
                    GenerateJsonMethods = SerializeMethothodsCheckbox.IsChecked.Value,
                    GenerateDataAnnotations = SerializeMethothodsCheckbox.IsChecked.Value,
                    PropertyNameGenerator = new CamelCasePropertyNameGenerator(),
                    TypeNameGenerator = new CamelCasePropertyNameGenerator(),
                    Namespace = NamespaceTextBox.Text,
                    ArrayBaseType = collection,
                    ArrayInstanceType= collection,
                    ArrayType = collection,
                    HandleReferences=true
                };
                string generatedCode = await Generate(JsonModelTextBox.Text, settings, ClassnameTextbox.Text);
                //generatedCode = Regex.Replace(generatedCode, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

                for (int i = 0; i < 15; i++)
                {
                    generatedCode += "\n";
                }
                
                CSModelTextBox.Text = generatedCode;
                Title = "Done - " + windowName;
            }
            catch (Exception ex)
            {

                Title = "Invalid json - " + windowName;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ParseAndWork();
        }

        private void JsonModel_TextChanged(object sender, TextChangedEventArgs e)
        {
            ParseAndWork();
        }

        private void ClassStyleDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParseAndWork();
        }

        private void SerializeMethothodsCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            ParseAndWork();

        }
        
    }

}
