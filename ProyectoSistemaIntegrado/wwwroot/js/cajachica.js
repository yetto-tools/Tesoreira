window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    let codigoTransaccion = "";
    if (nameController == "CajaChica") {
        switch (nameAction) {
            case "Index":
                fillComboCajasChicasFiltro();
                break;
            case "New":
                fillCombosNew();
                clearDataFactura();
                let setSemanaAnterior = parseInt(document.getElementById("uiSetSemanaAnterior").value);
                if (setSemanaAnterior == 1) {
                    fillComboSemanaAnterior();
                }
                break;
            case "Edit":
                codigoTransaccion = url1.searchParams.get("codigoTransaccion");
                clearDataFactura();
                fillCombosEdit(codigoTransaccion);
                break;
            case "SolicitudesCorreccion":
                MostrarSolicitudesCorreccion(-1, -1, -1);
                break;
            case "EditCorreccion":
                codigoTransaccion = url1.searchParams.get("codigoTransaccion");
                clearDataFactura();
                fillCombosEdit(codigoTransaccion);
                break;
            case "Configuracion":
                listarConfiguracionCajasChicas();
                listarMovimientosCajasChica();
                break;
            case "ConfiguracionAdmin":
                listarConfiguracionCajasChicasAdmin();
                listarMovimientosCajasChica();
                break;
            case "Movimientos":
                fillFiltroCajaChica();
                fillFiltroAnios();
                listarTodosLosMovimientosEnCajasChicas(-1, -1, -1);
                break;
            default:
                break;
        }// fin switch
    }// fin if
}

function fillFiltroCajaChica() {
    fetchGet("CajaChica/GetConfiguracionCajasChicas", "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            Warning("No existe cajas chicas asignadas al usuario");
        } else {
            FillCombo(rpta, "uiFiltroCajaChica", "codigoCajaChica", "nombreCajaChica", "- seleccione -", "-1");
        }
    })
}


function fillFiltroAnios() {
    fetchGet("CajaChica/GetAniosTransacciones", "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            Warning("No existe años");
        } else {
            FillCombo(rpta, "uiFiltroAnio", "anio", "anio", "- seleccione -", "-1");
        }
    })
}


function fillCombosNew() {
    fetchGet("CajaChica/fillCombosNewCajaChica", "json", function (rpta) {
        let listaOperaciones = rpta.listaOperaciones;
        let listaProgramacionSemanal = rpta.listaProgramacionSemanal;
        let listaCajasChicas = rpta.listaCajasChicas;
        let numeroSemana = rpta.numeroSemana;
        let anioOpcion = rpta.anioOperacion;
        FillCombo(listaCajasChicas, "uiCajaChica", "codigoCajaChica", "nombreCajaChica", "- seleccione -", "-1");
        FillCombo(listaOperaciones, "cboOperacion", "codigoOperacion", "nombre", "- seleccione -", "-1");
        fillProgramacionSemanal(listaProgramacionSemanal);
        setN("SemanaOperacion", numeroSemana);
        setN("AnioOperacion", anioOpcion);
    })
}

function fillCombosEdit(codigoTransaccion) {
    fetchGet("CajaChica/GetDataTransaccion/?codigoTransaccion=" + codigoTransaccion, "json", function (data) {
        if (data == null || data == undefined || data.length == 0) {
            MensajeError("Error en la búsqueda de la transacción " + codigoTransaccion);
        } else {
            let semanaOperacion = data.semanaOperacion;
            let anioOperacion = data.anioOperacion;
            fetchGet("CajaChica/FillCombosEditCajaChica/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString(), "json", function (rpta) {
                let listaOperaciones = rpta.listaOperaciones;
                let listaProgramacionSemanal = rpta.listaProgramacionSemanal;
                let listaCajasChicas = rpta.listaCajasChicas;
                FillCombo(listaCajasChicas, "uiCajaChica", "codigoCajaChica", "nombreCajaChica", "- seleccione -", "-1");
                FillCombo(listaOperaciones, "cboOperacion", "codigoOperacion", "nombre", "- seleccione -", "-1");
                fillProgramacionSemanal(listaProgramacionSemanal);
                set("uiCodigoTransaccion", data.codigoTransaccion.toString());
                set("uiAnioOperacion", anioOperacion.toString());
                set("uiNumeroSemanaActual", semanaOperacion.toString());
                set("uiCodigoReporte", data.codigoReporte.toString());
                document.querySelector('#uiCajaChica').value = data.codigoCajaChica.toString();
                setCheckedValueOfRadioButtonGroup('FechaStr', data.fechaStr);
                document.querySelector('#cboOperacion').value = data.codigoOperacion.toString();
                set("uiNitProveedor", data.nitProveedor);
                set("uiNombreProveedor", data.nombreProveedor);
                set("uiFechaDocumento", data.fechaDocumentoStr);
                set("uiSerieFactura", data.serieFactura);
                set("uiNumeroDocumento", data.numeroDocumento);
                set("uiMonto", data.monto.toString());
                set("uiDescripcion", data.descripcion);
                set("uiObservaciones", data.observaciones);
                // Para validad que no haya cambiado los datos de la factura
                set("uiNitProveedorOld", data.nitProveedor);
                set("uiSerieFacturaOld", data.serieFactura);
                set("uiNumeroDocumentoOld", data.numeroDocumento);
                setCheckedValueOfRadioButtonGroup('CodigoTipoDocumento', data.codigoTipoDocumento);

            });
        }
    })
}


