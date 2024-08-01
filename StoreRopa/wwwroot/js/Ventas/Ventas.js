$(document).ready(function () {
    $(".abonoArticulo").css("display", "none");
    $("#txtAbonoVenta").prop("disabled", true);
    $("#error").hide();
    ValidaCheckActivo();
    $("input[name=TipoBusqueda]").on('change', function () {
        ValidaCheckActivo();
    });
    $("#cliente").on('change', function () {
        DatosIniciales();
        if ($(this).val() > 0) {
            $("#VentaClienteId").text($(this).val().padStart(10, '0'));
            $("#lblNombreCliente").text($('select[id="cliente"] option:selected').text());
            $("#divDatosVenta").show();
            url = "/Ventas/ConsultarTipoVentaClienteById?id=" + $(this).val();
            $.get(url).done(function (data) {                
                if (data == "2") {
                    $("input[type='radio'][name='TipoVenta'][value='credito']").prop('checked', true);
                    $("#divTipoVenta").show();
                } else if (data == "1") {
                    $("input[type='radio'][name='TipoVenta'][value='contado']").prop('checked', true);
                    $("#divTipoVenta").hide();
                } else {
                    $("#error").show();
                    var err = $("#error");
                    err.html("Error al consultar Cliente");
                    $("#error").fadeOut(10000, function () {
                        $("#error").remove();
                    });
                }
                ValidaCheckTipoVenta();
            });
        }        
    });
    $("input[name=TipoVenta]").on('change', function () {
        ValidaCheckTipoVenta();
    });
});
function ValidaCheckActivo() {
    DatosIniciales();
    switch ($('input[name=TipoBusqueda]:checked', '#frmVentas').val()) {
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

$("#btnConsultarCliente").click(function (eve) {
    url = "/Ventas/ConsultarCliente?tipo=";
    if ($('input[name=TipoBusqueda]:checked', '#frmVentas').val() == "curp") {
        url = url + "2&curp=" + $("#Curp").val();
        if (!$('#Curp').valid()) {
            return;
        }
    } else if ($('input[name=TipoBusqueda]:checked', '#frmVentas').val() == "id") {
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
        $("#VentaClienteId").text(row.find('td:eq(1)').text().padStart(10, '0'));
        $("#lblNombreCliente").text(row.find('td:eq(2)').text());
        $('#myModal').modal('hide');
        $("#divDatosVenta").show();
        if (row.find('td:eq(5)').text() == "Crédito") {
            $("input[type='radio'][name='TipoVenta'][value='credito']").prop('checked', true);
            $("#divTipoVenta").show();
        } else {
            $("input[type='radio'][name='TipoVenta'][value='contado']").prop('checked', true);
        }
        ValidaCheckTipoVenta();
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
function DatosIniciales() {
    $("#divDatosVenta").hide();
    $("#VentaClienteId").val("");
    $("#NombreCliente").val("");
    $("#divTipoVenta").hide();
    $("#txtImporteVenta").val("");
    $("#txtAbonoVenta").val("");
    $("#tablaArticulos").hide();
    $("#txtDescripcion").val("");
    $("#txtTalla").val("");
    $("#txtColor").val("");
    $("#txtModelo").val("");
    $("#txtPrecioArticulo").val("");
    $("#txtDescuento").val("");
    $("#txtPrecioVenta").val("");
    $("#tablaArticulos tbody tr").each(function () {
        $(this).remove();
    });
    var sp = $("#spAbono");
    sp.html("");
}

function ValidaCheckTipoVenta() {
    switch ($('input[name=TipoVenta]:checked', '#frmVentas').val()) {
        case 'contado':
            $("#divPendientePago").hide();
            $(".abonoArticulo").css("display", "none");
            break;
        case 'credito':
            $("#divPendientePago").show();
            $(".abonoArticulo").css("display", "block");
            break;
    }
}

function ParseFloatTwoDigits(value) {
    let floatNumber = Number.parseFloat(value).toFixed(2);
    return floatNumber == 'NaN' ? 0 : floatNumber;
}

$('#txtPrecioArticulo').on('blur', function () {
    $('#txtPrecioArticulo').val(ParseFloatTwoDigits($('#txtPrecioArticulo').val()));
    let precioArticulo = $('#txtPrecioArticulo').val();
    let descuentoArticulo = ParseFloatTwoDigits($('#txtDescuento').val());
    
    if (Number.parseFloat(precioArticulo) < Number.parseFloat(descuentoArticulo)) {
        var sp = $("#spDescuento");
        sp.html("Descuento incorrecto!");
        return;
    }
    if ($('#txtDescuento').val() == "") {
        $('#txtDescuento').val("0");
    }
    $('#txtPrecioVenta').val(ParseFloatTwoDigits($('#txtPrecioArticulo').val() - $('#txtDescuento').val()));
});

$('#txtDescuento').on('blur', function () {
    $('#txtDescuento').val(ParseFloatTwoDigits($('#txtDescuento').val()));
    let descuentoArticulo = $('#txtDescuento').val();
    let precioArticulo = ParseFloatTwoDigits($('#txtPrecioArticulo').val());
    if (Number.parseFloat(precioArticulo) < Number.parseFloat(descuentoArticulo)) {
        var sp = $("#spDescuento");
        sp.html("Descuento incorrecto!");
        return;
    }
    if ($('#txtPrecioArticulo').val() == "") {
        $('#txtPrecioArticulo').val("0");
    }
    $('#txtPrecioVenta').val(ParseFloatTwoDigits($('#txtPrecioArticulo').val() - $('#txtDescuento').val()));
});

$("#btnAgregar").click(function (eve) {
    var sp = $("#spAbono").html();
    if (sp != "") {
        return;
    }
    if ($('#txtPrecioArticulo').val() == 0) {
        var sp = $("#spPrecioArticulo");
        sp.html("Precio incorrecto!");
        return;
    }
    if (!$('#txtDescripcion').valid() || !$('#txtTalla').valid() || !$('#txtColor').valid() || !$('#txtModelo').valid()
        || !$('#txtPrecioArticulo').valid() || !$('#txtDescuento').valid()) {
        return;
    }
    if (parseFloat($('#txtPrecioArticulo').val()) < parseFloat($('#txtDescuento').val())) {
        var sp = $("#spDescuento");
        sp.html("Descuento incorrecto!");
        return;
    }
    $('#txtPrecioVenta').val($('#txtPrecioArticulo').val() - $('#txtDescuento').val());
    $("#tablaArticulos").show();    
    agregarFila();
    $("#txtAbonoVenta").prop("disabled", false);
    $("#txtAbonoVenta").val("0");
    if ($('input[name=TipoVenta]:checked', '#frmVentas').val() == 'contado') {
        $(".abonoArticulo").css("display", "none");
    } else {
        $(".abonoArticulo").css("display", "block");
    }
    $('#txtPendientePago').val(parseFloat($('#txtImporteVenta').val() - $('#txtAbonoVenta').val()).toFixed(2));
    $("#tablaArticulos tbody tr input").each(function () {
        $(this).val(parseFloat(0.00).toFixed(2));
    });
});
function DecimalLongitud(event, txt) {
    var splitTxt = $(txt).val().split('.');
    if ($(txt).val().length > 10) {
        return false;
    }
    if ($.isNumeric(event.key) || (event.key == "." && $(txt).val().length > 0 && splitTxt.length < 2) ||
        event.key == 'Backspace' || event.key == 'Tab' || event.key == 'Delete' || event.key == 'ArrowLeft' || event.key == 'ArrowRight' ) {
        return true;
    }
    return false;
}

function formateaMoneda(txt,fila) {
    if ($('#txtAbonoVenta').val() <= 0) {
        $(txt).val("0");
        return;
    }
    if ($(txt).val() > 0)
        $(txt).val(parseFloat($(txt).val()).toFixed(2));
    
    if ($(txt).val() <= $('#txtAbonoVenta').val()) {
        var precio = parseFloat($("#fila" + fila).find('td:eq(7)').text());
        if (precio < $(txt).val()) {
            var sp = $("#spAbonoArticulo" + fila);
            sp.html("Abono incorrecto!");
            return;
        }
        var sp = $("#spAbonoArticulo" + fila);
        sp.html("");

    }
    else {
        var sp = $("#spAbonoArticulo" + fila);
        sp.html("Abono incorrecto!");
        return;
    }
        
}

function agregarFila() {   
    var fila = $("#tablaArticulos").find('tbody tr').length;
    var txt = "<div class='col col-lg-2'> <div class='form-outline'> <input type='text' class='form-control-sm' " +
        "id='txtAbonoArticulo' onkeypress = 'return DecimalLongitud(event, this);' onblur = 'formateaMoneda(this," + fila + ")'" +
        "data-val-number='La cantidad es incorrecta!' value='0.00'/> <span asp-validation-for='txtAbonoArticulo''" +
        " id='spAbonoArticulo" + fila + "' class='text-danger'></span></div> </div>";
    var boton = ' <a class="borrar" style="cursor: hand; cursor: pointer;" title="Eliminar Articulo">' +
        '<span style = " color: red" ><i class="bi bi-trash-fill"></i></span > </a > ';
    var htmlTags = '<tr id="fila' + fila + '"> <td>' + (fila + 1) + '</td> <td>' + $("#txtDescripcion").val() + '</td> <td>'
        + $("#txtTalla").val() + '</td> <td>' + $("#txtColor").val() + '</td> <td>' + $("#txtModelo").val() + '</td>'
        + '<td>' + $("#txtPrecioArticulo").val() + '</td> <td>' + $("#txtDescuento").val() + '</td> <td>' +
        $("#txtPrecioVenta").val() + '</td> <td class="abonoArticulo">' + txt + '</td> <td>' + boton + '</td> </tr>';
    $('#tablaArticulos tbody').append(htmlTags);
    var importe = parseFloat($("#txtImporteVenta").val());
    if (isNaN(importe)) importe = 0;
    var total = (importe + parseFloat($("#txtPrecioVenta").val()));
    $("#txtImporteVenta").val(parseFloat(total).toFixed(2));
    $("#txtDescripcion").val("");
    $("#txtTalla").val("");
    $("#txtColor").val("");
    $("#txtModelo").val("");
    $("#txtPrecioArticulo").val("");
    $("#txtDescuento").val("");
    $("#txtPrecioVenta").val("");
}

$(document).on('click', '.borrar', function (event) {
    event.preventDefault();
    var precio = parseFloat($(this).parents('tr').find('td:eq(7)').text());
    var total = (parseFloat($("#txtImporteVenta").val()) - precio);
    if (isNaN(total)) total = 0;
    $("#txtImporteVenta").val(total);
    var porPagar = $('#txtImporteVenta').val() - $('#txtAbonoVenta').val();
    if (parseFloat($('#txtImporteVenta').val()) < parseFloat($('#txtAbonoVenta').val())) {
        $('#txtAbonoVenta').val($('#txtImporteVenta').val())
    }
    if (porPagar < 0) {
        $('#txtPendientePago').val(0);
    }
    else {
        $('#txtPendientePago').val(porPagar);
    }
    $(this).parents('tr').remove();
    var rows = $("#tablaArticulos").find('tbody tr').length;
    if (rows == 0) {
        $("#tablaArticulos").hide();
    }
    var renglon = 1;
    $("#tablaArticulos tbody tr").each(function () {
        $(this).children().eq(0).text(renglon);
        renglon++;
    });

    calculaPago();
});

function calculaPago() {
    $('#txtPendientePago').val(parseFloat($('#txtImporteVenta').val() - $('#txtAbonoVenta').val()).toFixed(2));
    var filas = $("#tablaArticulos").find('tbody tr').length;
    var cantidad = $('#txtAbonoVenta').val() / filas;

    var renglon = 1;
    var modificaCantidad = false;
    var cantidadAdicional = 0;
    var cantidadAux = 0;
    var cantidades = [];
    var cantidadesAux = [];
    $("#tablaArticulos tbody tr input").each(function () {
        cantidades.push(parseFloat($("#tablaArticulos tbody tr").eq((renglon - 1)).find('td:eq(7)').text()) + "-" + (renglon - 1));
        renglon++;
    });
    cantidades.sort();

    let registrado = 0;
    let isModificatedCantidad = false;
    let cantidadInicial = cantidad;

    for (let i = 0; i < cantidades.length; i++) {
        renglon = cantidades[i].split('-');
        if (modificaCantidad) {
            cantidad = cantidadAux;
            modificaCantidad = false;
        }
        if (renglon[0] >= cantidad) {
            if ((i == (cantidades.length - 2)) && cantidades.length > 2) {
                cantidadesAux.push(renglon[0] + "-" + renglon[1] + "-" + cantidad);
                registrado += parseFloat(cantidad);
                cantidadAux = parseFloat($('#txtAbonoVenta').val()) - registrado;
                modificaCantidad = true;
            }
            else {
                cantidadesAux.push(renglon[0] + "-" + renglon[1] + "-" + cantidad);
                registrado += parseFloat(cantidad);
            }

        } else {
            cantidadesAux.push(renglon[0] + "-" + renglon[1] + "-" + renglon[0]);
            registrado += parseFloat(renglon[0]);
            cantidadAdicional = cantidad - renglon[0];
            if ((i == (cantidades.length - 2)) && cantidades.length > 2) {
                cantidadAux = parseFloat($('#txtAbonoVenta').val()) - registrado;
            }
            else {
                if (!isModificatedCantidad) {
                    cantidadAux = cantidad + cantidadAdicional;
                    isModificatedCantidad = true;
                }
                else {
                    cantidadAux = cantidadInicial + cantidadAdicional;
                }
            }
            modificaCantidad = true;

        }
    }

    let totAbono = 0;
    renglon = 0;
    $("#tablaArticulos tbody tr input").each(function () {
        for (let i = 0; i < cantidadesAux.length; i++) {
            if (renglon == cantidadesAux.length - 1) {
                let ultimoAbono = $('#txtAbonoVenta').val() - totAbono;
                $(this).val(parseFloat(ultimoAbono).toFixed(2));
                break;
            }
            let linea = cantidadesAux[i].split('-');
            if (parseInt(linea[1]) == renglon) {
                $(this).val(parseFloat(linea[2]).toFixed(2));
                totAbono += parseFloat(linea[2]);
                break;
            }

        }
        renglon += 1;
    });
}

$('#txtAbonoVenta').on('blur', function () {
    var importe = $('#txtImporteVenta').val();
    var abono = $('#txtAbonoVenta').val();
    if (importe == 0) importe = 0;
    if (parseFloat(importe) < abono) {
        $("#tablaArticulos tbody tr input").each(function () {
            $(this).val("0");
        });
        var sp = $("#spAbono");
        sp.html("Abono/Pago incorrecto!");
        return;
    }
    if ($('input[name=TipoVenta]:checked', '#frmVentas').val() == 'contado') {
        if (parseFloat(abono) != parseFloat(importe)) {
            $("#tablaArticulos tbody tr input").each(function () {
                $(this).val("0");
            });
            var sp = $("#spAbono");
            sp.html("Abono/Pago incorrecto!");
            return;
        }
    }
    if ($(this).val() > 0)
        $(this).val(parseFloat($(this).val()).toFixed(2));

    calculaPago();

});


$("#btnCancelar").click(function (eve) {
    ValidaCheckActivo();
});

$("#btnVender").click(function (eve) {
    event.preventDefault();
    var form = $(this).parents('.row').find('form');
    var actionUrl = form.attr('action');

    let rows = $("#tablaArticulos").find('tbody tr').length;
    if (rows == 0) {
        Swal.fire({
            position: 'top-end',
            icon: 'error',
            title: 'No existen articulos para la venta.!',
            showConfirmButton: false,
            timer: 3000
        });
        return;
    }

    var importe = $('#txtImporteVenta').val();
    var abono = $('#txtAbonoVenta').val();
    if (parseFloat(importe) < abono) {
        var sp = $("#spAbono");
        sp.html("Abono/Pago incorrecto!");
        return;
    }

    var isCredito = true;
    if ($('input[name=TipoVenta]:checked', '#frmVentas').val() == 'contado') {
        isCredito = false;
        if (parseFloat(abono) != parseFloat(importe)) {
            var sp = $("#spAbono");
            sp.html("Abono/Pago incorrecto!");
            return;
        }
    }
    
    const venta = {
        ClienteId: parseInt($("#VentaClienteId").text()),
        ImporteVenta: parseFloat($("#txtImporteVenta").val()),
        AbonoVenta: parseFloat($("#txtAbonoVenta").val()),
        PendientePago: parseFloat($("#txtPendientePago").val()),
        EsVentaCredito: isCredito,
        EmpleadoId: 0
    };

    const VentaVo = { 
        Venta:  venta ,
        DetallesDeVentas:  []
    }

    let renglon = 0;
    $("#tablaArticulos tbody tr input").each(function () {
        let descripcion = $("#tablaArticulos tbody tr").eq(renglon).find('td:eq(1)').text();
        let talla = $("#tablaArticulos tbody tr").eq(renglon).find('td:eq(2)').text();
        let color = $("#tablaArticulos tbody tr").eq(renglon).find('td:eq(3)').text();
        let modelo = $("#tablaArticulos tbody tr").eq(renglon).find('td:eq(4)').text();
        let precioArticulo = $("#tablaArticulos tbody tr").eq(renglon).find('td:eq(5)').text();
        let descuento = $("#tablaArticulos tbody tr").eq(renglon).find('td:eq(6)').text();
        let precioVenta = $("#tablaArticulos tbody tr").eq(renglon).find('td:eq(7)').text();
        let abono = isCredito ? parseFloat($(this).val()) : 0;

        let detalleVenta = {
            Descripcion: descripcion,
            Talla: talla,	
            Color: color,	
            Modelo: modelo,
            PrecioArticulo: precioArticulo,	
            Descuento: descuento,
            PrecioVenta: precioVenta,
            Abono: abono
        };
        VentaVo.DetallesDeVentas.push(detalleVenta);
        renglon++;
    });
    
    Swal.fire({
        title: "Confirmar",
        text: "¿Deseas realizar la venta?",
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
                data: { venta: VentaVo },
                beforeSend: function (request) {
                    request.setRequestHeader("RequestVerificationToken", $("[name='__RequestVerificationToken']").val());
                },
                success: function (data) {
                    let response = data.split(':');
                    if (parseInt(response[0]) == 1) {
                        Swal.fire({
                            position: 'top-end',
                            icon: 'success',
                            title: 'Venta exitosa.!',
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
                            title: 'Error al realizar la venta.!',
                            showConfirmButton: false,
                            timer: 3000
                        });
                    }

                },
                error: function () {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'error',
                        title: 'Error al realizar la venta.!',
                        showConfirmButton: false,
                        timer: 3000
                    });
                }
            });

        }
    });   

});
