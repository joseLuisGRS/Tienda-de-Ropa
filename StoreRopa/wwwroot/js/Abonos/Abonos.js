$(document).ready(function () {
    $("#error").hide();
    ValidaCheckActivo();
    $("input[name=TipoBusqueda]").on('change', function () {
        ValidaCheckActivo();
    });
    $("#cliente").on('change', function () {
        DatosIniciales();
        if ($(this).val() > 0) {
            $("#lblAbonoClienteId").text($(this).val().padStart(10, '0'));
            $("#lblNombreCliente").text($('select[id="cliente"] option:selected').text());
            $("#divDatosAbono").show();
            getSaldo($(this).val());
            $("#tablaArticulos").show();
        }
    });
});

function ValidaCheckActivo() {
    DatosIniciales();
    switch ($('input[name=TipoBusqueda]:checked', '#frmAbono').val()) {
        case 'id':
            $("#txtClaveCliente").val("");
            $("#divClaveCliente").show();
            $("#divCurp").hide();
            $("#divClientes").hide();
            $("#divBtn").show();
            break;
        case 'curp':
            $("#divClaveCliente").hide();
            $("#Curp").val("");
            $("#divCurp").show();
            $("#divClientes").hide();
            $("#divBtn").show();
            break;
        case 'nombre':
            $("#divClaveCliente").hide();
            $("#divCurp").hide();
            $("#cliente option:contains(Seleccione)").prop('selected', 'selected');
            $("#divClientes").show();
            $("#divBtn").hide();
            break;
    }
}

function DatosIniciales() {
    $("#btnAbonar").prop('disabled', true);
    $("#btnCancelar").prop('disabled', true);
    $("#txtCantidadRecibida").val("");
    $("#txtPendientePago").val("");
    $("#lblAbonoClienteId").val("");
    $("#NombreCliente").val("");
    $("#divDatosAbono").hide();
    $("#txtSaldo").val("");
    $("#txtAbono").val("");
    $("#tablaArticulos").hide();
    $("#tablaArticulos tbody tr").each(function () {
        $(this).remove();
    });
    var sp = $("#spAbono");
    sp.html("");
    var sp = $("#spCantidadRecibida");
    sp.html("");
}
$("#btnConsultarCliente").click(function (eve) {
    url = "/Ventas/ConsultarCliente?tipo=";
    if ($('input[name=TipoBusqueda]:checked', '#frmAbono').val() == "curp") {
        url = url + "2&curp=" + $("#Curp").val();
        if (!$('#Curp').valid()) {
            return;
        }
    } else if ($('input[name=TipoBusqueda]:checked', '#frmAbono').val() == "id") {
        url = url + "1&id=" + $("#txtClaveCliente").val();
        if (!$('#txtClaveCliente').valid() || $("#txtClaveCliente").val() < 1) {
            return;
        }
    }
    ValidaCheckActivo();
    $.get(url).done(function (data) {
        $('#modal-content').html(data);
        $('#myModal').modal('toggle');
        $('#myModal > .modal').modal('show');
        $("#error").hide();
    });
});

$(".close").click(function (eve) {
    $('#myModal').modal('hide');
});

function ValidaCheckCliente(check) {
    $('.chk').prop('checked', false);
    $(check).prop('checked', true);
}

$("#btnSeleccionar").click(function (eve) {
    if ($("#dataTab tr.bus td input[type='checkbox']").is(":checked")) {
        var fila = $("#dataTab tr.bus td input[type='checkbox']:checked");
        row = $(fila).closest('tr');
        $("#lblAbonoClienteId").text(row.find('td:eq(1)').text().padStart(10, '0'));
        $("#lblNombreCliente").text(row.find('td:eq(2)').text());
        $('#myModal').modal('hide');
        $("#divDatosAbono").show();
        getSaldo(row.find('td:eq(1)').text());
        $("#tablaArticulos").show();
    } else {
        Swal.fire({
            position: 'top-end',
            icon: 'error',
            title: 'Debes seleccionar un cliente!',
            showConfirmButton: false,
            timer: 3000
        });
    }
});

function getSaldo(id) {
    uri = "/Abonos/GetVentasCreditoById?id=" + id;
    $.ajax({
        url: uri,
        type: 'GET',
        success: function (data) {
            let importeAPagar = 0;
            $.each(data, function (index, venta) {
                const fecha = new Date(venta.FechaAlta);
                $('#tablaArticulos').append('<tr><td>' + (index + 1) + '</td><td>' + venta.Id.toString().padStart(10, '0') + '</td><td>' +
                    formatearFechaCorta(fecha) + '</td><td>' + ParseFloatTwoDigits(venta.ImporteVenta) + '</td><td>' +
                    ParseFloatTwoDigits(venta.AbonoVenta) + '</td><td>' + ParseFloatTwoDigits(venta.PendientePago) + '</td></tr>');
                $.each(venta.DetalleVentas, function (index, DetalleVentas) {
                    $('#tablaArticulos').append('<tr><td></td><td>' + DetalleVentas.Descripcion
                        + '</td><td></td><td>' + ParseFloatTwoDigits(DetalleVentas.PrecioVenta)
                        + '</td><td></td><td></td></tr>');
                });
                importeAPagar += venta.PendientePago;
            });
            $("#txtSaldo").val(ParseFloatTwoDigits(importeAPagar));
        },
        error: function (err) {
            console.log(err);
        } 
    });
}

