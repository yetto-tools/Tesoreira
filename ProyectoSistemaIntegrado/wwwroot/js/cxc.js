window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    if (nameController == "CuentasPorCobrar") {
        switch (nameAction) {
            case "NewCargaInicial":
                FillComboUnicaOpcion("uiTipoCuentaPorCobrar", "-1", "-- No Aplica -- ");
                intelligenceSearch();
                break;
            case "New":
                //MostrarFechaOperacionReporteCxC();
                FillComboUnicaOpcion("uiTipoCuentaPorCobrar", "-1", "-- No Aplica -- ");
                intelligenceSearch();
                break;
            case "MostrarCuentasPorCobrarCargaInicial":
                ListarCuentasPorCobrarCargaInicial();
                break;
            case "RegistroCxC":
                ListarCuentasPorCobrarTemporal();
                break;
            default:
                break;
        }
    } else {
        let codigoReporte = "0";
        let anioOperacion = "0";
        let semanaOperacion = "0";
        let codigoEstadoReporte = "0";
        if (nameController == "CuentasPorCobrarReporte") {
            switch (nameAction) {         
                case "GenerarCuentasPorCobrar":
                    ListarCuentasPorCobrarReporteGeneracion();
                    break;
                case "MostrarDetalleReporteCuentasPorCobrar":
                    codigoReporte = url1.searchParams.get("codigoReporte");
                    anioOperacion = url1.searchParams.get("anioOperacion");
                    semanaOperacion = url1.searchParams.get("semanaOperacion");
                    codigoEstadoReporte = url1.searchParams.get("codigoEstado");
                    document.getElementById("uiExportarExcel").href = "/CuentasPorCobrarReporte/ExportarExcel/?codigoReporte=" + codigoReporte + "&anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion;
                    MostrarDetalleReporteCxCConsulta(codigoReporte, anioOperacion, semanaOperacion, codigoEstadoReporte);
                    break;
                case "MostrarReportesCuentasPorCobrarConsulta":
                    fillAniosReporteCuentasPorCobrar();
                    break;
                case "MostrarDetalleReporteCuentasPorCobrarConsulta":
                    codigoReporte = url1.searchParams.get("codigoReporte");
                    anioOperacion = url1.searchParams.get("anioOperacion");
                    semanaOperacion = url1.searchParams.get("semanaOperacion");
                    codigoEstadoReporte = url1.searchParams.get("codigoEstado");
                    document.getElementById("uiExportarExcel").href = "/CuentasPorCobrarReporte/ExportarExcel/?anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion;
                    MostrarDetalleReporteCxCConsulta(codigoReporte, anioOperacion, semanaOperacion, codigoEstadoReporte);
                    break;
                default:
                    break;
            }
        }
    }
}

function intelligenceSearch() {
    let objGlobalConfigTransaccion = {
        url: "Entidad/ListarEntidadesGenericasCxCPorPrestamosNoRegistradosEnTesoreria",
        cabeceras: ["Código", "Nombre Entidad", "Código Categoría", "Categoria", "Código Operación", "Código Area"],
        propiedades: ["codigoEntidad", "nombreEntidad", "codigoCategoriaEntidad", "nombreCategoria", "codigoOperacionCaja", "codigoArea"],
        divContenedorTabla: "divContenedorTabla",
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [3],
                "visible": false
            }, {
                "targets": [5],
                "visible": false
            }, {
                "targets": [6],
                "visible": false
            }],
        divPintado: "divTabla",
        radio: true,
        paginar: true,
        eventoradio: "Entidades",
        slug: "codigoEntidad"

    }
    pintar(objGlobalConfigTransaccion);
}

function getDataRowRadioEntidades(obj) {
    let table = $('#tabla').DataTable();
    //Set Checked Radio Value
    $('#tabla tbody').on('change', 'tr', 'input:radio', function () {
        let rowIdx = table.row(this).index();
        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
    });
}

