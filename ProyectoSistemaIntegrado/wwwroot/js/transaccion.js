window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    if (nameController == "Transaccion") {
        switch (nameAction) {
            case "Index":
                MostrarCompromisoFiscal();
                fillCombosBusqueda();
                MostrarTransacciones(-1, -1, -1);
                break;
            case "ConsultaTransacciones":
                fillCombosBusquedaConsulta();
                MostrarTransaccionesConsulta(-1,-1,-1, -1, -1, -1);
                break;
            case "TrasladoSemanaOperacion":
                MostrarTransaccionesParaCambioSemanaOperacion();
                break;
            case "Revision":
                fillCombosBusquedaConsulta();
                MostrarTransaccionesRevision(-1, -1, -1, -1, -1);
                break;
            case "SolicitudesCorreccion":
                fillCombosBusquedaConsulta();
                MostrarSolicitudesCorreccion(-1, -1, -1, -1, -1);
                break;
            case "CorreccionTransaccion":
                //fillCombosBusquedaConsulta();
                //MostrarTransaccionesCorreccion(-1, -1, -1, -1, -1);
                break;
            case "ComplementoDepositosBancarios":
                fillCombosBusquedaComplemento();
                MostrarDepositosBancarios(-1, -1, -1);
                break;
            default:
                break;
        }// fin switch
    }// fin if
}

function MostrarCompromisoFiscal() {
    let semanaOperacion = parseInt(document.getElementById("uiNumeroSemanaActualSistema").value);
    let anioOperacion = parseInt(document.getElementById("uiAnioSemanaActualSistema").value);
    fetchGet("CompromisoFiscal/GetMontoCompromisosFiscal/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString(), "json", function (data) {
        if (data == null || data == undefined || data.length == 0) {
            document.getElementById("uiMontoCompromisoFiscal").value = "No existe información";
        } else {
            document.getElementById("uiMontoCompromisoFiscal").value = data.montoTotalStr;
        }
    });
}

function fillCombosBusqueda() {
    fetchGet("Transaccion/FillCombosConsultaTransacciones", "json", function (rpta) {
        var listaOperaciones = rpta.listaOperaciones;
        let listaCategorias = rpta.listaCategoriasEntidades;
        FillCombo(listaOperaciones, "uiFiltroOperaciones", "codigoOperacion", "nombre", "- seleccione -", "-1");
        FillCombo(listaCategorias, "uiFiltroCategorias", "codigoCategoriaEntidad", "nombreCategoriaEntidad", "- seleccione -", "-1");
    })
}

function fillCombosBusquedaConsulta() {
    fetchGet("Transaccion/FillCombosConsultaTransacciones", "json", function (rpta) {
        var listaOperaciones = rpta.listaOperaciones;
        let listaCategorias = rpta.listaCategoriasEntidades;
        let listaAnios = rpta.listaAnios;
        FillCombo(listaOperaciones, "uiFiltroOperaciones", "codigoOperacion", "nombre", "- seleccione -", "-1");
        FillCombo(listaCategorias, "uiFiltroCategorias", "codigoCategoriaEntidad", "nombreCategoriaEntidad", "- seleccione -", "-1");
        if (listaAnios.length == 1) {
            let data = [{ "value": "-1", "text": "- seleccione -" }, { "value": listaAnios[0]["anio"], "text": listaAnios[0]["anio"] }]
            FillComboSelectOption(data, "uiFiltroAnio", "value", "text", "- seleccione -", "-1");
        } else {
            FillCombo(listaAnios, "uiFiltroAnio", "anio", "anio", "- seleccione -", "-1");
        }
        FillComboUnicaOpcion("uiFiltroSemana", "-1", "-- No Existen semanas -- ");
        FillComboUnicaOpcion("uiFiltroReporteCaja", "-1", "-- No Existen fechas -- ");
    })
}

function fillCombosBusquedaComplemento() {
    fetchGet("Transaccion/FillCombosConsultaTransacciones", "json", function (rpta) {
        let listaAnios = rpta.listaAnios;
        if (listaAnios.length == 1) {
            let data = [{ "value": "-1", "text": "- seleccione -" }, { "value": listaAnios[0]["anio"], "text": listaAnios[0]["anio"] }]
            FillComboSelectOption(data, "uiFiltroAnio", "value", "text", "- seleccione -", "-1");
        } else {
            FillCombo(listaAnios, "uiFiltroAnio", "anio", "anio", "- seleccione -", "-1");
        }
        FillComboUnicaOpcion("uiFiltroSemana", "-1", "-- No Existen semanas -- ");
        FillComboUnicaOpcion("uiFiltroReporteCaja", "-1", "-- No Existen fechas -- ");
    })
}


function fillSemanasTransacciones(obj) {
    let arrayDate = getFechaSistema();
    let anioActual = arrayDate[0]["anio"];
    let numeroSemanaActual = parseInt(document.getElementById("uiNumeroSemanaActualSistema").value);
    //let anioReporte = parseInt(get("uiFiltroAnio"));
    let anioReporte = parseInt(obj);
    if (anioReporte != anioActual) {
        numeroSemanaActual = 53;
    }
    fetchGet("ProgramacionSemanal/GetSemanasAnteriores/?anio=" + anioReporte.toString() + "&numeroSemanaActual=" + numeroSemanaActual.toString() + "&numeroSemanas=7&incluirSemanaActual=1", "json", function (rpta) {
        if (rpta != undefined && rpta != null && rpta.length != 0) {
            FillCombo(rpta, "uiFiltroSemana", "numeroSemana", "periodo", "- seleccione -", "-1");
        } else {
            FillComboUnicaOpcion("uiFiltroSemana", "-1", "-- No Existen Fechas -- ");
        }
    })
}


