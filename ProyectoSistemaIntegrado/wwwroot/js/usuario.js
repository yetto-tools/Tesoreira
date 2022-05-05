window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    if (nameController == "Usuario") {
        switch (nameAction) {
            case "Index":
                listarUsuarios();
                break;
            case "New":
                //listarEmpresas();
                break;
            case "Edit":
                let idUsuario = url1.searchParams.get("idUsuario");
                setDataUsuario(idUsuario);
                break;
            case "CambioPassword":
                break;
            case "CambioPasswordAdmin":
                listarUsuariosCambioPassword();
                break;
            default:
                break;
        }// fin switch
    }// fin if
}


function listarUsuarios() {
    let objConfiguracion = {
        url: "Usuario/ListarUsuarios",
        cabeceras: ["Id Usuario", "nombre usuario","Super Administrador","SuperAdmin","Estado"],
        propiedades: ["idUsuario", "nombreUsuario","superAdmin","esSuperAdmin", "estado"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        editar: true,
        funcioneditar: "Usuario",
        eliminar: true,
        funcioneliminar: "Usuario",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [2],
                "visible": false
            }],
        slug: "idUsuario"
    }
    pintar(objConfiguracion);
}


function EditarUsuario(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-editar', function () {
        let rowIdx = table.row(this).index();
        let idUsuario = table.cell(rowIdx, 0).data()
        Redireccionar("Usuario", "Edit/?idUsuario=" + idUsuario);
    });

}

function EliminarUsuario(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-eliminar', function () {
        let rowIdx = table.row(this).index();
        let idUsuario = table.cell(rowIdx, 0).data()
        let frm = new FormData();
        frm.set("idUsuario", idUsuario);
        Confirmacion("Eliminación de Usuario", "¿Está seguro de eliminar al usuario " + idUsuario + "?", function (rpta) {
            fetchPost("Usuario/EliminarUsuario", "text", frm, function (data) {
                if (data == "OK") {
                    listarUsuarios();
                } else {
                    MensajeError(data);
                }
            });
        });
    });
}

function setDataUsuario(idUsuario) {
    fetchGet("Usuario/GetDataUsuario/?idUsuario=" + idUsuario, "json", function (rpta) {
        set("uiIdUsuario", rpta.idUsuario);
        set("uiCui", rpta.cui);
        set("uiNombreUsuario", rpta.nombreUsuario);
        document.querySelector('#uiEsSuperAdmin').checked = rpta.superAdmin == 1 ? true : false;
        mostrarPermisosroles(idUsuario);
        mostrarPermisosCajasChicas(idUsuario);
        mostrarPermisosEmpresas(idUsuario);
        mostrarPermisosReportes(idUsuario);
    });
}

function mostrarPermisosroles(idUsuario) {
    let objConfiguracion = {
        url: "Usuario/GetPermisoRoles/?idUsuario=" + idUsuario,
        cabeceras: ["Código", "Nombre Rol", "Descripción", "Asignado"],
        propiedades: ["codigoRol", "nombre", "descripcion", "asignado"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        idtabla: "tabla-roles",
        check: true,
        checkid: "asignado",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "visible": true,
                "className": "dt-body-center"
            }, {
                "targets": [3],
                "visible": false
            }],
        slug: "codigoRol"
    }
    pintar(objConfiguracion);
}

function mostrarPermisosCajasChicas(idUsuario) {
    let objConfiguracion = {
        url: "Usuario/GetPermisoCajasChicas/?idUsuario=" + idUsuario,
        cabeceras: ["Código", "Nombre Caja Chica","asignado"],
        propiedades: ["codigoCajaChica", "nombreCajaChica", "asignado"],
        divContenedorTabla: "divContenedorTablaCajasChicas",
        divPintado: "divTabla-Cajas-Chicas",
        idtabla: "tabla-cajas-chicas",
        check: true,
        checkid: "asignado",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [{
                "targets": [0],
                "visible": true,
                "className": "dt-body-center"
            }, {
                "targets": [2],
                "visible": false
            }],
        slug: "codigoEmpresa"
    }
    pintar(objConfiguracion);
}

