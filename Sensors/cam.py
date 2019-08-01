#!/usr/bin/python
import os
import pygame, sys
from mqtt_publisher import sendData
from pygame.locals import *
#from signal1 import get_mac
import pygame.camera
import base64
import random, string
import math
import subprocess

packet_size=3000
def get_mac():
    p = subprocess.Popen('sudo iwlist wlan0 scan|egrep "Cell|ESSID|Signal"', stdout=subprocess.PIPE, shell = True) # |ESSID
    (output, err) = p.communicate()
    status = p.wait()
    resultline = output
    max_quality = 0
    mac_addr = ''
    cells = resultline.split('Cell')
    for i in range(len(cells)):
        splitted_cell = cells[i].split(' ')
        formatted_splitted_cell = list(filter(lambda x: x != '' and x != '\n', splitted_cell))
        if(formatted_splitted_cell):
            #print formatted_splitted_cell[3][:-1]
            #print formatted_splitted_cell[4]
            max_temp = int(formatted_splitted_cell[4][8:10])
            if(max_temp > max_quality):
                max_quality = max_temp
                mac_addr = formatted_splitted_cell[3][:-1]
    print "Max quality: %s" % max_quality
    print "Mac addr: %s" % mac_addr
    return (max_quality,mac_addr)

def takePicture():

	width = 640
	height = 480

	#initialise pygame
	pygame.init()
	pygame.camera.init()
	cam = pygame.camera.Camera("/dev/video0",(width,height))
	cam.start()

	#setup window
	windowSurfaceObj = pygame.display.set_mode((width,height),1,16)
	pygame.display.set_caption('Camera')

	#take a picture
	image = cam.get_image()
	cam.stop()

	#display the picture
	catSurfaceObj = image
	windowSurfaceObj.blit(catSurfaceObj,(0,0))
	pygame.display.update()

	#save picture
	pygame.image.save(windowSurfaceObj,'picture.jpg')
#takePicture()

def convertImageToBase64(addr):
    with open("picture.jpg", "rb") as image_file:
        encoded = base64.b64encode(image_file.read())
    sendData(encoded+"@"+addr,"sensors7","P3")
    #return encoded
#print len(str(convertImageToBase64()))

#generate random string to ID the image
def randomword(length):
 return ''.join(random.choice(string.lowercase) for i in range(length))


#split the data into chunks of size 3000, append some identifying information, then publish.
def webcam():
    new_addr = ''
    old_addr = ''
    while True:
       (quality,addr) = get_mac()
       new_addr = addr
       if(quality > 69 and new_addr != old_addr): #and new_addr != "94:B4:0F:83:50:A0"):
            takePicture() 
            convertImageToBase64(addr)
            old_addr = addr
            
    

def publishEncodedImage(encoded): 
 end = packet_size
 start = 0
 length = len(encoded)
 picId = randomword(8)
 pos = 0
 no_of_packets = math.ceil(length/packet_size)
 while start <= len(encoded):
     data = {"data": encoded[start:end], "pic_id":picId, "pos": pos, "size": no_of_packets}
     client.publishEvent("Image-Data",json.JSONEncoder().encode(data))
    
     end += packet_size
     start += packet_size
     pos = pos +1

webcam()

