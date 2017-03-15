#include <HarvbotRangefinder.h>

HarvbotRangefinder* rangefiner;

void setup() 
{
  Serial.begin(9600);
  rangefiner = new HarvbotRangefinder(&Serial1);
  rangefiner->start();
}

void loop() 
{
  float distance = rangefiner->read();
  Serial.println(String("Distance: ") + distance);
  delay(1000);
}
