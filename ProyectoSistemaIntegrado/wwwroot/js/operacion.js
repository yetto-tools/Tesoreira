window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    if (nameController == "Operacion") {
        switch (nameAction) {
            case "Index":
                mostrarConfiguracionOperaciones();
                break;
            default:
                break;
        }// fin switch
    }// fin if
}

function mostrarConfiguracionOperaciones() {
    let objConfigTransaccion = {
        url: "Operacion/GetAllOperacionesConfiguracion",
        cabeceras: ["Código", "Nombre Operación", "Nombre Operación (Caja)", "codigoCategoriaOperacion", "Categoría", "Descripción", "Habilitado Caja Tesorería", "codigoEstado", "Estado", "codigoConcepto", "Concepto", "Caja Chica", "Incluir Config Entidad Genética", "codigoTipoOperacion", "Tipo Operación", "Grupo 01", "Grupo 02", "Grupo 03"],
        propiedades: ["codigoOperacion", "nombre", "nombreReporteCaja","codigoCategoriaOperacion","categoriaOperacion","descripcion","habilitarParaCajaTesoreria","codigoEstado","estado","codigoConcepto","concepto","aplicaCajaChica","incluirEnConfiguracionEntidadGenerica","codigoTipoOperacion","tipoOperacion","grupo01","grupo02","grupo03"],
        divContenedorTabla: "divContenedorTabla",
        idtabla: "tabla-personas",
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
                "targets": [5],
                "visible": false
            }, {
                "targets": [6],
                "className": "dt-body-center",
                "visible": true
            },{
                "targets": [7],
                "visible": false
            }, {
                "targets": [9],
                "visible": false
            }, {
                "targets": [11],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [12],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [13],
                "visible": false
            }, {
                "targets": [15],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [16],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [17],
                "className": "dt-body-center",
                "visible": true
            }],
        autoWidth: false,
        divPintado: "divTabla",
        paginar: true,
        slug: "codigoOperacion"

    }
    pintar(objConfigTransaccion);
}