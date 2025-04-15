const int VRx = A0;         // Joystick X-axis
const int VRy = A1;         // Joystick Y-axis
const int SW  = 2;          // Joystick button (active LOW)
const int piezoPin = A3;    // Analog sensor (e.g., Piezo)
const int PIEZO_THRESHOLD = 40;

void setup() {
  Serial.begin(9600);
  pinMode(SW, INPUT_PULLUP);
}

void loop() {
  int x = analogRead(VRx);
  int y = analogRead(VRy);
  bool buttonPressed = (digitalRead(SW) == LOW);
  int piezo = analogRead(piezoPin);

  // Determine movement command
  String movementCommand = "";

  if (y > 800) movementCommand += "W";       // Forward
  else if (y < 200) movementCommand += "S";  // Backward

  if (x < 200) movementCommand += "A";       // Left
  else if (x > 800) movementCommand += "D";  // Right

  if (movementCommand.length() == 0) movementCommand = "N";  // Neutral

  // Determine jump command based on piezo threshold
  String jumpCommand = (piezo > PIEZO_THRESHOLD) ? "JUMP" : "NO";

  // Print all info to Serial Monitor
  Serial.print("X: "); Serial.print(x);
  Serial.print(" | Y: "); Serial.print(y);
  Serial.print(" | Button: "); Serial.print(buttonPressed);
  Serial.print(" | Piezo: "); Serial.print(piezo);
  Serial.print(" | Movement: "); Serial.print(movementCommand);
  Serial.print(" | Jump: "); Serial.println(jumpCommand);

  delay(200);
}
