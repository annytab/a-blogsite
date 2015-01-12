BEGIN TRANSACTION;

/* STATIC TEXTS */
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('translate_user_details',1,'Du måste uppdatera (översätta) ditt författarnamn och din beskrivning för att kunna betygsätta och kommentera den här posten!');
INSERT INTO dbo.static_texts (id, language_id, value) VALUES ('translate_user_details',2,'You need to update (translate) your author name and your description to be able to rate and comment this post!');

/* WEBSITE SETTINGS */
INSERT INTO dbo.website_settings (id, value) VALUES ('SEND-EMAIL-USE-SSL','false');
INSERT INTO dbo.website_settings (id, value) VALUES ('REDIRECT-HTTPS','false');

/* EXCECUTE THE TRANSACTION */
COMMIT;