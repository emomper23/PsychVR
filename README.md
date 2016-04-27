# PyschVR
VR 3D Graphics Project

This is a general guide for the setup, and development of the PyschVR Virtual Reality Simulation and Treatment tool.

Install Instructions:
    Getting Binaries:
    Download from https://drive.google.com/folderview?id=0BxwoxAg5uGw-VmpLaWJvUjIzVlU&usp=sharing

    Install the Oculus Rift Home Drivers:
    Download the drivers from https://www.oculus.com/en-us/setup/

    Install the Leap Motion Drivers:
    https://developer.leapmotion.com/get-started
    
    Install the Kinect For Windows Drivers:
    https://www.microsoft.com/en-us/download/details.aspx?id=44559
    

Development Instructions:

    Getting our software:
    
    These instructions assume you have already installed the hardware system device drivers listed in the Install section.

    Install Qt:
    Our Project uses Qt Creator 3.6.1 Based on Qt 5.6.0. Community (Opensource Version) 
    it can be installed from https://www.qt.io/download-open-source/
    For help with installation or more detailed instructions see http://doc.qt.io/qt-5/gettingstarted.html 

    Install Unity:
    Our Project uses Unity 5.3.4 
    it can be installed from https://unity3d.com/unity/whats-new/unity-5.3.4
    For help with installation or more detailed instructions see http://docs.unity3d.com/Manual/InstallingUnity.html


    Install Development SDK's for Hardward Systems:  
    
    Oculus Rift SDK:
    https://developer.oculus.com/downloads/
    This download page includes many examples and tutorials
    
    Leap Motion SDK:
    https://developer.leapmotion.com/get-started
    For more details on Using Leap with Unity https://developer.leapmotion.com/unity 
    
    
    Kinect SDK:
    https://www.microsoft.com/en-us/download/details.aspx?id=44561
    For more information on setup and developemnt  https://developer.microsoft.com/en-us/windows/kinect

    
Usage Instructions:

    Qt Configuration Tool Usage:
    
    Qt Analysis Tool Usage:

    Unity Scene Usage:

Cross Platform Support:

    As much as we would like to have full cross platform support for this app, As of right now Oculus Home is only supported on Windows. To build to other devices 
    to build the Qt project Open Qt creator and find the .pro file (this app should be fully cross platform incase the VR headset and being used is supported on other Operating systems.

    Unity scenes can also be built for 
