using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qinka.CIntN.Sample
{
    public enum Sign  {Positive,Negative}
    public class CIntN
    {
        //变量和属性
        protected List<byte> mData;
        public Sign 符号 { get ;private set; }

        //构造函数
        public CIntN(string input)
        {
            int nextcur = 0;
            if(0==input.IndexOf('-'))
            {
                符号 = Sign.Negative;
                nextcur++;
            }
            else if (0 == input.IndexOf('+'))
            {
                符号 = Sign.Positive;
                nextcur++;
            }
            else
            {
                符号 = Sign.Positive;
            }
            mData = new List<byte>();

            char[] tmp = input.ToCharArray(nextcur, input.Length - nextcur);
            for (int i = 0; i < tmp.Length;i++ )
            {
                    mData.Add((byte)(tmp[i]-'0'));
            }            
        }
        public CIntN(List<byte> data, Sign sg)
        {
            符号 = sg;
            mData = new List<byte>(data);
        }


        //操作符
        public static CIntN  operator+(CIntN addA,CIntN addB)
        {
            Sign sg = new Sign();
            List<byte>  rt = new List<byte> ();
            List<byte>  tmps = new List<byte> ();
            if(addA.符号==addB.符号)
            {
                byte tmp = 0;
                int minsize = (addA.mData.Count < addB.mData.Count) ? (addA.mData.Count) : (addB.mData.Count);
                int maxsize = (addA.mData.Count > addB.mData.Count) ? (addA.mData.Count) : (addB.mData.Count);
                for(int i=0;i<minsize;i++)
                {
                    tmps.Add( (byte)(( tmp + (byte)addA.mData[addA.mData.Count - 1 - i] + (byte)addB.mData[addB.mData.Count - 1 - i] ) % 10));
                    tmp = (byte)((tmp + (byte)addA.mData[addA.mData.Count - 1 - i] + (byte)addB.mData[addB.mData.Count - 1 - i]) / 10);
                }
                if(addA.mData.Count>addB.mData.Count)
                {
                    for(int i=minsize ;i<maxsize ;i++)
                    {
                        tmps.Add((byte)((tmp + addA.mData[addA.mData.Count - 1 - i]) % 10));
                        tmp = (byte)((tmp + addA.mData[addA.mData.Count - 1 - i]) / 10);
                    }
                    if (tmp != 0) tmps.Add(tmp);
                }
                else if(addA.mData.Count<addB.mData.Count)
                {
                    for (int i = minsize; i < maxsize; i++)
                    {
                        tmps.Add((byte)((tmp + addB.mData[addB.mData.Count - 1 - i]) % 10));
                        tmp = (byte)((tmp + addB.mData[addB.mData.Count - 1 - i]) / 10);
                    }
                    if (tmp != 0) tmps.Add(tmp);
                }
                else if (tmp != 0) tmps.Add(tmp);
                sg = addA.符号;
            }
            else
            {
               SByte tmp = 0;
                int minsize = (addA.mData.Count < addB.mData.Count) ? (addA.mData.Count) : (addB.mData.Count);
                int maxsize = (addA.mData.Count > addB.mData.Count) ? (addA.mData.Count) : (addB.mData.Count);
                for(int i=0;i<minsize ;i++)
                {
                    SByte tmp2 = (SByte)(tmp + addA.mData[addA.mData.Count - 1 - i] - addB.mData[addB.mData.Count - 1 - i]);
                    if(tmp2<0&&tmp2>=-9)
                    {
                        tmp = (SByte)(tmp2 / 10 - 1);
                        tmps.Add((byte)(10+(tmp2%10)));
                    }
                    else
                    {
                        tmp = 0;
                        tmps.Add((byte)tmp2);
                    }
                }
                if (addA.mData.Count > addB.mData.Count)
                {
                    for (int i = minsize; i < maxsize; i++)
                    {
                        SByte tmp2 = (SByte)(tmp + addA.mData[addA.mData.Count - 1 - i]);
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
                else if (addA.mData.Count < addB.mData.Count)
                {
                    for (int i = minsize; i < maxsize; i++)
                    {
                        SByte tmp2 = (SByte)(tmp - addB.mData[addB.mData.Count - 1 - i]);
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
                if(addA.符号 == Sign.Positive)
                {
                    if (addA.绝对值() > addB.绝对值()) sg = Sign.Negative;
                    else sg = Sign.Positive;
                }
                else
                {
                    if (addA.绝对值() > addB.绝对值()) sg = Sign.Positive;
                    else sg = Sign.Negative;
                }
            }
            for(int i = tmps.Count-1;i>=0;i--)
            {
                rt.Add(tmps[i]);
            }
            return new CIntN(rt, sg);
        }
        public static  CIntN operator -(CIntN a)
        {
            if (a.符号 == Sign.Negative)
                return new CIntN(a.mData, Sign.Positive);
            else
                return new CIntN(a.mData, Sign.Negative);
        }
        public static  CIntN operator-(CIntN subA,CIntN subB)
        {
            return subA + (-subB);
        }
        public static  CIntN operator*(CIntN timesA,byte timeB)
        {
            if (timeB == 0) return new CIntN("0");
            List<byte> tmps = new List<byte>(), rt = new List<byte>();
            byte tmp = 0;
            for(int i= timesA.mData.Count-1;i>=0;i--)
            {
                short tmp2 = (short)(tmp + (timesA.mData[i]) * timeB);
                tmps.Add((byte)(tmp2 % 10));
                tmp = (byte)(tmp2 / 10);
            }
            while(tmp != 0)
            {
                tmps.Add((byte)(tmp % 10));
                tmp /= 10;
            }
            for (int i = tmps.Count - 1; i >= 0; i--)
            {
                rt.Add(tmps[i]);
            }
            return new CIntN(rt, timesA.符号);
        }
        public static  CIntN operator *(CIntN timesA, CIntN timeB)
        {
            CIntN rt = new CIntN("0");
            for(int i =timeB.mData.Count-1;i>=0;i-- )
            {
                rt = rt +( (timesA * timeB.mData[i]) << (timeB.mData.Count - 1 - i));
            }
            return rt;
        }
        public static  bool operator <(CIntN A, CIntN B)
        {
            if(A.符号==B.符号)
            {
                if(A.符号 == Sign.Positive)
                {
                    if (A.mData.Count < B.mData.Count) return true;
                    else if (A.mData.Count > B.mData.Count) return false;
                    else
                    {
                        for(int i=0;i<A.mData.Count;i++)
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
        public static  bool operator >(CIntN A, CIntN B)
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
        public static  bool operator ==(CIntN A, CIntN B)
        {
            if (A.符号 != B.符号) return false;
            else
            {
                if (A.mData.Count != B.mData.Count) return false;
                else
                {
                    for(int i =0 ; i< A.mData.Count;i++)
                    {
                        if (A.mData[i] == B.mData[i]) return false;
                    }
                    return true;
                }
            }
        }
        public static  bool operator !=(CIntN A, CIntN B)
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
        public static  CIntN operator <<(CIntN A, int B)
        {
            for (int i = 0; i < B; i++)
            {
                A.mData.Add(0);
            }
            return A;
        }

        //方法
        public  CIntN 绝对值()
        {
            return new CIntN(this.mData, Sign.Positive);
        }
        public  CIntN 阶乘()
        {
            CIntN rt = new CIntN("1"), count = new CIntN("0");
            CIntN one = new CIntN("1");
            while(count <this)
            {
                count = count + one;
                rt = rt * count;
            }
            return rt;
        }
        public override  string ToString()
        {
            char[] tmp;
            if (this.符号 == Sign.Negative)
            {
                
                tmp = new char[mData.Count + 1];
                tmp[0] = '-';
                for (int i = 1; i < mData.Count+1; i++)
                {
                    tmp[i] = (char)(mData[i-1] + '0');
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
    }
}
