import socket
import serial

# Serial setup (check your port with `ls /dev/tty*`)
ser = serial.Serial('/dev/ttyACM0', 9600)

# TCP socket setup
HOST = ''  # Listen on all interfaces
PORT = 65431

server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
server_socket.bind((HOST, PORT))
server_socket.listen(1)

print("Waiting for client connection...")
conn, addr = server_socket.accept()
print("Connected by", addr)

try:
    while True:
        if ser.in_waiting:
            data = ser.readline().decode().strip()
            print("Sending:", data)
            conn.sendall((data + '\n').encode())
except KeyboardInterrupt:
    print("Exiting...")
finally:
    conn.close()
    server_socket.close()
    ser.close()
