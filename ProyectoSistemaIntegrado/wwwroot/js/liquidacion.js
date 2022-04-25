window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    let codigoTraslado = url1.searchParams.get("codigoTraslado");
    let anioOperacion = url1.searchParams.get("anioOperacion");
    let semanaOperacion = url1.searchParams.get("semanaOperacion");
    if (nameController == "Liquidacion") {
        switch (nameAction) {
            case "Index":
                let arrayDate = getFechaSistema();
                let anioActual = arrayDate[0]["anio"];
                document.getElementById("uiFiltroAnio").value = anioActual.toString();
                MostrarTrasladosLiquidacion(anioActual);
                break;
            case "Traslado":
                MostrarGeneracionTrasladosParaLiquidacion();
                break;
            case "Detalle":
                MostrarDetalleTrasladoLiquidacion(codigoTraslado, anioOperacion, semanaOperacion);
                break;
            case "DetalleConsulta":
                MostrarDetalleTrasladoLiquidacion(codigoTraslado, anioOperacion, semanaOperacion);
                break;
            default:
                break;
        }// fin switch
    }// fin if
}

function MostrarGeneracionTrasladosParaLiquidacion() {
    let objConfiguracion = {
        url: "Liquidacion/GetTrasladosParaLiquidacion",
        cabeceras: ["Código Traslado", "Año", "Semana", "Periodo", "Cantidad", "Monto", "codigoEstadoTraslado","Estado","bloqueado","permisoAnular"],
        propiedades: ["codigoTraslado", "anioOperacion", "semanaOperacion", "periodo","cantidad","montoTotal","codigoEstadoTraslado","estadoTraslado","bloqueado","permisoAnular"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        generar: true,
        eliminar: true,
        funcioneliminar: "TrasladoGenerado",
        paginar: true,
        verdetalle: true,
        funciondetalle: "TrasladoLiquidacion",
        displaydecimals: ["montoTotal"],
        reporte: true,
        funcionreporte: "TrasladoLiquidacion",
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "className": "dt-body-center"
            }, {
                "targets": [1],
                "className": "dt-body-center"
            }, {
                "targets": [2],
                "className": "dt-body-center"
            }, {
                "targets": [4],
                "className": "dt-body-center"
            }, {
                "targets": [5],
                "className": "dt-body-right"
            }, {
                "targets": [6],
                "visible": false
            }, {
                "targets": [8],
                "visible": false
            }, {
                "targets": [9],
                "visible": false
            }],
        slug: "codigoTraslado",
        aceptar: true,
        funcionaceptar: "TrasladarParaLiquidacion"
    }
    pintar(objConfiguracion);
}

function VerDetalleTrasladoLiquidacion(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-detalle', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        let anioOperacion = table.cell(rowIdx, 1).data();
        let semanaOperacion = table.cell(rowIdx, 2).data();
        Redireccionar("Liquidacion", "Detalle/?codigoTraslado=" + codigoTraslado + "&anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion);
    });
}

function GenerarReporte() {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-generar', function () {
        let rowIdx = table.row(this).index();
        let anioOperacion = parseInt(table.cell(rowIdx, 1).data());
        let semanaOperacion = parseInt(table.cell(rowIdx, 2).data());
        var frm = new FormData();
        frm.append("anioOperacion", anioOperacion.toString());
        frm.append("semanaOperacion", semanaOperacion.toString());
        fetchPost("Liquidacion/GenerarTraslado", "text", frm, function (rpta) {
            if (rpta != "OK") {
                MensajeError("Error al generar el traslado para liquidación");
            } else {
                Redireccionar("Liquidacion", "Traslado");
            }
        })
    });
}

function EliminarTrasladoGenerado(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-eliminar', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        var frm = new FormData();
        frm.append("codigoTraslado", codigoTraslado);
        fetchPost("Liquidacion/AnularTraslado", "text", frm, function (rpta) {
            if (rpta != "OK") {
                MensajeError(rpta);
            } else {
                MostrarGeneracionTrasladosParaLiquidacion();
            }
        })
    });
}

function GenerarPdfTrasladoLiquidacion(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-reporte', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        let anioOperacion = table.cell(rowIdx, 1).data();
        let semanaOperacion = table.cell(rowIdx, 2).data();
        let bloqueado = parseInt(table.cell(rowIdx, 8).data());
        if (bloqueado == 1) {
            fetchGet("Liquidacion/ViewReporteTrasladoLiquidacion/?codigoTraslado=" + codigoTraslado + "&anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion, "pdf", function (data) {
                var file = new Blob([data], { type: 'application/pdf' });
                var fileURL = URL.createObjectURL(file);
                window.open(fileURL, "EPrescription");
            })
        }
    });
}

