$(function () {
    var placeholderElement = $('#myModal');
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            var minFecha = new Date();
            minFecha.setFullYear(minFecha.getFullYear() - 18);
            $('#modal-content').html(data);
            $('#myModal').modal('toggle');
            $('#myModal > .modal').modal('show');
            var fecha = minFecha.toLocaleDateString('en-CA');
            $("#fechaNacimiento").attr("max", fecha);
            $("#fechaNacimiento").val(fecha);
            $("#error").hide();
        });
    });  
});

function validar(campo, limite) {
   valor = $('#' + campo).val();
    if (valor.length > limite)  return false;
}

function onlyNumbers(event) {
    var keynum = window.event ? window.event.keyCode : e.which;
    if (keynum == 8) return true;
    var simbolos = [ "!", '"', "#", "$", "%", "&", "/", "(", ")", "=", "´", "+", "*", "Dead"];
    patron = /[0-9]/;
    var foco = false;
    for (i = 0; i < simbolos.length; i++) {
        if (event.key.indexOf(simbolos[i]) == 0) {
            foco = true;
            i = simbolos.length;
        }
    }
    if (!foco) {
        tecla_final = String.fromCharCode(keynum);
    }
    else {
        tecla_final = String.fromCharCode(event.key);
    }
    return patron.test(tecla_final);
}

function onlyLetras(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    if ((tecla == 8) || (tecla == 32)) return true;
    patron = /[A-Za-záéíóúÁÉÍÓÚñÑ]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}

function letrasNumeros(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    if (tecla == 8) return true;
    patron = /[A-Za-z0-9]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}

function upper(e) {
    return e.toUpperCase();
}