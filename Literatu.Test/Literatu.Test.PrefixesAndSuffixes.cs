
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Literatu.Test {

  [TestClass]
  public class PrefixesAndSuffixesTests {
    [TestMethod]
    public void TestPrefix() {
      int c = PrefixesAndSuffixes.CommonPrefixLength("abc", "abdefgh");

      Assert.AreEqual(c, 2);
    }

    [TestMethod]
    public void TestSuffix() {
      int c = PrefixesAndSuffixes.CommonSuffixLength("fgh", "abdefgh");

      Assert.AreEqual(c, 3);

      Assert.AreEqual(PrefixesAndSuffixes.CommonSuffixLength("xfgh", "abdefgh"), 3);
    }
  }
}
