window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    if (nameController == "Rutas") {
        switch (nameAction) {
            case "Index":
                MostrarRutas();
                break;
            default:
                break;
        }// fin switch
    } else {
        if (nameController == "Vendedores") {
            switch (nameAction) {
                case "Index":
                    FillComboCanalVenta("uiCodigoCanalVenta");
                    ListarVendedores(-1);
                    break;
                case "ConsultaVendedoresRuta":
                    FillComboCanalVenta("uiCodigoCanalVenta");
                    ListarVendedoresRutaConsulta(-1, 0);
                    break;
                default:
                    break;
            }// fin switch
        }// fin if
        else {
            if (nameController == "TrasladoVentasContado") {
                switch (nameAction) {
                    case "Generacion":
                        let today = new Date();
                        const result = FormatDate(today);
                        set("uiFechaOperacionStr", result);
                        MostrarTrasladoMontoVentasEnProceso();
                        break;
                    case "RecepcionEfectivoVentas":
                        MostrarTrasladoMontoVentasAlContadoParaRecepcion();
                        break;
                    default:
                        break;
                }// fin switch
            } else {
                if (nameController == "TrasladoVentasCredito") {
                    switch (nameAction) {
                        case "Generacion":
                            let today = new Date();
                            const result = FormatDate(today);
                            set("uiFechaOperacionStr", result);
                            //MostrarTrasladoMontoVentasEnProceso();
                            break;
                        default:
                            break;
                    }// fin switch
                }
            }
        }
    }// fin if
}

function FillComboVendedores(idcontrol) {
    fetchGet("Vendedores/GetVendedores", "json", function (rpta) {
        if (rpta != null) {
            FillCombo(rpta, idcontrol, "codigoVendedor", "nombreVendedor", "- seleccione -", "-1");
        } else {
            FillComboUnicaOpcion(idcontrol, "-1", "-- No existen vendedores -- ");
        }
    })
}

//function SetComboVendedores(idcontrol, value) {
//    fetchGet("Vendedores/GetVendedores", "json", function (rpta) {
//        if (rpta != null) {
//            FillCombo(rpta, idcontrol, "codigoVendedor", "nombreVendedor", "- seleccione -", "-1");
//            document.querySelector('#' + idcontrol).value = value;
//        } else {
//            FillComboUnicaOpcion(idcontrol, "-1", "-- No existen vendedores -- ");
//        }
//    })
//}

function FillComboCanalVenta(idcontrol) {
    fetchGet("CanalesVentas/GetCanalesDeVentas", "json", function (rpta) {
        if (rpta != null) {
            FillCombo(rpta, idcontrol, "codigoCanalVenta", "canalVenta", "- seleccione -", "-1");
        } else {
            FillComboUnicaOpcion(idcontrol, "-1", "-- No existen canales de venta -- ");
        }
    })
}

function SetComboCanalVenta(idcontrol, value) {
    fetchGet("CanalesVentas/GetCanalesDeVentas", "json", function (rpta) {
        if (rpta != null) {
            FillCombo(rpta, idcontrol, "codigoCanalVenta", "canalVenta", "- seleccione -", "-1");
            document.querySelector('#' + idcontrol).value = value;
        } else {
            FillComboUnicaOpcion(idcontrol, "-1", "-- No existen canales de venta -- ");
        }
    })
}

function setCanalVenta(obj) {
    let ruta = parseInt(obj.value);
    if (ruta == -1) {
        FillComboCanalVenta("uiAsignacionCodigoCanalVenta");
    } else {
        fetchGet("CanalesVentas/GetCanalVenta/?ruta=" + obj.value, "json", function (rpta) {
            if (rpta != null) {
                FillCombo(rpta, "uiAsignacionCodigoCanalVenta", "codigoCanalVenta", "canalVenta", "- seleccione -", "-1");
            } else {
                FillComboUnicaOpcion("uiAsignacionCodigoCanalVenta", "-1", "-- No existen rutas -- ");
            }
        });
    }
}


function ListarVendedoresFiltro() {
    let codigoCanalVenta = parseInt(document.getElementById("uiCodigoCanalVenta").value);
    ListarVendedores(codigoCanalVenta);
}

