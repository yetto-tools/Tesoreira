window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    if (nameController == "CorteSemanalCajaChica") {
        let codigoReporte = url1.searchParams.get("codigoReporte");
        let codigoCajaChica = url1.searchParams.get("codigoCajaChica");
        let anioOperacion = url1.searchParams.get("anioOperacion");
        let semanaOperacion = url1.searchParams.get("semanaOperacion");
        switch (nameAction) {
            case "MostrarCortesSemanalesGeneracion":
                listarReportesSemanalCajaChicaGeneracion();
                break;
            case "MostrarCortesSemanalesRevision":
                ListarReportesSemanalCajaChicaRevision();
                break;
            case "MostrarCortesSemanalesConsulta":
                fillComboCajaChica();
                ListarReportesSemanalCajaChicaConsulta(0,0);
                break;
            case "MostrarDetalleRevision":
                ListarDetalleCajaChicaParaRevision(codigoReporte);
                break;
            case "MostrarDetalle":
                ListarDetalleCajaChica(codigoReporte, codigoCajaChica, anioOperacion, semanaOperacion);
                break;
            case "MostrarDetalleConsulta":
                document.getElementById("uiExportarExcel").href = "/CorteSemanalCajaChica/ExportarExcel/?codigoReporte=" + codigoReporte + "&codigoCajaChica=" + codigoCajaChica + "&anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion;
                set("uiCodigoReporte", codigoReporte);
                set("uiCodigoCajaChica", codigoCajaChica);
                set("uiAnioOperacion", anioOperacion);
                set("uiSemanaOperacion", semanaOperacion);
                ListarDetalleCajaChicaConsulta(codigoReporte, codigoCajaChica, anioOperacion, semanaOperacion);
                break;
            case "RecepcionReembolso":
                ListarReembolsosDeCajaChica();
                break;
            case "RecepcionReembolsoContabilidad":
                fillAnioRecepcionReembolso();
                ListarReembolsosDeCajaChicaContabilidad();
                break;
            default:
                
                break;
        }// fin switch
    }// fin if
}

function fillAnioRecepcionReembolso() {
    let arrayDate = getFechaSistema();
    let numeroMes = arrayDate[0]["mes"];
    let anioActual = arrayDate[0]["anio"];
    let anioAnterior = anioActual - 1;
    let select = document.getElementById("uiFiltroAnioOperacion");
    if (numeroMes == 1) {
    let data = [{ "value": anioActual.toString(), "text": anioActual.toString() }, { "value": anioAnterior.toString(), "text": anioAnterior.toString() }]
        FillCombo(data, "uiFiltroAnioOperacion", "value", "text", "- seleccione -", "-1");
        select.selectedIndex = 1;
        fillSemanasRecepcionReembolo(anioActual);

    } else {
        let data = [{ "value": anioActual.toString(), "text": anioActual.toString() }]
        FillCombo(data, "uiFiltroAnioOperacion", "value", "text", "- seleccione -", "-1");
        select.selectedIndex = 0;
        fillSemanasRecepcionReembolo(anioActual);
    }
}

function fillSemanasRecepcionReembolo(obj) {
    let arrayDate = getFechaSistema();
    let anioActual = arrayDate[0]["anio"];
    let numeroSemanaActual = parseInt(document.getElementById("uiNumeroSemanaActualSistema").value);
    let anioReporte = parseInt(obj);
    if (anioReporte != anioActual) {
        numeroSemanaActual = 53;
    }
    fetchGet("ProgramacionSemanal/GetSemanasAnteriores/?anio=" + anioReporte.toString() + "&numeroSemanaActual=" + numeroSemanaActual.toString() + "&numeroSemanas=3&incluirSemanaActual=1", "json", function (rpta) {
        if (rpta != undefined && rpta != null && rpta.length != 0) {
            FillCombo(rpta, "uiFiltroSemana", "numeroSemana", "periodo", "- seleccione -", "-1");
        } else {
            FillComboUnicaOpcion("uiFiltroSemana", "-1", "-- No Existen Fechas -- ");
        }
    })
}

function fillComboCajaChica() {
    fetchGet("CajaChica/GetCajasChicas", "json", function (rpta) {
        FillCombo(rpta, "uiFiltroCajaChica", "codigoCajaChica", "nombreCajaChica", "- seleccione -", "-1");
    })
}


function fillComboBanco(control) {
    fetchGet("Banco/GetAllBancos", "json", function (rpta) {
        FillCombo(rpta, control, "codigoBanco", "nombre", "- seleccione -", "-1");
    })
}