function fillComboCajasChicasFiltro() {
    fetchGet("CajaChica/GetConfiguracionCajasChicas", "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            Warning("No existe cajas chicas asignadas al usuario (2)");
        } else {
            FillCombo(rpta, "uiFiltroCajaChica", "codigoCajaChica", "nombreCajaChica", "- seleccione -", "-1");
            let codigoCajaChica = parseInt(document.getElementById("uiFiltroCajaChica").value);
            mostrarTransaccionesCajaChica(codigoCajaChica);
        }
    })
}

function MostrarSaldoActual(obj) {
    let codigoCajaChica = parseInt(obj.value);
    if (codigoCajaChica != -1) {
        fetchGet("CajaChica/GetSaldoActual/?codigoCajaChica=" + codigoCajaChica.toString(), "text", function (data) {
            let saldoActual = parseFloat(data);
            set("uiSaldoActual", data.replace(/\B(?=(\d{3})+(?!\d))/g, ","));
            if (saldoActual >= 0) {
                document.getElementById('div-alerta-saldo').style.display = 'none';
            } else {
                document.getElementById('div-alerta-saldo').style.display = 'block';
            }
        });
    }
}


function fillComboCajaChica() {
    fetchGet("CajaChica/GetConfiguracionCajasChicas", "json", function (rpta) {
        FillCombo(rpta, "uiFiltroCajaChica", "codigoCajaChica", "nombreCajaChica", "- seleccione -", "-1");
    })
}

function clearDataFactura() {
    set("uiNitBusqueda", "");
    set("uiNitProveedor", "");
    set("uiNombreProveedor", "");
    set("uiFechaDocumento", "");
    set("uiSerieFactura", "");
    set("uiNumeroDocumento", "");
    set("uiMonto", "");
    set("uiDescripcion", "");
    set("uiObservaciones", "");
    let elementOperacion = document.getElementById("cboOperacion");
    elementOperacion.selectedIndex = 0;
}

function mostrarTransaccionesCajaChica(codigoCajaChica) {
    let objConfiguracion = {
        url: "CajaChica/GetTransaccionesCajaChica/?codigoCajaChica=" + codigoCajaChica.toString(),
        cabeceras: ["Código", "Caja Chica","Año","Semana","Día", "Nit", "Proveedor", "Fecha Factura", "Serie factura", "Número factura", "monto", "Creado por","Fecha Creación", "Fecha Creación", "Fecha Impresión", "Anular", "Editar"],
        propiedades: ["codigoTransaccion", "nombreCajaChica", "anioOperacion", "semanaOperacion", "nombreDiaOperacion","nitProveedor", "nombreProveedor", "fechaDocumento", "serieFactura", "numeroDocumento", "monto","usuarioIng","fechaIng","fechaIngStr","fechaImpresionStr", "permisoAnular", "permisoEditar"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        editar: true,
        funcioneditar: "TransaccionCajaChica",
        imprimir: true,
        sumarcolumna: true,
        columnasuma: 10,
        eliminar: true,
        funcioneliminar: "TransaccionCajaChica",
        slug: "codigoTransaccion",
        datesWithoutTime: ["fechaDocumento"],
        displaydecimals: ["monto"],
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [2],
                "className": "dt-body-center"
            }, {
                "targets": [3],
                "className": "dt-body-center"
            }, {
                "targets": [10],
                "className": "dt-body-right"
            }, {
                "targets": [12],
                "visible": false
            }, {
                "targets": [14],
                "visible": false
            }, {
                "targets": [15],
                "visible": false
            }, {
                "targets": [16],
                "visible": false
            }]
    }
    pintar(objConfiguracion);
}

