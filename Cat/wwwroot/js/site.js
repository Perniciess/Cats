document.getElementById("getImageButton").addEventListener("click", async function() {
    const url = document.getElementById("urlInput").value;
    const urlPattern = new RegExp('^(http|https)://[^ "]+$');
    if (!urlPattern.test(url)) {
        document.getElementById("errorText").style.display = "block";
        document.getElementById("catImage").style.display = "none";
        return;
    }
    document.getElementById("errorText").style.display = "none";
    const response = await fetch(`/home/getcatstatusimage?url=${url}`);
    const imageData = await response.blob();
    const imageUrl = URL.createObjectURL(imageData);
    document.getElementById("catImage").style.display = "none";
    document.getElementById("catImage").src = imageUrl;
    document.getElementById("catImage").style.display = "block";
});