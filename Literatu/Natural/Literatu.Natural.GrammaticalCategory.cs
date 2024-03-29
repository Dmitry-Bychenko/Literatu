﻿using System;

namespace Literatu.Natural {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Gender
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  [Flags]
  public enum GrammaticalGender {
    None = 0b0000,
    Masculine = 0b0001,
    Feminine = 0b0010,
    Nueter = 0b0100,
    Common = Masculine | Feminine,
    Animate = 0b1000
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Number
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public enum GrammaticalNumber {
    Singular = 0,
    Plural = 1,
    Dual = 2,
    Trial = 3,
    Quadral = 4,
    Paucal = 5,
  }

}
