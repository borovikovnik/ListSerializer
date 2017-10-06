using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ListSerializer
{
    public class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        // Bad solution, but O(n)
        public void BadSerialize(FileStream s)
        {
            var byteList = new ByteList();

            // Repace data with Id of Node
            var currNode = Head;
            for (var i = 0; i < Count; i++)
            {
                byteList.DataSaver.Add(currNode.Data);
                currNode.Data = i.ToString();
                currNode = currNode.Next;
            }

            // Save information about randNode Id, size of data and data itself
            currNode = Head;
            for (var i = 0; i < Count; i++)
            {
                var rand = BitConverter.GetBytes(int.Parse(currNode.Rand.Data));
                byteList.Bytes.AddRange(rand);
                var data = byteList.DataSaver[i];
                var dataSize = BitConverter.GetBytes(data.Length);
                byteList.Bytes.AddRange(dataSize);
                byteList.Bytes.AddRange(Encoding.ASCII.GetBytes(data));
                byteList.Bytes.Add(0); // For control of corruption in process of deserialization
                currNode = currNode.Next;
            }


            // Return native data into nodes
            currNode = Head;
            for (var i = 0; i < Count; i++)
            {
                currNode.Data = byteList.DataSaver[i];
                currNode = currNode.Next;
            }

            var count = BitConverter.GetBytes(Count);
            s.Write(count, 0, sizeof(int));
            s.WriteByte(0); // For control of corruption in process of deserialization
            s.Write(byteList.Bytes.ToArray(), 0, byteList.Bytes.Count);
        }

        // Normal solution, but O(nlogn)
        public void Serialize(FileStream s)
        {
            var byteList = new ByteList();
            var nodeList = new List<ListNode> {Head};

            for (var i = 1; i < Count; i++)
            {
                nodeList.Add(nodeList.Last().Next);
            }

            // Save information about randNode Id, size of data and data itself
            var currNode = Head;
            for (var i = 0; i < Count; i++)
            {
                var rand = BitConverter.GetBytes(nodeList.FindIndex(x => x == currNode.Rand));
                byteList.Bytes.AddRange(rand);
                var data = currNode.Data;
                var dataSize = BitConverter.GetBytes(data.Length);
                byteList.Bytes.AddRange(dataSize);
                byteList.Bytes.AddRange(Encoding.ASCII.GetBytes(data));
                byteList.Bytes.Add(0); // For control of corruption in process of deserialization
                currNode = currNode.Next;
            }

            var count = BitConverter.GetBytes(Count);
            s.Write(count, 0, sizeof(int));
            s.WriteByte(0); // For control of corruption in process of deserialization
            s.Write(byteList.Bytes.ToArray(), 0, byteList.Bytes.Count);
        }

        public void Deserialize(FileStream s)
        {
            var randNodes = new List<int>();
            var buffer = new byte[sizeof(int)];
            s.Read(buffer, 0, sizeof(int));
            Count = BitConverter.ToInt32(buffer, 0);
            var nodeList = new ListNode[Count];

            if (s.ReadByte() != 0) throw new Exception("File corrupted!");

            if (Count == 0) return;

            for (var i = 0; i < Count; i++)
            {
                s.Read(buffer, 0, sizeof(int));
                var randId = BitConverter.ToInt32(buffer, 0);
                randNodes.Add(randId);

                s.Read(buffer, 0, sizeof(int));
                var dataSize = BitConverter.ToInt32(buffer, 0);

                var dataBuffer = new byte[dataSize];
                s.Read(dataBuffer, 0, dataSize);
                nodeList[i] = new ListNode { Data = Encoding.ASCII.GetString(dataBuffer) };

                if (s.ReadByte() != 0) throw new Exception("File corrupted!");
            }

            nodeList[0].Rand = nodeList[randNodes[0]];
            for (var i = 1; i < Count - 1; i++)
            {
                nodeList[i].Prev = nodeList[i - 1];
                nodeList[i].Next = nodeList[i + 1];
                nodeList[i].Rand = nodeList[randNodes[i]];
            }

            if (Count > 1)
            {
                nodeList[0].Next = nodeList[1];
                nodeList[Count - 1].Prev = nodeList[Count - 2];
                nodeList[Count - 1].Rand = nodeList[randNodes[Count - 1]];
            }
            Head = nodeList[0];
            Tail = nodeList[Count - 1];
        }

    }
    
}