function FillComboRutas(idcontrol) {
    fetchGet("Rutas/GetRutas", "json", function (rpta) {
        if (rpta != null) {
            FillCombo(rpta, idcontrol, "ruta", "ruta", "- seleccione -", "-1");
        } else {
            FillComboUnicaOpcion(idcontrol, "-1", "-- No existen rutas -- ");
        }
    })
}


function SetComboRutas(idcontrol, value) {
    fetchGet("Rutas/GetRutas", "json", function (rpta) {
        if (rpta != null) {
            FillCombo(rpta, idcontrol, "ruta", "ruta", "- seleccione -", "-1");
            document.querySelector('#' + idcontrol).value = value;
        } else {
            FillComboUnicaOpcion(idcontrol, "-1", "-- No existen rutas -- ");
        }
    })
}

function ListarVendedores(codigoCanalVenta) {
    let objConfiguracion = {
        url: "Vendedores/GetListaVendedores/?codigoCanalVenta=" + codigoCanalVenta.toString(),
        cabeceras: ["codigoConfiguracion","Código Vendedor", "Nombre Vendedor", "Código Canal Venta", "Canal de Venta","Ruta","Estado","Anular","Editar"],
        propiedades: ["codigoConfiguracion","codigoVendedor", "nombreVendedor", "codigoCanalVenta", "canalVenta", "ruta","estadoRutaVendedor","permisoAnular","permisoEditar"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        editar: true,
        funcioneditar: "ConfiguracionVendedorRuta",
        eliminar: true,
        funcioneliminar: "RutaVendedor",
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "visible": false
            }, {
                "targets": [1],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [3],
                "visible": false
            }, {
                "targets": [7],
                "visible": false
            }, {
                "targets": [8],
                "visible": false
            }],
        slug: "codigoVendedor"
    }
    pintar(objConfiguracion);
}


function EliminarRutaVendedor(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-eliminar', function () {
        let rowIdx = table.row(this).index();
        let codigoConfiguracion = table.cell(rowIdx, 0).data();
        let codigoVendedor = table.cell(rowIdx, 1).data();
        let nombreVendedor = table.cell(rowIdx, 2).data();
        let codigoCanalVenta = parseInt(table.cell(rowIdx,3).data());
        let canalVenta = table.cell(rowIdx, 4).data();
        let ruta = parseInt(table.cell(rowIdx, 5).data());
        Confirmacion("Anulación de Configuración", "¿Desea Anular la configuración de la ruta " + ruta + " del vendedor " + nombreVendedor + " con canal de venta " + canalVenta + "?", function (rpta) {
            fetchGet("Vendedores/AnularConfiguracionVendedorRuta/?codigoConfiguracion=" + codigoConfiguracion, "text", function (data) {
                if (data == "OK") {
                    ListarVendedores(-1);
                } else {
                    MensajeError(data);
                }
            })
        })
    });
}

function EditarConfiguracionVendedorRuta(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-editar', function () {
        let rowIdx = table.row(this).index();
        let codigoConfiguracion = table.cell(rowIdx, 0).data();
        let codigoVendedor = table.cell(rowIdx, 1).data();
        let nombreVendedor = table.cell(rowIdx, 2).data();
        let codigoCanalVenta = parseInt(table.cell(rowIdx, 3).data());
        let ruta = parseInt(table.cell(rowIdx, 5).data());
        setI("uiTitlePopupEditRutaVendedor", "Edición de Configuración Ruta Vendedor");
        set("uiEditCodigoConfiguracion", codigoConfiguracion);
        set("uiEditCodigoVendedor", codigoVendedor);
        set("uiEditNombreVendedor", nombreVendedor);
        SetComboRutas("uiEditRuta", ruta.toString());
        SetComboCanalVenta("uiEditCodigoCanalVenta", codigoCanalVenta.toString());
        document.getElementById("ShowPopupEditRutaVendedor").click();
    });
}


function setDataNuevoVendedor() {
    setI("uiTitlePopupNewVendedor", "Nueva Vendedor");
    set("uiNewCodigoVendedor", "");
    set("uiNewNombreVendedor", "");
    document.getElementById('div-tabla-clientes').style.display = 'block';
    intelligenceSearchCliente();

}


