using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This class represents a blog post
/// </summary>
public class Post
{
    #region Variables

    public Int32 id;
    public Int32 category_id;
    public Int32 administrator_id;
    public string meta_robots;
    public string title;
    public string main_content;
    public string meta_description;
    public string meta_keywords;
    public string page_name;
    public DateTime date_added;
    public DateTime date_updated;
    public decimal rating;
    public Int32 page_views;
    public bool inactive;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new post with default properties
    /// </summary>
    public Post()
    {
        // Set values for instance variables
        this.id = 0;
        this.category_id = 0;
        this.administrator_id = 0;
        this.meta_robots = "";
        this.title = "";
        this.main_content = "";
        this.meta_description = "";
        this.meta_keywords = "";
        this.page_name = "";
        this.date_added = DateTime.Now;
        this.date_updated = DateTime.Now;
        this.rating = 0;
        this.page_views = 0;
        this.inactive = false;

    } // End of the constructor

    /// <summary>
    /// Create a new post from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public Post(SqlDataReader reader)
    {
        // Set values for instance variables
        this.id = Convert.ToInt32(reader["id"]);
        this.category_id = Convert.ToInt32(reader["category_id"]);
        this.administrator_id = Convert.ToInt32(reader["administrator_id"]);
        this.meta_robots = reader["meta_robots"].ToString();
        this.title = reader["title"].ToString();
        this.main_content = reader["main_content"].ToString();
        this.meta_description = reader["meta_description"].ToString();
        this.meta_keywords = reader["meta_keywords"].ToString();
        this.page_name = reader["page_name"].ToString();
        this.date_added = Convert.ToDateTime(reader["date_added"]);
        this.date_updated = Convert.ToDateTime(reader["date_updated"]);
        this.rating = Convert.ToDecimal(reader["rating"]);
        this.page_views = Convert.ToInt32(reader["page_views"]);
        this.inactive = Convert.ToBoolean(reader["inactive"]);

    } // End of the constructor

    #endregion

    #region Insert methods