function EditarTransaccionCajaChica(obj) {
    Redireccionar("CajaChica", "Edit/?codigoTransaccion=" + obj.toString());
}


function EliminarTransaccionCajaChica(obj) {
    let observaciones = "";
    Confirmacion("Anulación de Transacción", "¿Desea anular el registro?", function (rpta) {
        fetchGet("CajaChica/AnularTransaccionCajaChica/?codigoTransaccion=" + obj.toString() + "&observaciones=" + observaciones, "text", function (data) {
            if (data != "OK") {
                Redireccionar("CajaChica", "Index");
                return;
            } else {
                listarTransaccionesCajaChica();
            }
        })
    })
}

function Imprimir(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-imprimir', function () {
        let rowIdx = table.row(this).index();
        let codigoTransaccion = table.cell(rowIdx, 0).data();
        let nitProveedor = table.cell(rowIdx, 5).data();
        let nombreProveedor = table.cell(rowIdx, 6).data();
        let monto = table.cell(rowIdx, 10).data();
        let usuarioCreacion = table.cell(rowIdx, 11).data();
        let fechaCreacion = table.cell(rowIdx, 13).data();
        let fechaImpresion = table.cell(rowIdx, 14).data();
        imprimirConstancia(codigoTransaccion, nitProveedor, nombreProveedor, monto, usuarioCreacion, fechaCreacion, fechaImpresion);

    });
}


function BuscarTransaccionesCajaChica() {
    let codigoCajaChica = parseInt(document.getElementById("uiFiltroCajaChica").value);
    mostrarTransaccionesCajaChica(codigoCajaChica);
}


function fillComboSemana(obj) {
    let habilitarSemanaAnterior = 0;
    if (obj.checked) {
        habilitarSemanaAnterior = 1;
    }
    fetchGet("Transaccion/FillComboSemana/?habilitarSemanaAnterior=" + habilitarSemanaAnterior.toString(), "json", function (rpta) {
        let listaProgramacionSemanal = rpta.listaProgramacionSemanal;
        let numeroSemana = rpta.numeroSemana;
        fillProgramacionSemanal(listaProgramacionSemanal);
        setN("SemanaOperacion", numeroSemana);
    })
}

// Función temporal, debido a que la implementación del sistema empezará del lado de contabilidad, se debe de colocar por default la semana anterior a ser registrada
function fillComboSemanaAnterior() {
    let semanaOperacion = parseInt(document.getElementById("uiNumeroSemanaActualSistema").value);
    let anioOperacion = parseInt(document.getElementById("uiAnioSemanaActualSistema").value);
    let checkSemanaAnterior = document.getElementById("uiCheckSemanaAnterior")
    if (semanaOperacion == 1) {
        anioOperacion = anioOperacion - 1;
        setN("AnioOperacion", anioOperacion);
    }
    fetchGet("Transaccion/FillComboSemana/?habilitarSemanaAnterior=1", "json", function (rpta) {
        let listaProgramacionSemanal = rpta.listaProgramacionSemanal;
        let numeroSemana = rpta.numeroSemana;
        fillProgramacionSemanal(listaProgramacionSemanal);
        setN("SemanaOperacion", numeroSemana);
        checkSemanaAnterior.checked = true;
        checkSemanaAnterior.disabled = true;
    })
}


function fillProgramacionSemanal(res) {
    let objConfiguracionSemana = {
        propiedadid: "uiFechaOperacion",
        propiedadnombre: "FechaStr",
        propiedades: ["fechaStr", "dia"],
        divContenedorTabla: "divContainerTablaRadioButtonList",
        divPintado: "divTablaRadioButtonList",
    }
    pintarRadioButtonList(objConfiguracionSemana, res);
}


function GuardarDatos() {
    let errores = ValidarDatos("frmTransaccionCajaChica")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    var frmGuardar = document.getElementById("frmTransaccionCajaChica");
    var frm = new FormData(frmGuardar);

    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("CajaChica/GuardarDatos", "text", frm, function (data) {
            if (data == "OK") {
                Exito("CajaChica","Index",true);
            } else {
                MensajeError(data);
            }
        })
    })
}

