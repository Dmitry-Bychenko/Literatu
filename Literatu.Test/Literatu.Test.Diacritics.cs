
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Literatu.Test {

  [TestClass]
  public class DiacriticsTests {
    [TestMethod]
    public void DiacriticsCharTests() {
      Assert.IsFalse(Diacritics.Contains('A'));
      Assert.AreEqual('e', Diacritics.Remove('é'));
    }

    [TestMethod]
    public void GetAndAdd() {
      char from = 'é';
      char to = 'a';

      char result = Diacritics.Add(to, Diacritics.Get(from));

      Assert.AreEqual('á', result);
    }
  }
}