function MostrarTransacciones(codigoOperacion, codigoCategoriaEntidad, diaOperacion) {
    let objConfiguracion = {
        url: "Transaccion/BuscarTransacciones/?codigoOperacion=" + codigoOperacion.toString() + "&codigoCategoriaEntidad=" + codigoCategoriaEntidad.toString() + "&diaOperacion=" + diaOperacion.toString(),
        cabeceras: ["Código", "codigoTransaccionAnt", "correccion", "Código Operación", "Operación", "Código Cuenta por Cobrar","Año","Semana", "Fecha Operación", "Día Operación", "Fecha Recibo", "Número Recibo", "Entidad", "Categoría", "Monto", "Estado", "Fecha Transacción", "Creado por", "Anular", "Editar", "Signo", "Número Recibo", "Ruta", "Fecha Impresión", "Recursos","codigoSeguridad"],
        propiedades: ["codigoTransaccion", "codigoTransaccionAnt", "correccion","codigoOperacion", "operacion", "codigoCuentaPorCobrar","anioOperacion", "semanaOperacion", "fechaStr", "nombreDiaOperacion", "fechaReciboStr", "numeroRecibo", "nombreEntidad", "categoriaEntidad", "monto", "estado", "fechaIngStr", "usuarioIng", "permisoAnular", "permisoEditar", "signo", "numeroReciboStr", "ruta", "fechaImpresionStr", "recursos","codigoSeguridad"],
        displaydecimals: ["monto"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        editar: true,
        funcioneditar: "Transaccion",
        eliminar: true,
        imprimir: true,
        paginar: true,
        sumarcolumna: true,
        columnasuma: 14,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [1],
                "visible": false
            }, {
                "targets": [2],
                "visible": false
            }, {
                "targets": [3],
                "visible": false
            }, {
                "targets": [5],
                "visible": false
            }, {
                "targets": [14],
                "className": "dt-body-right"
            }, {
                "targets": [18],
                "visible": false
            }, {
                "targets": [19],
                "visible": false
            }, {
                "targets": [20],
                "visible": false
            }, {
                "targets": [21],
                "visible": false
            }, {
                "targets": [22],
                "visible": false
            }, {
                "targets": [23],
                "visible": false
            }, {
                "targets": [24],
                "visible": false
            }, {
                "targets": [25],
                "visible": false
            }],
        slug: "codigoTransaccion"
    }
    pintar(objConfiguracion);
}

function EditarTransaccion() {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-editar', function () {
        let rowIdx = table.row(this).index();
        let codigoTransaccion = table.cell(rowIdx, 0).data();
        Redireccionar("Transaccion", "Edit/?codigoTransaccion=" + codigoTransaccion);
    });
}


function Eliminar(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-eliminar', function () {
        let rowIdx = table.row(this).index();
        let codigoOperacion = parseInt(table.cell(rowIdx, 3).data());
        let nombreOperacion = table.cell(rowIdx, 4).data();
        let codigoCuentaPorCobrar = parseInt(table.cell(rowIdx, 5).data());
        let nombreEntidad = table.cell(rowIdx, 12).data();
        let monto = table.cell(rowIdx, 14).data();
        setI("uiTitlePopupAnulacion", "Anulación de Transacción");
        document.getElementById("ShowPopupAnulacion").click();
        set("uiCodigoTransaccion", obj.toString());
        set("uiCodigoOperacion", codigoOperacion.toString());
        set("uiOperacion", nombreOperacion);
        set("uiNombreEntidad", nombreEntidad);
        set("uiMonto", monto);
        set("uiCodigoCuentaPorCobrar", codigoCuentaPorCobrar.toString());
        set("uiObservaciones", "");
    });
}

function AnularTransaccion() {
    let errores = ValidarDatos("frmAnulacionTransaccion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmAnulacionTransaccion");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Transaccion/AnularTransaccion", "text", frm, function (data) {
            if (data == "OK") {
                MostrarTransacciones(-1, -1, -1);
            } else {
                MensajeError(data);
            }
            document.getElementById("uiClosePopupAnulacion").click();
        });
    });
}


function Imprimir(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-imprimir', function () {
        let rowIdx = table.row(this).index();
        let codigoOperacion = parseInt(table.cell(rowIdx, 3).data());
        let nombreOperacion = table.cell(rowIdx, 4).data();
        let fechaReciboStr = table.cell(rowIdx, 10).data();
        let nombreEntidad = table.cell(rowIdx, 12).data();
        let monto = table.cell(rowIdx, 14).data();
        let usuarioCreacion = table.cell(rowIdx, 17).data();
        let signo = parseInt(table.cell(rowIdx, 20).data());
        let numeroReciboStr = table.cell(rowIdx, 21).data();
        let ruta = table.cell(rowIdx, 22).data();
        let fechaImpresion = table.cell(rowIdx, 23).data();
        let recurso = table.cell(rowIdx, 24).data();
        let codigoSeguridad = table.cell(rowIdx, 25).data();
        let recursos = "";
        if (recurso != "") {
            if (recurso.endsWith(',', recurso.length) == true) {
                recursos = recurso.substring(0, recurso.length - 1);
            } else {
                recursos = recurso;
            }
        }

        if (signo == 1) { // Ingreso
            ImprimirConstanciaIngresos(codigoOperacion, numeroReciboStr, fechaReciboStr, nombreEntidad, nombreOperacion, monto, recursos, usuarioCreacion, ruta, fechaImpresion, codigoSeguridad);
        } else { // Egreso
            ImprimirConstanciaEgresos(numeroReciboStr, fechaReciboStr, nombreEntidad, nombreOperacion, monto, usuarioCreacion, fechaImpresion, codigoSeguridad);
        }
    });
}

