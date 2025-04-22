# server_pi.py
import socket
import serial

# Set up serial (adjust based on your port and baud rate)
ser = serial.Serial('/dev/ttyACM0', 9600)

# TCP socket setup
HOST = ''  # Listen on all interfaces
PORT = 65431
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind((HOST, PORT))
server_socket.listen(1)

print("Waiting for client...")
conn, addr = server_socket.accept()
print("Connected to:", addr)

try:
    while True:
        if ser.in_waiting > 0:
            data = ser.readline().decode().strip()
            print("Sending:", data)
            conn.sendall(data.encode() + b'\n')
except KeyboardInterrupt:
    print("Exiting")
finally:
    conn.close()
    server_socket.close()
    ser.close()
