BEGIN TRANSACTION;

/* DROP PRIMARY KEYS */
ALTER TABLE [dbo].[custom_themes_templates] DROP CONSTRAINT [PK_custom_themes_templates];
ALTER TABLE [dbo].[static_texts] DROP CONSTRAINT [PK_static_texts];
ALTER TABLE [dbo].[website_sessions] DROP CONSTRAINT [PK_website_sessions];
ALTER TABLE [dbo].[website_settings] DROP CONSTRAINT [PK_website_settings];

/* DROP UNIQUE KEYS */
ALTER TABLE [dbo].[administrators] DROP CONSTRAINT [UK_administrators_user_name];
ALTER TABLE [dbo].[categories_detail] DROP CONSTRAINT [UK_categories_detail_page_name];
ALTER TABLE [dbo].[posts_detail] DROP CONSTRAINT [UK_posts_detail_page_name];
ALTER TABLE [dbo].[static_pages_detail] DROP CONSTRAINT [UK_static_pages_detail_page_name];
ALTER TABLE [dbo].[web_domains] DROP CONSTRAINT [UK_web_domains_domain_name];

/* DROP INDEXES */
DROP INDEX [IX_administrators_facebook_id] ON [dbo].[administrators];
DROP INDEX [IX_administrators_google_id] ON [dbo].[administrators];
DROP INDEX [IX_ads_ad_slot] ON [dbo].[ads];
DROP INDEX [IX_categories_detail_language] ON [dbo].[categories_detail];
DROP INDEX [IX_categories_parent_id] ON [dbo].[categories];
DROP INDEX [IX_posts_detail_language] ON [dbo].[posts_detail];
DROP INDEX [IX_posts_comments_search] ON [dbo].[posts_comments];
DROP INDEX [IX_posts_comments_administrator_id] ON [dbo].[posts_comments];
DROP INDEX [IX_posts_comments_post_id] ON [dbo].[posts_comments];
DROP INDEX [IX_posts_files_post_id] ON [dbo].[posts_files];
DROP INDEX [IX_posts_ratings_administrator_id] ON [dbo].[posts_ratings];
DROP INDEX [IX_posts_ratings_post_id] ON [dbo].[posts_ratings];

