using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class represent a administrator
/// </summary>
public class Administrator
{
    #region Variables

    public Int32 id;
    public string admin_user_name;
    public string admin_password;
    public string admin_role;
    public string email;
    public string author_name;
    public string author_description;
    public string facebook_user_id;
    public string google_user_id;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new administrator with default properties
    /// </summary>
    public Administrator()
    {
        // Set values for instance variables
        this.id = 0;
        this.admin_user_name = "";
        this.admin_password = "";
        this.admin_role = "";
        this.email = "";
        this.facebook_user_id = "";
        this.google_user_id = "";
        this.author_name = "";
        this.author_description = "";

    } // End of the constructor

    /// <summary>
    /// Create a new administrator from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public Administrator(SqlDataReader reader)
    {
        // Set values for instance variables
        this.id = Convert.ToInt32(reader["id"]);
        this.admin_user_name = reader["admin_user_name"].ToString();
        this.admin_password = "";
        this.admin_role = reader["admin_role"].ToString();
        this.email = reader["email"].ToString();
        this.facebook_user_id = reader["facebook_user_id"].ToString();
        this.google_user_id = reader["google_user_id"].ToString();
        this.author_name = reader["author_name"].ToString();
        this.author_description = reader["author_description"].ToString();
        
    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one master post
    /// </summary>
    /// <param name="post">A reference to a administrator post</param>
    public static long AddMasterPost(Administrator post)
    {
        // Create the long to return
        long idOfInsert = 0;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.administrators (admin_user_name, admin_password, admin_role, email, facebook_user_id, google_user_id) "
            + "VALUES (@admin_user_name, @admin_password, @admin_role, @email, @facebook_user_id, @google_user_id);SELECT SCOPE_IDENTITY();";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The Using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@admin_user_name", post.admin_user_name);
                cmd.Parameters.AddWithValue("@admin_password", "");
                cmd.Parameters.AddWithValue("@admin_role", post.admin_role);
                cmd.Parameters.AddWithValue("@email", post.email);
                cmd.Parameters.AddWithValue("@facebook_user_id", post.facebook_user_id);
                cmd.Parameters.AddWithValue("@google_user_id", post.google_user_id);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection
                    cn.Open();

                    // Execute the insert
                    idOfInsert = Convert.ToInt64(cmd.ExecuteScalar());

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the id of the inserted item
        return idOfInsert;

    } // End of the AddMasterPost method

    /// <summary>
    /// Add one language post
    /// </summary>
    /// <param name="post">A reference to a post</param>
    /// <param name="languageId">A language id</param>
    public static void AddLanguagePost(Administrator post, Int32 languageId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.administrators_detail (administrator_id, language_id, author_name, author_description) "
            + "VALUES (@administrator_id, @language_id, @author_name, @author_description);";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@administrator_id", post.id);
                cmd.Parameters.AddWithValue("@language_id", languageId);
                cmd.Parameters.AddWithValue("@author_name", post.author_name);
                cmd.Parameters.AddWithValue("@author_description", post.author_description);

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

    } // End of the AddLanguagePost method

    #endregion

    #region Update methods

    /// <summary>
    /// Update a master post
    /// </summary>
    /// <param name="post">A reference to a post</param>
    public static void UpdateMasterPost(Administrator post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.administrators SET admin_user_name = @admin_user_name, admin_role = @admin_role, " 
            + "email = @email, facebook_user_id = @facebook_user_id, google_user_id = @google_user_id WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The Using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@admin_user_name", post.admin_user_name);
                cmd.Parameters.AddWithValue("@admin_role", post.admin_role);
                cmd.Parameters.AddWithValue("@email", post.email);
                cmd.Parameters.AddWithValue("@facebook_user_id", post.facebook_user_id);
                cmd.Parameters.AddWithValue("@google_user_id", post.google_user_id);

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

    } // End of the UpdateMasterPost method

