using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Management.Instrumentation;
using System.Management;
using System.Runtime.Remoting.Messaging;
using Qinka.CIntN;



namespace Qinka.CIntN
{
    public static class SysInfo
    {
        static bool  isInit = false;
        public static int NumofCPU { get; private set; }
        public static void Init()
        {
            if (isInit) return;
            isInit = true;
            mCPU = new ManagementClass("win32_Processor");
            moc = mCPU.GetInstances();
            foreach(ManagementObject mo in moc)
            {
                PropertyDataCollection p = mo.Properties;
                NumofCPU =    Convert.ToInt32( p["NumberOfLogicalProcessors"].Value);
            }
        }
        public static ManagementClass  mCPU;
        public static ManagementObjectCollection moc;
        public static int threadcount { get; set; }
    }
}




namespace Qinka.CIntN.ConcurrencyEdition
{
    //这里多线程只优化乘法
    public class CIntN_MT 
        : CIntN.Sample.CIntN
    {
        //变量和属性
        protected int NumofCPU;





        //构造函数
        public CIntN_MT(string input) :base(input)
        {
            SysInfo.Init();

            //获取CPU核数:
            NumofCPU = SysInfo.NumofCPU;
            mThread = new List<Thread>();

        }
        public CIntN_MT(List<byte> data, Sign sg):base(data,sg)
        {
            SysInfo.Init();
            //获取CPU核数:
            NumofCPU = SysInfo.NumofCPU;
            mThread = new List<Thread>();

        }
        /*public override CIntN_MT(CIntN.Sample.CIntN data)
        {
            符号 = data.符号;
            mData = new List<byte>(data.);

        }*/

        //操作符
        public static CIntN_MT operator +(CIntN_MT A, CIntN_MT B)
        {
            Sign sg = new Sign();
            List<byte> rt = new List<byte>();
            List<byte> tmps = new List<byte>();
            if (A.符号 == B.符号)
            {
                byte tmp = 0;
                int minsize = (A.mData.Count < B.mData.Count) ? (A.mData.Count) : (B.mData.Count);
                int maxsize = (A.mData.Count > B.mData.Count) ? (A.mData.Count) : (B.mData.Count);
                for (int i = 0; i < minsize; i++)
                {
                    tmps.Add((byte)((tmp + (byte)A.mData[A.mData.Count - 1 - i] + (byte)B.mData[B.mData.Count - 1 - i]) % 10));
                    tmp = (byte)((tmp + (byte)A.mData[A.mData.Count - 1 - i] + (byte)B.mData[B.mData.Count - 1 - i]) / 10);
                }
                if (A.mData.Count > B.mData.Count)
                {
                    for (int i = minsize; i < maxsize; i++)
                    {
                        tmps.Add((byte)((tmp + A.mData[A.mData.Count - 1 - i]) % 10));
                        tmp = (byte)((tmp + A.mData[A.mData.Count - 1 - i]) / 10);
                    }
                    if (tmp != 0) tmps.Add(tmp);
                }
                else if (A.mData.Count < B.mData.Count)
                {
                    for (int i = minsize; i < maxsize; i++)
                    {
                        tmps.Add((byte)((tmp + B.mData[B.mData.Count - 1 - i]) % 10));
                        tmp = (byte)((tmp + B.mData[B.mData.Count - 1 - i]) / 10);
                    }
                    if (tmp != 0) tmps.Add(tmp);
                }
                else if (tmp != 0) tmps.Add(tmp);
                sg = A.符号;
            }
            else
            {
                SByte tmp = 0;
                int minsize = (A.mData.Count < B.mData.Count) ? (A.mData.Count) : (B.mData.Count);
                int maxsize = (A.mData.Count > B.mData.Count) ? (A.mData.Count) : (B.mData.Count);
                for (int i = 0; i < minsize; i++)
                {
                    SByte tmp2 = (SByte)(tmp + A.mData[A.mData.Count - 1 - i] - B.mData[B.mData.Count - 1 - i]);
                    if (tmp2 < 0 && tmp2 >= -9)
                    {
                        tmp = (SByte)(tmp2 / 10 - 1);
                        tmps.Add((byte)(10 + (tmp2 % 10)));
                    }
                    else
                    {
                        tmp = 0;
                        tmps.Add((byte)tmp2);
                    }
                }
                if (A.mData.Count > B.mData.Count)
                {
                    for (int i = minsize; i < maxsize; i++)
                    {
                        SByte tmp2 = (SByte)(tmp + A.mData[A.mData.Count - 1 - i]);
                        if (tmp2 < 0 && tmp2 >= -9)
                        {
                            tmp = (SByte)(tmp2 / 10 - 1);
                            tmps.Add((byte)(10 + (tmp2 % 10)));
                        }
                        else
                        {
                            tmp = 0;
                            tmps.Add((byte)tmp2);
                        }
                    }
                }
                else if (A.mData.Count < B.mData.Count)
                {
                    for (int i = minsize; i < maxsize; i++)
                    {
                        SByte tmp2 = (SByte)(tmp - B.mData[B.mData.Count - 1 - i]);
                        if (tmp2 < 0 && tmp2 >= -9)
                        {
                            tmp = (SByte)(tmp2 / 10 - 1);
                            tmps.Add((byte)(10 + (tmp2 % 10)));
                        }
                        else
                        {
                            tmp = 0;
                            tmps.Add((byte)tmp2);
                        }
                    }
                }
                else { ; }
                if (A.符号 == Sign.Positive)
                {
                    if (A.绝对值() > B.绝对值()) sg = Sign.Negative;
                    else sg = Sign.Positive;
                }
                else
                {
                    if (A.绝对值() > B.绝对值()) sg = Sign.Positive;
                    else sg = Sign.Negative;
                }
            }
            for (int i = tmps.Count - 1; i >= 0; i--)
            {
                rt.Add(tmps[i]);
            }
            return new CIntN_MT(rt, sg);
        }
        public static CIntN_MT operator -(CIntN_MT a, CIntN_MT b)
        {
            return a + (-b);
        }
        public static CIntN_MT operator -(CIntN_MT a)
        {
            if (a.符号 == Sign.Negative)
                return new CIntN_MT(a.mData, Sign.Positive);
            else
                return new CIntN_MT(a.mData, Sign.Negative);

        }
        /*
         public static CIntN_MT operatortimes (CIntN_MT a, byte b,ref CIntN_MT rt)
        {
            if (b == 0) return new CIntN_MT("0");
            List<byte> tmps = new List<byte>(), rt = new List<byte>();
            byte tmp = 0;
            for (int i = a.mData.Count - 1; i >= 0; i--)
            {
                short tmp2 = (short)(tmp + (a.mData[i]) * b);
                tmps.Add((byte)(tmp2 % 10));
                tmp = (byte)(tmp2 / 10);
            }
            while (tmp != 0)
            {
                tmps.Add((byte)(tmp % 10));
                tmp /= 10;
            }
            for (int i = tmps.Count - 1; i >= 0; i--)
            {
                rt.Add(tmps[i]);
            }
            return new CIntN_MT(rt, a.符号);

        }
         */
        public static   CIntN_MT  operatortimes (CIntN_MT a, byte b,int count)
        {
#if DEBUG

            Thread.CurrentThread.Name = "SDSDSDS";

#endif



            if (b == 0)
            {
                
                lock ((object)SysInfo.threadcount)
                {
                    SysInfo.threadcount--;
                }
                return new CIntN_MT("0");
            }
            List<byte> tmps = new List<byte>(), rt = new List<byte>();
            byte tmp = 0;
            for (int i = a.mData.Count - 1; i >= 0; i--)
            {
                short tmp2 = (short)(tmp + (a.mData[i]) * b);
                tmps.Add((byte)(tmp2 % 10));
                tmp = (byte)(tmp2 / 10);
            }
            while (tmp != 0)
            {
                tmps.Add((byte)(tmp % 10));
                tmp /= 10;
            }
            for (int i = tmps.Count - 1; i >= 0; i--)
            {
                rt.Add(tmps[i]);
            }
            lock ((object)SysInfo.threadcount)
            {
                SysInfo.threadcount--;
            }
            return (new CIntN_MT(rt, a.符号) << count);
        }

        protected delegate CIntN_MT  ATh(CIntN_MT a, byte b,int count);
       
        private class datas
        {
            public ATh a;
            public IAsyncResult i;
        }
        public static CIntN_MT operator *(CIntN_MT a, CIntN_MT b)
        {
            CIntN_MT rt = new CIntN_MT("0");
            int i = (b.mData.Count - 1);
            List<datas> d = new List<datas> ();
            while(i>=0)
            {
                if (SysInfo.threadcount >= (SysInfo.NumofCPU*20)) continue;
                lock ((object)SysInfo.threadcount)
                {
                    SysInfo.threadcount++;
                    datas tmp =new datas();
                    tmp.a =new ATh(operatortimes);
                    tmp.i = tmp.a.BeginInvoke(a, b.mData[i], b.mData.Count - 1 - i, null, null);
                    d.Add(tmp);
                    i--;
                }
            }
            int ic = 0;
            while(true)
            {
                if (d.Count == 0) break;
                if(ic>=d.Count)
                {
                    ic = 0;
                    continue;
                }
                if(d[ic].i.IsCompleted)
                {
                    rt = rt + d[ic].a.EndInvoke(d[ic].i);
                    d.Remove(d[ic]);
                }
                ic++;
            }
            return rt;
        }
        public static bool operator <(CIntN_MT A, CIntN_MT B)
        {
            if (A.符号 == B.符号)
            {
                if (A.符号 == Sign.Positive)
                {
                    if (A.mData.Count < B.mData.Count) return true;
                    else if (A.mData.Count > B.mData.Count) return false;
                    else
                    {
                        for (int i = 0; i < A.mData.Count; i++)
                        {
                            if (A.mData[i] == B.mData[i]) continue;
                            else
                            {
                                if (A.mData[i] < B.mData[i]) return true;
                                else return false;
                            }
                        }
                        return false;
                    }
                }
                else
                {
                    if (A.mData.Count > B.mData.Count) return true;
                    else if (A.mData.Count < B.mData.Count) return false;
                    else
                    {
                        for (int i = 0; i < A.mData.Count; i++)
                        {
                            if (A.mData[i] == B.mData[i]) continue;
                            else
                            {
                                if (A.mData[i] > B.mData[i]) return true;
                                else return false;
                            }
                        }
                        return false;
                    }
                }
            }
            else
            {
                if (A.符号 == Sign.Positive) return false;
                else return true;
            }

        }
        public static bool operator >(CIntN_MT A, CIntN_MT B)
        {
            if (A.符号 == B.符号)
            {
                if (A.符号 == Sign.Positive)
                {
                    if (A.mData.Count > B.mData.Count) return true;
                    else if (A.mData.Count < B.mData.Count) return false;
                    else
                    {
                        for (int i = 0; i < A.mData.Count; i++)
                        {
                            if (A.mData[i] == B.mData[i]) continue;
                            else
                            {
                                if (A.mData[i] > B.mData[i]) return true;
                                else return false;
                            }
                        }
                        return false;
                    }
                }
                else
                {
                    if (A.mData.Count < B.mData.Count) return true;
                    else if (A.mData.Count > B.mData.Count) return false;
                    else
                    {
                        for (int i = 0; i < A.mData.Count; i++)
                        {
                            if (A.mData[i] == B.mData[i]) continue;
                            else
                            {
                                if (A.mData[i] < B.mData[i]) return true;
                                else return false;
                            }
                        }
                        return false;
                    }
                }
            }
            else
            {
                if (A.符号 == Sign.Positive) return true;
                else return false;
            }

        }
        public static bool operator ==(CIntN_MT A, CIntN_MT B)
        {
            if (A.符号 != B.符号) return false;
            else
            {
                if (A.mData.Count != B.mData.Count) return false;
                else
                {
                    for (int i = 0; i < A.mData.Count; i++)
                    {
                        if (A.mData[i] == B.mData[i]) return false;
                    }
                    return true;
                }
            }

        }
        public static bool operator !=(CIntN_MT A, CIntN_MT B)
        {
            if (A.符号 != B.符号) return true;
            else
            {
                if (A.mData.Count != B.mData.Count) return true;
                else
                {
                    for (int i = 0; i < A.mData.Count; i++)
                    {
                        if (A.mData[i] == B.mData[i]) return true;
                    }
                    return false;
                }
            }

        }
        public static CIntN_MT operator <<(CIntN_MT A, int B)
        {
            for (int i = 0; i < B; i++)
            {
                A.mData.Add(0);
            }
            return A;

        }

        //方法
        public new CIntN_MT 绝对值()
        {
            return new CIntN_MT(this.mData, Sign.Positive);
        }
        public new CIntN_MT 阶乘()
        {
            CIntN_MT rt = new CIntN_MT("1"), count = new CIntN_MT("0");
            CIntN_MT one = new CIntN_MT("1");
            while (count < this)
            {
                count = count + one;
                rt = rt * count;
            }
            return rt;

        }
        public override string ToString()
        {
            char[] tmp;
            if (this.符号 == Sign.Negative)
            {

                tmp = new char[mData.Count + 1];
                tmp[0] = '-';
                for (int i = 1; i < mData.Count + 1; i++)
                {
                    tmp[i] = (char)(mData[i - 1] + '0');
                }
            }
            else
            {
                tmp = new char[mData.Count];
                for (int i = 0; i < mData.Count; i++)
                {
                    tmp[i] = (char)(mData[i] + '0');
                }
            }
            return new string(tmp);
        }

        //用于多线程
        protected List<Thread> mThread;
        public delegate CIntN_MT opcTb(CIntN_MT a, byte b);
        


    }
}