function listarReportesSemanalCajaChicaGeneracion() {
    let objConfiguracion = {
        url: "CorteSemanalCajaChica/ListarReportesCajaChica",
        cabeceras: ["Código Reporte", "codigoCajaChica", "Caja Chica", "Año", "Semana", "Saldo Inicial", "Monto F", "Monto NF", "Monto Egreso","Saldo","Estado","bloqueado","Anular","Editar"],
        propiedades: ["codigoReporte", "codigoCajaChica", "nombreCajaChica", "anioOperacion", "semanaOperacion", "montoDisponible", "montoFiscal", "montoNoFiscal", "montoEgreso","montoSaldo", "estado","bloqueado","permisoAnular","permisoEditar"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        generar: true,
        eliminar: true,
        funcioneliminar: "ReporteGenerado",
        paginar: true,
        verdetalle: true,
        displaydecimals: ["montoDisponible", "montoFiscal", "montoNoFiscal", "montoEgreso","montoSaldo"],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "className": "dt-body-center"
            }, {
                "targets": [1],
                "visible": false
            }, {
                "targets": [3],
                "className": "dt-body-center"
            }, {
                "targets": [4],
                "className": "dt-body-center"
            }, {
                "targets": [5],
                "className": "dt-body-right"
            }, {
                "targets": [6],
                "className": "dt-body-right"
            }, {
                "targets": [7],
                "className": "dt-body-right"
            }, {
                "targets": [8],
                "className": "dt-body-right"
            }, {
                "targets": [9],
                "className": "dt-body-right"
            }, {
                "targets": [11],
                "visible": false
            }, {
                "targets": [12],
                "visible": false
            }, {
                "targets": [13],
                "visible": true
            }],
        slug: "codigoReporte",
        aceptar: true,
        funcionaceptar: "TrasladarReporteParaRevision"
    }
    pintar(objConfiguracion);
}

function GenerarReporte(slug) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', 'tr', 'input:button', function () {
        let rowIdx = table.row(this).index();
        let codigoCajaChica = table.cell(rowIdx, 1).data();
        let anioOperacion = table.cell(rowIdx, 3).data();
        let semanaOperacion = table.cell(rowIdx, 4).data();
        fetchGet("CorteSemanalCajaChica/GenerarReporteSemanal/?codigoCajaChica=" + codigoCajaChica.toString() + "&anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString(), "text", function (rpta) {
            if (rpta != "0") {
                MensajeError("Error al generar el reporte semanal de Caja Chica");
                return;
            } else {
                listarReportesSemanalCajaChicaGeneracion();
            }
        })
    });
}


function EliminarReporteGenerado(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-eliminar', function () {
        let rowIdx = table.row(this).index();
        let codigoReporte = table.cell(rowIdx, 0).data();
        Confirmacion("Anular Reporte", "¿Seguro de Anular el Reporte Generado?", function (rpta) {
            fetchGet("CorteSemanalCajaChica/AnularReporteGenerado/?codigoReporte=" + codigoReporte, "text", function (data) {
                if (data != "OK") {
                    MensajeError(data);
                    return;
                } else {
                    listarReportesSemanalCajaChicaGeneracion();
                }
            });
        });
    });
}

function clickTrasladarReporteParaRevision(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-aceptar', function () {
        let rowIdx = table.row(this).index();
        let codigoReporte = table.cell(rowIdx, 0).data();
        let codigoCajaChica = table.cell(rowIdx, 1).data();
        let montoGastosFiscales = table.cell(rowIdx, 6).data();
        Confirmacion("Revisión de Pagos de Caja Chica", "¿Desea trasladar los Pagos de Caja Chica para revisión?", function (rpta) {
            fetchGet("CorteSemanalCajaChica/TrasladarReporteParaRevision/?codigoReporte=" + codigoReporte + "&codigoCajaChica=" + codigoCajaChica + "&montoGastosFiscales=" + montoGastosFiscales, "text", function (data) {
                if (data != "OK") {
                    MensajeError(data);
                    return;
                } else {
                    listarReportesSemanalCajaChicaGeneracion();
                }
            });
        });
    });
}


function ListarReportesSemanalCajaChicaRevision() {
    let objConfiguracion = {
        url: "CorteSemanalCajaChica/ListarReportesCajaChicaParaRevision",
        cabeceras: ["Código Reporte", "codigoCajaChica", "Caja Chica", "Año", "Semana","Periodo", "Monto", "Monto (F)", "Monto (NF)","Fecha Corte", "Estado", "Bloqueado","Corregir"],
        propiedades: ["codigoReporte", "codigoCajaChica", "nombreCajaChica", "anioOperacion", "semanaOperacion", "periodo", "monto", "montoFiscal", "montoNoFiscal","fechaCorteStr", "estado", "bloqueado","permisoCorregir"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        displaydecimals: ["monto", "montoFiscal", "montoNoFiscal"],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "visible": true,
                "className": "dt-body-center"
            }, {
                "targets": [1],
                "visible": false
            }, {
                "targets": [3],
                "visible": true,
                "className": "dt-body-center"
            }, {
                "targets": [5],
                "visible": true,
                "className": "dt-body-center"
            }, {
                "targets": [6],
                "className": "dt-body-right"
            }, {
                "targets": [7],
                "className": "dt-body-right"
            }, {
                "targets": [8],
                "className": "dt-body-right"
            }, {
                "targets": [11],
                "visible": false
            }, {
                "targets": [12],
                "visible": false
            }],
        slug: "codigoReporte",
        revision: true,
        funcionrevision: "RevisarReporte",
        nombrecolumnarevision: "Revisar",
        aceptar: true,
        funcionaceptar: "FinalizarRevision"
    }
    pintar(objConfiguracion);
}