function formatearFechaCorta(fecha) {
    const options = { year: 'numeric', month: '2-digit', day: '2-digit' };
    return fecha.toLocaleDateString('es-ES', options); 
}

function ParseFloatTwoDigits(value) {
    let floatNumber = Number.parseFloat(value).toFixed(2);
    return floatNumber == 'NaN' ? 0 : floatNumber;
}

$('#txtAbono').on('blur', function () {
    var saldo = $('#txtSaldo').val();
    var abono = $('#txtAbono').val();
    if (parseFloat($("#txtCantidadRecibida").val()) > 0) {
        $("#btnAbonar").prop('disabled', false);
        $("#btnCancelar").prop('disabled', false);
    }
    else {
        $("#btnAbonar").prop('disabled', true);
        $("#btnCancelar").prop('disabled', true);
    }
    if (parseFloat(abono) < 0) {
        var sp = $("#spAbono");
        sp.html("Abono/Pago incorrecto!");
        return;
    }
    if (parseFloat(saldo) < abono) {
        var sp = $("#spAbono");
        sp.html("Abono/Pago incorrecto!");
        return;
    }
    $("#txtPendientePago").val((parseFloat(saldo) - parseFloat(abono)).toFixed(2));

    if ($(this).val() > 0)
        $(this).val(parseFloat($(this).val()).toFixed(2));

});

$('#txtCantidadRecibida').on('blur', function () {
    validaCantidadRecibida();
});

function validaCantidadRecibida() {
    $('#txtCantidadRecibida').val(ParseFloatTwoDigits($('#txtCantidadRecibida').val()));
    let cantidadRecibida = $('#txtCantidadRecibida').val();
    let abonoVenta = ParseFloatTwoDigits($('#txtAbono').val());

    if (Number.parseFloat(cantidadRecibida) > 0 && Number.parseFloat(abonoVenta) <= 0) {
        var sp = $("#spCantidadRecibida");
        sp.html("Cantidad recibida incorrecta!");
        return false;
    }

    if (Number.parseFloat(cantidadRecibida) < Number.parseFloat(abonoVenta)) {
        var sp = $("#spCantidadRecibida");
        sp.html("Cantidad recibida incorrecta!");
        return false;
    }

    $("#btnAbonar").prop('disabled', false);
    $("#btnCancelar").prop('disabled', false);
    return true;
}

$("#btnCancelar").click(function (eve) {
    ValidaCheckActivo();
});

$("#btnAbonar").click(function (eve) {
    event.preventDefault();
    var form = $(this).parents('.row').find('form');
    var actionUrl = form.attr('action');

    var abono = $('#txtAbono').val();
   
    if (parseFloat(abono) > 0) {
        if (!$('#txtCantidadRecibida').valid() || parseFloat($('#txtCantidadRecibida').val()) <= 0) {
            return;
        }
    }

    if (!validaCantidadRecibida()) {
        return;
    }

    const AbonoVO = {
        AbonoClienteId: parseInt($("#lblAbonoClienteId").text()),
        Abono: parseFloat($("#txtAbono").val()),
        CantidadRecibida: parseFloat($("#txtCantidadRecibida").val()),
        Saldo: parseFloat($("#txtSaldo").val())
    };
    
    Swal.fire({
        title: "Confirmar",
        text: "¿Deseas realizar el abono?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si",
        cancelButtonText: "No"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "POST",
                cache: false,
                url: actionUrl,
                data: { abonoVO: AbonoVO },
                beforeSend: function (request) {
                    request.setRequestHeader("RequestVerificationToken", $("[name='__RequestVerificationToken']").val());
                },
                success: function (data) {
                    let response = data.split(':');
                    if (parseInt(response[0]) == 1) {
                        Swal.fire({
                            position: 'top-end',
                            icon: 'success',
                            title: 'Abono exitoso.!',
                            showConfirmButton: false,
                            timer: 3000
                        });
                        setTimeout(function () {
                            location.reload();
                        }, 3000);
                    } else {
                        var message = response.length == 2 ? response[1] : 'error';
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
                        } else {
                            $("#error").hide();
                        }
                        Swal.fire({
                            position: 'top-end',
                            icon: 'error',
                            title: 'Error al realizar el abono.!',
                            showConfirmButton: false,
                            timer: 3000
                        });
                    }

                },
                error: function () {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'error',
                        title: 'Error al realizar el abono.!',
                        showConfirmButton: false,
                        timer: 3000
                    });
                }
            });

        }
    });
});