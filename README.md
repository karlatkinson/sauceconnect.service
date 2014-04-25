sauceconnect.service
====================

Windows service to create a sauce connect tunnel

Configuration is set in the application settings file

<add key="sauceConnectUserName" value="{USERNAME}"/>
<add key="sauceConnectAccessKey" value="{ACCESSKEY}"/>
<add key="sauceConnectIdentifier" value="{IDENTIFIER}"/>

To install run at the path you wish the service to live:
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" "SauceConnect.Service.exe"

DEV
====
Uses rake to build the solution.
To build, you require ruby 1.9.* installed with the rake and bundler gems.

Run:
bundle install
Then:
rake