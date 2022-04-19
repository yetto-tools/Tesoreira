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
                    ListarVendedores(-1, 0);
                    break;
                default:
                    break;
            }// fin switch
        }// fin if
    }// fin if
  
}

function FillComboCanalVenta(idcontrol) {
    fetchGet("CanalesVentas/GetCanalesDeVentas", "json", function (rpta) {
        if (rpta != null) {
            FillCombo(rpta, idcontrol, "codigoCanalVenta", "canalVenta", "- seleccione -", "-1");
        } else {
            FillComboUnicaOpcion(idcontrol, "-1", "-- No existen canales de venta -- ");
        }
    })
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
    let incluirBloqueados = 0;
    let codigoCanalVenta = parseInt(document.getElementById("uiCodigoCanalVenta").value);
    let checkIncluirBloqueados = document.getElementById("uiIncluirBloqueados").checked;
    if (checkIncluirBloqueados == true) {
        incluirBloqueados = 1;
    }
    ListarVendedores(codigoCanalVenta, incluirBloqueados);
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

function ListarVendedores(codigoCanalVenta, incluirBloqueados) {
    let objConfiguracion = {
        url: "Vendedores/GetListaVendedores/?codigoCanalVenta=" + codigoCanalVenta.toString() + "&incluirBloqueados=" + incluirBloqueados.toString(),
        cabeceras: ["Código", "Nombre Vendedor", "Código Canal Venta", "Canal de Venta","Ruta","Estado","Anular","Editar"],
        propiedades: ["codigoVendedor", "nombreVendedor", "codigoCanalVenta", "canalVenta", "ruta","estadoRutaVendedor","permisoAnular","permisoEditar"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        paginar: true,
        editar: true,
        funcioneditar: "EstadoVendedorRuta",
        eliminar: true,
        funcioneliminar: "RutaVendedor",
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [2],
                "visible": false
            }, {
                "targets": [6],
                "visible": false
            }, {
                "targets": [7],
                "visible": false
            }],
        slug: "codigoVendedor"
    }
    pintar(objConfiguracion);
}


function EliminarRutaVendedor(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', 'tr', 'input:button', function () {
        let rowIdx = table.row(this).index();
        let codigoVendedor = table.cell(rowIdx, 0).data();
        let nombreVendedor = table.cell(rowIdx, 1).data();
        let codigoCanalVenta = parseInt(table.cell(rowIdx, 2).data());
        let canalVenta = table.cell(rowIdx, 3).data();
        let ruta = parseInt(table.cell(rowIdx, 4).data());
        Confirmacion("Bloqueo de Ruta de Vendedor", "¿Desea bloquear el canal de venta " + canalVenta + " de la ruta " + ruta + " del vendedor " + nombreVendedor + "?", function (rpta) {
            fetchGet("Vendedores/BloquearVendedorRuta/?codigoVendedor=" + codigoVendedor + "&codigoCanalVenta=" + codigoCanalVenta.toString() + "&ruta=" + ruta.toString(), "text", function (data) {
                if (data == "OK") {
                    ListarVendedores(-1, 0);
                } else {
                    MensajeError(data);
                }
            })
        })
    });
}

function EditarEstadoVendedorRuta(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', 'tr', 'input:button', function () {
        let rowIdx = table.row(this).index();
        let codigoVendedor = table.cell(rowIdx, 0).data();
        let nombreVendedor = table.cell(rowIdx, 1).data();
        let codigoCanalVenta = parseInt(table.cell(rowIdx, 2).data());
        let canalVenta = table.cell(rowIdx, 3).data();
        let ruta = parseInt(table.cell(rowIdx, 4).data());
        Confirmacion("Estado de Ruta de Vendedor", "¿Desea desbloquear el canal de venta " + canalVenta + " de la ruta " + ruta + " del vendedor " + nombreVendedor + "?", function (rpta) {
            fetchGet("Vendedores/DesbloquearVendedorRuta/?codigoVendedor=" + codigoVendedor + "&codigoCanalVenta=" + codigoCanalVenta.toString() + "&ruta=" + ruta.toString(), "text", function (data) {
                if (data == "OK") {
                    ListarVendedores(-1, 0);
                } else {
                    MensajeError(data);
                }
            })
        })
    });
}


function setDataNuevoVendedor() {
    setI("uiTitlePopupNewVendedor", "Nueva Vendedor");
    set("uiNewCodigoVendedor", "");
    set("uiNewNombreVendedor", "");
}

function GuardarVendedorRuta() {
    let errores = ValidarDatos("frmAsignacionRutaVendedor")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmAsignacionRutaVendedor");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Vendedores/GuardarVendedorRuta", "text", frm, function (data) {
            if (data == "OK") {
                ListarVendedores(-1, 0);
            } else {
                MensajeError(data);
            }
            document.getElementById("uiClosePopupRutaVendedor").click();
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
    set("uiEditRuta", "");
    set("uiEditNombre", "");
    set("uiEditNombreCorto", "");
    set("uiEditDescripcion", "");
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