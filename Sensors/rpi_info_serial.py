import os, urllib, json, serial, time

#start serial conection with arduino
ser = serial.Serial('/dev/ttyUSB0', 9600)

betWait = 1 #wait before sending second string cant go lower than 1 because of arduino StringRead timeout
sleepTime = 3 #wait before sending next batch of info

# Return CPU temperature as a character string                                      
def getCPUtemperature():
    res = os.popen('vcgencmd measure_temp').readline()
    return(res.replace("temp=","").replace("'C\n",""))

# Return RAM information (unit=kb) in a list                                        
# Index 0: total RAM                                                                
# Index 1: used RAM                                                                 
# Index 2: free RAM                                                                 
def getRAMinfo():
    p = os.popen('free')
    i = 0
    while 1:
        i = i + 1
        line = p.readline()
        if i==2:
            return(line.split()[1:4])

# Return % of CPU used by user as a character string                                
def getCPUuse():
    return(str(os.popen("top -n1 | awk '/Cpu\(s\):/ {print $2}'").readline().strip(\
)))

# Return information about disk space as a list (unit included)                     
# Index 0: total disk space                                                         
# Index 1: used disk space                                                          
# Index 2: remaining disk space                                                     
# Index 3: percentage of disk used                                                  
def getDiskSpace():
    p = os.popen("df -h /")
    i = 0
    while 1:
        i = i +1
        line = p.readline()
        if i==2:
            return(line.split()[1:5])
			
#Get external IP
#def getIP():
#	data = urllib.urlopen("http://echoip.com/").read()
#	return data
			
def serialClear():
	ser.write("00clr")
	
def serialWrite(Line1, Line2):
	serialClear();
	time.sleep(betWait)
	ser.write("00"+Line1)
	time.sleep(betWait)
	ser.write("01"+Line2)

def getPID():
	pid = os.getpid()
	return pid
	
time.sleep(3)  #wait for arduino to reset

while True:
	# CPU informatiom
	CPU_temp = getCPUtemperature()
	CPU_usage = getCPUuse()

	# RAM information
	# Output is in kb, here I convert it in Mb for readability
	RAM_stats = getRAMinfo()
	RAM_total = round(int(RAM_stats[0]) / 1000,1)
	RAM_used = round(int(RAM_stats[1]) / 1000,1)
	RAM_free = round(int(RAM_stats[2]) / 1000,1)

	# Disk information	
	DISK_stats = getDiskSpace()
	DISK_total = DISK_stats[0]
	DISK_free = DISK_stats[1]
	DISK_perc = DISK_stats[3]
	
	#external IP
	#IP = getIP()
	
	#Python script pid
	PID = getPID()
	
	#OUTPUT TO ARDUINO
	serialWrite("Process ID", str(PID))
	time.sleep(sleepTime)
	serialWrite("Temperature:", CPU_temp + " C")
	time.sleep(sleepTime)
	serialWrite("CPU Usage:", CPU_usage + "%")
	time.sleep(sleepTime)
	serialWrite("Total RAM:", str(RAM_total) + " MB")
	time.sleep(sleepTime)
	serialWrite("Used RAM:", str(RAM_used) + " MB")
	time.sleep(sleepTime)
	serialWrite("Free RAM:", str(RAM_free) + " MB")
	time.sleep(sleepTime)
	serialWrite("Total Disk Space:", str(DISK_total)+"B")
	time.sleep(sleepTime)
	serialWrite("Free Disk Space:", str(DISK_free) + "B")
	time.sleep(sleepTime)
	serialWrite("Disk Used:", str(DISK_perc))
	time.sleep(sleepTime)
	#serialWrite("Public IP:", str(IP))
	#time.sleep(sleepTime)
	serialWrite("I'm" , "HUNGRRYY")
	
	time.sleep(sleepTime)
	
	 
	
	#DEBUG OUTPUTS
	# print(CPU_temp)
	# print(CPU_usage)
	# print(RAM_total)
	# print(RAM_used)
	# print(RAM_free)
	# print(DISK_total)
	# print(DISK_free)
	# print(DISK_perc)
	# print(IP)
	# print("\n")
	# time.sleep(3)