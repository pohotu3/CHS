'''
Crystal Home Systems 
Created by Austin and Ezra
Open Source with Related GitHub Repo
UNDER DEVELOPMENT

Copyright© 2015 Austin VanAlstyne, Bailey Thorson

This file is part of Cyrstal Home Systems.

Cyrstal Home Systems is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Cyrstal Home Systems is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Cyrstal Home Systems.  If not, see <http://www.gnu.org/licenses/>.
'''

from datetime import datetime
from os.path import expanduser
from http.server import BaseHTTPRequestHandler, HTTPServer
from os import path
import os, threading, sys, os.path

class HTTPRequestHandler(BaseHTTPRequestHandler):
    def log_request(self, code='-', size='-'):
        return
    
    #handle GET command
    def do_GET(self):
        try:
            rootdir = expanduser("~") + "/CrystalHomeSys/Heart" #file location
            
            print("PYTHON API: " + "GET " + self.path + " requested")
            sys.stdout.flush()
            
            if os.path.isfile(rootdir + self.path):
                f = open(rootdir + self.path) #open requested file
                self.send_response(200)
                self.send_header('Content-type','text-html')
                self.end_headers()
                self.wfile.write(bytes(f.read(), "UTF-8"))
                f.close()
            else:
                file_paths = []  # List which will store all of the full filepaths.
                used_files = []

                # Walk the tree.
                for root, directories, files in os.walk(rootdir + self.path):
                    for filename in files:
                        # Join the two strings in order to form the full filepath.
                        filepath = os.path.join(root, filename)
                        file_paths.append(filepath)  # Add it to the list.
                
                for f in file_paths:
                    temp = f.rsplit('/', 1)
                    used_files.append(temp[1])
                
                self.send_response(200)
                self.send_header('Content-type','text-html')
                self.end_headers()
                self.wfile.write(bytes('\n'.join(used_files), "UTF-8"))
                
            return
      
        except IOError:
            print("PYTHON API: " + "GET requested " + self.path + ". 404 Not Found")
            sys.stdout.flush()
            self.send_error(404, 'File not found')
            
    def do_PUT(self):
        rootdir = expanduser("~") + "/CrystalHomeSys/Heart" #file location
        try:
            content_len = int(self.headers['Content-Length'])
            post_body = self.rfile.read(content_len)
            post_body = post_body.decode("utf-8")
            
            if not os.path.isfile(rootdir + self.path):
                self.send_response(201)
                print("PYTHON API: " + "PUT " + post_body + " to " + self.path + ". File was created and updated.")
                sys.stdout.flush()
            else:
                self.send_response(204)
                print("PYTHON API: " + "PUT " + post_body + " to " + self.path + ". File updated.")
                sys.stdout.flush()
            f = open(rootdir + self.path, "a")
            
            self.send_header('Content-type','text-html')
            self.end_headers()
            
            f.write(post_body + "\n")
            f.close()
            return
        
        except IOError:
            print("PYTHON API: " + "PUT requested " + self.path + ". 404 Not Found")
            sys.stdout.flush()
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
            
            print("PYTHON API: " + "POST requested " + self.path + "" + post_body)
            sys.stdout.flush()
            
            self.send_response(201)
            
            self.send_header('Content-type','text-html')
            self.end_headers()
            
            self.wfile.write(bytes("Created new file " + self.path + "" + post_body, "UTF-8"))
            return
        except IOError:
            print("PYTHON API: " + "POST requested " + self.path + "" + post_body + ". 404 Not Found")
            sys.stdout.flush()
            self.send_error(404, "File not found")
        return
    
    def do_DELETE(self):
        rootdir = expanduser("~") + "/CrystalHomeSys/Heart" #file location
        try:
            os.remove(rootdir + self.path)
            print("PYTHON API: " + "DELETE requested " + self.path)
            sys.stdout.flush()
            self.send_response(204)
            
            self.send_header('Content-type','text-html')
            self.end_headers()
            return
        except IOError:
            print("PYTHON API: " + "DELETE requested " + self.path + ". 404 Not Found")
            sys.stdout.flush()
            self.send_error(404, "File not found")

#ip and port of server
#by default http server port is 80
ip = sys.argv[1]
port = int(sys.argv[2])
port_works = False
while port_works == False:
    try:
        server_address = (ip, port)
        httpd = HTTPServer(server_address, HTTPRequestHandler)
        port_works = True
    except:
        port = port + 1

print("Python API Server starting on IP: " + ip + " Port: " + str(port))
sys.stdout.flush()
httpd.serve_forever()