function intelligenceSearchCliente() {
    // Incluir el radioButton al inicio, por eso se comienza por la columna 1
    let objGlobalConfigTransaccion = {
        url: "Cliente/GetListAllClientes",
        cabeceras: ["Código", "Nombre Cliente"],
        propiedades: ["codigoCliente", "nombreCompleto"],
        divContenedorTabla: "divContenedorTablaClientes",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [1],
                "visible": true,
                "className": "dt-body-center"
            }],
        divPintado: "divTablaClientes",
        idtabla: "tablaClientes",
        slug: "codigoCliente",
        radio: true,
        eventoradio: "Vendedores",
        autoWidth: false
    }
    pintar(objGlobalConfigTransaccion);
}

function getDataRowRadioVendedores(obj) {
    let table = $('#tablaClientes').DataTable();
    $('#tablaClientes tbody').on('change', 'tr', 'input:radio', function () {
        let rowIdx = table.row(this).index();
        set("uiNewCodigoVendedor", table.cell(rowIdx, 1).data());
        set("uiNewNombreVendedor", table.cell(rowIdx, 2).data());
    });
}


function GuardarVendedorRuta() {
    let errores = ValidarDatos("frmAsignacionRutaVendedor")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmAsignacionRutaVendedor");
    let frm = new FormData(frmGuardar);

    let codigoVendedor = frm.get("CodigoVendedor");
    let codigoCanalVenta = frm.get("CodigoCanalVenta");
    let ruta = frm.get("Ruta");

    Confirmacion(undefined, undefined, function (rpta) {
        fetchGet("Vendedores/ExisteConfiguracionVendedorRuta/?codigoVendedor=" + codigoVendedor + "&codigoCanalVenta=" + codigoCanalVenta + "&ruta=" + ruta, "text", function (rpta) {
            let resultado = parseInt(rpta);
            if (resultado == 0) {// No existe configuración de vendedor ruta
                fetchPost("Vendedores/GuardarVendedorRuta", "text", frm, function (data) {
                    if (data == "OK") {
                        ListarVendedores(-1);
                    } else {
                        MensajeError(data);
                    }
                    document.getElementById("uiClosePopupRutaVendedor").click();
                });
            } else {
                if (resultado == -1) {
                    MensajeError("Error en búsqueda de configuración");
                } else {
                    MensajeError("Configuración ya existente");
                }
            }
        });
    });
}

function ActualizarVendedorRuta() {
    let errores = ValidarDatos("frmEditRutaVendedor")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmEditRutaVendedor");
    let frm = new FormData(frmGuardar);

    let codigoVendedor = frm.get("CodigoVendedor");
    let codigoCanalVenta = frm.get("CodigoCanalVenta");
    let ruta = frm.get("Ruta");

    Confirmacion("Actualización de Configuración", "¿Está seguro(a) de esta actualización?", function (rpta) {
        fetchGet("Vendedores/ExisteConfiguracionVendedorRuta/?codigoVendedor=" + codigoVendedor + "&codigoCanalVenta=" + codigoCanalVenta + "&ruta=" + ruta, "text", function (rpta) {
            let resultado = parseInt(rpta);
            if (resultado == 0)
            {// No existe configuración de vendedor ruta
                fetchPost("Vendedores/ActualizarConfiguracionVendedorRuta", "text", frm, function (data) {
                    if (data == "OK") {
                        ListarVendedoresFiltro();
                    } else {
                        MensajeError(data);
                    }
                    document.getElementById("uiClosePopupEditRutaVendedor").click();
                });
            } else {
                if (resultado == -1) {
                    MensajeError("Error en búsqueda de configuración");
                } else {
                    MensajeError("Configuración ya existente");
                }
            }
        });
    });
}