    /// <summary>
    /// Add one master post
    /// </summary>
    /// <param name="post">A reference to a post</param>
    public static long AddMasterPost(Post post)
    {
        // Create the long to return
        long idOfInsert = 0;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.posts (category_id, administrator_id, meta_robots) "
            + "VALUES (@category_id, @administrator_id, @meta_robots);SELECT SCOPE_IDENTITY();";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@category_id", post.category_id);
                cmd.Parameters.AddWithValue("@administrator_id", post.administrator_id);
                cmd.Parameters.AddWithValue("@meta_robots", post.meta_robots);

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
    public static void AddLanguagePost(Post post, Int32 languageId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "INSERT INTO dbo.posts_detail (post_id, language_id, title, main_content, meta_description, " 
            + "meta_keywords, page_name, date_added, date_updated, rating, page_views, inactive) "
            + "VALUES (@post_id, @language_id, @title, @main_content, @meta_description, @meta_keywords, " 
            + "@page_name, @date_added, @date_updated, @rating, @page_views, @inactive);";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@post_id", post.id);
                cmd.Parameters.AddWithValue("@language_id", languageId);
                cmd.Parameters.AddWithValue("@title", post.title);
                cmd.Parameters.AddWithValue("@main_content", post.main_content);
                cmd.Parameters.AddWithValue("@meta_description", post.meta_description);
                cmd.Parameters.AddWithValue("@meta_keywords", post.meta_keywords);
                cmd.Parameters.AddWithValue("@page_name", post.page_name);
                cmd.Parameters.AddWithValue("@date_added", post.date_added);
                cmd.Parameters.AddWithValue("@date_updated", post.date_updated);
                cmd.Parameters.AddWithValue("@rating", post.rating);
                cmd.Parameters.AddWithValue("@page_views", post.page_views);
                cmd.Parameters.AddWithValue("@inactive", post.inactive);

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
    public static void UpdateMasterPost(Post post)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.posts SET category_id = @category_id, administrator_id = @administrator_id, "
            + "meta_robots = @meta_robots WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", post.id);
                cmd.Parameters.AddWithValue("@category_id", post.category_id);
                cmd.Parameters.AddWithValue("@administrator_id", post.administrator_id);
                cmd.Parameters.AddWithValue("@meta_robots", post.meta_robots);

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
    public static void UpdateLanguagePost(Post post, int languageId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.posts_detail SET title = @title, main_content = @main_content, meta_description = @meta_description, "
            + "meta_keywords = @meta_keywords, page_name = @page_name, date_added = @date_added, date_updated = @date_updated, "
            + "rating = @rating, page_views = @page_views, inactive = @inactive WHERE post_id = @post_id AND language_id = @language_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@post_id", post.id);
                cmd.Parameters.AddWithValue("@language_id", languageId);
                cmd.Parameters.AddWithValue("@title", post.title);
                cmd.Parameters.AddWithValue("@main_content", post.main_content);
                cmd.Parameters.AddWithValue("@meta_description", post.meta_description);
                cmd.Parameters.AddWithValue("@meta_keywords", post.meta_keywords);
                cmd.Parameters.AddWithValue("@page_name", post.page_name);
                cmd.Parameters.AddWithValue("@date_added", post.date_added);
                cmd.Parameters.AddWithValue("@date_updated", post.date_updated);
                cmd.Parameters.AddWithValue("@rating", post.rating);
                cmd.Parameters.AddWithValue("@page_views", post.page_views);
                cmd.Parameters.AddWithValue("@inactive", post.inactive);

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
    /// Update page views for the post
    /// </summary>
    /// <param name="id">The id for the product</param>
    /// <param name="language_id">The language id</param>
    /// <param name="pageViews">The total number of page views</param>
    public static void UpdatePageviews(Int32 id, Int32 language_id, Int32 pageViews)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.posts_detail SET page_views = @page_views WHERE post_id = @post_id " 
            + "AND language_id = @language_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@post_id", id);
                cmd.Parameters.AddWithValue("@language_id", language_id);
                cmd.Parameters.AddWithValue("@page_views", pageViews);

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

    } // End of the UpdatePageviews method

    /// <summary>
    /// Update the rating for the post
    /// </summary>
    /// <param name="id">The id</param>
    /// <param name="languageId">The language id</param>
    public static void UpdateRating(Int32 id, Int32 languageId)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "UPDATE dbo.posts_detail SET rating = ISNULL((SELECT (SUM(rating) / COUNT(post_id)) AS rating "
            + "FROM dbo.posts_ratings WHERE post_id = @post_id AND language_id = @language_id),0) "
            + "WHERE post_id = @post_id AND language_id = @language_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@post_id", id);
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
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

    } // End of the UpdateRating method

    #endregion

    #region Count methods

    /// <summary>
    /// Count the number of active posts
    /// </summary>
    /// <param name="languageId">The id of the language</param>
    /// <returns>The number of posts as an int</returns>
    public static Int32 GetActiveCount(Int32 languageId)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id = P.id "
            + "WHERE D.language_id = @language_id AND D.inactive = 0;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);

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

    } // End of the GetActiveCount method

    /// <summary>
    /// Count the number of posts by search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="languageId">The id of the language</param>
    /// <returns>The number of posts as an int</returns>
    public static Int32 GetCountBySearch(string[] keywords, Int32 languageId)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id = P.id "
            + "WHERE D.language_id = @language_id";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (CAST(P.id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + " OR D.title LIKE @keyword_" + i.ToString()
                + " OR D.meta_description LIKE @keyword_" + i.ToString() + ")";
        }

        // Add the final touch to the sql string
        sql += ";";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
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

    /// <summary>
    /// Count the number of active posts by search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="languageId">The id of the language</param>
    /// <returns>The number of posts as an int</returns>
    public static Int32 GetActiveCountBySearch(string[] keywords, Int32 languageId)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id = P.id "
            + "WHERE D.language_id = @language_id AND D.inactive = 0";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (D.title LIKE @keyword_" + i.ToString() + " OR D.meta_description LIKE @keyword_" + i.ToString() 
                + " OR D.meta_keywords LIKE @keyword_" + i.ToString() + ")";
        }

        // Add the final touch to the sql string
        sql += ";";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
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

    } // End of the GetActiveCountBySearch method

    /// <summary>
    /// Count the number of posts by administrator id
    /// </summary>
    /// <param name="administratorId">The id of the administrator</param>
    /// <param name="languageId">The id of the language</param>
    /// <returns>The number of posts as an int</returns>
    public static Int32 GetCountByAdministratorId(Int32 administratorId, Int32 languageId)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id = P.id "
            + "WHERE P.administrator_id = @administrator_id AND D.language_id = @language_id;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@administrator_id", administratorId);
                cmd.Parameters.AddWithValue("@language_id", languageId);

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

    } // End of the GetCountByAdministratorId method

    /// <summary>
    /// Count the number of active posts by administrator id
    /// </summary>
    /// <param name="administratorId">The id of the administrator</param>
    /// <param name="languageId">The id of the language</param>
    /// <returns>The number of posts as an int</returns>
    public static Int32 GetActiveCountByAdministratorId(Int32 administratorId, Int32 languageId)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(id) AS count FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id = P.id "
            + "WHERE P.administrator_id = @administrator_id AND D.language_id = @language_id AND D.inactive = 0;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@administrator_id", administratorId);
                cmd.Parameters.AddWithValue("@language_id", languageId);

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

    } // End of the GetActiveCountByAdministratorId method

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
        string sql = "SELECT COUNT(id) AS COUNT FROM dbo.posts WHERE id = @id;";

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
    /// Get one post based on id
    /// </summary>
    /// <param name="id">The id</param>
    /// <param name="languageId">The language id</param>
    /// <returns>A reference to a post</returns>
    public static Post GetOneById(Int32 id, Int32 languageId)
    {
        // Create the post to return
        Post post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id = P.id " 
            + "WHERE D.post_id = @id AND D.language_id = @language_id;";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@language_id", languageId);

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with one row of data.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read and add values
                    while (reader.Read())
                    {
                        post = new Post(reader);
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
    /// Get one post based on page name
    /// </summary>
    /// <param name="pageName">A page name</param>
    /// <returns>A reference to a  post</returns>
    public static Post GetOneByPageName(string pageName, Int32 languageId)
    {
        // Create the post to return
        Post post = null;

        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id "
            + "= P.id WHERE D.page_name = @page_name AND D.language_id = @language_id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add a parameters
                cmd.Parameters.AddWithValue("@page_name", pageName);
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
                        post = new Post(reader);
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

    } // End of the GetOneByPageName method

    /// <summary>
    /// Get all posts
    /// </summary>
    /// <param name="languageId">The id of the language</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of posts</returns>
    public static List<Post> GetAll(Int32 languageId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Post> posts = new List<Post>(10);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id = P.id "
            + "WHERE D.language_id = @language_id ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);

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
                        posts.Add(new Post(reader));
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

    } // End of the GetAll method

    /// <summary>
    /// Get all active posts
    /// </summary>
    /// <param name="languageId">The id of the language</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of posts</returns>
    public static List<Post> GetAllActive(Int32 languageId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Post> posts = new List<Post>(10);

        // Create the connection string and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id = P.id "
            + "WHERE D.language_id = @language_id AND D.inactive = 0 ORDER BY " 
            + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there is a exception
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@language_id", languageId);

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
                        posts.Add(new Post(reader));
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

    } // End of the GetAllActive method

    /// <summary>
    /// Get active posts
    /// </summary>
    /// <param name="languageId">The id of the language</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order ASC or DESC</param>
    /// <returns>A list of posts</returns>
    public static List<Post> GetActive(Int32 languageId, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Post> posts = new List<Post>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id = P.id "
            + "WHERE D.language_id = @language_id AND D.inactive = 0 ORDER BY " + sortField + " " 
            + sortOrder + " OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

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

                // Create a reader
                SqlDataReader reader = null;

                // The Try/Catch/Finally statement is used to handle unusual exceptions in the code to
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
                    while (reader.Read())
                    {
                        posts.Add(new Post(reader));
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

    } // End of the GetActive method

    /// <summary>
    /// Get posts for a specific category
    /// </summary>
    /// <param name="categoryId">The id of the category</param>
    /// <param name="languageId">The id of the language</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of posts</returns>
    public static List<Post> GetByCategoryId(Int32 categoryId, Int32 languageId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Post> posts = new List<Post>(10);

        // Create the connection string and the sql statement.
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id = P.id "
            + "WHERE P.category_id = @category_id AND D.language_id = @language_id "
            + "ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@category_id", categoryId);
                cmd.Parameters.AddWithValue("@language_id", languageId);

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
                        posts.Add(new Post(reader));
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

    } // End of the GetByCategoryId method

    /// <summary>
    /// Get active posts for a specific category
    /// </summary>
    /// <param name="categoryId">The id of the category</param>
    /// <param name="languageId">The id of the language</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of posts</returns>
    public static List<Post> GetActiveByCategoryId(Int32 categoryId, Int32 languageId, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Post> posts = new List<Post>(10);

        // Create the connection string and the sql statement.
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id = P.id "
            + "WHERE P.category_id = @category_id AND D.language_id = @language_id AND D.inactive = 0 "
            + "ORDER BY " + sortField + " " + sortOrder + ";";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Add parameters
                cmd.Parameters.AddWithValue("@category_id", categoryId);
                cmd.Parameters.AddWithValue("@language_id", languageId);

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
                        posts.Add(new Post(reader));
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

    } // End of the GetActiveByCategoryId method

    /// <summary>
    /// Get posts by a administrator
    /// </summary>
    /// <param name="administratorId">The id of the administrator</param>
    /// <param name="languageId">The id of the language</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of posts</returns>
    public static List<Post> GetByAdministratorId(Int32 administratorId, Int32 languageId, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Post> posts = new List<Post>(10);

        // Create the connection string and the sql statement.
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id = P.id "
            + "WHERE P.administrator_id = @administrator_id AND D.language_id = @language_id "
            + "ORDER BY " + sortField + " " + sortOrder + " OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@administrator_id", administratorId);
                cmd.Parameters.AddWithValue("@language_id", languageId);
                cmd.Parameters.AddWithValue("@pageNumber", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

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
                        posts.Add(new Post(reader));
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

    } // End of the GetByAdministratorId method

    /// <summary>
    /// Get active posts by administrator id
    /// </summary>
    /// <param name="administratorId">The id of the administrator</param>
    /// <param name="languageId">The id of the language</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order</param>
    /// <returns>A list of posts</returns>
    public static List<Post> GetActiveByAdministratorId(Int32 administratorId, Int32 languageId, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Post> posts = new List<Post>(10);

        // Create the connection string and the sql statement.
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id = P.id "
            + "WHERE P.administrator_id = @administrator_id AND D.language_id = @language_id AND D.inactive = 0 "
            + "ORDER BY " + sortField + " " + sortOrder + " OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";

        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@administrator_id", administratorId);
                cmd.Parameters.AddWithValue("@language_id", languageId);
                cmd.Parameters.AddWithValue("@pageNumber", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

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
                        posts.Add(new Post(reader));
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

    } // End of the GetActiveByAdministratorId method

    /// <summary>
    /// Get posts that contains search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="languageId">The id of the language</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order ASC or DESC</param>
    /// <returns>A list of posts</returns>
    public static List<Post> GetBySearch(string[] keywords, Int32 languageId, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Post> posts = new List<Post>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id = P.id "
            + "WHERE D.language_id = @language_id";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (CAST(P.id AS nvarchar(20)) LIKE @keyword_" + i.ToString() + " OR D.title LIKE @keyword_" + i.ToString()
                + " OR D.meta_description LIKE @keyword_" + i.ToString() + ")";
        }

        // Add the final touch to the sql string
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
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
                    while (reader.Read())
                    {
                        posts.Add(new Post(reader));
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
    /// Get active posts that contains search keywords
    /// </summary>
    /// <param name="keywords">An array of keywords</param>
    /// <param name="languageId">The id of the language</param>
    /// <param name="pageSize">The number of pages on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort order ASC or DESC</param>
    /// <returns>A list of posts</returns>
    public static List<Post> GetActiveBySearch(string[] keywords, Int32 languageId, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = GetValidSortField(sortField);
        sortOrder = GetValidSortOrder(sortOrder);

        // Create the list to return
        List<Post> posts = new List<Post>(pageSize);

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT * FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id = P.id "
            + "WHERE D.language_id = @language_id AND D.inactive = 0 ";

        // Append keywords to the sql string
        for (int i = 0; i < keywords.Length; i++)
        {
            sql += " AND (D.title LIKE @keyword_" + i.ToString() + " OR D.meta_description LIKE @keyword_" + i.ToString()
                + " OR D.meta_keywords LIKE @keyword_" + i.ToString() + ")";
        }

        // Add the final touch to the sql string
        sql += "ORDER BY " + sortField + " " + sortOrder + " OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";


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
                // avoid having our application crash in such cases
                try
                {
                    // Open the connection.
                    cn.Open();

                    // Fill the reader with data from the select command.
                    reader = cmd.ExecuteReader();

                    // Loop through the reader as long as there is something to read.
                    while (reader.Read())
                    {
                        posts.Add(new Post(reader));
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

    } // End of the GetActiveBySearch method

    #endregion

    #region Delete methods

    /// <summary>
    /// Delete a master post on id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>An error code</returns>
    public static Int32 DeleteOnId(Int32 id)
    {
        // Create the connection and the sql statement
        string connection = Tools.GetConnectionString();
        string sql = "DELETE FROM dbo.posts WHERE id = @id;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
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
        string sql = "DELETE FROM dbo.posts_detail WHERE post_id = @id AND language_id = @language_id;";

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
        if (sortField != "id" && sortField != "title" && sortField != "page_views" && sortField != "rating" 
            && sortField != "date_added" && sortField != "date_updated" && sortField != "inactive")
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