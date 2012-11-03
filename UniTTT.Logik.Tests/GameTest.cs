using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UniTTT.Logik;
using NUnit.Mocks;

namespace UniTTT.Logik.Tests
{
    [TestFixture]
    public class GameTest
    {
        [Test]
        public void TestNextPlayer()
        {
            Game.Game game = new Game.Game(new Player.Player('X'), new Player.Player('O'), null, new Fields.Brett(3, 3));
            Assert.AreEqual('O', game.GetNextPlayer().Symbol);
            game.PlayerChange();
            Assert.AreEqual('X', game.GetNextPlayer().Symbol);
        }

        [Test]
        public void TestPlayerChange()
        {
            Game.Game game = new Game.Game(new Player.Player('X'), new Player.Player('O'), null, new Fields.Brett(3, 3));
            Assert.AreEqual('X', game.Player.Symbol);
            game.PlayerChange();
            Assert.AreEqual('O', game.Player.Symbol);
            game.PlayerChange();
            Assert.AreEqual('X', game.Player.Symbol);
        }

        [Test]
        public void TestHasEnd()
        {
            Game.Game game = new Game.Game(new Player.Player('X'), new Player.Player('O'), null, new Fields.Brett(3, 3));
            //false, da kein Feld gesetzt
            Assert.AreEqual(game.HasEnd(), false);

            //true, da X gewonnen
            game.Field.SetField(new Vector2i(0, 0), 'X');
            game.Field.SetField(new Vector2i(1, 1), 'X');
            game.Field.SetField(new Vector2i(2, 2), 'X');

            Assert.AreEqual(game.HasEnd(), true);

            //true, da O gewonnen
            game.Field.SetField(new Vector2i(0, 0), 'O');
            game.Field.SetField(new Vector2i(1, 1), 'O');
            game.Field.SetField(new Vector2i(2, 2), 'O');

            Assert.AreEqual(game.HasEnd(), true);

            //true, da unentschieden
            game.Field.Initialize();

            game.Field.SetField(new Vector2i(0, 0), 'O');
            game.Field.SetField(new Vector2i(0, 1), 'X');
            game.Field.SetField(new Vector2i(0, 2), 'O');
            game.Field.SetField(new Vector2i(1, 0), 'O');
            game.Field.SetField(new Vector2i(1, 1), 'X');
            game.Field.SetField(new Vector2i(1, 2), 'O');
            game.Field.SetField(new Vector2i(2, 0), 'X');
            game.Field.SetField(new Vector2i(2, 1), 'O');
            game.Field.SetField(new Vector2i(2, 2), 'X');
            Assert.AreEqual(game.HasEnd(), true);
        }
    }
}