function MostrarRutas() {
    let objConfiguracion = {
        url: "Rutas/GetListaRutas",
        cabeceras: ["Ruta", "Nombre", "Nombre Corto", "codigoTipoRuta", "Tipo Ruta", "codigoCanalVenta", "Canal de Venta", "migracionCompleta","Descripción Ruta","Descripción Migración","codigoEstado", "Estado","Anular","Editar"],
        propiedades: ["ruta", "nombre", "nombreCorto", "codigoTipoRuta", "tipoRuta", "codigoCanalVenta", "canalVenta", "migracionCompleta","descripcion","descripcionMigracion","codigoEstado","estado","permisoAnular","permisoEditar"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        editar: true,
        funcioneditar: "Ruta",
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
                "visible": false
            }, {
                "targets": [7],
                "visible": false
            }, {
                "targets": [10],
                "visible": false
            }, {
                "targets": [12],
                "visible": false
            }, {
                "targets": [13],
                "visible": false
            }],
        slug: "ruta"
    }
    pintar(objConfiguracion);
}


function EditarRuta(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-editar', function () {
        let rowIdx = table.row(this).index();
        let nombre = table.cell(rowIdx, 1).data();
        let nombreCorto = table.cell(rowIdx, 2).data();
        let codigoTipoRuta = table.cell(rowIdx, 3).data();
        let codigoCanalVenta = table.cell(rowIdx, 5).data();
        let descripcion = table.cell(rowIdx, 8).data();
        let codigoEstado = table.cell(rowIdx, 10).data();
        document.getElementById("ShowPopupEditRuta").click();
        set("uiEditRuta", obj);
        set("uiEditNombre", nombre);
        set("uiEditNombreCorto", nombreCorto);
        set("uiEditDescripcion", descripcion);
        SetComboCanalVenta("uiEditCodigoCanalVenta", codigoCanalVenta);
        document.querySelector('#uiEditCodigoTipoRuta').value = codigoTipoRuta;
        document.querySelector('#uiEditCodigoEstado').value = codigoEstado;
    });
}



function ActualizarRuta() {
    let errores = ValidarDatos("frmEditRuta")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmEditRuta");
    let frm = new FormData(frmGuardar);
    let checkMigracionCompleta = document.getElementById("uiEditMigracionCompleta");
    if (checkMigracionCompleta.checked == true) {
        frm.set("MigracionCompleta", "1");
    } else {
        frm.set("MigracionCompleta", "0");
    }
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Rutas/ActualizarRuta", "text", frm, function (data) {
            if (data == "OK") {
                MostrarRutas();
            } else {
                MensajeError(data);
            }
            document.getElementById("uiClosePopupEditRuta").click();
        });
    });
}

function AgregarRuta(obj) {
    document.getElementById("ShowPopupNewRuta").click();
    set("uiNewRuta", "");
    set("uiNewNombre", "");
    set("uiNewNombreCorto", "");
    set("uiNewDescripcion", "");
    document.getElementById("uiNewCodigoTipoRuta").selectedIndex = 0;
    document.getElementById("uiNewCodigoEstado").selectedIndex = 0;
    document.getElementById("uiNewMigracionCompleta").checked = false;
    FillComboCanalVenta("uiNewCodigoCanalVenta");
}

function GuardarRuta() {
    let errores = ValidarDatos("frmNewRuta")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmNewRuta");
    let frm = new FormData(frmGuardar);
    let checkMigracionCompleta = document.getElementById("uiNewMigracionCompleta");
    if (checkMigracionCompleta.checked == true) {
        frm.set("MigracionCompleta", "1");
    } else {
        frm.set("MigracionCompleta", "0");
    }
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Rutas/GuardarRuta", "text", frm, function (data) {
            if (data == "OK") {
                MostrarRutas();
            } else {
                MensajeError(data);
            }
            document.getElementById("uiClosePopupNewRuta").click();
        });
    });

}



function setDataRutaNuevoVendedor() {
    setI("uiTitlePopupRutaVendedor", "Asignación de Ruta a Vendedor");
    FillComboVendedores("uiAsignacionCodigoVendedor");
    FillComboRutas("uiAsignacionRuta");
    FillComboCanalVenta("uiAsignacionCodigoCanalVenta");
}


function GuardarVendedor() {
    let errores = ValidarDatos("frmNewVendedor")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmNewVendedor");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Vendedores/GuardarVendedor", "text", frm, function (data) {
            if (data == "OK") {
                ListarVendedores(-1, 0);
            } else {
                MensajeError(data);
            }
            document.getElementById("uiClosePopupNewVendedor").click();
        });
    });
}