function AgregarCuentaPorCobrarCargaInicial() {
    let errores = ValidarDatos("frmNuevaCuentaPorCobrar")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    if (!document.querySelector('input[name="CodigoOperacion"]:checked')) {
        MensajeError("Debe seleccionar el tipo de operación");
        return;
    }

    var frmGuardar = document.getElementById("frmNuevaCuentaPorCobrar");
    var frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("CuentasPorCobrar/GuardarCuentaPorCobrar/?cargaInicial=1", "text", frm, function (data) {
            if (/^[0-9]+$/.test(data)) {
                Exito("CuentasPorCobrar", "MostrarCuentasPorCobrarCargaInicial", true);
            } else {
                MensajeError(data);
            }
        })
    })
}

function AgregarCuentaPorCobrar() {
    let errores = ValidarDatos("frmNuevaCuentaPorCobrar")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    if (!document.querySelector('input[name="CodigoOperacion"]:checked')) {
        MensajeError("Debe seleccionar el tipo de operación");
        return;
    }

    var frmGuardar = document.getElementById("frmNuevaCuentaPorCobrar");
    var frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("CuentasPorCobrar/GuardarCuentaPorCobrar/?cargaInicial=0", "text", frm, function (data) {
            if (/^[0-9]+$/.test(data)) {
                Exito("CuentasPorCobrar", "RegistroCxC", true);
            } else {
                MensajeError(data);
            }
        })
    })
}


function emptyComboTipoCuentaPorCobrar() {
    let elementFechaPrestamo = document.getElementById("uiFechaPrestamo");
    let elementFechaInicioPago = document.getElementById("uiFechaInicioPago");
    let elementTipoCuentaPorCobrar = document.getElementById("uiTipoCuentaPorCobrar");
    elementFechaPrestamo.classList.remove('obligatorio');
    elementFechaInicioPago.classList.remove('obligatorio');
    elementTipoCuentaPorCobrar.classList.remove('obligatorio');
    document.getElementById('div-tipo-cuenta-por-cobrar').style.display = 'none';
    document.getElementById('div-prestamo').style.display = 'none';
    FillComboUnicaOpcion("uiTipoCuentaPorCobrar", "-1", "-- No Aplica -- ");
}

function fillComboTipoCuentaPorCobrar() {
    let elementFechaPrestamo = document.getElementById("uiFechaPrestamo");
    let elementFechaInicioPago = document.getElementById("uiFechaInicioPago");
    let elementTipoCuentaPorCobrar = document.getElementById("uiTipoCuentaPorCobrar");
    elementFechaPrestamo.classList.add('obligatorio');
    elementFechaInicioPago.classList.add('obligatorio');
    elementTipoCuentaPorCobrar.classList.add('obligatorio');
    document.getElementById('div-tipo-cuenta-por-cobrar').style.display = 'block';
    document.getElementById('div-prestamo').style.display = 'block';
    fetchGet("CuentasPorCobrar/GetListTiposCuentasPorCobrar", "json", function (rpta) {
        FillCombo(rpta, "uiTipoCuentaPorCobrar", "codigoTipoCuentaPorCobrar", "nombre", "- seleccione -", "-1");
    });
}

function ListarCuentasPorCobrarCargaInicial() {
    let objConfiguracion = {
        url: "CuentasPorCobrar/GetCuentasPorCobrarCargaInicial",
        cabeceras: ["Código Entidad", "Nombre Entidad", "Código Categoría", "Categoría", "Monto","Operacion"],
        propiedades: ["codigoEntidad", "nombreEntidad", "codigoCategoria", "categoria", "monto","operacion"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        slug: "codigoEntidad",
        displaydecimals: ["monto"],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [2],
                "visible": false
            }, {
                "targets": [4],
                "className": "dt-body-right"
            }]
    }
    pintar(objConfiguracion);
}



function ListarCuentasPorCobrarTemporal() {
    let objConfiguracion = {
        url: "CuentasPorCobrar/GetCuentasPorCobrarTemporal",
        cabeceras: ["Código","Código Entidad", "Nombre Entidad", "Código Categoría", "Categoría", "Monto", "Operacion","Editar","Anular"],
        propiedades: ["codigoCuentaPorCobrar","codigoEntidad", "nombreEntidad", "codigoCategoria", "categoria", "monto", "operacion","permisoEditar","permisoAnular"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        editar: true,
        funcioneditar: "CuentaPorCobrarTemporal",
        eliminar: true,
        funcioneliminar: "CuentaPorCobrarTemporal",
        paginar: true,
        slug: "codigoEntidad",
        displaydecimals: ["monto"],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [3],
                "visible": false
            }, {
                "targets": [5],
                "className": "dt-body-right"
            }, {
                "targets": [7],
                "visible": false
            }, {
                "targets": [8],
                "visible": false
            }]
    }
    pintar(objConfiguracion);
}

