document.getElementById('fileInput').addEventListener('change', function (event) {
    var files = event.target.files;
    var previewContainer = document.getElementById('imagePreviewContainer');
    previewContainer.innerHTML = ''; // Clear existing previews
    
    
    if (files.length > 0) {
        
        for (let i = 0; i < files.length; i++) {
            let file = files[i];
            let reader = new FileReader();

            reader.onload = function (e) {
                // Create a new div to hold the image and radio button
                var div = document.createElement('div');
                div.classList.add('form-check');

                // Create the image element
                var img = document.createElement('img');
                img.src = e.target.result;
                img.classList.add('img-thumbnail');
                img.style.width = '100px';

                // Create the radio button for selecting the main image
                var radio = document.createElement('input');
                radio.type = 'radio';
                radio.name = 'mainImageName';
                radio.value = file.name; // Use index as the value for now
                radio.required = true; // Make the radio button required

                // Create a label for the radio button
                var label = document.createElement('label');
                label.innerText = 'Set as Main';

                // Append the image and radio button to the div
                div.appendChild(img);
                div.appendChild(radio);
                div.appendChild(label);

                // Append the div to the preview container
                previewContainer.appendChild(div);
            }

            // Read the file as a data URL
            reader.readAsDataURL(file);
        }
    }
});


