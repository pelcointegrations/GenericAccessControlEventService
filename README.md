# GenericAccessControlEventService
Generic Access Control Event Service

The GenericAccessControlEventService is a generic codebase for starting a new Access Control integration with VideoXpert.
It provides an easily modifiable UI based on Winforms, a service that may be launched from the command line
for debug purposes that normally runs as a service and an installer that installs both the configuration
utility and service into c:\program files (x86)\Pelco\[Integration].  

The UI will query the user to start/re-start the service
upon configuration changes.  Configuration is saved in xml files in the shared install directory.

Installer is currently working and may be tested as is.  Works with Access Control System Viewer plugin.

To create a new integration based upon this code:
1.  Globally replace "GenericAccessControl" with your integration name (i.e "AccessXpert").
2.  Replace all guids found in the installer product.wxs file.
3.  Replace all assembly guids.
4.  Replace all project and solution guids (may not be necessary, but would be a good idea).  
5.  Look for text "Generic" anywhere else and change where it makes sense.
6.  Look for text "todo:" for areas where your integration will need work.
7.  Change ACSWrapper.cs for your needs.  Replace file with something else if desired - this is the wrapper to
    communicate with your integration.
    Note: this file is duplicated in both the UI and Event Service apps.  Tried to make it common but it got pulled
    into the individual directories for some reason.  It would be nice to fix it to a common directory.
8.  Modify IACSServer interface handlers found at end of VxEventHandler.cs.
9.  Change installer product.wxs to add/change any files you want installed.