function EditarCuentaPorCobrarTemporal()
{
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-editar', function () {
        let rowIdx = table.row(this).index();
        let codigoCuentaPorCobrar = table.cell(rowIdx, 0).data();
        let codigoEntidad = table.cell(rowIdx, 1).data();
        let nombreEntidad = table.cell(rowIdx, 2).data();
        let categoria = table.cell(rowIdx, 4).data();
        let monto = table.cell(rowIdx, 5).data();
        let operacion = table.cell(rowIdx, 6).data();
        setI("uiTitlePopupEditCxCTemporal", "Edición de Cuenta por Cobrar");
        document.getElementById("ShowPopupEditCxCTemporal").click();
        set("uiCodigoCuentaPorCobrar", codigoCuentaPorCobrar);
        set("uiCodigoEntidad", codigoEntidad);
        set("uiNombreEntidad", nombreEntidad);
        set("uiCategoria", categoria);
        set("uiOperacion", operacion);
        set("uiMonto", monto);
    });
}

function ActualizarCuentaPorCobrarTemporal() {
    let errores = ValidarDatos("frmEditCxCTemporal")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let frmGuardar = document.getElementById("frmEditCxCTemporal");
    let frm = new FormData(frmGuardar);
    Confirmacion("Actualización de Cuenta por Cobrar", "¿Está seguro de la actualización?", function (rpta) {
        fetchPost("CuentasPorCobrar/ActualizarCuentaPorCobrarTemporal", "text", frm, function (data) {
            if (data == "OK") {
                document.getElementById("uiClosePopupEditCxCTemporal").click();
                ListarCuentasPorCobrarTemporal();
            } else {
                MensajeError(data);
            }
        })
    })
}

function EliminarCuentaPorCobrarTemporal() {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-eliminar', function () {
        let rowIdx = table.row(this).index();
        let codigoCuentaPorCobrar = table.cell(rowIdx, 0).data();
        Confirmacion("Anulación de Cuenta Por Cobrar", "¿Está seguro de la anulación?", function (rpta) {
            fetchGet("CuentasPorCobrar/AnularCuentaPorCobrarTemporal/?codigoCuentaPorCobrar=" + codigoCuentaPorCobrar, "text", function (data) {
                if (data == "OK") {
                    ListarCuentasPorCobrarTemporal();
                    return;
                } else {
                    MensajeError(data);
                }
            })
        });
    });
}



