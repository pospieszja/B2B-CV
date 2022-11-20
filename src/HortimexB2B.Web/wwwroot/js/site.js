// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.


// Disallow to click disabled hyperlinks
$('body').on('click', '.disabled', function (e) {
    e.preventDefault();
    return false;
});