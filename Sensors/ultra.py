from mqtt_publisher import sendData
import Adafruit_DHT
import RPi.GPIO as GPIO
import time

def acquireDistance():
    while True:
            try:
                    GPIO.setmode(GPIO.BOARD)
                    PIN_TRIGGER = 7
                    PIN_ECHO = 11
                    GPIO.setup(PIN_TRIGGER, GPIO.OUT)
                    GPIO.setup(PIN_ECHO, GPIO.IN)
                    GPIO.output(PIN_TRIGGER, GPIO.LOW)
                    GPIO.output(PIN_TRIGGER, GPIO.HIGH)
                    time.sleep(0.51111)
                    GPIO.output(PIN_TRIGGER, GPIO.LOW)
                    while GPIO.input(PIN_ECHO) == 0:
                            pulse_start_time = time.time()
                    while GPIO.input(PIN_ECHO) == 1:
                            pulse_end_time = time.time()
                    pulse_duration = pulse_end_time - pulse_start_time
                    distance = round(pulse_duration * 17150, 1)
                    print "Distance: ", distance, " cm "
		    sendData(distance,"sensors6","P2")
            finally:
                    GPIO.cleanup()
            #return distance
acquireDistance()