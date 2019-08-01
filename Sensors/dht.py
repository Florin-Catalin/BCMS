#!/usr/bin/python
from mqtt_publisher import sendData
import Adafruit_DHT
import RPi.GPIO as GPIO
import time


def dht():
    BCM_DHT_PIN = 27
    while True:

        humidity, temperature = Adafruit_DHT.read_retry(Adafruit_DHT.DHT11, BCM_DHT_PIN )
        if humidity is None or temperature is None:
            print "Could not retrieve data"
        else:
            #distance = acquireDistance()
            print("Temp={0:0.1f}*C Humidity={1:0.1f}%".format(temperature,humidity))
            s = str(temperature) + ',' + str(humidity) + ',' + str(715)
            sendData(s,"sensors5","P1")
dht()

    
