window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    if (nameController == "TipoReporte") {
        switch (nameAction) {
            case "Index":
                MostrarReportesGestion();
                break;
            default:
                break;
        }// fin switch
    } else {
        if (nameController == "Reportes") {
            switch (nameAction) {
                case "Index":
                    //document.getElementById("uiExportarExcel").href = "/ReportesQSystems/ExportarExcel";
                    MostrarReportes();
                    break;
                case "MostrarReporte":
                    let codigoTipoReporte = parseInt(url1.searchParams.get("codigoTipoReporte"));
                    ListarReportes(codigoTipoReporte);
                    break;
                case "MostrarReporteCorteCajaChica":
                    fillComboCajaChica();
                    ListarCortesCajaChica(0, -1)
                    break;
                case "MostrarReporteCompromisoFiscal":
                    //fillComboCajaChica();
                    ListarReportesCompromisoFiscal(-1);
                    break;
                case "MostrarReporteVentasFacturadas":
                    fillComboEmpresasComercializadoras();
                    break;
                case "MostrarReporteValesSalida":
                    fillComboEmpresasComercializadoras();
                    break;
                default:
                    break;
            }// fin switch
        }
    }
}


function fillComboCajaChica() {
    fetchGet("CajaChica/GetCajasChicas", "json", function (rpta) {
        FillCombo(rpta, "uiFiltroCajaChica", "codigoCajaChica", "nombreCajaChica", "- seleccione -", "-1");
    })
}


function MostrarReportesGestion() {
    let objConfiguracion = {
        url: "TipoReporte/GetAllTiposReportes",
        cabeceras: ["Código", "Nombre", "Descripción", "Nombre Controlador", "Nombre Accion","PDF","EXCEL","WEB","Editar","Anular"],
        propiedades: ["codigoTipoReporte", "nombre", "descripcion", "nombreControlador", "nombreAccion","pdf","excel","web","permisoEditar","permisoAnular"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        editar: true,
        funcioneditar: "Reporte",
        eliminar: true,
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
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
                "targets": [9],
                "visible": false
            }],
        slug: "codigoTipoReporte"
    }
    pintar(objConfiguracion);
}

function AgregarNuevoReporte() {
    setI("uiTitlePopupNewReporte", "Nuevo Reporte");
    set("uiNewNombre", "");
    set("uiNewDescripcion", "");
    set("uiNewNombreControlador", "");
    set("uiNewNombreAccion", "");
    document.querySelector('#uiNewPdf').checked = false;
    document.querySelector('#uiNewExcel').checked = false;
    document.querySelector('#uiNewWeb').checked = false;
    document.getElementById("ShowPopupNewReport").click();
}

function GuardarReporte() {
    let errores = ValidarDatos("frmNewReporte")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmNewReporte");
    let frm = new FormData(frmGuardar);

    let checkPdf = document.getElementById('uiNewPdf');
    let checkExcel = document.getElementById('uiNewExcel');
    let checkWeb = document.getElementById('uiNewWeb');
    if (checkPdf.checked == true) {
        frm.set("Pdf", "1");
    } else {
        frm.set("Pdf", "0");
    }
    if (checkExcel.checked == true) {
        frm.set("Excel", "1");
    } else {
        frm.set("Excel", "0");
    }
    if (checkWeb.checked == true) {
        frm.set("Web", "1");
    } else {
        frm.set("Web", "0");
    }

    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("TipoReporte/GuardarTipoReporte", "text", frm, function (data) {
            if (data == "OK") {
                MostrarReportesGestion();
            } else {
                MensajeError(data);
            }
            document.getElementById("uiClosePopupNewReporte").click();
        });
    });
}

function EditarReporte() {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-editar', function () {
        let rowIdx = table.row(this).index();
        let codigoTipoReporte = table.cell(rowIdx, 0).data();
        let nombre = table.cell(rowIdx, 1).data();
        let descripcion = table.cell(rowIdx, 2).data();
        let nombreControlador = table.cell(rowIdx, 3).data();
        let nombreAccion = table.cell(rowIdx, 4).data();
        let pdf = table.cell(rowIdx, 5).data();
        let excel = table.cell(rowIdx, 6).data();
        let web = table.cell(rowIdx, 7).data();
        setI("uiTitlePopupEditReporte", "Edición de Reporte");
        set("uiEditCodigoTipoReporte", codigoTipoReporte);
        set("uiEditNombre", nombre);
        set("uiEditDescripcion", descripcion);
        set("uiEditNombreControlador", nombreControlador);
        set("uiEditNombreAccion", nombreAccion);
        document.querySelector('#uiEditPdf').checked = pdf == "1" ? true : false;
        document.querySelector('#uiEditExcel').checked = excel == "1" ? true : false;
        document.querySelector('#uiEditWeb').checked = web == "1" ? true : false;
        document.getElementById("ShowPopupEditReporte").click();
    });
}

