window.onload = function () {
    ListarConfiguracion();
}

function ListarConfiguracion() {
    let arrayDate = getFechaSistema();
    let anio = arrayDate[0]["anio"];
    set("uiFiltroAnio", anio.toString());
    MostrarConfiguraciones(anio);
}

function BuscarConfiguraciones() {
    let anioStr = document.getElementById("uiFiltroAnio").value;
    if (!/^[0-9]+$/.test(anioStr)) {
        MensajeError("Año con carácter no válido");
        return;
    }
    if (anioStr.length != 4) {
        MensajeError("Año no válido");
        return;
    }
    let anio = parseInt(anioStr);
    MostrarConfiguraciones(anio);
}

function MostrarConfiguraciones(anio) {
    let objConfiguracion = {
        url: "ConfiguracionSueldoIndirecto/GetConfiguracionSueldoIndirecto/?anio=" + anio.toString(),
        cabeceras: ["Año","Mes", "Mes", "Monto", "Código Estado", "estado"],
        propiedades: ["anio", "mes", "nombreMes", "monto", "codigoEstado", "estado"],
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
                "targets": [3],
                "className": "dt-body-right"
            }, {
                "targets": [4],
                "visible": false
            }],
        editar: true,
        funcioneditar: "Configuracion",
        slug: "anio"
    }
    pintar(objConfiguracion);
}


function NuevaConfiguracion() {
    setI("uiTitlePopupNuevaConfiguracion", "Nueva Configuración");
    let anio = parseInt(document.getElementById("uiFiltroAnio").value);
    document.getElementById("uiNewMes").selectedIndex = 0;
    set("uiNewAnio", anio.toString());
    set("uiNewMonto", "");
}


function GuardarConfiguracion() {
    let errores = ValidarDatos("frmNewConfiguracion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let frmGuardar = document.getElementById("frmNewConfiguracion");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("ConfiguracionSueldoIndirecto/GuardarConfiguracion", "text", frm, function (data) {
            if (data == "OK") {
                document.getElementById("uiClosePopupNuevaConfiguracion").click();
                BuscarConfiguraciones();
            } else {
                MensajeError(data);
            }
        });
    });
}

function EditarConfiguracion(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-editar', function () {
        let rowIdx = table.row(this).index();

        let anio = table.cell(rowIdx, 0).data();
        let mes = table.cell(rowIdx, 1).data();
        let nombreMes = table.cell(rowIdx, 2).data();
        let monto = table.cell(rowIdx, 3).data();

        setI("uiTitlePopupEditConfiguracion", "Edición de Configuración");
        document.getElementById("ShowPopupEditConfiguracion").click();
        set("uiEditAnio", anio);
        set("uiEditMes", mes);
        set("uiEditNombreMes", nombreMes);
        set("uiEditMonto", monto);
    });
}


function ActualizarConfiguracion() {
    let errores = ValidarDatos("frmEditConfiguracion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let frmGuardar = document.getElementById("frmEditConfiguracion");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("ConfiguracionSueldoIndirecto/ActualizarConfiguracion", "text", frm, function (data) {
            if (data == "OK") {
                document.getElementById("uiClosePopupEditConfiguracion").click();
                BuscarConfiguraciones();
            } else {
                MensajeError(data);
            }
        });
    });

}