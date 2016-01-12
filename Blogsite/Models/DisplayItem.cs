using System;
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// This class represent a display item
/// </summary>
public class DisplayItem
{
    #region Variables

    public Int32 id;
    public Int32 administrator_id;
    public string title;
    public string description;
    public string page_name;
    public DateTime date_added;
    public DateTime date_updated;
    public decimal rating;
    public Int32 page_views;
    public byte type_code; // 0: category, 1: post
    
    #endregion

    #region Constructors

    /// <summary>
    /// Create a new display item with default properties
    /// </summary>
    public DisplayItem()
    {
        // Set values for instance variables
        this.id = 0;
        this.administrator_id = 0;
        this.title = "";
        this.description = "";
        this.page_name = "";
        this.date_added = new DateTime(2000, 1, 1);
        this.date_updated = new DateTime(2000, 1, 1);
        this.rating = 0;
        this.page_views = 0;
        this.type_code = 0;

    } // End of the constructor

    /// <summary>
    /// Create a new display item from a reader
    /// </summary>
    /// <param name="reader">A reference to a reader</param>
    public DisplayItem(SqlDataReader reader)
    {
        // Set values for instance variables
        this.id = Convert.ToInt32(reader["id"]);
        this.administrator_id = Convert.ToInt32(reader["administrator_id"]);
        this.title = reader["title"].ToString();
        this.description = reader["main_content"].ToString();
        this.page_name = reader["page_name"].ToString();
        this.date_added = Convert.ToDateTime(reader["date_added"]);
        this.date_updated = Convert.ToDateTime(reader["date_updated"]);
        this.rating = Convert.ToDecimal(reader["rating"]);
        this.page_views = Convert.ToInt32(reader["page_views"]);
        this.type_code = Convert.ToByte(reader["type_code"]);

    } // End of the constructor

    #endregion

    #region Count methods

    /// <summary>
    /// Count the number of display items
    /// </summary>
    /// <param name="categoryId">The id of the category</param>
    /// <param name="languageId">The id of the language</param>
    /// <returns>The number of categories as an int</returns>
    public static Int32 GetCount(Int32 categoryId, Int32 languageId)
    {
        // Create the variable to return
        Int32 count = 0;

        // Create the connection string and the select statement
        string connection = Tools.GetConnectionString();
        string sql = "SELECT COUNT(*) FROM (SELECT C.id FROM dbo.categories_detail AS D INNER JOIN "
            + "dbo.categories AS C ON D.category_id = C.id WHERE C.parent_category_id = @category_id "
            + "AND D.language_id = @language_id AND D.inactive = 0 "
            + "UNION ALL "
            + "SELECT P.id FROM dbo.posts_detail AS D INNER JOIN dbo.posts AS P ON D.post_id "
            + "= P.id WHERE P.category_id = @category_id AND D.language_id = @language_id "
            + "AND D.inactive = 0) AS count;";

        // The using block is used to call dispose automatically even if there are an exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there are an exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@category_id", categoryId);
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

    } // End of the GetCount method

    #endregion

    #region Get methods

    /// <summary>
    /// Get all the display items for a category id
    /// </summary>
    /// <param name="categoryId">The id of the category</param>
    /// <param name="languageId">The id of the language</param>
    /// <param name="pageSize">The number of posts on one page</param>
    /// <param name="pageNumber">The page number of a page from 1 and above</param>
    /// <param name="sortField">The field to use for sorting</param>
    /// <param name="sortOrder">The order to use for sorting, ASC or DESC</param>
    /// <returns>A list of display item posts</returns>
    public static List<DisplayItem> GetChunk(Int32 categoryId, Int32 languageId, Int32 pageSize, Int32 pageNumber, string sortField, string sortOrder)
    {
        // Make sure that sort variables are valid
        sortField = Post.GetValidSortField(sortField);
        sortOrder = Post.GetValidSortOrder(sortOrder);

        // Create the list to return
        List<DisplayItem> posts = new List<DisplayItem>(10);

        // Create the connection string and the sql statement.
        string connection = Tools.GetConnectionString();
        string sql = "SELECT C.id AS id, 0 AS administrator_id, D.title AS title, D.main_content AS main_content, D.page_name AS page_name, "
            + "D.date_added AS date_added, D.date_added AS date_updated, 0 AS rating, D.page_views AS page_views, 0 AS type_code "
            + "FROM dbo.categories_detail AS D INNER JOIN dbo.categories AS C ON D.category_id = C.id WHERE C.parent_category_id = @category_id AND " 
            + "D.language_id = @language_id AND D.inactive = 0 "
            + "UNION ALL "
            + "SELECT P.id AS id, P.administrator_id AS administrator_id, D.title AS title, D.main_content AS main_content, D.page_name AS page_name, D.date_added AS date_added, " 
            + "D.date_updated AS date_updated, D.rating AS rating, D.page_views AS page_views, 1 AS type_code FROM dbo.posts_detail AS D INNER JOIN dbo.posts "
            + "AS P ON D.post_id = P.id WHERE P.category_id = @category_id AND D.language_id = @language_id AND D.inactive = 0 "
            + "ORDER BY " + sortField + " " + sortOrder + " OFFSET @pageNumber ROWS FETCH NEXT @pageSize ROWS ONLY;";
        
        // The using block is used to call dispose automatically even if there is a exception.
        using (SqlConnection cn = new SqlConnection(connection))
        {
            // The using block is used to call dispose automatically even if there is a exception.
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                // Add parameters
                cmd.Parameters.AddWithValue("@category_id", categoryId);
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
                        posts.Add(new DisplayItem(reader));
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

    } // End of the GetChunk method

    #endregion

} // End of the class