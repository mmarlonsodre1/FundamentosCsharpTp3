// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function handleSelected(event) {
    const { name, checked } = event.target
    const viewName = document.URL.includes("IndexBirthday") ? "IndexBirthday" : "IndexEmail"

    const req = new XMLHttpRequest()
    const baseURI = event.target.baseURI.split("?")[0]
    let url = document.URL.includes("IndexBirthday") ? baseURI.replace("IndexBirthday", "HandleSelection") : baseURI.replace("IndexEmail", "HandleSelection")
    req.open("POST", url)
    req.setRequestHeader("Content-Type", "application/x-www-form-urlencoded")
    req.send(`Id=${name}&NewValue=${checked}&ViewName=${viewName}`)
}

document.querySelectorAll("#Selected").forEach(element => element.addEventListener("change", handleSelected))