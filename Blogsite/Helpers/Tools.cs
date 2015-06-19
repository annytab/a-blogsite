using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Data.SqlTypes;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Azure;
using Annytab;

/// <summary>
/// This static class includes handy methods
/// </summary>
public static class Tools
{
    /// <summary>
    /// Get the database connection string
    /// </summary>
    /// <returns>A connection string</returns>
    public static string GetConnectionString()
    {
        // Return the connection string
        return CloudConfigurationManager.GetSetting("ConnectionString");

    } // End of the GetConnectionString method

    /// <summary>
    /// Get the current domain
    /// </summary>
    /// <returns>The current domain, a empty domain if it is null</returns>
    public static Domain GetCurrentDomain()
    {
        // Get the domain name
        string domainName = HttpContext.Current.Request.Url.Host;

        // Replace www.
        domainName = domainName.Replace("www.", "");

        // Get the domain post
        Domain domain = Domain.GetOneByDomainName(domainName);

        // Make sure that the domain not is null
        if(domain == null)
        {
            domain = new Domain();
            domain.id = 0;
            domain.domain_name = "localhost";
            domain.web_address = "https://localhost:80";
            domain.front_end_language = 2;
            domain.back_end_language = 2;
            domain.custom_theme_id = 0;
            domain.analytics_tracking_id = "";
            domain.facebook_app_id = "";
            domain.facebook_app_secret = "";
            domain.google_app_id = "";
            domain.google_app_secret = "";
            domain.noindex = true;
        }

        // Return the current front end language id
        return domain;

    } // End of the GetCurrentDomain method

    /// <summary>
    /// Get the stemmer based on the language
    /// </summary>
    /// <param name="language">A reference to the language</param>
    /// <returns>A reference to a Stemmer</returns>
    public static Stemmer GetStemmer(Language language)
    {
        // Create a default stemmer
        Stemmer stemmer = new DefaultStemmer();

        // Get the language code in lower case
        string language_code = language.language_code.ToLower();

        // Get a stemmer depending on the language
        if (language_code == "da")
        {
            stemmer = new DanishStemmer();
        }
        else if (language_code == "nl")
        {
            stemmer = new DutchStemmer();
        }
        else if (language_code == "en")
        {
            stemmer = new EnglishStemmer();
        }
        else if (language_code == "fi")
        {
            stemmer = new FinnishStemmer();
        }
        else if (language_code == "fr")
        {
            stemmer = new FrenchStemmer();
        }
        else if (language_code == "de")
        {
            stemmer = new GermanStemmer();
        }
        else if (language_code == "it")
        {
            stemmer = new ItalianStemmer();
        }
        else if (language_code == "no")
        {
            stemmer = new NorwegianStemmer();
        }
        else if (language_code == "pt")
        {
            stemmer = new PortugueseStemmer();
        }
        else if (language_code == "ro")
        {
            stemmer = new RomanianStemmer();
        }
        else if (language_code == "es")
        {
            stemmer = new SpanishStemmer();
        }
        else if (language_code == "sv")
        {
            stemmer = new SwedishStemmer();
        }

        // Return the stemmer
        return stemmer;

    } // End of the GetStemmer method

    /// <summary>
    /// Get the culture info
    /// </summary>
    /// <param name="language">A reference to a language</param>
    /// <returns>A reference to a culture info object</returns>
    public static CultureInfo GetCultureInfo(Language language)
    {
        // Create the culture info to return
        CultureInfo cultureInfo = new CultureInfo("en-US");

        // Create the culture info to return
        try
        {
            cultureInfo = new CultureInfo(language.language_code.ToLower() + "-" + language.country_code.ToUpper());
        }
        catch(Exception ex)
        {
            string exMessage = ex.Message;
            cultureInfo = new CultureInfo("en-US");
        }          

        // Return the culture info
        return cultureInfo;

    } // End of the GetCultureInfo method

