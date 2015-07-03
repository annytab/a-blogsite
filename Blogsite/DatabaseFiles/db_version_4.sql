BEGIN TRANSACTION;

/* STATIC TEXTS */
DELETE FROM dbo.static_texts WHERE id = 'upload_main_image';

/* UPLOAD MAIN IMAGE */
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'upload_main_image' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('upload_main_image',1,'Bild, 256 kb, jpg|jpeg (1:1)')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'upload_main_image' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('upload_main_image',2,'Image, 256 kb, jpg|jpeg (1:1)')
END;

/* IMAGE SIZE LIMIT */
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'error_image_size' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('error_image_size',1,'Den uppladdade bilden får inte vara större än {0}!')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'error_image_size' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('error_image_size',2,'The size of the uploaded image can not be greater than {0}!')
END;

/* MARKDOWN SUPPORT */
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'markdown_supported' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('markdown_supported',1,'Stöd för markdown')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'markdown_supported' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('markdown_supported',2,'Markdown supported')
END;

/* EXCECUTE THE TRANSACTION */
COMMIT;