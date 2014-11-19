#include "stdafx.h"
#include "IntN.h"



CIntN::CIntN(string in)
{
	char loopstop = 0;
	if (in.c_str()[0] == '+')
	{
		is_���� = false;
		loopstop++;
	}
	else if (in.c_str()[0] == '-')
	{
		is_���� = true;
		loopstop++;
	}
	else
	{
		is_���� = false;
	}
	for (unsigned int i = loopstop; i < in.length(); i++)
	{
		m_data.push_back(in.c_str()[i]-'0');
	}
}
bool CIntN::����() const
{
	return !is_����;
}


bool CIntN::operator<(CIntN a)
{
	if (this->is_���� == a.is_����)
	{
		if (this->is_���� == false)
		{
			if (this->m_data.size() < a.m_data.size()) return true;
			else if (this->m_data.size() > a.m_data.size()) return false;
			else
			{
				for (unsigned int i = 0; i < m_data.size(); i++)
				{
					if (this->m_data[i] == a.m_data[i]) continue;
					else
					{
						if (this->m_data[i] < a.m_data[i]) return true;
						else return false;
					}
				}
				return false;
			}
		}
		else
		{
			if (this->m_data.size() > a.m_data.size()) return true;
			else if (this->m_data.size() < a.m_data.size()) return false;
			else
			{
				for (unsigned int i = 0; i < m_data.size(); i++)
				{
					if (this->m_data[i] == a.m_data[i]) continue;
					else
					{
						if (this->m_data[i] > a.m_data[i]) return true;
						else return false;
					}
				}
				return false;
			}
		}
	}
	else
	{
		if (this->is_���� == false)
			return false;
		else return true;
	}
}
bool CIntN::operator>(CIntN a)
{
	if (this->is_���� == a.is_����)
	{
		if (this->is_���� == false)
		{
			if (this->m_data.size() > a.m_data.size()) return true;
			else if (this->m_data.size() < a.m_data.size()) return false;
			else
			{
				for (unsigned int i = 0; i < m_data.size(); i++)
				{
					if (this->m_data[i] == a.m_data[i]) continue;
					else
					{
						if (this->m_data[i] > a.m_data[i]) return true;
						else return false;
					}
				}
				return false;
			}
		}
		else
		{
			if (this->m_data.size() < a.m_data.size()) return true;
			else if (this->m_data.size() > a.m_data.size()) return false;
			else
			{
				for (unsigned int i = 0; i < m_data.size(); i++)
				{
					if (this->m_data[i] == a.m_data[i]) continue;
					else
					{
						if (this->m_data[i] < a.m_data[i]) return true;
						else return false;
					}
				}
				return false;
			}
		}
	}
	else
	{
		if (this->is_���� == false)
			return true;
		else return false;
	}
}

CIntN CIntN::����ֵ()
{
	return CIntN(this->m_data, true);
}

CIntN::CIntN(vector<char> in,bool ����)
{
	m_data = vector<char>(in);
	is_���� = !����;
}

