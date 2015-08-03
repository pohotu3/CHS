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

#request command to server
if len(cmd) == 2:
    conn.request(cmd[0], cmd[1], cmd[1])
elif len(cmd) == 3:
    conn.request(cmd[0], cmd[1], cmd[2])

#conn.request(cmd[0], cmd[1], cmd[2])

#get response from server
rsp = conn.getresponse()

#print server response and data
print(rsp.status, rsp.reason)
if rsp.status == 200:
    data_received = rsp.read()
    print(data_received)
  
conn.close() 
