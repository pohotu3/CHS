#!/usr/bin/env python
 
import http.client
import sys
 
#get http server ip
http_server = sys.argv[1]
http_socket = sys.argv[2]
#create a connection
conn = http.client.HTTPConnection(http_server, http_socket)
cmd = input('input command (ex. GET index.html): ')
cmd = cmd.split()

body = []

for x in range(2, len(cmd)):
    body.append(cmd[x])
    
body = " ".join(body)

#request command to server
conn.request(cmd[0], cmd[1], body)

#get response from server
rsp = conn.getresponse()

#print server response and data
print(rsp.status, rsp.reason)
if not rsp.status == 404:
    data_received = rsp.read()
    data_received = data_received
    print(data_received)
  
conn.close() 
