﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apollo.Virtual.AGC.Base
{
    public class OnesCompliment
    {
        public const ushort NegativeZero = 0xFFFF;
        public const ushort PositiveOne = 0x0001;
        public const ushort NegativeOne = 0xFFFE;

        public ushort Value { get; protected set; }

        public OnesCompliment(ushort v)
        {
            Value = v;
        }

        public OnesCompliment(int v)
        {
            Value = ConvertToOnesCompliment(v);
        }

        public bool IsNegativeZero {
            get
            {
                return Value == NegativeZero;
            }
        }

        public bool IsPositiveZero
        {
            get
            {
                return Value == 0;
            }
        }

        public bool IsNegative
        {
            get
            {
                return (Value & 0x8000) > 0;
            }
        }

        /// <summary>
        /// Automatically return ushort value for OnesCompliment value
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static implicit operator ushort(OnesCompliment a)
        {
            return a.Value;
        }


        /// <summary>
        /// Mainly used for converting 2's compliment negative numbers into Ones Compliment values
        /// </summary>
        /// <param name="value">2's Compliment Value (normal .net value)</param>
        /// <returns>Ones Compliment coded value</returns>
        protected static ushort ConvertToOnesCompliment(int value)
        {
            // if this is negative, 
            // return the 14 lower bits of the 1's compliment of the positive value 
            // (this is just subtracting 1 from the value), 
            // and append a 1 at bit-15
            if (value < 0)
            {
                return (ushort)(0x4000 | (value - 1));
            }
            // else return the positive value
            else
                return (ushort)value;
        }

        /// <summary>
        /// Performs overflow correction on a 16bit value, converting it to a 15 bit value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void OverflowCorrect()
        {
            uint newValue = Value;

            // get lower 14 bits
            uint lowerBits = newValue & 0x3FFF;

            // move 16-th bit, into 15th position, isolate it, and set it in above value;
            newValue = (newValue >> 1 & 0x4000) | lowerBits;

            Value = (ushort)newValue;
        }

        /// <summary>
        /// Performs sign extending on a 15bit value, converting it to a 16 bit value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SignExtend()
        {
            uint newValue = Value;

            // take lower 15-bits
            newValue = newValue & 0x7FFF;

            // shift left 1 and take 16th bit, combine with lower 15 bits
            newValue = ((newValue << 1) & 0x8000) | newValue;

            Value = (ushort)newValue;
        }
    }

    public static class OnesComplimentHelpers
    {
        public static OnesCompliment ToOnesCompliment(this int v)
        {
            return new OnesCompliment(v);
        }
    }
}
