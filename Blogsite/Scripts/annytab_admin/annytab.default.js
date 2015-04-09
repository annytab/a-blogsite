// Initialize when the form is loaded
$(document).ready(start);

// Start this instance
function start() 
{
    // Register events
    $(document).on("keydown", "input:text, input:file, input[type=number], input:password, input:radio, button, input:checkbox, select", enterAsTab);
    $(document).on("change", "input[data-id='previewDomainImage']", previewDomainImage);
    $("#uploadMainImage").change(previewMainImage);
    $(document).on("click", "#messageBoxOkButton, #messageBoxCancelButton, #messageBoxClose", unlockForm);
    $(document).on("click", ".annytab-list-row-main, .annytab-list-row-alt", toggleRowBgColor);
    $("input[data-id='deletePost']").each(function()
    {
        var deleteButton = $(this);
        deleteButton.click({ url: deleteButton.attr("data-url") }, deletePostConfirmation);
    });
    $("input[data-id='btnPreviewMediaFile']").click(previewMediaFile);
    $("input[data-id='btnAddMediaFile']").click(addMediaFile);
    $("#btnPreviewPost").click(previewPost);
    
    // Images in container
    $("#uploadImages").change(previewImages);
    $(document).on("click", "img[data-id='deleteImage']", deleteImage);
    $("#clearFileUpload").click(clearFileUpload);

    // Get the message box container
    var messageBoxContainer = $("#messageBoxContainer");

    // Check for an error
    var errorCode = $("#errorCode").attr("data-error");
    if (errorCode != "0")
    {
        // Show the message box
        messageBoxContainer.find("span").css("display", "none");
        messageBoxContainer.find("[data-number='" + errorCode + "']").css("display", "inline");
        messageBoxContainer.fadeIn(500);
    }

} // End of the start method

// Unlock the form
function unlockForm()
{
    // Get the url
    var url = $(this).attr("data-url");

    if (url != null) 
    {
        // Fade out the message box
        $("#messageBoxContainer").fadeOut(500);

        // Redirect the user
        window.setTimeout(function () {
            window.location.href = url;
        }, 500);
        
        return false;
    }
    else
    {
        // Fade out the message box
        $("#messageBoxContainer").fadeOut(500);
    }

} // End of the unlockForm method

// Make enter work as tab
function enterAsTab(event)
{
    
    // Check if the enter key is pressed
    if (event.keyCode == 13)
    {
        // Get the current control
        var control = $(this);

        // Get all the controls that can gain focus :::: filter(":not([readonly])").
        var controls = $(document).find("button, input, textarea, select").filter("[tabindex!='-1']").filter(":visible");

        // Get the index of the current control
        var index = controls.index(control);

        // Set focus to the next control
        if (index > -1 && (index + 1) < controls.length)
        {
            if (controls.eq(index + 1) != null)
                controls.eq(index + 1).focus();
            else
                return true;
        }

        return false;
    }

} // End of the enterAsTab method

// Preview domain images
function previewDomainImage(event)
{
    // Get the file upload control
    var control = $(this);
    
    // Get the file collection
    var fileCollection = event.target.files;

    // Get the file extension
    var fileExtension = control.val().substring(control.val().lastIndexOf('.') + 1);

    if(fileExtension != "jpg" && fileExtension != "jpeg")
    {
        // Replace the control
        control.replaceWith(control = control.clone(true));

        // Show the message box
        var messageBoxContainer = $("#messageBoxContainer");
        messageBoxContainer.find("span").css("display", "none");
        messageBoxContainer.find("[data-number='3']").css("display", "inline");
        messageBoxContainer.fadeIn(500);
    }
    else if (fileCollection[0].size >= 1048576)
    {
        // Replace the file upload control
        control.replaceWith(control = control.clone(true));

        // Show the message box
        var messageBoxContainer = $("#messageBoxContainer");
        messageBoxContainer.find("span").css("display", "none");
        messageBoxContainer.find("[data-number='4']").css("display", "inline");
        messageBoxContainer.fadeIn(500);
    }
    else
    {
        // Get image id
        var imageId = control.attr("data-img");

        // Create a file reader
        var reader = new FileReader();

        // Load the image
        reader.onload = function (e)
        {
            $("#" + imageId).attr("src", e.target.result);
        }

        // Read the image file
        reader.readAsDataURL(fileCollection[0]);
    }

} // End of the previewDomainImage method

