#include "HarvbotRangefinder.h"

HarvbotRangefinder::HarvbotRangefinder(HardwareSerial* serial)
{
	this->m_Serial=serial;
	this->m_Serial->begin(HarvbotRangefinder_BaudRate);
}

HarvbotRangefinder::~HarvbotRangefinder()
{
}

float HarvbotRangefinder::read() 
{
	byte* buffer = new byte[11];
	int read = this->m_Serial->readBytes(buffer, 11);

	float result = -1;
	if (read == 11)
	{
		if (buffer[0] == HarvbotRangefinder_DefaultAddr && 
			buffer[1] == 0x06 && buffer[2] == 0x83 && buffer[6] == 0x2E)
		{
			result = 0;

			result += (buffer[3] - 48) * 100;
			result += (buffer[4] - 48) * 10;
			result += (buffer[5] - 48);

			result += (buffer[7] - 48) * 0.1;
			result += (buffer[8] - 48) * 0.01;
			result += (buffer[9] - 48) * 0.001;
		}
	}
	
	delete buffer;
	
	return result;
}

void HarvbotRangefinder::start() 
{
	byte command[] = {HarvbotRangefinder_DefaultAddr, 0x06, 0x03, 0x77};

	this->m_Serial->write(command, 4);
}

void HarvbotRangefinder::stop() 
{
	byte command[] = {HarvbotRangefinder_DefaultAddr, 0x04, 0x02, 0x7A};

	this->m_Serial->write(command, 4);
}