/* CHANGE COLLATIONS */
ALTER TABLE [dbo].[administrators] ALTER COLUMN [admin_password] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[administrators] ALTER COLUMN [admin_role] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[administrators] ALTER COLUMN [admin_user_name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[administrators] ALTER COLUMN [email] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[administrators] ALTER COLUMN [facebook_user_id] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[administrators] ALTER COLUMN [google_user_id] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[administrators_detail] ALTER COLUMN [author_description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[administrators_detail] ALTER COLUMN [author_name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[ads] ALTER COLUMN [ad_code] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[ads] ALTER COLUMN [ad_slot] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[ads] ALTER COLUMN [name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[categories] ALTER COLUMN [meta_robots] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[categories_detail] ALTER COLUMN [main_content] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[categories_detail] ALTER COLUMN [meta_description] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[categories_detail] ALTER COLUMN [meta_keywords] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[categories_detail] ALTER COLUMN [page_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[categories_detail] ALTER COLUMN [title] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[custom_themes] ALTER COLUMN [name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[custom_themes_templates] ALTER COLUMN [comment] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[custom_themes_templates] ALTER COLUMN [master_file_url] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[custom_themes_templates] ALTER COLUMN [user_file_content] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[custom_themes_templates] ALTER COLUMN [user_file_name] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[languages] ALTER COLUMN [country_code] [nchar] (2) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[languages] ALTER COLUMN [language_code] [nchar] (2) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[languages_detail] ALTER COLUMN [name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[media_files] ALTER COLUMN [media_type] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[media_files] ALTER COLUMN [src] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[media_files] ALTER COLUMN [title] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[posts] ALTER COLUMN [meta_robots] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[posts_comments] ALTER COLUMN [comment_text] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[posts_detail] ALTER COLUMN [main_content] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[posts_detail] ALTER COLUMN [meta_description] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[posts_detail] ALTER COLUMN [meta_keywords] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[posts_detail] ALTER COLUMN [page_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[posts_detail] ALTER COLUMN [title] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[posts_files] ALTER COLUMN [src] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[posts_files] ALTER COLUMN [title] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_pages] ALTER COLUMN [meta_robots] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_pages_detail] ALTER COLUMN [link_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_pages_detail] ALTER COLUMN [main_content] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_pages_detail] ALTER COLUMN [meta_description] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_pages_detail] ALTER COLUMN [meta_keywords] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_pages_detail] ALTER COLUMN [page_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_pages_detail] ALTER COLUMN [title] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_texts] ALTER COLUMN [id] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[static_texts] ALTER COLUMN [value] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [analytics_tracking_id] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [domain_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [facebook_app_id] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [facebook_app_secret] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [google_app_id] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [google_app_secret] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [web_address] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[web_domains] ALTER COLUMN [website_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[weblinks] ALTER COLUMN [rel] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[weblinks] ALTER COLUMN [target] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[weblinks_detail] ALTER COLUMN [link_name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[weblinks_detail] ALTER COLUMN [url] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[website_sessions] ALTER COLUMN [application_name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[website_sessions] ALTER COLUMN [id] [nvarchar] (80) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[website_sessions] ALTER COLUMN [session_items] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[website_settings] ALTER COLUMN [id] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL;
ALTER TABLE [dbo].[website_settings] ALTER COLUMN [value] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL;

/* CREATE PRIMARY KEYS */
ALTER TABLE [dbo].[custom_themes_templates] ADD CONSTRAINT [PK_custom_themes_templates] PRIMARY KEY CLUSTERED ([custom_theme_id] ASC, [user_file_name] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[static_texts] ADD CONSTRAINT [PK_static_texts] PRIMARY KEY CLUSTERED ([id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[website_sessions] ADD CONSTRAINT [PK_website_sessions] PRIMARY KEY CLUSTERED ([id] ASC, [application_name] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[website_settings] ADD CONSTRAINT [PK_website_settings] PRIMARY KEY CLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);

/* CREATE UNIQUE KEYS */
ALTER TABLE [dbo].[administrators] ADD CONSTRAINT [UK_administrators_user_name] UNIQUE NONCLUSTERED ([admin_user_name] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[categories_detail] ADD CONSTRAINT [UK_categories_detail_page_name] UNIQUE NONCLUSTERED ([page_name] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[posts_detail] ADD CONSTRAINT [UK_posts_detail_page_name] UNIQUE NONCLUSTERED ([page_name] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[static_pages_detail] ADD CONSTRAINT [UK_static_pages_detail_page_name] UNIQUE NONCLUSTERED ([page_name] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[web_domains] ADD CONSTRAINT [UK_web_domains_domain_name] UNIQUE NONCLUSTERED ([domain_name] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);

/* CREATE INDEXES */
CREATE NONCLUSTERED INDEX [IX_administrators_facebook_id] ON [dbo].[administrators] ([facebook_user_id] ASC);
CREATE NONCLUSTERED INDEX [IX_administrators_google_id] ON [dbo].[administrators] ([google_user_id] ASC);
CREATE NONCLUSTERED INDEX [IX_ads_ad_slot] ON [dbo].[ads] ([language_id] ASC, [ad_slot] ASC, [inactive] ASC);
CREATE NONCLUSTERED INDEX [IX_categories_detail_language] ON [dbo].[categories_detail] ([language_id] ASC, [inactive] ASC) INCLUDE ([title], [date_added], [page_views]);
CREATE NONCLUSTERED INDEX [IX_categories_parent_id] ON [dbo].[categories] ([parent_category_id] ASC) INCLUDE ([id]);
CREATE NONCLUSTERED INDEX [IX_posts_detail_language] ON [dbo].[posts_detail] ([language_id] ASC, [inactive] ASC) INCLUDE ([title], [page_views], [rating], [date_added], [date_updated]);
CREATE NONCLUSTERED INDEX [IX_posts_comments_search] ON [dbo].[posts_comments] ([id] ASC, [post_id] ASC, [administrator_id] ASC) INCLUDE ([language_id], [comment_date]);
CREATE NONCLUSTERED INDEX [IX_posts_comments_administrator_id] ON [dbo].[posts_comments] ([administrator_id] ASC, [language_id] ASC) INCLUDE ([id], [post_id], [comment_date]);
CREATE NONCLUSTERED INDEX [IX_posts_comments_post_id] ON [dbo].[posts_comments] ([post_id] ASC, [language_id] ASC) INCLUDE ([id], [administrator_id], [comment_date]);
CREATE NONCLUSTERED INDEX [IX_posts_files_post_id] ON [dbo].[posts_files] ([post_id] ASC, [language_id] ASC) INCLUDE ([id], [title]);
CREATE NONCLUSTERED INDEX [IX_posts_ratings_administrator_id] ON [dbo].[posts_ratings] ([administrator_id] ASC, [language_id] ASC) INCLUDE ([post_id], [rating_date], [rating]);
CREATE NONCLUSTERED INDEX [IX_posts_ratings_post_id] ON [dbo].[posts_ratings] ([post_id] ASC, [language_id] ASC) INCLUDE ([administrator_id], [rating_date], [rating]);

/* REBUILD INDEXES */
ALTER INDEX ALL ON [dbo].[administrators] REBUILD;
ALTER INDEX ALL ON [dbo].[administrators_detail] REBUILD;
ALTER INDEX ALL ON [dbo].[ads] REBUILD;
ALTER INDEX ALL ON [dbo].[categories] REBUILD;
ALTER INDEX ALL ON [dbo].[categories_detail] REBUILD;
ALTER INDEX ALL ON [dbo].[custom_themes] REBUILD;
ALTER INDEX ALL ON [dbo].[custom_themes_templates] REBUILD;
ALTER INDEX ALL ON [dbo].[languages] REBUILD;
ALTER INDEX ALL ON [dbo].[languages_detail] REBUILD;
ALTER INDEX ALL ON [dbo].[media_files] REBUILD;
ALTER INDEX ALL ON [dbo].[posts] REBUILD;
ALTER INDEX ALL ON [dbo].[posts_comments] REBUILD;
ALTER INDEX ALL ON [dbo].[posts_detail] REBUILD;
ALTER INDEX ALL ON [dbo].[posts_files] REBUILD;
ALTER INDEX ALL ON [dbo].[posts_ratings] REBUILD;
ALTER INDEX ALL ON [dbo].[static_pages] REBUILD;
ALTER INDEX ALL ON [dbo].[static_pages_detail] REBUILD;
ALTER INDEX ALL ON [dbo].[static_texts] REBUILD;
ALTER INDEX ALL ON [dbo].[web_domains] REBUILD;
ALTER INDEX ALL ON [dbo].[weblinks] REBUILD;
ALTER INDEX ALL ON [dbo].[weblinks_detail] REBUILD;
ALTER INDEX ALL ON [dbo].[website_sessions] REBUILD;
ALTER INDEX ALL ON [dbo].[website_settings] REBUILD;

/* EXCECUTE THE TRANSACTION */
COMMIT TRANSACTION;