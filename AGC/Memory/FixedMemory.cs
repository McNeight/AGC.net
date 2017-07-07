﻿using Apollo.Virtual.AGC.Core;

namespace Apollo.Virtual.AGC.Memory
{
    /// <summary>
    /// 15 bit memory location that is readonly
    /// </summary>
    class FixedMemory : MemoryWord, IWord
    {
        public FixedMemory(ushort address, MemoryBank bank)
            : base(address, bank)
        {
        }

        public void Write(ushort value)
        {
            //throw new InvalidOperationException("Cannot write to a fixed memory location");
        }

        public ushort Read()
        {
            var v = new OnesCompliment(Get());
            v.SignExtend();

            return v;
        }
    }
}