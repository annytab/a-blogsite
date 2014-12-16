// Initialize when the form is loaded
$(document).ready(start);

// Start this instance
function start()
{
    // Register events
    $("#uploadMainImage").change(previewMainImage);

} // End of the start method

// Preview the image to upload
function previewMainImage(event)
{
    // Get the file upload control
    var control = $(this);

    // Get the file collection
    var fileCollection = event.target.files;

    // Get the file extension
    var fileExtension = control.val().substring(control.val().lastIndexOf('.') + 1);

    try
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
    catch(err)
    {
        alert(err);
    }

} // End of the previewMainImage method
