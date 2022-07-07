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
            case "GeneracionTrasladosConta":
                MostrarTrasladosEspeciales2GeneradosConta();
                break;
            case "ImportEspeciales2":
                MostrarTrasladosParaImportacion();
                break;
            case "ConsultaTrasladosEspeciales2":
                MostrarTrasladosEspeciales2Consulta("");
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
        if (data == "GENERADO") {
            MostrarTrasladosEspeciales2Generados()
        } else {
            if (data == "VACIO") {
                Warning("No existen pedidos");
            } else {
                MensajeError(data);
            }
        }
    });
}

function GenerarTrasladosEspeciales2Conta() {
    let fechaStr = document.getElementById("uiFechaOperacion").value;
    if (fechaStr == "") {
        Warning("No existe fecha de operación");
        return;
    }

    let fechaOperacionStr = convertFormatDate(fechaStr);
    fetchGet("Especiales2/GenerarTrasladosDeEspeciales2/?fechaOperacion=" + fechaOperacionStr, "text", function (data) {
        if (data == "GENERADO") {
            MostrarTrasladosEspeciales2GeneradosConta()
        } else {
            if (data == "VACIO") {
                Warning("No existen pedidos");
            } else {
                MensajeError(data);
            }
        }
    });
}

function MostrarTrasladosEspeciales2Generados() {
    fetchGet("Especiales2/GetTrasladosEnProceso", "json", function (rpta) {
        if (rpta == undefined || rpta == null) {
            Warning("No existe informacion")
        } else {
            let codigoTraslado = -1;
            let observaciones = "";
            if (rpta.length > 0) {
                codigoTraslado = parseInt(rpta[0]["codigoTraslado"].toString());
                observaciones = rpta[0]["observacionesTraslado"].toString();
            }

            if (codigoTraslado == 0) {
                MensajeError(observaciones);
            } else {
                let objConfiguracion = {
                    cabeceras: ["Código Traslado", "Fecha Operación", "Pedidos", "Monto Total", "Fecha Generación", "Usuario Ingreso", "Fecha Traslado", "observaciones", "Codigo Estado", "Estado", "permisoAnular", "permisoTraslado", "permisoImprimir", "permisoEditar","permisoActualizar"],
                    propiedades: ["codigoTraslado", "fechaOperacionStr", "numeroPedidos", "montoTotal", "fechaIngresoStr", "usuarioIngreso", "fechaTrasladoStr", "observacionesTraslado", "codigoEstado", "estado", "permisoAnular", "permisoTraslado", "permisoImprimir", "permisoEditar","permisoActualizar"],
                    displaydecimals: ["montoTotal"],
                    divContenedorTabla: "divContenedorTabla",
                    divPintado: "divTabla",
                    aceptartraslado: true,
                    funcionaceptartraslado: "GeneracionEspeciales2",
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
                            "targets": [5],
                            "visible": false
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
                        }, {
                            "targets": [11],
                            "visible": false
                        }, {
                            "targets": [12],
                            "visible": false
                        }, {
                            "targets": [13],
                            "visible": false
                        }, {
                            "targets": [14],
                            "visible": false
                        }],
                    slug: "codigoTraslado",
                    editar: true,
                    funcioneditar: "DetalleTrasladoEspeciales2",
                    imprimir: true,
                    eliminar: true,
                    funcioneliminar: "TrasladoEspeciales2",
                    actualizar: true,
                    funcionactualizar: "DetallesEspeciales2Trasladados"
                }
                pintarEntidades(objConfiguracion, rpta);
            }
        }
    });
}

function AceptarTrasladoGeneracionEspeciales2(value) {
    let frm = new FormData();
    frm.set("CodigoTraslado", value.toString());
    frm.set("CodigoEstado", CODIGO_ESTADO_POR_RECEPCIONAR.toString());
    Confirmacion("Traslado Especiales 2", "¿Está seguro(a) del monto de especiales 2?", function (rpta) {
        fetchPost("Especiales2/CambiarEstadoTrasladoEspeciales2", "text", frm, function (data) {
            if (data == "OK") {
                Redireccionar("Especiales2", "GeneracionTraslados");
                //MostrarTrasladosEspeciales2Generados();
            } else {
                MensajeError(data);
            }
        });
    });
}


