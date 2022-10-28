// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function addAnimal() {
    window.location.href = "/Animals/AddAnimal";
}

$('#modal-confirm').click(e => {
    console.log(e.target.value);
    window.location.href = "/Animals/DeleteAnimal/" + e.target.value;
})

$("#exampleModal").bind('show.bs.modal', event => {

    const button = event.relatedTarget

    const recipient = button.getAttribute('data-bs-id')

    $("#modal-confirm").val(recipient)
})
