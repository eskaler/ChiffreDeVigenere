using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ChiffreDeVigenereApp.Vigenere.SetAlphabet(1);
            Assert.AreEqual("ATTACKATDAWN", ChiffreDeVigenereApp.Vigenere.Decode("LXFOPVEFRNHR", "LEMON", false));
         
        }
    }
}
