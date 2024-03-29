﻿using Literatu.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Literatu.Test {

  [TestClass]
  public class EditToTest {

    [TestMethod]
    public void EditDistance() {
      var edit = "abracadabra".ToEditProcedure("alakazam",
        c => 1,
        c => 1,
        (a, b) => a == b ? 0 : 1);

      Assert.AreEqual(edit.EditDistance, 7, string.Join(Environment.NewLine, edit.EditSequence));
    }
  }

}
