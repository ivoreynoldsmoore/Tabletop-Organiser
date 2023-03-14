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

namespace Tabletop_Organiser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Character character;
        private AbilityScores bonuses;
        public MainWindow()
        {
            InitializeComponent();
            character = new();

            raceComboBox.ItemsSource = Races.races;
            raceComboBox.DisplayMemberPath = "name";
            raceComboBox.SelectedValuePath = "index";
            raceComboBox.SelectedIndex = 0;

            subraceComboBox.DisplayMemberPath = "Key";
            subraceComboBox.SelectedValuePath = "Value";

            BindScore("scores.strength");   
            BindScore("scores.dexterity");
            BindScore("scores.constitution");
            BindScore("scores.intelligence");
            BindScore("scores.wisdom");
            BindScore("scores.charisma");
            bonuses = new();
        }

        private void BindScore(string PropertyPath)
        {
            Binding bindStat = new()
            {
                Source = character,
                Path = new PropertyPath(PropertyPath),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            chaDisplay.SetBinding(ContentProperty, bindStat);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            character.scores = AbilityScores.GenerateScores(AbilityScores.Add(bonuses,Races.GetRacialBonus(character.race)));
            strDisplay.Text = character.scores.strength.ToString();
            dexDisplay.Text = character.scores.dexterity.ToString();
            conDisplay.Text = character.scores.constitution.ToString();
            intDisplay.Text = character.scores.intelligence.ToString();
            wisDisplay.Text = character.scores.wisdom.ToString();
            chaDisplay.Text = character.scores.charisma.ToString();
        }

        private void RaceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            Races.RaceIndex? possibleIndex = (Races.RaceIndex)comboBox.SelectedValue;
            if (possibleIndex is Races.RaceIndex index)
            {
                bonuses = Races.GetRacialBonus(index);
                character.race = index;
                Debug.Print(character.race.ToString());
                Dictionary<string, AbilityScores> subraces = Races.GetSubraces(index);
                Debug.Print(subraces.Keys.Count.ToString());
                if (subraces.Keys.Count > 0)
                {
                    subraceComboBox.Visibility = Visibility.Visible;
                    subraceComboBox.ItemsSource = subraces;
                    subraceComboBox.SelectedIndex = 0;
                }
                else
                {
                    character.subrace = "";
                    subraceComboBox.SelectedIndex = -1;
                    subraceComboBox.Visibility = Visibility.Hidden;
                }
            }
        }

        private void SubraceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            int itemIndex = comboBox.SelectedIndex;
            if (itemIndex >= 0)
            {
                bonuses = AbilityScores.Add(Races.GetRacialBonus(character.race), (AbilityScores)comboBox.SelectedValue);
                character.subrace = comboBox.Text;
            }
            else
            {
                bonuses = Races.GetRacialBonus(character.race);
                character.subrace = "";
            }
        }
    }
}
