#!/usr/bin/env python3
import serial

THRESHOLD = 35  # Match Arduino threshold

def parse_line(line):
    try:
        parts = line.split('|')
        x = int(parts[0].split(':')[1].strip())
        y = int(parts[1].split(':')[1].strip())
        button = parts[2].split(':')[1].strip() == "True"
        sensor = int(parts[3].split(':')[1].strip().split(' ')[0])  # Strip " -->" if present

        above_threshold = "Above threshold!" in line
        return {
            "x": x,
            "y": y,
            "button": button,
            "sensor": sensor,
            "above_threshold": above_threshold
        }
    except (IndexError, ValueError):
        return None

if __name__ == '__main__':
    ser = serial.Serial('/dev/ttyACM0', 9600, timeout=1)
    ser.reset_input_buffer()

    print("Listening for Arduino joystick and sensor data...\n")
    while True:
        if ser.in_waiting > 0:
            line = ser.readline().decode('utf-8').rstrip()
            data = parse_line(line)
            if data:
                print(f"X = {data['x']}, Y = {data['y']}, Button = {data['button']}, Sensor = {data['sensor']}", end='')
                if data['above_threshold']:
                    print(" --> Above threshold!")
                else:
                    print()
