﻿using Apollo.Virtual.AGC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AGC.Tests.Instructions
{
    [TestClass]
    public class Add : BaseTest
    {
        [TestMethod]
        public void Add_TwoPositiveNumbers()
        {
            // arrange
            Memory[0x200] = (10).ToOnesCompliment();
            Memory[0x201] = (15).ToOnesCompliment();

            // insert instructions
            Memory.LoadFixedRom(new ushort[] {
                0x06000 | 0x200,
                0x06000 | 0x201
            });

            // act - run the instructions
            CPU.Execute();
            CPU.Execute();

            // assert
            CustomAssert.AreEqual(25, Memory[0x0]);
        }

        [TestMethod]
        public void Add_TwoNegativeNumbers()
        {
            // arrange
            Memory[0x200] = (~10).ToOnesCompliment(); // -10
            Memory[0x201] = (~15).ToOnesCompliment(); // -15

            // insert instructions
            Memory.LoadFixedRom(new ushort[] {
                0x06000 | 0x200,
                0x06000 | 0x201
            });

            // act - run the instructions
            CPU.Execute();
            CPU.Execute();

            // assert
            CustomAssert.AreEqual(~25, Memory[0x0]); // -25
        }

        [TestMethod]
        public void Add_Postive1AndNegative1()
        {
            // arrange
            Memory[0x200] = (~10).ToOnesCompliment(); // -10
            Memory[0x201] = (15).ToOnesCompliment();

            // insert instructions
            Memory.LoadFixedRom(new ushort[] {
                0x06000 | 0x200,
                0x06000 | 0x201
            });

            // act - run the instructions
            CPU.Execute();
            CPU.Execute();

            // assert
            CustomAssert.AreEqual(5, Memory[0x0]);
        }

        [TestMethod]
        public void Add_TwoNumbersThatEqualNegativeZero()
        {
            // arrange
            Memory[0x200] = (0xC000).ToOnesCompliment(); // most negative number 15 bit number
            Memory[0x201] = OnesCompliment.NegativeOne;

            // insert instructions
            Memory.LoadFixedRom(new ushort[] {
                0x06000 | 0x200,
                0x06000 | 0x201
            });

            // act - run the instructions
            CPU.Execute();
            CPU.Execute();

            //save acumulator value to a place in memory so we can view the overflow corrected value, for now
            Memory[0x202] = Memory[0x0];

            // assert
            Assert.AreEqual(OnesCompliment.NegativeZero, Memory[0x202]);
        }

        [TestMethod]
        public void Add_TwoNumbersThatEqualPositiveZero()
        {
            // arrange
            Memory[0x200] = (0x3FFF).ToOnesCompliment(); // most positive number 15 bit number
            Memory[0x201] = OnesCompliment.PositiveOne;

            // insert instructions
            Memory.LoadFixedRom(new ushort[] {
                0x06000 | 0x200,
                0x06000 | 0x201
            });

            // act - run the instructions
            CPU.Execute();
            CPU.Execute();

            //save acumulator value to a place in memory so we can view the overflow corrected value, for now
            Memory[0x202] = Memory[0x0];

            // assert
            Assert.AreEqual(OnesCompliment.PositiveZero, Memory[0x202]);
        }

        [TestMethod]
        public void Add_TwoNegativeNumbersThatCauseUnderflow()
        {
            // arrange
            Memory[0x200] = (0xC000).ToOnesCompliment(); // most negative number 15 bit number
            Memory[0x201] = (~3).ToOnesCompliment(); // -3

            // insert instructions
            Memory.LoadFixedRom(new ushort[] {
                0x06000 | 0x200,
                0x06000 | 0x201
            });

            // act - run the instructions
            CPU.Execute();
            CPU.Execute();

            //save acumulator value to a place in memory so we can view the overflow corrected value, for now
            Memory[0x202] = Memory[0x0];

            // assert
            CustomAssert.AreEqual(~2, Memory[0x202]); // -2
        }

        [TestMethod]
        public void Add_TwoPositiveNumbersThatCauseOverflow()
        {
            // arrange
            Memory[0x200] = (0x3FFF).ToOnesCompliment(); // most positive number 15 bit number
            Memory[0x201] = (3).ToOnesCompliment();

            // insert instructions
            Memory.LoadFixedRom(new ushort[] {
                0x06000 | 0x200,
                0x06000 | 0x201
            });

            // act - run the instructions
            CPU.Execute();
            CPU.Execute();

            //save acumulator value to a place in memory so we can view the overflow corrected value, for now
            Memory[0x202] = Memory[0x0];

            // assert
            CustomAssert.AreEqual(2, Memory[0x202]);
        }

        [TestMethod]
        public void Add_NegativeZeroAndPositiveNumber()
        {
            // arrange
            Memory[0x200] = OnesCompliment.NegativeZero;
            Memory[0x201] = (4).ToOnesCompliment();

            // insert instructions
            Memory.LoadFixedRom(new ushort[] {
                0x06000 | 0x200,
                0x06000 | 0x201
            });

            // act - run the instructions
            CPU.Execute();
            CPU.Execute();

            //save acumulator value to a place in memory so we can view the overflow corrected value, for now
            Memory[0x202] = Memory[0x0];

            // assert
            CustomAssert.AreEqual(4, Memory[0x202]);
        }

        [TestMethod]
        public void Add_NegativeZeroAndNegativeNumber()
        {
            // arrange
            Memory[0x200] = OnesCompliment.NegativeZero;
            Memory[0x201] = (~4).ToOnesCompliment();

            // insert instructions
            Memory.LoadFixedRom(new ushort[] {
                0x06000 | 0x200,
                0x06000 | 0x201
            });

            // act - run the instructions
            CPU.Execute();
            CPU.Execute();

            //save acumulator value to a place in memory so we can view the overflow corrected value, for now
            Memory[0x202] = Memory[0x0];

            // assert
            CustomAssert.AreEqual(~4, Memory[0x202]); // -4
        }

        [TestMethod]
        public void Add_1AndNegative1EqualsNegative0()
        {
            // arrange
            Memory[0x200] = OnesCompliment.PositiveOne;
            Memory[0x201] = OnesCompliment.NegativeOne;

            // insert instructions
            Memory.LoadFixedRom(new ushort[] {
                0x06000 | 0x200,
                0x06000 | 0x201
            });

            // act - run the instructions
            CPU.Execute();
            CPU.Execute();

            //save acumulator value to a place in memory so we can view the overflow corrected value, for now
            Memory[0x202] = Memory[0x0];

            // assert
            Assert.AreEqual(OnesCompliment.NegativeZero, Memory[0x202]);
        }

        [TestMethod]
        public void Add_Double()
        {
            // arrange
            Memory[0x000] = new OnesCompliment(2);

            // insert instructions
            Memory.LoadFixedRom(new ushort[] {
                0x06000 | 0x000
            });

            // act - run the instructions
            CPU.Execute();

            // assert
            CustomAssert.AreEqual(4, Memory[0x000]);
        }
    }
}
