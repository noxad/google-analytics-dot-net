Google Analytcis Dot Net
========================

A library that can send Google Analytics tracking requests from a .NET application. Provides a simple method of tracking application usage by logging page views to Google Analytics. What a 'page view' means in this context is up to the user. It could be the application name, a particular page/dialog of the application, or just an empty string if none is desired. 

Leverages the free Google Analytics service, avoiding paid services, or the hassle of building and maintaining your own custom solution (web services and server, database and server, build custom reports, etc.)

Features
---------------------

* Desktop App Usage Tracking
** Allows you to track a .NET application's usage with free Google Analytics
* Simple
** One line of code logs a tracking request once a reference to the library is added to your project
* Anonymous
** Anonymizes user data, enabling you to track unique users, but do so without sending data that could identify the users
* Reports
** Take advantage of the already available Google Analytics reports to analyze your application's usage

How to Use
---------------------

1. Create a new Google Analytics property (or use an existing property)
	- Set the value 'Website's URL' to 'Not a website'
	- Tracking ID in the format of UA-XXXXXXXX-XX will be created for you to use in tracking your application
2. Add a reference to the GoogleAnalyticsDotNet library to your .NET project
3. Add this line of code anywhere you want to send a tracking request from:

    ```csharp
    GoogleAnalyticsDotNet.SendTrackingRequest("UA-XXXXXXXX-XX", "optional page name");
    ```
4. Done

Attribution
---------------------

Code based on ASP.NET version of Google Analytics for Mobile Websites:  
* https://developers.google.com/analytics/devguides/collection/other/mobileWebsites