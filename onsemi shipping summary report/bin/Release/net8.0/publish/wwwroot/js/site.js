// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#getDetails').on('click', function () {
        var lotAlias = $('#lotAlias').val();

        $.ajax({
            url: '/Home/GetLotDetails',
            type: 'GET',
            data: { lotAlias: lotAlias },
            success: function (response) {
                // Show the returned data (handle accordingly based on your design)
                $('#lotDetailsContainer').html(response);
            },
            error: function () {
                alert('Error retrieving lot details');
            }
        });
    });
});