/* --- Generacion ---- */
//bloqueado: "bloqueado",
function ListarCuentasPorCobrarReporteGeneracion() {
    let objConfiguracion = {
        url: "CuentasPorCobrarReporte/GetReportesCuentasPorCobrarParaGeneracion",
        cabeceras: ["Código Reporte","Año", "Semana", "Anticipo Liquidable", "Devolución Anticipo Liquidable", "Anticipo Salario", "Descuento Anticipo Salario", "Préstamo", "Abono Préstamo","BTB (Devolución)","BTB (Pago)","Retiro Socios","Devolución Socios","bloqueado","Anular","Código Estado","estado"],
        propiedades: ["codigoReporte", "anioOperacion", "semanaOperacion", "anticipoLiquidable", "devolucionAnticipoLiquidable", "anticipoSalario", "descuentoAnticipoSalario", "prestamo", "abonoPrestamo", "backToBack","backToBackPagoPlanilla","retiroSocios","devolucionSocios","bloqueado","permisoAnular","codigoEstado","estado"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        eliminar: true,
        eliminarreporte: true,
        funcioneliminar: "ReporteGenerado",
        paginar: true,
        generar: true,
        verdetalle: true,
        funciondetalle: "ReporteGenerado",
        displaydecimals: ["anticipoLiquidable", "devolucionAnticipoLiquidable", "anticipoSalario", "descuentoAnticipoSalario", "prestamo", "abonoPrestamo", "backToBack","backToBackPagoPlanilla","retiroSocios","devolucionSocios"],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [3],
                "className": "dt-body-right"
            }, {
                "targets": [4],
                "className": "dt-body-right"
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
                "targets": [10],
                "className": "dt-body-right"
            }, {
                "targets": [11],
                "className": "dt-body-right"
            }, {
                "targets": [12],
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
        slug: "codigoReporte",
        aceptar: true,
        funcionaceptar: "AceptarReporteSemanalCxC"
    }
    pintar(objConfiguracion);
}

// Revisado
function GenerarReporte(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', 'tr', '.option-generar', function () {
        let rowIdx = table.row(this).index();
        let codigoReporte = table.cell(rowIdx, 0).data();
        let anioOperacion = table.cell(rowIdx, 1).data();
        let semanaOperacion = table.cell(rowIdx, 2).data();
        fetchGet("CuentasPorCobrarReporte/GenerarReporteCuentaPorCobrar/?codigoReporte=" + codigoReporte + "&anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString(), "text", function (rpta) {
            if (rpta != "0") {
                MensajeError("Error al generar el reporte semanal de cuentas por cobrar");
                return;
            } else {
                ListarCuentasPorCobrarReporteGeneracion();
            }
        })
    });
}

/// Revisado
function clickAceptarReporteSemanalCxC(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click','.option-aceptar', function () {
        let rowIdx = table.row(this).index();
        let codigoReporte = table.cell(rowIdx, 0).data();
        let anioOperacion = table.cell(rowIdx, 1).data();
        let semanaOperacion = table.cell(rowIdx, 2).data();
        let codigoEstadoReporte = parseInt(table.cell(rowIdx, 15).data());
        if (codigoEstadoReporte == ESTADO_REPORTE_CXC_GENERADO) {
            Confirmacion("Reporte de Cuenta por Cobrar", "¿Aceptar el reporte como válido?", function (rpta) {
                fetchGet("CuentasPorCobrarReporte/AceptarReporteComoValido/?codigoReporte=" + codigoReporte + "&anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion, "text", function (data) {
                    if (data != "OK") {
                        MensajeError(data);
                        return;
                    } else {
                        ListarCuentasPorCobrarReporteGeneracion();
                    }
                })
            })
        } else {
            Warning("Falta la generación del reporte");
        }
    });
}

function EliminarReporteGenerado(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', 'tr', 'option-eliminar', function () {
        let rowIdx = table.row(this).index();
        let codigoReporte = table.cell(rowIdx, 0).data();
        let anioOperacion = table.cell(rowIdx, 1).data();
        let semanaOperacion = table.cell(rowIdx, 2).data();
        Confirmacion("Anular Reporte de CxC", "¿Seguro de Eliminar el Reporte Generado?", function (rpta) {
            fetchGet("CuentasPorCobrarReporte/EliminarReporteGenerado/?codigoReporte=" + codigoReporte + "&anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion, "text", function (data) {
                if (data != "OK") {
                    MensajeError(data);
                    return;
                } else {
                    ListarCuentasPorCobrarReporteGeneracion();
                }
            })
        })
    });
}

function VerDetalleReporteGenerado() {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', 'tr', 'button', function () {
        let rowIdx = table.row(this).index();
        let codigoReporte = table.cell(rowIdx, 0).data();
        let anioOperacion = table.cell(rowIdx, 1).data();
        let semanaOperacion = table.cell(rowIdx, 2).data();
        let codigoEstadoReporte = table.cell(rowIdx, 14).data();
        Redireccionar("CuentasPorCobrarReporte", "MostrarDetalleReporteCuentasPorCobrar/?codigoReporte=" + codigoReporte + "&anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion + "&codigoEstado=" + codigoEstadoReporte);
    });
}

