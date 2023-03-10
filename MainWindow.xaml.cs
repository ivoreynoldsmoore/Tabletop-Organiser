using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        static readonly string[] RaceList = new string[] { "Human", "Elf", "Half Elf", "Dwarf", "Half Orc", "Dragonborn", "Halfling", "Gnome", "Tiefling"};
        public Character character;
        public MainWindow()
        {
            InitializeComponent();
            character = new Character();
            string[] raceNames = Races.GetRaces();
            //foreach (string raceName in raceNames)
            //{
            //    ComboBoxItem item = new ComboBoxItem();
            //    item.Content = raceName;
            //    RaceComboBox.Items.Add(item);
            //}

            RaceComboBox.ItemsSource = Races.races;
            RaceComboBox.DisplayMemberPath = "name";
            RaceComboBox.SelectedValuePath = "index";
            RaceComboBox.SelectedIndex = 0;

            BindScore("scores.strength");   
            BindScore("scores.dexterity");
            BindScore("scores.constitution");
            BindScore("scores.intelligence");
            BindScore("scores.wisdom");
            BindScore("scores.charisma");
        }

        private void BindScore(string PropertyPath)
        {
            Binding bindStat = new Binding();
            bindStat.Source = character;
            bindStat.Path = new PropertyPath(PropertyPath);
            bindStat.Mode = BindingMode.TwoWay;
            bindStat.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            ChaDisplay.SetBinding(ContentProperty, bindStat);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            character.scores = AbilityScores.GenerateScores(Races.GetRacialBonus(character.race));
            StrDisplay.Text = character.scores.strength.ToString();
            DexDisplay.Text = character.scores.dexterity.ToString();
            ConDisplay.Text = character.scores.constitution.ToString();
            IntDisplay.Text = character.scores.intelligence.ToString();
            WisDisplay.Text = character.scores.wisdom.ToString();
            ChaDisplay.Text = character.scores.charisma.ToString();
        }

        private void RaceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            Races.RaceIndex? possibleIndex = (Races.RaceIndex)comboBox.SelectedIndex;
            if (possibleIndex is Races.RaceIndex index)
            {
                character.race = index;
                Debug.Print(character.race.ToString());
                Dictionary<string, AbilityScores[]> subraces = Races.GetSubraces(index);
                Debug.Print(subraces.Keys.Count.ToString());
                if (subraces.Keys.Count > 0)
                {
                    SubraceComboBox.Visibility = Visibility.Visible;
                    SubraceComboBox.ItemsSource = subraces;
                    RaceComboBox.DisplayMemberPath = "key";
                    RaceComboBox.SelectedValuePath = "value";
                }
                else
                {
                    SubraceComboBox.Visibility = Visibility.Hidden;
                }
            }
        }

        private void SubraceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