function clickFinalizarRevision(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-aceptar', function () {
        let rowIdx = table.row(this).index();
        let codigoReporte = table.cell(rowIdx, 0).data();
        let codigoCajaChica = table.cell(rowIdx, 1).data();
        let nombreCajaChica = table.cell(rowIdx, 2).data();
        let anioOperacion = table.cell(rowIdx, 3).data();
        let semanaOperacion = table.cell(rowIdx, 4).data();
        let montoReintegro = table.cell(rowIdx, 7).data(); // Monto Fiscal
        setI("uiTitlePopupReintegro", "Reintegro a Caja Chica");
        set("uiMontoReintegroCalculado", montoReintegro);
        set("uiCodigoReporte", codigoReporte);
        set("uiCodigoCajaChica", codigoCajaChica);
        set("uiNombreCajaChica", nombreCajaChica);
        set("uiAnioOperacion", anioOperacion);
        set("uiSemanaOperacion", semanaOperacion);
        set("uiMontoReintegro", montoReintegro);
        set("uiObservaciones", "");
        document.getElementById("ShowPopupReintegro").click();
    });

}

function RegistrarReintegro() {
    let errores = ValidarDatos("frmReintegro")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmReintegro");
    let frm = new FormData(frmGuardar);
    let codigoReporte = frm.get("CodigoReporte");
    let montoReintegro = parseFloat(frm.get("MontoReintegro"));
    let montoReintegroStr = frm.get("MontoReintegro");
    let montoReintegroCalculadoStr = frm.get("MontoReintegroCalculado");

    Confirmacion("Reintegro a Caja Chica", "¿Está seguro del monto a reintegrar?", function (rpta) {
        fetchGet("CorteSemanalCajaChica/FinalizarRevision/?codigoReporte=" + codigoReporte + "&montoReintegroCalculado=" + montoReintegroCalculadoStr + "&montoReintegro=" + montoReintegroStr, "text", function (data) {
            document.getElementById("uiClosePopupReintegro").click();
            if (data != "0") {
                MensajeError(data);
            } else {
                ListarReportesSemanalCajaChicaRevision();
            }
        })
    })

}



function RevisarReporte(obj) {
    Redireccionar("CorteSemanalCajaChica", "MostrarDetalleRevision/?codigoReporte=" + obj.toString());
}

function ListarDetalleCajaChicaParaRevision(obj) {
    let objConfiguracion = {
        url: "CajaChica/ListarTransaccionesCajaChicaParaRevision/?codigoReporte=" + obj.toString(),
        cabeceras: ["Código Reporte", "Código Transacción", "codigoTransaccionAnt", "correccion","Caja Chica", "nit", "proveedor", "fecha factura", "serie factura", "número factura", "monto", "Descripción", "Código Operacion", "Operacion", "excluir","permisoEditar","permisoCorregir"],
        propiedades: ["codigoReporte", "codigoTransaccion", "codigoTransaccionAnt", "correccion","nombreCajaChica", "nitProveedor", "nombreProveedor", "fechaDocumento", "serieFactura", "numeroDocumento", "monto", "descripcion", "codigoOperacion", "operacion", "excluirFactura","permisoEditar","permisoCorregir"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        revision: true,
        funcionrevision: "SolicitaAprobacionCorreccion",
        nombrecolumnarevision: "Solicitar",
        alerta: true,
        funcionalerta: "CorreccionTransaccion",
        editar: true,
        funcioneditar: "TransaccionParaCorreccion",
        excluir: true,
        fieldNameExcluir: "excluirFactura",
        ocultarColumnas: true,
        datesWithoutTime: ["fechaDocumento"],
        displaydecimals: ["monto"],
        paginar: true,
        hideColumns: [
            {
                "targets": [0],
                "visible": true,
                "className": "dt-body-center"
            }, {
                "targets": [2],
                "visible": false
            }, {
                "targets": [3],
                "visible": false
            }, {
                "targets": [10],
                "className": "dt-body-right"
            }, {
                "targets": [12],
                "visible": false
            }, {
                "targets": [14],
                "visible": false
            }, {
                "targets": [15],
                "visible": false
                
            }, {
                "targets": [16],
                "visible": false
            }, {
                "targets": [17],
                "visible": true,
                "className": "dt-body-center"
            }],
        slug: "codigoTransaccion",
    }
    pintar(objConfiguracion);
}


function EditarTransaccionParaCorreccion(obj) {
    Redireccionar("CajaChica", "EditCorreccion/?codigoTransaccion=" + obj.toString());
}

function Excluir(obj) {
    let checked = obj.checked;
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-check', function () {
        let rowIdx = table.row(this).index();
        let codigoReporte = table.cell(rowIdx, 0).data();
        let codigoTransaccion = table.cell(rowIdx, 1).data();
        let nitProveedor = table.cell(rowIdx, 5).data();
        let nombreProvedor = table.cell(rowIdx, 6).data();
        let monto = parseFloat(table.cell(rowIdx, 10).data()).toFixed(2);
        let descripcion = table.cell(rowIdx, 11).data();
        let excluirFactura = table.cell(rowIdx, 14).data();
        if (checked == true) { // Excluir Factura
            obj.checked = false;
            setI("uiTitlePopupExclusion", "Exclusión de Factura");
            set("uiCodigoReporte", codigoReporte.toString());
            set("uiCodigoTransaccion", codigoTransaccion.toString());
            set("uiNitProveedor", nitProveedor);
            set("uiNombreProveedor", nombreProvedor);
            set("uiMonto", monto.toString());
            set("uiDescripcion", descripcion);
            fillCombosMotivosExclusion();
            set("uiObservaciones", "");
            document.getElementById("ShowPopupExclusion").click();
        } else { // No excluir factura
            if (excluirFactura == 1) {
                let codigoMotivoExclusion = -1;
                let observaciones = "";
                fetchGet("CajaChica/ExcluirFacturaDeCajaChica/?codigoTransaccion=" + codigoTransaccion.toString() + "&excluirFactura=0&codigoMotivoExclusion=" + codigoMotivoExclusion.toString() + "&observaciones=" + observaciones, "text", function (rpta) {
                    if (rpta != "OK") {
                        MensajeError(rpta);
                        return;
                    } else {
                        RevisarReporte(codigoReporte);
                    }
                })
            }
        }// fin else
    });
}