function MostrarTransaccionesConsulta(anioOperacion, semanaOperacion, codigoReporte, codigoOperacion, codigoCategoriaEntidad, diaOperacion) {
    let errores = ValidarDatos("frmBusquedaTransacciones")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let objConfiguracion = {
        url: "Transaccion/BuscarTransaccionesConsulta/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString() + "&codigoReporte=" + codigoReporte.toString() + "&codigoOperacion=" + codigoOperacion.toString() + "&codigoCategoriaEntidad=" + codigoCategoriaEntidad.toString() + "&diaOperacion=" + diaOperacion.toString(),
        cabeceras: ["Código", "codigoTransaccionAnt", "correccion","Código Operación", "Operación", "Código Cuenta por Cobrar", "Año", "Semana", "Fecha Operación", "Día Operación", "Fecha Recibo", "Número Recibo", "Entidad", "Categoría", "Monto", "Estado", "Fecha Transacción", "Creado por", "Anular", "Editar", "Signo", "Número Recibo", "Ruta", "Fecha Impresión", "Recursos"],
        propiedades: ["codigoTransaccion", "codigoTransaccionAnt", "correccion","codigoOperacion", "operacion", "codigoCuentaPorCobrar", "anioOperacion", "semanaOperacion", "fechaStr", "nombreDiaOperacion", "fechaReciboStr", "numeroRecibo", "nombreEntidad", "categoriaEntidad", "monto", "estado", "fechaIngStr", "usuarioIng", "permisoAnular", "permisoEditar", "signo", "numeroReciboStr", "ruta", "fechaImpresionStr", "recursos"],
        displaydecimals: ["monto"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        alerta: true,
        funcionalerta: "CorreccionTransaccion",
        imprimir: true,
        sumarcolumna: true,
        columnasuma: 14,
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [1],
                "visible": false
            }, {
                "targets": [2],
                "visible": false
            }, {
                "targets": [3],
                "visible": false
            }, {
                "targets": [5],
                "visible": false
            }, {
                "targets": [14],
                "className": "dt-body-right"
            }, {
                "targets": [18],
                "visible": false
            }, {
                "targets": [19],
                "visible": false
            }, {
                "targets": [20],
                "visible": false
            }, {
                "targets": [21],
                "visible": false
            }, {
                "targets": [22],
                "visible": false
            }, {
                "targets": [23],
                "visible": false
            }, {
                "targets": [24],
                "visible": false
            }],
        slug: "codigoTransaccion"
    }
    pintar(objConfiguracion);
}


//function MostrarTransaccionesCorreccion(anioOperacion, semanaOperacion, codigoReporte, codigoOperacion, codigoCategoriaEntidad) {
//    let errores = ValidarDatos("frmBusquedaTransacciones")
//    if (errores != "") {
//        MensajeError(errores);
//        return;
//    }

//    let objConfiguracion = {
//        url: "Transaccion/BuscarTransaccionesParaCorreccion/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString() + "&codigoReporte=" + codigoReporte.toString() + "&codigoOperacion=" + codigoOperacion.toString() + "&codigoCategoriaEntidad=" + codigoCategoriaEntidad.toString(),
//        cabeceras: ["Código", "Código Operación", "Operación", "Código Cuenta por Cobrar", "Fecha Operación", "Día Operación", "Fecha Recibo", "Número Recibo", "Entidad", "Categoría", "Monto", "Estado", "Fecha Transacción", "Creado por", "Anular", "Editar", "Signo", "Número Recibo", "Ruta", "Fecha Impresión", "Recursos","codigoTransaccionAnt"],
//        propiedades: ["codigoTransaccion", "codigoOperacion", "operacion", "codigoCuentaPorCobrar", "fechaStr", "nombreDiaOperacion", "fechaReciboStr", "numeroReciboStr", "nombreEntidad", "categoriaEntidad", "monto", "estado", "fechaIngStr", "usuarioIng", "permisoAnular", "permisoEditar", "signo", "numeroReciboStr", "ruta", "fechaImpresionStr", "recursos","codigoTransaccionAnt"],
//        displaydecimals: ["monto"],
//        divContenedorTabla: "divContenedorTabla",
//        divPintado: "divTabla",
//        editar: true,
//        funcioneditar: "CorreccionTransaccion",
//        paginar: true,
//        ocultarColumnas: true,
//        hideColumns: [
//            {
//                "targets": [1],
//                "visible": false
//            }, {
//                "targets": [3],
//                "visible": false
//            }, {
//                "targets": [10],
//                "className": "dt-body-right"
//            }, {
//                "targets": [14],
//                "visible": false
//            }, {
//                "targets": [15],
//                "visible": false
//            }, {
//                "targets": [16],
//                "visible": false
//            }, {
//                "targets": [17],
//                "visible": false
//            }, {
//                "targets": [18],
//                "visible": false
//            }, {
//                "targets": [19],
//                "visible": false
//            }, {
//                "targets": [20],
//                "visible": false
//            }, {
//                "targets": [21],
//                "visible": false
//            }],
//        slug: "codigoTransaccion"
//    }
//    pintar(objConfiguracion);
//}


//function EditarCorreccionTransaccion() {
//    let table = $('#tabla').DataTable();
//    $('#tabla tbody').on('click', '.option-editar', function () {
//        let rowIdx = table.row(this).index();
//        let codigoTransaccion = table.cell(rowIdx, 0).data();
//        Redireccionar("Transaccion", "Edit/?codigoTransaccion=" + codigoTransaccion);
//    });
//}

function buscarTransacciones() {
    let objOperacion = document.getElementById("uiFiltroOperaciones");
    let objCategoriaEntidad = document.getElementById("uiFiltroCategorias");
    let objDiaOperacion = document.getElementById("uiFiltroDias");
    let codigoOperacion = parseInt(objOperacion.options[objOperacion.selectedIndex].value);
    let codigoCategoriaEntidad = parseInt(objCategoriaEntidad.options[objCategoriaEntidad.selectedIndex].value);
    let diaOperacion = parseInt(objDiaOperacion.options[objDiaOperacion.selectedIndex].value);
    MostrarTransacciones(codigoOperacion, codigoCategoriaEntidad, diaOperacion);
}

