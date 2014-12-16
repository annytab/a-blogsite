BEGIN TRANSACTION;

/* CREATE TABLES */
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
 CREATE TABLE [dbo].[administrators](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[admin_user_name] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[admin_password] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[admin_role] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[email] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[facebook_user_id] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[google_user_id] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
 CREATE TABLE [dbo].[administrators_detail](
	[administrator_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[author_name] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[author_description] [nvarchar](max) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[ads](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[language_id] [int] NOT NULL,
	[name] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[ad_slot] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[ad_code] [nvarchar](max) COLLATE Latin1_General_CI_AI NOT NULL,
	[inactive] [tinyint] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent_category_id] [int] NOT NULL,
	[meta_robots] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[categories_detail](
	[category_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[title] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[main_content] [nvarchar](max) COLLATE Latin1_General_CI_AI NOT NULL,
	[meta_description] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[meta_keywords] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[page_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[date_added] [datetime] NOT NULL,
	[page_views] [int] NOT NULL,
	[inactive] [tinyint] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[custom_themes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[custom_themes_templates](
	[custom_theme_id] [int] NOT NULL,
	[user_file_name] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[master_file_url] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[user_file_content] [nvarchar](max) COLLATE Latin1_General_CI_AI NOT NULL,
	[comment] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[languages](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[language_code] [nchar](2) COLLATE Latin1_General_CI_AI NOT NULL,
	[country_code] [nchar](2) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[languages_detail](
	[language_id] [int] NOT NULL,
	[translation_language_id] [int] NOT NULL,
	[name] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[media_files](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[media_type] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[src] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[posts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[category_id] [int] NOT NULL,
	[administrator_id] [int] NOT NULL,
	[meta_robots] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[posts_comments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[post_id] [int] NOT NULL,
	[administrator_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[comment_date] [datetime] NOT NULL,
	[comment_text] [nvarchar](max) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[posts_detail](
	[post_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[title] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[main_content] [nvarchar](max) COLLATE Latin1_General_CI_AI NOT NULL,
	[meta_description] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[meta_keywords] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[page_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[date_added] [datetime] NOT NULL,
	[date_updated] [datetime] NOT NULL,
	[rating] [decimal](8,2) NOT NULL,
	[page_views] [int] NOT NULL,
	[inactive] [tinyint] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[posts_files](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[post_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[title] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[src] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[posts_ratings](
	[post_id] [int] NOT NULL,
	[administrator_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[rating_date] [datetime] NOT NULL,
	[rating] [decimal](8,2) NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[static_pages](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[connected_to_page] [tinyint] NOT NULL,
	[meta_robots] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[static_pages_detail](
	[static_page_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[link_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[title] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[main_content] [nvarchar](max) COLLATE Latin1_General_CI_AI NOT NULL,
	[meta_description] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[meta_keywords] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL,
	[page_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[inactive] [tinyint] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[static_texts](
	[id] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[language_id] [int] NOT NULL,
	[value] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[web_domains](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[website_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[domain_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[web_address] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[front_end_language] [int] NOT NULL,
	[back_end_language] [int] NOT NULL,
	[custom_theme_id] [int] NOT NULL,
	[analytics_tracking_id] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[facebook_app_id] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[facebook_app_secret] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[google_app_id] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[google_app_secret] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[weblinks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[rel] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[target] [nvarchar](20) COLLATE Latin1_General_CI_AI NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[weblinks_detail](
	[weblink_id] [int] NOT NULL,
	[language_id] [int] NOT NULL,
	[link_name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[url] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[inactive] [tinyint] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[website_sessions](
	[id] [nvarchar](80) COLLATE Latin1_General_CI_AI NOT NULL,
	[application_name] [nvarchar](255) COLLATE Latin1_General_CI_AI NOT NULL,
	[created_date] [datetime] NOT NULL,
	[expires_date] [datetime] NOT NULL,
	[lock_date] [datetime] NOT NULL,
	[lock_id] [int] NOT NULL,
	[timeout_limit] [int] NOT NULL,
	[locked] [tinyint] NOT NULL,
	[session_items] [nvarchar](max) COLLATE Latin1_General_CI_AI NOT NULL,
	[flags] [int] NOT NULL);

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
CREATE TABLE [dbo].[website_settings](
	[id] [nvarchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
	[value] [nvarchar](200) COLLATE Latin1_General_CI_AI NOT NULL);

/* CREATE PRIMARY KEYS */
ALTER TABLE [dbo].[administrators] ADD CONSTRAINT [PK_administrators] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[administrators_detail] ADD CONSTRAINT [PK_administrators_detail] PRIMARY KEY NONCLUSTERED ([administrator_id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[ads] ADD CONSTRAINT [PK_ads] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[categories] ADD CONSTRAINT [PK_categories] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[categories_detail] ADD CONSTRAINT [PK_categories_detail] PRIMARY KEY NONCLUSTERED ([category_id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[custom_themes] ADD CONSTRAINT [PK_custom_themes] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[custom_themes_templates] ADD CONSTRAINT [PK_custom_themes_templates] PRIMARY KEY NONCLUSTERED ([custom_theme_id] ASC, [user_file_name] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[languages] ADD CONSTRAINT [PK_languages] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[languages_detail] ADD CONSTRAINT [PK_languages_detail] PRIMARY KEY NONCLUSTERED ([language_id] ASC, [translation_language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[media_files] ADD CONSTRAINT [PK_media_files] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[posts] ADD CONSTRAINT [PK_posts] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[posts_comments] ADD CONSTRAINT [PK_posts_comments] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[posts_detail] ADD CONSTRAINT [PK_posts_detail] PRIMARY KEY NONCLUSTERED ([post_id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[posts_files] ADD CONSTRAINT [PK_posts_files] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[posts_ratings] ADD CONSTRAINT [PK_posts_ratings] PRIMARY KEY NONCLUSTERED ([post_id] ASC, [administrator_id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[static_pages] ADD CONSTRAINT [PK_static_pages] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[static_pages_detail] ADD CONSTRAINT [PK_static_pages_detail] PRIMARY KEY NONCLUSTERED ([static_page_id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[static_texts] ADD CONSTRAINT [PK_static_texts] PRIMARY KEY NONCLUSTERED ([id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[web_domains] ADD CONSTRAINT [PK_web_domains] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[weblinks] ADD CONSTRAINT [PK_weblinks] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[weblinks_detail] ADD CONSTRAINT [PK_weblinks_detail] PRIMARY KEY NONCLUSTERED ([weblink_id] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[website_sessions] ADD CONSTRAINT [PK_website_sessions] PRIMARY KEY NONCLUSTERED ([id] ASC, [application_name] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[website_settings] ADD CONSTRAINT [PK_website_settings] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);

/* CREATE CLUSTERED INDEXES */
CREATE CLUSTERED INDEX [CDX_administrators] ON [dbo].[administrators] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_administrators_detail] ON [dbo].[administrators_detail] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_ads] ON [dbo].[ads] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_categories] ON [dbo].[categories] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_categories_detail] ON [dbo].[categories_detail] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_custom_themes] ON [dbo].[custom_themes] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_custom_themes_templates] ON [dbo].[custom_themes_templates] ([custom_theme_id] ASC);
CREATE CLUSTERED INDEX [CDX_languages] ON [dbo].[languages] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_languages_detail] ON [dbo].[languages_detail] ([translation_language_id] ASC);
CREATE CLUSTERED INDEX [CDX_media_files] ON [dbo].[media_files] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_posts] ON [dbo].[posts] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_posts_comments] ON [dbo].[posts_comments] ([post_id] ASC);
CREATE CLUSTERED INDEX [CDX_posts_detail] ON [dbo].[posts_detail] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_posts_files] ON [dbo].[posts_files] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_posts_ratings] ON [dbo].[posts_ratings] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_static_pages] ON [dbo].[static_pages] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_static_pages_detail] ON [dbo].[static_pages_detail] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_static_texts] ON [dbo].[static_texts] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_web_domains] ON [dbo].[web_domains] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_weblinks] ON [dbo].[weblinks] ([id] ASC);
CREATE CLUSTERED INDEX [CDX_weblinks_detail] ON [dbo].[weblinks_detail] ([language_id] ASC);
CREATE CLUSTERED INDEX [CDX_website_sessions] ON [dbo].[website_sessions] ([application_name] ASC);
CREATE CLUSTERED INDEX [CDX_website_settings] ON [dbo].[website_settings] ([id] ASC);

/* CREATE UNIQUE KEYS */
ALTER TABLE [dbo].[administrators] ADD CONSTRAINT [UK_administrators_user_name] UNIQUE NONCLUSTERED ([admin_user_name] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[categories_detail] ADD CONSTRAINT [UK_categories_detail_page_name] UNIQUE NONCLUSTERED ([page_name] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[posts_detail] ADD CONSTRAINT [UK_posts_detail_page_name] UNIQUE NONCLUSTERED ([page_name] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[static_pages_detail] ADD CONSTRAINT [UK_static_pages_detail_page_name] UNIQUE NONCLUSTERED ([page_name] ASC, [language_id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);
ALTER TABLE [dbo].[web_domains] ADD CONSTRAINT [UK_web_domains_domain_name] UNIQUE NONCLUSTERED ([domain_name] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF);

/* CREATE DEFAULT VALUES */
ALTER TABLE [dbo].[administrators] ADD CONSTRAINT [DF_administrators_password]  DEFAULT (N'1000:BS3ZEKeB3ZOuI1LL:dSYvveOPOyJgAtKb') FOR [admin_password];

/* CREATE FOREIGN KEYS */
ALTER TABLE [dbo].[administrators_detail] WITH CHECK ADD CONSTRAINT [FK_administrators_detail_administrators] FOREIGN KEY([administrator_id]) REFERENCES [dbo].[administrators] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[administrators_detail] CHECK CONSTRAINT [FK_administrators_detail_administrators];
ALTER TABLE [dbo].[administrators_detail] WITH CHECK ADD CONSTRAINT [FK_administrators_detail_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[administrators_detail] CHECK CONSTRAINT [FK_administrators_detail_languages];
ALTER TABLE [dbo].[ads] WITH CHECK ADD CONSTRAINT [FK_ads_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[ads] CHECK CONSTRAINT [FK_ads_languages];
ALTER TABLE [dbo].[categories_detail] WITH CHECK ADD CONSTRAINT [FK_categories_detail_categories] FOREIGN KEY([category_id]) REFERENCES [dbo].[categories] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[categories_detail] CHECK CONSTRAINT [FK_categories_detail_categories];
ALTER TABLE [dbo].[categories_detail] WITH CHECK ADD CONSTRAINT [FK_categories_detail_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[categories_detail] CHECK CONSTRAINT [FK_categories_detail_languages];
ALTER TABLE [dbo].[custom_themes_templates] WITH CHECK ADD CONSTRAINT [FK_custom_themes_templates_custom_themes] FOREIGN KEY([custom_theme_id]) REFERENCES [dbo].[custom_themes] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[custom_themes_templates] CHECK CONSTRAINT [FK_custom_themes_templates_custom_themes];
ALTER TABLE [dbo].[languages_detail] WITH CHECK ADD CONSTRAINT [FK_languages_detail_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[languages_detail] CHECK CONSTRAINT [FK_languages_detail_languages];
ALTER TABLE [dbo].[languages_detail] WITH CHECK ADD CONSTRAINT [FK_languages_detail_translation_languages] FOREIGN KEY([translation_language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[languages_detail] CHECK CONSTRAINT [FK_languages_detail_translation_languages];
ALTER TABLE [dbo].[posts] WITH CHECK ADD CONSTRAINT [FK_posts_administrators] FOREIGN KEY([administrator_id]) REFERENCES [dbo].[administrators] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[posts] CHECK CONSTRAINT [FK_posts_administrators];
ALTER TABLE [dbo].[posts] WITH CHECK ADD CONSTRAINT [FK_posts_categories] FOREIGN KEY([category_id]) REFERENCES [dbo].[categories] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[posts] CHECK CONSTRAINT [FK_posts_categories];
ALTER TABLE [dbo].[posts_comments] WITH CHECK ADD CONSTRAINT [FK_posts_comments_administrators] FOREIGN KEY([administrator_id]) REFERENCES [dbo].[administrators] ([id]);
ALTER TABLE [dbo].[posts_comments] CHECK CONSTRAINT [FK_posts_comments_administrators];
ALTER TABLE [dbo].[posts_comments] WITH CHECK ADD CONSTRAINT [FK_posts_comments_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[posts_comments] CHECK CONSTRAINT [FK_posts_comments_languages];
ALTER TABLE [dbo].[posts_comments] WITH CHECK ADD CONSTRAINT [FK_posts_comments_posts] FOREIGN KEY([post_id]) REFERENCES [dbo].[posts] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[posts_comments] CHECK CONSTRAINT [FK_posts_comments_posts];
ALTER TABLE [dbo].[posts_detail] WITH CHECK ADD CONSTRAINT [FK_posts_detail_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[posts_detail] CHECK CONSTRAINT [FK_posts_detail_languages];
ALTER TABLE [dbo].[posts_detail] WITH CHECK ADD CONSTRAINT [FK_posts_detail_posts] FOREIGN KEY([post_id]) REFERENCES [dbo].[posts] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[posts_detail] CHECK CONSTRAINT [FK_posts_detail_posts];
ALTER TABLE [dbo].[posts_files] WITH CHECK ADD CONSTRAINT [FK_posts_files_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[posts_files] CHECK CONSTRAINT [FK_posts_files_languages];
ALTER TABLE [dbo].[posts_files] WITH CHECK ADD CONSTRAINT [FK_posts_files_posts] FOREIGN KEY([post_id]) REFERENCES [dbo].[posts] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[posts_files] CHECK CONSTRAINT [FK_posts_files_posts];
ALTER TABLE [dbo].[posts_ratings] WITH CHECK ADD CONSTRAINT [FK_posts_ratings_administrators] FOREIGN KEY([administrator_id]) REFERENCES [dbo].[administrators] ([id]);
ALTER TABLE [dbo].[posts_ratings] CHECK CONSTRAINT [FK_posts_ratings_administrators];
ALTER TABLE [dbo].[posts_ratings] WITH CHECK ADD CONSTRAINT [FK_posts_ratings_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[posts_ratings] CHECK CONSTRAINT [FK_posts_ratings_languages];
ALTER TABLE [dbo].[posts_ratings] WITH CHECK ADD CONSTRAINT [FK_posts_ratings_posts] FOREIGN KEY([post_id]) REFERENCES [dbo].[posts] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[posts_ratings] CHECK CONSTRAINT [FK_posts_ratings_posts];
ALTER TABLE [dbo].[static_pages_detail] WITH CHECK ADD CONSTRAINT [FK_static_pages_detail_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[static_pages_detail] CHECK CONSTRAINT [FK_static_pages_detail_languages];
ALTER TABLE [dbo].[static_pages_detail] WITH CHECK ADD CONSTRAINT [FK_static_pages_detail_pages] FOREIGN KEY([static_page_id]) REFERENCES [dbo].[static_pages] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[static_pages_detail] CHECK CONSTRAINT [FK_static_pages_detail_pages];
ALTER TABLE [dbo].[static_texts] WITH CHECK ADD CONSTRAINT [FK_static_texts_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[static_texts] CHECK CONSTRAINT [FK_static_texts_languages];
ALTER TABLE [dbo].[web_domains] WITH CHECK ADD CONSTRAINT [FK_web_domains_back_end_languages] FOREIGN KEY([back_end_language]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[web_domains] CHECK CONSTRAINT [FK_web_domains_back_end_languages];
ALTER TABLE [dbo].[web_domains] WITH CHECK ADD CONSTRAINT [FK_web_domains_front_end_languages] FOREIGN KEY([front_end_language]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[web_domains] CHECK CONSTRAINT [FK_web_domains_front_end_languages];
ALTER TABLE [dbo].[weblinks_detail] WITH CHECK ADD CONSTRAINT [FK_weblinks_detail_languages] FOREIGN KEY([language_id]) REFERENCES [dbo].[languages] ([id]);
ALTER TABLE [dbo].[weblinks_detail] CHECK CONSTRAINT [FK_weblinks_detail_languages];
ALTER TABLE [dbo].[weblinks_detail] WITH CHECK ADD CONSTRAINT [FK_weblinks_detail_weblinks] FOREIGN KEY([weblink_id]) REFERENCES [dbo].[weblinks] ([id]) ON DELETE CASCADE;
ALTER TABLE [dbo].[weblinks_detail] CHECK CONSTRAINT [FK_weblinks_detail_weblinks];

/* EXCECUTE THE TRANSACTION */
COMMIT;