function ListarVendedoresRutaFiltroConsulta() {
    let codigoCanalVenta = parseInt(document.getElementById("uiCodigoCanalVenta").value);
    ListarVendedoresRutaConsulta(codigoCanalVenta);
}

function ListarVendedoresRutaConsulta(codigoCanalVenta) {
    let objConfiguracion = {
        url: "Vendedores/GetListaVendedores/?codigoCanalVenta=" + codigoCanalVenta.toString(),
        cabeceras: ["Código", "Nombre Vendedor", "Código Canal Venta", "Canal de Venta", "Ruta", "Estado"],
        propiedades: ["codigoVendedor", "nombreVendedor", "codigoCanalVenta", "canalVenta", "ruta", "estadoRutaVendedor"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [2],
                "visible": false
            }],
        slug: "codigoVendedor"
    }
    pintar(objConfiguracion);
}

/* ------------------------------------------------------------ */
/* --------------- Traslado de Monto de Ventas ---------------- */
/* ------------------------------------------------------------ */
function clearFiltroBusquedaGeneracionMontoVentas() {
    document.getElementById("uiMontoEfectivo").value = "";
    document.getElementById("uiMontoCheque").value = "";
    document.getElementById("uiMontoTransferencia").value = "";
}

function GuardarTrasladoMontoVentas() {
    let errores = ValidarDatos("frmNewTraslado")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmNewTraslado");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("TrasladoVentasContado/GuardarTraslado", "text", frm, function (data) {
            if (data == "OK") {
                clearFiltroBusquedaGeneracionMontoVentas();;
                MostrarTrasladoMontoVentasEnProceso();
            } else {
                MensajeError(data);
            }
        });
    });
}

function MostrarTrasladoMontoVentasEnProceso() {
    let objConfiguracion = {
        url: "TrasladoVentasContado/GetTrasladosEnProceso",
        cabeceras: ["Código Traslado", "Fecha Operación", "Monto Efectivo", "Monto Cheques","Montro Transferencia", "Monto", "Fecha Generación", "Generado Por", "codigoEstado", "Estado", "permisoAnular", "permisoImprimir", "permisoTraslado"],
        propiedades: ["codigoTraslado","fechaOperacionStr", "montoEfectivo", "montoCheques","montoTransferencia", "monto", "fechaGeneracionStr", "usuarioIng", "codigoEstado", "estado", "permisoAnular", "permisoImprimir", "permisoTraslado"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        displaydecimals: ["montoEfectivo", "montoCheques", "montoTransferencia","monto"],
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [1],
                "visible": false
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
                "className": "dt-body-right",
                "visible": true
            }, {
                "targets": [5],
                "className": "dt-body-right",
                "visible": true
            }, {
                "targets": [7],
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
                "targets": [12],
                "visible": false
            }],
        slug: "codigoTraslado",
        eliminar: true,
        funcioneliminar: "TrasladoMontoVentas",
        imprimir: true,
        funcionimprimir: "ConstanciaTrasladoMontoVentas",
        aceptartraslado: true,
        funcionaceptartraslado: "MontoVentas"
    }
    pintar(objConfiguracion);
}

function EliminarTrasladoMontoVentas(obj) {
    let codigoTraslado = parseInt(obj);
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-eliminar', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        Confirmacion("Anulación de Traslado", "¿Desea anular este traslado de monto de caja?", function (rpta) {
            fetchGet("TrasladoVentasContado/AnularTraslado/?codigoTraslado=" + codigoTraslado.toString(), "text", function (data) {
                if (data == "OK") {
                    MostrarTrasladoMontoVentasEnProceso();
                } else {
                    MensajeError(data);
                }
            })
        })
    });
}


function AceptarTrasladoMontoVentas(obj) {
    let codigoTraslado = parseInt(obj);
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-traslado', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        Confirmacion("Aceptación de traslado", "¿Desea aceptar el traslado?", function (rpta) {
            fetchGet("TrasladoVentasContado/AceptarTraslado/?codigoTraslado=" + codigoTraslado.toString(), "text", function (data) {
                if (data == "OK") {
                    MostrarTrasladoMontoVentasEnProceso();
                } else {
                    MensajeError(data);
                }
            })
        })
    });
}