function EliminarTrasladoEspeciales2(value) {
    let frm = new FormData();
    frm.set("CodigoTraslado", value.toString());
    fetchPost("Especiales2/EliminarTrasladoEspeciales2", "text", frm, function (data) {
        if (data == "OK") {
            Redireccionar("Especiales2", "GeneracionTraslados");
            //MostrarTrasladosEspeciales2Generados();
        } else {
            MensajeError(data);
        }
    });
}

function ActualizarDetallesEspeciales2Trasladados(value) {
    let frm = new FormData();
    frm.set("CodigoTraslado", value.toString());
    Confirmacion("Traslado Especiales 2", "¿Está seguro(a) de actualizar el traslado?", function (rpta) {
        fetchPost("Especiales2/ActualizarDetallesTrasladados", "text", frm, function (data) {
            if (data == "OK") {
                Redireccionar("Especiales2", "GeneracionTraslados");
            } else {
                MensajeError(data);
            }
        });
    });
}


function EditarDetalleTrasladoEspeciales2(codigoTraslado, obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-editar', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        MostrarDetalleEspeciales2Edicion(codigoTraslado);
    });
}

function MostrarDetalleEspeciales2Edicion(codigoTraslado) {
    let objConfiguracion = {
        url: "Especiales2/GetDetalleTrasladosEspeciales2Edicion/?codigoTraslado=" + codigoTraslado,
        cabeceras: ["Empresa", "Serie", "Número Pedido", "Código Cliente", "Nombre Cliente", "Monto", "Fecha registro", "permisoAnular"],
        propiedades: ["codigoEmpresa","serie","numeroPedido", "codigoCliente", "nombreCliente", "monto", "fechaGrabadoStr","permisoAnular"],
        displaydecimals: ["monto"],
        divContenedorTabla: "divContenedorTablaDetalle",
        divPintado: "divTablaDetalle",
        idtabla: "tablaDetalle",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "visible": false
            }, {
                "targets": [2],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [3],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [5],
                "className": "dt-body-right",
                "visible": true
            }, {
                "targets": [7],
                "visible": false
            }],
        slug: "serie",
        eliminar: true,
        funcioneliminar: "DetalleTrasladoEspeciales2"
    }
    pintar(objConfiguracion);
}

function EliminarDetalleTrasladoEspeciales2() {
    let table = $('#tablaDetalle').DataTable();
    $('#tablaDetalle tbody').on('click', '.option-eliminar', function () {
        let rowIdx = table.row(this).index();
        let codigoEmpresa = table.cell(rowIdx, 0).data();
        let serie = table.cell(rowIdx, 1).data();
        let numeroPedido = table.cell(rowIdx, 2).data();
        let frm = new FormData();
        frm.set("CodigoEmpresa", codigoEmpresa);
        frm.set("Serie", serie);
        frm.set("NumeroPedido", numeroPedido);
        Confirmacion("Detalle Traslado Especiales 2", "¿Está seguro(a) de quitarlo del listado?", function (rpta) {
            fetchPost("Especiales2/EliminarDetalleTrasladoEspeciales2", "text", frm, function (data) {
                if (data == "OK") {
                    //table.clear().draw();
                    Redireccionar("Especiales2", "GeneracionTraslados");
                } else {
                    MensajeError(data);
                }
            });
        });
    });
}




function Imprimir(codigoTraslado, obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-imprimir', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        let fechaOperacionStr = table.cell(rowIdx, 1).data();
        let fechaGeneracionStr = table.cell(rowIdx, 4).data();
        fetchGet("Especiales2/GetDetalleTrasladosEspeciales2/?codigoTraslado=" + codigoTraslado, "json", function (rpta) {
            let jsonData = JSON.stringify(rpta);
            fetchPostJson("Especiales2/PrintAPI/?codigoTraslado=" + codigoTraslado + "&fechaOperacionStr=" + fechaOperacionStr + "&fechaGeneracionStr=" + fechaGeneracionStr, "text", jsonData, function (data) {

            })
        });
    });
}