function ActualizarReporte() {
    let errores = ValidarDatos("frmEditReporte")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmEditReporte");
    let frm = new FormData(frmGuardar);

    let checkPdf = document.getElementById('uiEditPdf');
    let checkExcel = document.getElementById('uiEditExcel');
    let checkWeb = document.getElementById('uiEditWeb');
    if (checkPdf.checked == true) {
        frm.set("Pdf", "1");
    } else {
        frm.set("Pdf", "0");
    }
    if (checkExcel.checked == true) {
        frm.set("Excel", "1");
    } else {
        frm.set("Excel", "0");
    }
    if (checkWeb.checked == true) {
        frm.set("Web", "1");
    } else {
        frm.set("Web", "0");
    }

    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("TipoReporte/ActualizarTipoReporte", "text", frm, function (data) {
            if (data == "OK") {
                MostrarReportesGestion();
            } else {
                MensajeError(data);
            }
            document.getElementById("uiClosePopupEditReporte").click();
        });
    });
}

function MostrarReportes() {
    let objConfiguracion = {
        url: "Reportes/GetTiposDeReportesAsignados",
        cabeceras: ["Código", "Nombre", "Descripción", "Nombre Controlador", "Nombre Accion","PDF","EXCEL","WEB","Categoría"],
        propiedades: ["codigoTipoReporte", "nombre", "descripcion", "nombreControlador", "nombreAccion","pdf","excel","web","categoria"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        verdetalle: true,
        funciondetalle: "Reporte",
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [3],
                "visible": false
            }, {
                "targets": [4],
                "visible": false
            }, {
                "targets": [5],
                "visible": false
            }, {
                "targets": [6],
                "visible": false
            }, {
                "targets": [7],
                "visible": false
            }],
        slug: "codigoTipoReporte"
    }
    pintar(objConfiguracion);
}

function VerDetalleReporte(obj) {
    let codigoTipoReporte = obj;
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-detalle', function () {
        let rowIdx = table.row(this).index();
        //let codigoTipoReporte = table.cell(rowIdx, 0).data();
        switch (codigoTipoReporte) {
            case REPORTE_CORTE_CAJA_CHICA:
                Redireccionar("Reportes", "MostrarReporteCorteCajaChica");
                break;
            case REPORTE_COMPROMISO_FISCAL:
                Redireccionar("Reportes", "MostrarReporteCompromisoFiscal");
                break;
            case REPORTE_VENTAS_FACTURADAS:
                Redireccionar("Reportes", "MostrarReporteVentasFacturadas");
                break;
            case REPORTE_VALES_SALIDA:
                Redireccionar("Reportes", "MostrarReporteValesSalida");
                break;
            case REPORTE_DEPOSITOS_BANCARIOS_Y_BTB_MULTIPLES_SEMANAS:
                Redireccionar("Reportes", "MostrarReporteDepositosBancarios");
                break;
            default:
                Redireccionar("Reportes", "MostrarReporte/?codigoTipoReporte=" + codigoTipoReporte.toString());
                break;
        }// fin switch
    });
}

// Columna que contiene si el check esta enable
//checkid
function ListarReportes(codigoTipoReporte) {
    let objConfiguracion = {
        url: "Reportes/GetReportes/?codigoTipoReporte=" + codigoTipoReporte.toString(),
        cabeceras: ["codigoTipoReporte", "Arqueo", "Código Reporte", "Año", "Semana", "Periodo", "Nombre Reporte", "Estado", "Fecha Generación", "Nombre Controlador", "Nombre Accion", "PDF", "EXCEL", "WEB", "Tipo","deshabilitarCheck"],
        propiedades: ["codigoTipoReporte", "arqueo", "codigoReporte", "anio", "numeroSemana", "semana", "nombreReporte", "estado", "fechaIngStr", "nombreControlador", "nombreAccion", "pdf", "excel", "web", "tipo","deshabilitarCheck"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        reporte: true,
        funcionreporte: "Reporte",
        paginar: true,
        excel: true,
        funcionexcel: "Reporte",
        excelvalue: "excel",
        web: true,
        webvalue: "web",
        paginar: true,
        check: true,
        funcioncheck: "ExcluirCeros",
        checkheader: "Excluir Ceros",
        checkid: "excluirCeros",
        disablecheck: "deshabilitarCheck",
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "visible": false
            }, {
                "targets": [1],
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
                "targets": [4],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [9],
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
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [15],
                "visible": false
            }, {
                "targets": [16],
                "className": "dt-body-center",
                "visible": true
            }],
        slug: "codigoReporte"
    }
    pintar(objConfiguracion);
}


