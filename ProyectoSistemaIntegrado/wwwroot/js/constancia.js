function getHtmlConstanciaIngreso(codigoTransaccion, codigoOperacion, numeroRecibo, fechaRecibo, codigoEntidad, nombreEntidad, nombreOperacion, monto, recursos, usuarioCreacion, ruta, fechaImpresion, codigoSeguridad) {
    let table = `<div style="width: 287px; max-width: 287px; text-align: center; align-content: center; margin-left: 25px;">
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif'; font-weight: bold;">TESORERÍA</p>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif'; font-weight: bold;">${codigoSeguridad}</p>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif';">ING</p>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif';">${codigoTransaccion}</p>
                 <table style="border-top: 1px solid black; border-collapse: collapse;">
                    <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Recibo:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${numeroRecibo}</td>
                    </tr>
                    <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Fecha:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${fechaRecibo}</td>
                    </tr>`;
                    if (codigoOperacion == VENTAS_EN_RUTA) {
                        table += `<tr style="border-collapse: collapse;">
                                    <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Código:</td>
                                    <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${codigoEntidad}</td>
                                  </tr>`;
                    }
    table += `      <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Nombre:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${nombreEntidad}</td>
                    </tr>`;
                    if (codigoOperacion == VENTAS_EN_RUTA) {
                        table += `<tr style="border-collapse: collapse;">
                                    <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Ruta:</td>
                                    <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${ruta}</td>
                                  </tr>`;
                    }
    table += `      <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Concepto:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${nombreOperacion}</td>
                    </tr>
                    <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Recurso:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${recursos}</td>
                    </tr>
                    <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Monto:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${monto}</td>
                    </tr>
                 </table>
                 <p style="text-align: center; align-content: center; font-size: 16px; font-family: 'Arial, sans-serif';">${fechaImpresion}<br>
                 <p style="text-align: center; align-content: center; font-size: 16px; font-family: 'Arial, sans-serif';">${usuarioCreacion}<br>
                 </div>`;
    return table;
}

function getHtmlConstanciaEgreso(codigoTransaccion, numeroRecibo, fechaRecibo, nombreEntidad, nombreOperacion, monto, usuarioCreacion, fechaImpresion, codigoSeguridad) {
    let table = `<div style="width: 287px; max-width: 287px; text-align: center; align-content: center; margin-left: 25px;">
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif'; font-weight: bold;">TESORERÍA</p>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif'; font-weight: bold;">${codigoSeguridad}</p>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif';">EGR</p>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif';">${codigoTransaccion}</p>
                 <table style="border-top: 1px solid black; border-collapse: collapse;">
                    <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Recibo:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${numeroRecibo}</td>
                    </tr>
                    <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Fecha:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${fechaRecibo}</td>
                    </tr>
                    <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Nombre:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${nombreEntidad}</td>
                    </tr>
                    <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Concepto:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${nombreOperacion}</td>
                    </tr>
                    <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Monto:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${monto}</td>
                    </tr>
                 </table>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif';">${fechaImpresion}<br>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif';">${usuarioCreacion}<br>
                 </div>`;

    return table;
}


function PrintConstanciaIngresos(codigoTransaccion, codigoTipoOperacion) {
    let constancia = "";
    switch (codigoTipoOperacion) {
        case 1:
            constancia = "ViewConstanciaIngreso";
            break;
        case -1:
            constancia = "ViewConstanciaEgreso";
            break;
        default:
            break;
    }
        //contentType: 'application/my-binary-type',
    fetchGet("Reportes/" + constancia + "/?codigoTransaccion=" + codigoTransaccion, "pdf", function (data) {
        let blob = new Blob([data], { type: 'application/pdf' });
        /*var fileURL = URL.createObjectURL(blob);
        window.open(fileURL, "EPrescription");*/
        var formData = new FormData()
        formData.append('source', blob)
        $.ajax({
            url: "http://127.0.0.1:8000/pdf",
            type: "POST",
            data: formData,
            dataType: 'pdf',
            processData: false,
            contentType: false,
            success: function (response) {
                //alert(response);
                //Exito("Transaccion", "Index", true);
            },
            error: function (type) {
                //alert("ERROR!!" + type.responseText);
                //Exito("Transaccion", "Index", true);
            }
        });
    });

}

