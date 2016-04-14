﻿using System;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class handles the settings for the blog
/// </summary>
public class WebsiteSetting
{
    #region Variables

    public string id;
    public string value;

    // Create a static write lock
    private static object writeLock = new object();

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new setting with default properties
    /// </summary>
    public WebsiteSetting()
    {
        // Set values for instance variables
        this.id = "";
        this.value = "";

    } // End of the constructor

    /// <summary>
    /// Create a new setting from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public WebsiteSetting(SqlDataReader reader)
    {
        // Set values for instance variables
        this.id = reader["id"].ToString();
        this.value = reader["value"].ToString();

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one setting
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="value">The value</param>
    public static void Add(string key, string value)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.website_settings (id, value) VALUES (@id, @value);";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", key);
                cmd.Parameters.AddWithValue("@value", value);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection
                    cn.Open();

                    // Execute the insert
                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Clear the cache
        Tools.RemoveKeyFromCache("WebsiteSettings");

    } // End of the Add method

    #endregion

    #region Update methods

    /// <summary>
    /// Update a setting
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="value">The value</param>
    public static void Update(string key, string value)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.website_settings SET value = @value WHERE id = @id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", key);
                cmd.Parameters.AddWithValue("@value", value);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Execute the update
                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Clear the cache
        Tools.RemoveKeyFromCache("WebsiteSettings");

    } // End of the Update method

    #endregion

    #region Get methods

    /// <summary>
    /// Get one value by key
    /// </summary>
    /// <param name="key">The key</param>
    /// <returns>The value as a string</returns>
    public static string GetOneByKey(string key)
    {
        // Create the string to return
        string value = "";

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT value FROM dbo.website_settings WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", key);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Get the value
                    value = cmd.ExecuteScalar().ToString();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the value
        return value;

    } // End of the GetOneByKey method

    /// <summary>
    /// Get all settings from the cache or from the database if the cache is null
    /// </summary>
    /// <returns>A key string list with settings</returns>
    public static KeyStringList GetAllFromCache()
    {
        // Get the cached settings
        KeyStringList settings = (KeyStringList)HttpContext.Current.Cache["WebsiteSettings"];

        // Check if settings is null
        if(settings == null)
        {
            // Add a lock to only insert once
            lock(writeLock)
            {
                // Check if the cache still is null
                if(HttpContext.Current.Cache["WebsiteSettings"] == null)
                {
                    // Get settings from the database
                    settings = GetAll();

                    if (settings != null)
                    {
                        // Create the cache
                        HttpContext.Current.Cache.Insert("WebsiteSettings", settings, null, DateTime.UtcNow.AddHours(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                    }
                }
                else
                {
                    // Get settings from cache
                    settings = (KeyStringList)HttpContext.Current.Cache["WebsiteSettings"];
                }
            }
        }

        // Return the settings for the blog
        return settings;

    } // End of the GetAllFromCache method

    /// <summary>
    /// Get all the settings as a key string list
    /// </summary>
    /// <returns>A key string list of settings</returns>
    public static KeyStringList GetAll()
    {
        // Create the KeyStringList to return
        KeyStringList posts = new KeyStringList(20);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.website_settings ORDER BY id ASC;";

        // The using block is used to call dispose automatically even if there are an exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read
                    while (reader.Read())
                    {
                        posts.Add(reader["id"].ToString(), reader["value"].ToString());
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the list of posts
        return posts;

    } // End of the GetAll method

    #endregion

} // End of the class