function ActualizarDatos() {
    let errores = ValidarDatos("frmTransaccionCajaChica")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    var frmGuardar = document.getElementById("frmTransaccionCajaChica");
    var frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("CajaChica/ActualizarDatos/?codigoTipoActualizacion=1", "text", frm, function (data) {
            if (data == "OK") {
                Exito("CajaChica", "Index", true);
            } else {
                MensajeError(data);
            }
        })
    })
}

function ActualizarDatosCorreccion() {
    let errores = ValidarDatos("frmTransaccionCajaChica")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    var frmGuardar = document.getElementById("frmTransaccionCajaChica");
    var frm = new FormData(frmGuardar);
    let codigoReporte = frm.get("CodigoReporte");
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("CajaChica/ActualizarDatos/?codigoTipoActualizacion=2", "text", frm, function (data) {
            if (data == "OK") {
                Exito("CorteSemanalCajaChica", "MostrarDetalleRevision/?codigoReporte=" + codigoReporte, true);
            } else {
                MensajeError(data);
            }
        })
    })
}

function buscarDatosContribuyente() {
    let valor = get("uiNitBusqueda");
    if (valor === "") {
    }
    else {
        fetchGet("Contribuyente/GetDataContribuyente/?nit=" + valor, "json", function (rpta) {
            if (rpta == null) {
                MensajeError("Error en la búsqueda de datos del nit: " + valor);
            } else {
                //if (Object.keys(rpta).length > 0) {
                if (rpta.nit != null) {
                    // El nit existe en la base de datos de contribuyente
                    set("uiNitBusqueda", "");
                    set("uiNitProveedor", rpta.nit);
                    set("uiNombreProveedor", rpta.nombre);
                } else {
                    // nit no existe en la base de datos de contribuyen, se tiene que registrar
                    document.getElementById("ShowPopupContribuyente").click();
                    set("uiNewNit", valor);
                    set("uiNewNombreContribuyente", "");
                }
                
            }
        })
    }
}


function GuardarContribuyente() {
    let errores = ValidarDatos("frmGuardaContribuyente")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    var frmGuardar = document.getElementById("frmGuardaContribuyente");
    var frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Contribuyente/GuardarContribuyente", "text", frm, function (data) {
            if (data == "OK") {
                document.getElementById("uiClosePopupContribuyente").click();
                buscarDatosContribuyente();
            } else {
                MensajeError(data);
            }
        })
    })
}

// "width": "4%",
// "width": "4%",
function listarConfiguracionCajasChicas() {
    let objConfiguracion = {
        url: "CajaChica/GetConfiguracionCajasChicas",
        cabeceras: ["Código", "Caja Chica", "Monto Limite", "monto Disponible","Editar","Anular"],
        propiedades: ["codigoCajaChica", "nombreCajaChica", "montoLimite","montoDisponible","permisoEditar","permisoAnular"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        editar: true,
        slug: "codigoCajaChica",
        paginar: true,
        autoWidth: true,
        displaydecimals: ["montoLimite","montoDisponible"],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [2],
                "className": "dt-body-right",
                "visible": true
            }, {
                "targets": [3],
                "className": "dt-body-right",
                "visible": true
            }, {
                "targets": [4],
                "visible": false
            }, {
                "targets": [5],
                "visible": false
            }]
    }
    pintar(objConfiguracion);
}


function Editar(obj) {
    setI("uiTitlePopupEdicionConfig", "Edición de Configuración Caja Chica");
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-editar', function () {
        let rowIdx = table.row(this).index();
        let codigoCajaChica = table.cell(rowIdx, 0).data();
        let nombreCajaChica = table.cell(rowIdx, 1).data();
        let montoLimite = table.cell(rowIdx, 2).data();
        let montoDisponible = table.cell(rowIdx, 3).data();
        set("uiCodigoCajaChica", codigoCajaChica.toString());
        set("uiNombreCajaChica", nombreCajaChica);
        set("uiMontoActualLimite", Number(montoLimite).toFixed(2));
        set("uiMontoLimite", Number(montoLimite).toFixed(2));
        set("uiMontoActualDisponible", Number(montoDisponible).toFixed(2));
        set("uiObservaciones","");
    });
    document.getElementById("ShowPopupEdicionConfig").click();
}

