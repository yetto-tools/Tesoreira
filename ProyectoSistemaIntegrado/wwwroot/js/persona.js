window.onload = function ()
{
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    if (nameController == "Persona") {
        switch (nameAction) {
            case "Index":
                MostrarPersonas(0);
                break;
            default:
                break;
        }// fin switch
    }// fin if
}


function MostrarPersonas(noIncluidoEnPlanilla) {
    objConfiguracion = {
        url: "Persona/GetAllPersonas/?noIncluidoEnPlanilla=0",
        cabeceras: ["Cui", "Primer Nombre", "Segundo Nombre", "Tercer Nombre", "Primer Apellido", "Segundo Apellido", "Apellido Casada", "Fecha Nacimiento", "codigoGenero", "Sexo", "Correo Electrónico", "codigoDepartamentoResidencia", "Departamento Residencia", "codigoMunicipioResidencia", "Municipio Residencia", "Zona", "Dirección Residencia", "noIncluidoEnPlanilla", "codigoArea", "Area","codigoEstado","Estado","permisoEditar"],
        propiedades: ["cui", "primerNombre", "segundoNombre","tercerNombre", "primerApellido", "segundoApellido", "apellidoCasada", "fechaNacimientoStr", "codigoGenero", "genero", "correoElectronico", "codigoDepartamentoResidencia", "departamentoResidencia", "codigoMunicipioResidencia","municipioResidencia","zona","direccionResidencia","noIncluidoEnPlanilla","codigoArea","area","codigoEstado","estado","permisoEditar"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        editar: true,
        funcioneditar: "Persona",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [6],
                "visible": false
            }, {
                "targets": [8],
                "visible": false
            }, {
                "targets": [10],
                "visible": false
            }, {
                "targets": [11],
                "visible": false
            }, {
                "targets": [13],
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
                "targets": [20],
                "visible": false
            }, {
                "targets": [22],
                "visible": false
            }],
        slug: "cui"
    }
    pintar(objConfiguracion);
}

function EditarPersona(obj) {
    document.getElementById("ShowPopupEditPersona").click();
    clearDataEditPersona();
    fetchGet("Persona/BusquedaPersona/?cui=" + obj, "json", function (rpta) {
        set("uiEditCui", rpta.cui);
        set("uiEditPrimerNombre", rpta.primerNombre);
        set("uiEditSegundoNombre", rpta.segundoNombre);
        set("uiEditTercerNombre", rpta.tercerNombre);
        set("uiEditPrimerApellido", rpta.primerApellido);
        set("uiEditSegundoApellido", rpta.segundoApellido);
        set("uiEditApellidoCasada", rpta.apellidoCasada);
        set("uiEditFechaNacimiento", rpta.fechaNacimientoStr);
        document.querySelector('#uiEditGenero').value = rpta.codigoGenero;
        setEditDepartamentos(rpta.codigoDepartamentoResidencia);
        setEditMunicipios(rpta.codigoDepartamentoResidencia, rpta.codigoMunicipioResidencia);
        set("uiEditZona", rpta.zona);
        set("uiEditDireccionResidencia", rpta.direccionResidencia);
        set("uiEditCorreoElectronico", rpta.correoElectronico);
        document.querySelector('#uiEditNoIncluidoEnPlanilla').checked = rpta.noIncluidoEnPlanilla == 1 ? true : false;
        if (rpta.noIncluidoEnPlanilla == 1) {
            setEditAreas(rpta.codigoArea);
        } else {
            FillComboUnicaOpcion("uiEditArea", "-1", "-- No Aplica -- ");
        }
    })
}

function clearDataEditPersona() {
    set("uiNewCui", "");
    set("uiNewPrimerNombre", "");
    set("uiNewSegundoNombre", "");
    set("uiNewTercerNombre", "");
    set("uiNewPrimerApellido", "");
    set("uiNewSegundoApellido", "");
    set("uiNewApellidoCasada", "");
    set("uiNewFechaNacimiento", "");
    set("uiNewZona", "");
    set("uiNewDireccionResidencia", "");
    set("uiEditCorreoElectronico", "");
    let elementNoIncluidoEnPlanilla = document.getElementById('uiNewNoIncluidoEnPlanilla');
    let elementArea = document.getElementById('uiNewArea');
    elementNoIncluidoEnPlanilla.checked = false;
    elementArea.classList.remove('obligatorio');
    FillComboUnicaOpcion("uiNewArea", "-1", "-- No Aplica -- ");
}

function setEditDepartamentos(codigoDepartamento) {
    fetchGet("Departamento/GetAllDepartamentos", "json", function (rpta) {
        FillCombo(rpta, "uiEditDepartamentoResidencia", "codigoDepartamento", "nombre", "- seleccione -", "-1");
        document.querySelector('#uiEditDepartamentoResidencia').value = codigoDepartamento;
    })
}

