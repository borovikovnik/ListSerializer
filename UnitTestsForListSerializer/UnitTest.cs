using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ListSerializer;

namespace UnitTestsForListSerializer
{
    [TestClass]
    public class UnitTest
    {

        private static bool Cmp(ListRand l1, ListRand l2)
        {
            if (l1.Count != l2.Count) return false;

            var currNode1 = l1.Head;
            var currNode2 = l2.Head;
            for (var i = 0; i < l1.Count; i++)
            {
                if (currNode1.Data != currNode2.Data ||
                    currNode1.Rand.Data != currNode2.Rand.Data) return false;
                currNode1 = currNode1.Next;
                currNode2 = currNode2.Next;
            }

            currNode1 = l1.Tail;
            currNode2 = l2.Tail;
            for (var i = 0; i < l1.Count; i++)
            {
                if (currNode1.Data != currNode2.Data ||
                    currNode1.Rand.Data != currNode2.Rand.Data) return false;
                currNode1 = currNode1.Prev;
                currNode2 = currNode2.Prev;
            }

            return true;
        }

        [TestMethod]
        public void TestMethodSimple()
        {

            var node1 = new ListNode()
            {
                Prev = null,
                Next = null,
                Rand = null,
                Data = "node1"
            };
            var node2 = new ListNode()
            {
                Prev = node1,
                Next = null,
                Rand = null,
                Data = "node2"
            };
            var node3 = new ListNode()
            {
                Prev = node2,
                Next = null,
                Rand = null,
                Data = "node3"
            };
            node1.Next = node2;
            node1.Rand = node1;
            node2.Next = node3;
            node2.Rand = node3;
            node3.Rand = node1;
            var list = new ListRand
            {
                Count = 3,
                Head = node1,
                Tail = node3
            };


            var checkList = ListSerializer.Program.CheckSer(list);
            Assert.IsTrue(Cmp(list, checkList));
        }

        [TestMethod]
        public void TestMethodOneElement()
        {

            var node1 = new ListNode()
            {
                Prev = null,
                Next = null,
                Rand = null,
                Data = "node1"
            };
            node1.Rand = node1;
            var list = new ListRand
            {
                Count = 1,
                Head = node1,
                Tail = node1
            };

            var checkList = ListSerializer.Program.CheckSer(list);
            Assert.IsTrue(Cmp(list, checkList));
        }

        [TestMethod]
        public void TestMethodNoElements()
        {
            var list = new ListRand
            {
                Count = 0
            };

            var checkList = ListSerializer.Program.CheckSer(list);
            Assert.IsTrue(Cmp(list, checkList));
        }
    }
}