function ActualizarMontoDisponible() {
    let errores = ValidarDatos("frmConfiguracionCajaChica")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    var frmGuardar = document.getElementById("frmConfiguracionCajaChica");
    var frm = new FormData(frmGuardar);

    let codigoOperacion = 0;
    let codigoCajaChica = frm.get("CodigoCajaChica");
    let montoActualLimite = parseFloat(frm.get("MontoActualLimite"));
    let montoActualDisponible = parseFloat(frm.get("MontoActualDisponible"));
    let montoLimite = parseFloat(frm.get("MontoLimite"));
    let observaciones = frm.get("Observaciones")
    let diferencia = 0;
    let mensajeAdvertencia = "";

    if (montoLimite != montoActualLimite) {
        if (montoLimite < montoActualLimite) {
            // Reducción de Caja Chica
            diferencia = montoActualLimite - montoLimite;

            if ((montoActualDisponible - diferencia) >= 0) {
                codigoOperacion = "69";
                mensajeAdvertencia = "¿Está seguro de reducir de " + montoActualLimite.toString() + " a " + montoLimite.toString() + "?";
            } else {
                MensajeError("El saldo actual " + montoActualDisponible.toString() + " es insuficiente para reducir los " + diferencia.toString() + " de la caja chica");
                return;
            }

        } else {
            if (montoLimite > montoActualLimite) {
                // Abono a Caja Chica
                codigoOperacion = "70";
                diferencia = montoLimite - montoActualLimite;
                mensajeAdvertencia = "¿Está seguro de aumentar de " + montoActualLimite.toString() + " a " + montoLimite.toString() + "?";
            }
        }
    } else {
        MensajeError("No existe actualización del monto asignado a caja chica");
        return;
    }

    let anioOperacion = parseInt(document.getElementById("uiAnioSemanaActualSistema").value);
    let semanaOperacion = parseInt(document.getElementById("uiNumeroSemanaActualSistema").value);

    Confirmacion("Actualización de Monto Asignado a Caja Chica", mensajeAdvertencia, function (rpta) {
        fetchGet("CajaChica/ActualizarMontoDisponibleCajaChica/?codigoCajaChica=" + codigoCajaChica + "&codigoOperacion=" + codigoOperacion + "&anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString() + "&monto=" + diferencia.toString() + "&observaciones=" + observaciones, "text", function (data) {
            document.getElementById("uiClosePopupEdicionConfig").click();
            if (data == "0") {
                listarConfiguracionCajasChicas();
                listarMovimientosCajasChica();
            } else {
                MensajeError(data);
            }
        })
    })
}


function listarMovimientosCajasChica() {
    let objConfiguracion = {
        url: "CajaChica/GetMovimientosEnConfiguracionCajasChicas",
        cabeceras: ["Transacción", "codigoReporte", "codigoCajaChica", "Caja Chica", "codigoOperacion", "Operación", "Fecha Operación", "Año", "Semana", "Monto", "Observaciones", "Creado por", "Fecha Creación", "codigoEstadoRecepcion", "Estado"],
        propiedades: ["codigoTransaccion","codigoReporte","codigoCajaChica","nombreCajaChica","codigoOperacion","operacion","fechaOperacionStr","anioOperacion","semanaOperacion","monto","observaciones","usuarioIng","fechaIngStr","codigoEstadoRecepcion","estadoRecepcion"],
        divContenedorTabla: "divContenedorTablaMovimiento",
        idtabla: "tabla-movimiento",
        divPintado: "divTablaMovimiento",
        eliminar: true,
        funcioneliminar: "MovimientoCajaChica",
        slug: "codigoTransaccion",
        paginar: true,
        displaydecimals: ["monto"],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [1],
                "visible": false
            }, {
                "targets": [2],
                "visible": false
            }, {
                "targets": [4],
                "visible": false
            }, {
                "targets": [7],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [8],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [9],
                "className": "dt-body-right"
            }, {
                "targets": [13],
                "visible": false
            }]
    }
    pintar(objConfiguracion);
}

function buscarMovimientosEnCajaChica() {
    let codigoCajaChica = parseInt(document.getElementById("uiFiltroCajaChica").value);
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnio").value);
    let codigoOperacion = parseInt(document.getElementById("uiFiltroOperacion").value);
    listarTodosLosMovimientosEnCajasChicas(codigoCajaChica, anioOperacion, codigoOperacion);
}