function ExcluirFactura() {
    let errores = ValidarDatos("frmExclusion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmExclusion");
    let frm = new FormData(frmGuardar);
    let codigoReporte = frm.get("CodigoReporte");
    let codigoTransaccion = frm.get("CodigoTransaccion");
    let codigoMotivoExclusion = frm.get("CodigoMotivoExclusion");
    let observaciones = frm.get("Observaciones");
    fetchGet("CajaChica/ExcluirFacturaDeCajaChica/?codigoTransaccion=" + codigoTransaccion.toString() + "&excluirFactura=1&codigoMotivoExclusion=" + codigoMotivoExclusion.toString() + "&observaciones=" + observaciones, "text", function (rpta) {
        if (rpta != "OK") {
            MensajeError(rpta);
            return;
        } else {
            document.getElementById("uiClosePopupExclusion").click();
            RevisarReporte(codigoReporte);
        }
    })
}


function SolicitaAprobacionCorreccion(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-revision', function () {
        let rowIdx = table.row(this).index();
        let codigoReporte = table.cell(rowIdx, 0).data();
        let codigoTransaccion = table.cell(rowIdx, 1).data();
        let nitProveedor = table.cell(rowIdx, 5).data();
        let nombreProveedor = table.cell(rowIdx, 6).data();
        let fechaDocumento = table.cell(rowIdx, 7).data();
        let serieFactura = table.cell(rowIdx, 8).data();
        let numeroDocumento = table.cell(rowIdx, 9).data();
        let monto = table.cell(rowIdx, 10).data();
        let descripcion = table.cell(rowIdx, 11).data();
        
        setI("uiTitlePopupSolicitudAprobacion", "Solicitud de Aprobación de Corrección");
        document.getElementById("ShowPopupSolicitudAprobacion").click();
        set("uiCodigoReporteRevision", codigoReporte);
        set("uiCodigoTransaccionRevision", codigoTransaccion);
        set("uiNitProveedorRevision", nitProveedor);
        set("uiNombreProveedorRevision", nombreProveedor);
        set("uiFechaDocumentoRevision", fechaDocumento);
        set("uiSerieFacturaRevision", serieFactura);
        set("uiNumeroDocumentoRevision", numeroDocumento);
        set("uiMontoRevision", monto);
        set("uiDescripcionRevision", descripcion);
        set("uiObservacionesRevision", "");
        document.getElementById("uiCodigoTipoCorreccion").value = "-1";
    });
}

function RegistrarSolicitudCorreccion() {
    let errores = ValidarDatos("frmNewSolicitudAprobacion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmNewSolicitudAprobacion");
    let frm = new FormData(frmGuardar);
    let codigoReporte = parseInt(frm.get("CodigoReporte"));
    Confirmacion("Solicitud de Aprobación de Corrección", "¿Está seguro de realizar esta solicitud?", function (rpta) {
        fetchPost("CajaChica/RegistrarSolicitudDeCorreccion", "text", frm, function (data) {
            if (data == "OK") {
                document.getElementById("uiClosePopupSolicitudAprobacion").click();
                ListarDetalleCajaChicaParaRevision(codigoReporte);
            } else {
                MensajeError(data);
            }
        });
    });
}