// Preview the image to upload
function previewMainImage(event)
{
    // Get the file upload control
    var control = $(this);

    // Get the file collection
    var fileCollection = event.target.files;

    // Get the file extension
    var fileExtension = control.val().substring(control.val().lastIndexOf('.') + 1);

    // Make sure that there is files
    if (fileCollection[0].size >= 1048576)
    {
        // Replace the file upload control
        control.replaceWith(control = control.clone(true));

        // Show the message box
        var messageBoxContainer = $("#messageBoxContainer");
        messageBoxContainer.find("span").css("display", "none");
        messageBoxContainer.find("[data-number='4']").css("display", "inline");
        messageBoxContainer.fadeIn(500);

    }
    else if (fileExtension == "jpg" || fileExtension == "jpeg")
    {
        // Create a file reader
        var reader = new FileReader();

        // Load the image
        reader.onload = function (e)
        {
            $("#mainImage").attr("src", e.target.result);
        }

        // Read the image file
        reader.readAsDataURL(fileCollection[0]);
    }
    else
    {
        // Replace the control
        control.replaceWith(control = control.clone(true));

        // Show the message box
        var messageBoxContainer = $("#messageBoxContainer");
        messageBoxContainer.find("span").css("display", "none");
        messageBoxContainer.find("[data-number='3']").css("display", "inline");
        messageBoxContainer.fadeIn(500);
    }

} // End of the previewMainImage method

// Preview images that have been added
function previewImages(event)
{
    // Get the file upload control
    var control = $(this);

    // Get the file collection
    var fileCollection = event.target.files;

    // Get the images container
    var imagesContainer = $("#imagesContainer");

    // Delete images that has been created temporary
    imagesContainer.find("[data-temp='true']").remove();

    // Get the size of all the images
    var fileSize = 0;
    for (var i = 0; i < fileCollection.length; i++)
    {
        fileSize += fileCollection[i].size;
    }

    if (fileSize > 4194304) // 4 mb
    {
        // Replace the file upload control
        control.replaceWith(control = control.clone(true));

        // Show the message box
        var messageBoxContainer = $("#messageBoxContainer");
        messageBoxContainer.find("span").css("display", "none");
        messageBoxContainer.find("[data-number='4']").css("display", "inline");
        messageBoxContainer.fadeIn(500);
    }
    else
    {
        for (var i = 0; i < fileCollection.length; i++)
        {
            // Create a file reader
            var reader = new FileReader();

            // Load the image
            reader.onload = function (e)
            {
                var control = "<div data-id='imageContainer'" + " data-temp='true' " + "class='annytab-image-square' >";
                control += "<img src='" + e.target.result + "' style='max-width:128px;' />";
                control += "</div>";

                imagesContainer.prepend(control);
            }

            // Read the image file
            reader.readAsDataURL(fileCollection[i]);
        }
    }

} // End of the previewImages method

// Delete the image
function deleteImage()
{
    // Get the div to delete
    var imageContainer = $(this).closest("div[data-id='imageContainer']");

    // Delete the image container
    imageContainer.remove();

} // End of the deleteImage method

// Clear the file upload field
function clearFileUpload()
{
    // Get the file upload control
    var control = $("#uploadImages");

    // Get the other image container
    var imagesContainer = $("#imagesContainer");

    // Delete images that has been created temporary
    imagesContainer.find("[data-temp='true']").remove();

    // Replace the file upload control
    control.replaceWith(control = control.clone(true));

    // Return false to supress submit
    return false;

} // End of the clearFileUpload method

// Give the user a chance to confirm the deletion
function deletePostConfirmation(event)
{
    // Show the message box
    var messageBoxContainer = $("#messageBoxContainer");
    messageBoxContainer.find("span").css("display", "none");
    messageBoxContainer.find("#messageBoxOkButton").attr("data-url", event.data.url);
    messageBoxContainer.find("[data-number='2']").css("display", "inline");
    messageBoxContainer.fadeIn(500);

} // End of the deletePostConfirmation method

// Toggle the background color of the row
function toggleRowBgColor()
{
    // Toggle the background color
    $(this).toggleClass("highlight");
    
} // End of the toggleRowBgColor method

