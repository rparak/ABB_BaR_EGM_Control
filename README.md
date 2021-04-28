# Control of the ABB robot through B&R Automation PLC - EGM (Externally Guided Motion)

## Requirements:

**Software:**
```bash
ABB RobotStudio, B&R Automation Studio
```
ABB RS: https://new.abb.com/products/robotics/robotstudio/downloads

ABB EGB: https://new.abb.com/products/3HAC054376-001/ext-guided-motion-egm

B&R Automation: https://www.br-automation.com/en/downloads/#categories=Software-1344987434933

## Project Description:

The project is focused on a simple demonstration of client / server communication via OPC UA, which is implemented in C# Console App. (Server - B&R Automation PLC, Client - C# Console App). The console application communicates with the ABB robot via UDP (User Datagram Protocol) and controls the robot throught EGM (Externally Guided Motion). The application uses performance optimization using multi-threaded programming.

**Notes:**

EGM (Externally Guided Motion) is an interface for ABB robots that allows smoothless control of the robotic arm from an external application (in our case it is a C# Console Application). EGM can be used to stream positions to the robot controller either in the Joint / Cartesian space. 

```bash
Autogenerated code from the egm.proto -file:
ABB_BaR_EGM_Control/C_Sharp_App/EGM_OPCUa_ABB_BR_Client/EGM_OPCUa_ABB_BR_Client/Egm.cs
```

EGM communication must be defined in: Configuration Editor/Communication/Transmission Protocol 

<p align="center">
  <img src="https://github.com/rparak/ABB_BaR_EGM_Control/blob/main/images/egm_config.png" width="800" height="400">
</p>

**MappView (HMI):**
```bash
Simulation Address
PLC_ADDRESS = localhost or 127.0.0.1

http://PLC_ADDRESS:81/index.html?visuId=abb_move
```

The project was realized at Institute of Automation and Computer Science, Brno University of Technology, Faculty of Mechanical Engineering (NETME Centre - Cybernetics and Robotics Division).

<p align="center">
  <img src="https://github.com/rparak/ABB_BaR_EGM_Control/blob/main/images/egm_diagram.png" width="800" height="450">
</p>

**Appendix:**

Example of a simple data processing application (OPC UA):

[OPC UA B&R Automation - Data Processing](https://github.com/rparak/OPCUA_Simple)

Example of a simple data processing application (Robot Web Services):

[ABB Robot - Data Processing](https://github.com/rparak/ABB_Robot_data_processing/)

## Project Hierarchy:

**Repositary [/ABB_BaR_EGM_Control/]:**

```bash
[] /.../
```

## Application:

## ABB RobotStudio:

<p align="center">
  <img src="https://github.com/rparak/ABB_BaR_EGM_Control/blob/main/images/abb_app.PNG" width="800" height="450">
</p>

## HMI (Human-Machine Interface) - MappView:

<p align="center">
  <img src="https://github.com/rparak/ABB_BaR_EGM_Control/blob/main/images/mv_1.png" width="800" height="450">
  <img src="https://github.com/rparak/ABB_BaR_EGM_Control/blob/main/images/mv_2.png" width="800" height="450">
  <img src="https://github.com/rparak/ABB_BaR_EGM_Control/blob/main/images/mv_3.png" width="800" height="450">
  <img src="https://github.com/rparak/ABB_BaR_EGM_Control/blob/main/images/mv_4.png" width="800" height="450">
</p>

## Result:

Youtube: ...

## Contact Info:
Roman.Parak@outlook.com

## License
[MIT](https://choosealicense.com/licenses/mit/)
