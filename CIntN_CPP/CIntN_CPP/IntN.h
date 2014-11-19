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
	//��true,��false
	CIntN(vector<char> in, bool ����);
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
	// С��ʮ
	virtual CIntN operator*(char a);
	virtual bool operator<(CIntN a);
	virtual bool operator>(CIntN a);
	virtual CIntN �׳�();
	virtual CIntN ����ֵ();
	virtual CIntN ����ȡ��();
	//�����true,�����false
	bool ����()const;
	virtual CIntN operator>>(int in);
protected:
	
	vector<char> m_data;
	bool is_����;
};