    /// <summary>
    /// This method is used to generate a random password
    /// </summary>
    /// <returns>A password</returns>
    public static string GeneratePassword()
    {
        // Create the string to return
        string passwordString = "";

        // Create variables for the password generation
        Char[] possibleCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        Random randGenerator = new Random();

        // Create the password
        for (int i = 0; i < 10; i++)
        {
            int randomNumber = randGenerator.Next(possibleCharacters.Length);
            passwordString += possibleCharacters[randomNumber];
        }

        // Return the password string
        return passwordString;

    } // End of the GeneratePassword method

    /// <summary>
    /// Send an email to the webmaster
    /// </summary>
    /// <param name="customerAddress">The customer address</param>
    /// <param name="subject">The subject for the mail message</param>
    /// <param name="message">The message</param>
    public static bool SendEmailToHost(string customerAddress, string subject, string message)
    {
        // Create the boolean to return
        bool successful = true;

        // Get website settings
        KeyStringList websiteSettings = WebsiteSetting.GetAllFromCache();

        // Create variables
        string host = websiteSettings.Get("SEND-EMAIL-HOST");
        Int32 port = 0;
        Int32.TryParse(websiteSettings.Get("SEND-EMAIL-PORT"), out port);
        string emailAddress = websiteSettings.Get("SEND-EMAIL-ADDRESS");
        string password = websiteSettings.Get("SEND-EMAIL-PASSWORD");
        string toAddress = websiteSettings.Get("CONTACT-US-EMAIL");
        string useSSL = websiteSettings.Get("SEND-EMAIL-USE-SSL");

        // Get the customer email
        MailAddress copyAddress = AnnytabDataValidation.IsEmailAddressValid(customerAddress);

        // Create the SmtpClient instance
        SmtpClient smtp = new SmtpClient(host, port);
        smtp.Credentials = new NetworkCredential(emailAddress, password);

        // Check if SSL should be used
        if(useSSL.ToLower() == "true")
        {
            smtp.EnableSsl = true;
        }
        
        // Try to send the mail message
        try
        {
            // Create the mail message instance
            MailMessage mailMessage = new MailMessage(emailAddress, toAddress);

            // Add a carbon copy to the customer
            if(copyAddress != null)
            {
                mailMessage.CC.Add(copyAddress);
            }

            // Create the mail message
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;

            // Send the mail message
            smtp.Send(mailMessage);

        }
        catch (Exception ex)
        {
            string exceptionMessage = ex.Message;
            successful = false;
        }

        // Return the boolean
        return successful;

    } // End of the SendEmailToHost method

    /// <summary>
    /// Send an email to a user
    /// </summary>
    /// <param name="toAddress">The address to send the email to</param>
    /// <param name="subject">The subject for the mail message</param>
    /// <param name="message">The mail message</param>
    public static bool SendEmailToUser(string toAddress, string subject, string message)
    {
        // Create the boolean to return
        bool successful = true;

        // Get the webshop settings
        KeyStringList webshopSettings = WebsiteSetting.GetAllFromCache();

        // Create variables
        string host = webshopSettings.Get("SEND-EMAIL-HOST");
        Int32 port = 0;
        Int32.TryParse(webshopSettings.Get("SEND-EMAIL-PORT"), out port);
        string emailAddress = webshopSettings.Get("SEND-EMAIL-ADDRESS");
        string password = webshopSettings.Get("SEND-EMAIL-PASSWORD");
        string useSSL = webshopSettings.Get("SEND-EMAIL-USE-SSL");

        // Create the SmtpClient instance
        SmtpClient smtp = new SmtpClient(host, port);
        smtp.Credentials = new NetworkCredential(emailAddress, password);

        // Check if SSL should be used
        if (useSSL.ToLower() == "true")
        {
            smtp.EnableSsl = true;
        }

        // Try to send the mail message
        try
        {
            // Create the mail message instance
            MailMessage mailMessage = new MailMessage(emailAddress, toAddress);

            // Create the mail message
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;

            // Send the mail message
            smtp.Send(mailMessage);

        }
        catch (Exception ex)
        {
            string exceptionMessage = ex.Message;
            successful = false;
        }

        // Return the boolean
        return successful;

    } // End of the SendEmailToUser method