function MostrarTrasladosParaImportacion() {
    fetchGet("Especiales2/GetTrasladosParaImportacion", "json", function (rpta) {
        if (rpta == undefined || rpta == null) {
            Warning("No existe informacion")
        } else {
            let codigoTraslado = -1;
            let observaciones = "";
            if (rpta.length > 0) {
                codigoTraslado = parseInt(rpta[0]["codigoTraslado"].toString());
                observaciones = rpta[0]["observacionesTraslado"].toString();
            }

            if (codigoTraslado == 0) {
                MensajeError(observaciones);
            } else {
                let objConfiguracion = {
                    cabeceras: ["Código Traslado", "Fecha Operación", "Pedidos", "Monto Total", "observaciones", "Fecha Generación", "Generado por", "Fecha Traslado", "Codigo Estado", "Estado", "permisoImportar", "permisoDepurar", "permisoRegistrar", "permisoEditar"],
                    propiedades: ["codigoTraslado", "fechaOperacionStr", "numeroPedidos", "montoTotal", "observacionesTraslado", "fechaIngresoStr", "usuarioIngreso", "fechaTrasladoStr", "codigoEstado", "estado", "permisoImportar", "permisoDepurar", "permisoRegistrar", "permisoEditar"],
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
                        }, {
                            "targets": [13],
                            "visible": false
                        }],
                    slug: "codigoTraslado",
                    import: true,
                    funcionimport: "Especiales2",
                    depurar: true,
                    funciondepurar: "NombresClientesEspeciales2",
                    registrar: true,
                    funcionregistrar: "Especiales2",
                    editar: true,
                    funcioneditar: "NombresClientesEspeciales2"

                }
                pintarEntidades(objConfiguracion, rpta);
            }
        }
    });
}

