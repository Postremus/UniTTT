using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace UniTTT.Logik.Tests
{
    [TestFixture]
    public class Vector2iTest
    {
        [Test]
        public void TestConstructor()
        {
            Vector2i vect = new Vector2i(0, 0);
            Assert.AreEqual(0, vect.X);
            Assert.AreEqual(0, vect.Y);

            vect = new Vector2i(25, 25);
            Assert.AreEqual(25, vect.X);
            Assert.AreEqual(25, vect.Y);

            vect = new Vector2i(-25, -25);
            Assert.AreEqual(-25, vect.X); ;
            Assert.AreEqual(-25, vect.Y);
        }

        [Test]
        public void TestFromIndex()
        {
            Vector2i vect = Vector2i.FromIndex(0, 3, 3);
            Assert.AreEqual(0, vect.X);
            Assert.AreEqual(0, vect.Y);

            vect = Vector2i.FromIndex(5, 3, 3);
            Assert.AreEqual(1, vect.X);
            Assert.AreEqual(2, vect.Y);

            vect = Vector2i.FromIndex(8, 3, 3);
            Assert.AreEqual(2, vect.X);
            Assert.AreEqual(2, vect.Y);
        }

        [Test]
        public void TestFromString()
        {
            Vector2i vect = Vector2i.FromString("X:0;Y:0", true, ';');
            Assert.AreEqual(0, vect.X);
            Assert.AreEqual(0, vect.Y);

            vect = Vector2i.FromString("X:2!Y:1", true, '!');
            Assert.AreEqual(2, vect.X);
            Assert.AreEqual(1, vect.Y);

            vect = Vector2i.FromString("0\"0", false, '"');
            Assert.AreEqual(0, vect.X);
            Assert.AreEqual(0, vect.Y);

            vect = Vector2i.FromString("2?1", false, '?');
            Assert.AreEqual(2, vect.X);
            Assert.AreEqual(1, vect.Y);
        }

        [Test]
        public void TestToString()
        {
            Vector2i vect = new Vector2i(0, 0);
            Assert.AreEqual("X:0|Y:0", vect.ToString());

            vect = new Vector2i(2, 2);
            Assert.AreEqual("X:2|Y:2", vect.ToString());

            vect = new Vector2i(1, 2);
            Assert.AreEqual("X:1|Y:2", vect.ToString());
        }

        [Test]
        public void TestAdditionOperator()
        {
            Vector2i vect = new Vector2i(2, 1) + new Vector2i(0, 1);
            Assert.AreEqual(2, vect.X);
            Assert.AreEqual(2, vect.Y);

            vect = new Vector2i(0, 0) + new Vector2i(2, 1);
            Assert.AreEqual(2, vect.X);
            Assert.AreEqual(1, vect.Y);
        }

        [Test]
        public void TestSubtractionOperator()
        {
            Vector2i vect = new Vector2i(2, 1) - new Vector2i(0, 1);
            Assert.AreEqual(2, vect.X);
            Assert.AreEqual(0, vect.Y);

            vect = new Vector2i(0, 0) - new Vector2i(2, 1);
            Assert.AreEqual(-2, vect.X);
            Assert.AreEqual(-1, vect.Y);
        }
    }
}
