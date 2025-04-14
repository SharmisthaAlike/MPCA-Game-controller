void setup() {
    Serial.begin(9600); // match baud rate with Unity
  }
  
  void loop() {
    int x = analogRead(A0); // Joystick X
    int y = analogRead(A1); // Joystick Y
  
    Serial.print(x);
    Serial.print(",");
    Serial.println(y);
  
    delay(50); // 20 readings per second
  }
  