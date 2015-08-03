#!/usr/bin/env python
 
import http.client
import sys
 
#get http server ip
http_server = sys.argv[1]
http_socket = sys.argv[2]
#create a connection
conn = http.client.HTTPConnection(http_server, http_socket)
 
while 1:
  cmd = input('input command (ex. GET index.html): ')
  cmd = cmd.split()
 
  if cmd[0] == 'exit': #tipe exit to end it
    break
  
  #request command to server
  conn.request(cmd[0], cmd[1])
 
  #get response from server
  rsp = conn.getresponse()
  
  #print server response and data
  print(rsp.status, rsp.reason)
  if rsp.status == 200:
    data_received = rsp.read()
    print(data_received)
  
conn.close() 
