# Self-Publish #
<br/>

This project is aimed at simplifying publishing. We are currently interested in scenarios below to start with.

<br/>
## 1. Publishing the latest version of your app to a Virtual Machine ##
## 2. Publishing a web and a windows service ##
## 3. Publishing a web with either MSDeploy or git ##
<br />

***
## Publishing the latest version of your app to a Virtual Machine ##
For scenarios where you are spinning up new virtual machines based on the load for your site you should be able to easily publish the latest version of your application to that machine. This can be difficult because many publishing technologies work more in a push model than a pull model. The pull model is better suited for these cloud based VM scenarios than is a push model.

The way in which we would like to enable this is to install a very lightweight service on the virtual machine. This service will invoked when the machine starts up, and it will occassionaly ping a web service to determine if there are any updates.

> Note: we are also looking at ways in which we could perform a "real time" publish (i.e. the virtual machine will be notified of an update), but that is not a primary feature at this point.

## Publishing a web and a windows service ##
There are a lot of tools which can be used for publishing web sites but there are not a lot of good tools which allow you to easily publish both a web site as well as a windows service. The way that we would like to solve this problem is to allow you to easily execute arbitrary code on the server which is being published to. For example if you need to publish an ASP.NET web site and a Windows service then you will need to create a script which can perform the following actions:

1. Copy the files to the correct location
1. Execute appcmd.exe in order to provision the web site
1. Execute sc.exe to provision the windows service and start it

## Unifying publish methods like MSDeploy/git/TFS ##
Currently people are using different ways to publish their applications like:

1. MSDeploy
2. git
3. TFS
4. Network Copy
5. You name it

We would like to create a framework which these different publishing mechanisms can plug into and provide a consistent end-user experience.


> Note: More to come later here.

