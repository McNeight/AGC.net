﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apollo.Virtual.AGC.Base
{
    // https://github.com/rburkey2005/virtualagc/blob/master/yaAGC/agc_engine.c
    public class DoublePrecision
    {
        public SinglePrecision MostSignificantWord { get; protected set; }
        public SinglePrecision LeastSignificantWord { get; protected set; }

        public DoublePrecision(ushort most, ushort least)
        {
            MostSignificantWord = new SinglePrecision(most);
            LeastSignificantWord = new SinglePrecision(least);
        }

        /// <summary>
        /// Double Precision Addition
        /// </summary>
        /// <param name="left">left operand</param>
        /// <param name="right">right operand</param>
        /// <returns></returns>
        public static DoublePrecision operator +(DoublePrecision left, DoublePrecision right)
        {
            // single preceision add the least significant word and most significant word
            var lsw = left.LeastSignificantWord + right.LeastSignificantWord;
            var msw = left.MostSignificantWord + right.MostSignificantWord;

            // check for overflow and adjust
            if (lsw.IsPositiveOverflow)
                msw += OnesCompliment.PositiveOne;
            else if (lsw.IsNegativeOverflow)
                msw += OnesCompliment.NegativeOne;

            lsw.OverflowCorrect();


            return new DoublePrecision(msw, lsw);
        }
    }
}
