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
            character = new Character()
            {
                baseScores = new AbilityScores(10, 10, 10, 10, 10, 10),
            };
            InitializeComponent();
            FileHandler.Initialise();
            InitialiseControls();
            FileHandler.loadedRoles += InitialiseRoles;
            FileHandler.loadedRaces += InitialiseRaces;
        }

        private void InitialiseRaces(object? _, EventArgs e)
        {
            raceComboBox.ItemsSource = Races.races;
            raceComboBox.DisplayMemberPath = "name";
            raceComboBox.SelectedValuePath = "index";
            raceComboBox.SelectedIndex = 0;

            subraceComboBox.DisplayMemberPath = "name";
            subraceComboBox.SelectedValue = "index";
            subraceComboBox.SelectedIndex = -1;
        }

        private void InitialiseRoles(object? _, EventArgs e)
        {
            roleComboBox.ItemsSource = Roles.roles;
            roleComboBox.DisplayMemberPath = "name";
            roleComboBox.SelectedValuePath = "index";
            roleComboBox.SelectedIndex = 0;

            subroleComboBox.DisplayMemberPath = "name";
            subroleComboBox.SelectedIndex = -1;
        }

        private void InitialiseControls()
        {
            characterName.Text = "Name";
            characterName.Foreground = new SolidColorBrush(Color.FromArgb(155, 0, 0, 0));

            Level.Text = "1";

            Binding bindSpeed = new()
            {
                Source = character,
                Path = new PropertyPath("speed"),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            Speed.SetBinding(ContentProperty, bindSpeed);

            //BindScore(strDisplay, "scores.strength");
            //BindScore(dexDisplay, "scores.dexterity");
            //BindScore(conDisplay, "scores.constitution");
            //BindScore(intDisplay, "scores.intelligence");
            //BindScore(wisDisplay, "scores.wisdom");
            //BindScore(chaDisplay, "scores.charisma");
            UpdateAC();
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
            character.baseScores = AbilityScores.RollScores();
            UpdateScores();
        }

        private void RaceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Races.RaceIndex? possibleIndex = (Races.RaceIndex)raceComboBox.SelectedValue;
            if (possibleIndex is Races.RaceIndex index)
            {
                int boxIndex;
                Races.Subrace[] subraces = Races.GetSubraces(index);
                if (subraces.Length > 0)
                {
                    subraceComboBox.Visibility = Visibility.Visible;
                    subraceComboBox.ItemsSource = subraces;
                    boxIndex = 0;
                }
                else
                {
                    subraceComboBox.Visibility = Visibility.Hidden;
                    subraceComboBox.ItemsSource = Array.Empty<Races.Subrace>();
                    boxIndex = -1;
                }
                character.race = new Race(index, character.race.subraceIndex);
                subraceComboBox.SelectedIndex = boxIndex;
                UpdateScores();
                UpdateFeaturesList();
                UpdateSpeed();
            }
        }

        private void UpdateSpeed()
        {
            Speed.Content = character.movementSpeed.ToString();
        }

        private void SubraceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (subraceComboBox.SelectedIndex >= 0)
            {
                Races.SubraceIndex? possibleIndex = ((Races.Subrace)subraceComboBox.SelectedValue).index;
                if (possibleIndex is Races.SubraceIndex subraceIndex)
                {
                    character.race = new Race(character.race.raceIndex, subraceIndex);
                }
            }
            else
            {
                character.race = new Race(character.race.raceIndex, Races.SubraceIndex.none);
            }
            UpdateScores();
            UpdateFeaturesList();
            character.OnRaceChanged();
        }

        private void characterName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (characterName.Text == "Name")
            {
                characterName.Text = "";
                characterName.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            }
        }

        private void characterName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (characterName.Text == "")
            {
                characterName.Text = "Name";
                characterName.Foreground = new SolidColorBrush(Color.FromArgb(155, 0, 0, 0));
            }
        }

        private void characterName_TextChanged(object sender, TextChangedEventArgs e)
        {
            character.name = characterName.Text;
        }

        private void roleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Roles.RoleIndex? possibleIndex = (Roles.RoleIndex)roleComboBox.SelectedValue;
            if (possibleIndex is Roles.RoleIndex index)
            {
                if (character.level > Roles.GetSubclassLevelReq(character.role.roleIndex))
                {
                    subroleComboBox.Visibility = Visibility.Visible;
                    subroleComboBox.ItemsSource = Roles.GetSubclasses(index);
                    subroleComboBox.SelectedIndex = 0;
                }
                else
                {
                    subroleComboBox.SelectedIndex= -1;
                    subroleComboBox.Visibility = Visibility.Hidden;
                }
                character.role = new Roles.CharacterRole(index, subraceComboBox.SelectedIndex);
                UpdateFeaturesList();
            }
            character.OnRoleChanged();
        }

        private void subroleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            character.role = new Roles.CharacterRole(character.role.roleIndex, subroleComboBox.SelectedIndex);
            UpdateFeaturesList();
            character.OnRoleChanged();
        }

        private void PreviewLevelInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !isInteger.IsMatch(e.Text);
        }

        private void UpdateScores()
        {
            if (AutoBonusCheckbox.IsChecked != null)
            {
                UpdateAC();
                AbilityScores displayedScores = (bool)AutoBonusCheckbox.IsChecked ? character.scores : character.baseScores;
                strDisplay.Text = displayedScores.strength.ToString();
                dexDisplay.Text = displayedScores.dexterity.ToString();
                conDisplay.Text = displayedScores.constitution.ToString();
                intDisplay.Text = displayedScores.intelligence.ToString();
                wisDisplay.Text = displayedScores.wisdom.ToString();
                chaDisplay.Text = displayedScores.charisma.ToString();
            }
        }

        private void Level_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (text != "")
            {
                character.level = Math.Clamp(int.Parse(text), 1, 20);
            }
        }

        private void AC_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (text != "")
            {
                character.ac = int.Parse(text);
            }
        }

        private void UpdateAC()
        {
            if (autoACCheckBox.IsChecked.HasValue && (bool)autoACCheckBox.IsChecked)
            {
                character.calcAC();
                acTextBox.Text = character.ac.ToString();
            }
        }

        private void UpdateFeaturesList()
        {
            FeaturePanel.Children.Clear();
            foreach (Feature feature in character.features)
            {
                Expander expand = new Expander();
                TextBlock box = new TextBlock();
                box.Text = feature.description;
                box.TextWrapping = TextWrapping.Wrap;
                expand.Name = feature.name + "Expander";
                expand.Header = feature.displayName;
                expand.Content = box;
                FeaturePanel.Children.Add(expand);
            }
        }

        private void AutoACCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            character.calcAC();
            acTextBox.Text = character.ac.ToString();
            acTextBox.IsEnabled = false;
            Console.WriteLine(character.ac);
            Console.WriteLine(character.dexterityModifier);
        }

        private void AutoACCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            acTextBox.IsEnabled = true;
        }

        private void Enter_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
            }
        }

        private void AutoBonusCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateScores();
            dexDisplay.IsEnabled = false;
            strDisplay.IsEnabled = false;
            conDisplay.IsEnabled = false;
            intDisplay.IsEnabled = false;
            wisDisplay.IsEnabled = false;
            chaDisplay.IsEnabled = false;
        }

        private void AutoBonusCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateScores();
            dexDisplay.IsEnabled = true;
            strDisplay.IsEnabled = true;
            conDisplay.IsEnabled = true;
            intDisplay.IsEnabled = true;
            wisDisplay.IsEnabled = true;
            chaDisplay.IsEnabled = true;
        }
    }
}