function mostrarPermisosEmpresas(idUsuario) {
    let objConfiguracion = {
        url: "Usuario/GetPermisoEmpresas/?idUsuario=" + idUsuario,
        cabeceras: ["Código Empresa", "Nombre Empresa", "Asignado"],
        propiedades: ["codigoEmpresa", "nombreComercial", "asignado"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla-empresas",
        idtabla: "tabla-empresas",
        check: true,
        checkid: "asignado",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [{
                "targets": [0],
                "visible": true,
                "className": "dt-body-center"
            }, {
                "targets": [2],
                "visible": false
            }],
        slug: "codigoEmpresa"
    }
    pintar(objConfiguracion);
}

function mostrarPermisosReportes(idUsuario) {
    let objConfiguracion = {
        url: "Usuario/GetPermisoReportes/?idUsuario=" + idUsuario,
        cabeceras: ["Código", "Nombre Reporte", "Asignado"],
        propiedades: ["codigoTipoReporte", "nombre", "asignado"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla-reportes",
        idtabla: "tabla-reportes",
        check: true,
        checkid: "asignado",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [{
            "targets": [0],
            "visible": true,
            "className": "dt-body-center"
        }, {
            "targets": [2],
            "visible": false
        }],
        slug: "codigoTipoReporte"
    }
    pintar(objConfiguracion);
}


function intelligenceSearchPersona() {
    let objConfigTransaccion = {
        url: "Persona/GetAllPersonasSinUsuario",
        cabeceras: ["CUI", "Nombre Completo", "primerNombre","primerApellido"],
        propiedades: ["cui", "nombreCompleto", "primerNombre","primerApellido"],
        divContenedorTabla: "divContenedorTablaPersonas",
        idtabla: "tabla-personas",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [3],
                "visible": false
            }, {
                "targets": [4],
                "visible": false
            }],
        autoWidth: false,
        divPintado: "divTablaPersonas",
        radio: true,
        eventoradio: "Personas",
        slug: "cui"
    }
    pintar(objConfigTransaccion);
}



function buscarPersona() {
    let valor = get("uiCuiBusqueda");
    if (valor === "") {
        intelligenceSearchPersona();
        setI("uiTitlePopupPersonas", "Búsqueda de Personas");
        document.getElementById("ShowPopupPersonas").click();
    }
    else {
        fetchGet("Persona/BusquedaPersona/?cui=" + valor, "json", function (rpta) {
            if (rpta == null) {
                MensajeError("Error en la búsqueda de datos del CUI " + valor);
            } else {
                if (Object.keys(rpta).length > 0) {
                    set("uiCuiBusqueda", "");
                    set("uiCui", rpta.cui);
                    set("uiPrimerNombre", rpta.primerNombre);
                    set("uiPrimerApellido", rpta.primerApellido);
                    set("uiIdUsuario", rpta.primerNombre.toLowerCase() + '.' + rpta.primerApellido.toLowerCase());
                }
            }
        })
    }
}

function getDataRowRadioPersonas(obj) {
    let table = $('#tabla-personas').DataTable();
    $('#tabla-personas tbody').on('change', 'tr', 'input:radio', function () {
        let rowIdx = table.row(this).index();
        // Incluir el radioButton al inicio, por eso se comienza por la columna 1
        let cui = table.cell(rowIdx, 1).data();
        let primerNombre = table.cell(rowIdx, 3).data();
        let primerApellido = table.cell(rowIdx, 4).data();
        set("uiCuiBusqueda", "");
        set("uiCui", cui);
        set("uiPrimerNombre", primerNombre);
        set("uiPrimerApellido", primerApellido);
        set("uiIdUsuario", primerNombre.toLowerCase() + '.' + primerApellido.toLowerCase());
        document.getElementById("uiClosePopupPersonas").click();
    });
}

function changeValueCheckSuperAdmin(obj) {
    if (document.getElementById('uiEsSuperAdmin').checked) {
        set("uiEsSuperAdmin", "1");
    }
    else {
        set("uiEsSuperAdmin", "0");
    }
}

function guardarUsuario() {
    let errores = ValidarDatos("frmGuardarUsuario")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let frmGuardar = document.getElementById("frmGuardarUsuario");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Usuario/GuardarUsuario", "text", frm, function (data) {
            if (data == "OK") {
                Exito("Usuario", "Index", true);
            } else {
                MensajeError(data);
            }
        })
    })
}

