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

namespace UniTTT.Wpf
{
    public class BrettDarsteller : Logik.IGraphicalBrettDarsteller
    {
        public BrettDarsteller(int breite, int hoehe, ref MainWindow mainwindow)
        {
            Breite = breite;
            Hoehe = hoehe;
            btn = new Button[breite, hoehe];
            MWindow = mainwindow;
            Erstellen();
        }

        #region Fields
        public int Breite { get; private set; }
        public int Hoehe { get; private set; }
        public Button[,] btn { get; private set; }
        private MainWindow MWindow;
        #endregion 

        public void Update(char[,] brett)
        {
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                    btn[x, y].Content = brett[x, y] != ' ' ? brett[x, y].ToString() : null;
        }

        public void Draw()
        {
        }

        public void Erstellen()
        {
            Thickness margin = new Thickness();
            margin.Left = 148;
            margin.Top = 20;
            int count = 0;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    btn[x, y] = new Button();
                    btn[x, y].Margin = margin;
                    btn[x, y].Click += MWindow.Btn_Click;
                    btn[x, y].Name = "btn_" + count;
                    btn[x, y].Height = 55;
                    btn[x, y].Width = 70;
                    btn[x, y].VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    btn[x, y].HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                    Grid.SetRow(btn[x, y], 2);
                    MWindow.grid.Children.Add(btn[x, y]);

                    margin.Top += 61;
                    count++;
                }
                margin.Top = 20;
                margin.Left += 76;
            }
            MWindow.Content = MWindow.grid;
        }

        public void Sperren()
        {
            foreach (var button in btn)
                button.IsEnabled = false;
        }

        public void EntSperren()
        {
            foreach (var button in btn)
                button.IsEnabled = true;
        }
    }
}
