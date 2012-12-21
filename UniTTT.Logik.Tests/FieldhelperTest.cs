using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UniTTT.Logik;

namespace UniTTT.Logik.Tests
{
    [TestFixture]
    public class FieldHelperTest
    {
        [Test]
        public void TestGetFullFields()
        {
            Fields.Brett field = new Fields.Brett(3, 3);
            field.SetField(0, 'X');
            Assert.AreEqual(1, FieldHelper.GetFullFields(field));

            field.SetField(1, 'X');
            field.SetField(2, 'X');
            field.SetField(3, 'X');
            field.SetField(4, 'X');
            field.SetField(5, 'X');

            Assert.AreEqual(6, FieldHelper.GetFullFields(field));
        }

        [Test]
        public void TestGetAllPlayerSymbols()
        {
            Fields.Brett field = new Fields.Brett(3, 3);
            field.SetField(0, 'X');
            Assert.Contains('X', FieldHelper.GetAllPlayerSymbols(field));
            field.SetField(1, 'O');
            field.SetField(8, 'I');

            Assert.Contains('X', FieldHelper.GetAllPlayerSymbols(field));
            Assert.Contains('O', FieldHelper.GetAllPlayerSymbols(field));
            Assert.Contains('I', FieldHelper.GetAllPlayerSymbols(field));
        }

        [Test]
        public void TestHasEmptyFields()
        {
            Fields.Brett field = new Fields.Brett(3, 3);
            Assert.AreEqual(true, FieldHelper.HasEmptyFields(field));

            field.SetField(0, 'X');
            Assert.AreEqual(true, FieldHelper.HasEmptyFields(field));

            field.SetField(1, 'X');
            field.SetField(2, 'X');
            field.SetField(3, 'X');
            field.SetField(4, 'X');
            field.SetField(5, 'X');
            field.SetField(6, 'X');
            field.SetField(7, 'X');
            field.SetField(8, 'X');
            Assert.AreEqual(false, FieldHelper.HasEmptyFields(field));
        }

        [Test]
        public void TestGetGameState()
        {
            Fields.Field field = new Fields.Brett(3, 3);
            Assert.AreEqual(GameStates.Laufend, FieldHelper.GetGameState(field, new Logik.Player.Player('X')));

            field.SetField(0, 'X');
            field.SetField(1, 'X'); 
            field.SetField(2, 'X');
            Assert.AreEqual(GameStates.Gewonnen, FieldHelper.GetGameState(field, new Player.Player('X')));

            Assert.AreEqual(GameStates.Verloren, FieldHelper.GetGameState(field, new Player.Player('O')));
            field.SetField(1, 'O');
            field.SetField(3, 'X');
            field.SetField(4, 'O');
            field.SetField(5, 'X');
            field.SetField(6, 'O');
            field.SetField(7, 'X');
            field.SetField(8, 'O');
            Assert.AreEqual(GameStates.Unentschieden, FieldHelper.GetGameState(field, new Player.Player('X')));
        }

        [Test]
        public void TestGetString()
        {
            Fields.Brett field = new Fields.Brett(3, 3);
            Assert.AreEqual("         ", FieldHelper.GetString(field));
            field.SetField(0, 'X');
            Assert.AreEqual("X        ", FieldHelper.GetString(field));
            field.SetField(1, 'X');
            field.SetField(2, 'O');
            field.SetField(3, 'X');
            field.SetField(4, 'O');
            field.SetField(5, 'X');
            field.SetField(6, 'O');
            field.SetField(7, 'X');
            field.SetField(8, 'X');
            Assert.AreEqual("XXOXOXOXX", FieldHelper.GetString(field));
        }
    }
}