/*function listarEmpresas() {
    let objConfiguracion = {
        url: "Empresa/GetAllEmpresas",
        cabeceras: ["codigo", "nombre empresa"],
        propiedades: ["codigoEmpresa", "nombreComercial"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        check: true

    }
    pintar(objConfiguracion);
}*/


function ActualizarUsuario() {
    let idUsuario = document.getElementById("uiIdUsuario").value;
    let obj = null;

    // Roles
    let arrayProperties = new Array();
    let table = $('#tabla-roles').DataTable();
    var data = table.rows().data();
    data.each(function (value, index) {
        obj = {
            Codigo: table.cell(index, 0).data(),
            Asignado: table.cell(index, 4).nodes().to$().find('input').prop('checked') == true ? 1 : 0,
            CodigoTipoPermiso : 1
        };
        arrayProperties.push(obj);
    });

    // Cajas Chicas
    table = $('#tabla-cajas-chicas').DataTable();
    data = table.rows().data();
    data.each(function (value, index) {
        obj = {
            Codigo: table.cell(index, 0).data(),
            Asignado: table.cell(index, 3).nodes().to$().find('input').prop('checked') == true ? 1 : 0,
            CodigoTipoPermiso: 2
        };
        arrayProperties.push(obj);
    });


    // Empresas
    table = $('#tabla-empresas').DataTable();
    data = table.rows().data();
    data.each(function (value, index) {
        obj = {
            Codigo: table.cell(index, 0).data(),
            Asignado: table.cell(index, 3).nodes().to$().find('input').prop('checked') == true ? 1 : 0,
            CodigoTipoPermiso: 3
        };
        arrayProperties.push(obj);
    });

    // Empresas
    table = $('#tabla-reportes').DataTable();
    data = table.rows().data();
    data.each(function (value, index) {
        obj = {
            Codigo: table.cell(index, 0).data(),
            Asignado: table.cell(index, 3).nodes().to$().find('input').prop('checked') == true ? 1 : 0,
            CodigoTipoPermiso: 4
        };
        arrayProperties.push(obj);
    });

    if (Array.isArray(arrayProperties) && arrayProperties.length) {
        let jsonData = JSON.stringify(arrayProperties);
        Confirmacion(undefined, undefined, function (rpta) {
            fetchPostJson("Usuario/GuardarPermisos/?idUsuario=" + idUsuario, "text", jsonData, function (data) {
                if (data == "OK") {
                    Redireccionar("Usuario", "Index");
                } else {
                    MensajeError(data);
                }
            })
        })
    } else {
        Warning("Error en la asignación de permisos");
    }
}



function ActualizarPassword() {
    let errores = ValidarDatos("frmCambioContrasenia")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmCambioContrasenia");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Usuario/ActualizarContrasenia", "text", frm, function (data) {
            if (data == "OK") {
                Exito("Home", "BlankPage", true, "Actualización realizada");
            } else {
                MensajeError(data);
            }
        });
    });
}


function listarUsuariosCambioPassword() {
    let objConfiguracion = {
        url: "Usuario/ListarUsuarios",
        cabeceras: ["Id Usuario", "nombre usuario", "Super Administrador", "SuperAdmin", "Estado"],
        propiedades: ["idUsuario", "nombreUsuario", "superAdmin", "esSuperAdmin", "estado"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        editar: true,
        funcioneditar: "CambioPassword",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [2],
                "visible": false
            }],
        slug: "idUsuario"
    }
    pintar(objConfiguracion);
}

function EditarCambioPassword(obj) {
    setI("uiTitlePopupUpdatePassword", "Actualización de contraseña");
    set("uiIdUsuario",obj);
    document.getElementById("ShowPopupUpdatePassword").click();
}


function ActualizarPasswordAdmin() {
    let errores = ValidarDatos("frmCambioContrasenia")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmCambioContrasenia");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Usuario/ActualizarContrasenia", "text", frm, function (data) {
            if (data == "OK") {
                Exito("Usuario", "CambioPasswordAdmin", true, "Actualización realizada");
            } else {
                MensajeError(data);
            }
        });
    });
}