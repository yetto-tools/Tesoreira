window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    if (nameController == "Notificacion") {
        switch (nameAction) {
            case "Index":
                MostrarConfiguracionNotificacion();
                break;
            default:
                break;
        }// fin switch
    }// fin if
}

//min-width
//max-width
//"width": "300"
//"width": "400px"
function intelligenceSearch() {
    let objConfigTransaccion = {
        url: "Persona/GetAllPersonas/?noIncluidoEnPlanilla=0",
        cabeceras: ["CUI", "Nombre Completo", "Correo Electrónico"],
        propiedades: ["cui", "nombreCompleto", "correoElectronico"],
        divContenedorTabla: "divContenedorTablaPersonas",
        idtabla: "tabla-personas",
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [1],
                "visible": true
            }, {
                "targets": [2],
                "visible": true
            }],
        autoWidth: false,
        divPintado: "divTablaPersonas",
        radio: true,
        paginar: true,
        eventoradio: "Personas",
        slug: "cui"
    }
    pintar(objConfigTransaccion);
}


function BusquedaPersona() {
    intelligenceSearch();
    setI("uiTitlePopupPersonas", "Búsqueda para Notificación");
    document.getElementById("ShowPopupPersonas").click();
    set("uiCui", "");
    set("uiNombreCompleto", "");
    set("uiCorreoElectronico", "");
    fillCombosTipoNotificacion();
}

function fillCombosTipoNotificacion() {
    fetchGet("TipoNotificacion/GetAllTipoNotificacion", "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            FillComboUnicaOpcion("uiTipoNotificacion", "-1", "-- No Existen tipos de notificación -- ");
        } else {
            FillCombo(rpta, "uiTipoNotificacion", "codigoTipoNotificacion", "nombre", "- seleccione -", "-1");
        }
    })
}


function getDataRowRadioPersonas(obj) {
    let table = $('#tabla-personas').DataTable();
    $('#tabla-personas tbody').on('change', 'tr', 'input:radio', function () {
        let rowIdx = table.row(this).index();
        // Incluir el radioButton al inicio, por eso se comienza por la columna 1
        let cui = table.cell(rowIdx, 1).data();
        let nombreCompleto = table.cell(rowIdx, 2).data();
        let correoElectronico = table.cell(rowIdx, 3).data();
        set("uiCui", cui);
        set("uiNombreCompleto", nombreCompleto);
        set("uiCorreoElectronico", correoElectronico);
    });
}


function GuardarConfiguracionNotificacion() {
    let errores = ValidarDatos("frmNewConfiguracion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmNewConfiguracion");
    let frm = new FormData(frmGuardar);
    Confirmacion("Nueva Configuración", "¿Está seguro?", function (rpta) {
        fetchPost("Notificacion/GuardarConfiguracion", "text", frm, function (data) {
            document.getElementById("uiClosePopupPersonas").click();
            if (data == "OK") {
                MostrarConfiguracionNotificacion();
            } else {
                MensajeError(data);
            }
        });
    });
}

function MostrarConfiguracionNotificacion() {
    let objConfiguracion = {
        url: "Notificacion/GetAllConfiguraciones",
        cabeceras: ["codigoTipoNotificacion","Tipo Notificación","CUI","Nombre", "Correo Electrónico","codigoEstado","Estado","Anular"],
        propiedades: ["codigoTipoNotificacion","tipoNotificacion","cui","nombreCompleto", "correoElectronico","codigoEstado","estado","permisoAnular"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        eliminar: true,
        funcioneliminar: "ConfiguracionNotificacion",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "visible": false
            }, {
                "targets": [5],
                "visible": false
            }, {
                "targets": [7],
                "visible": false
            }],
        slug: "codigoTraslado"
    }
    pintar(objConfiguracion);
}


function EliminarConfiguracionNotificacion(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-eliminar', function () {
        let rowIdx = table.row(this).index();
        let codigoTipoNotificacion = table.cell(rowIdx, 0).data();
        let cui = table.cell(rowIdx, 2).data();
        var frm = new FormData();
        frm.append("cui", cui);
        frm.append("codigoTipoNotificacion", codigoTipoNotificacion);
        Confirmacion("Eliminación de Configuración", "¿Está seguro de la eliminación?", function (rpta) {
            fetchPost("Notificacion/EliminarConfiguracion", "text", frm, function (rpta) {
                if (rpta != "OK") {
                    MensajeError(rpta);
                } else {
                    MostrarConfiguracionNotificacion();
                }
            })
        });
    });
}