using System;
using System.Collections.Generic;
using System.Text;

using Literatu;
using Literatu.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Literatu.Test {
  
  [TestClass]
  public class QuotationTests {

    [TestMethod]
    public void EncodePascalTest() {
      string was = "'abc''def'";
      string now = was.Enquote(Quotation.Pascal).Dequote(Quotation.Pascal);

      Assert.AreEqual(now, was);
    }

    [TestMethod]
    public void EncodeSqlTest() {
      string was = "'abc[]''def'";
      string now = was.Enquote(Quotation.MsSql).Dequote(Quotation.MsSql);

      Assert.AreEqual(now, was);
    }
  }
}
