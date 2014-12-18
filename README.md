a-blogsite (A Name Not Yet Taken AB)
==========

This is a tool or a content management system (CMS) for a website that is based on ASP.NET, MVC, C# and MS SQL. The frontend design can be changed in the backend interface of a-blogsite, this makes it possible to have unique websites that are based on the same codebase.

The documentation for this tool is under development and should be able to read more about this solution at <a href="http://www.a-blogsite.com">a-blogsite.com (English)</a> or <a href="http://www.a-blogsite.se">a-blogsite.se (Swedish)</a>

You can see a demo of the blogsite at <a href="http://a-blogsite-demo.azurewebsites.net/">a-blogsite (demo)</a>

<b>A quick start guide</a>
Set up a website and a MS SQL database on your server. The connection string to the database should be a "appSetting" and the key should be called "ConnectionString". If you use Windows Azure like we do, you can add the appSetting under the CONFIGURE tab in the settings for the website. You can also add a app.config file to the solution with contents like this:

<?xml version="1.0" encoding="utf-8" ?>
<appSettings>
<add key="ConnectionString" value="Server=XXXXXXXXXXXXXXXXXXX" />
</appSettings>

You can also add the connection string directly in the Web.Config file if you want.

When you have deployed the website, go to ~/admin_default and sign in with Master as the username and an empty password field. The next thing to do is to go to Domains and change the domain name and the webaddress for the first post in the list. The domain name should be stated without http://wwww or https://www, it should just be the domain name. The webaddress is the full webaddress to the website. 