function clickAlertaCorreccionTransaccion(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-alerta', function () {
        let rowIdx = table.row(this).index();
        let codigoTransaccion = BigInt(table.cell(rowIdx, 1).data());
        let codigoTransaccionAnt = BigInt(table.cell(rowIdx, 2).data());
        let correccion = parseInt(table.cell(rowIdx, 3).data());
        if (correccion == 1)
        {
            if (codigoTransaccionAnt != 0) {
                document.getElementById("uiObservacionesEliminacion").innerHTML = "Esta transacción también es una actualización de la transacción " + codigoTransaccionAnt.toString();
                document.getElementById('uiObservacionesEliminacion').style.color = "red";
                document.getElementById("uiObservacionesEliminacion").style.fontSize = "large";
                document.getElementById("uiObservacionesEliminacion").style.fontWeight = 'bold';
            } else {
                document.getElementById("uiObservacionesEliminacion").innerHTML = "";
            }
            setI("uiTitlePopupSolicitudCorreccion", "Solicitud de Corrección");
            document.getElementById("ShowPopupSolicitudCorreccion").click();
            fetchGet("CajaChica/GetDataCorreccion/?codigoTransaccion=" + codigoTransaccion.toString(), "json", function (data) {
                set("uiSolicitudCodigoTransaccion", data.codigoTransaccion);
                set("uiSolicitudObservacionesSolicitud", data.observacionesSolicitud);
                set("uiSolicitudUsuarioSolicitud", data.usuarioIng);
                set("uiSolicitudFechaSolicitud", data.fechaIngStr);
                set("uiSolicitudResultado", data.resultado);
                set("uiSolicitudUsuarioAprobacion", data.usuarioAprobacion);
                set("uiSolicitudFechaAprobacion", data.fechaAprobacionStr);
                set("uiSolicitudObservacionesAprobacion", data.observacionesAprobacion);
                set("uiSolicitudCodigoTransaccionCorregido", data.codigoTransaccionCorrecta);
            });
        } else {
            Warning("La transacción " + codigoTransaccion.toString() + " es una actualización de la Transacción " + codigoTransaccionAnt.toString());
        }
    });
}


function ListarReportesSemanalCajaChicaConsulta(codigoCajaChica, anioOperacion) {
    let objConfiguracion = {
        url: "CorteSemanalCajaChica/ListarReportesCajaChicaConsulta/?codigoCajaChica=" + codigoCajaChica.toString() + "&anioOperacion=" + anioOperacion.toString(),
        cabeceras: ["Código Reporte", "codigoCajaChica", "Caja Chica", "Año", "Semana", "Monto", "Monto F", "Monto NF", "Efectivo Disponible", "Reembolso Calculado","Reembolso Autorizado","Fecha Generación","Estado", "bloqueado", "Anular", "Editar"],
        propiedades: ["codigoReporte", "codigoCajaChica", "nombreCajaChica", "anioOperacion", "semanaOperacion", "monto", "montoFiscal", "montoNoFiscal", "montoDisponible", "montoReembolsoCalculado", "montoReembolso", "fechaCorteStr","estado", "bloqueado", "permisoAnular", "permisoEditar"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        verdetalle: true,
        reporte: true,
        funciondetalle: "Consulta",
        displaydecimals: ["monto", "montoFiscal", "montoNoFiscal", "montoDisponible","montoReembolsoCalculado","montoReembolso"],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "className": "dt-body-center"
            }, {
                "targets": [1],
                "visible": false
            }, {
                "targets": [3],
                "className": "dt-body-center"
            }, {
                "targets": [4],
                "className": "dt-body-center"
            },{
                "targets": [5],
                "className": "dt-body-right"
            }, {
                "targets": [6],
                "className": "dt-body-right"
            }, {
                "targets": [7],
                "className": "dt-body-right"
            }, {
                "targets": [8],
                "className": "dt-body-right"
            }, {
                "targets": [9],
                "className": "dt-body-right"
            }, {
                "targets": [10],
                "className": "dt-body-right"
            }, {
                "targets": [13],
                "visible": false
            }, {
                "targets": [14],
                "visible": false
            }, {
                "targets": [15],
                "visible": false
            }],
        slug: "codigoReporte"
    }
    pintar(objConfiguracion);
}

function GenerarPdf(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-reporte', function () {
        let rowIdx = table.row(this).index();
        let codigoReporte = parseInt(table.cell(rowIdx, 0).data());
        let codigoCajaChica = table.cell(rowIdx, 1).data();
        let anioOperacion = table.cell(rowIdx, 3).data();
        let semanaOperacion = table.cell(rowIdx, 4).data();
        if (codigoReporte != 0) {
            fetchGet("CorteSemanalCajaChica/ViewReporteReintegroCajaChica/?codigoReporte=" + codigoReporte.toString() + "&anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion + "&codigoCajaChica=" + codigoCajaChica, "pdf", function (data) {
                var file = new Blob([data], { type: 'application/pdf' });
                var fileURL = URL.createObjectURL(file);
                window.open(fileURL, "EPrescription");
            });
        }
    });
}

