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

function GenerarTrasladosEspeciales2() {
    let fechaStr = document.getElementById("uiFechaOperacion").value;
    if (fechaStr == "") {
        Warning("No existe fecha de operación");
        return;
    }

    let fechaOperacionStr = convertFormatDate(fechaStr);
    fetchGet("Especiales2/GenerarTrasladosDeEspeciales2/?fechaOperacion=" + fechaOperacionStr, "text", function (data) {
        if (data == "OK") {
            MostrarTrasladosEspeciales2Generados()
        } else {
            MensajeError(data);
        }
    });
}

function MostrarTrasladosEspeciales2Generados() {
    let objConfiguracion = {
        url: "Especiales2/GetTrasladosEnProceso",
        cabeceras: ["Código Traslado", "Fecha Operación", "Pedidos", "Monto Total", "Fecha Generación", "Usuario Ingreso", "Fecha Traslado", "observaciones", "Codigo Estado", "Estado", "permisoImprimir"],
        propiedades: ["codigoTraslado", "fechaOperacionStr", "numeroPedidos", "montoTotal", "fechaIngresoStr", "usuarioIngreso", "fechaTrasladoStr", "observacionesTraslado", "codigoEstado", "estado", "permisoImprimir"],
        displaydecimals: ["montoTotal"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        aceptar: true,
        funcionaceptar: "AceptarGeneracionTrasladoEspeciales2",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [1],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [2],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [3],
                "className": "dt-body-right",
                "visible": true
            }, {
                "targets": [6],
                "visible": false
            }, {
                "targets": [7],
                "visible": false
            }, {
                "targets": [8],
                "visible": false
            }, {
                "targets": [10],
                "visible": false
            }],
        slug: "codigoTraslado",
        imprimir: true
    }
    pintar(objConfiguracion);
}

function clickAceptarGeneracionTrasladoEspeciales2(value) {
    let frm = new FormData();
    frm.set("CodigoTraslado", value.toString());
    frm.set("CodigoEstado", CODIGO_ESTADO_POR_RECEPCIONAR.toString());
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Especiales2/CambiarEstadoTrasladoEspeciales2", "text", frm, function (data) {
            if (data == "OK") {
                MostrarTrasladosEspeciales2Generados();
            } else {
                MensajeError(data);
            }
        });
    });
}


function Imprimir(codigoTraslado, obj) {
    fetchGet("Especiales2/GetDetalleTrasladosEspeciales2/?codigoTraslado=" + codigoTraslado, "json", function (rpta) {
        let jsonData = JSON.stringify(rpta);
        fetchPostJson("Especiales2/PrintAPI", "text", jsonData, function (data) {

        })
    });
}

function MostrarTrasladosParaImportacion() {
    let objConfiguracion = {
        url: "Especiales2/GetTrasladosParaImportacion",
        cabeceras: ["Código Traslado", "Fecha Operación", "Pedidos", "Monto Total", "observaciones", "Fecha Generación", "Generado por", "Fecha Traslado", "Codigo Estado", "Estado","permisoImportar","permisoDepurar","permisoRegistrar"],
        propiedades: ["codigoTraslado", "fechaOperacionStr", "numeroPedidos", "montoTotal", "observacionesTraslado", "fechaIngresoStr", "usuarioIngreso", "fechaTrasladoStr", "codigoEstado", "estado","permisoImportar","permisoDepurar","permisoRegistrar"],
        displaydecimals: ["montoTotal"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [1],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [2],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [3],
                "className": "dt-body-right",
                "visible": true
            }, {
                "targets": [4],
                "visible": false
            }, {
                "targets": [7],
                "visible": false
            }, {
                "targets": [8],
                "visible": false
            }, {
                "targets": [10],
                "visible": false
            }, {
                "targets": [11],
                "visible": false
            }, {
                "targets": [12],
                "visible": false
            }],
        slug: "codigoTraslado",
        import: true,
        funcionimport: "Especiales2",
        depurar: true,
        funciondepurar: "NombresClientesEspeciales2",
        registrar: true,
        funcionregistrar: "Especiales2"

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
                        let frm = new FormData();
                        frm.set("CodigoTraslado", codigoTraslado);
                        frm.set("CodigoEstado", CODIGO_ESTADO_RECEPCIONADO.toString());
                        fetchPost("Especiales2/CambiarEstadoTrasladoEspeciales2", "text", frm, function (data2) {
                            if (data2 == "OK") {
                                MostrarTrasladosParaImportacion();
                            } else {
                                MensajeError(data2);
                            }
                        });
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

function DepurarNombresClientesEspeciales2(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-depurar', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        MostrarDetalleEspeciales2ParaDepuracion(codigoTraslado);
    });
}

function RegistrarEspeciales2(obj) {
    let semanaOperacion = parseInt(document.getElementById("uiNumeroSemanaActualSistema").value);
    let anioOperacion = parseInt(document.getElementById("uiAnioSemanaActualSistema").value);
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-registrar', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        let fechaOperacion = table.cell(rowIdx, 1).data();
        fetchGet("TrasladosEspeciales2/GetDetalleEspeciales2/?codigoTraslado=" + codigoTraslado, "json", function (rpta) {
            if (rpta != undefined && rpta != null && rpta.length != 0) {
                let jsonData = JSON.stringify(rpta);
                fetchPostJson("TrasladosEspeciales2/RegistrarEspeciales2/?codigoTraslado=" + codigoTraslado + "&fechaOperacion=" + fechaOperacion + "&semanaOperacion=" + semanaOperacion.toString() + "&anioOperacion=" + anioOperacion.toString(), "text", jsonData, function (data) {

                });
            }
        });
    });
}

function MostrarDetalleEspeciales2ParaDepuracion(codigoTraslado) {
    fetchGet("TrasladosEspeciales2/GetTrasladosParaDepuracion/?codigoTraslado=" + codigoTraslado, "json", function (rpta) {
        if (rpta != undefined && rpta != null && rpta.length != 0) {
            let jsonData = JSON.stringify(rpta);
            fetchPostJson("TrasladosEspeciales2/ActualizarDetalleEspeciales2/?codigoTraslado=" + codigoTraslado, "text", jsonData, function (data) {
                if (data == "OK") {
                    let frm = new FormData();
                    frm.set("CodigoTraslado", codigoTraslado);
                    frm.set("CodigoEstado", CODIGO_ESTADO_DEPURADO.toString());
                    fetchPost("Especiales2/CambiarEstadoTrasladoEspeciales2", "text", frm, function (data) {
                        if (data == "OK") {
                            MostrarTrasladosParaImportacion();
                            //MostrarDetalleEspeciales2Depurado(codigoTraslado);
                        } else {
                            MensajeError(data);
                        }
                    });
                } else {
                    MensajeError(data);
                }
            });
        }
    });
}




function MostrarDetalleEspeciales2Depurado(codigoTraslado) {
    let objConfiguracion = {
        url: "TrasladosEspeciales2/GetDetalleEspeciales2/?codigoTraslado=" + codigoTraslado,
        cabeceras: ["codigoEntidad", "nombreEntidad", "monto", "codigoEstadoDepuracion"],
        propiedades: ["codigoEntidad", "nombreEntidad", "monto", "codigoEstadoDepuracion"],
        displaydecimals: ["monto"],
        divContenedorTabla: "divContenedorTablaDetalle",
        divPintado: "divTablaDetalle",
        idtabla: "tablaDetalle",
        paginar: true,
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
