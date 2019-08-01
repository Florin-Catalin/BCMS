import paho.mqtt.client as mqtt #import the client1
broker_address="192.168.43.130" 
#broker_address="iot.eclipse.org"

def sendData(MESSAGE,topic,client_name):
    #print("creating new instance")
    client = mqtt.Client(client_name) #create new instance
    #print("connecting to broker")
    client.connect(broker_address) #connect to broker
    #print("Subscribing to topic",topic)
    client.subscribe(topic)
    #print("Publishing message to topic",topic)
    client.publish(topic,str(MESSAGE))

    