    /// <summary>
    /// Update a language post
    /// </summary>
    /// <param name="post">A reference to a post</param>
    /// <param name="languageId">A language id</param>
    public static void UpdateLanguagePost(Administrator post, Int32 languageId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.administrators_detail SET author_name = @author_name, author_description = @author_description "
            + "WHERE administrator_id = @administrator_id AND language_id = @language_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@administrator_id", post.id);
                cmd.Parameters.AddWithValue("@language_id", languageId);
                cmd.Parameters.AddWithValue("@author_name", post.author_name);
                cmd.Parameters.AddWithValue("@author_description", post.author_description);

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

    } // End of the UpdateLanguagePost method

    /// <summary>
    /// Update the password for a administrator
    /// </summary>
    /// <param name="id">The id of the administrator</param>
    /// <param name="password">The new password</param>
    public static void UpdatePassword(Int32 id, string password)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.administrators SET admin_password = @admin_password WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@admin_password", password);

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

    } // End of the UpdatePassword method

    #endregion

    #region Count methods

    /// <summary>
    /// Count the number of administrators by search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="languageId">The language id</param>
    /// <returns>The number of posts as an int</returns>
    public static Int32 GetCountBySearch(string[] keywords, Int32 languageId)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.administrators_detail AS D INNER JOIN dbo.administrators AS A " 
            + "ON D.administrator_id = A.id WHERE D.language_id = @language_id";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (CAST(A.id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + " OR A.admin_user_name LIKE @keyword_" + i.ToString()
                + " OR D.author_name LIKE @keyword_" + i.ToString() + ")";
        }

        // Add the final touch to the sql string
        sql += ";";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);

                // Add parameters for search keywords
                for (int i = 0; i < keywords.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@keyword_" + i.ToString(), "%" + keywords[i].ToString() + "%");
                }

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection
                    cn.Open();

                    // Execute the select statment
                    count = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the count
        return count;

    } // End of the GetCountBySearch method

    #endregion

    #region Authorization

    /// <summary>
    /// Check if the password is correct
    /// </summary>
    /// <param name="userName">The user name</param>
    /// <param name="password">The password</param>
    /// <returns>A boolean that indicates if the password is correct</returns>
    public static bool ValidatePassword(string userName, string password)
    {
        // Create a hash of the password
        string passwordHash = "";

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT admin_password FROM dbo.administrators WHERE admin_user_name = @admin_user_name;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add a parameters
                cmd.Parameters.AddWithValue("@admin_user_name", userName);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Get the password hash
                    passwordHash = cmd.ExecuteScalar().ToString();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the boolean that indicates if the password is correct
        return PasswordHash.ValidatePassword(password, passwordHash);

    } // End of the ValidatePassword method

    /// <summary>
    /// Check if the administrator is authorized
    /// </summary>
    /// <param name="allowedRoles">The roles that are allowed</param>
    /// <returns>A boolean that indicates if the administrator is authorized</returns>
    public static bool IsAuthorized(string[] allowedRoles)
    {
        // Create the boolean to return
        bool isAuthorized = false;

        // Get the signed in administrator
        Administrator administrator = Administrator.GetSignedInAdministrator();

        // Make sure that the administrator not is null
        if (administrator != null)
        {
            for (int i = 0; i < allowedRoles.Length; i++)
            {
                if (allowedRoles[i] == administrator.admin_role)
                {
                    isAuthorized = true;
                }
            }
        }

        // Return the boolean
        return isAuthorized;

    } // End of the IsAuthorized method

    /// <summary>
    /// Check if the administrator is authorized
    /// </summary>
    /// <param name="allowedRoles">The roles that are allowed</param>
    /// <param name="administratorRole">The admin role for the administrator</param>
    /// <returns>A boolean that indicates if the administrator is authorized</returns>
    public static bool IsAuthorized(string[] allowedRoles, string administratorRole)
    {
        // Create the boolean to return
        bool isAuthorized = false;

        // Check if the administrator is authorized
        for (int i = 0; i < allowedRoles.Length; i++)
        {
            if (allowedRoles[i] == administratorRole)
            {
                isAuthorized = true;
            }
        }

        // Return the boolean
        return isAuthorized;

    } // End of the IsAuthorized method

    /// <summary>
    /// Get all the administrator roles
    /// </summary>
    /// <returns>A string array with administrator roles</returns>
    public static string[] GetAllAdminRoles()
    {
        return new string[] { "Administrator", "Editor", "Translator", "Author", "Demo" };

    } // End of the GetAllAdminRoles method

    /// <summary>
    /// Get all the roles
    /// </summary>
    /// <returns>A string array with roles</returns>
    public static string[] GetAllRoles()
    {
        return new string[] { "Administrator", "Editor", "Translator", "Author", "Demo", "User" };

    } // End of the GetAllRoles method

    #endregion

    #region Get methods

    /// <summary>
    /// Check if a master post exists
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A boolean that indicates if the post exists</returns>
    public static bool MasterPostExists(Int32 id)
    {
        // Create the boolean to return
        bool postExists = false;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS COUNT FROM dbo.administrators WHERE id = @id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add a parameters
                cmd.Parameters.AddWithValue("@id", id);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Check if the post exist
                    postExists = Convert.ToInt32(cmd.ExecuteScalar()) > 0 ? true : false;

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the boolean
        return postExists;

    } // End of the MasterPostExists method

    /// <summary>
    /// Get one administrator based on id
    /// </summary>
    /// <param name="id">The id for the post</param>
    /// <returns>A reference to a administrator post</returns>
    public static Administrator GetOneById(Int32 id)
    {
        // Create the post to return
        Administrator post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT id, admin_user_name, admin_password, admin_role, email, admin_user_name AS author_name, admin_role AS author_description, "
            + "facebook_user_id, google_user_id FROM dbo.administrators WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);

                // Create a SqlDataReader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with one row of data.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read and add values
                    while (reader.Read())
                    {
                        post = new Administrator(reader);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the post
        return post;

    } // End of the GetOneById method

    /// <summary>
    /// Get one administrator based on id
    /// </summary>
    /// <param name="id">The id for the post</param>
    /// <param name="languageId">The language id</param>
    /// <returns>A reference to a administrator post</returns>
    public static Administrator GetOneById(Int32 id, Int32 languageId)
    {
        // Create the post to return
        Administrator post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.administrators_detail AS D INNER JOIN dbo.administrators AS A ON D.administrator_id = A.id " 
            + "WHERE A.id = @id AND D.language_id = @language_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@language_id", languageId);

                // Create a SqlDataReader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with one row of data.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read and add values
                    while (reader.Read())
                    {
                        post = new Administrator(reader);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the post
        return post;

    } // End of the GetOneById method

    /// <summary>
    /// Get one administrator based on facebook user id
    /// </summary>
    /// <param name="facebookUserId">A facebook user id</param>
    /// <param name="languageId">The language id</param>
    /// <returns>A reference to a administrator post</returns>
    public static Administrator GetOneByFacebookUserId(string facebookUserId, Int32 languageId)
    {
        // Create the post to return
        Administrator post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.administrators_detail AS D INNER JOIN dbo.administrators AS A ON D.administrator_id = A.id "
            + "WHERE A.facebook_user_id = @facebook_user_id AND D.language_id = @language_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@facebook_user_id", facebookUserId);
                cmd.Parameters.AddWithValue("@language_id", languageId);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with one row of data.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read and add values
                    while (reader.Read())
                    {
                        post = new Administrator(reader);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the post
        return post;

    } // End of the GetOneByFacebookUserId method

    /// <summary>
    /// Get one administrator based on google user id
    /// </summary>
    /// <param name="googleUserId">A google user id</param>
    /// <param name="languageId">The language id</param>
    /// <returns>A reference to a administrator post</returns>
    public static Administrator GetOneByGoogleUserId(string googleUserId, Int32 languageId)
    {
        // Create the post to return
        Administrator post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.administrators_detail AS D INNER JOIN dbo.administrators AS A ON D.administrator_id = A.id "
            + "WHERE A.google_user_id = @google_user_id AND D.language_id = @language_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@google_user_id", googleUserId);
                cmd.Parameters.AddWithValue("@language_id", languageId);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with one row of data.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read and add values
                    while (reader.Read())
                    {
                        post = new Administrator(reader);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the post
        return post;

    } // End of the GetOneByGoogleUserId method

    /// <summary>
    /// Get one administrator based on user name
    /// </summary>
    /// <param name="userName">The user name</param>
    /// <returns>A reference to a post</returns>
    public static Administrator GetOneByUserName(string userName)
    {
        // Create the post to return
        Administrator post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT id, admin_user_name, admin_password, admin_role, email, admin_user_name AS author_name, admin_role AS author_description, " 
            + "facebook_user_id, google_user_id FROM dbo.administrators WHERE admin_user_name = @admin_user_name;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The Using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@admin_user_name", userName);

                // Create a MySqlDataReader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with one row of data.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read and add values
                    while (reader.Read())
                    {
                        post = new Administrator(reader);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the post
        return post;

    } // End of the GetOneByUserName method

    /// <summary>
    /// Get one administrator based on user name
    /// </summary>
    /// <param name="userName">The user name for the post</param>
    /// <param name="languageId">The language id</param>
    /// <returns>A reference to a post</returns>
    public static Administrator GetOneByUserName(string userName, Int32 languageId)
    {
        // Create the post to return
        Administrator post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.administrators_detail AS D INNER JOIN dbo.administrators AS A ON D.administrator_id = A.id " 
            + "WHERE A.admin_user_name = @admin_user_name AND D.language_id = @language_id;";
         
        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The Using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@admin_user_name", userName);
                cmd.Parameters.AddWithValue("@language_id", languageId);

                // Create a MySqlDataReader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with one row of data.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read and add values
                    while (reader.Read())
                    {
                        post = new Administrator(reader);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the post
        return post;

    } // End of the GetOneByUserName method

    /// <summary>
    /// Get administrators by search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="languageId">The language id</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order, ASC or DESC</param>
    /// <returns>A list of administrators</returns>
    public static List<Administrator> GetBySearch(string[] keywords, Int32 languageId, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Administrator> posts = new List<Administrator>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.administrators_detail AS D INNER JOIN dbo.administrators AS A " 
            + "ON D.administrator_id = A.id WHERE D.language_id = @language_id";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (CAST(A.id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + " OR A.admin_user_name LIKE @keyword_" + i.ToString()
                + " OR D.author_name LIKE @keyword_" + i.ToString() + ")";
        }

        // Add the final touch to the select string
        sql += " ORDER BY " + sortField + " " + sortOrder + " OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);
                cmd.Parameters.AddWithValue("@pageNumber", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                // Add parameters for search keywords
                for (int i = 0; i < keywords.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@keyword_" + i.ToString(), "%" + keywords[i].ToString() + "%");
                }

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
                    while (reader.Read())
                    {
                        posts.Add(new Administrator(reader));
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Call Close when done reading to avoid memory leakage.
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        // Return the list of posts
        return posts;

    } // End of the GetBySearch method

    /// <summary>
    /// Get the signed-in administrator
    /// </summary>
    /// <returns>A reference to a administrator</returns>
    public static Administrator GetSignedInAdministrator()
    {
        // Create the administrator to return
        Administrator administrator = null;

        // Get the administrator cookie
        HttpCookie administratorCookie = HttpContext.Current.Request.Cookies.Get("Administrator");

        if (administratorCookie != null)
        {
            Int32 id = 0;
            Int32.TryParse(Tools.UnprotectCookieValue(administratorCookie.Value, "Administration"), out id);
            administrator = Administrator.GetOneById(id);
        }

        // Return the administrator
        return administrator;

    } // End of the GetSignedInAdministrator method

    /// <summary>
    /// Get the signed-in administrator
    /// </summary>
    /// <param name="languageId">The language id</param>
    /// <returns>A reference to a administrator</returns>
    public static Administrator GetSignedInAdministrator(Int32 languageId)
    {
        // Create the administrator to return
        Administrator administrator = null;

        // Get the administrator cookie
        HttpCookie administratorCookie = HttpContext.Current.Request.Cookies.Get("Administrator");

        if (administratorCookie != null)
        {
            Int32 id = 0;
            Int32.TryParse(Tools.UnprotectCookieValue(administratorCookie.Value, "Administration"), out id);
            administrator = Administrator.GetOneById(id, languageId);
        }

        // Return the administrator
        return administrator;

    } // End of the GetSignedInAdministrator method

    #endregion

    #region Delete methods

    /// <summary>
    /// Delete a administrator post on id
    /// </summary>
    /// <param name="id">The id of the administrator post</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 id)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.administrators WHERE id = @id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Execute the update
                    cmd.ExecuteNonQuery();

                }
                catch (SqlException e)
                {
                    // Check for a foreign key constraint error
                    if (e.Number == 547)
                    {
                        return 5;
                    }
                    else
                    {
                        throw e;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the code for success
        return 0;

    } // End of the DeleteOnId method

    /// <summary>
    /// Delete a language post on id
    /// </summary>
    /// <param name="id">The id</param>
    /// <param name="languageId">The language id</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteLanguagePostOnId(Int32 id, Int32 languageId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.administrators_detail WHERE administrator_id = @id AND language_id = @language_id;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@language_id", languageId);

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases.
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Execute the update
                    cmd.ExecuteNonQuery();

                }
                catch (SqlException e)
                {
                    // Check for a foreign key constraint error
                    if (e.Number == 547)
                    {
                        return 5;
                    }
                    else
                    {
                        throw e;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Return the code for success
        return 0;

    } // End of the DeleteLanguagePostOnId method

    #endregion

    #region Validation

    /// <summary>
    /// Get a valid sort field
    /// </summary>
    /// <param name="sortField">The sort field</param>
    /// <returns>A valid sort field as a string</returns>
    public static string GetValidSortField(string sortField)
    {
        // Make sure that the sort field is valid
        if (sortField != "id" && sortField != "admin_user_name" && sortField != "admin_role" && sortField != "author_name")
        {
            sortField = "id";
        }

        // Return the string
        return sortField;

    } // End of the GetValidSortField method

    /// <summary>
    /// Get a valid sort order
    /// </summary>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A valid sort order as a string</returns>
    public static string GetValidSortOrder(string sortOrder)
    {
        // Make sure that the sort order is valid
        if (sortOrder != "ASC" && sortOrder != "DESC")
        {
            sortOrder = "ASC";
        }

        // Return the string
        return sortOrder;

    } // End of the GetValidSortOrder method

    #endregion

} // End of the class