function buscarCortesSemanales() {
    let errores = ValidarDatos("frmBusquedaCortesSemanales")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let codigoCajaChica = parseInt(document.getElementById("uiFiltroCajaChica").value);
    let anioOperacionStr = document.getElementById("uiFiltroAnioOperacion").value;
    let anioOperacion = 0;
    if (anioOperacionStr != "") {
        anioOperacion = parseInt(anioOperacionStr);
    }

    ListarReportesSemanalCajaChicaConsulta(codigoCajaChica, anioOperacion);
}



function ListarDetalleCajaChica(codigoReporte, codigoCajaChica, anioOperacion, semanaOperacion) {
    let objConfiguracion = {
        url: "CajaChica/GetTransaccionesCajaChicaConsulta/?codigoReporte=" + codigoReporte + "&codigoCajaChica=" + codigoCajaChica + "&anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion,
        cabeceras: ["Código Transacción", "Caja Chica", "Nit", "Proveedor", "Fecha factura", "Serie factura", "Número factura", "Monto", "Descripción"],
        propiedades: ["codigoTransaccion", "nombreCajaChica", "nitProveedor", "nombreProveedor", "fechaDocumento", "serieFactura", "numeroDocumento", "monto", "descripcion"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        slug: "codigoTransaccion",
        datesWithoutTime: ["fechaDocumento"],
        displaydecimals: ["monto"],
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [7],
                "className": "dt-body-right"
            }]
    }
    pintar(objConfiguracion);
}

function ListarDetalleCajaChicaConsulta(codigoReporte, codigoCajaChica, anioOperacion, semanaOperacion) {
    let objConfiguracion = {
        url: "CajaChica/GetTransaccionesCajaChicaConsulta/?codigoReporte=" + codigoReporte + "&codigoCajaChica=" + codigoCajaChica + "&anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion,
        cabeceras: ["Código Transacción", "Caja Chica", "Nit", "Proveedor", "Fecha factura", "Serie factura", "Número factura", "Monto", "Descripción"],
        propiedades: ["codigoTransaccion", "nombreCajaChica", "nitProveedor", "nombreProveedor", "fechaDocumento", "serieFactura", "numeroDocumento", "monto", "descripcion"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        slug: "codigoTransaccion",
        datesWithoutTime: ["fechaDocumento"],
        displaydecimals: ["monto"],
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [7],
                "className": "dt-body-right"
            }]
    }
    pintar(objConfiguracion);
}


function fillCombosMotivosExclusion() {
    fetchGet("CajaChica/GetMotivosExclusionDeFacturas", "json", function (rpta) {
        if (rpta != null) {
            FillCombo(rpta, "uiMotivoExclusion", "codigoMotivoExclusion", "nombre", "- seleccione -", "-1");
        } else {
            FillComboUnicaOpcion("uiMotivoExclusion", "-1", "-- Error -- ");
        }
    })
}


function ActualizarTransaccionCajaChica() {
    let errores = ValidarDatos("frmEdicionTransaccion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmEdicionTransaccion");
    let frm = new FormData(frmGuardar);
    let codigoReporte = frm.get("CodigoReporte");
    let codigoTransaccion = frm.get("CodigoTransaccion");
    let codigoOperacion = frm.get("CodigoOperacion");
    let descripcion = frm.get("Descripcion");
    fetchGet("CajaChica/ActualizarTransaccionCajaChica/?codigoTransaccion=" + codigoTransaccion.toString() + "&codigoOperacion=" + codigoOperacion.toString() + "&descripcion=" + descripcion, "text", function (rpta) {
        if (rpta != "OK") {
            MensajeError(rpta);
            return;
        } else {
            document.getElementById("uiClosePopupEditarTransaccion").click();
            RevisarReporte(codigoReporte);
        }
    })
}


function VerDetalle(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', 'tr', 'button', function () {
        let rowIdx = table.row(this).index();
        let codigoCajaChica = table.cell(rowIdx, 1).data();
        let anioOperacion = table.cell(rowIdx, 3).data();
        let semanaOperacion = table.cell(rowIdx, 4).data();
        Redireccionar("CorteSemanalCajaChica", "MostrarDetalle/?codigoReporte=" + obj + "&codigoCajaChica=" + codigoCajaChica + "&anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion);
    });
}

function VerDetalleConsulta(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', 'tr', 'button', function () {
        let rowIdx = table.row(this).index();
        let codigoCajaChica = table.cell(rowIdx, 1).data();
        let anioOperacion = table.cell(rowIdx, 3).data();
        let semanaOperacion = table.cell(rowIdx, 4).data();
        Redireccionar("CorteSemanalCajaChica", "MostrarDetalleConsulta/?codigoReporte=" + obj + "&codigoCajaChica=" + codigoCajaChica + "&anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion);
    });
}



