using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSerializer
{
    public class Program
    {
        public static ListRand CheckSer(ListRand inpList)
        {
            var path = Path.GetTempPath() + "test.xml";

            var s = new FileStream(path, FileMode.OpenOrCreate);
            inpList.Serialize(s);
            s.Close();

            s = new FileStream(path, FileMode.OpenOrCreate);
            var outList = new ListRand();
            outList.Deserialize(s);
            s.Close();
            return outList;
        }

        static void Main(string[] args)
        {

        }
    }
}
