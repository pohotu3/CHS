from datetime import datetime
from os.path import expanduser
from http.server import BaseHTTPRequestHandler, HTTPServer
import os, threading, sys, os.path

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

log = Log() # create my log object

class HTTPRequestHandler(BaseHTTPRequestHandler):
    #handle GET command
    def do_GET(self):
        try:
            rootdir = expanduser("~") + "/CrystalHomeSys/Heart" #file location
            f = open(rootdir + self.path) #open requested file
            
            log.write("GET " + self.path + " requested")
            
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
            log.write("GET requested " + self.path + ". 404 Not Found")
            self.send_error(404, 'File not found')
            
    def do_PUT(self):
        rootdir = expanduser("~") + "/CrystalHomeSys/Heart" #file location
        try:
            content_len = int(self.headers['Content-Length'])
            post_body = self.rfile.read(content_len)
            post_body = post_body.decode("utf-8")
            
            if not os.path.isfile(rootdir + self.path):
                self.send_response(201)
                log.write("PUT " + post_body + " to " + self.path + ". File was created and updated.")
            else:
                self.send_response(204)
                log.write("PUT " + post_body + " to " + self.path + ". File updated.")

            f = open(rootdir + self.path, "a")
            
            self.send_header('Content-type','text-html')
            self.end_headers()
            
            f.write(post_body + "\n")
            f.close()
            return
        
        except IOError:
            log.write("PUT requested " + self.path + ". 404 Not Found")
            self.send_error(404, "File not found")
        return
    
    def do_POST(self):
        rootdir = expanduser("~") + "/CrystalHomeSys/Heart" #file location
        try:
            content_len = int(self.headers['Content-Length'])
            post_body = self.rfile.read(content_len)
            post_body = post_body.decode("utf-8")
            
            if not os.path.exists(rootdir + self.path):
                os.makedirs(rootdir + self.path)
            
            f = open(rootdir + self.path + post_body, "a")
            f.close()
            
            log.write("POST requested " + self.path + "" + post_body)
            
            self.send_response(201)
            
            self.send_header('Content-type','text-html')
            self.end_headers()
            
            self.wfile.write(bytes("Created new file " + self.path + "" + post_body, "UTF-8"))
            return
        except IOError:
            log.write("POST requested " + self.path + "" + post_body + ". 404 Not Found")
            self.send_error(404, "File not found")
        return
    
    def do_DELETE(self):
        rootdir = expanduser("~") + "/CrystalHomeSys/Heart" #file location
        try:
            os.remove(rootdir + self.path)
            log.write("DELETE requested " + self.path)
            self.send_response(204)
            
            self.send_header('Content-type','text-html')
            self.end_headers()
            return
        except IOError:
            log.write("DELETE requested " + self.path + ". 404 Not Found")
            self.send_error(404, "File not found")

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