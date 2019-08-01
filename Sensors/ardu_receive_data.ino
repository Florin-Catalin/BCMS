#include <Wire.h> 
#include <LiquidCrystal_I2C.h>   
//hope I remember the right I2C bus address
LiquidCrystal_I2C lcd(0x5F, 2, 1, 0, 4, 5, 6, 7, 3, POSITIVE);

String input = "";
String stringRec = "";
String pos = "";
int posX = 0;
int posY = 0;

void setup() {
   
    lcd.begin(16,2);             // 16 characters, 2 rows
    lcd.clear();
    
    Serial.begin(9600);
    Serial.setTimeout(500);
}

void loop() {
  if(Serial.available() > 0){
    input = Serial.readString();
    pos = input.substring(0,2);
    posX = pos.substring(0,1).toInt();
    posY = pos.substring(1,2).toInt();
    stringRec = input.substring(2); 
    /*Serial.println("String: ");
    Serial.println(stringRec);
    Serial.println();
    Serial.println("Pos: ");
    Serial.print(posX);
    Serial.print(" ");
    Serial.print(posY);
    Serial.println();*/
  }
  if(stringRec.equals("clr")){
    lcd.clear();
    stringRec = "";    
  }
  lcd.setCursor(posX,posY);
  lcd.print(stringRec); 
}
