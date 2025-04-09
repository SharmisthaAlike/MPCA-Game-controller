const int VRx = A0;
const int VRy = A1;
const int SW = 2;

void setup() {
  Serial.begin(9600);
  pinMode(SW, INPUT_PULLUP); // Joystick button is active LOW
}

void loop() {
  int xVal = analogRead(VRx);
  int yVal = analogRead(VRy);
  bool buttonPressed = digitalRead(SW) == LOW;

  Serial.print("X: "); Serial.print(xVal);
  Serial.print(" | Y: "); Serial.print(yVal);
  Serial.print(" | Button: "); Serial.println(buttonPressed);

  delay(100);
}