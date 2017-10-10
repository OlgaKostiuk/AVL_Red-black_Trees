using System;
using NUnit.Framework;
using TreeCollections;

namespace UnitTestProject1
{
    [TestFixture(typeof(BsTreeRB))]
    [TestFixture(typeof(BsTreeAVL))]
    public class NUnitBalanceTests<TTree> where TTree : ITreeBalanced, new()
    {
        ITreeBalanced lst = new TTree();

        [SetUp]
        public void SetUp()
        {
            lst.Clear();
        }

        [Test]
        [TestCase(null, new int[] { })]
        [TestCase(new int[] { }, new int[] { })]
        [TestCase(new int[] { 2 }, new int[] { 2 })]
        [TestCase(new int[] { 5, 6 }, new int[] { 5, 6 })]
        [TestCase(new int[] { 3, 7, 4, 9, 1 }, new int[] { 1, 3, 4, 7, 9 })]
        public void TestToArray(int[] input, int[] res)
        {
            lst.Init(input);
            CollectionAssert.AreEqual(res, lst.ToArray());
        }

        [Test]
        [TestCase(null, new int[] { })]
        [TestCase(new int[] { }, new int[] { })]
        [TestCase(new int[] { 2 }, new int[] { 2 })]
        [TestCase(new int[] { 5, 6 }, new int[] { 5, 6 })]
        [TestCase(new int[] { 3, 7, 4, 9, 1 }, new int[] { 1, 3, 4, 7, 9 })]
        public void TestInit(int[] input, int[] res)
        {
            lst.Init(input);
            CollectionAssert.AreEqual(res, lst.ToArray());
        }

        [Test]
        [TestCase(null, "")]
        [TestCase(new int[] { }, "")]
        [TestCase(new int[] { 2 }, "2")]
        [TestCase(new int[] { 5, 6 }, "5, 6")]
        [TestCase(new int[] { 3, 7, 4, 9, 1 }, "1, 3, 4, 7, 9")]
        public void TestToString(int[] input, string res)
        {
            lst.Init(input);
            Assert.AreEqual(res, lst.ToString());
        }

        [Test]
        [TestCase(null, 0)]
        [TestCase(new int[] { }, 0)]
        [TestCase(new int[] { 2 }, 1)]
        [TestCase(new int[] { 5, 6 }, 2)]
        [TestCase(new int[] { 3, 7, 4, 9, 1 }, 5)]
        public void TestSize(int[] input, int res)
        {
            lst.Init(input);
            Assert.AreEqual(res, lst.Size());
        }

        [Test]
        [TestCase(null, 0)]
        [TestCase(new int[] { }, 0)]
        [TestCase(new int[] { 2 }, 1)]
        [TestCase(new int[] { 5, 6 }, 2)]
        [TestCase(new int[] { 3, 7, 4, 9, 1 }, 3)]
        [TestCase(new int[] { 3, 7, 4, 9, 1, 12, 2, -5, 5 }, 4)]
        public void TestHeight(int[] input, int res)
        {
            lst.Init(input);
            Assert.AreEqual(res, lst.Height());
        }

        [Test]
        [TestCase(null, 0)]
        [TestCase(new int[] { }, 0)]
        [TestCase(new int[] { 2 }, 1)]
        [TestCase(new int[] { 5, 6 }, 1)]
        [TestCase(new int[] { 3, 7, 4, 9, 1 }, 2)]
        [TestCase(new int[] { 3, 7, 4, 9, 1, 12, 2, -5, 5 }, 3)]
        public void TestWidth(int[] input, int res)
        {
            lst.Init(input);
            Assert.AreEqual(res, lst.Width());
        }

        [Test]
        [TestCase(null, 0)]
        [TestCase(new int[] { }, 0)]
        [TestCase(new int[] { 2 }, 0)]
        [TestCase(new int[] { 5, 6 }, 1)]
        [TestCase(new int[] { 3, 7, 4, 9, 1 }, 2)]
        [TestCase(new int[] { 3, 7, 4, 9, 1, 12, 2, 5 }, 5)]
        public void TestNodes(int[] input, int res)
        {
            lst.Init(input);
            Assert.AreEqual(res, lst.Nodes());
        }

        [Test]
        [TestCase(null, 0)]
        [TestCase(new int[] { }, 0)]
        [TestCase(new int[] { 2 }, 1)]
        [TestCase(new int[] { 5, 6 }, 1)]
        [TestCase(new int[] { 3, 7, 4, 9, 1 }, 3)]
        public void TestLeaves(int[] input, int res)
        {
            lst.Init(input);
            Assert.AreEqual(res, lst.Leaves());
        }