function clickTrasladarParaLiquidacion(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-aceptar', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        var frm = new FormData();
        frm.append("codigoTraslado", codigoTraslado);
        Confirmacion("Traslado para liquidación", "¿Está seguro de realizar el traslado para liquidación?", function (rpta) {
            fetchPost("Liquidacion/TrasladarParaLiquidacion", "text", frm, function (rpta) {
                if (rpta != "OK") {
                    MensajeError(rpta);
                } else {
                    MostrarGeneracionTrasladosParaLiquidacion();
                }
            });
        });
    });

}

function MostrarDetalleTrasladoLiquidacion(codigoTraslado, anioOperacion, semanaOperacion) {
    let objConfiguracion = {
        url: "Liquidacion/GetDetalleTrasladoLiquidacion/?codigoTraslado=" + codigoTraslado + "&anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion,
        cabeceras: ["Código Transacción", "codigoTransaccionAnt", "codigoTraslado", "codigoEmpresa", "Empresa","Fecha Operación","Día Operación", "Código Vendedor","Nombre Vendedor", "Ruta", "codigoCanalVenta", "Canal Venta", "Monto", "codigoTipoTraslado", "Tipo Traslado", "codigoEstado", "Estado", "Creado por", "Fecha Creación"],
        propiedades: ["codigoTransaccion", "codigoTransaccionAnt", "codigoTraslado", "codigoEmpresa", "nombreEmpresa","fechaOperacionStr","nombreDia","codigoVendedor","nombreVendedor", "ruta", "codigoCanalVenta", "canalVenta", "monto", "codigoTipoTraslado", "tipoTraslado", "codigoEstado","estado","usuarioIng","fechaIngStr"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        displaydecimals: ["monto"],
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
                "targets": [2],
                "visible": false
            }, {
                "targets": [3],
                "visible": false
            }, {
                "targets": [7],
                "visible": false
            }, {
                "targets": [9],
                "visible": true,
                "className": "dt-body-center"
            }, {
                "targets": [10],
                "visible": false
            }, {
                "targets": [12],
                "className": "dt-body-right"
            }, {
                "targets": [13],
                "visible": false
            }, {
                "targets": [15],
                "visible": false
            }],
        slug: "codigoTransaccion"
    }
    pintar(objConfiguracion);
}


function BuscarTrasladosLiquidacion() {
    let errores = ValidarDatos("frmBusquedaTraslados")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let anio = parseInt(document.getElementById("uiFiltroAnio").value);
    MostrarTrasladosLiquidacion(anio);
}

function MostrarTrasladosLiquidacion(anioOperacion) {
    let objConfiguracion = {
        url: "Liquidacion/GetTrasladosLiquidacionConsulta/?anioOperacion=" + anioOperacion.toString(),
        cabeceras: ["Código Traslado", "Año", "Semana", "Periodo", "Cantidad", "Monto", "codigoEstadoTraslado", "Estado","bloqueado"],
        propiedades: ["codigoTraslado", "anioOperacion", "semanaOperacion", "periodo", "cantidad", "montoTotal", "codigoEstadoTraslado", "estadoTraslado","bloqueado"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        verdetalle: true,
        funciondetalle: "TrasladoLiquidacionConsulta",
        displaydecimals: ["montoTotal"],
        reporte: true,
        funcionreporte: "TrasladoLiquidacion",
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "className": "dt-body-center"
            }, {
                "targets": [1],
                "className": "dt-body-center"
            }, {
                "targets": [2],
                "className": "dt-body-center"
            }, {
                "targets": [4],
                "className": "dt-body-center"
            }, {
                "targets": [5],
                "className": "dt-body-right"
            }, {
                "targets": [6],
                "visible": false
            }, {
                "targets": [8],
                "visible": false
            }],
        slug: "codigoTraslado"
    }
    pintar(objConfiguracion);
}

function VerDetalleTrasladoLiquidacionConsulta(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-detalle', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        let anioOperacion = table.cell(rowIdx, 1).data();
        let semanaOperacion = table.cell(rowIdx, 2).data();
        Redireccionar("Liquidacion", "DetalleConsulta/?codigoTraslado=" + codigoTraslado + "&anioOperacion=" + anioOperacion + "&semanaOperacion=" + semanaOperacion);
    });
}