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
using TicTacToe.Logik;

namespace TicTacToe.Wpf
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Brett b = new Brett(3, 3);
        Logik.Player.AbstractPlayer player;
        HumanPlayer player1 = new HumanPlayer('X');
        Logik.Player.AbstractPlayer player2 = new HumanPlayer('O');
        BrettDarsteller darsteller;

        public MainWindow()
        {
            InitializeComponent();
            darsteller = new BrettDarsteller(3, 3, ref mainwindow);
            player1 = new HumanPlayer('X');
            player2 = new HumanPlayer('O');
            SpielerTausch();
            lbl_1.Content = player.Ausgabe();

            Title = "TicTacToe - " + this.ToString();
        }

        private void Beenden(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WinMessage()
        {
            Brett.GameStates state = b.GetGameState(b.VarBrett, player.Spieler);
            string title = state.ToString();
            string message = state == Brett.GameStates.Gewonnen ?
                string.Format("Spieler {0} hat Gewonnen.", player.Spieler) :
                "Keiner hat Gewonnen, Unentschieden.";
            MessageBox.Show(message, title);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Neuespiel(object sender, RoutedEventArgs e)
        {
            b = new Brett(3, 3);
            lbl_1.Content = player.Ausgabe();
            darsteller = new BrettDarsteller(3, 3, ref mainwindow);
        }

        public void Btn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button btnsender = sender as Button;

                int field = int.Parse(btnsender.Name.Replace("btn_", null), System.Globalization.CultureInfo.CurrentCulture);

                if (!btnsender.HasContent)
                {
                    b.Setzen(field, player.Spieler);
                    darsteller.Update(b.VarBrett);
                    if (b.GetGameState(b.VarBrett, player.Spieler) != Brett.GameStates.Laufend)
                    {
                        WinMessage();
                        darsteller.Sperren();
                    }
                    else
                    {
                        SpielerTausch();
                        lbl_1.Content = player.Ausgabe();
                    }
                }
            }
        }

        private void SpielerTausch()
        {
            player = player1 == player ? player2 : player1;
        }

        public override string ToString()
        {
            return (player1 is Logik.Player.KIPlayer) && (player2 is Logik.Player.KIPlayer) ? "KiGame" : "HumanGame";
        }
    }
}