// Get a html string with media file content
function getMediaFileContent(title, mediaType, src)
{
    // Create the string to return
    var html = '';

    // Set the media content
    if (mediaType == "image") {
        html += '<img src="' + src + '" alt="' + title + '" style="max-width:100%;" />';
    }
    else if (mediaType == "url") {
        html += '<a href="' + src + '">' + title + '</a>';
    }
    else if (mediaType == "object") {
        html += '<object width="640" data="' + src + '"></object>';
    }
    else if (mediaType == "iframe") {
        html += '<iframe width="640" src="' + src + '"></iframe>';
    }
    else if (mediaType == "video_mp4") {
        html += '<video width="640" controls>';
        html += '<source src="' + src + '" type="video/mp4">';
        html += 'Your browser does not support the video tag.';
        html += '</video>';
    }
    else if (mediaType == "video_webm") {
        html += '<video width="640" controls>';
        html += '<source src="' + src + '" type="video/webm">';
        html += 'Your browser does not support the video tag.';
        html += '</video>';
    }
    else if (mediaType == "video_ogg") {
        html += '<video width="640" controls>';
        html += '<source src="' + src + '" type="video/ogg">';
        html += 'Your browser does not support the video tag.';
        html += '</video>';
    }
    else if (mediaType == "audio_mpeg") {
        html += '<audio width="320" controls>';
        html += '<source src="' + src + '" type="audio/mpeg">';
        html += 'Your browser does not support the audio element.';
        html += '</audio>';
    }
    else if (mediaType == "audio_ogg") {
        html += '<audio width="320" controls>';
        html += '<source src="' + src + '" type="audio/ogg">';
        html += 'Your browser does not support the audio element.';
        html += '</audio>';
    }
    else if (mediaType == "audio_wav") {
        html += '<audio width="320" controls>';
        html += '<source src="' + src + '" type="audio/wav">';
        html += 'Your browser does not support the audio element.';
        html += '</audio>';
    }

    // Return the string
    return html;

} // End of the getMediaFileContent method

// Preview the media file
function previewMediaFile()
{
    // Get data
    var control = $(this);
    var title = $(this).attr("data-title");
    var media_type = $(this).attr("data-media-type");
    var src = $(this).attr("data-src");

    // Create the string to print
    var html = '<!DOCTYPE html><html><head><title>' + title + '</title></head><body>';

    // Set the media content
    html += getMediaFileContent(title, media_type, src);

    // Add the final touch to the html
    html += '</body></html>';

    // Calculate offsets
    var left = (screen.width / 2) - (1280 / 2);
    var top = (screen.height / 2) - (720 / 2);

    // Create the preview window
    var printWindow = window.open("", "", "resizable=yes,scrollbars=yes,height=720,width=1280,top=" + top + ",left=" + left, true);
    printWindow.document.write(html);
    printWindow.document.close();

} // End of the previewMediaFile method

// Add the media file
function addMediaFile()
{
    // Get data
    var control = $(this);
    var title = control.attr("data-title");
    var media_type = control.attr("data-media-type");
    var src = control.attr("data-src");
    var text_area = $("#txtDescription");
    var selection = text_area.getSelection();

    // Create the string to append
    var html = getMediaFileContent(title, media_type, src);

    // Add html to the text area
    text_area.insertText(html, selection.end, 'collapseToEnd');

} // End of the addMediaFile method

// Preview a post
function previewPost()
{
    // Get data
    var title = $("#txtTitle").val();
    var content = $("#txtDescription").val();
    var theme = $(this).attr("data-theme");

    // Create the string to print
    var html = '<!DOCTYPE html><html><head><title>' + title + '</title>';
    html += theme == "0" ? '<link type="text/css" rel="stylesheet" href="/Content/annytab_css/front_default_style.css" />' : '<link type="text/css" rel="stylesheet" href="/Content/theme/front_default_style.css" />';
    html += '<script src="https://google-code-prettify.googlecode.com/svn/loader/run_prettify.js"><\/script>';
    html += '</head><body>';

    // Set the content for the body
    html += content;

    // Add the final touch to the html
    html += '</body></html>';

    // Calculate offsets
    var left = (screen.width / 2) - (1280 / 2);
    var top = (screen.height / 2) - (720 / 2);

    // Create the preview window
    var printWindow = window.open("", "", "resizable=yes,scrollbars=yes,height=720,width=1280,top=" + top + ",left=" + left, true);
    printWindow.document.write(html);
    printWindow.document.close();

} // End of the previewPost method