function buscarTransaccionesConsulta() {
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnio").value);
    let semanaOperacion = parseInt(document.getElementById("uiFiltroSemana").value);
    let codigoReporte = parseInt(document.getElementById("uiFiltroReporteCaja").value);
    let objOperacion = document.getElementById("uiFiltroOperaciones");
    let objCategoriaEntidad = document.getElementById("uiFiltroCategorias");
    let objDiaOperacion = document.getElementById("uiFiltroDias");
    let codigoOperacion = parseInt(objOperacion.options[objOperacion.selectedIndex].value);
    let codigoCategoriaEntidad = parseInt(objCategoriaEntidad.options[objCategoriaEntidad.selectedIndex].value);
    let diaOperacion = parseInt(objDiaOperacion.options[objDiaOperacion.selectedIndex].value);
    MostrarTransaccionesConsulta(anioOperacion, semanaOperacion, codigoReporte, codigoOperacion, codigoCategoriaEntidad, diaOperacion);
}


function buscarTransaccionesCorreccion() {
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnio").value);
    let semanaOperacion = parseInt(document.getElementById("uiFiltroSemana").value);
    let codigoReporte = parseInt(document.getElementById("uiFiltroReporteCaja").value);
    let objOperacion = document.getElementById("uiFiltroOperaciones");
    let objCategoriaEntidad = document.getElementById("uiFiltroCategorias");
    let codigoOperacion = parseInt(objOperacion.options[objOperacion.selectedIndex].value);
    let codigoCategoriaEntidad = parseInt(objCategoriaEntidad.options[objCategoriaEntidad.selectedIndex].value);
    MostrarTransaccionesCorreccion(anioOperacion, semanaOperacion, codigoReporte, codigoOperacion, codigoCategoriaEntidad);
}

function buscarSolicitudesDeCorreccion() {
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnio").value);
    let semanaOperacion = parseInt(document.getElementById("uiFiltroSemana").value);
    let codigoReporte = parseInt(document.getElementById("uiFiltroReporteCaja").value);
    let objOperacion = document.getElementById("uiFiltroOperaciones");
    let objCategoriaEntidad = document.getElementById("uiFiltroCategorias");
    let codigoOperacion = parseInt(objOperacion.options[objOperacion.selectedIndex].value);
    let codigoCategoriaEntidad = parseInt(objCategoriaEntidad.options[objCategoriaEntidad.selectedIndex].value);
    MostrarSolicitudesCorreccion(anioOperacion, semanaOperacion, codigoReporte, codigoOperacion, codigoCategoriaEntidad);
}


function MostrarTransaccionesRevision(anioOperacion, semanaOperacion, codigoReporte, codigoOperacion, codigoCategoriaEntidad) {
    let errores = ValidarDatos("frmBusquedaTransacciones")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let objConfiguracion = {
        url: "Transaccion/BuscarTransaccionesParaCorreccion/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString() + "&codigoReporte=" + codigoReporte.toString() + "&codigoOperacion=" + codigoOperacion.toString() + "&codigoCategoriaEntidad=" + codigoCategoriaEntidad.toString(),
        cabeceras: ["Código", "CodigoTransaccionAnt", "Corrección", "Código Operación", "Operación", "Código Cuenta por Cobrar", "Fecha Operación", "Día Operación", "Fecha Recibo", "Número Recibo", "Entidad", "Categoría", "Monto", "Estado", "Fecha Transacción", "Creado por", "Anular", "Editar", "Signo", "Número Recibo", "Ruta", "Fecha Impresión", "Recursos", "Revisado", "Corregir", "Anular", "codigoEstadoSolicitudCorreccion", "Estado Corrección", ],
        propiedades: ["codigoTransaccion", "codigoTransaccionAnt", "correccion", "codigoOperacion", "operacion", "codigoCuentaPorCobrar", "fechaStr", "nombreDiaOperacion", "fechaReciboStr", "numeroReciboStr", "nombreEntidad", "categoriaEntidad", "monto", "estado", "fechaIngStr", "usuarioIng", "permisoAnular", "permisoEditar", "signo", "numeroReciboStr", "ruta", "fechaImpresionStr", "recursos", "revisado", "permisoCorregir", "permisoAnular","codigoEstadoSolicitudCorreccion", "estadoSolicitudCorreccion", ],
        displaydecimals: ["monto"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        editar: true,
        funcioneditar: "RevisionTransaccion",
        eliminar: true,
        funcioneliminar: "TransaccionPorCorreccion",
        revision: true,
        funcionrevision: "SolicitaAprobacionCorreccion",
        nombrecolumnarevision: "Solicitar",
        alerta: true,
        funcionalerta: "CorreccionTransaccion",
        check: true,
        checkid: "revisado",
        funcioncheck: "RevisionTransaccion",
        checkheader: "Revisado",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [1],
                "visible": false
            }, {
                "targets": [2],
                "visible": false
            }, {
                "targets": [3],
                "visible": false
            }, {
                "targets": [5],
                "visible": false
            }, {
                "targets": [12],
                "className": "dt-body-right"
            }, {
                "targets": [16],
                "visible": false
            }, {
                "targets": [17],
                "visible": false
            }, {
                "targets": [18],
                "visible": false
            }, {
                "targets": [19],
                "visible": false
            }, {
                "targets": [20],
                "visible": false
            }, {
                "targets": [21],
                "visible": false
            }, {
                "targets": [22],
                "visible": false
            }, {
                "targets": [23],
                "visible": false
            }, {
                "targets": [24],
                "visible": false
            }, {
                "targets": [25],
                "visible": false
            }, {
                "targets": [26],
                "visible": false
            }, {
                "targets": [29],
                "className": "dt-body-center",
                "visible": true
            }],
        slug: "codigoTransaccion"
    }
    pintar(objConfiguracion);
}

