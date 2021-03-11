using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Literatu.Text;

namespace Literatu.Test {

  [TestClass]
  public class EscapementTests {

    [DataTestMethod]
    [DataRow("abc")]
    [DataRow("a'bc")]
    [DataRow("a''bc")]
    [DataRow("'a'bc'")]
    [DataRow("''")]
    [DataRow("'''")]
    public void EscapeAndUnescape(string value) {
      string result = Escaper.PascalString.Unescape(Escaper.PascalString.Escape(value));

      Assert.AreEqual(value, result);
    }

    [DataTestMethod]
    [DataRow("abc")]
    [DataRow("a'bc")]
    [DataRow("a''bc")]
    [DataRow("'a'bc'")]
    [DataRow("''")]
    [DataRow("'''")]
    [DataRow("a/bc")]
    [DataRow("a'/bc")]
    [DataRow("a''///bc")]
    [DataRow("'a'/bc'/")]
    [DataRow("//")]
    [DataRow("///")]
    public void EscapeAndUnescape2(string value) {
      Escaper escaper = new (new char[] { '\'' }, '/');

      string result = escaper.Unescape(escaper.Escape(value));

      Assert.AreEqual(value, result);
    }
  }
}
