﻿using Apollo.Virtual.AGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apollo.Virtual.AGC.Architecture.Instructions
{
    /// <summary>
    /// DAS - 0010 00
    /// QuarterCode Instruction
    /// 
    ///  A double-precision (DP) add of the A,L register pair to a pair of variables in erasable memory
    /// </summary>
    class DoubleAddToStorage : IInstruction
    {
        public ushort Code
        {
            get { return 0x00; }
        }

        public Processor CPU { get; set; }

        public void Execute(ushort K)
        {
            // TODO: need to handle DDOUBL

            // FIXME: is this supposed to be a subtraction?
            // find previous address
            var K0 = (ushort)(K - 1);
            
            // read DP values from registers and memroy
            var dp1 = new DoublePrecision(CPU.A.Read(), CPU.L.Read());
            var dp2 = new DoublePrecision(CPU.Memory[K0], CPU.Memory[K]);

            // create sum
            var sum = dp1 + dp2;

            // store result in memory
            CPU.Memory[K0] = sum.MostSignificantWord;
            CPU.Memory[K] = sum.LeastSignificantWord;

            // L always cleared to +0
            CPU.L.Write(OnesCompliment.PositiveZero);

            // A set based upon overflow
            if(sum.MostSignificantWord.IsPositiveOverflow)
                CPU.A.Write(OnesCompliment.PositiveOne);
            else if(sum.MostSignificantWord.IsNegativeOverflow)
                CPU.A.Write(OnesCompliment.NegativeOne);
            else
                CPU.A.Write(OnesCompliment.PositiveZero);
        }
    }
}