function ImprimirConstanciaTrasladoMontoVentas(codigoTraslado, obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-imprimir', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        let fechaOperacionStr = table.cell(rowIdx, 1).data();
        let montoEfectivo = table.cell(rowIdx, 2).data();
        let montoCheques = table.cell(rowIdx, 3).data();
        let montoTransferencia = table.cell(rowIdx, 4).data();
        let montoTotal = table.cell(rowIdx, 5).data();
        let fechaGeneracionStr = table.cell(rowIdx, 6).data();
        fetchGet("TrasladoVentasContado/PrintConstanciaTrasladoVentasAlContado/?codigoTraslado=" + codigoTraslado + "&fechaOperacionStr=" + fechaOperacionStr + "&fechaGeneracionStr=" + fechaGeneracionStr + "&montoEfectivo=" + montoEfectivo + "&montoCheques=" + montoCheques + "&montoTransferencia=" + montoTransferencia + "&montoTotal=" + montoTotal, "text", function (data) {

        })
    });
}

/* --------------- Recepción de Monto de Ventas ---------------- */
function BuscarTrasladosDeVentasAlContado() {
    let fechaStr = document.getElementById("uiFechaOperacionStr").value;
    MostrarTrasladoMontoVentasAlContadoParaRecepcion();
}

function MostrarTrasladoMontoVentasAlContadoParaRecepcion() {
    let objConfiguracion = {
        url: "TrasladoVentasContado/GetTrasladosParaRecepcion/?codigoTipoTraslado=1",
        cabeceras: ["Código Traslado", "Fecha Operación", "Monto Efectivo", "Monto Cheques","Monto Transferencia", "Monto", "Fecha Generación", "Generado Por", "codigoEstado", "Estado", "permisoRegistrar"],
        propiedades: ["codigoTraslado", "fechaOperacionStr", "montoEfectivo", "montoCheques","montoTransferencia" ,"monto", "fechaGeneracionStr", "usuarioIng", "codigoEstado", "estado", "permisoRegistrar"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        displaydecimals: ["montoEfectivo", "montoCheques","montoTransferencia", "monto"],
        paginar: true,
        sumarcolumna: true,
        columnasumalist: [2,3,4,5],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [1],
                "visible": false
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
                "className": "dt-body-right",
                "visible": true
            }, {
                "targets": [5],
                "className": "dt-body-right",
                "visible": true
            }, {
                "targets": [8],
                "visible": false
            }, {
                "targets": [10],
                "visible": false
            }],
        slug: "codigoTraslado",
        registrar: true,
        funcionregistrar: "EfectivoVentasEstablecimiento"
    }
    pintar(objConfiguracion);
}

function RegistrarEfectivoVentasEstablecimiento() {
    let semanaOperacion = parseInt(document.getElementById("uiNumeroSemanaActualSistema").value);
    let anioOperacion = parseInt(document.getElementById("uiAnioSemanaActualSistema").value);

    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-registrar', function () {
        let rowIdx = table.row(this).index();
        let codigoTraslado = table.cell(rowIdx, 0).data();
        let fechaOperacionStr = table.cell(rowIdx, 1).data();

        let montoEfectivo = table.cell(rowIdx, 2).data();
        let montoCheques = table.cell(rowIdx, 3).data();
        let montoTransferencia = table.cell(rowIdx, 4).data();
        let montoTotal = table.cell(rowIdx, 5).data();

        let frm = new FormData();
        frm.set("FechaStr", fechaOperacionStr);
        frm.set("AnioOperacion", anioOperacion);
        frm.set("SemanaOperacion", semanaOperacion);
        frm.set("MontoEfectivo", montoEfectivo);
        frm.set("MontoCheques", montoCheques);
        frm.set("MontoTransferencia", montoTransferencia);
        frm.set("Monto", montoTotal);
        Confirmacion("Recepción de Efectivo de Ventas", "Monto a recepcionar es el correcto?", function (rpta) {
            fetchPost("Transaccion/RegistrarEfectivoVentasEstablecimiento/?codigoTraslado=" + codigoTraslado, "text", frm, function (data) {
                if (!/^[0-9]+$/.test(data)) {
                    MensajeError(data);
                } else {
                    BuscarTrasladosDeVentasAlContado();
                }
            });
        });
    });
}


