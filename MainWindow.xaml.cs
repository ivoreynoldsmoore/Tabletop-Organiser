using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
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
using Utils;
using Tabletop_Organiser.CharacterBuilder;
using System.Text.RegularExpressions;
using Tabletop_Organiser.FileHandling;

namespace Tabletop_Organiser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Character character;
        private static readonly Regex isInteger = new Regex("[0-9]+");
        public MainWindow()
        {
            InitializeComponent();
            FileHandler.Initialise();
            character = new();
            InitialiseControls();
        }

        private void InitialiseControls()
        {
            characterName.Text = "Name";

            raceComboBox.ItemsSource = Races.races;
            raceComboBox.DisplayMemberPath = "name";
            raceComboBox.SelectedValuePath = "index";
            raceComboBox.SelectedIndex = 0;

            subraceComboBox.DisplayMemberPath = "name";
            subraceComboBox.SelectedIndex = -1;

            Level.Text = "1";

            BindScore(strDisplay, "scores.strength");
            BindScore(dexDisplay, "scores.dexterity");
            BindScore(conDisplay, "scores.constitution");
            BindScore(intDisplay, "scores.intelligence");
            BindScore(wisDisplay, "scores.wisdom");
            BindScore(chaDisplay, "scores.charisma");
        }

        private void BindScore(TextBox Display, string PropertyPath)
        {
            Binding bindStat = new()
            {
                Source = character,
                Path = new PropertyPath(PropertyPath),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            Display.SetBinding(ContentProperty, bindStat);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            character.scores = AbilityScores.GenerateScores(Races.GetRacialBonus(character.race));
            strDisplay.Text = character.scores.strength.ToString();
            dexDisplay.Text = character.scores.dexterity.ToString();
            conDisplay.Text = character.scores.constitution.ToString();
            intDisplay.Text = character.scores.intelligence.ToString();
            wisDisplay.Text = character.scores.wisdom.ToString();
            chaDisplay.Text = character.scores.charisma.ToString();
        }

        private void RaceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Races.RaceIndex? possibleIndex = (Races.RaceIndex)raceComboBox.SelectedValue;
            if (possibleIndex is Races.RaceIndex index)
            {
                Races.Race.Subrace[] subraces = Races.GetSubraces(index);
                Debug.Print(subraces.Length.ToString());
                if (subraces.Length > 0)
                {
                    subraceComboBox.Visibility = Visibility.Visible;
                    subraceComboBox.ItemsSource = subraces;
                    subraceComboBox.SelectedIndex = 0;
                }
                else
                {
                    subraceComboBox.SelectedIndex = -1;
                    subraceComboBox.Visibility = Visibility.Hidden;
                }
                Debug.Print(character.race.ToString());
                character.race = new Race(index, subraceComboBox.SelectedIndex);
            }
        }

        private void SubraceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            character.race = new Race(character.race.raceIndex, subraceComboBox.SelectedIndex);
        }

        private void characterName_TextChanged(object sender, TextChangedEventArgs e)
        {
            character.name = characterName.Text;
        }


        private void PreviewLevelInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !isInteger.IsMatch(e.Text);
        }

        private void Level_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (text != "")
            {
                character.level = Math.Clamp(int.Parse(text), 1, 20);
            }
        }

        private void UpdateFeaturesList()
        {
            FeaturePanel.Children.Clear();
            Feature[] features = Races.GetFeatures(character.race);
            features = features.Concat(Roles.GetFeatures(character.role)).ToArray();
            foreach (Feature feature in features)
            {
                Expander expand = new Expander();
                TextBox box = new TextBox();
                box.Text = feature.description;
                expand.Name = feature.name + "Expander";
                expand.Header = feature.name;
                expand.Content = box;
                FeaturePanel.Children.Add(expand);
            }
        }
    }
}