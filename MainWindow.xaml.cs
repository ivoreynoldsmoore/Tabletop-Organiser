﻿using System;
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
            character = new();
            InitialiseControls();
        }

        private void InitialiseControls()
        {
            raceComboBox.ItemsSource = Races.races;
            raceComboBox.DisplayMemberPath = "name";
            raceComboBox.SelectedValuePath = "index";
            raceComboBox.SelectedIndex = 0;

            subraceComboBox.DisplayMemberPath = "name";
            subraceComboBox.SelectedIndex = -1;

            classComboBox.ItemsSource = Roles.roles;
            classComboBox.DisplayMemberPath = "name";
            classComboBox.SelectedValuePath = "index";
            classComboBox.SelectedIndex = 0;
            characterName.Text = "Name";
            characterName.Foreground = new SolidColorBrush(Color.FromArgb(155, 0, 0, 0));

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
            character.baseScores = AbilityScores.RollScores();
            UpdateScores();
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
                UpdateScores();
            }
        }

        private void SubraceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            character.race = new Race(character.race.raceIndex, subraceComboBox.SelectedIndex);

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

        private void classComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Roles.RoleIndex? possibleIndex = (Roles.RoleIndex)raceComboBox.SelectedValue;
            if (possibleIndex is Roles.RoleIndex index)
            {
                if (character.level > Roles.GetSubclassLevelReq(character.role.classIndex))
                {
                    subclassComboBox.Visibility = Visibility.Visible;
                    subclassComboBox.ItemsSource = Roles.GetSubclasses(index);
                    subclassComboBox.SelectedIndex = 0;
                }
                else
                {
                    subclassComboBox.SelectedIndex= -1;
                    subclassComboBox.Visibility = Visibility.Hidden;
                }
                character.role = new Roles.CharacterRole(index, subraceComboBox.SelectedIndex);
            }
        }

        private void subclassComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            character.role = new Roles.CharacterRole(character.role.classIndex, subclassComboBox.SelectedIndex);
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
        private void Enter_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
            }
        }
    }
}