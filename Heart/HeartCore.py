from datetime import datetime
from os.path import expanduser
from http.server import BaseHTTPRequestHandler, HTTPServer
import os, threading, sys

class Log:
    now = datetime.now()
    fileName = str(now.month) + "-" + str(now.day) + "-" + str(now.year)
    home = expanduser("~")
    fileDir = home + "/CrystalHomeSys/Heart/Logs/" + fileName + ".txt"
    logFile = open(fileDir, "a")
    
    def __init__(self):
        self.write("-------------SYSTEM STARTUP----------------")
        self.write("Welcome to Crystal Home Systems")
        self.write("Created Log file at " + self.fileDir)
        return
    
    def write(self, s):
        print(s)
        self.logFile.write(str(self.now.hour) + ":" + str(self.now.minute) + ":" + str(self.now.second) + ">>> " + s + "\n")
        return
    
class HTTPRequestHandler(BaseHTTPRequestHandler):
    #handle GET command
    def do_GET(self):
        rootdir = expanduser("~") + "/CrystalHomeSys/Heart" #file location
        try:
            print(rootdir + self.path)
            f = open(rootdir + self.path) #open requested file
            
            #send code 200 response
            self.send_response(200)
            
            #send header first
            self.send_header('Content-type','text-html')
            self.end_headers()
            
            #send file content to client
            self.wfile.write(bytes(f.read(), "UTF-8"))
            f.close()
            return
      
        except IOError:
            self.send_error(404, 'File not found')

log = Log() # create my log object
#ip and port of server
#by default http server port is 80
ip = 'localhost'
port = 80
port_works = False
while port_works == False:
    try:
        server_address = (ip, port)
        httpd = HTTPServer(server_address, HTTPRequestHandler)
        port_works = True
    except:
        port = port + 1

def listenForClose():
    running = True
    while running:
        keystroke = input()
        if keystroke == "k":
            running = False
            log.write("Closing server...")
            os._exit(0)
            return

close_thread = threading.Thread(target=listenForClose)
close_thread.daemon = True
close_thread.start()

log.write("Starting HTTP server on ip " + ip + " port " + str(port))
log.write('HTTP server is running... Press \'k\' to quit.')
httpd.serve_forever()