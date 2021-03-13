using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Literatu;
using Literatu.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Literatu.Test {

  [TestClass]
  public class CsvTests {
    
    [TestMethod]
    public void TestParseCsv() {

      List<string> csv = new() {
        "ab\r\nc;\"123\"#comment",
        "def;456",
      };

      string[] line = CommaSeparateValue
        .ExcelWithComments
        .ParseCsv(csv)
        .FirstOrDefault();

      Assert.AreEqual(line[1], "123");
    }
    //csv.
  }

}