    /// <summary>
    /// Get image urls for the domain
    /// </summary>
    /// <param name="domainId">The id for the domain</param>
    /// <returns>A key string list with urls</returns>
    public static KeyStringList GetDomainImageUrls(Int32 domainId)
    {
        // Create the list to return
        KeyStringList domainImageUrls = new KeyStringList(5);

        // Create paths
        string directoryPath = "/Content/domains/" + domainId.ToString() + "/images/";

        // Add images to the key string list
        domainImageUrls.Add("background_image", directoryPath + "background_image.jpg");
        domainImageUrls.Add("default_logotype", directoryPath + "default_logotype.jpg");
        domainImageUrls.Add("mobile_logotype", directoryPath + "mobile_logotype.jpg");
        domainImageUrls.Add("big_icon", directoryPath + "big_icon.jpg");
        domainImageUrls.Add("small_icon", directoryPath + "small_icon.jpg");

        // Return the list
        return domainImageUrls;

    } // End of the GetDomainImageUrls method

    /// <summary>
    /// Get the main image url for a administrator
    /// </summary>
    /// <param name="administratorId">The administrator id</param>
    /// <returns>An image url as a string</returns>
    public static string GetAdministratorMainImageUrl(Int32 administratorId)
    {
        // Set the standard image url
        string imageUrl = "/Content/images/annytab_design/no_user_image.jpg";

        // Create the main image url
        string mainImageUrl = "/Content/administrators/" + (administratorId / 100).ToString() + "/" + administratorId.ToString() + "/main_image.jpg";

        // Set the image url if the file exists
        if(System.IO.File.Exists(HttpContext.Current.Server.MapPath(mainImageUrl)) == true)
        {
            imageUrl = mainImageUrl;
        }

        // Return the image url
        return imageUrl;

    } // End of the GetAdministratorMainImageUrl method

    /// <summary>
    /// Get the IP-address of the customer
    /// </summary>
    /// <returns>The IP-address as a string</returns>
    public static string GetUserIP()
    {
        string ipList = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (string.IsNullOrEmpty(ipList) == false)
        {
            return ipList.Split(',')[0];
        }

        return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

    } // End of the GetUserIP method

    /// <summary>
    /// Protect a cookie value
    /// </summary>
    public static string ProtectCookieValue(string value, string purpose)
    {
        // Create the string to return
        string encodedString = "";

        try
        {
            // Get the byte array
            byte[] stream = Encoding.UTF8.GetBytes(value);

            // Protect the value
            byte[] encodedValue = MachineKey.Protect(stream, purpose);

            // Get encoded string
            encodedString = HttpServerUtility.UrlTokenEncode(encodedValue);
        }
        catch (Exception e)
        {
            string exMessage = e.Message;
        }
        
        // Return the encrypted value as a string
        return encodedString;

    } // End of the ProtectCookieValue method

    /// <summary>
    /// Unprotect a cookie value
    /// </summary>
    public static string UnprotectCookieValue(string value, string purpose)
    {
        // Create the string to return
        string decodedString = "";

        try
        {
            // Get the byte array
            byte[] stream = HttpServerUtility.UrlTokenDecode(value);

            // Unprotect the value
            byte[] decodedValue = MachineKey.Unprotect(stream, purpose);

            // Return the value as string
            decodedString = Encoding.UTF8.GetString(decodedValue);
        }
        catch (Exception e)
        {
            string exMessage = e.Message;
        }

        // Return the value as string
        return decodedString;

    } // End of the UnprotectCookieValue method