// Revisado
function MostrarDetalleReporteCxCConsulta(codigoReporte, anioOperacion, semanaOperacion, codigoEstadoReporte) {
    let objConfiguracion = {
        url: "CuentasPorCobrarReporte/GetDetalleReporteCuentasPorCobrar/?codigoReporte=" + codigoReporte + "&anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString() + "&codigoEstado=" + codigoEstadoReporte,
        cabeceras: ["Código Entidad", "Nombre Entidad", "Código Categoría", "Categoría", "Saldo Inicial", "Devoluciones", "Saldo Final", "Operacion"],
        propiedades: ["codigoEntidad", "nombreEntidad", "codigoCategoria", "categoria", "saldoInicial", "montoDevolucion", "saldoFinal", "operacion"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        slug: "codigoEntidad",
        displaydecimals: ["saldoInicial", "montoDevolucion", "saldoFinal"],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [2],
                "visible": false
            }, {
                "targets": [4],
                "className": "dt-body-right"
            }, {
                "targets": [5],
                "className": "dt-body-right"
            }, {
                "targets": [6],
                "className": "dt-body-right"
            }]
    }
    pintar(objConfiguracion);
}


/*----- Consulta ----------*/ 
function ListarCuentasPorCobrarReporteConsulta() {
    let objConfiguracion = {
        url: "CuentasPorCobrarReporte/GetReportesCuentasPorCobrarParaConsulta",
        cabeceras: ["Código Reporte","Año", "Semana", "Anticipo Liquidable", "Devolución Anticipo Liquidable", "Anticipo Salario", "Descuento Anticipo Salario", "Préstamo", "Abono Préstamo", "Back to Back", "Retiro Socios", "Devolución Socios","Código Estado", "estado"],
        propiedades: ["codigoReporte","anioOperacion", "semanaOperacion", "anticipoLiquidable", "devolucionAnticipoLiquidable", "anticipoSalario", "descuentoAnticipoSalario", "prestamo", "abonoPrestamo", "backToBack", "retiroSocios", "devolucionSocios","codigoEstado", "estado"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        verdetalle: true,
        funciondetalle: "Consulta",
        displaydecimals: ["anticipoLiquidable", "devolucionAnticipoLiquidable", "anticipoSalario", "descuentoAnticipoSalario", "prestamo", "abonoPrestamo", "backToBack", "retiroSocios", "devolucionSocios"],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [3],
                "className": "dt-body-right"
            }, {
                "targets": [4],
                "className": "dt-body-right"
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
                "targets": [10],
                "className": "dt-body-right"
            }, {
                "targets": [11],
                "className": "dt-body-right"
            }],
        slug: "codigoReporte"
    }
    pintar(objConfiguracion);
}


// Revisado
function VerDetalleConsulta() {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', 'tr', 'button', function () {
        let rowIdx = table.row(this).index();
        let codigoReporte = table.cell(rowIdx, 0).data();
        let anioOperacion = table.cell(rowIdx, 1).data();
        let semanaOperacion = table.cell(rowIdx, 2).data();
        let codigoEstadoReporte = table.cell(rowIdx, 12).data();
        Redireccionar("CuentasPorCobrarReporte", "MostrarDetalleReporteCuentasPorCobrarConsulta/?codigoReporte=" + codigoReporte + "&anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion + "&codigoEstado=" + codigoEstadoReporte);
    });
}



function fillAniosReporteCuentasPorCobrar() {
    fetchGet("ProgramacionSemanal/GetAniosProgramacionSemanal", "json", function (rpta) {
        if (rpta != undefined && rpta != null && rpta.length != 0) {
            FillCombo(rpta, "uiAnioReporte", "anio", "anio", "- seleccione -", "-1");
            fillSemanasReporteCuentasPorCobrar();
        } else {
            FillComboUnicaOpcion("uiAnioReporte", "-1", "-- No Existen Años -- ");
        }
    })
}

function fillSemanasReporteCuentasPorCobrar() {
    let arrayDate = getFechaSistema();
    let anioActual = arrayDate[0]["anio"];
    let numeroSemanaActual = parseInt(document.getElementById("uiNumeroSemanaActualSistema").value);
    let anioReporte = parseInt(get("uiAnioReporte"));
    if (anioReporte != anioActual) {
        numeroSemanaActual = 53;
    }
    fetchGet("ProgramacionSemanal/GetSemanasAnteriores/?anio=" + anioReporte.toString() + "&numeroSemanaActual=" + numeroSemanaActual.toString() + "&numeroSemanas=55&incluirSemanaActual=1", "json", function (rpta) {
        if (rpta != undefined && rpta != null && rpta.length != 0) {
            FillCombo(rpta, "uiSemanaReporte", "numeroSemana", "periodo", "- seleccione -", "-1");
        } else {
            FillComboUnicaOpcion("uiSemanaReporte", "-1", "-- No Existen Fechas -- ");
        }
    })
}