function GenerarPdfReporte(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-reporte', function () {
        let rowIdx = table.row(this).index();
        let codigoTipoReporte = table.cell(rowIdx, 0).data();
        let arqueo = table.cell(rowIdx, 1).data();
        let codigoReporte = table.cell(rowIdx, 2).data();
        let anioOperacion = table.cell(rowIdx, 3).data();
        let semanaOperacion = table.cell(rowIdx, 4).data();

        let nombreControlador = table.cell(rowIdx, 9).data();
        let nombreAccion = table.cell(rowIdx, 10).data();
        let excluirCeros = table.cell(rowIdx, 16).nodes().to$().find('input:checkbox').prop('checked') == true ? "1" : "0";
        fetchGet(nombreControlador + "/" + nombreAccion + "/?anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion + "&codigoReporte=" + codigoReporte + "&arqueo=" + arqueo + "&excluirCeros=" + excluirCeros, "pdf", function (data) {
            var file = new Blob([data], { type: 'application/pdf' });
            var fileURL = URL.createObjectURL(file);
            window.open(fileURL, "reporte" + codigoTipoReporte + codigoReporte);
        });
    });
}

function GenerarExcelReporte(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-excel', function () {
        let rowIdx = table.row(this).index();
        let codigoTipoReporte = table.cell(rowIdx, 0).data();
        let arqueo = table.cell(rowIdx, 1).data();
        let codigoReporte = table.cell(rowIdx, 2).data();
        let anioReporte = table.cell(rowIdx, 3).data();
        let semanaReporte = table.cell(rowIdx, 4).data();
        let excluirCeros = table.cell(rowIdx, 16).nodes().to$().find('input:checkbox').prop('checked') == true ? "1" : "0";

        document.getElementById("uiExportarExcel").href = "/Reportes/ExportarExcel/?codigoTipoReporte=" + codigoTipoReporte + "&anioReporte=" + anioReporte + "&semanaReporte=" + semanaReporte + "&codigoReporte=" + codigoReporte + "&arqueo=" + arqueo + "&excluirCeros=" + excluirCeros;
        document.getElementById("uiExportarExcel").click();
    });
}

function clickCheckExcluirCeros(obj) {


}

function buscarReporteCortesCajaChica() {
    let errores = ValidarDatos("frmBusquedaCortesCajaChica")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let codigoCajaChica = parseInt(document.getElementById("uiFiltroCajaChica").value);
    let anioOperacionStr = document.getElementById("uiFiltroAnioOperacion").value;
    let anioOperacion = 0;
    if (anioOperacionStr == "") {
        anioOperacion = parseInt(document.getElementById("uiFiltroAnioOperacion").value);
    }
    ListarCortesCajaChica(anioOperacion, codigoCajaChica);
}

function ListarCortesCajaChica(anioOperacion, codigoCajaChica) {
    let objConfiguracion = {
        url: "Reportes/GetCortesCajaChica/?anioOperacion=" + anioOperacion.toString() + "&codigoCajaChica=" + codigoCajaChica.toString(),
        cabeceras: ["codigoTipoReporte", "Arqueo", "Código Reporte", "Año", "Semana", "Caja Chica", "Estado", "Fecha Generación", "Nombre Controlador", "Nombre Accion", "PDF", "EXCEL", "WEB", "Tipo"],
        propiedades: ["codigoTipoReporte", "arqueo", "codigoReporte", "anioOperacion", "semanaOperacion", "nombreCajaChica", "estado", "fechaIngStr", "nombreControlador", "nombreAccion", "pdf", "excel", "web", "tipo"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        reporte: true,
        funcionreporte: "Reporte",
        paginar: true,
        excel: true,
        funcionexcel: "Reporte",
        excelvalue: "excel",
        web: true,
        webvalue: "web",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "visible": false
            }, {
                "targets": [1],
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
                "targets": [4],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [8],
                "visible": false
            }, {
                "targets": [9],
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
                "className": "dt-body-center",
                "visible": true
            }],
        slug: "codigoReporte"
    }
    pintar(objConfiguracion);
}


