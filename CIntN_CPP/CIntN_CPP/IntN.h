#pragma once
#include"CIntN_CPP.h"
#include<vector>
#include <iostream>
#include<string>
using namespace std;
 class CINTN_CPP_API CIntN
{
public:
	CIntN(string in);
	//正true,富false
	CIntN(vector<char> in, bool 符号);
	//string ToString();
	~CIntN();
	virtual CIntN operator+(CIntN a);
	virtual CIntN operator-(CIntN a);
	virtual CIntN operator*(CIntN a);
	friend  ostream& operator <<(ostream& os, const CIntN& ms)
	{
		for (int i = 0; i < ms.m_data.size(); i++)
		{
			os << (int)(ms.m_data[i]);
		}
		return os;
	}
	// 小于十
	virtual CIntN operator*(char a);
	virtual bool operator<(CIntN a);
	virtual bool operator>(CIntN a);
	virtual CIntN 阶乘();
	virtual CIntN 绝对值();
	virtual CIntN 符号取反();
	//正输出true,富输出false
	bool 正负()const;
	virtual CIntN operator>>(int in);
protected:
	
	vector<char> m_data;
	bool is_负数;
};

