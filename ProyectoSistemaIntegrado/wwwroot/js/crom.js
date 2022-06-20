window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    if (nameController == "Especiales2") {
        switch (nameAction) {
            case "GeneracionTraslados":
                MostrarTrasladosEspeciales2Generados();
                break;
            case "ImportEspeciales2":
                MostrarTrasladosParaImportacion();
                break;
            default:
                break;
        }// fin switch
    } 
}

function MostrarTrasladosEspeciales2Generados() {
    let objConfiguracion = {
        url: "Especiales2/GetTrasladosEnProceso",
        cabeceras: ["Código", "Fecha Operación", "Fecha Ingreso", "Fecha Traslado", "Monto Total", "Número Pedidos", "observaciones", "Usuario Ingreso", "Codigo Estado", "Estado", "permisoImprimir"],
        propiedades: ["codigoTraslado", "fechaOperacionStr", "fechaIngresoStr", "fechaTrasladoStr", "montoTotal", "numeroPedidos", "observacionesTraslado", "usuarioIngreso", "codigoEstado", "estado", "permisoImprimir"],
        displaydecimals: ["montoTotal"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        ocultarColumnas: true,
        aceptar: true,
        funcionaceptar: "AceptarGeneracionTrasladoEspeciales2",
        hideColumns: [
            {
                "targets": [0],
                "visible": true
            }, {
                "targets": [1],
                "visible": true
            }, {
                "targets": [2],
                "visible": true
            }],
        slug: "codigoTraslado",
        imprimir: true
    }
    pintar(objConfiguracion);
}

function clickAceptarGeneracionTrasladoEspeciales2(value) {
    let frm = new FormData();
    frm.set("CodigoTraslado", value.toString());
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Especiales2/AceptarGeneracionTrasladoEspeciales2", "text", frm, function (data) {
            if (data == "OK") {
                MostrarTrasladosEspeciales2Generados();
            } else {
                MensajeError(data);
            }
        });
    });
}


function GenerarTrasladosEspeciales2() {
    let fechaStr = document.getElementById("uiFechaOperacion").value;
    if (fechaStr == "") {
        Warning("No existe fecha de operación");
        return;
    }

    let fechaOperacionStr = convertFormatDate(fechaStr);
    fetchGet("Especiales2/GenerarTrasladosDeEspeciales2/?fechaOperacion=" + fechaOperacionStr, "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            Warning("Error en la obtención de entidades");
        }
        else {
            let objConfiguracion = {
                cabeceras: ["Código", "Fecha Operación", "Fecha Ingreso", "Fecha Traslado", "Monto Total", "Número Pedidos", "observaciones", "Usuario Ingreso", "Codigo Estado", "Estado","permisoImprimir"],
                propiedades: ["codigoTraslado", "fechaOperacionStr", "fechaIngresoStr", "fechaTrasladoStr", "montoTotal", "numeroPedidos", "observacionesTraslado", "usuarioIngreso", "codigoEstado", "estado","permisoImprimir"],
                displaydecimals: ["montoTotal"],
                divContenedorTabla: "divContenedorTabla",
                divPintado: "divTabla",
                ocultarColumnas: true,
                hideColumns: [
                    {
                        "targets": [0],
                        "visible": true
                    }, {
                        "targets": [1],
                        "visible": true
                    }, {
                        "targets": [2],
                        "visible": true
                    }],
                slug: "codigoTraslado",
                imprimir: true
            }
            pintarEntidades(objConfiguracion, rpta);
        }
    });
}

function MostrarTrasladosParaImportacion() {
    let objConfiguracion = {
        url: "Especiales2/GetTrasladosParaImportacion",
        cabeceras: ["Código", "Fecha Operación", "Fecha Ingreso", "Fecha Traslado", "Monto Total", "Número Pedidos", "observaciones", "Usuario Ingreso", "Codigo Estado", "Estado","permisoImportar","permisoDepurar"],
        propiedades: ["codigoTraslado", "fechaOperacionStr", "fechaIngresoStr", "fechaTrasladoStr", "montoTotal", "numeroPedidos", "observacionesTraslado", "usuarioIngreso", "codigoEstado", "estado","permisoImportar","permisoDepurar"],
        displaydecimals: ["montoTotal"],
        datesWithoutTime: ["fechaOperacionStr"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "visible": true
            }, {
                "targets": [1],
                "visible": true
            }, {
                "targets": [2],
                "visible": true
            }],
        slug: "codigoTraslado",
        import: true,
        funcionimport: "Especiales2",
        depurar: true,
        funciondepurar: "NombresClientesEspeciales2"
    }
    pintar(objConfiguracion);
}

function ImportarEspeciales2() {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click','.option-import', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        let fechaOperacionStr = table.cell(rowIdx, 1).data();
        fetchGet("Especiales2/GetDetalleTrasladosEspeciales2/?codigoTraslado=" + codigoTraslado, "json", function (rpta) {
            if (rpta != undefined && rpta != null && rpta.length != 0) {
                let jsonData = JSON.stringify(rpta);
                fetchPostJson("Especiales2/GuardarTraslados/?codigoTraslado=" + codigoTraslado + "&fechaOperacionStr=" + fechaOperacionStr, "text", jsonData, function (data) {
                    if (data == "OK") {
                        clickAceptarImportacion(codigoTraslado);
                    } else {
                        MensajeError(data);
                    }
                })
            } else {
                MensajeError("Detalle vacio");
            }
        })
    });
}

function clickAceptarImportacion(codigoTraslado) {
    let frm = new FormData();
    frm.set("CodigoTraslado", codigoTraslado);
    fetchPost("Especiales2/AceptarImportacion", "text", frm, function (data) {
        if (data == "OK") {
            MostrarTrasladosParaImportacion();
        } else {
            MensajeError(data);
        }
    });
}


function DepurarNombresClientesEspeciales2(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-depurar', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        MostrarDetalleEspeciales2ParaDepuracion(codigoTraslado);
    });
}

function MostrarDetalleEspeciales2ParaDepuracion(codigoTraslado) {
    let objConfiguracion = {
        url: "TrasladosEspeciales2/GetTrasladosParaDepuracion/?codigoTraslado=" + codigoTraslado,
        cabeceras: ["serie", "numeroPedido", "codigoEmpresa", "codigoCliente", "nombreCliente", "codigoEntidad", "nombreEntidad", "monto", "modificacion", "codigoEstadoDepuracion", "ratioSimilitud"],
        propiedades: ["serie", "numeroPedido", "codigoEmpresa", "codigoCliente", "nombreCliente", "codigoEntidad", "nombreEntidad", "monto", "modificacion", "codigoEstadoDepuracion", "ratioSimilitud"],
        displaydecimals: ["monto"],
        divContenedorTabla: "divContenedorTablaDetalle",
        divPintado: "divTablaDetalle",
        idtabla: "tablaDetalle",
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "visible": true
            }, {
                "targets": [1],
                "visible": true
            }, {
                "targets": [2],
                "visible": true
            }],
        slug: "serie"
    }
    pintar(objConfiguracion);
}


//function MostrarTransacciones(codigoTraslado) {
//    fetchGet("Especiales2/GetTrasladosParaImportacion/?codigoTraslado=" + codigoTraslado.toString(), "json", function (rpta) {
//        if (rpta != undefined && rpta != null && rpta.length != 0) {
         
//        } else {
//        }
//    })
//}





