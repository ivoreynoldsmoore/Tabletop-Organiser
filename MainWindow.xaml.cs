using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AbilityScores bonuses = new AbilityScores( 2, 0, 1, 0, 0, 0 );
            AbilityScores abilityScores = AbilityScores.GenerateScores(bonuses);
            StrRollDisplay.Content = abilityScores.strength.ToString();
            DexRollDisplay.Content = abilityScores.dexterity.ToString();
            ConRollDisplay.Content = abilityScores.constitution.ToString();
            IntRollDisplay.Content = abilityScores.intelligence.ToString();
            WisRollDisplay.Content = abilityScores.wisdom.ToString();
            ChaRollDisplay.Content = abilityScores.charisma.ToString();
        }
    }
}