function buscarTransaccionesRevision() {
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnio").value);
    let semanaOperacion = parseInt(document.getElementById("uiFiltroSemana").value);
    let codigoReporte = parseInt(document.getElementById("uiFiltroReporteCaja").value);
    let objOperacion = document.getElementById("uiFiltroOperaciones");
    let objCategoriaEntidad = document.getElementById("uiFiltroCategorias");
    let codigoOperacion = parseInt(objOperacion.options[objOperacion.selectedIndex].value);
    let codigoCategoriaEntidad = parseInt(objCategoriaEntidad.options[objCategoriaEntidad.selectedIndex].value);
    MostrarTransaccionesRevision(anioOperacion, semanaOperacion, codigoReporte, codigoOperacion, codigoCategoriaEntidad);
}


function SolicitaAprobacionCorreccion(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click','.option-revision', function () {
        let rowIdx = table.row(this).index();
        let codigoTransaccion = table.cell(rowIdx, 0).data();
        let operacion = table.cell(rowIdx, 4).data();
        let nombreEntidad = table.cell(rowIdx, 10).data();
        let categoriaEntidad = table.cell(rowIdx, 11).data();
        let monto = table.cell(rowIdx, 12).data();
        setI("uiTitlePopupSolicitudAprobacion", "Solicitud de Aprobación de Corrección");
        document.getElementById("ShowPopupSolicitudAprobacion").click();
        set("uiCodigoTransaccion", codigoTransaccion);
        set("uiOperacion", operacion);
        set("uiNombreEntidad", nombreEntidad);
        set("uiCategoriaEntidad", categoriaEntidad);
        set("uiMonto", monto);
        set("uiObservaciones", "");
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
    Confirmacion("Solicitud de Aprobación de Corrección", "¿Está seguro de realizar esta solicitud?", function (rpta) {
        fetchPost("Transaccion/RegistrarSolicitudAprobacionDeCorreccion", "text", frm, function (data) {
            if (data == "OK") {
                document.getElementById("uiClosePopupSolicitudAprobacion").click();
                buscarTransaccionesRevision();
            } else {
                MensajeError(data);
            }
        });
    });
}


function EditarRevisionTransaccion() {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-editar', function () {
        let rowIdx = table.row(this).index();
        let codigoTransaccion = table.cell(rowIdx, 0).data();
        Redireccionar("Transaccion", "EditRevision/?codigoTransaccion=" + codigoTransaccion);
    });
}

function EliminarTransaccionPorCorreccion(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-eliminar', function () {
        let rowIdx = table.row(this).index();
        let codigoOperacion = parseInt(table.cell(rowIdx, 3).data());
        let nombreOperacion = table.cell(rowIdx, 4).data();
        let codigoCuentaPorCobrar = parseInt(table.cell(rowIdx, 5).data());
        let nombreEntidad = table.cell(rowIdx, 10).data();
        let monto = table.cell(rowIdx, 12).data();
        setI("uiTitlePopupAnulacion", "Anulación de Transacción");
        document.getElementById("ShowPopupAnulacion").click();
        set("uiCodigoTransaccionAnulacion", obj.toString());
        set("uiCodigoOperacionAnulacion", codigoOperacion.toString());
        set("uiOperacionAnulacion", nombreOperacion);
        set("uiNombreEntidadAnulacion", nombreEntidad);
        set("uiMontoAnulacion", monto);
        set("uiCodigoCuentaPorCobrarAnulacion", codigoCuentaPorCobrar.toString());
        set("uiObservacionesAnulacion", "");
    });
}

function AnularTransaccionPorCorreccion() {
    let errores = ValidarDatos("frmAnulacionTransaccion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmAnulacionTransaccion");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Transaccion/AnularTransaccion", "text", frm, function (data) {
            document.getElementById("uiClosePopupAnulacion").click();
            if (data == "OK") {
                buscarTransaccionesRevision();
            } else {
                MensajeError(data);
            }
        });
    });
}



function clickCheckRevisionTransaccion(obj) {
    let checked = 0;
    if (obj.checked == 1) {
        checked = 1;
    }
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click','.option-check', function () {
        let rowIdx = table.row(this).index();
        let codigoTransaccion = table.cell(rowIdx, 0).data();
        fetchGet("Transaccion/AceptarRevision/?codigoTransaccion=" + codigoTransaccion + "&revisado=" + checked.toString(), "text", function (rpta) {
            if (rpta == "OK") {
                buscarTransaccionesRevision();
            } else {
                MensajeError(data);
            }
        })
    });
}



function clickAlertaCorreccionTransaccion(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-alerta', function () {
        let rowIdx = table.row(this).index();
        let codigoTransaccion = BigInt(table.cell(rowIdx, 0).data());
        let codigoTransaccionAnt = BigInt(table.cell(rowIdx, 1).data());
        let correccion = parseInt(table.cell(rowIdx, 2).data());
        if (correccion == 1) {
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
            fetchGet("Transaccion/GetDataCorreccion/?codigoTransaccion=" + codigoTransaccion.toString(), "json", function (data) {
                if (data == null || data == undefined || data.length == 0) {
                    MensajeError("Error en la obtención de datos de la solicitud de corrección de la transacción " + codigoTransaccion.toString());
                } else {
                    set("uiSolicitudCodigoTransaccion", data.codigoTransaccion);
                    set("uiSolicitudObservacionesSolicitud", data.observacionesSolicitud);
                    set("uiSolicitudUsuarioSolicitud", data.usuarioIng);
                    set("uiSolicitudFechaSolicitud", data.fechaIngStr);
                    set("uiSolicitudResultado", data.resultado);
                    set("uiSolicitudUsuarioAprobacion", data.usuarioAprobacion);
                    set("uiSolicitudFechaAprobacion", data.fechaAprobacionStr);
                    set("uiSolicitudObservacionesAprobacion", data.observacionesAprobacion);
                    set("uiSolicitudCodigoTransaccionCorregido", data.codigoTransaccionCorrecta);
                    set("uiSolicitudTipoCorreccion", data.tipoCorreccion);
                }
            });
        } else {
            Warning("La transacción " + codigoTransaccion.toString() + " es una actualización de la Transacción " + codigoTransaccionAnt.toString());
        }
    });
}