function ListarReportesCompromisoFiscal(anioOperacion) {
    let objConfiguracion = {
        url: "Reportes/GetReportesCompromisoFiscal/?anioOperacion=" + anioOperacion.toString(),
        cabeceras: ["nombreControlador","nombreAccion", "Año", "Semana", "Periodo", "Monto"],
        propiedades: ["nombreControlador","nombreAccion", "anioOperacion", "semanaOperacion", "periodo","montoTotal"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        reporte: true,
        funcionreporte: "ReporteDetalleCompromisoFiscal",
        paginar: true,
        excel: true,
        funcionexcel: "ReporteDetalleCompromisoFiscal",
        excelvalue: "excel",
        web: true,
        webvalue: "web",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "visible": false
            }, {
                "targets": [1],
                "visible": false
            }, {
                "targets": [2],
                "visible": true,
                "className": "dt-body-center"
            }, {
                "targets": [3],
                "visible": true,
                "className": "dt-body-center"
            }, {
                "targets": [5],
                "className": "dt-body-right",
                "visible": true
            }],
        slug: "anioOperacion"
    }
    pintar(objConfiguracion);
}

function GenerarPdfReporteDetalleCompromisoFiscal(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-reporte', function () {
        let rowIdx = table.row(this).index();
        let nombreControlador = table.cell(rowIdx, 0).data();
        let nombreAccion = table.cell(rowIdx, 1).data();
        let anioOperacion = table.cell(rowIdx, 2).data();
        let semanaOperacion = table.cell(rowIdx, 3).data();
        fetchGet(nombreControlador + "/" + nombreAccion + "/?anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion, "pdf", function (data) {
            var file = new Blob([data], { type: 'application/pdf' });
            var fileURL = URL.createObjectURL(file);
            window.open(fileURL, "EPrescription");
        });
    });
}


/* Filtro para los Reportes de Ventas Facturadas, Vales de Salida */
function fillComboEmpresasComercializadoras() {
    fetchGet("Empresa/GetAllEmpresasComercializadoras", "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            FillComboUnicaOpcion("uiFiltroEmpresa", "-1", "-- No Existen empresas -- ");
        } else {
            FillCombo(rpta, "uiFiltroEmpresa", "codigoQsystem", "nombreComercial", "- seleccione -", "-1");
        }
    })
}


function mostrarVentasFacturadas() {


}

function mostrarValesDeSalida() {


}

async function generarExcelVentasFacturadas() {
    let errores = ValidarDatos("frmBusqueda")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let codigoEmpresa = document.getElementById("uiFiltroEmpresa").value;
    let fechaInicio = document.getElementById("uiFechaInicio").value;
    let fechaFin = document.getElementById("uiFechaFin").value;

    fetchGetDownload("ReportesQSystems/ExportarExcelVentaPorRangoFechaDetallado/?codigoEmpresa=" + codigoEmpresa + "&fechaInicio=" + fechaInicio + "&fechaFin=" + fechaFin, function (data) {
        var file = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        var fileURL = URL.createObjectURL(file);
        window.open(fileURL, "EPrescription");
    }).finally(() => {
        document.getElementById('divLoading').style.display = 'none';
    });
}

async function generarExcelValesSalida() {
    let errores = ValidarDatos("frmBusqueda")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let codigoEmpresa = document.getElementById("uiFiltroEmpresa").value;
    let fechaInicio = document.getElementById("uiFechaInicio").value;
    let fechaFin = document.getElementById("uiFechaFin").value;

    fetchGetDownload("ReportesQSystems/ExportarExcelValesSalidaPorRangoFechaDetallado/?codigoEmpresa=" + codigoEmpresa + "&fechaInicio=" + fechaInicio + "&fechaFin=" + fechaFin, function (data) {
        var file = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        var fileURL = URL.createObjectURL(file);
        window.open(fileURL, "EPrescription");
    }).finally(() => {
        document.getElementById('divLoading').style.display = 'none';
    });
}


// cookies
/*function deleteCookie() {
    var cook = getCookie('ExcelDownloadFlag');
    if (cook != "") {
        //document.cookie = "ExcelDownloadFlag=; Path = /; expires=Thu, 01 Jan 1970 00:00:00 UTC";
        document.cookie = "ExcelDownloadFlag=; Path = /; expires=Thu, 01 Jan 1970 00: 00: 01 GMT";
    }
}

function IsCookieValid() {
    var cook = getCookie('ExcelDownloadFlag');
    return cook != '';
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}*/