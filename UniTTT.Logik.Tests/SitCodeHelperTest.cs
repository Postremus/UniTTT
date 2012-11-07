using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace UniTTT.Logik.Tests
{
    [TestFixture]
    public class SitCodeHelperTest
    {
        [Test]
        public void TestPlayertoSitCode()
        {
            Assert.AreEqual('1', SitCodeHelper.PlayertoSitCode(' '));
            Assert.AreEqual('2', SitCodeHelper.PlayertoSitCode('X'));
            Assert.AreEqual('3', SitCodeHelper.PlayertoSitCode('O'));
            Assert.AreEqual('2', SitCodeHelper.PlayertoSitCode('x'));
            Assert.AreEqual('3', SitCodeHelper.PlayertoSitCode('o'));
            Assert.AreEqual('n', SitCodeHelper.PlayertoSitCode('n'));
        }

        [Test]
        public void TestToPlayer()
        {
            Assert.AreEqual(' ', SitCodeHelper.ToPlayer('1'));
            Assert.AreEqual('X', SitCodeHelper.ToPlayer('2'));
            Assert.AreEqual('O', SitCodeHelper.ToPlayer('3'));
            Assert.AreEqual('4', SitCodeHelper.ToPlayer('4'));
        }

        [Test]
        public void TestPlayerChange()
        {
            Assert.AreEqual('3', SitCodeHelper.PlayerChange('2'));
            Assert.AreEqual('2', SitCodeHelper.PlayerChange('3'));
            Assert.AreEqual('2', SitCodeHelper.PlayerChange('1'));
        }

        [Test]
        public void TestGetEmpty()
        {
            string test = SitCodeHelper.GetEmpty(9);
            Assert.AreEqual(9, test.Count(f => f == '1'));

            test = SitCodeHelper.GetEmpty(18);
            Assert.AreEqual(18, test.Count(f => f == '1'));
        }

        [Test]
        public void TestStringToSitCode()
        {
            Assert.AreEqual("222333222", SitCodeHelper.StringToSitCode("XXXOOOXXX"));
            Assert.AreEqual("111333222", SitCodeHelper.StringToSitCode("   OOOXXX"));
        }
    }
}