/* --------------------- Traslados de Ventas Al Credito ----------------------- */

function BuscarPedidosAlCredito() {
    setI("uiTitlePopupPedidos", "Pedidos al Crédito");
    document.getElementById("ShowPopupPedidos").click();
    MostrarPedidosAlCredito();
}

function MostrarPedidosAlCredito() {
    let fechaStr = document.getElementById("uiFechaCreditoStr").value;
    if (fechaStr == "") {
        Warning("No existe fecha de crédito");
        return;
    }

    let fechaCreditoStr = convertFormatDate(fechaStr);
    fetchGet("Pedidos/GetPedidosAlCredito/?fechaCreditoStr=" + fechaCreditoStr, "json", function (rpta) {
        if (rpta == undefined || rpta == null) {
            Warning("No existe informacion")
        } else {
            let codigoPedido = -1;
            let observaciones = "";
            if (rpta.length > 0) {
                codigoPedido = Number(rpta[0]["numeroPedido"].toString());
                observaciones = rpta[0]["observaciones"].toString();
            }

            if (codigoPedido == 0) {
                MensajeError(observaciones);
            } else {
                let objConfiguracion = {
                    cabeceras: ["codigoEmpresa", "seriePedido", "Pedido", "Cliente", "Nombre Cliente", "Monto", "serieFactura", "numeroFactura", "numeroVale", "numeroPedidoQSystems","permisoSelect"],
                    propiedades: ["codigoEmpresa", "seriePedido", "numeroPedido", "codigoCliente", "nombreCliente", "monto", "serieFactura","numeroFactura","numeroVale","numeroPedidoQSystems","permisoSelect"],
                    displaydecimals: ["montoTotal"],
                    divContenedorTabla: "divContenedorTablaPedidos",
                    divPintado: "divTablaPedidos",
                    idtabla: "tablaPedidos",
                    aceptarselect: true,
                    funcionaceptarselect: "PedidoConCreditoParaPago",
                    paginar: true,
                    ocultarColumnas: true,
                    hideColumns: [
                        {
                            "targets": [0],
                            "visible": false
                        }, {
                            "targets": [1],
                            "visible": false
                        }, {
                            "targets": [2],
                            "className": "dt-body-center",
                            "visible": true
                        }, {
                            "targets": [3],
                            "className": "dt-body-center",
                            "visible": true
                        }, {
                            "targets": [5],
                            "className": "dt-body-right",
                            "visible": true
                        }, {
                            "targets": [6],
                            "visible": false
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
                        }
                    ],
                    slug: "codigoTraslado",
                    autoWidth: false
                }
                pintarEntidades(objConfiguracion, rpta);
            }
        }
    });
}


function AceptarSelectPedidoConCreditoParaPago() {
    let table = $('#tablaPedidos').DataTable();
    $('#tablaPedidos tbody').on('click', '.option-select', function () {
        let rowIdx = table.row(this).index();
        set("uiCodigoEmpresa", table.cell(rowIdx, 0).data());
        set("uiSeriePedido", table.cell(rowIdx, 1).data());
        set("uiNumeroPedido", table.cell(rowIdx, 2).data());
        set("uiCodigoCliente", table.cell(rowIdx, 3).data());
        set("uiNombreCliente", table.cell(rowIdx, 4).data());
        set("uiMontoEfectivo", table.cell(rowIdx, 5).data());
        set("uiSerieFactura", table.cell(rowIdx, 6).data());
        set("uiNumeroFactura", table.cell(rowIdx, 7).data());
        set("uiNumeroVale", table.cell(rowIdx, 8).data());
        set("uiNumeroPedidoQSystems", table.cell(rowIdx, 9).data());

    });
    document.getElementById("uiClosePopupPedidos").click();


}