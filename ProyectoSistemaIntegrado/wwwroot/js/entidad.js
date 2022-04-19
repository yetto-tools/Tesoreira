window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    if (nameController == "Entidad") {
        switch (nameAction) {
            case "AsignacionOperacionContable":
                FillComboCategoriaEntidad();
                ListarEntidadesGenericasParaConfiguracion(-1);
                break;
            default:
                break;
        }// fin switch
    }// fin if
}
function FiltrarEntidades() {
    let codigoCategoriaEntidad = parseInt(document.getElementById("uiFiltroCodigoCategoriaEntidad").value);
    ListarEntidadesGenericasParaConfiguracion(codigoCategoriaEntidad);
}


function FillComboCategoriaEntidad() {
    fetchGet("EntidadCategoria/GetCategoriaParaAsignacionDeOperacion", "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            MensajeError("Error en la carga de categoría de entidades");
        } else {
            FillCombo(rpta, "uiFiltroCodigoCategoriaEntidad", "codigoCategoriaEntidad", "nombreCategoriaEntidad", "- seleccione -", "-1");
        }
    })
}

function FillComboOperacionesConfigEntidad(idControl) {
    fetchGet("Operacion/GetOperacionesParaAsignacionAEntidadesGenericas", "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            MensajeError("Error en la carga de las operaciones");
        } else {
            FillCombo(rpta, idControl, "codigoOperacion", "nombre", "- seleccione -", "-1");
        }
    })
}


function ListarEntidadesGenericasParaConfiguracion(codigoCategoriaEntidad) {
    let objConfigEntidad = {
        url: "Entidad/GetEntidadesGenericasConfiguracion/?codigoCategoriaEntidad=" + codigoCategoriaEntidad.toString(),
        cabeceras: ["Código", "Nombre Entidad", "codigoCategoriaEntidad", "Categoría", "codigoOperacion", "Operación","Editar"],
        propiedades: ["codigoEntidad", "nombre","codigoCategoriaEntidad", "nombreCategoriaEntidad", "codigoOperacion", "operacion","permisoEditar"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        editar: true,
        funcioneditar: "ConfiguracionEntidad",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [2],
                "visible": false
            }, {
                "targets": [4],
                "visible": false
            }, {
                "targets": [6],
                "visible": false
            }],
        slug: "codigoEntidad"
    }
    pintar(objConfigEntidad);
}


function EditarConfiguracionEntidad() {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-editar', function () {
        let rowIdx = table.row(this).index();
        let codigoEntidad = table.cell(rowIdx, 0).data();
        let nombreEntidad = table.cell(rowIdx, 1).data();
        let categoriaEntidad = table.cell(rowIdx, 3).data();
        let codigoOperacion = table.cell(rowIdx, 4).data();
        setI("uiTitlePopupEditConfiguracion", "Edición de Configuración de Entidad");
        document.getElementById("ShowPopupEditConfiguracion").click();
        set("uiEditCodigoEntidad", codigoEntidad);
        set("uiEditNombreEntidad", nombreEntidad);
        set("uiEditNombreCategoriaEntidad", categoriaEntidad);
        FillComboOperacionesConfigEntidad("uiEditCodigoOperacion");
    });
}

function ActualizarConfiguracionEntidad(){
    let errores = ValidarDatos("frmEditConfiguracion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmEditConfiguracion");
    let frm = new FormData(frmGuardar);

    Confirmacion("Actualización de Operación Asignada a Entidad", "¿Esta seguro de la actualización?", function (rpta) {
        fetchPost("Entidad/ActualizarOperacionEntidad", "text", frm, function (data) {
            if (data == "OK") {
                FiltrarEntidades();
            } else {
                MensajeError(data);
            }
            document.getElementById("uiClosePopupEditConfiguracion").click();
        });
    });
}

