$(document).ready(function () {
    let actionUrl = "/Home/DetallesVentas";
    $.get(actionUrl).done(function (data) {
        $("#lblVentas").text(data.ventas);
        $("#lblImporteVenta").text(data.importeVenta.toLocaleString('es-mx', { style: 'currency', currency: 'MXN' }));
        $("#lblAbonos").text(data.abonos);
        $("#lblImporteAbonos").text(data.importeAbonos.toLocaleString('es-mx', { style: 'currency', currency: 'MXN' }));
        $("#lblImporteTotal").text(data.importeTotal.toLocaleString('es-mx', { style: 'currency', currency: 'MXN' }));
        $("#lblVentasG").text(data.ventasG);
        $("#lblImporteVentaG").text(data.importeVentaG.toLocaleString('es-mx', { style: 'currency', currency: 'MXN' }));
        $("#lblAbonosG").text(data.abonosG);
        $("#lblImporteAbonosG").text(data.importeAbonosG.toLocaleString('es-mx', { style: 'currency', currency: 'MXN' }));
        $("#lblImporteTotalG").text(data.importeTotalG.toLocaleString('es-mx', { style: 'currency', currency: 'MXN' }));
        if (data.rolUsuario != "Administrador") {
            $("#divDatosGenerales").hide();
        }        
    });
});