BEGIN TRANSACTION;

/* STATIC TEXTS */
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'translate_user_details' AND language_id = 1)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('translate_user_details',1,'Du måste uppdatera (översätta) ditt författarnamn och din beskrivning för att kunna betygsätta och kommentera den här posten!')
END;
IF NOT EXISTS (SELECT * FROM dbo.static_texts WHERE id = 'translate_user_details' AND language_id = 2)
BEGIN
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('translate_user_details',2,'You need to update (translate) your author name and your description to be able to rate and comment this post!')
END;

/* WEBSITE SETTINGS */
IF NOT EXISTS (SELECT * FROM dbo.website_settings WHERE id = 'SEND-EMAIL-USE-SSL')
BEGIN
INSERT INTO dbo.website_settings (id, value) VALUES ('SEND-EMAIL-USE-SSL','false')
END;
IF NOT EXISTS (SELECT * FROM dbo.website_settings WHERE id = 'REDIRECT-HTTPS')
BEGIN
INSERT INTO dbo.website_settings (id, value) VALUES ('REDIRECT-HTTPS','false')
END;

/* EXCECUTE THE TRANSACTION */
COMMIT;