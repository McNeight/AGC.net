﻿using Apollo.Virtual.AGC.Memory;

namespace Apollo.Virtual.AGC.Registers
{
    class Accumulator : FullRegister
    {
        public Accumulator(ushort address, MemoryBank bank)
            : base(address, bank)
        {
        }

        public void Add(OnesCompliment value)
        {
            var sum = value + Read();

            Write(sum);
        }

        public bool IsOverflow
        {
            get
            {
                // look at bits 16 and 15 to see if they are different
                var value = Read() & 0xC000;

                return value == 0x8000 || value == 0x4000;
            }
        }
    }
}
