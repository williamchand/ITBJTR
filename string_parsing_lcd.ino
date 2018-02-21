//library
#include <LiquidCrystal.h>

//const
const int ledpin[] = {4,5,6,7};
const int ledcount = sizeof(ledpin)/sizeof(int);

//variable
LiquidCrystal lcd(13, 12,11, 10, 9, 8);
String PCin,PCout,subPC;
bool enable;
int panjang;
int show;
void setup() 
{
  // put your setup code here, to run once:
  // set up the LCD's number of columns and rows:
  lcd.begin(16, 2);
  // initialize the serial communications:
  Serial.begin(9600);
  //initialize pin
  for (int i=0;i<ledcount;i++) 
   {
    pinMode(ledpin[i],OUTPUT);
   }
}

void reading() 
{
  enable = Serial.available()>0;
  if (enable) 
  {
    PCin=Serial.readString();
    PCout=PCin;
    lcd.clear();
  }
}

void process()
{
    if (PCout=="LED1 ON")
    {
      show=1;
    }
    else if(PCout=="LED2 ON")
    {
      show=2;
    }
    else if(PCout=="LED3 ON")
    {
      show=3;
    }
    else if(PCout=="LED4 ON")
    {
      show=4;
    }
    else if (PCout=="LED1 OFF")
    {
      show=5;
    }
    else if(PCout=="LED2 OFF")
    {
      show=6;
    }
    else if(PCout=="LED3 OFF")
    {
      show=7;
    }
    else if(PCout=="LED4 OFF")
    {
      show=8;
    }
    else
    {
      show=9;
    }
}

void writing()
{
  if (enable)
  {
    panjang=PCin.length();
    panjang=panjang;//antiglitch
    if (panjang<=16) {
      lcd.setCursor(0,0);
      lcd.print(PCout.substring(0,panjang));
    }
    else
    {
      lcd.setCursor(0,0);
      lcd.print(PCout.substring(0,16));
      lcd.setCursor(0,1);
      lcd.print(PCout.substring(16,panjang));
    }
    Serial.print(PCout);
  }
  subPC=PCout.substring(0,16);
}

void LED1ON()
{
  digitalWrite(ledpin[0],HIGH);
}

void LED2ON()
{
  digitalWrite(ledpin[1],HIGH);
}

void LED3ON()
{
  digitalWrite(ledpin[2],HIGH);
}

void LED4ON()
{
  digitalWrite(ledpin[3],HIGH);
}

void LED1OFF()
{
  digitalWrite(ledpin[0],LOW);
}

void LED2OFF()
{
  digitalWrite(ledpin[1],LOW);
}

void LED3OFF()
{
  digitalWrite(ledpin[2],LOW);
}

void LED4OFF()
{
  digitalWrite(ledpin[3],LOW);
}

void loop()
{
  reading();
  process();
  if (show==1) 
  {
    LED1ON();
  }
  else if (show==2)
  {
    LED2ON();
  }
  else if (show==3)
  {
    LED3ON();
  }
  else if (show==4)
  {
    LED4ON();
  }
  else if (show==5)
  {
    LED1OFF();
  }
  else if (show==6)
  {
    LED2OFF();
  }
  else if (show==7)
  {
    LED3OFF();
  }
  else if (show==8)
  {
    LED4OFF();
  }
  else if (show==9)
  {
    writing();
  }
}
