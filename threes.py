import serial
import time

# Set your Arduino's serial port
# Use 'ls /dev/tty*' or 'dmesg | grep tty' to confirm if it's ttyACM0 or ttyUSB0
serial_port = '/dev/ttyACM0'
baud_rate = 9600

try:
    print(f"[INFO] Connecting to Arduino on {serial_port} at {baud_rate} baud...")
    ser = serial.Serial(serial_port, baud_rate, timeout=1)
    time.sleep(2)  # Allow time for Arduino to reset

    print("[INFO] Connected. Reading sensor data:\n")

    while True:
        if ser.in_waiting > 0:
            line = ser.readline().decode('utf-8', errors='ignore').strip()
            if line:
                print(f"[ARDUINO] {line}")

        time.sleep(0.1)

except serial.SerialException as e:
    print(f"[ERROR] Serial connection failed: {e}")

except KeyboardInterrupt:
    print("\n[INFO] Exiting gracefully...")

finally:
    if 'ser' in locals() and ser.is_open:
        ser.close()
        print("[INFO] Serial port closed.")
