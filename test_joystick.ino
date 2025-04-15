const int VRx = A0;         // Joystick X-axis
const int VRy = A1;         // Joystick Y-axis
const int SW  = 2;          // Joystick button (active LOW)
const int sensorPin = A3;   // Additional analog sensor
const int THRESHOLD = 35;  // Sensor threshold

void setup() {
  Serial.begin(9600);
  pinMode(SW, INPUT_PULLUP); // Joystick button is active LOW
}

void loop() {
  int xVal = analogRead(VRx);
  int yVal = analogRead(VRy);
  bool buttonPressed = (digitalRead(SW) == LOW);
  int sensorValue = analogRead(sensorPin);  // Read additional sensor

  // Print all values to Serial Monitor
  Serial.print("X: "); Serial.print(xVal);
  Serial.print(" | Y: "); Serial.print(yVal);
  Serial.print(" | Button: "); Serial.print(buttonPressed);
  Serial.print(" | Sensor (A3): "); Serial.print(sensorValue);

  // Check threshold
  if (sensorValue >= THRESHOLD) {
    Serial.print(" --> Above threshold!");
  }

  Serial.println();  // Newline after each loop
  delay(200);
}