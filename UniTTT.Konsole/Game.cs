using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniTTT.Logik;
using System.Globalization;

namespace UniTTT.Konsole
{
    class Game
    {
        Logik.Game.Game _gameMode;

        public Game(Logik.Game.Game gameMode)
        {
            _gameMode = gameMode;
            _gameMode.GetIntEvent += GetInt;
            _gameMode.GetStringEvent += Console.ReadLine;
            _gameMode.ShowMessageEvent += Console.WriteLine;
            _gameMode.WinMessageEvent += WinMessage;
            _gameMode.PlayerOutputEvent += PlayerOutput;
            _gameMode.WindowTitleChangeEvent += TitleChange;
        }

        public void Run()
        {
            do
            {
                if (!_gameMode.HasStoped)
                {
                    _gameMode.LogikLoop();
                }
                if (_gameMode is Logik.Game.NetworkGame)
                {
                    ((Logik.Game.NetworkGame)_gameMode).OnWinMessageEvent(_gameMode.Player1.Symbol, UniTTT.Logik.FieldHelper.GetGameState(_gameMode.Field, _gameMode.Player, _gameMode.Player1));
                    if (_gameMode.Player1.Symbol == 'X')
                    {
                        if (NewGameQuestion())
                        {
                            _gameMode.OnNewGameEvent();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Warte auf neues Spiel..");
                        while (!_gameMode.HasStarted) { };
                    }
                }
                else
                {
                    if (NewGameQuestion())
                    {
                        _gameMode.NewGame();
                    }
                    else
                    {
                        break;
                    }
                }
            } while (true);
        }

        private bool NewGameQuestion()
        {
            Console.WriteLine("Wollen Sie eine neue Partie spielen? (J/N)");
            return Console.ReadLine().Trim().ToUpper(CultureInfo.CurrentCulture) == "J";
        }

        private void WinMessage(char player, Logik.FieldHelper.GameStates state)
        {
            if (state == UniTTT.Logik.FieldHelper.GameStates.Gewonnen)
                Console.WriteLine("Spieler {0} hat Gewonnen", player);
            else
                Console.WriteLine(state);
        }

        private void PlayerOutput(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
        }

        private void TitleChange(string title)
        {
            Console.Title = title;
        }

        private int GetInt()
        {
            int ret;
            int.TryParse(Console.ReadLine(), out ret);
            return ret;
        }
    }
}