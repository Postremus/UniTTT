﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public class NetworkGame : Game
    {
        #region privates
        private string ip;
        private int port;
        private Network.Network client;
        private bool isSending;
        private const string connectionString = "UniTTT";
        private event Network.NewVector2iReceivedHandler newVector2iReceivedEvent;
        private event Network.NewFieldReceivedHandler newFieldReceivedEvent;
        private event Network.NewGameStartedHandler newGameStartedEvent;
        private bool isNewVector2iReceivedEventRaised;
        #endregion

        public NetworkGame(Logik.Player.AbstractPlayer p1, Logik.IBrettDarsteller bdar, Logik.IOutputDarsteller odar, Logik.Fields.IField field, string ip, int port, Network.Network client)
        {
            this.ip = ip;
            this.port = port;
            this.client = client;

            client.NewMessageReceivedEvent += ReceiveVector;
            client.NewMessageReceivedEvent += ReceiveField;
            client.NewMessageReceivedEvent += ReceiveNewGame;
            newVector2iReceivedEvent += SetVectorOnField;
            newFieldReceivedEvent += EqualFieldSizes;
            newGameStartedEvent += NewGame;
            newGameStartedEvent += SendNewGame;

            isSending = p1.Symbol == 'X';

            Initialize(p1, new Player.HumanPlayer(SitCodeHelper.ToPlayer(SitCodeHelper.PlayerChange(SitCodeHelper.PlayertoSitCode(p1.Symbol)))), bdar, odar, field);

            if (!isSending)
            {
                PlayerChange();
            }
            client.Send(Field.GetBytes().ToString());
        }

        public void Logik()
        {
            PlayerChange();
            if (!HastStarted)
            {
                HastStarted = true;
            }

            if (HasStoped)
            {
                return;
            }

            if (IsODarstellerValid())
            {
                ODarsteller.PlayerAusgabe(Player.Ausgabe());
            }

            if (isSending)
            {
                Vector2i vect = Player.Play(Field);
                client.Send(string.Format("UniTTT!{0}", vect.ToString()));
                isSending = false;
                SetVectorOnField(vect);
            }
            else
            {
                while (!isNewVector2iReceivedEventRaised) { }
                isSending = true;
                isNewVector2iReceivedEventRaised = false;
            }

            if (IsBDarstellerValid())
            {
                BDarsteller.Update(Field);
                BDarsteller.Draw();
            }
        }

        public void LogikLoop()
        {
            do
            {
                Logik();
            } while (!HasEnd());
        }

        public void SetVectorOnField(Vector2i vect)
        {
            if (Field.IsFieldEmpty(vect))
            {
                Field.SetField(vect, Player.Symbol);
            }
        }

        private void ReceiveVector(string value)
        {
            //UNITTT!X:5|Y:1
            Vector2i vect = null;
            if (!value.Contains("UniTTT!"))
            {
                return;
            }
            List<string> subs = value.GetSubstrs();
            foreach (string item in subs)
            {
                if (item.Contains("UniTTT!"))
                {
                    string str = item.Remove(0, item.IndexOf('!')+1);
                    vect = Vector2i.StringToVector(str, true);
                    break;
                }
            }
            OnNewVector2iReceivedEvent(vect);
        }

        private void ReceiveField(string value)
        {
            if (!value.Contains("Field:"))
            {
                return;
            }

            Fields.IField field = (Fields.IField)value.GetObject();
            if (field != null)
            {
                OnNewFieldReceivedEvent(field);
            }
        }

        private void ReceiveNewGame(string value)
        {
            if (!value.Contains("NewGame"))
                return;
            NewGame();
        }

        private void SendNewGame()
        {
            client.Send("NewGame");
        }

        public void OnNewVector2iReceivedEvent(Vector2i vect)
        {
            isNewVector2iReceivedEventRaised = false;
            Network.NewVector2iReceivedHandler vector2iReceivedEvent = newVector2iReceivedEvent;
            if (vector2iReceivedEvent != null)
            {
                vector2iReceivedEvent(vect);
                isNewVector2iReceivedEventRaised = true;
            }
        }

        public void OnNewFieldReceivedEvent(Fields.IField field)
        {
            Network.NewFieldReceivedHandler fieldreceivedEvent = newFieldReceivedEvent;
            if (fieldreceivedEvent != null)
            {
                fieldreceivedEvent(field);
            }
        }

        public void OnNewGameStartedEvent()
        {
            Network.NewGameStartedHandler gameStartedEvent = newGameStartedEvent;
            if (gameStartedEvent != null)
            {
                gameStartedEvent();
            }
        }

        public void NewGame()
        {
            Field.Initialize();
            Player = null;
            if (IsBDarstellerGraphical())
            {
                ((Logik.IGraphicalBrettDarsteller)BDarsteller).DeLock();
            }
            BDarsteller.Update(Field);
            BDarsteller.Draw();
        }

        public void EqualFieldSizes(Fields.IField field)
        {
            HasStoped = base.Field.Width != field.Width && base.Field.Height != field.Height;
        }
    }
}