        [Test]
        [TestCase(null, new int[] { 1 }, 1)]
        [TestCase(new int[] { }, new int[] { 2 }, 2)]
        [TestCase(new int[] { 2 }, new int[] { 0, 2 }, 0)]
        [TestCase(new int[] { 5, 8 }, new int[] { 5, 6, 8 }, 6)]
        [TestCase(new int[] { 3, 7, 4, 9, 1 }, new int[] { 0, 1, 3, 4, 7, 9 }, 0)]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 10)]
        public void TestAdd(int[] input, int[] res, int val)
        {
            lst.Init(input);
            lst.Add(val);
            CollectionAssert.AreEqual(res, lst.ToArray());
            Assert.IsTrue(lst.IsBalanced());
        }

        [Test]
        [TestCase(new int[] { 2 }, "", 2)]
        [TestCase(new int[] { 5, 8 }, "5", 8)]
        [TestCase(new int[] { 3, 7, 1, 0, 9, 2, 8 }, "0, 1, 3, 7, 8, 9", 2)]
        [TestCase(new int[] { 3, 7, 1, 0, 9, 2, 8 }, "0, 1, 2, 3, 7, 8", 9)]
        [TestCase(new int[] { 3, 7, 1, 0, 9, 2, 8 }, "0, 1, 2, 3, 8, 9", 7)]
        [TestCase(new int[] { 3, 7, 1, 0, 9, 2, 8 }, "0, 1, 2, 7, 8, 9", 3)]
        [TestCase(new int[] { 3, 7, 1, 0, 9, 2, 8 }, "0, 2, 3, 7, 8, 9", 1)]
        [TestCase(new int[] { 3, 7, 4, 9, 1 }, "1, 4, 7, 9", 3)]
        public void TestDel(int[] input, string res, int val)
        {
            lst.Init(input);
            lst.Del(val);
            Assert.AreEqual(res, lst.ToString());
            Assert.IsTrue(lst.IsBalanced());
        }

        [Test]
        [TestCase(null, 5)]
        [TestCase(new int[] { }, 2)]
        public void TestDelEx(int[] input, int val)
        {
            lst.Init(input);
            var ex = Assert.Throws<EmptyTreeEx>(() => lst.Del(val));
            Assert.AreEqual(typeof(EmptyTreeEx), ex.GetType());
        }

        [Test]
        [TestCase(null, 5)]
        [TestCase(new int[] { }, 2)]
        public void TestDelExEmpty(int[] input, int val)
        {
            lst.Init(input);
            var ex = Assert.Throws<EmptyTreeEx>(() => lst.Del(val));
            Assert.AreEqual(typeof(EmptyTreeEx), ex.GetType());
        }


        [Test]
        [TestCase(new int[] { 3, 7, 4, 9, 1 }, 5)]
        public void TestDelExNull(int[] input, int val)
        {
            lst.Init(input);
            var ex = Assert.Throws<ValueNotFoundEx>(() => lst.Del(val));
            Assert.AreEqual(typeof(ValueNotFoundEx), ex.GetType());
        }

        [Test]
        [TestCase(null, new int[] { })]
        [TestCase(new int[] { }, new int[] { })]
        [TestCase(new int[] { 2 }, new int[] { 2 })]
        [TestCase(new int[] { 5, 8 }, new int[] { 8, 5 })]
        [TestCase(new int[] { 3, 7, 4, 9, 1 }, new int[] { 9, 7, 4, 3, 1 })]
        [TestCase(new int[] { 3, 7, 4, 9, 1, -23, 2, -5, 5 }, new int[] { 9, 7, 5, 4, 3, 2, 1, -5, -23 })]
        public void TestReverse(int[] input, int[] res)
        {
            lst.Init(input);
            lst.Reverse();
            CollectionAssert.AreEqual(res, lst.ToArray());
            Assert.IsTrue(lst.IsBalanced());
        }

        [Test]
        [TestCase(null, new int[] { })]
        [TestCase(new int[] { }, new int[] { })]
        [TestCase(new int[] { 2 }, new int[] { })]
        [TestCase(new int[] { 5, 6 }, new int[] { })]
        [TestCase(new int[] { 3, 7, 4, 9, 1 }, new int[] { })]
        public void TestClear(int[] input, int[] res)
        {
            lst.Init(input);
            lst.Clear();
            CollectionAssert.AreEqual(res, lst.ToArray());
        }

        [Test]
        [TestCase(null)]
        [TestCase(new int[] { })]
        [TestCase(new int[] { 2 })]
        [TestCase(new int[] { 5, 8 })]
        [TestCase(new int[] { 3, 7, 4, 9, 1 })]
        [TestCase(new int[] { 3, 7, 4, 9, 1, 12, 2, -5, 5 })]
        public void TestBalance(int[] input)
        {
            lst.Init(input);
            Assert.IsTrue(lst.IsBalanced());
        }
    }
}
