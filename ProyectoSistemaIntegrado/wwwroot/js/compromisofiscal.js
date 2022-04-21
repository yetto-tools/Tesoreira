window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    if (nameController == "CompromisoFiscal") {FillComboDiasOperacion
        let codigoEmpresa = parseInt(url1.searchParams.get("codigoEmpresa"));
        let anioOperacion = parseInt(url1.searchParams.get("anioOperacion"));
        let semanaOperacion = parseInt(url1.searchParams.get("semanaOperacion"));
        switch (nameAction) {
            case "RegistroFacturasAlContado":
                FillAniosCompromisoFiscal();
                FillComboUnicaOpcion("uiFiltroDiaOperacion", "-1", "-- No existe días -- ");
                MostrarTransaccionesFacturaAlContado();
                break;
            case "ConsultaFacturasAlContado":
                FillComboEmpresa();
                FillAniosCompromisoFiscal();
                MostrarCompromisosFiscales(-1,-1,-1);
                break;
            case "EdicionFacturasAlContado":
                FillComboEmpresa();
                FillAniosCompromisoFiscal();
                MostrarCompromisosFiscalesEdicion(-1, -1, -1);
                break;
            case "DetalleFacturasAlContado":
                MostrarDetalleCompromisoFiscal(codigoEmpresa, anioOperacion, semanaOperacion);
                break;
            case "EdicionDetalleFacturasAlContado":
                MostrarDetalleCompromisoFiscalEdicion(codigoEmpresa, anioOperacion, semanaOperacion);
                break;
            default:
                break;
        }// fin switch
    }// fin if
}

function FillComboEmpresa() {
    fetchGet("Empresa/GetAllEmpresas", "json", function (rpta) {
        FillCombo(rpta, "uiFiltroEmpresa", "codigoEmpresa", "nombreComercial", "- seleccione -", "-1");
    })
}

function FillAniosCompromisoFiscal() {
    fetchGet("ProgramacionSemanal/GetAniosProgramacionSemanal", "json", function (rpta) {
        if (rpta != undefined && rpta != null && rpta.length != 0) {
            FillCombo(rpta, "uiFiltroAnio", "anio", "anio", "- seleccione -", "-1");
            FillFiltroSemanas();
        } else {
            FillComboUnicaOpcion("uiFiltroAnio", "-1", "-- No Existen Años -- ");
        }
    })
}

function FillReportesDeCaja() {
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnio").value);
    let semanaOperacion = parseInt(document.getElementById("uiFiltroSemanaOperacion").value);
}

function FillComboDiasOperacion() {
    let anio = parseInt(document.getElementById("uiFiltroAnio").value);
    let numeroSemana = parseInt(document.getElementById("uiFiltroSemanaOperacion").value);
    fetchGet("ProgramacionSemanal/GetDiasOperacion/?anio=" + anio.toString() + "&numeroSemana=" + numeroSemana.toString(), "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            FillComboUnicaOpcion("uiFiltroDiaOperacion", "-1", "-- No existe días -- ");
        } else {
            FillCombo(rpta, "uiFiltroDiaOperacion", "fechaStr", "dia", "- seleccione -", "-1");
        }
    });
}

function FillFiltroSemanas() {
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

function MostrarTransaccionesFacturaAlContado() {
    let objConfiguracion = {
        url: "Transaccion/BuscarTransaccionesReporteFacturadoAlContado",
        cabeceras: ["Código", "Código Operación", "Operación", "Año", "Semana", "Día", "Entidad", "Categoría", "Monto", "Estado", "Fecha Transacción", "Creado por", "Anular", "Editar", "Signo", "Número Recibo", "Ruta", "Fecha Impresión", "Recursos"],
        propiedades: ["codigoTransaccion", "codigoOperacion", "operacion", "anioOperacion", "semanaOperacion", "nombreDiaOperacion", "nombreEntidad", "categoriaEntidad", "monto", "estado", "fechaIngStr", "usuarioIng", "permisoAnular", "permisoEditar", "signo", "numeroReciboStr", "ruta", "fechaImpresionStr", "recursos"],
        displaydecimals: ["monto"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        eliminar: true,
        funcioneliminar: "TransaccionComplemento",
        paginar: true,
        sumarcolumna: true,
        columnasumalist: [8],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [1],
                "visible": false
            }, {
                "targets": [8],
                "className": "dt-body-right"
            }, {
                "targets": [12],
                "visible": false
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
            }],
        slug: "codigoTransaccion"
    }
    pintar(objConfiguracion);
}

function setFiltroRegistroFacturasAlContado() {
    let selectEmpresa = document.getElementById("uiFiltroEmpresa");
    set("uiMonto", "");
    MostrarTransaccionesFacturaAlContado();
}

function GuardarFacturasAlContado() {
    let errores = ValidarDatos("frmFacturasAlContado")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let frmGuardar = document.getElementById("frmFacturasAlContado");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Transaccion/GuardarDatos/?complemento=2", "text", frm, function (data) {
            if (data == "OK") {
                setFiltroRegistroFacturasAlContado();
            } else {
                MensajeError(data);
            }
        })
    })
}

