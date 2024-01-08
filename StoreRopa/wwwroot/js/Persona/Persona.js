$(function () {
    $("#datapicker").datepicker();
});

$(".close").click(function (eve) {
    $('#myModal').modal('hide');
    $('#myModalDet').modal('hide');
    $('#myModalEdit').modal('hide');
});

$("#btnSave").click(function (eve) {
    event.preventDefault();
    var form = $(this).parents('.modal').find('form');
    var actionUrl = form.attr('action');
    var dataToSend = form.serialize();
    Swal.fire({
        title: "Confirmar",
        text: "¿Deseas guardar el registro?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si",
        cancelButtonText: "No"
    }).then((result) => {
        if (result.isConfirmed) {
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
                        title: 'Registro guardado exitosamente.!',
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
                            title: 'Error al guardar registro.!',
                            showConfirmButton: false,
                            timer: 3000
                        });
                    } else {
                        $("#error").hide();
                    }
                }
            });
        }
    });
});

function showInPopup (url){
    $.get(url).done(function (data) {
        $('#modal-contentD').html(data);
        $('#myModalDet').modal('toggle');
        $('#myModalDet > .modal').modal('show');
        $("#error").hide();
    });
}

function editInPopup(url) {
    $.get(url).done(function (data) {
        var minFecha = new Date();
        minFecha.setFullYear(minFecha.getFullYear() - 18);
        $('#modal-contentE').html(data);
        $('#myModalEdit').modal('toggle');
        $('#myModalEdit > .modal').modal('show');
        var fecha = minFecha.toLocaleDateString('en-CA');
        $("#fechaNacimiento").attr("max", fecha);;
        $("#error").hide();
    });
}

$("#btnEdit").click(function (eve) {
    event.preventDefault();
    var form = $(this).parents('.modal').find('form');
    var actionUrl = form.attr('action');
    var dataToSend = form.serialize();
    Swal.fire({
        title: "Confirmar",
        text: "¿Deseas actualizar el registro?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si",
        cancelButtonText: "No"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: actionUrl,
                type: "PUT",
                data: dataToSend,
                error: function (er) {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'error',
                        title: 'Error al actualizar registro.!',
                        showConfirmButton: false,
                        timer: 3000
                    });
                },
                success: function (data) {
                    $('#myModalEdit').find('.modal-body').replaceWith("<div class = 'modal-body'>" + data + "</div>");
                    var minFecha = new Date();
                    minFecha.setFullYear(minFecha.getFullYear() - 18);
                    var fecha = minFecha.toLocaleDateString('en-CA');
                    $("#fechaNacimiento").attr("max", fecha);
                    if ($("#fechaNacimiento").val() == "") {
                        $("#fechaNacimiento").val(fecha);
                    } 
                    if ($('#txtExito').val() == "1") {
                        $('#myModalEdit').modal('hide');
                        Swal.fire({
                            position: 'top-end',
                            icon: 'success',
                            title: 'Registro actualizado exitosamente.!',
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
                                title: 'Error al actualizar registro.!',
                                showConfirmButton: false,
                                timer: 3000
                            });
                        } else {
                            $("#error").hide();
                        }
                    }
                }
            });

        }
    });
});

$(document).ready(function () {
    $("#txtBuscar").on("keyup", function () {
        $("#dataTab tr td.noFound").remove();
        var value = $(this).val().toLowerCase();
        var allItems = $("#dataTab tr.bus");
        var matchedItems = $("#dataTab tr.bus").filter(function () {
            return $(this).text().toLowerCase().indexOf(value) > -1
        });
        allItems.toggle(false);
        matchedItems.toggle(true);
        if (matchedItems.length == 0) {
            var htmlTags = '<tr><td colspan="7" class="noFound" style=" text-align: center;">' +
            'No se encontro información.</td ></tr > ';
            $('#dataTab tbody').append(htmlTags);
        }
    });
});

function estatusYEliminar(uri, opcion) {
    Swal.fire({
        title: "Confirmar",
        text: "¿Deseas " + opcion + " el registro?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si",
        cancelButtonText: "No"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: uri,
                type: "PATCH",
                data: {},
                beforeSend: function (request) {
                    request.setRequestHeader("RequestVerificationToken", $("[name='__RequestVerificationToken']").val());
                },
                error: function () {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'error',
                        title: 'Error al actualizar registro.!',
                        showConfirmButton: false,
                        timer: 3000
                    });
                },
                dataType: 'json',
                success: function (data) {
                    if (data == "1") {
                        Swal.fire({
                            position: 'top-end',
                            icon: 'success',
                            title: 'Registro actualizado exitosamente.!',
                            showConfirmButton: false,
                            timer: 3000
                        });
                        setTimeout(function () {
                            location.reload();
                        }, 3000);
                    } else if (data == "2") {
                        Swal.fire({
                            position: 'top-end',
                            icon: 'error',
                            title: 'El cliente tiene saldo a pagar, No se puede ' + opcion + '.!',
                            showConfirmButton: false,
                            timer: 3000
                        });
                    } else {
                        Swal.fire({
                            position: 'top-end',
                            icon: 'error',
                            title: 'Error al actualizar registro.!',
                            showConfirmButton: false,
                            timer: 3000
                        });
                    }
                }
            });
        }
    });
}

$("#btnCerrarE").click(function (eve) {
    if ($("#txtExito").val() == "0") {
        location.reload();
    }
});