function MostrarSolicitudesCorreccion(anioOperacion, semanaOperacion, codigoReporte, codigoOperacion, codigoCategoriaEntidad) {
    let objConfiguracion = {
        url: "Transaccion/GetSolicitudesDeCorreccion/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString() + "&codigoReporte=" + codigoReporte.toString() + "&codigoOperacion=" + codigoOperacion.toString() + "&codigoCategoriaEntidad=" + codigoCategoriaEntidad.toString(),
        cabeceras: ["Código", "codigoTransaccionAnt", "correccion", "Código Operación", "Operación", "Código Cuenta por Cobrar", "Fecha Operación", "Día Operación", "Fecha Recibo", "Número Recibo", "Entidad", "Categoría", "Monto", "Estado", "Fecha Transacción", "Creado por", "Anular", "Editar", "Signo", "Número Recibo", "Ruta", "Fecha Impresión", "Recursos", "Revisado", "Corregir", "Autorizar", "codigoEstadoSolicitudCorreccion", "Estado Corrección", "codigoTipoCorreccion","Tipo Corrección"],
        propiedades: ["codigoTransaccion", "codigoTransaccionAnt", "correccion", "codigoOperacion", "operacion", "codigoCuentaPorCobrar", "fechaStr", "nombreDiaOperacion", "fechaReciboStr", "numeroReciboStr", "nombreEntidad", "categoriaEntidad", "monto", "estado", "fechaIngStr", "usuarioIng", "permisoAnular", "permisoEditar", "signo", "numeroReciboStr", "ruta", "fechaImpresionStr", "recursos", "revisado", "permisoCorregir", "permisoAutorizar", "codigoEstadoSolicitudCorreccion", "estadoSolicitudCorreccion", "codigoTipoCorreccion", "tipoCorreccion"],
        displaydecimals: ["monto"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        autorizar: true,
        funcionautorizar: "AutorizarCorreccion",
        alerta: true,
        funcionalerta: "CorreccionTransaccion",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [1],
                "visible": false
            }, {
                "targets": [2],
                "visible": false
            }, {
                "targets": [3],
                "visible": false
            }, {
                "targets": [5],
                "visible": false
            }, {
                "targets": [12],
                "className": "dt-body-right"
            }, {
                "targets": [16],
                "visible": false
            }, {
                "targets": [17],
                "visible": false
            }, {
                "targets": [18],
                "visible": false
            }, {
                "targets": [19],
                "visible": false
            }, {
                "targets": [20],
                "visible": false
            }, {
                "targets": [21],
                "visible": false
            }, {
                "targets": [22],
                "visible": false
            }, {
                "targets": [23],
                "visible": false
            }, {
                "targets": [24],
                "visible": false
            }, {
                "targets": [25],
                "visible": false
            }, {
                "targets": [26],
                "visible": false
            }, {
                "targets": [27],
                "visible": true
            }, {
                "targets": [28],
                "visible": false
            }, {
                "targets": [29],
                "visible": true
            }],
        slug: "codigoTransaccion"
    }
    pintar(objConfiguracion);
}


function clickAutorizarCorreccion(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click','.option-autorizar', function () {
        let rowIdx = table.row(this).index();
        let codigoTransaccion = table.cell(rowIdx, 0).data();
        let operacion = table.cell(rowIdx, 4).data();
        let nombreEntidad = table.cell(rowIdx, 10).data();
        let categoriaEntidad = table.cell(rowIdx, 11).data();
        let monto = table.cell(rowIdx, 12).data();
        setI("uiTitlePopupCorreccionAprobada", "Aprobación de Corrección");
        document.getElementById("ShowPopupCorreccionAprobada").click();
        set("uiCodigoTransaccion", codigoTransaccion);
        set("uiOperacion", operacion);
        set("uiNombreEntidad", nombreEntidad);
        set("uiCategoriaEntidad", categoriaEntidad);
        set("uiMonto", monto);
        document.getElementById("uiResultado").selectedIndex = 0;
        set("uiObservaciones", "");
    });
}

function AutorizarCorreccion() {
    let errores = ValidarDatos("frmAutorizacionCorreccion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmAutorizacionCorreccion");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Transaccion/AutorizarCorreccion", "text", frm, function (data) {
            if (data == "OK") {
                document.getElementById("uiClosePopupCorreccionAprobada").click();
                MostrarSolicitudesCorreccion(-1, -1, -1, -1, -1);
            } else {
                MensajeError(data);
            }
        });
    });
}


function ImprimirConstanciaIngresos(codigoOperacion, numeroRecibo, fechaRecibo, nombreEntidad, nombreOperacion, monto, recursos, usuarioCreacion, ruta, fechaImpresion, codigoSeguridad) {
    let html = getHtmlConstanciaIngreso(codigoOperacion, numeroRecibo, fechaRecibo, nombreEntidad, nombreOperacion, monto, recursos, usuarioCreacion, ruta, fechaImpresion, codigoSeguridad);
    var ventana = window.open();
    ventana.document.write(html);
    ventana.print();
    ventana.close();
}

function ImprimirConstanciaEgresos(numeroRecibo, fechaRecibo, nombreEntidad, nombreOperacion, monto, usuarioCreacion, fechaImpresion, codigoSeguridad) {
    let html = getHtmlConstanciaEgreso(numeroRecibo, fechaRecibo, nombreEntidad, nombreOperacion, monto, usuarioCreacion, fechaImpresion, codigoSeguridad);

    var ventana = window.open();
    ventana.document.write(html);
    ventana.print();
    ventana.close();
}

