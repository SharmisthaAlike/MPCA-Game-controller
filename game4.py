import socket
import serial
import time

# Set up serial connection to Arduino
serial_port = '/dev/ttyACM0'  # Update this if necessary
baud_rate = 9600
ser = serial.Serial(serial_port, baud_rate, timeout=1)

# Set up TCP server
host = '0.0.0.0'  # Listen on all interfaces
port = 5003     # Port used for Unity connection

server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
server_socket.bind((host, port))
server_socket.listen(1)

print(f"[INFO] Server listening on {host}:{port}...")

# Wait for Unity client to connect
client_socket, client_address = server_socket.accept()
print(f"[INFO] Connection established with {client_address}")

try:
    while True:
        # Read incoming serial data
        if ser.in_waiting > 0:
            data = ser.readline().decode('utf-8', errors='ignore').strip()
            if data:
                print(f"[ARDUINO] {data}")

                # Optional: Parse and reformat data here if Unity needs a custom structure
                # For now, forward the raw Arduino output
                client_socket.sendall((data + '\n').encode('utf-8'))

        time.sleep(0.05)

except KeyboardInterrupt:
    print("\n[INFO] Server is shutting down...")

finally:
    client_socket.close()
    server_socket.close()
    ser.close()
    print("[INFO] Cleaned up resources.")