function ListarReembolsosDeCajaChica() {
    let objConfiguracion = {
        url: "CajaChica/GetTransaccionesPorRecepcionarEnTesoreria",
        cabeceras: ["Código Transacción", "codigoReporte", "codigoCajaChica", "Caja Chica", "Año", "Semana", "codigoOperacion", "Operación", "Monto", "Creado por", "Fecha Creación", "codigoEstadoRecepcion", "Estado","Observaciones"],
        propiedades: ["codigoTransaccion","codigoReporte","codigoCajaChica","nombreCajaChica","anioOperacion","semanaOperacion","codigoOperacion","operacion","monto","usuarioIng","fechaIngStr","codigoEstadoRecepcion","estadoRecepcion","observaciones"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        displaydecimals: ["monto"],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [1],
                "visible": false
            }, {
                "targets": [2],
                "visible": false
            },{
                "targets": [6],
                "visible": false
            }, {
                "targets": [8],
                "className": "dt-body-right"
            }, {
                "targets": [11],
                "visible": false
            }, {
                "targets": [13],
                "visible": false
            }],
        slug: "codigoTransaccion",
        aceptar: true,
        funcionaceptar: "RecepcionarReembolso"
    }
    pintar(objConfiguracion);
}

function clickRecepcionarReembolso(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-aceptar', function () {
        let rowIdx = table.row(this).index();
        let codigoTransaccion = table.cell(rowIdx, 0).data();
        let codigoReporte = table.cell(rowIdx, 1).data();
        let codigoCajaChica = table.cell(rowIdx, 2).data();
        let nombreCajaChica = table.cell(rowIdx, 3).data();
        let codigoOperacion = parseInt(table.cell(rowIdx, 6).data());
        let nombreOperacion = table.cell(rowIdx, 7).data();
        let monto = table.cell(rowIdx, 8).data();
        let observaciones = table.cell(rowIdx, 13).data();

        let elementCodigoBanco = document.getElementById("uiCodigoBanco");
        let elementNumeroCheque = document.getElementById("uiNumeroCheque");
        let elementFechaCheque = document.getElementById("uiFechaCheque");


        if (codigoOperacion == REINTEGRO_CAJA_CHICA || codigoOperacion == ABONO_CAJA_CHICA) {
            fillComboBanco("uiCodigoBanco");
            elementCodigoBanco.classList.add('obligatorio');
            elementNumeroCheque.classList.add('obligatorio');
            elementFechaCheque.classList.add('obligatorio');
            document.getElementById('div-reembolso-abono-caja-chica').style.display = 'block';
        }
        else {
            FillComboUnicaOpcion("uiCodigoBanco", "-1", "-- No Aplica -- ");
            elementCodigoBanco.classList.remove('obligatorio');
            elementNumeroCheque.classList.remove('obligatorio');
            elementFechaCheque.classList.remove('obligatorio');
            document.getElementById('div-reembolso-abono-caja-chica').style.display = 'none';
        }

        setI("uiTitlePopupConfirmacionReembolso", "Recepción de " + nombreOperacion);
        let anioOperacion = parseInt(document.getElementById("uiAnioSemanaActualSistema").value);
        let semanaOperacion = parseInt(document.getElementById("uiNumeroSemanaActualSistema").value);
        set("uiCodigoReporte", codigoReporte);
        set("uiAnioOperacion", anioOperacion.toString());
        set("uiSemanaOperacion", semanaOperacion.toString());
        set("uiCodigoTransaccion", codigoTransaccion);
        set("uiCodigoCajaChica", codigoCajaChica);
        set("uiNombreCajaChica", nombreCajaChica);
        set("uiCodigoOperacion", codigoOperacion.toString());
        set("uiNombreOperacion", nombreOperacion);
        set("uiObservaciones", observaciones);
        set("uiMonto", monto);
        set("uiObservacionesRecepcion", "");
        set("uiNumeroCheque", "");
        set("uiFechaCheque", "");

        document.getElementById("ShowPopupConfirmacionReembolso").click();
    });
}

function AceptarRecepcionDeTransaccion() {
    let errores = ValidarDatos("frmConfirmacionRecepcion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmConfirmacionRecepcion");
    let frm = new FormData(frmGuardar);

    fetchPost("CajaChica/RecepcionarTransaccion", "text", frm, function (data) {
        document.getElementById("uiClosePopupConfirmacionReembolso").click();
        if (data != "0") {
            MensajeError(rpta);
        } else {
            ListarReembolsosDeCajaChica();
        }
    })
}


