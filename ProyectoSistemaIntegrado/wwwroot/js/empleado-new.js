window.onload = function () {
    fillCombosNewEmpleado();
    clearDataNewEmpleado();
}

function clearDataNewEmpleado() {
    set("uiCodigoEmpleado", "");
    set("filtroCui", "");
    set("uiCui", "");
    set("uiPrimerNombre", "");
    set("uiSegundoNombre", "");
    set("uiTercerNombre", "");
    set("uiPrimerApellido", "");
    set("uiSegundoApellido", "");
    set("uiApellidoCasada", "");
    set("uiFechaNacimiento", "");
    document.getElementById('uiGenero').selectedIndex = "0";
    set("uiCorreoElectronico", "");
    set("uiNumeroAfiliacion", "");
    document.getElementById('cboUbicacion').selectedIndex = "0";
    let elementTipoCuentaAhorro = document.getElementById('uiAhorro');
    let elementTipoCuentaCheque = document.getElementById('uiCheque');
    let elementTipoCuentaEfectivo = document.getElementById('uiEfectivo');
    let elementTipoCuentaMonetario = document.getElementById('uiMonetario');
    elementTipoCuentaAhorro.checked = false;
    elementTipoCuentaCheque.checked = false;
    elementTipoCuentaEfectivo.checked = false;
    elementTipoCuentaMonetario.checked = false;
    set("uiNumeroCuenta", "");
    set("uiMontoDevengado", "");
    let elementJornadaDiurna = document.getElementById('uiDiurna');
    let elementJornadaNocturna = document.getElementById('uiNocturna');
    let elementJornadaMixta = document.getElementById('uiMixta');
    elementJornadaDiurna.checked = false;
    elementJornadaNocturna.checked = false;
    elementJornadaMixta.checked = false;
    let elementPeriodicidadMensual = document.getElementById('uiPeriodicidadMensual');
    let elementPeriodicidadQuincenal = document.getElementById('uiPeriodicidadQuincenal');
    let elementPeriodicidadSemanal = document.getElementById('uiPeriodicidadSemanal');
    elementPeriodicidadMensual.checked = false;
    elementPeriodicidadQuincenal.checked = false;
    elementPeriodicidadSemanal.checked = false;
    let elementOtrosIGSS = document.getElementById('uiIGSS');
    let elementOtrosBonoDeLey = document.getElementById('uiBonoDeLey');
    let elementOtrosRetencionISR = document.getElementById('uiRetencionISR');
    let elementEmpleadoExterno = document.getElementById('uiEmpleadoExterno');
    elementOtrosIGSS.checked = false;
    elementOtrosBonoDeLey.checked = false;
    elementOtrosRetencionISR.checked = false;
    elementEmpleadoExterno.checked = false;
    set("uiFechaIngreso", "");
}



function fillCombosNewEmpleado() {
    fetchGet("Empleado/FillCombosNewEmpleado", "json", function (rpta) {
        var listaEmpresas = rpta.listaEmpresas;
        let listaAreas = rpta.listaAreas;
        let listaSecciones = rpta.listaSecciones;
        let listaPuestos = rpta.listaPuestos;
        FillCombo(listaEmpresas, "cboEmpresa", "codigoEmpresa", "nombreComercial", "- seleccione -", "-1");
        FillCombo(listaAreas, "cboArea", "codigoArea", "nombre", "- seleccione -", "-1");
        FillCombo(listaSecciones, "cboSeccion", "codigoSeccion", "nombre", "- seleccione -", "-1");
        FillCombo(listaPuestos, "cboPuesto", "codigoPuesto", "nombre", "- seleccione -", "-1");
    })
}


function fillCombosNewPersona() {
    fetchGet("Departamento/GetAllDepartamentos", "json", function (rpta) {
        FillCombo(rpta, "uiNewDepartamentoResidencia", "codigoDepartamento", "nombre", "- seleccione -", "-1");
     })
}

function fillMunicipiosByDepartamento(obj) {
    fetchGet("Municipio/GetAllMunicipios/?codigoDepartamento=" + obj.value, "json", function (rpta) {
        FillCombo(rpta, "uiNewMunicipioResidencia", "codigoMunicipio", "nombre", "- seleccione -", "-1");
    })
}

function BuscarCUI() {
    var cui = document.getElementById("filtroCui").value;
    if (!/^[0-9]+$/.test(cui)) {
        MensajeError("CUI con Carácter no válido");
        return;
    }

    if (cui.length != 13) {
        MensajeError("Longitud de CUI debe ser de 13 dígitos");
        return;
    }

    fetchGet("Persona/BusquedaPersona/?cui=" + cui, "json", function (data) {
        if (data.cui == null) {
            document.getElementById("ShowPopupNewPersona").click();
            set("uiNewCui", cui);
            fillCombosNewPersona();
        } else {
            set("filtroCui", "");
            set("uiCui", data.cui);
            set("uiPrimerNombre", data.primerNombre);
            set("uiSegundoNombre", data.segundoNombre);
            set("uiTercerNombre", data.tercerNombre);
            set("uiPrimerApellido", data.primerApellido);
            set("uiSegundoApellido", data.segundoApellido);
            set("uiApellidoCasada", data.apellidoCasada);
            set("uiFechaNacimiento", (new Date(data.fechaNacimiento)).toLocaleDateString());
            set("uiGenero", data.codigoGenero);
        }
    });
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
                BuscarCUI();
                //Exito("Empleado", "New", true);
            } else {
                MensajeError(data);
            }
        })
    })
}


function GuardarEmpleado() {
    let errores = ValidarDatos("frmGuardaEmpleado")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    if (!document.querySelector('input[name="CodigoTipoCuenta"]:checked')) {
        MensajeError("Debe seleccionar el tipo de cuenta");
        return;
    }

    if (!document.querySelector('input[name="CodigoJornada"]:checked')) {
        MensajeError("Debe seleccionar la jornada");
        return;
    }

    if (!document.querySelector('input[name="CodigoFrecuenciaPago"]:checked')) {
        MensajeError("Debe seleccionar la frecuencia de pago");
        return;
    }

    let checkIGSS = document.getElementById('uiIGSS');
    let checkBonoDeLey = document.getElementById('uiBonoDeLey');
    let checkRetencionISR = document.getElementById('uiRetencionISR');
    let checkEmpleadoExterno = document.getElementById('uiEmpleadoExterno');
   
    var frmGuardar = document.getElementById("frmGuardaEmpleado");
    var frm = new FormData(frmGuardar);
    if (checkIGSS.checked == true) {
        frm.set("Igss", "1");
    } else {
        frm.set("Igss", "0");
    }
    if (checkBonoDeLey.checked == true) {
        frm.set("BonoDeLey", "1");
    } else {
        frm.set("BonoDeLey", "0");
    }
    if (checkRetencionISR.checked == true) {
        frm.set("RetencionIsr", "1");
    } else {
        frm.set("RetencionIsr", "0");
    }
    if (checkEmpleadoExterno.checked == true) {
        frm.set("EmpleadoExterno", "1");
    } else {
        frm.set("EmpleadoExterno", "0");
    }

    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Empleado/GuardarEmpleado", "text", frm, function (data) {
            if (data == "OK") {
                Exito("Empleado", "Index", true);
            } else {
                MensajeError(data);
            }
        })
    })
}