function EliminarMovimientoCajaChica() {
    let table = $('#tabla-movimiento').DataTable();
    $('#tabla-movimiento tbody').on('click', '.option-eliminar', function () {
        let rowIdx = table.row(this).index();
        let codigoTransaccion = table.cell(rowIdx, 0).data();
        let nombreCajaChica = table.cell(rowIdx, 3).data();

        var frm = new FormData();
        frm.set("CodigoTransaccion", codigoTransaccion);

        Confirmacion("Anulación de movimiento de caja chica", "¿Está seguro de anular este movimiento de caja chica de " + nombreCajaChica + "?", function (rpta) {
            fetchPost("CajaChica/AnularMovimientoCajaChica", "text", frm, function (data) {
                if (data == "OK") {
                    listarMovimientosCajasChica();
                } else {
                    MensajeError(data);
                }
            });
        });
    });

}

function listarTodosLosMovimientosEnCajasChicas(codigoCajaChica, anioOperacion, codigoOperacion) {
    let objConfiguracion = {
        url: "CajaChica/GetAllMovimientosEnCajasChicas/?codigoCajaChica=" + codigoCajaChica.toString() + "&anioOperacion=" + anioOperacion.toString() + "&codigoOperacion=" + codigoOperacion.toString(),
        cabeceras: ["Transacción", "codigoReporte", "codigoCajaChica", "Caja Chica", "codigoOperacion", "Operación", "Fecha Operación", "Año", "Semana", "Monto", "Observaciones", "Creado por", "Fecha Creación", "codigoEstadoRecepcion", "Estado"],
        propiedades: ["codigoTransaccion", "codigoReporte", "codigoCajaChica", "nombreCajaChica", "codigoOperacion", "operacion", "fechaOperacionStr", "anioOperacion", "semanaOperacion", "monto", "observaciones", "usuarioIng", "fechaIngStr", "codigoEstadoRecepcion", "estadoRecepcion"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        slug: "codigoTransaccion",
        paginar: true,
        displaydecimals: ["monto"],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [1],
                "visible": false
            }, {
                "targets": [2],
                "visible": false
            }, {
                "targets": [4],
                "visible": false
            }, {
                "targets": [7],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [8],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [9],
                "className": "dt-body-right"
            }, {
                "targets": [13],
                "visible": false
            }]
    }
    pintar(objConfiguracion);
}


function listarConfiguracionCajasChicasAdmin() {
    let objConfiguracion = {
        url: "CajaChica/GetConfiguracionCajasChicas",
        cabeceras: ["Código", "Caja Chica", "Monto Limite", "monto Disponible", "Editar", "Anular"],
        propiedades: ["codigoCajaChica", "nombreCajaChica", "montoLimite", "montoDisponible", "permisoEditar", "permisoAnular"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        editar: true,
        funcioneditar: "ConfiguracionAdmin",
        slug: "codigoCajaChica",
        paginar: true,
        autoWidth: true,
        displaydecimals: ["montoDisponible","montoSaldo"],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [2],
                "className": "dt-body-right",
                "visible": true
            }, {
                "targets": [3],
                "className": "dt-body-right",
                "visible": true
            }, {
                "targets": [4],
                "visible": false
            }, {
                "targets": [5],
                "visible": false
            }]
    }
    pintar(objConfiguracion);
}

function EditarConfiguracionAdmin(obj) {
    setI("uiTitlePopupEdicionConfig", "Edición de Configuración Caja Chica");
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-editar', function () {
        let rowIdx = table.row(this).index();
        let codigoCajaChica = table.cell(rowIdx, 0).data();
        let nombreCajaChica = table.cell(rowIdx, 1).data();
        let montoLimite = table.cell(rowIdx, 2).data();
        let montoDisponible = table.cell(rowIdx, 3).data();
        set("uiCodigoCajaChica", codigoCajaChica.toString());
        set("uiNombreCajaChica", nombreCajaChica);
        set("uiMontoLimite", Number(montoLimite).toFixed(2));
        set("uiMontoDisponible", Number(montoDisponible).toFixed(2));
        set("uiObservaciones", "");
        document.getElementById("ShowPopupEdicionConfig").click();
    });
    
}