function MostrarTransaccionesParaCambioSemanaOperacion() {
    let objConfiguracion = {
        url: "Transaccion/GetTransaccionParaCambioDeSemanaOperacion",
        cabeceras: ["Año Operación", "Semana Operación", "Cantidad Transacciones","Editar"],
        propiedades: ["anio", "numeroSemana", "cantidadTransacciones","permisoEditar"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        editar: true,
        funcioneditar: "CambioSemanaOperacion",
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "visible": true,
                "className": "dt-body-center"
            }, {
                "targets": [1],
                "visible": true,
                "className": "dt-body-center"
            }, {
                "targets": [2],
                "visible": true,
                "className": "dt-body-center"
            }, {
                "targets": [3],
                "visible": false
            }],
        slug: "anio"
    }
    pintar(objConfiguracion);
}

function fillAniosReporte() {
    fetchGet("ProgramacionSemanal/GetAniosProgramacionSemanal", "json", function (rpta) {
        if (rpta != undefined && rpta != null && rpta.length != 0) {
            FillCombo(rpta, "uiFiltroAnio", "anio", "anio", "- seleccione -", "-1");
            fillSemanasReporte();
        } else {
            FillComboUnicaOpcion("uiFiltroAnio", "-1", "-- No Existen Años -- ");
        }
    })
}

function fillSemanasReporte() {
    let arrayDate = getFechaSistema();
    let anioActual = arrayDate[0]["anio"];
    let numeroSemanaActual = parseInt(document.getElementById("uiNumeroSemanaActualSistema").value);
    let anioReporte = parseInt(get("uiFiltroAnio"));
    if (anioReporte != anioActual) {
        numeroSemanaActual = 53;
    }
    fetchGet("ProgramacionSemanal/GetSemanasAnteriores/?anio=" + anioReporte.toString() + "&numeroSemanaActual=" + numeroSemanaActual.toString() + "&numeroSemanas=55&incluirSemanaActual=1", "json", function (rpta) {
        if (rpta != undefined && rpta != null && rpta.length != 0) {
            FillCombo(rpta, "uiFiltroSemanaOperacion", "numeroSemana", "periodo", "- seleccione -", "-1");
        } else {
            FillComboUnicaOpcion("uiFiltroSemanaOperacion", "-1", "-- No Existen Fechas -- ");
        }
    })
}

function EditarCambioSemanaOperacion(){
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-editar', function () {
        let rowIdx = table.row(this).index();
        fillAniosReporte();
        setI("uiTitlePopupSemanaOperacion", "Cambio de Semana de Operación");
        document.getElementById("ShowPopupSemanaOperacion").click();
    });
}


function CambiarSemanaOperacion() {
    let errores = ValidarDatos("frmCambioSemanaOperacion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmCambioSemanaOperacion");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Transaccion/CambiarSemanaOperacionTransacciones", "text", frm, function (data) {
            if (data == "OK") {
                MostrarTransaccionesParaCambioSemanaOperacion();
            } else {
                MensajeError(data);
            }
            document.getElementById("uiClosePopupSemanaOperacion").click();
        });
    });
}

function FillReportesDeCaja() {
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnio").value);
    let semanaOperacion = parseInt(document.getElementById("uiFiltroSemana").value);
    FillFechasDeCorte(anioOperacion, semanaOperacion);
}

function FillFechasDeCorte(anioOperacion, semanaOperacion) {
    fetchGet("CorteCajaSemanal/GetReportesCaja/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString(), "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            FillComboUnicaOpcion("uiFiltroReporteCaja", "-1", "-- No existe reportes -- ");
        } else {
            FillCombo(rpta, "uiFiltroReporteCaja", "codigoReporte", "fechaCorteStr", "- seleccione -", "-1");
        }
    })
}

function FillReportesDeCajaRevision() {
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnio").value);
    let semanaOperacion = parseInt(document.getElementById("uiFiltroSemana").value);
    FillFechasDeCorteRevision(anioOperacion, semanaOperacion);
}

function FillFechasDeCorteRevision(anioOperacion, semanaOperacion) {
    fetchGet("CorteCajaSemanal/GetReportesCajaEnProceso/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString(), "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            FillComboUnicaOpcion("uiFiltroReporteCaja", "-1", "-- No existe reportes -- ");
        } else {
            FillCombo(rpta, "uiFiltroReporteCaja", "codigoReporte", "fechaCorteStr", "- seleccione -", "-1");
        }
    })
}

function FillReportesDeCajaConsulta() {
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnio").value);
    let semanaOperacion = parseInt(document.getElementById("uiFiltroSemana").value);
    FillFechasDeCorteConsulta(anioOperacion, semanaOperacion);
}

function FillFechasDeCorteConsulta(anioOperacion, semanaOperacion) {
    fetchGet("CorteCajaSemanal/GetReportesCajaConsulta/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString(), "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            FillComboUnicaOpcion("uiFiltroReporteCaja", "-1", "-- No existe reportes -- ");
        } else {
            FillCombo(rpta, "uiFiltroReporteCaja", "codigoReporte", "fechaCorteStr", "- seleccione -", "-1");
        }
    })
}


function buscarTransaccionesBancarias() {
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnio").value);
    let semanaOperacion = parseInt(document.getElementById("uiFiltroSemana").value);
    let codigoReporte = parseInt(document.getElementById("uiFiltroReporteCaja").value);
    MostrarDepositosBancarios(anioOperacion, semanaOperacion, codigoReporte);
}

