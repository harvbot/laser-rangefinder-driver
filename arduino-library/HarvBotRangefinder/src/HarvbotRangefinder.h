#ifndef HarvbotRangefinder_H_
#define HarvbotRangefinder_H_

#include <Arduino.h>

#define HarvbotRangefinder_BaudRate 9600
#define HarvbotRangefinder_DefaultAddr 0x80

class HarvbotRangefinder {
	private: 
		// Serial port reference.
		HardwareSerial* m_Serial;
		
	public:
		HarvbotRangefinder(HardwareSerial* serial);
		~HarvbotRangefinder();
		void start();
		float read();
		void stop();
};

#endif /* HarvbotRangefinder_H_ */