CIntN CIntN::operator+(CIntN a)
{
	bool ����;
	vector<char> rt = vector<char>(), tmps = vector<char>();
	if (a.is_���� == this->is_����)
	{

		char tmp = 0;
		long long minsize = min(this->m_data.size(), a.m_data.size());
		long long maxsize = max(this->m_data.size(), a.m_data.size());

		for (unsigned int i = 0; i < minsize; i++)
		{
			tmps.push_back((tmp + m_data[m_data.size() - 1 - i] + a.m_data[a.m_data.size() - 1 - i]) % 10);
			tmp = (tmp + m_data[m_data.size() - 1 - i] + a.m_data[a.m_data.size() - 1 - i]) / 10;
		}
		if (this->m_data.size() > a.m_data.size())
		{
			for (long long i = minsize; i < maxsize; i++)
			{
				tmps.push_back((tmp + m_data[m_data.size() - 1 - i] ) % 10);
				         tmp = (tmp + m_data[m_data.size() - 1 - i] ) / 10;
			}
			if (tmp)
				tmps.push_back(tmp);
		}
		else if (this->m_data.size() < a.m_data.size())
		{
			for (long long i = minsize; i < maxsize; i++)
			{
				tmps.push_back((tmp + a.m_data[a.m_data.size() - 1 - i]) % 10);
				         tmp = (tmp + a.m_data[a.m_data.size() - 1 - i]) / 10;
			}
			if (tmp)
				tmps.push_back(tmp);
		}
		else
		{
			if (tmp)
				tmps.push_back(tmp);
		}
		���� = !this->is_����;
		
	}
	else
	{
		char tmp = 0;
		long long minsize = min(this->m_data.size(), a.m_data.size());
		long long maxsize = max(this->m_data.size(), a.m_data.size());

		for (unsigned int i = 0; i < minsize; i++)
		{
			char tmp2 = (tmp + m_data[m_data.size() - 1 - i] - a.m_data[a.m_data.size() - 1 - i]);
			if (tmp2 < 0 && tmp2>=-9)
			{
				tmp = tmp2 / 10 - 1;
				tmps.push_back(10 + (tmp2%10));
			}
			else
			{
				tmp = 0;
				tmps.push_back(tmp2);
			}
			
		}
		if (this->m_data.size() > a.m_data.size())
		{
			for (long long i = minsize; i < maxsize; i++)
			{
				char tmp2 = (tmp + m_data[m_data.size() - 1 - i] );
				if (tmp2 < 0 && tmp2 >= -9)
				{
					tmp = tmp2 / 10 - 1;
					tmps.push_back(10 - (tmp2 % 10));
				}
				else
				{
					tmp = 0;
					tmps.push_back(tmp2);
				}

			}
		}
		else if (this->m_data.size() < a.m_data.size())
		{
			for (long long i = minsize; i < maxsize; i++)
			{
				char tmp2 = (tmp - a.m_data[a.m_data.size() - 1 - i]);
				if (tmp2 < 0 && tmp2 >= -9)
				{
					tmp = tmp2 / 10 - 1;
					tmps.push_back(10 - (tmp2 % 10));
				}
				else
				{
					tmp = 0;
					tmps.push_back(tmp2);
				}

			}
		}
		else
		{

		}
		if (this->is_���� == false)
		{
			if (this->����ֵ() < a.����ֵ())  ���� = false;
			else if (this->����ֵ() > a.����ֵ()) ���� = true;
			else  ���� = false;
		}
		else
		{
			if (this->����ֵ() < a.����ֵ())  ���� = true;
			else if (this->����ֵ() > a.����ֵ()) ���� = false;
			else ���� = true;
		}
	}
	for (long long i = tmps.size() - 1; i >= 0; i--)
		rt.push_back(tmps[i]);
	return CIntN(rt, ����);
}

CIntN CIntN::����ȡ��()
{
	return CIntN(this->m_data, is_����);
}

CIntN CIntN::operator-(CIntN a)
{
	return (*this) + a.����ȡ��();
}
CIntN CIntN::operator*(char a)
{
	bool ����;
	if (!this->is_���� == (a > 0)) ���� = true;
	else if (0 == a) return CIntN(vector<char>(0), true);
	else ���� = false;
	a = abs(a);

	vector<char> tmps,rt;
	char tmp = 0;
	for (long long i = this->m_data.size() - 1; i >= 0; i--)
	{
		short tmp2 = tmp + (m_data[i]) *a;
		tmps.push_back(tmp2 % 10);
		tmp = tmp2 / 10;
	}
	while (tmp)
	{
		tmps.push_back(tmp % 10);
		tmp /= 10;
	}
	for (long long i = tmps.size() - 1; i >= 0; i--) rt.push_back(tmps[i]);

	return CIntN(rt, ����);
}
CIntN CIntN::operator>>(int in)
{
	CIntN rt = CIntN(*this);
	for (int i = 0; i < in; i++) rt.m_data.push_back(0);
	return rt;
}

CIntN CIntN::operator*(CIntN in)
{
	CIntN rt = "0";
	for (long long i = in.m_data.size() - 1; i >= 0; i--)
	{
		rt = rt + (((*this)*in.m_data[i]) >> (in.m_data.size() - (long long)1 - i));
	}
	return rt;
}

CIntN CIntN::�׳�()
{
	CIntN rt = "1",count="0";
	while (count < *this  )
	{
		
		count = count + CIntN("1");
		rt = rt *count;
	}
	return rt;
}





CIntN::~CIntN()
{

}
