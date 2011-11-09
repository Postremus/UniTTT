using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UniTTT.Logik;

namespace UniTTT.Wpf
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Logik.NormalGame g { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            g = new NormalGame(3, 3, new Logik.Player.HumanPlayer('X'), new Logik.Player.KIPlayer(6, 3, 3, 'O'), new BrettDarsteller(3, 3, ref mainwindow), new OutputDarsteller(ref mainwindow));
            g.ODarsteller.PlayerAusgabe(g.player1.Ausgabe());
        }

        private void Beenden(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Neuespiel(object sender, RoutedEventArgs e)
        {
            g.ODarsteller.PlayerAusgabe(g.player1.Ausgabe());
            g.NewGame();
        }

        public void Btn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button btnsender = sender as Button;

                int field = int.Parse(btnsender.Name.Replace("btn_", null), System.Globalization.CultureInfo.CurrentCulture);

                if (!btnsender.HasContent)
                {
                    g.Logik(field);
                    if (g.HasEnd())
                    {
                        g.ODarsteller.WinMessage(g.player.Spieler, g.brett.GetGameState(g.brett.VarBrett, g.player.Spieler));
                        ((Logik.IGraphicalBrettDarsteller)g.BDarsteller).Sperren();
                        g.WinCounter();
                    }
                }
            }
        }
    }
}
