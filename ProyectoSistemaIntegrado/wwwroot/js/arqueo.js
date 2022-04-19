window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    if (nameController == "Arqueo") {
        switch (nameAction) {
            case "Index":
                listarArqueos();
                break;
            default:
                break;
        }// fin switch
    }// fin if
}

//reporte: true,
function listarArqueos() {
    let objConfiguracion = {
        url: "Arqueo/GetListaArqueos",
        cabeceras: ["Código Reporte", "Año", "Número Semana", "Semana", "Estado", "Generado por", "fecha Generación", "bloqueado", "Editar", "Anular", "Arqueo"],
        propiedades: ["codigoReporte", "anio", "numeroSemana", "semana", "estado", "usuarioIng", "fechaIngStr", "bloqueado", "permisoEditar", "permisoAnular", "permisoArqueo"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        eliminar: true,
        eliminarreporte: true,
        arqueo: true,
        slug: "codigoReporte",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {   "targets": [0],
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
                "targets": [7],
                "visible": false
            }, {
                "targets": [8],
                "visible": false
            }, {
                "targets": [9],
                "visible": false
            }, {
                "targets": [10],
                "visible": false
            }]
    }
    pintar(objConfiguracion);
}

function GenerarArqueo(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-arqueo', function () {
        let rowIdx = table.row(this).index();
        let anioOperacion = parseInt(table.cell(rowIdx, 1).data());
        let semanaOperacion = parseInt(table.cell(rowIdx, 2).data());
        fetchGet("Arqueo/GenerarArqueo/?anio=" + anioOperacion.toString() + "&numeroSemana=" + semanaOperacion.toString(), "text", function (rpta) {
            if (rpta == "OK") {
                listarArqueos();
            } else {
                MensajeError("Error en la generación del arqueo", rpta);
                return;
            }
        })
    });

}

/*function Eliminar(codigoReporte) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-eliminar', function () {
        let rowIdx = table.row(this).index();
        let anioOperacion = parseInt(table.cell(rowIdx, 1).data());
        let semanaOperacion = parseInt(table.cell(rowIdx, 2).data());
        fetchGet("CorteCajaSemanal/EliminarReporteSemanal/?codigoReporte=" + codigoReporte.toString() + "&anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString(), "text", function (rpta) {
            if (rpta != "OK") {
                MensajeError("Error al eliminar el reporte " + codigoReporte.toString());
                return;
            } else {
                listarReportesGeneracion();
            }
        })
    });
}*/


/*function GenerarReporte(slug) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-generar', function () {
        let rowIdx = table.row(this).index();
        let anioOperacion = parseInt(table.cell(rowIdx, 1).data());
        let semanaOperacion = parseInt(table.cell(rowIdx, 2).data());
        let checkTipoArqueo = table.cell(rowIdx, 10).nodes().to$().find('.option-check').prop("checked");
        fetchGet("CorteCajaSemanal/GenerarReporteSemanal/?anio=" + anioOperacion.toString() + "&numeroSemana=" + semanaOperacion.toString(), "text", function (rpta) {
            if (rpta == "OK") {
                listarReportesGeneracion();
            } else {
                MensajeError("Error al generar el reporte semanal ", rpta);
                return;
            }
        })
    });
}


function listarReportesConsulta(anio) {
    let objConfiguracion = {
        url: "CorteCajaSemanal/GetReportesSemanalesCaja/?anio=" + anio.toString(),
        cabeceras: ["Código Reporte", "Año", "Número Semana", "Semana", "Estado", "Código Estado", "Generado por", "fecha Generación"],
        propiedades: ["codigoReporte", "anio", "numeroSemana", "semana", "codigoEstado", "estado", "usuarioIng", "fechaIngStr"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        reporte: true,
        slug: "codigoReporte",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [4],
                "visible": false
            }]
    }
    pintar(objConfiguracion);
}

function BuscarReportesSemanales() {
    let errores = ValidarDatos("frmBusquedaReportes")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let anio = parseInt(document.getElementById("uiFiltroAnio").value);
    listarReportesConsulta(anio);
}


function clickAceptarReporteSemanalGenerado(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', 'tr', 'input:button', function () {
        let rowIdx = table.row(this).index();
        let anioOperacion = parseInt(table.cell(rowIdx, 1).data());
        let semanaOperacion = parseInt(table.cell(rowIdx, 2).data());
        Confirmacion("Visto Bueno Reporte Generado", "¿Reporte Válido?", function (rpta) {
            fetchGet("CorteCajaSemanal/AceptarReporteGenerado/?codigoReporte=" + obj.toString() + "&anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString(), "text", function (data) {
                if (data != "0") {
                    MensajeError(data);
                    return;
                } else {
                    listarReportesGeneracion();
                }
            })
        })
    });
}*/

/*
function GenerarPdf(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-reporte', function () {
        let rowIdx = table.row(this).index();
        let codigoReporte = parseInt(table.cell(rowIdx, 0).data());
        fetchGet("CorteCajaSemanal/ViewReporteSemanalCajaPDF/?codigoReporte=" + codigoReporte.toString(), "pdf", function (data) {
            var file = new Blob([data], { type: 'application/pdf' });
            var fileURL = URL.createObjectURL(file);
            window.open(fileURL, "EPrescription");
        })
    });
   
}
*/

/*function MostrarReportesGeneradosParaElVistoBuenoDeContabilidad() {
    let objConfiguracion = {
        url: "ReportesTesoreria/GetReportesSemanalesCajaParaVistoBueno",
        cabeceras: ["Código", "Año", "Número Semana", "Semana", "Estado", "Generado por", "fecha Generación", "bloqueado", "Editar", "Anular"],
        propiedades: ["codigoReporte", "anio", "numeroSemana", "semana", "estado", "usuarioIng", "fechaIngStr", "bloqueado", "permisoEditar", "permisoAnular"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        reporte: true,
        slug: "codigoReporte",
        bloqueado: "bloqueado",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [7],
                "visible": false
            }, {
                "targets": [8],
                "visible": false
            }, {
                "targets": [9],
                "visible": false
            }],
        aceptar: true,
        funcionaceptar: "AceptarReporteTesoreria"
    }
    pintar(objConfiguracion);
}*/

/*
function clickAceptarReporteTesoreria(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-aceptar', function () {
        let rowIdx = table.row(this).index();
        let codigoReporte = parseInt(table.cell(rowIdx, 0).data());
        let anioOperacion = parseInt(table.cell(rowIdx, 1).data());
        let semanaOperacion = parseInt(table.cell(rowIdx, 2).data());
        Confirmacion("Visto Bueno de Reporte de Tesorería", "¿Reporte Válido?", function (rpta) {
            fetchGet("CorteCajaSemanal/AceptarReportePorContabilidad/?codigoReporte=" + codigoReporte.toString() + "&anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString(), "text", function (data) {
                if (data != "0") {
                    MensajeError(data);
                    return;
                } else {
                    MostrarReportesGeneradosParaElVistoBuenoDeContabilidad();
                }
            })
        })
    });
}*/