function ActualizarConfigCajasChicasAdmin() {
    let errores = ValidarDatos("frmConfiguracionCajaChica")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    var frmGuardar = document.getElementById("frmConfiguracionCajaChica");
    var frm = new FormData(frmGuardar);

    let codigoCajaChica = frm.get("CodigoCajaChica");
    let montoLimite = parseFloat(frm.get("MontoLimite"));
    let montoDisponible = parseFloat(frm.get("MontoDisponible"));
    let observaciones = frm.get("Observaciones")

    Confirmacion("Actualización de montos de Caja Chica", "¿Está seguro de la modificación de montos?", function (rpta) {
        fetchGet("CajaChica/ActualizarMontosCajaChicaAdmin/?codigoCajaChica=" + codigoCajaChica + "&montoLimite=" + montoLimite.toString() + "&montoDisponible=" + montoDisponible.toString() + "&observaciones=" + observaciones, "text", function (data) {
            document.getElementById("uiClosePopupEdicionConfig").click();
            if (data == "OK") {
                listarConfiguracionCajasChicasAdmin();
                listarMovimientosCajasChica();
            } else {
                MensajeError(data);
            }
        })
    })
}





function imprimirConstancia(codigoTransaccion, nitProveedor, nombreProveedor, monto, usuarioCreacion, fechaCreacion, fechaImpresion) {
    let table = `<div style="width: 287px; max-width: 287px; text-align: center; align-content: center; margin-left: 25px;">
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif'; font-weight: bold;">CAJA CHICA<br>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif'; font-weight: bold;">OMAVOHDNN<br>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif';">COMPROBANTE EGRESO<br>
                 <table style="border-top: 1px solid black; border-collapse: collapse;">
                    <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Recibo:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${codigoTransaccion}</td>
                    </tr>
                    <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Fecha:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${fechaCreacion}</td>
                    </tr>
                    <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Nit:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${nitProveedor}</td >
                    </tr>
                    <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Proveedor:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${nombreProveedor}</td>
                    </tr>
                    <tr style="border-collapse: collapse;">
                        <td style="width: 75px; max-width: 75px; border-collapse: collapse; font-size: 18px; font-family: 'Arial, sans-serif';">Monto:</td>
                        <td style="font-size: 18px; font-family: 'Arial, sans-serif';">${monto}</td>
                    </tr>
                 </table>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif';">${fechaImpresion}<br>
                 <p style="text-align: center; align-content: center; font-size: 18px; font-family: 'Arial, sans-serif';">${usuarioCreacion}<br>
                 </div>`;
    var pagina = window.document.body;
    var ventana = window.open();
    ventana.document.write(table);
    ventana.print();
    ventana.close();
}


function MostrarSolicitudesCorreccion(anioOperacion, semanaOperacion, codigoReporte) {
    let objConfiguracion = {
        url: "CajaChica/GetSolicitudesDeCorreccion/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString() + "&codigoReporte=" + codigoReporte.toString(),
        cabeceras: ["Código Reporte","Código Transacción","codigoTransaccionAnt","correccion", "Caja Chica", "Año", "Semana", "Día", "Nit", "Proveedor", "Fecha Factura", "Serie factura", "Número factura", "monto", "Creado por", "Fecha Creación", "Fecha Creación","Respuesta","codigoTipoCorreccion","tipoCorreccion","Autorizar","descripcion","observacionesSolicitud","observacionesAprobacion"],
        propiedades: ["codigoReporte", "codigoTransaccion","codigoTransaccionAnt","correccion", "nombreCajaChica", "anioOperacion", "semanaOperacion", "nombreDiaOperacion", "nitProveedor", "nombreProveedor", "fechaDocumento", "serieFactura", "numeroDocumento", "monto", "usuarioIng", "fechaIng", "fechaIngStr","respuestaCorreccion","codigoTipoCorreccion","tipoCorreccion", "permisoAutorizar","descripcion","observacionesSolicitud","observacionesAprobacion"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        autorizar: true,
        funcionautorizar: "AutorizarCorreccion",
        alerta: true,
        funcionalerta: "CorreccionTransaccion",
        sumarcolumna: true,
        columnasuma: 13,
        slug: "codigoTransaccion",
        datesWithoutTime: ["fechaDocumento"],
        displaydecimals: ["monto"],
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "visible": true,
                "className": "dt-body-center"
            }, {
                "targets": [2],
                "visible": false
            }, {
                "targets": [3],
                "visible": false
            }, {
                "targets": [5],
                "className": "dt-body-center"
            }, {
                "targets": [6],
                "className": "dt-body-center"
            }, {
                "targets": [13],
                "className": "dt-body-right"
            }, {
                "targets": [15],
                "visible": false
            }, {
                "targets": [18],
                "visible": false
            }, {
                "targets": [19],
                "visible": true
            }, {
                "targets": [20],
                "visible": false
            }, {
                "targets": [21],
                "visible": false
            }, {
                "targets": [22],
                "visible": false
            }, {
                "targets": [23],
                "visible": false
            }]
    }
    pintar(objConfiguracion);
}