function MostrarDepositosBancarios(anioOperacion, semanaOperacion, codigoReporte) {
    let objConfiguracion = {
        url: "Transaccion/BuscarTransaccionesDepositosBancarios/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString() + "&codigoReporte=" + codigoReporte.toString(),
        cabeceras: ["Código", "Código Operación", "Operación", "Fecha Operación", "Día Operación", "Fecha Recibo", "Número Recibo", "Entidad", "Cuenta", "Categoría","Monto", "Número Voucher","Número Boleta", "Estado", "codigoEstadoSolicitudCorreccion", "Estado Corrección", "Fecha Transacción", "Creado por", "Signo", "Número Recibo", "Ruta", "Fecha Impresión", "codigoTransaccionAnt", "Revisado","permisoActualizar"],
        propiedades: ["codigoTransaccion", "codigoOperacion", "operacion", "fechaStr", "nombreDiaOperacion", "fechaReciboStr", "numeroReciboStr", "nombreEntidad","numeroCuenta","categoriaEntidad", "monto","numeroVoucher","numeroBoleta", "estado", "codigoEstadoSolicitudCorreccion", "estadoSolicitudCorreccion", "fechaIngStr", "usuarioIng", "signo", "numeroReciboStr", "ruta", "fechaImpresionStr", "codigoTransaccionAnt", "revisado","permisoActualizar"],
        displaydecimals: ["monto"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        addTextBox: true,
        propertiesColumnTextBox: [
            {
                "header": "Número Boleta",
                "value": "numeroBoleta",
                "name": "NumeroBoleta",
                "align": "text-center",
                "validate": "solonumeros"
            }],
        actualizar: true,
        funcionactualizar: "NumeroBoletaDeposito",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [1],
                "visible": false
            }, {
                "targets": [5],
                "visible": false
            }, {
                "targets": [6],
                "visible": false
            }, {
                "targets": [9],
                "visible": false
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
            }, {
                "targets": [16],
                "visible": false
            }, {
                "targets": [17],
                "visible": false
            }, {
                "targets": [18],
                "visible": false
            }, {
                "targets": [19],
                "visible": false
            }, {
                "targets": [20],
                "visible": false
            }, {
                "targets": [21],
                "visible": false
            }, {
                "targets": [22],
                "visible": false
            }, {
                "targets": [23],
                "visible": false
            }, {
                "targets": [24],
                "visible": false
            }],
        slug: "codigoTransaccion"
    }
    pintar(objConfiguracion);
}

         
function clickActualizarNumeroBoletaDeposito(obj) {
    let codigoTransaccion = obj;
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-aceptar', function () {
        let rowIdx = table.row(this).index();
        let numeroBoletaDeposito = table.cell(rowIdx, 25).nodes().to$().find('input').val();
        if (numeroBoletaDeposito != "") {
            fetchGet("Transaccion/ActualizarNumeroBoletaDeposito/?codigoTransaccion=" + codigoTransaccion.toString() + "&numeroBoletaDeposito=" + numeroBoletaDeposito, "text", function (data) {
                if (data == "OK") {
                    buscarTransaccionesBancarias();
                } else {
                    MensajeError(data);
                }
            });
        } else {
            Warning("No ha registrado número de boleta de depósito");
        }
    });
}

function GenerarExcelTransaccionesRevision() {
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnio").value);
    let semanaOperacion = parseInt(document.getElementById("uiFiltroSemana").value);
    let codigoReporte = parseInt(document.getElementById("uiFiltroReporteCaja").value);
    let objOperacion = document.getElementById("uiFiltroOperaciones");
    let objCategoriaEntidad = document.getElementById("uiFiltroCategorias");
    let codigoOperacion = parseInt(objOperacion.options[objOperacion.selectedIndex].value);
    let codigoCategoriaEntidad = parseInt(objCategoriaEntidad.options[objCategoriaEntidad.selectedIndex].value);
    document.getElementById("uiExportarExcel").href = "/Transaccion/ExportarExcelTransaccionRevision/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString() + "&codigoReporte=" + codigoReporte.toString() + "&codigoOperacion=" + codigoOperacion.toString() + "&codigoCategoriaEntidad=" + codigoCategoriaEntidad.toString();
    document.getElementById("uiExportarExcel").click();
}

function GenerarExcelTransaccionesConsulta() {
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnio").value);
    let semanaOperacion = parseInt(document.getElementById("uiFiltroSemana").value);
    let codigoReporte = parseInt(document.getElementById("uiFiltroReporteCaja").value);
    let objOperacion = document.getElementById("uiFiltroOperaciones");
    let objCategoriaEntidad = document.getElementById("uiFiltroCategorias");
    let objDiaOperacion = document.getElementById("uiFiltroDias");
    let codigoOperacion = parseInt(objOperacion.options[objOperacion.selectedIndex].value);
    let codigoCategoriaEntidad = parseInt(objCategoriaEntidad.options[objCategoriaEntidad.selectedIndex].value);
    let diaOperacion = parseInt(objDiaOperacion.options[objDiaOperacion.selectedIndex].value);
    document.getElementById("uiExportarExcel").href = "/Transaccion/ExportarExcelTransaccionConsulta/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString() + "&codigoReporte=" + codigoReporte.toString() + "&codigoOperacion=" + codigoOperacion.toString() + "&codigoCategoriaEntidad=" + codigoCategoriaEntidad.toString() + "&diaOperacion=" + diaOperacion.toString();
    document.getElementById("uiExportarExcel").click();
}


function GenerarExcelTransaccionesEnProceso() {
    let objOperacion = document.getElementById("uiFiltroOperaciones");
    let objCategoriaEntidad = document.getElementById("uiFiltroCategorias");
    let objDiaOperacion = document.getElementById("uiFiltroDias");
    let codigoOperacion = parseInt(objOperacion.options[objOperacion.selectedIndex].value);
    let codigoCategoriaEntidad = parseInt(objCategoriaEntidad.options[objCategoriaEntidad.selectedIndex].value);
    let diaOperacion = parseInt(objDiaOperacion.options[objDiaOperacion.selectedIndex].value);
    document.getElementById("uiExportarExcel").href = "/Transaccion/ExportarExcelTransaccionesEnProceso/?codigoOperacion=" + codigoOperacion.toString() + "&codigoCategoriaEntidad=" + codigoCategoriaEntidad.toString() + "&diaOperacion=" + diaOperacion.toString();
    document.getElementById("uiExportarExcel").click();

}