import math
import UdpComms as U

sock = U.UdpComms(udpIP="127.0.0.1", portTX=8000, portRX=8001, enableRX=True, suppressWarnings=True)

#Camera Pixel Delta
deltas = [0,0,0] #x,y,z delta

#Camera Resolution, UPDATE IF NECC
x_res = 960
y_res = 480

#Camera XY P Controller
xy_kp = 0.0725
velo = [0,0] #vx,vy
'''
TO RETUNE BASED ON DJI SIMULATOR
ku = 0.145
tu = 1.625
kp = 0.116
kd = 0.0235625
'''

#XY Normalisation
angle_xy = 0
top_vxy = 15
xy_normal = 1
'''
8.5 max linear speed for typhoon drone under gazebo environment
change top_vxy to 15 for implementation under dji sdk
change xy_normal to 1 for implementation under dji sdk
'''

#Z Estimation & Accounting
camera_md = [1]*6 #JSON Format: Obj ID, Confidence, xmin, ymin, xmax, ymax
bound_area = 1
z_est_const = 1; #placeholder
delta_inverse = 0.01953; # x * delta = 1 when delta = 50, accounting for altitude delta

def state_update():
    #use this function to read from json and update global vars
    #placeholder

    #Centralise camera frame
    deltas[0] = ((camera_md[2] + camera_md[4]) / 2) - (x_res / 2)
    deltas[1] = ((camera_md[3] + camera_md[5]) / 2) - (y_res / 2)
    camera_chase()

def send_cmd():
    #use this function to transmit data
    sock.SendData(f"{velo[0]},{velo[1]}")
    #placeholder

def z_delta_estimation(): #z_delta and bound_area share inverse relationship
    bound_area = (camera_md[4] - camera_md[2]) * (camera_md[5] - camera_md[3]) #UPDATE IF CAMERA_MD LIST FORMAT CHANGES
    deltas[2] = 7.992481 + (84074700 - 7.992481)/(1 + pow((bound_area/0.00004412605),0.9062109)) #Calculated relation based on GT

def camera_chase():
    z_delta_estimation()
    velo[0] = (xy_kp * deltas[0]) * (delta_inverse * deltas[2])
    velo[1] = (xy_kp * deltas[1]) * (delta_inverse * deltas[2])
    #z_delta to be replaced with estimated delta based on bounding box

    #Normalise wrt max speed
    if(velo[0] > top_vxy):
        velo[0] = top_vxy
    elif(velo[0] < -top_vxy):
        velo[0] = -top_vxy
    if(velo[1] > top_vxy):
        velo[1] = top_vxy
    elif(velo[1] < -top_vxy):
        velo[1] = -top_vxy

    #Normalise wrt throttle control
    velo[0] = velo[0] / top_vxy * xy_normal
    velo[1] = velo[1] / top_vxy * xy_normal
    send_cmd()

def main():
    while True:
        data = sock.ReadReceivedData() # read data
        if data != None: # if NEW data has been received since last ReadReceivedData function call
            camera_md[2:6] = map(float,data.split(','))
            state_update()

main()