function clickAutorizarCorreccion(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-autorizar', function () {
        let rowIdx = table.row(this).index();
        let codigoTransaccion = table.cell(rowIdx, 1).data();
        let nitProveedor = table.cell(rowIdx, 8).data();
        let nombreProveedor = table.cell(rowIdx, 9).data();
        let fechaDocumento = table.cell(rowIdx, 10).data();
        let serieDocumento = table.cell(rowIdx, 11).data();
        let numeroDocumento = table.cell(rowIdx, 12).data();
        let monto = table.cell(rowIdx, 13).data();
        let descripcion = table.cell(rowIdx, 21).data();
        let observacionesSolicitud = table.cell(rowIdx, 22).data();
        setI("uiTitlePopupCorreccionAprobada", "Aprobación de Corrección");
        document.getElementById("ShowPopupCorreccionAprobada").click();
        set("uiCodigoTransaccion", codigoTransaccion);
        set("uiNitProveedor", nitProveedor);
        set("uiNombreProveedor", nombreProveedor);
        set("uiFechaDocumento", fechaDocumento);
        set("uiSerieFactura", serieDocumento);
        set("uiNumeroDocumento", numeroDocumento);
        set("uiMonto", monto);
        set("uiDescripcion", descripcion);
        set("uiObservacionesSolicitud", observacionesSolicitud);
        set("uiObservacionesAprobacion", "");
        document.getElementById("uiResultado").value = "-1";
        
    });
}

function AutorizarCorreccion() {
    let errores = ValidarDatos("frmAutorizacionCorreccion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmAutorizacionCorreccion");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("CajaChica/AutorizarCorreccion", "text", frm, function (data) {
            if (data == "OK") {
                document.getElementById("uiClosePopupCorreccionAprobada").click();
                MostrarSolicitudesCorreccion(-1, -1, -1);
            } else {
                MensajeError(data);
            }
        });
    });
}

function clickAlertaCorreccionTransaccion(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-alerta', function () {
        let rowIdx = table.row(this).index();
        let codigoTransaccion = BigInt(table.cell(rowIdx, 1).data());
        let codigoTransaccionAnt = BigInt(table.cell(rowIdx, 2).data());
        let correccion = parseInt(table.cell(rowIdx, 3).data());
        if (correccion == 1) {
            if (codigoTransaccionAnt != 0) {
                document.getElementById("uiObservacionesEliminacion").innerHTML = "Esta transacción también es una actualización de la transacción " + codigoTransaccionAnt.toString();
                document.getElementById('uiObservacionesEliminacion').style.color = "red";
                document.getElementById("uiObservacionesEliminacion").style.fontSize = "large";
                document.getElementById("uiObservacionesEliminacion").style.fontWeight = 'bold';
            } else {
                document.getElementById("uiObservacionesEliminacion").innerHTML = "";
            }
            setI("uiTitlePopupSolicitudCorreccion", "Solicitud de Corrección");
            document.getElementById("ShowPopupSolicitudCorreccion").click();
            fetchGet("CajaChica/GetDataCorreccion/?codigoTransaccion=" + codigoTransaccion.toString(), "json", function (data) {
                set("uiSolicitudCodigoTransaccion", data.codigoTransaccion);
                set("uiSolicitudObservacionesSolicitud", data.observacionesSolicitud);
                set("uiSolicitudUsuarioSolicitud", data.usuarioIng);
                set("uiSolicitudFechaSolicitud", data.fechaIngStr);
                set("uiSolicitudResultado", data.resultado);
                set("uiSolicitudUsuarioAprobacion", data.usuarioAprobacion);
                set("uiSolicitudFechaAprobacion", data.fechaAprobacionStr);
                set("uiSolicitudObservacionesAprobacion", data.observacionesAprobacion);
                set("uiSolicitudCodigoTransaccionCorregido", data.codigoTransaccionCorrecta);
            });
        } else {
            Warning("La transacción " + codigoTransaccion + " es una actualización de la Transacción Anterior: " + codigoTransaccionAnt);
        }
    });
}