function fillSemanasAnteriores(obj) {
    let arrayDate = getFechaSistema();
    let anioActual = arrayDate[0]["anio"];
    let numeroSemanaActual = parseInt(document.getElementById("uiNumeroSemanaActualSistema").value);
    let anioOperacion = parseInt(obj.value);
    if (anioOperacion != anioActual) {
        numeroSemanaActual = 53;
    }
    fetchGet("ProgramacionSemanal/GetSemanasAnteriores/?anio=" + anioOperacion.toString() + "&numeroSemanaActual=" + numeroSemanaActual.toString() + "&numeroSemanas=4&incluirSemanaActual=1", "json", function (rpta) {
        if (rpta != undefined && rpta != null && rpta.length != 0) {
            FillCombo(rpta, "uiNumeroSemanaOperacion", "numeroSemana", "periodo", "- seleccione -", "-1");
        } else {
            FillComboUnicaOpcion("uiNumeroSemanaOperacion", "-1", "-- No Existen Fechas -- ");
        }
    })
}

function fillCargaSaldosIniciales() {
    let numeroSemanaActual = parseInt(document.getElementById("uiNumeroSemanaActualSistema").value);
    let select = document.getElementById("uiAnioAperacion");
    select.selectedIndex = 0;
    let anioOperacion = parseInt(select.value);
    setI("uiTitlePopupCargaSaldosIniciales", "Datos Carga Inicial de Saldos");
    document.getElementById("ShowPopupCargaSaldosIniciales").click();
    fetchGet("ProgramacionSemanal/GetSemanasAnteriores/?anio=" + anioOperacion.toString() + "&numeroSemanaActual=" + numeroSemanaActual.toString() + "&numeroSemanas=6&incluirSemanaActual=1", "json", function (rpta) {
        if (rpta != undefined && rpta != null && rpta.length != 0) {
            FillCombo(rpta, "uiNumeroSemanaOperacion", "numeroSemana", "periodo", "- seleccione -", "-1");
        } else {
            FillComboUnicaOpcion("uiNumeroSemanaOperacion", "-1", "-- No Existen Fechas -- ");
        }
    })
}

function CargarSaldosIniciales() {
    let errores = ValidarDatos("frmCargaInicialSaldos")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let frmGuardar = document.getElementById("frmCargaInicialSaldos");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("CuentasPorCobrar/CargarSaldosIniciales", "text", frm, function (data) {
            if (data == "OK") {
                document.getElementById("uiClosePopupCargaSaldosIniciales").click();
                ListarCuentasPorCobrarCargaInicial();
            } else {
                MensajeError(data);
            }
        })
    })
}


function CargarCuentasPorCobrarTemporal() {
    Confirmacion("Carga de Cuentas por Cobrar", "¿Está seguro de registrar estas cuentas por cobrar?", function (rpta) {
        fetchGet("CuentasPorCobrar/CargarCxCTemporal", "text", function (data) {
            if (data != "OK") {
                MensajeError(data);
                return;
            } else {
                ListarCuentasPorCobrarTemporal();
            }
        });
    });
}


/*function MostrarFechaOperacionReporteCxC() {
    fetchGet("PlanillaTemporal/GetFechaReporteCxCPagoBTBYDescuento", "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            alert("No existe datos");
        } else {
            document.getElementById("uiAnioOperacionReporteCxC").value = rpta.anioOperacion;
            document.getElementById("uiSemanaOperacionReporteCxC").value = rpta.semanaOperacion;
            document.getElementById("uiDescripcionReporteCxC").innerText = "La cuenta por cobrar se registrá en el reporte de cuentas por cobrar en el año " + rpta.anioOperacion + " y semana " + rpta.semanaOperacion;
        }
    })
}*/