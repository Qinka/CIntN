using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qinka.CIntN.Sample;
using Qinka.CIntN.ConcurrencyEdition;

namespace CIntNTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string x;
            CIntN_MT one = new CIntN_MT("1"),i= new CIntN_MT("1");
            CIntN_MT df = new CIntN_MT("11");
            x = Console.ReadLine();
            CIntN_MT xx = new CIntN_MT(x);
            CIntN sx = new CIntN(x);
            Console.WriteLine("多线程>>");
            Console.WriteLine("{0}: {1}",xx.ToString(),xx.阶乘().ToString());
            Console.WriteLine("单线程>>");
            Console.WriteLine("{0}: {1}", sx.ToString(), sx.阶乘().ToString());

        //    Console.WriteLine((xx*df).ToString());
            Console.ReadKey();
        }
    }
}