    /// <summary>
    /// Get a 404 not found page
    /// </summary>
    /// <returns>A string with html</returns>
    public static string GetHttpNotFoundPage()
    {
        // Create the string to return
        string htmlString = "";

        // Get the current domain
        Domain currentDomain = Tools.GetCurrentDomain();

        // Get the error page
        StaticPage staticPage = StaticPage.GetOneByConnectionId(4, currentDomain.front_end_language);
        staticPage = staticPage != null ? staticPage : new StaticPage();

        // Get the translated texts
        KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

        // Create the Route data
        System.Web.Routing.RouteData routeData = new System.Web.Routing.RouteData();
        routeData.Values.Add("controller", "home");

        // Create the controller context
        System.Web.Mvc.ControllerContext context = new System.Web.Mvc.ControllerContext(new System.Web.Routing.RequestContext(new HttpContextWrapper(HttpContext.Current), routeData), new Annytab.Blogsite.Controllers.homeController());

        // Create the bread crumb list
        List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
        breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
        breadCrumbs.Add(new BreadCrumb(staticPage.link_name, "/home/error/404"));

        // Set form values
        context.Controller.ViewBag.BreadCrumbs = breadCrumbs;
        context.Controller.ViewBag.CurrentCategory = new Category();
        context.Controller.ViewBag.TranslatedTexts = tt;
        context.Controller.ViewBag.CurrentDomain = currentDomain;
        context.Controller.ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
        context.Controller.ViewBag.StaticPage = staticPage;

        // Render the view
        using (StringWriter stringWriter = new StringWriter(new StringBuilder(), CultureInfo.InvariantCulture))
        {
            string viewPath = currentDomain.custom_theme_id == 0 ? "/Views/home/error.cshtml" : "/Views/theme/error.cshtml";
            System.Web.Mvc.RazorView razor = new System.Web.Mvc.RazorView(context, viewPath, null, false, null);
            razor.Render(new System.Web.Mvc.ViewContext(context, razor, context.Controller.ViewData, context.Controller.TempData, stringWriter), stringWriter);
            htmlString += stringWriter.ToString();
        }

        // Return the string
        return htmlString;

    } // End of the GetHttpNotFoundPage method

    /// <summary>
    /// Get the first and last name from a name string
    /// </summary>
    /// <param name="name">The name to convert</param>
    /// <returns>A string array with a length of 2</returns>
    public static string[] GetFirstAndLastName(string name)
    {
        // Create the string array to return
        string[] names = new string[] { "Firstname", "Lastname" };

        // Split the name string by space
        string[] nameParts = name.Split(' ');

        if (nameParts.Length > 1)
        {
            for (int i = 0; i < nameParts.Length; i++)
            {
                if (i == nameParts.Length - 1)
                {
                    names[1] = nameParts[i];
                }
                else
                {
                    names[0] += nameParts[i];
                }
            }
        }

        // Return the string array
        return names;

    } // End of the GetFirstAndLastName method

    /// <summary>
    /// Check if a file is a jpg/jpeg file
    /// </summary>
    /// <param name="postedFile">A reference to the posted file</param>
    /// <returns>A boolean that indicates if the image is valid</returns>
    public static bool IsImageJpeg(HttpPostedFileBase postedFile)
    {
        //  Check the image mime types
        if (postedFile.ContentType.ToLower() != "image/jpg" && postedFile.ContentType.ToLower() != "image/jpeg")
        {
            return false;
        }

        //  Check the image extension
        if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg" && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
        {
            return false;
        }

        //  Attempt to read the file and check the first bytes
        try
        {
            if (postedFile.InputStream.CanRead == false)
            {
                return false;
            }

            if (postedFile.ContentLength < 512)
            {
                return false;
            }

            byte[] buffer = new byte[512];
            postedFile.InputStream.Read(buffer, 0, 512);
            string content = System.Text.Encoding.UTF8.GetString(buffer);
            if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
            {
                return false;
            }

        }
        catch (Exception)
        {
            return false;
        }

        //  Try to instantiate new Bitmap, if .NET will throw exception
        //  we can assume that it's not a valid image
        try
        {
            using (var bitmap = new System.Drawing.Bitmap(postedFile.InputStream))
            {
            }
        }
        catch (Exception)
        {
            return false;
        }

        return true;

    } // End of the IsImageJpeg method

    /// <summary>
    /// Remove the key from cache
    /// </summary>
    public static void RemoveKeyFromCache(string key)
    {
        // Make sure that the cache reference not is null
        if (HttpContext.Current.Cache[key] != null)
        {
            // Remove the cached data by key
            HttpContext.Current.Cache.Remove(key);
        }

    } // End of the RemoveKeyFromCache method

} // End of the class