function setEditMunicipios(codigoDepartamento, codigoMunicipio) {
    fetchGet("Municipio/GetAllMunicipios/?codigoDepartamento=" + codigoDepartamento, "json", function (rpta) {
        FillCombo(rpta, "uiEditMunicipioResidencia", "codigoMunicipio", "nombre", "- seleccione -", "-1");
        document.querySelector('#uiEditMunicipioResidencia').value = codigoMunicipio;
    })
}

function fillEditMunicipiosByDepartamento(codigoDepartamento) {
    fetchGet("Municipio/GetAllMunicipios/?codigoDepartamento=" + codigoDepartamento, "json", function (rpta) {
        FillCombo(rpta, "uiEditMunicipioResidencia", "codigoMunicipio", "nombre", "- seleccione -", "-1");
    })
}

function setEditAreas(codigoArea) {
    fetchGet("Area/GetAllAreas", "json", function (rpta) {
        FillCombo(rpta, "uiEditArea", "codigoArea", "nombre", "- seleccione -", "-1");
        document.querySelector('#uiEditArea').value = codigoArea;
    })
}

function addNewPersona() {
    document.getElementById("ShowPopupNewPersona").click();
    clearDataNewPersona();
    fillNewCombosDepartamentos();
}

function fillNewCombosDepartamentos() {
    fetchGet("Departamento/GetAllDepartamentos", "json", function (rpta) {
        FillCombo(rpta, "uiNewDepartamentoResidencia", "codigoDepartamento", "nombre", "- seleccione -", "-1");
    })
}

function fillNewMunicipiosByDepartamento(codigoDepartamento) {
    fetchGet("Municipio/GetAllMunicipios/?codigoDepartamento=" + codigoDepartamento, "json", function (rpta) {
        FillCombo(rpta, "uiNewMunicipioResidencia", "codigoMunicipio", "nombre", "- seleccione -", "-1");
    })
}

function clearDataNewPersona() {
    set("uiNewCui", "");
    set("uiNewPrimerNombre", "");
    set("uiNewSegundoNombre", "");
    set("uiNewTercerNombre", "");
    set("uiNewPrimerApellido", "");
    set("uiNewSegundoApellido", "");
    set("uiNewApellidoCasada", "");
    set("uiNewFechaNacimiento", "");
    set("uiNewZona", "");
    set("uiNewDireccionResidencia", "");
    set("uiNewCorreoElectronico", "");
    let elementNoIncluidoEnPlanilla = document.getElementById('uiNewNoIncluidoEnPlanilla');
    let elementArea = document.getElementById('uiNewArea');
    elementNoIncluidoEnPlanilla.checked = false;
    elementArea.classList.remove('obligatorio');
    FillComboUnicaOpcion("uiNewArea", "-1", "-- No Aplica -- ");
}


function FillAreas(idcontrol, idControlNoIncluidoEnPlanilla) {
    let elementNoIncluidoEnPlanilla = document.getElementById(idControlNoIncluidoEnPlanilla);
    let elementArea = document.getElementById(idcontrol);
    if (elementNoIncluidoEnPlanilla.checked == true) {
        elementNoIncluidoEnPlanilla.value = "1";
        elementArea.classList.add('obligatorio');
        fetchGet("Area/GetAllAreas", "json", function (rpta) {
            FillCombo(rpta, idcontrol, "codigoArea", "nombre", "- seleccione -", "-1");
        })
    } else {
        elementNoIncluidoEnPlanilla.value = "0";
        FillComboUnicaOpcion(idcontrol, "-1", "-- No Aplica -- ");
        elementArea.classList.remove('obligatorio');
    }
}

function FillNewAreas() {
    FillAreas("uiNewArea","uiNewNoIncluidoEnPlanilla");
}

function FillEditAreas() {
    FillAreas("uiEditArea","uiEditNoIncluidoEnPlanilla");
}


function GuardarPersona() {
    let errores = ValidarDatos("frmGuardaPersona")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    var frmGuardar = document.getElementById("frmGuardaPersona");
    var frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Persona/GuardarPersona", "text", frm, function (data) {
            if (data == "OK") {
                document.getElementById("uiClosePopupNewPersona").click();
                MostrarPersonas(0);
            } else {
                MensajeError(data);
            }
        })
    })
}


function ActualizarPersona() {
    let errores = ValidarDatos("frmActualizarPersona")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    var frmGuardar = document.getElementById("frmActualizarPersona");
    var frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Persona/ActualizarPersona", "text", frm, function (data) {
            if (data == "OK") {
                document.getElementById("uiClosePopupEditPersona").click();
                MostrarPersonas(0);
            } else {
                MensajeError(data);
            }
        })
    })
}