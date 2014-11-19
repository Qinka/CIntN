using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qinka.CIntN.Sample;

namespace CIntNTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string x;
            CIntN one = new CIntN("1"),i= new CIntN("1");
            CIntN df = new CIntN("11");
            x = Console.ReadLine();
            CIntN xx = new CIntN(x);

                Console.WriteLine("{0} : {1}", xx.ToString(), xx.阶乘().ToString());

        //    Console.WriteLine((xx*df).ToString());
            Console.ReadKey();
        }
    }
}
