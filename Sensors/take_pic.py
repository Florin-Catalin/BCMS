import os
def take_pic():
	take_pic = 'fswebcam -r 640x480 --no-banner picture.jpg'
	os.system(take_pic)
take_pic()	