function EliminarTransaccionComplemento(obj) {
    Confirmacion("Anulación de Operación", "¿Está seguro de anular la operación?", function (rpta) {
        fetchGet("Transaccion/AnularTransaccionComplementoContabilidad/?codigoTransaccion=" + obj.toString(), "text", function (data) {
            if (data != "OK") {
                MensajeError(data);
                return;
            } else {
                MostrarTransaccionesFacturaAlContado();
            }
        })
    })
}

function MostrarCompromisosFiscales(codigoEmpresa, anioOperacion, semanaOperacion) {
    let objConfiguracion = {
        url: "CompromisoFiscal/GetCompromisosFiscales/?codigoEmpresa=" + codigoEmpresa.toString() + "&anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString(),
        cabeceras: ["Código Empresa", "Nombre Empresa", "Año", "Número Semana","Código Reporte", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sábado","Domingo","Total"],
        propiedades: ["codigoEmpresa", "nombreEmpresa", "anioOperacion", "semanaOperacion","codigoReporte", "montoLunes", "montoMartes", "montoMiercoles", "montoJueves", "montoViernes", "montoSabado","montoDomingo", "montoTotal"],
        displaydecimals: ["montoLunes","montoMartes","montoMiercoles","montoJueves","montoViernes","montoSabado","montoDomingo","montoTotal"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        ocultarColumnas: true,
        verdetalle: true,
        sumarcolumna: true,
        columnasumalist: [12],
        funciondetalle: "CompromisoFiscal",
        hideColumns: [
            {
                "targets": [1],
                "visible": false
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
        slug: "codigoEmpresa"
    }
    pintar(objConfiguracion);
}

function VerDetalleCompromisoFiscal(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-detalle', function () {
        let rowIdx = table.row(this).index();
        let codigoEmpresa = parseInt(table.cell(rowIdx, 0).data());
        let anioOperacion = parseInt(table.cell(rowIdx, 2).data());
        let semanaOperacion = parseInt(table.cell(rowIdx, 3).data());
        Redireccionar("CompromisoFiscal", "DetalleFacturasAlContado/?codigoEmpresa=" + codigoEmpresa.toString() + "&anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString());
    });
}

function MostrarDetalleCompromisoFiscal(codigoEmpresa, anioOperacion, semanaOperacion) {
    let objConfiguracion = {
        url: "CompromisoFiscal/GetDetalleCompromisoFiscal/?codigoEmpresa=" + codigoEmpresa + "&anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion,
        cabeceras: ["Transacción", "Código Empresa", "Nombre Empresa", "Año", "Número Semana", "Número Dia", "Día", "Monto", "Creado por", "Fecha Creación"],
        propiedades: ["codigoTransaccion", "codigoEmpresa", "nombreEmpresa", "anioOperacion", "semanaOperacion", "diaOperacion", "nombreDia", "monto", "usuarioIng", "fechaIngresoStr"],
        displaydecimals: ["monto"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        ocultarColumnas: true,
        sumarcolumna: true,
        columnasumalist: [7],
        hideColumns: [
            {
                "targets": [1],
                "visible": false
            }, {
                "targets": [5],
                "visible": false
            }, {
                "targets": [7],
                "className": "dt-body-right"
            }],
        slug: "codigoEmpresa"
    }
    pintar(objConfiguracion);
}


function MostrarCompromisosFiscalesEdicion(codigoEmpresa, anioOperacion, semanaOperacion) {
    let objConfiguracion = {
        url: "CompromisoFiscal/GetCompromisosFiscales/?codigoEmpresa=" + codigoEmpresa.toString() + "&anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString(),
        cabeceras: ["Código Empresa", "Nombre Empresa", "Año", "Número Semana","Código Reporte", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sábado","Domingo","Total"],
        propiedades: ["codigoEmpresa", "nombreEmpresa", "anioOperacion", "semanaOperacion","codigoReporte", "montoLunes", "montoMartes", "montoMiercoles", "montoJueves", "montoViernes", "montoSabado","montoDomingo", "montoTotal"],
        displaydecimals: ["montoLunes", "montoMartes", "montoMiercoles", "montoJueves", "montoViernes", "montoSabado","montoDomingo","montoTotal"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        ocultarColumnas: true,
        verdetalle: true,
        sumarcolumna: true,
        columnasumalist: [12],
        funciondetalle: "CompromisoFiscalEdicion",
        hideColumns: [
            {
                "targets": [0],
                "visible": false
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
        slug: "codigoEmpresa"
    }
    pintar(objConfiguracion);
}

function VerDetalleCompromisoFiscalEdicion(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', 'tr', '.option-detalle', function () {
        let rowIdx = table.row(this).index();
        let codigoEmpresa = parseInt(table.cell(rowIdx, 0).data());
        let anioOperacion = parseInt(table.cell(rowIdx, 2).data());
        let semanaOperacion = parseInt(table.cell(rowIdx, 3).data());
        Redireccionar("CompromisoFiscal", "EdicionDetalleFacturasAlContado/?codigoEmpresa=" + codigoEmpresa.toString() + "&anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString());
    });
}

function MostrarDetalleCompromisoFiscalEdicion(codigoEmpresa, anioOperacion, semanaOperacion) {
    let objConfiguracion = {
        url: "CompromisoFiscal/GetDetalleCompromisoFiscal/?codigoEmpresa=" + codigoEmpresa.toString() + "&anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString(),
        cabeceras: ["Transacción", "Código Empresa", "Nombre Empresa", "Año", "Número Semana", "Número Dia", "Día", "Código Reporte","Monto", "Creado por", "Fecha Creación", "Anular","Código Operación","Operación"],
        propiedades: ["codigoTransaccion", "codigoEmpresa", "nombreEmpresa", "anioOperacion", "semanaOperacion", "diaOperacion", "nombreDia","codigoReporte", "monto", "usuarioIng", "fechaIngresoStr", "permisoAnular","codigoOperacion","operacion"],
        displaydecimals: ["monto"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        ocultarColumnas: true,
        sumarcolumna: true,
        eliminar: true,
        funcioneliminar: "DetalleCompromisoFiscalEdicion",
        columnasumalist: [8],
        hideColumns: [
            {
                "targets": [1],
                "visible": false
            }, {
                "targets": [5],
                "visible": false
            }, {
                "targets": [8],
                "className": "dt-body-right"
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
        slug: "codigoTransaccion"
    }
    pintar(objConfiguracion);
}

function EliminarDetalleCompromisoFiscalEdicion(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', 'tr', 'input:button', function () {
        let rowIdx = table.row(this).index();
        let codigoCuentaPorCobrar = 0;
        let codigoEmpresa = table.cell(rowIdx, 1).data();
        let nombreEntidad = table.cell(rowIdx, 2).data();
        let anioOperacion = table.cell(rowIdx, 3).data();
        let semanaOperacion = table.cell(rowIdx, 4).data();
        let codigoReporte = table.cell(rowIdx, 7).data();
        let monto = table.cell(rowIdx, 7).data();
        let codigoOperacion = parseInt(table.cell(rowIdx, 11).data());
        let nombreOperacion = table.cell(rowIdx, 12).data();
        setI("uiTitlePopupAnulacion", "Anulación de Transacción");
        document.getElementById("ShowPopupAnulacion").click();
        set("uiCodigoTransaccion", obj.toString());
        set("uiCodigoEmpresa", codigoEmpresa);
        set("uiAnioOperacion", anioOperacion);
        set("uiSemanaOperacion", semanaOperacion);
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
    let codigoTransaccion = frm.get("CodigoTransaccion");
    let codigoOperacion = frm.get("CodigoEmpresa");
    let codigoEmpresa = frm.get("CodigoEmpresa");
    let anioOperacion = frm.get("AnioOperacion");
    let semanaOperacion = frm.get("SemanaOperacion");
    let observaciones = frm.get("Observaciones");
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Transaccion/AnularTransaccion?codigoTransaccion=" + codigoTransaccion + "&codigoOperacion=" + codigoOperacion + "&codigoCuentaPorCobrar=0&observaciones=" + observaciones, "text", frm, function (data) {
            if (data == "OK") {
                MostrarDetalleCompromisoFiscalEdicion(codigoEmpresa, anioOperacion, semanaOperacion);
            } else {
                MensajeError(data);
            }
            document.getElementById("uiClosePopupAnulacion").click();
        });
    });
}


function BuscarCompromisosFiscales() {
    let anioOperacion = parseInt(get("uiFiltroAnio"));
    let codigoEmpresa = parseInt(get("uiFiltroEmpresa"));
    let semanaOperacion = parseInt(get("uiFiltroSemanaOperacion"));
    MostrarCompromisosFiscales(codigoEmpresa, anioOperacion, semanaOperacion);
}

function BuscarCompromisosFiscalesEdicion() {
    let anioOperacion = parseInt(get("uiFiltroAnio"));
    let codigoEmpresa = parseInt(get("uiFiltroEmpresa"));
    let semanaOperacion = parseInt(get("uiFiltroSemanaOperacion"));
    MostrarCompromisosFiscalesEdicion(codigoEmpresa, anioOperacion, semanaOperacion);
}



function CargarCompromisoFiscal() {
    let table = $('#tabla').DataTable();
    var data = table.rows().data();
    let arrayProperties = new Array();
    let obj = null;
    data.each(function (value, index) {
        obj = {
            CodigoTransaccion: table.cell(index, 0).data()
        };
        arrayProperties.push(obj);
    });
    let jsonData = JSON.stringify(arrayProperties);
    if (obj != null) {
        Confirmacion(undefined, undefined, function (rpta) {
            fetchPostJson("CompromisoFiscal/CargarCompromisoFiscal", "text", jsonData, function (data) {
                if (data == "OK") {
                    MostrarTransaccionesFacturaAlContado();
                } else {
                    MensajeError(data);
                }
            })
        })
    } else {
        Warning("No existen transacciones");
    }

}