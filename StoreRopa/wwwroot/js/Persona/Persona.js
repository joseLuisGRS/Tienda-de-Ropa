$(function () {
    $("#datapicker").datepicker();
});
$(".close").click(function (eve) {
    //jQuery.noConflict();
    $('#myModal').modal('toggle');
});

$("#btnSave").click(function (eve) {
    event.preventDefault();
    var form = $(this).parents('.modal').find('form');
    var actionUrl = form.attr('action');
    var dataToSend = form.serialize();
    $.post(actionUrl, dataToSend).done(function (data) { 
        $('#myModal').find('.modal-body').replaceWith("<div class = 'modal-body'>" + data + "</div>");  
        var minFecha = new Date();
        minFecha.setFullYear(minFecha.getFullYear() - 18);
        var fecha = minFecha.toLocaleDateString('en-CA');
        $("#fechaNacimiento").attr("max", fecha);       
        if ($("#fechaNacimiento").val() == "") {
            $("#fechaNacimiento").val(fecha);
        }
    }).always(function () {
        if ($('#txtExito').val() == "1") {
            $('#myModal').modal('hide');
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Registro guardado exitosamente',
                showConfirmButton: false,
                timer: 3000
            });
            setTimeout(function () {               
                location.reload();
            }, 3000);
        } else {           
            var message = $('#txtErrorMessage').val();
            if (message != '' && message != undefined) {
                if (message != "error") {
                    $("#error").show();
                    var err = $("#error");
                    err.html(message);
                    $("#error").fadeOut(10000, function () {
                        $("#error").remove();
                    });
                } else {
                    $("#error").hide();
                }              
                Swal.fire({
                    position: 'top-end',
                    icon: 'error',
                    title: 'Error al guardar registro.',
                    showConfirmButton: false,
                    timer: 3000
                });
            } else {                
                $("#error").hide();
            }          
        }
    });
});
