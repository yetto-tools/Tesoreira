﻿function getHtmlConstanciaIngreso(codigoTransaccion, codigoOperacion, numeroRecibo, fechaRecibo, codigoEntidad, nombreEntidad, nombreOperacion, monto, recursos, usuarioCreacion, ruta, fechaImpresion, codigoSeguridad, montoSaldoAnteriorCxC, montoSaldoActualCxC, observaciones, codigoTransaccionAnt) {
    let table = `<div style="width: 287px; max-width: 287px; text-align: center; align-content: center; margin-left: 25px;">
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif'; font-weight: bold;">TESORERÍA</p>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif'; font-weight: bold;">${codigoSeguridad}</p>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif';">ING</p>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif';">${codigoTransaccion}</p>`
                 if (codigoTransaccionAnt != 0) {
                     table += `<p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif'; font-weight: bold;">MODIFICA (${codigoTransaccionAnt})</p>`;
                 }

                table += `<table style="border-top: 1px solid black; border-collapse: collapse;">
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
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Motivo:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${observaciones}</td>
                    </tr>
                    <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Recurso:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${recursos}</td>
                    </tr>`;


                    if (codigoOperacion == DEVOLUCION_ANTICIPIO_LIQUIDABLE || codigoOperacion == DEVOLUCION_ANTICIPO_SALARIO || codigoOperacion == ABONO_A_PRESTAMO) {

                        table += `<tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Saldo Anterior:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${formatRegional(montoSaldoAnteriorCxC)}</td>
                        </tr>`;
                    }


    table +=        `<tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Monto:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">`
    if (codigoOperacion == DEVOLUCION_ANTICIPIO_LIQUIDABLE || codigoOperacion == DEVOLUCION_ANTICIPO_SALARIO || codigoOperacion == ABONO_A_PRESTAMO) {
        table += `<div style="text-align:left">(-) ${formatRegional(monto)}</div>`;
    } else {
        table += `<div style="text-align:left">${formatRegional(monto)}</div>`;
    }
    table +=        ` </td>
                    </tr>`;

                    if (codigoOperacion == DEVOLUCION_ANTICIPIO_LIQUIDABLE || codigoOperacion == DEVOLUCION_ANTICIPO_SALARIO || codigoOperacion == ABONO_A_PRESTAMO) {

                        table += `<tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Saldo Actual:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${formatRegional(montoSaldoActualCxC)}</td>
                        </tr>`;
                    }


    table +=     `</table>
                 <p style="text-align: center; align-content: center; font-size: 16px; font-family: 'Arial, sans-serif';">${fechaImpresion}<br>
                 <p style="text-align: center; align-content: center; font-size: 16px; font-family: 'Arial, sans-serif';">${usuarioCreacion}<br>
                 </div>`;
    return table;
}

function getHtmlConstanciaEgreso(codigoTransaccion, codigoOperacion, codigoOperacionCaja, numeroRecibo, fechaRecibo, nombreEntidad, nombreOperacion, monto, usuarioCreacion, fechaImpresion, codigoSeguridad, montoSaldoAnteriorCxC, montoSaldoActualCxC, numeroCuenta, observaciones, periodoComision, codigoTransaccionAnt) {
    let table = `<div style="width: 287px; max-width: 287px; text-align: center; align-content: center; margin-left: 25px;">
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif'; font-weight: bold;">TESORERÍA</p>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif'; font-weight: bold;">${codigoSeguridad}</p>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif';">EGR</p>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif';">${codigoTransaccion}</p>`
                 if (codigoTransaccionAnt != 0) {
                     table += `<p style = "text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif'; font-weight: bold;" >MODIFICA (${codigoTransaccionAnt})</p>`;
                 }

                table += `<table style="border-top: 1px solid black; border-collapse: collapse;">
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
                    </tr>`
                    if (codigoOperacion == DEPOSITOS_BANCARIOS)
                    {
                        table += `<tr style="border-collapse: collapse;">
                            <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Cuenta:</td>
                            <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${numeroCuenta}</td>
                        </tr>`
                    }

        table += `  <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Concepto:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${nombreOperacion}</td>
                    </tr>`;

                    if (codigoOperacion == PLANILLA_BONOS_EXTRAS && codigoOperacionCaja == PLANILLA_BONOS_EXTRAS) {
                        table += `<tr style="border-collapse: collapse;">
                            <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Periodo:</td>
                            <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${periodoComision}</td>
                        </tr>`
                    }

        table += `  <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Motivo:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${observaciones}</td>
                    </tr>`;

                    if (codigoOperacion == ANTICIPO_LIQUIDABLE || codigoOperacion == ANTICIPO_SALARIO || codigoOperacion == PRESTAMO) {

                        table += `<tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Saldo Anterior:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${formatRegional(montoSaldoAnteriorCxC)}</td>
                        </tr>`;
                    }

          table += `<tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Monto:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">`
                    if (codigoOperacion == ANTICIPO_LIQUIDABLE || codigoOperacion == ANTICIPO_SALARIO || codigoOperacion == PRESTAMO) {
                        table += `<div style="text-align:left">(+) ${formatRegional(monto)}</div>`;
                    } else {
                        table += `<div style="text-align:left">${formatRegional(monto)}</div>`;
                    }

            table += ` </td>
                    </tr>`

                    if (codigoOperacion == ANTICIPO_LIQUIDABLE || codigoOperacion == ANTICIPO_SALARIO || codigoOperacion == PRESTAMO) {

                        table += `<tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Saldo Actual:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${formatRegional(montoSaldoActualCxC)}</td>
                        </tr>`;
                    }


    table += `     </table>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif';">${fechaImpresion}<br>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif';">${usuarioCreacion}<br>
                 </div>`;

    return table;
}


function PrintConstanciaIngresos(codigoTransaccion, codigoTipoOperacion, nombreImpresora, numeroCopias) {
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
            url: "http://127.0.0.1:8000/pdf?" + "&printto=" + nombreImpresora + "&numbercopies=" + numeroCopias.toString(),
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