/* Recepcion de reembolso desde contabilidad */
function ListarReembolsosDeCajaChicaContabilidad() {
    let objConfiguracion = {
        url: "CajaChica/GetTransaccionesPorRecepcionarEnTesoreria",
        cabeceras: ["Código Transacción", "codigoReporte", "codigoCajaChica", "Caja Chica", "Año", "Semana", "codigoOperacion", "Operación", "Monto", "Creado por", "Fecha Creación", "codigoEstadoRecepcion", "Estado", "Observaciones"],
        propiedades: ["codigoTransaccion", "codigoReporte", "codigoCajaChica", "nombreCajaChica", "anioOperacion", "semanaOperacion", "codigoOperacion", "operacion", "monto", "usuarioIng", "fechaIngStr", "codigoEstadoRecepcion", "estadoRecepcion", "observaciones"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        displaydecimals: ["monto"],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [1],
                "visible": false
            }, {
                "targets": [2],
                "visible": false
            }, {
                "targets": [6],
                "visible": false
            }, {
                "targets": [8],
                "className": "dt-body-right"
            }, {
                "targets": [11],
                "visible": false
            }, {
                "targets": [13],
                "visible": false
            }],
        slug: "codigoTransaccion",
        aceptar: true,
        funcionaceptar: "RecepcionarReembolsoConta"
    }
    pintar(objConfiguracion);
}


function FillComboDiasOperacion() {
    let anio = parseInt(document.getElementById("uiFiltroAnioOperacion").value);
    let numeroSemana = parseInt(document.getElementById("uiFiltroSemana").value);
    fetchGet("ProgramacionSemanal/GetDiasOperacion/?anio=" + anio.toString() + "&numeroSemana=" + numeroSemana.toString(), "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            FillComboUnicaOpcion("uiFiltroDiaOperacion", "-1", "-- No existe días -- ");
        } else {
            FillCombo(rpta, "uiFiltroDiaOperacion", "fechaStr", "dia", "- seleccione -", "-1");
        }
    });
}

function clickRecepcionarReembolsoConta(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-aceptar', function () {
        let rowIdx = table.row(this).index();
        let codigoTransaccion = table.cell(rowIdx, 0).data();
        let codigoReporte = table.cell(rowIdx, 1).data();
        let codigoCajaChica = table.cell(rowIdx, 2).data();
        let nombreCajaChica = table.cell(rowIdx, 3).data();
        let codigoOperacion = parseInt(table.cell(rowIdx, 6).data());
        let nombreOperacion = table.cell(rowIdx, 7).data();
        let monto = table.cell(rowIdx, 8).data();
        let observaciones = table.cell(rowIdx, 13).data();

        let elementAnioOperacion = document.getElementById("uiFiltroAnioOperacion");
        let elementSemanaOperacion = document.getElementById("uiFiltroSemana");
        let elementCodigoBanco = document.getElementById("uiCodigoBanco");
        let elementNumeroCheque = document.getElementById("uiNumeroCheque");
        let elementFechaCheque = document.getElementById("uiFechaCheque");

        elementAnioOperacion.classList.add('obligatorio');
        elementSemanaOperacion.classList.add('obligatorio');

        if (codigoOperacion == REINTEGRO_CAJA_CHICA || codigoOperacion == ABONO_CAJA_CHICA) {
            fillComboBanco("uiCodigoBanco");
            elementCodigoBanco.classList.add('obligatorio');
            elementNumeroCheque.classList.add('obligatorio');
            elementFechaCheque.classList.add('obligatorio');
            document.getElementById('div-reembolso-abono-caja-chica').style.display = 'block';
        }
        else {
            FillComboUnicaOpcion("uiCodigoBanco", "-1", "-- No Aplica -- ");
            elementCodigoBanco.classList.remove('obligatorio');
            elementNumeroCheque.classList.remove('obligatorio');
            elementFechaCheque.classList.remove('obligatorio');
            document.getElementById('div-reembolso-abono-caja-chica').style.display = 'none';
        }

        setI("uiTitlePopupConfirmacionReembolso", "Recepción de " + nombreOperacion);
        set("uiCodigoReporte", codigoReporte);
        set("uiCodigoTransaccion", codigoTransaccion);
        set("uiCodigoCajaChica", codigoCajaChica);
        set("uiNombreCajaChica", nombreCajaChica);
        set("uiCodigoOperacion", codigoOperacion.toString());
        set("uiNombreOperacion", nombreOperacion);
        set("uiObservaciones", observaciones);
        set("uiMonto", monto);
        set("uiObservacionesRecepcion", "");
        set("uiNumeroCheque", "");
        set("uiFechaCheque", "");

        document.getElementById("ShowPopupConfirmacionReembolso").click();
    });
}


function AceptarRecepcionDeTransaccionConta() {
    let errores = ValidarDatos("frmConfirmacionRecepcion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmConfirmacionRecepcion");
    let frm = new FormData(frmGuardar);
    fetchPost("CajaChica/RecepcionarTransaccion", "text", frm, function (data) {
        document.getElementById("uiClosePopupConfirmacionReembolso").click();
        if (data != "0") {
            MensajeError(rpta);
        } else {
            ListarReembolsosDeCajaChicaContabilidad();
        }
    })
}