function ImportarEspeciales2() {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click','.option-import', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        let fechaOperacionStr = table.cell(rowIdx, 1).data();
        fetchGet("Especiales2/GetDetalleTrasladosEspeciales2/?codigoTraslado=" + codigoTraslado, "json", function (rpta) {
            if (rpta != undefined && rpta != null && rpta.length != 0) {
                let codigoTraslado = parseInt(rpta[0]["codigoTraslado"].toString());
                let observaciones = rpta[0]["nombreClienteDepurado"].toString();
                if (codigoTraslado == 0) {
                    MensajeError(observaciones);
                } else {
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
                }
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

function EditarNombresClientesEspeciales2(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-editar', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        MostrarDetalleEspeciales2(codigoTraslado);
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
        let frm = new FormData();
        frm.set("CodigoTraslado", codigoTraslado);
        frm.set("FechaOperacionStr", fechaOperacion);
        frm.set("SemanaOperacion", semanaOperacion.toString());
        frm.set("AnioOperacion", anioOperacion.toString());
        fetchPost("TrasladosEspeciales2/RegistrarEspeciales2", "text", frm, function (data) {
            if (data == "OK") {
                let frm1 = new FormData();
                frm1.set("CodigoTraslado", codigoTraslado);
                frm1.set("CodigoEstado", CODIGO_ESTADO_COMPLETADO.toString());
                fetchPost("Especiales2/CambiarEstadoTrasladoEspeciales2", "text", frm1, function (data2) {
                    if (data2 == "OK") {
                        Redireccionar("Especiales2", "ImportEspeciales2");
                        //MostrarTrasladosParaImportacion();
                    } else {
                        MensajeError(data2);
                    }
                });
            } else {
                MensajeError(data);
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
                            Redireccionar("Especiales2", "ImportEspeciales2");
                            //MostrarTrasladosParaImportacion();
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



function MostrarDetalleEspeciales2(codigoTraslado) {
    let objConfiguracion = {
        url: "TrasladosEspeciales2/GetDetalleEspeciales2/?codigoTraslado=" + codigoTraslado,
        cabeceras: ["codigoCliente","nombreCliente","codigoEntidad", "nombreEntidad", "monto","Fecha registro", "codigoEstadoDepuracion","Estado"],
        propiedades: ["codigoCliente", "nombreCliente","codigoEntidad", "nombreEntidad", "monto","fechaGrabadoStr", "codigoEstadoDepuracion","estadoDepuracion"],
        displaydecimals: ["monto"],
        divContenedorTabla: "divContenedorTablaDetalle",
        divPintado: "divTablaDetalle",
        idtabla: "tablaDetalle",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [4],
                "className": "dt-body-right",
                "visible": true
            }, {
                "targets": [6],
                "visible": false
            }],
        slug: "serie"
    }
    pintar(objConfiguracion);
}


function BuscarTrasladosEspeciales2() {
    let fechaStr = document.getElementById("uiFechaOperacion").value;
    if (fechaStr == "") {
        Warning("No existe fecha de operación");
        return;
    }

    let fechaOperacionStr = convertFormatDate(fechaStr);
    MostrarTrasladosEspeciales2Consulta(fechaOperacionStr);
}

//url: "Especiales2/GetConsultaTraslados/?fechaOperacion=" + fechaOperacion,
function MostrarTrasladosEspeciales2Consulta(fechaOperacion) {
    if (fechaOperacion == "") {
        return;
    }

    fetchGet("Especiales2/GetConsultaTraslados/?fechaOperacion=" + fechaOperacion, "json", function (rpta) {
        if (rpta == undefined || rpta == null || rpta.length == 0) {
            Warning("No existe informacion")
        } else {
            let codigoTraslado = parseInt(rpta[0]["codigoTraslado"].toString());
            let observaciones = rpta[0]["observacionesTraslado"].toString();
            if (codigoTraslado == 0) {
                MensajeError(observaciones);
            } else {
                let objConfiguracion = {
                    cabeceras: ["Código Traslado", "Fecha Operación", "Pedidos", "Monto Total", "Fecha Generación", "Usuario Ingreso", "Fecha Traslado", "observaciones", "Codigo Estado", "Estado", "permisoImprimir"],
                    propiedades: ["codigoTraslado", "fechaOperacionStr", "numeroPedidos", "montoTotal", "fechaIngresoStr", "usuarioIngreso", "fechaTrasladoStr", "observacionesTraslado", "codigoEstado", "estado", "permisoImprimir"],
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
                pintarEntidades(objConfiguracion, rpta);
                //pintar(objConfiguracion);
            }
        }
    });
}

function MostrarTrasladosEspeciales2GeneradosConta() {
    fetchGet("Especiales2/GetTrasladosEnProcesoContabilidad", "json", function (rpta) {
        if (rpta == undefined || rpta == null) {
            Warning("No existe informacion")
        } else {
            let codigoTraslado = -1;
            let observaciones = "";
            if (rpta.length > 0) {
                codigoTraslado = parseInt(rpta[0]["codigoTraslado"].toString());
                observaciones = rpta[0]["observacionesTraslado"].toString();
            }

            if (codigoTraslado == 0) {
                MensajeError(observaciones);
            } else {
                let objConfiguracion = {
                    cabeceras: ["Código Traslado", "Fecha Operación", "Pedidos", "Monto Total", "Fecha Generación", "Usuario Ingreso", "Fecha Traslado", "observaciones", "Codigo Estado", "Estado", "permisoAnular", "permisoTraslado", "permisoImprimir", "permisoEditar", "permisoActualizar"],
                    propiedades: ["codigoTraslado", "fechaOperacionStr", "numeroPedidos", "montoTotal", "fechaIngresoStr", "usuarioIngreso", "fechaTrasladoStr", "observacionesTraslado", "codigoEstado", "estado", "permisoAnular", "permisoTraslado", "permisoImprimir", "permisoEditar", "permisoActualizar"],
                    displaydecimals: ["montoTotal"],
                    divContenedorTabla: "divContenedorTabla",
                    divPintado: "divTabla",
                    aceptartraslado: true,
                    funcionaceptartraslado: "GeneracionEspeciales2Conta",
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
                            "targets": [5],
                            "visible": false
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
                        }, {
                            "targets": [11],
                            "visible": false
                        }, {
                            "targets": [12],
                            "visible": false
                        }, {
                            "targets": [13],
                            "visible": false
                        }, {
                            "targets": [14],
                            "visible": false
                        }],
                    slug: "codigoTraslado",
                    eliminar: true,
                    funcioneliminar: "TrasladoEspeciales2Conta"
                }
                pintarEntidades(objConfiguracion, rpta);
            }
        }
    });
}

function AceptarTrasladoGeneracionEspeciales2Conta(value) {
    let frm = new FormData();
    frm.set("CodigoTraslado", value.toString());
    frm.set("CodigoEstado", CODIGO_ESTADO_POR_RECEPCIONAR.toString());
    Confirmacion("Traslado Especiales 2", "¿Está seguro(a) del monto de especiales 2?", function (rpta) {
        fetchPost("Especiales2/CambiarEstadoTrasladoEspeciales2", "text", frm, function (data) {
            if (data == "OK") {
                Redireccionar("Especiales2", "GeneracionTrasladosConta");
            } else {
                MensajeError(data);
            }
        });
    });
}


function EliminarTrasladoEspeciales2Conta(value) {
    let frm = new FormData();
    frm.set("CodigoTraslado", value.toString());
    fetchPost("Especiales2/EliminarTrasladoEspeciales2", "text", frm, function (data) {
        if (data == "OK") {
            Redireccionar("Especiales2", "GeneracionTrasladosConta");
        } else {
            MensajeError(data);
        }
    });
}