window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    let codigoTransaccion = "0";
    if (nameController == "Transaccion") {
        switch (nameAction) {
            case "New":
                fillCombosNew();
                document.getElementById("uiNitEmpresaConcedeIva").checked = false;
                setComboConsumidorConcedeIVA();
                FillComboUnicaOpcion("uiNumeroCuenta", "-1", "-- No existe cuenta -- ");
                clearDataFormulario();
                intelligenceSearch();
                let setSemanaAnterior = parseInt(document.getElementById("uiSetSemanaAnterior").value);
                if (setSemanaAnterior == 1) {
                    fillComboSemanaAnterior();
                }
                break;
            case "Edit":
                codigoTransaccion = url1.searchParams.get("codigoTransaccion");
                fillCombosEdit(codigoTransaccion);
                document.getElementById("uiNitEmpresaConcedeIva").checked = false;
                setComboConsumidorConcedeIVA();
                FillComboUnicaOpcion("uiNumeroCuenta", "-1", "-- No existe cuenta -- ");
                intelligenceSearch();
                break;
            case "EditRevision":
                codigoTransaccion = url1.searchParams.get("codigoTransaccion");
                fillCombosEdit(codigoTransaccion);
                document.getElementById("uiNitEmpresaConcedeIva").checked = false;
                setComboConsumidorConcedeIVA();
                FillComboUnicaOpcion("uiNumeroCuenta", "-1", "-- No existe cuenta -- ");
                clearDataFormulario();
                intelligenceSearch();
                break;
            default:
                break;
        }// fin switch
    }// fin if
}

function clearDataTransaccion() {
    set("uiCodigoCanalVenta", "0");
    set("uiCodigoVendedor", "");
    set("uiCodigoCategoriaVendedor","0");
    set("uiNombreVendedor", "");
    set("uiCodigoArea", "0");
    set("uiCodigoEntidad", "");
    set("uiCodigoCategoriaEntidad", "0");
    set("uiNombreEntidad", "");
    set("uiObservaciones", "");
}


function clearDataDepositosBancarios() {
    let elementBanco = document.getElementById("uiBanco");
    let elementNumeroCuenta = document.getElementById("uiNumeroCuenta");
    let elementNumeroBoleta = document.getElementById("uiNumeroBoleta");
    let elementMontoEfectivo = document.getElementById("uiMontoEfectivo");
    let elementMontoCheques = document.getElementById("uiMontoCheques");
    
    elementBanco.selectedIndex = 0;
    elementBanco.classList.remove('obligatorio');
    elementNumeroCuenta.classList.remove('obligatorio');
    elementNumeroBoleta.classList.remove('obligatorio');
    elementMontoEfectivo.classList.remove('obligatorio');
    elementMontoCheques.classList.remove('obligatorio');
}

function clearDataPlanilla() {
    FillComboUnicaOpcion("uiAnioPlanilla", "-1", "-- No Aplica -- ");
    document.getElementById('uiCodigoFrecuenciaPago').selectedIndex = 0;
    FillComboUnicaOpcion("uiMesPlanilla", "0", "-- No Aplica -- ");
    FillComboUnicaOpcion("uiSemanaPlanilla", "0", "-- No Aplica -- ");
    FillComboUnicaOpcion("uiCodigoQuincenaPlanilla", "-1", "-- No Aplica -- ");
    let elementAnioPlanilla = document.getElementById("uiAnioPlanilla");
    let elementTipoPlanilla = document.getElementById("uiCodigoFrecuenciaPago");
    let elementMesPlanilla = document.getElementById("uiMesPlanilla");
    let elementSemanaPlanilla = document.getElementById("uiSemanaPlanilla");
    let elementQuincenaPlanilla = document.getElementById("uiCodigoQuincenaPlanilla");
    let elementTipoPagoPlanilla = document.getElementById("uiCodigoTipoPago");
    elementAnioPlanilla.classList.remove('obligatorio');
    elementTipoPlanilla.classList.remove('obligatorio');
    elementMesPlanilla.classList.remove('obligatorio');
    elementSemanaPlanilla.classList.remove('obligatorio');
    elementQuincenaPlanilla.classList.remove('obligatorio');
    elementTipoPagoPlanilla.classList.remove('obligatorio');
    elementTipoPlanilla.selectedIndex = 0;
    elementTipoPagoPlanilla.selectedIndex = 0;
}

function clearDataFormulario() {
    set("uiCodigoOperacionCaja", "0");
    document.getElementById("uiTitleTipoDocumentoDeposito").innerHTML = "Número de Documento";
    document.getElementById("uiOptionTipoDocumentoNumeroVoucher").checked = false;
    document.getElementById("uiNumeroBoleta").readOnly = true;
    document.getElementById("uiOptionTipoTransaccionNF").checked = true;

    clearDataTransaccion();
    // inicializa los datos de planilla
    document.getElementById('uiEsAjustePlanilla').value = "0";
    let elementTipoPlanillaPago = document.getElementById('div-tipo-planilla-pago');
    elementTipoPlanillaPago.style.display = 'none'
    elementTipoPlanillaPago.checked = false;
    clearDataPlanilla();
    // Inicializa datos de comisión (Bonos Extras)
    let elementOptionBonoExtraComision = document.getElementById("uiOptionPorComisiones");
    let elementOptionBonoQuintalaje = document.getElementById("uiOptionBonoQuintalaje");
    let elementOptionFeriadosODomingos = document.getElementById("uiOptionBonosFeriadosODomingos");
    let elementOptionBonosExtras = document.getElementById("uiOptionBonosOtros");
    elementOptionBonoExtraComision.checked = false;
    elementOptionBonoQuintalaje.checked = false;
    elementOptionFeriadosODomingos.checked = false;
    elementOptionBonosExtras.checked = false;
    set("uiNumeroRuta", "");
    FillComboUnicaOpcion("uiSemanaComision", "-1", "-- No Aplica -- ");
    FillComboUnicaOpcion("uiAnioComision", "-1", "-- No Aplica -- ");
    let elementRuta = document.getElementById("uiNumeroRuta");
    let elementSemanaComision = document.getElementById("uiSemanaComision");
    let elementAnioComision = document.getElementById("uiAnioComision");
    elementRuta.classList.remove('obligatorio');
    elementSemanaComision.classList.remove('obligatorio');
    elementAnioComision.classList.remove('obligatorio');
    // Inicialización de datos
    let elementOptionEspecialCliente = document.getElementById("uiOptionEspecialCliente");
    let elementOptionEspecdialOtrosIngresos = document.getElementById("uiOptioneEspecialOtrosIngresos");
    elementOptionEspecialCliente.checked = false;
    elementOptionEspecdialOtrosIngresos.checked = false;
    // Datos de los depositos bancarios
    document.getElementById('div-forma-pago-deposito').style.display = 'none';
    clearDataDepositosBancarios();
    // Tipo Documento
    document.getElementById("uiVale").disabled = false;
    document.getElementById("uiFactura").disabled = false;
    document.getElementById("uiBoletaDeposito").disabled = false;
    document.getElementById("uiNingunTipoDocumento").disabled = false;

    document.getElementById("uiVale").checked = false;
    document.getElementById("uiFactura").checked = false;
    document.getElementById("uiBoletaDeposito").checked = false;
    document.getElementById("uiNingunTipoDocumento").checked = true;  // Default
    document.getElementById('uiNingunTipoDocumento').onClick = setDataAdicionaTipoDocumento("0");
    // Forma de Pago
    document.getElementById("uiEfectivo").disabled = false;
    document.getElementById("uiDeposito").disabled = false;
    document.getElementById("uiCheque").disabled = false;

    document.getElementById("uiEfectivo").checked = true;  // Default
    document.getElementById("uiDeposito").checked = false;
    document.getElementById("uiCheque").checked = false;
    // Préstamos
    emptyComboTipoCuentaPorCobrar();
    document.getElementById('div-tipo-cuenta-por-cobrar').style.display = 'none';
    // Gastos Indirectos
    document.getElementById('div-tipo-gasto-indirecto').style.display = 'none';
    document.getElementById('uiEsSueldosIndirectos').value = "0";
    document.getElementById('uiEsSueldosIndirectos').checked = false;
    document.getElementById('div-sueldo-indirecto-entidad-don-pepe').style.display = 'none';
    let elementAnioSueldoIndirecto = document.getElementById("uiAnioSueldoIndirecto");
    let elementMesSueldoIndirecto = document.getElementById("uiMesSueldoIndirecto");
    elementAnioSueldoIndirecto.classList.remove('obligatorio');
    elementMesSueldoIndirecto.classList.remove('obligatorio');
    FillComboUnicaOpcion("uiAnioSueldoIndirecto", "-1", "-- No aplica -- ");
    FillComboUnicaOpcion("uiMesSueldoIndirecto", "-1", " -- Sin Meses -- ");

    // Ventas en Ruta
    document.getElementById('div-ventas-en-ruta').style.display = 'none';
    FillComboUnicaOpcion("uiRutaVendedor", "-1", "-- Sin Ruta -- ");
    let elementRutaVendedor = document.getElementById("uiRutaVendedor");
    elementRutaVendedor.classList.remove('obligatorio');

    // Proveedor
    document.getElementById('div-captura-proveedor').style.display = 'none';
    let elementNombreProveedor = document.getElementById('uiNombreProveedor');
    elementNombreProveedor.value = "";
    elementNombreProveedor.classList.remove('obligatorio');

    // Especiales 1
    let elementOtrosIngresos = document.getElementById("uiCodigoOtroIngreso");
    elementOtrosIngresos.classList.remove('obligatorio');
    document.getElementById('div-otros-ingresos').style.display = 'none';
}

function setValueNumeroDocumentoDeposito(obj) {
    let codigoTipoDocumentoDeposito = parseInt(obj.value);
    document.getElementById("uiNumeroBoleta").readOnly = false;
    switch (codigoTipoDocumentoDeposito) {
        case 1:
            document.getElementById("uiTitleTipoDocumentoDeposito").innerHTML = "Número de Boleta";
            break;
        case 2:
            document.getElementById("uiTitleTipoDocumentoDeposito").innerHTML = "Número de Vaucher";
            break;
    }
}


function sumarMontos() {
    let montoEfectivoStr = document.getElementById("uiMontoEfectivo").value;
    let montoChequesStr = document.getElementById("uiMontoCheques").value;
    let monto = document.getElementById("uiMontoTransaccion").value;
    
    let montoEfectivo = 0.00;
    let montoCheques = 0.00;
    if (montoEfectivoStr != "") {
        montoEfectivo = parseFloat(montoEfectivoStr);
    }
    if (montoChequesStr != "") {
        montoCheques = parseFloat(montoChequesStr);
    }
    if (montoEfectivo > 0 || montoCheques > 0) {
        let montoTotal = montoEfectivo + montoCheques;
        document.getElementById("uiMontoTransaccion").value = montoTotal.toString();
    }

}

function fillAnioPlanilla() {
    let arrayDate = getFechaSistema();
    let numeroMes = arrayDate[0]["mes"];
    let anioActual = arrayDate[0]["anio"];
    let anioAnterior = anioActual - 1;
    let select = document.getElementById("uiAnioPlanilla");
    if (numeroMes == 1) {
        let data = [{ "value": anioAnterior.toString(), "text": anioAnterior.toString() }, { "value": anioActual.toString(), "text": anioActual.toString() }]
        FillCombo(data, "uiAnioPlanilla", "value", "text", "- seleccione -", "-1");
        select.selectedIndex = 1;

    } else {
        let data = [{ "value": anioActual.toString(), "text": anioActual.toString()}]
        FillCombo(data, "uiAnioPlanilla", "value", "text", "- seleccione -", "-1");
        select.selectedIndex = 0;
    }
}

function fillAnioSueldosIndirectorDonPepe() {
    let arrayDate = getFechaSistema();
    let numeroMes = arrayDate[0]["mes"];
    let anioActual = arrayDate[0]["anio"];
    let anioAnterior = anioActual - 1;
    let select = document.getElementById("uiAnioSueldoIndirecto");
    if (numeroMes == 1) {
        let data = [{ "value": anioActual.toString(), "text": anioActual.toString() }, { "value": anioAnterior.toString(), "text": anioAnterior.toString()}]
        FillCombo(data, "uiAnioSueldoIndirecto", "value", "text", "- seleccione -", "-1");
        select.selectedIndex = 1;
        fillMesesSueldosIndirectosDonPepe(anioActual);
    } else {
        let data = [{ "value": anioActual.toString(), "text": anioActual.toString() }]
        FillCombo(data, "uiAnioSueldoIndirecto", "value", "text", "- seleccione -", "-1");
        select.selectedIndex = 0;
        fillMesesSueldosIndirectosDonPepe(anioActual);
    }
}

function fillComboNewOtrosIngresos(obj) {
    let valor = parseInt(obj);
    // 1. POR CLIENTES
    // 2. INGRESOS VARIOS
    let elementOtrosIngresos = document.getElementById("uiCodigoOtroIngreso");
    if (valor == TIPO_ESPECIAL1_OTROS_INGRESOS) {
        fetchGet("Configuracion/GetListaOtrosIngresos", "json", function (rpta) {
            FillCombo(rpta, "uiCodigoOtroIngreso", "codigoOtroIngreso", "nombre", "- seleccione -", "-1");
            elementOtrosIngresos.classList.add('obligatorio');
            document.getElementById('div-otros-ingresos').style.display = 'block';
        })
    } else {
        elementOtrosIngresos.classList.remove('obligatorio');
        document.getElementById('div-otros-ingresos').style.display = 'none';
    }
}

function fillCombosNew() {
    fetchGet("Transaccion/FillCombosNuevaTransaccion/?codigoTipoOperacion=1", "json", function (rpta) {
        let listaOperaciones = rpta.listaOperaciones;
        let listaProgramacionSemanal = rpta.listaProgramacionSemanal;
        let listaEmpresas = rpta.listaEmpresasTesoreria;
        let numeroSemana = rpta.numeroSemana;
        let anioOperacion = rpta.anioOperacion;
        FillCombo(listaEmpresas, "cboEmpresas", "codigoEmpresa", "nombreComercial", "- seleccione -", "-1");
        FillCombo(listaOperaciones, "cboOperacion", "codigoOperacion", "nombre","- seleccione -","-1");
        fillProgramacionSemanal(listaProgramacionSemanal);
        setN("SemanaOperacion", numeroSemana);
        setN("AnioOperacion", anioOperacion);
    })
}

function fillCombosEdit(codigoTransaccion) {
    fetchGet("Transaccion/GetDataTransaccion/?codigoTransaccion=" + codigoTransaccion, "json", function (data) {
        if (data == null || data == undefined || data.length == 0) {
            MensajeError("Error en la búsqueda de la transacción " + codigoTransaccion);
        } else {
            let semanaOperacion = data.semanaOperacion;
            let anioOperacion = data.anioOperacion;
            let codigoTipoOperacion = data.signo;
            let codigoOperacion = data.codigoOperacion;
            fetchGet("Transaccion/FillCombosEditTransaccion/?codigoTipoOperacion=" + codigoTipoOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString() + "&anioOperacion=" + anioOperacion.toString(), "json", function (rpta) {

                let listaOperaciones = rpta.listaOperaciones;
                let listaProgramacionSemanal = rpta.listaProgramacionSemanal;
                let listaEmpresas = rpta.listaEmpresasTesoreria;

                FillCombo(listaEmpresas, "cboEmpresas", "codigoEmpresa", "nombreComercial", "- seleccione -", "-1");
                FillCombo(listaOperaciones, "cboOperacion", "codigoOperacion", "nombre", "- seleccione -", "-1");
                fillProgramacionSemanal(listaProgramacionSemanal);

                clearDataFormulario();
                set("uiCodigoTransaccion", data.codigoTransaccion.toString());
                set("uiNumeroSemanaActual", semanaOperacion.toString());
                set("uiAnioOperacion", anioOperacion.toString());
                setCheckedValueOfRadioButtonGroup('CodigoTipoOperacion', codigoTipoOperacion.toString());
                setCheckedValueOfRadioButtonGroup('CodigoTipoTransaccion', data.codigoTipoTransaccion);
                setCheckedValueOfRadioButtonGroup('FechaStr', data.fechaOperacionStr);
                document.querySelector('#cboOperacion').value = codigoOperacion.toString();
                setCheckedValueOfRadioButtonGroup('CodigoTipoDocumento', data.codigoTipoDocumento);
                document.querySelector('#uiEfectivo').checked = data.efectivo == 1 ? true : false;
                document.querySelector('#uiDeposito').checked = data.deposito == 1 ? true : false;
                document.querySelector('#uiCheque').checked = data.cheque == 1 ? true : false;
                set("uiCodigoEntidad", data.codigoEntidad);
                set("uiCodigoCategoriaEntidad", data.codigoCategoriaEntidad.toString());
                set("uiCategoriaEntidad", data.categoriaEntidad);
                set("uiCodigoCanalVenta", data.codigoCanalVenta.toString());
                set("uiNombreEntidad", data.nombreEntidad);
                set("uiMontoTransaccion", Number(data.monto).toFixed(2).toString());
                set("uiObservaciones", data.observaciones);
                set("uiCodigoReporte", data.codigoReporte.toString());
                set("uiCodigoArea", data.codigoArea.toString());
                set("uiCodigoOperacionCaja", data.codigoOperacionCaja.toString());
                set("uiNumeroRecibo", data.numeroRecibo.toString());
                set("uiFechaReciboStr", data.fechaReciboStr);
                set("uiNombreProveedor", data.nombreProveedor);
                setDataControls(codigoOperacion, data);
            })
        }
    });
}


function fillComboSemana(obj) {
    let semanaOperacion = parseInt(document.getElementById("uiNumeroSemanaActual").value);
    let anioOperacion = parseInt(document.getElementById("uiAnioOperacion").value);
    let anioTemp = anioOperacion;
    let habilitarSemanaAnterior = 0;
    if (obj.checked) {
        // Validar que no se haya generado el reporte de esa semana
        habilitarSemanaAnterior = 1;
        if (semanaOperacion == 1) {
            anioOperacion = anioOperacion - 1;
            setN("AnioOperacion", anioOperacion);
        }
        fetchGet("Transaccion/FillComboSemana/?habilitarSemanaAnterior=" + habilitarSemanaAnterior.toString(), "json", function (rpta) {
            let listaProgramacionSemanal = rpta.listaProgramacionSemanal;
            let numeroSemana = rpta.numeroSemana;
            fetchGet("CorteCajaSemanal/ExisteReporte/?anio=" + anioOperacion.toString() + "&numeroSemana=" + numeroSemana.toString(), "text", function (rpta) {
                if (rpta == "0") {
                    fillProgramacionSemanal(listaProgramacionSemanal);
                    setN("SemanaOperacion", numeroSemana);
                } else {
                    setN("AnioOperacion", anioTemp);
                    document.getElementById("uiCheckSemanaAnterior").checked = false;
                    Warning("No se puede registrar en semana anterior, ya existe reporte generado de esa semana");
                }
            });
        })
    } else {
        let arrayDate = getFechaSistema();
        let anioActual = arrayDate[0]["anio"];
        setN("AnioOperacion", anioActual);
        fetchGet("Transaccion/FillComboSemana/?habilitarSemanaAnterior=" + habilitarSemanaAnterior.toString(), "json", function (rpta) {
            let listaProgramacionSemanal = rpta.listaProgramacionSemanal;
            let numeroSemana = rpta.numeroSemana;
            fillProgramacionSemanal(listaProgramacionSemanal);
            setN("SemanaOperacion", numeroSemana);
        })
    }
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
    });
}

async function fillProgramacionSemanal(res) {
    let objGlobalConfigTransaccion = {
        propiedadnombre: "FechaStr",
        propiedades: ["fechaStr", "dia"],
        propiedadid: "uiFechaOperacion",
        divContenedorTabla: "divContainerTablaRadioButtonList",
        divPintado: "divTablaRadioButtonList"
    }
    pintarRadioButtonList(objGlobalConfigTransaccion, res);
}

function fillComboOpciones() {
    //ECMAScript 6 version
    let valor = parseInt(Array.from(document.getElementsByName("CodigoTipoOperacion")).find(r => r.checked).value, 10);
    fetchGet("Operacion/FillComboOperacion/?codigoTipoOperacion=" + valor.toString(), "json", function (rpta) {
        let listaOperaciones = rpta.listaOperaciones;
        if (listaOperaciones.length > 0) {
            FillCombo(listaOperaciones, "cboOperacion", "codigoOperacion", "nombreReporteCaja", "- seleccione -", "-1");
        }
    })
    //  1.INGRESOS
    // -1.EGRESOS
    if (valor == 1) { // INGRESO
        document.getElementById("uiEfectivo").checked = true;
        document.getElementById("uiDeposito").disabled = false;
        document.getElementById("uiCheque").disabled = false;
    } else {
        document.getElementById("uiEfectivo").checked = true;
        document.getElementById("uiDeposito").disabled = true;
        document.getElementById("uiCheque").disabled = true;
    }
    showControls("-1");
 }


function fillCuentasBancarias(obj) {
    let valor = parseInt(obj.value, 10);
    let codigoOperacion = parseInt(document.getElementById('cboOperacion').value)
    if (codigoOperacion != BACK_TO_BACK) {
        set("uiCodigoEntidad", valor.toString());
        if (obj.selectedIndex > 0) {
            set("uiNombreEntidad", obj.options[obj.selectedIndex].text);
        }
        else {
            set("uiNombreEntidad", "BANCO");
        }
    }
    fetchGet("CuentaBancaria/GetCuentasBancariasTesoreria/?codigoBanco=" + valor.toString(), "json", function (rpta) {
        if (rpta.length > 0) {
            FillCombo(rpta, "uiNumeroCuenta", "numeroCuenta", "numeroCuentaDescriptivo", "- seleccione cuenta -", "-1");
        } else {
            FillComboUnicaOpcion("uiNumeroCuenta", "-1", "-- No existe cuenta -- ");
        }
    })
}


function fillPeriodosDeComision(obj) {
    let numeroSemanaActual = document.getElementById("uiNumeroSemanaActual").value;
    let anioOperacion = parseInt(document.getElementById("uiAnioOperacion").value);
    let anioComision = parseInt(obj.value, 10);
    let ultimaSemanaAnioAnterior = "0";
    if (anioComision < anioOperacion) {
        ultimaSemanaAnioAnterior = "1";
    }
    fetchGet("ProgramacionSemanal/GetListaSemanasComision/?anio=" + obj.value + "&numeroSemana=" + numeroSemanaActual + "&ultimaSemanaAnioAnterior=" + ultimaSemanaAnioAnterior, "json", function (rpta) {
        if (rpta.length > 0) {
            FillCombo(rpta, "uiSemanaComision", "semanaComision", "periodo", "- seleccione semana -", "-1");
        } else {
            FillComboUnicaOpcion("uiSemanaComision", "-1", "-- No existen semanas -- ");
        }
    })
}

function fillAnioComision() {
    let contenido = "";
    let arrayDate = getFechaSistema();
    let mesActual = arrayDate[0]["mes"];
    let anioActual = arrayDate[0]["anio"];
    let anioAnterior = anioActual - 1;
    let anioOperacion = parseInt(document.getElementById("uiAnioOperacion").value);
    let numeroSemanaActual = document.getElementById("uiNumeroSemanaActual").value;

    if (mesActual == 1 && anioActual == anioOperacion) {
        contenido += "<option selected = 'selected' value = '" + anioOperacion.toString() + "'>" + anioOperacion.toString() + "</option>"
        contenido += "<option value = '" + anioAnterior.toString() + "'>" + anioAnterior.toString() + "</option>"
        document.getElementById("uiAnioComision").selectedIndex = "0";
        document.getElementById("uiAnioComision").innerHTML = contenido;
    } else {
        contenido += "<option selected = 'selected' value = '" + anioOperacion.toString() + "'>" + anioOperacion.toString() + "</option>"
        document.getElementById("uiAnioComision").selectedIndex = "0";
        document.getElementById("uiAnioComision").innerHTML = contenido;
    }

    fetchGet("ProgramacionSemanal/GetListaSemanasComision/?anio=" + anioOperacion.toString() + "&numeroSemana=" + numeroSemanaActual + "&ultimaSemanaAnioAnterior=0", "json", function (rpta) {
        if (rpta.length > 0) {
            FillCombo(rpta, "uiSemanaComision", "semanaComision", "periodo", "- seleccione semana -", "-1");
        } else {
            FillComboUnicaOpcion("uiSemanaComision", "-1", "-- No existen semanas -- ");
        }
    })

    // Agregar obligatoriedad de los campos
    let elementRuta = document.getElementById("uiNumeroRuta");
    let elementSemanaComision = document.getElementById("uiSemanaComision");
    let elementAnioComision = document.getElementById("uiAnioComision");
    elementRuta.classList.add('obligatorio');
    elementSemanaComision.classList.add('obligatorio');
    elementAnioComision.classList.add('obligatorio');
}

function setCamposObligatoriosDepositosBancarios() {
    let elementBanco = document.getElementById("uiBanco");
    let elementNumeroCuenta = document.getElementById("uiNumeroCuenta");
    let elementNumeroBoleta = document.getElementById("uiNumeroBoleta");
    let elementMontoEfectivo = document.getElementById("uiMontoEfectivo");
    let elementMontoCheques = document.getElementById("uiMontoCheques");
    elementBanco.classList.add('obligatorio');
    elementNumeroCuenta.classList.add('obligatorio');
    elementNumeroBoleta.classList.add('obligatorio');
    elementMontoEfectivo.classList.add('obligatorio');
    elementMontoCheques.classList.add('obligatorio');
}

function showContainerBonosExtrasPorComisiones() {
    clearDataTransaccion();
    document.getElementById('div-bonos-extra').style.display = 'block';
    let elementRuta = document.getElementById("uiNumeroRuta");
    let elementSemanaComision = document.getElementById("uiSemanaComision");
    let elementAnioComision = document.getElementById("uiAnioComision");
    fillAnioComision();
    elementRuta.classList.add('obligatorio');
    elementSemanaComision.classList.add('obligatorio');
    elementAnioComision.classList.add('obligatorio');
}


function showContainerBonosExtrasOtros() {
    clearDataTransaccion();
    document.getElementById('div-bonos-extra').style.display = 'none';
    let elementRuta = document.getElementById("uiNumeroRuta");
    let elementSemanaComision = document.getElementById("uiSemanaComision");
    let elementAnioComision = document.getElementById("uiAnioComision");
    set("uiNumeroRuta", "");
    FillComboUnicaOpcion("uiSemanaComision", "-1", "-- No Aplica -- ");
    FillComboUnicaOpcion("uiAnioComision", "-1", "-- No Aplica -- ");
    elementRuta.classList.remove('obligatorio');
    elementSemanaComision.classList.remove('obligatorio');
    elementAnioComision.classList.remove('obligatorio');
    let optionQuintalaje = document.getElementById("uiOptionBonoQuintalaje");
    let optionFeriadosyDomingos = document.getElementById("uiOptionBonosFeriadosODomingos");
    if (optionQuintalaje.checked == true || optionFeriadosyDomingos.checked == true) {
        set("uiCodigoCategoriaEntidad", CATEGORIA_EMPLEADO.toString());
        set("uiCodigoArea", "0");
        if (optionQuintalaje.checked == true) {
            set("uiCodigoEntidad", ENTIDAD_EMPLEADOS.toString());
            set("uiNombreEntidad", "PANADEROS");
        } else {
            set("uiCodigoEntidad", ENTIDAD_EMPLEADOS.toString());
            set("uiNombreEntidad", "EMPLEADOS");
        }
    } else {
        set("uiCodigoArea", "0");
        set("uiCodigoEntidad", "");
        set("uiCodigoCategoriaEntidad", "0");
        set("uiNombreEntidad", "");
    }
}

function showPlanillaPago(obj) {
    clearDataPlanilla();
    if (obj.checked == true) { // Es ajuste de planilla
        document.getElementById('div-planilla-pago').style.display = 'none';
        document.getElementById('uiEsAjustePlanilla').value = "1";
        document.getElementById('divTabla').style.display = 'block';

    } else {
        document.getElementById('div-planilla-pago').style.display = 'block';
        document.getElementById('uiEsAjustePlanilla').value = "0";
    }
}

function setSueldosIndirectosDonPepe(habilitar, checked) {
    let elementAnioSueldoIndirecto = document.getElementById("uiAnioSueldoIndirecto");
    let elementMesSueldoIndirecto = document.getElementById("uiMesSueldoIndirecto");
    let elementCheckSueldoIndirecto = document.getElementById("uiEsSueldosIndirectos");
    if (habilitar == true) {
        document.getElementById('div-sueldo-indirecto-entidad-don-pepe').style.display = 'block';
        elementCheckSueldoIndirecto.checked = checked;
        elementCheckSueldoIndirecto.value = checked == true ? "1": "0";
        elementAnioSueldoIndirecto.classList.add('obligatorio');
        elementMesSueldoIndirecto.classList.add('obligatorio');
        fillAnioSueldosIndirectorDonPepe();
    } else {
        document.getElementById('div-sueldo-indirecto-entidad-don-pepe').style.display = 'none';
        elementCheckSueldoIndirecto.checked = checked;
        elementCheckSueldoIndirecto.value = checked == true ? "1" : "0";
        elementAnioSueldoIndirecto.classList.remove('obligatorio');
        elementMesSueldoIndirecto.classList.remove('obligatorio');
        FillComboUnicaOpcion("uiAnioSueldoIndirecto", "-1", "-- No aplica -- ");
        FillComboUnicaOpcion("uiMesSueldoIndirecto", "-1", "-- Sin Meses -- ");
    }
}

function setTipoGastoIndirecto(obj) {
    if (obj.checked == true) { // es por sueldo indirecto
        let codigoEntidad = get("uiCodigoEntidad");
        let codigoCategoriaEntidad = parseInt(get("uiCodigoCategoriaEntidad"));
        if (codigoCategoriaEntidad == CATEGORIA_SOCIO && codigoEntidad == ENTIDAD_DON_PEPE) {
            setSueldosIndirectosDonPepe(true, true);
        } else {
            setSueldosIndirectosDonPepe(false, true);
        }
    } else {
        setSueldosIndirectosDonPepe(false, false);
        set("uiCodigoArea", "0");
        set("uiCodigoEntidad", "");
        set("uiCodigoCategoriaEntidad", "0");
        set("uiNombreEntidad", "");
        set("uiObservaciones", "");
        let table = $('#tabla').DataTable();
        table.$("input[type=radio]").prop("checked", false);
    }
}

function MostrarEntidadesGenericas(value) {
    if (value == "F") {
        set("uiCodigoCategoriaEntidad", "");
        set("uiCodigoEntidad", "");
        set("uiCodigoArea", "");
        set("uiNombreEntidad", "");
        document.getElementById('divTabla').style.display = 'block';
    } else {
        set("uiCodigoCategoriaEntidad", CATEGORIA_EMPLEADO.toString());
        set("uiCodigoEntidad", ENTIDAD_EMPLEADOS.toString());
        set("uiCodigoArea", "0");
        set("uiNombreEntidad", "EMPLEADOS");
        document.getElementById('divTabla').style.display = 'none';
    }
}


function showControls(obj) {
    let valor = parseInt(obj);
    let table = $('#tabla').DataTable();
    table.$("input[type=radio]").prop("checked", false);
    clearDataFormulario();
    switch (valor) {
        case PLANILLA_BONOS_EXTRAS:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'block';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            break;
        case DEPOSITOS_BANCARIOS:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            // Seleccionar el tipo de documento BOLETA DE DEPOSITO
            document.getElementById('uiBoletaDeposito').checked = true;
            document.getElementById('uiBoletaDeposito').onClick = setDataAdicionaTipoDocumento("3");
            // colocar como obligatorio los campos para esta opcion
            setCamposObligatoriosDepositosBancarios();
            // Habilitar la forma de pago
            document.getElementById("uiEfectivo").disabled = false;
            document.getElementById("uiDeposito").disabled = true;
            document.getElementById("uiCheque").disabled = false;
            set("uiCodigoCategoriaEntidad", CATEGORIA_BANCO.toString());
            set("uiCodigoEntidad", "-1");
            set("uiCodigoArea", "0");
            set("uiNombreEntidad", "BANCO");
            break;
        case BACK_TO_BACK:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            // Seleccionar el tipo de documento NINGUN DOCUMENTO
            document.getElementById("uiNingunTipoDocumento").checked = true;
            document.getElementById('uiNingunTipoDocumento').onClick = setDataAdicionaTipoDocumento("0");
            // Habilitar la forma de pago
            document.getElementById("uiEfectivo").disabled = false;
            document.getElementById("uiDeposito").disabled = false;
            document.getElementById("uiCheque").disabled = true;

            document.getElementById("uiEfectivo").checked = true; // Checked por Default
            document.getElementById("uiDeposito").checked = false;
            document.getElementById("uiCheque").checked = false;
            set("uiObservaciones", "Back to Back");
            break;
        case GASTOS_INDIRECTOS:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-tipo-gasto-indirecto').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            break;
        case GASTOS_ADMINISTRATIVOS:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            break;
        case PLANILLA_PAGO:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'block';
            document.getElementById('div-tipo-planilla-pago').style.display = 'block';
            document.getElementById('uiContainerTipoPago').style.display = 'block';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            set("uiCodigoCategoriaEntidad", CATEGORIA_EMPLEADO.toString());
            set("uiCodigoEntidad", ENTIDAD_EMPLEADOS.toString());
            set("uiCodigoArea", "0");
            set("uiNombreEntidad", "EMPLEADOS");
            fillAnioPlanilla();
            fillShowDataAdicionalPlanilla("-1");
            break;
        case PLANILLA_BONO_14:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'block';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            set("uiCodigoCategoriaEntidad", CATEGORIA_EMPLEADO.toString());
            set("uiCodigoEntidad", ENTIDAD_EMPLEADOS.toString());
            set("uiCodigoArea", "0");
            set("uiNombreEntidad", "EMPLEADOS");
            fillAnioPlanilla();
            fillShowDataAdicionalPlanilla("-1");
            break;
        case PLANILLA_AGUINALDO:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'block';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            set("uiCodigoCategoriaEntidad", CATEGORIA_EMPLEADO.toString());
            set("uiCodigoEntidad", ENTIDAD_EMPLEADOS.toString());
            set("uiCodigoArea", "0");
            set("uiNombreEntidad", "EMPLEADOS");
            fillAnioPlanilla();
            fillShowDataAdicionalPlanilla("-1");
            break;
        case VENTAS_ESTABLECIMIENTO:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            set("uiCodigoCategoriaEntidad", CATEGORIA_CAJA.toString());
            set("uiCodigoEntidad", ENTIDAD_CAJA.toString());
            set("uiCodigoArea", "0");
            set("uiNombreEntidad", "VENTAS ESTABLECIMIENTO");
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            break;
        case VENTAS_EN_RUTA:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            document.getElementById('div-ventas-en-ruta').style.display = 'block';
            let elementRutaVendedor = document.getElementById("uiRutaVendedor");
            elementRutaVendedor.classList.add('obligatorio');
            break;
        case OPERACION_ESPECIALES_1:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'block';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            break;
        case PRESTAMO:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            fillComboTipoCuentaPorCobrar();
            break;
        case ABONO_A_PRESTAMO:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            // Tipo de Documento
            document.getElementById("uiNingunTipoDocumento").checked = true;
            document.getElementById('uiNingunTipoDocumento').onClick = setDataAdicionaTipoDocumento("0");
            // Forma de Pago
            document.getElementById("uiEfectivo").disabled = false;
            document.getElementById("uiDeposito").disabled = true;
            document.getElementById("uiCheque").disabled = true;
            document.getElementById("uiEfectivo").checked = true;
            document.getElementById("uiDeposito").checked = false;
            document.getElementById("uiCheque").checked = false;
            break;
        case DEVOLUCION_ANTICIPIO_LIQUIDABLE:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            // Tipo de Documento
            document.getElementById("uiNingunTipoDocumento").checked = true;
            document.getElementById('uiNingunTipoDocumento').onClick = setDataAdicionaTipoDocumento("0");
            // Forma de Pago
            document.getElementById("uiEfectivo").disabled = false;
            document.getElementById("uiDeposito").disabled = true;
            document.getElementById("uiCheque").disabled = true;
            document.getElementById("uiEfectivo").checked = true;
            document.getElementById("uiDeposito").checked = false;
            document.getElementById("uiCheque").checked = false;
            break;
        case TRASLADO_DEPARTAMENTO_CREDITOS:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            set("uiCodigoCategoriaEntidad", CATEGORIA_GENERICA.toString());
            set("uiCodigoEntidad", ENTIDAD_DEPARTAMENTO_DE_CREDITOS.toString());
            set("uiCodigoArea", "0");
            set("uiNombreEntidad", "DEPARTAMENTO DE CRÉDITOS");
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            break;
        case JUSTIFICACION_DEVOLUCIONES_SOCIOS:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            set("uiCodigoCategoriaEntidad", CATEGORIA_EMPRESA.toString());
            set("uiCodigoEntidad", ENTIDAD_EMPRESA_JOSE_MARIA.toString());
            set("uiCodigoArea", "0");
            set("uiNombreEntidad", "GASTOS VARIOS AMERICANA");
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            break;
        case EGRESO_CAJA_CHICA_JOSE_MARIA:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            set("uiCodigoCategoriaEntidad", CATEGORIA_GENERICA.toString());
            set("uiCodigoEntidad", FACTURAS_DE_CONTADO_FISCALES_PAGADAS_CON_CAJA_CHICA.toString());
            set("uiCodigoArea", "0");
            set("uiNombreEntidad", "CAJA CHICA JOSE MARIA");
            set("uiObservaciones", "FACTURAS DE CONTADO(FISCALES PAGADAS CON CAJA CHICA)");
            document.getElementById("uiNingunTipoDocumento").checked = true;
            document.getElementById('uiNingunTipoDocumento').onClick = setDataAdicionaTipoDocumento("0");
            document.getElementById("uiEfectivo").checked = true;
            break;
        default:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;

            break;
    }
}

function setDataControls(codigoOperacion, data) {
    let arrayDate = getFechaSistema();
    let mesActual = arrayDate[0]["mes"];
    let anioActual = arrayDate[0]["anio"];
    let anioAnterior = anioActual - 1;
    let selectAnioPlanilla = document.getElementById("uiAnioPlanilla");
    let table = $('#tabla').DataTable();
    table.$("input[type=radio]").prop("checked", false);
    //clearDataFormulario();
    switch (codigoOperacion) {
        case PLANILLA_BONOS_EXTRAS:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'block';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            // Set data
            setCheckedValueOfRadioButtonGroup('CodigoBonoExtra', data.codigoBonoExtra.toString());
            let elementRuta = document.getElementById("uiNumeroRuta");
            let elementSemanaComision = document.getElementById("uiSemanaComision");
            let elementAnioComision = document.getElementById("uiAnioComision");
            switch (data.codigoBonoExtra) {
                case BONO_COMISIONES:
                    document.getElementById('div-bonos-extra').style.display = 'block';
                    elementRuta.classList.add('obligatorio');
                    elementSemanaComision.classList.add('obligatorio');
                    elementAnioComision.classList.add('obligatorio');
                    elementRuta.value = data.ruta;

                    // Fill Anio Comision
                    let contenido = "";
                    let anioOperacion = parseInt(document.getElementById("uiAnioOperacion").value);
                    let numeroSemanaActual = document.getElementById("uiNumeroSemanaActual").value;
                    if (mesActual == 1 && anioActual == anioOperacion) {
                        contenido += "<option selected = 'selected' value = '" + anioOperacion.toString() + "'>" + anioOperacion.toString() + "</option>"
                        contenido += "<option value = '" + anioAnterior.toString() + "'>" + anioAnterior.toString() + "</option>"
                        document.getElementById("uiAnioComision").selectedIndex = "0";
                        document.getElementById("uiAnioComision").innerHTML = contenido;
                        document.getElementById("uiAnioComision").value = data.anioComision;

                    } else {
                        contenido += "<option selected = 'selected' value = '" + anioOperacion.toString() + "'>" + anioOperacion.toString() + "</option>"
                        document.getElementById("uiAnioComision").selectedIndex = "0";
                        document.getElementById("uiAnioComision").innerHTML = contenido;
                        document.getElementById("uiAnioComision").value = data.anioComision;
                    }

                    fetchGet("ProgramacionSemanal/GetListaSemanasComision/?anio=" + anioOperacion.toString() + "&numeroSemana=" + numeroSemanaActual + "&ultimaSemanaAnioAnterior=0", "json", function (rpta) {
                        if (rpta.length > 0) {
                            FillCombo(rpta, "uiSemanaComision", "semanaComision", "periodo", "- seleccione semana -", "-1");
                            elementSemanaComision.value = data.semanaComision;
                        } else {
                            FillComboUnicaOpcion("uiSemanaComision", "-1", "-- No existen semanas -- ");
                        }
                    });

                    break;
                case BONO_QUINTALAJE:
                    document.getElementById('div-bonos-extra').style.display = 'none';
                    elementRuta.classList.remove('obligatorio');
                    elementSemanaComision.classList.remove('obligatorio');
                    elementAnioComision.classList.remove('obligatorio');
                    set("uiNumeroRuta", "");
                    FillComboUnicaOpcion("uiSemanaComision", "-1", "-- No Aplica -- ");
                    FillComboUnicaOpcion("uiAnioComision", "-1", "-- No Aplica -- ");
                    break;
                case BONO_FERIADOS_O_DOMINGOS:
                    document.getElementById('div-bonos-extra').style.display = 'none';
                    set("uiNumeroRuta", "");
                    FillComboUnicaOpcion("uiSemanaComision", "-1", "-- No Aplica -- ");
                    FillComboUnicaOpcion("uiAnioComision", "-1", "-- No Aplica -- ");
                    elementRuta.classList.remove('obligatorio');
                    elementSemanaComision.classList.remove('obligatorio');
                    elementAnioComision.classList.remove('obligatorio');
                    break;
                case BONO_OTROS:
                    document.getElementById('div-bonos-extra').style.display = 'none';
                    set("uiNumeroRuta", "");
                    FillComboUnicaOpcion("uiSemanaComision", "-1", "-- No Aplica -- ");
                    FillComboUnicaOpcion("uiAnioComision", "-1", "-- No Aplica -- ");
                    elementRuta.classList.remove('obligatorio');
                    elementSemanaComision.classList.remove('obligatorio');
                    elementAnioComision.classList.remove('obligatorio');
                    break;
            }
            break;
        case DEPOSITOS_BANCARIOS:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';

            // Seleccionar el tipo de documento BOLETA DE DEPOSITO
            document.getElementById('uiBoletaDeposito').checked = true;
            document.getElementById('uiBoletaDeposito').onClick = setDataAdicionaTipoDocumento("3");
            // colocar como obligatorio los campos para esta opcion
            setCamposObligatoriosDepositosBancarios();
            document.getElementById('uiBanco').value = data.codigoBancoDeposito.toString();
            // Fill Cuentas Bancarias
            fetchGet("CuentaBancaria/GetCuentasBancariasTesoreria/?codigoBanco=" + data.codigoBancoDeposito.toString(), "json", function (rpta) {
                if (rpta.length > 0) {
                    FillCombo(rpta, "uiNumeroCuenta", "numeroCuenta", "numeroCuentaDescriptivo", "- seleccione cuenta -", "-1");
                    document.getElementById('uiNumeroCuenta').value = data.numeroCuenta;
                } else {
                    FillComboUnicaOpcion("uiNumeroCuenta", "-1", "-- No existe cuenta -- ");
                }
            })
            document.getElementById('uiMontoEfectivo').value = data.montoEfectivo.toString();
            document.getElementById('uiMontoCheques').value = data.montoCheques.toString();
            // SetData
            setCheckedValueOfRadioButtonGroup('CodigoTipoDocumentoDeposito', data.codigoTipoDocumentoDeposito.toString());
            switch (data.codigoTipoDocumentoDeposito) {
                case NUMERO_BOLETA:
                    document.getElementById('uiTitleTipoDocumentoDeposito').innerHTML = "Número de boleta";
                    document.getElementById('uiNumeroBoleta').value = data.numeroBoleta;
                    break;
                case NUMERO_VAUCHER:
                    document.getElementById('uiTitleTipoDocumentoDeposito').innerHTML = "Número de vaucher";
                    document.getElementById('uiNumeroBoleta').value = data.numeroVoucher;
                    break;
                default:
                    document.getElementById('uiTitleTipoDocumentoDeposito').innerHTML = "";
                    document.getElementById('uiNumeroBoleta').value = "";
                    break;
            }
            break;
        case BACK_TO_BACK:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            break;
        case GASTOS_INDIRECTOS:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-tipo-gasto-indirecto').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            break;
        case GASTOS_ADMINISTRATIVOS:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            break;
        case PLANILLA_PAGO:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'block';
            document.getElementById('div-tipo-planilla-pago').style.display = 'block';
            document.getElementById('uiContainerTipoPago').style.display = 'block';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            // Fill Año Planilla
            if (mesActual == 1) {
                let arrayAnioPlanilla = [{ "value": anioAnterior.toString(), "text": anioAnterior.toString() }, { "value": anioActual.toString(), "text": anioActual.toString() }]
                FillCombo(arrayAnioPlanilla, "uiAnioPlanilla", "value", "text", "- seleccione -", "-1");
                selectAnioPlanilla.value = data.anioPlanilla.toString();

            } else {
                let arrayAnioPlanilla = [{ "value": anioActual.toString(), "text": anioActual.toString() }]
                FillCombo(arrayAnioPlanilla, "uiAnioPlanilla", "value", "text", "- seleccione -", "-1");
                selectAnioPlanilla.value = data.anioPlanilla.toString();
            }
            // Set Frecuencia Pago
            document.getElementById("uiCodigoFrecuenciaPago").value = data.codigoFrecuenciaPago.toString();
            setDataAdicionalPlanilla(data.codigoFrecuenciaPago, data);

            break;
        case PLANILLA_BONO_14:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'block';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            // Fill Año Planilla
            if (mesActual == 1) {
                let arrayAnioPlanilla = [{ "value": anioAnterior.toString(), "text": anioAnterior.toString() }, { "value": anioActual.toString(), "text": anioActual.toString() }]
                FillCombo(arrayAnioPlanilla, "uiAnioPlanilla", "value", "text", "- seleccione -", "-1");
                selectAnioPlanilla.value = data.anioPlanilla.toString();

            } else {
                let arrayAnioPlanilla = [{ "value": anioActual.toString(), "text": anioActual.toString() }]
                FillCombo(arrayAnioPlanilla, "uiAnioPlanilla", "value", "text", "- seleccione -", "-1");
                selectAnioPlanilla.value = data.anioPlanilla.toString();
            }
            // Set Frecuencia Pago
            document.getElementById("uiCodigoFrecuenciaPago").value = data.codigoFrecuenciaPago.toString();
            setDataAdicionalPlanilla(data.codigoFrecuenciaPago, data);
            break;
        case PLANILLA_AGUINALDO:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'block';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            // Fill Año Planilla
            if (mesActual == 1) {
                let arrayAnioPlanilla = [{ "value": anioAnterior.toString(), "text": anioAnterior.toString() }, { "value": anioActual.toString(), "text": anioActual.toString() }]
                FillCombo(arrayAnioPlanilla, "uiAnioPlanilla", "value", "text", "- seleccione -", "-1");
                selectAnioPlanilla.value = data.anioPlanilla.toString();
            } else {
                let arrayAnioPlanilla = [{ "value": anioActual.toString(), "text": anioActual.toString() }]
                FillCombo(arrayAnioPlanilla, "uiAnioPlanilla", "value", "text", "- seleccione -", "-1");
                selectAnioPlanilla.value = data.anioPlanilla.toString();
            }
            // Set Frecuencia Pago
            document.getElementById("uiCodigoFrecuenciaPago").value = data.codigoFrecuenciaPago.toString();
            setDataAdicionalPlanilla(data.codigoFrecuenciaPago, data);
            break;
        case VENTAS_ESTABLECIMIENTO:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            break;
        case VENTAS_EN_RUTA:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-ventas-en-ruta').style.display = 'block';
            let elementRutaVendedor = document.getElementById("uiRutaVendedor");
            elementRutaVendedor.classList.add('obligatorio');
            let codigoCategoriaEntidad = data.codigoCategoriaEntidad.toString();
            let codigoVendedor = data.codigoEntidad;
            fetchGet("Vendedores/GetRutasDelVendedor/?codigoCategoriaEntidad=" + codigoCategoriaEntidad + "&codigoVendedor=" + codigoVendedor, "json", function (rpta) {
                if (rpta.length > 0) {
                    FillCombo(rpta, "uiRutaVendedor", "ruta", "ruta", "- seleccione ruta -", "-1");
                    document.getElementById('uiRutaVendedor').value = data.ruta;
                } else {
                    FillComboUnicaOpcion("uiRutaVendedor", "-1", "-- No existem rutas -- ");
                }
            });

            break;
        case OPERACION_ESPECIALES_1:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'block';
            document.getElementById('div-bonos-extra').style.display = 'none';
            setCheckedValueOfRadioButtonGroup('TipoEspeciales1', data.tipoEspeciales1);
            let elementOtrosIngresos = document.getElementById("uiCodigoOtroIngreso");
            if (data.tipoEspeciales1 == TIPO_ESPECIAL1_OTROS_INGRESOS) {
                fetchGet("Configuracion/GetListaOtrosIngresos", "json", function (rpta) {
                    FillCombo(rpta, "uiCodigoOtroIngreso", "codigoOtroIngreso", "nombre", "- seleccione -", "-1");
                    elementOtrosIngresos.classList.add('obligatorio');
                    document.getElementById('div-otros-ingresos').style.display = 'block';
                    document.querySelector('#uiCodigoOtroIngreso').value = data.codigoOtroIngreso.toString();
                })
            } else {
                document.getElementById('div-otros-ingresos').style.display = 'none';
                elementOtrosIngresos.classList.remove('obligatorio');
            }

            break;
        case PRESTAMO:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-cuenta-por-cobrar').style.display = 'block';

            let elementTipoCuentaPorCobrar = document.getElementById("uiTipoCuentaPorCobrar");
            let elementFechaPrestamo = document.getElementById("uiFechaPrestamo");
            let elementFechaInicioPago = document.getElementById("uiFechaInicioPago");
            elementTipoCuentaPorCobrar.classList.add('obligatorio');
            elementFechaPrestamo.classList.add('obligatorio');
            elementFechaInicioPago.classList.add('obligatorio');
            fetchGet("CuentasPorCobrar/GetListTiposCuentasPorCobrar", "json", function (rpta) {
                FillCombo(rpta, "uiTipoCuentaPorCobrar", "codigoTipoCuentaPorCobrar", "nombre", "- seleccione -", "-1");
                document.getElementById("uiTipoCuentaPorCobrar").value = data.codigoTipoCuentaPorCobrar.toString();
            });
            break;
        case ABONO_A_PRESTAMO:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            // Forma de Pago
            document.getElementById("uiEfectivo").disabled = false;
            document.getElementById("uiDeposito").disabled = true;
            document.getElementById("uiCheque").disabled = true;
            break;
        case DEVOLUCION_ANTICIPIO_LIQUIDABLE:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            // Forma de Pago
            document.getElementById("uiEfectivo").disabled = false;
            document.getElementById("uiDeposito").disabled = true;
            document.getElementById("uiCheque").disabled = true;
            break;
        case TRASLADO_DEPARTAMENTO_CREDITOS:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            break;
        case JUSTIFICACION_DEVOLUCIONES_SOCIOS:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            break;
        case EGRESO_CAJA_CHICA_JOSE_MARIA:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            break;
        default:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            break;
    }
}


function fillMesesPlanilla(mes) {
    let codigoOperacion = parseInt(document.getElementById("cboOperacion").value);
    let select = document.getElementById("uiMesPlanilla")
    let data;
    if (codigoOperacion != PLANILLA_AGUINALDO && codigoOperacion != PLANILLA_BONO_14) {
        switch (mes) {
            case 1: // ENERO
                data = [{ "text": "ENERO", "value": "1" }, { "text": "DICIEMBRE", "value": "12" }]
                FillCombo(data, "uiMesPlanilla", "value", "text", "- seleccione -", "-1");
                select.selectedIndex = 1;
                break;
            case 2: // FEBRERO
                data = [{ "text": "FEBRERO", "value": "2" }, { "text": "ENERO", "value": "1" }]
                FillCombo(data, "uiMesPlanilla", "value", "text", "- seleccione -", "-1");
                select.selectedIndex = 1;
                break;
            case 3: // MARZO
                data = [{ "text": "FEBRERO", "value": "2" }, { "text": "ENERO", "value": "1" }]
                FillCombo(data, "uiMesPlanilla", "value", "text", "- seleccione -", "-1");
                select.selectedIndex = 1;
                break;
            case 4: // ABRIL
                data = [{ "text": "ABRIL", "value": "4" }, { "text": "MARZO", "value": "3" }]
                FillCombo(data, "uiMesPlanilla", "value", "text", "- seleccione -", "-1");
                select.selectedIndex = 1;
                break;
            case 5: // MAYO
                data = [{ "text": "MAYO", "value": "5" }, { "text": "ABRIL", "value": "4" }]
                FillCombo(data, "uiMesPlanilla", "value", "text", "- seleccione -", "-1");
                select.selectedIndex = 1;
                break;
            case 6: // JUNIO
                data = [{ "text": "JUNIO", "value": "6" }, { "text": "MAYO", "value": "5" }]
                FillCombo(data, "uiMesPlanilla", "value", "text", "- seleccione -", "-1");
                select.selectedIndex = 1;
                break;
            case 7: // JULIO
                data = [{ "text": "JULIO", "value": "7" }, { "text": "JUNIO", "value": "6" }]
                FillCombo(data, "uiMesPlanilla", "value", "text", "- seleccione -", "-1");
                select.selectedIndex = 1;
                break;
            case 8: // AGOSTO
                data = [{ "text": "AGOSTO", "value": "8" }, { "text": "JULIO", "value": "7" }]
                FillCombo(data, "uiMesPlanilla", "value", "text", "- seleccione -", "-1");
                select.selectedIndex = 1;
                break;
            case 9: // SEPTIEMBRE
                data = [{ "text": "SEPTIEMBRE", "value": "9" }, { "text": "AGOSTO", "value": "8" }]
                FillCombo(data, "uiMesPlanilla", "value", "text", "- seleccione -", "-1");
                select.selectedIndex = 1;
                break;
            case 10: // NOVIEMBRE
                data = [{ "text": "OCTUBRE", "value": "10" }, { "text": "SEPTIEMBRE", "value": "9" }]
                FillCombo(data, "uiMesPlanilla", "value", "text", "- seleccione -", "-1");
                select.selectedIndex = 1;
                break;
            case 11:
                data = [{ "text": "NOVIEMBRE", "value": "11" }, { "text": "OCTUBRE", "value": "10" }]
                FillCombo(data, "uiMesPlanilla", "value", "text", "- seleccione -", "-1");
                select.selectedIndex = 1;
                break;
            case 12:
                data = [{ "text": "DICIEMBRE", "value": "12" }, { "text": "NOVIEMBRE", "value": "11" }]
                FillCombo(data, "uiMesPlanilla", "value", "text", "- seleccione -", "-1");
                select.selectedIndex = 1;
                break;
        }
    } else {
        if (codigoOperacion == PLANILLA_AGUINALDO) {
            FillComboUnicaOpcion("uiMesPlanilla", "12", "DICIEMBRE");
            select.selectedIndex = 0;
        } else {
            if (codigoOperacion == PLANILLA_BONO_14) {
                FillComboUnicaOpcion("uiMesPlanilla", "6", "JUNIO");
                select.selectedIndex = 0;
            }
        }
    }
}


function fillMesesSueldosIndirectosDonPepe(obj) {
    let anio = parseInt(obj);
    let arrayDate = getFechaSistema();
    let anioActual = arrayDate[0]["anio"];
    let mesActual = arrayDate[0]["mes"];
    let select = document.getElementById("uiMesSueldoIndirecto")
    let data;
    if (anio != -1) {
        if (anio == anioActual) {
            switch (mesActual) {
                case 1: // ENERO
                    data = [{ "text": "- seleccione - ", "value": "-1" }, { "text": "ENERO", "value": "1" }, { "text": "DICIEMBRE", "value": "12" }]
                    FillCombo(data, "uiMesSueldoIndirecto", "value", "text", "- seleccione -", "-1");
                    select.selectedIndex = 1;
                    break;
                case 2: // FEBRERO
                    data = [{ "text": "- seleccione - ", "value": "-1" }, { "text": "FEBRERO", "value": "2" }, { "text": "ENERO", "value": "1" }]
                    FillCombo(data, "uiMesSueldoIndirecto", "value", "text", "- seleccione -", "-1");
                    select.selectedIndex = 1;
                    break;
                case 3: // MARZO
                    data = [{ "text": "- seleccione - ", "value": "-1" }, { "text": "FEBRERO", "value": "2" }, { "text": "ENERO", "value": "1" }]
                    FillCombo(data, "uiMesSueldoIndirecto", "value", "text", "- seleccione -", "-1");
                    select.selectedIndex = 1;
                    break;
                case 4: // ABRIL
                    data = [{ "text": "- seleccione - ", "value": "-1" }, { "text": "ABRIL", "value": "4" }, { "text": "MARZO", "value": "3" }]
                    FillCombo(data, "uiMesSueldoIndirecto", "value", "text", "- seleccione -", "-1");
                    select.selectedIndex = 1;
                    break;
                case 5: // MAYO
                    data = [{ "text": "- seleccione - ", "value": "-1" }, { "text": "MAYO", "value": "5" }, { "text": "ABRIL", "value": "4" }]
                    FillCombo(data, "uiMesSueldoIndirecto", "value", "text", "- seleccione -", "-1");
                    select.selectedIndex = 1;
                    break;
                case 6: // JUNIO
                    data = [{ "text": "- seleccione - ", "value": "-1" }, { "text": "JUNIO", "value": "6" }, { "text": "MAYO", "value": "5" }]
                    FillCombo(data, "uiMesSueldoIndirecto", "value", "text", "- seleccione -", "-1");
                    select.selectedIndex = 1;
                    break;
                case 7: // JULIO
                    data = [{ "text": "- seleccione - ", "value": "-1" }, { "text": "JULIO", "value": "7" }, { "text": "JUNIO", "value": "6" }]
                    FillCombo(data, "uiMesSueldoIndirecto", "value", "text", "- seleccione -", "-1");
                    select.selectedIndex = 1;
                    break;
                case 8: // AGOSTO
                    data = [{ "text": "- seleccione - ", "value": "-1" }, { "text": "AGOSTO", "value": "8" }, { "text": "JULIO", "value": "7" }]
                    FillCombo(data, "uiMesSueldoIndirecto", "value", "text", "- seleccione -", "-1");
                    select.selectedIndex = 1;
                    break;
                case 9: // SEPTIEMBRE
                    data = [{ "text": "- seleccione - ", "value": "-1" }, { "text": "SEPTIEMBRE", "value": "9" }, { "text": "AGOSTO", "value": "8" }]
                    FillCombo(data, "uiMesSueldoIndirecto", "value", "text", "- seleccione -", "-1");
                    select.selectedIndex = 1;
                    break;
                case 10: // NOVIEMBRE
                    data = [{ "text": "- seleccione - ", "value": "-1" }, { "text": "OCTUBRE", "value": "10" }, { "text": "SEPTIEMBRE", "value": "9" }]
                    FillCombo(data, "uiMesSueldoIndirecto", "value", "text", "- seleccione -", "-1");
                    select.selectedIndex = 1;
                    break;
                case 11:
                    data = [{ "text": "- seleccione - ", "value": "-1" }, { "text": "NOVIEMBRE", "value": "11" }, { "text": "OCTUBRE", "value": "10" }]
                    FillCombo(data, "uiMesSueldoIndirecto", "value", "text", "- seleccione -", "-1");
                    select.selectedIndex = 1;
                    break;
                case 12:
                    data = [{ "text": "- seleccione - ", "value": "-1" }, { "text": "DICIEMBRE", "value": "12" }, { "text": "NOVIEMBRE", "value": "11" }]
                    FillCombo(data, "uiMesSueldoIndirecto", "value", "text", "- seleccione -", "-1");
                    select.selectedIndex = 1;
                    break;

            }// fin switch
        } else {
            data = [{ "text": "- seleccione - ", "value": "-1" }, { "text": "DICIEMBRE", "value": "12" }, { "text": "NOVIEMBRE", "value": "11" }]
            FillCombo(data, "uiMesSueldoIndirecto", "value", "text", "- seleccione -", "-1");
            select.selectedIndex = 1;
        }
    } else {
        FillComboUnicaOpcion("uiMesSueldoIndirecto", "-1", " -- Sin Meses -- ");
    }
}

function fillComboSemanasPlanilla() {
    let anio = document.getElementById("uiAnioPlanilla").value;
    let numeroSemanaActual = parseInt(document.getElementById("uiNumeroSemanaActualSistema").value);
    fetchGet("ProgramacionSemanal/GetListaSemanasPlanilla/?anio=" + anio.toString() + "&numeroSemana=" + numeroSemanaActual.toString(), "json", function (rpta) {
        if (rpta != undefined && rpta != null) {
            FillCombo(rpta, "uiSemanaPlanilla", "numeroSemana", "periodo", "- seleccione -", "-1");
        }
    })
}


function fillComboPlanilla(obj) {
    let arrayDate = getFechaSistema();
    //let mesActual = arrayDate[0]["mes"];
    let anioActual = arrayDate[0]["anio"];
    let numeroMesSelected = parseInt(document.getElementById("uiMesPlanilla").value);
    let numeroMes = 0;
    let anio = parseInt(obj.value);
    let codigoOperacion = parseInt(document.getElementById("uiCodigoFrecuenciaPago").value);
    let numeroSemana = 1;
    switch (codigoOperacion) {
        case TIPO_PLANILLA_MENSUAL:
            fillMesesPlanilla(numeroMes);
            break;
        case TIPO_PLANILLA_QUINCENAL:
            if (anioActual == anio) {
                numeroMes = numeroMesSelected;
            } else {
                numeroMes = 12;
            }
            fetchGet("ProgramacionQuincenal/GetListaQuincenas/?anio=" + anio.toString() + "&numeroMes=" + numeroMes.toString(), "json", function (rpta) {
                if (rpta != undefined && rpta != null && rpta.length != 0) {
                    FillCombo(rpta, "uiCodigoQuincenaPlanilla", "codigoQuincenaPlanilla", "periodo", "- seleccione -", "-1");
                } else {
                    FillComboUnicaOpcion("uiCodigoQuincenaPlanilla", "-1", "-- No Existen Fechas -- ");
                }
            })
            break;
        case TIPO_PLANILLA_SEMANAL:
            fetchGet("ProgramacionSemanal/GetListaSemanasPlanilla/?anio=" + anio.toString() + "&numeroSemana=" + numeroSemana.toString(), "json", function (rpta) {
                if (rpta != undefined && rpta != null && rpta.length != 0) {
                    FillCombo(rpta, "uiSemanaPlanilla", "numeroSemana", "periodo", "- seleccione -", "-1");
                } else {
                    FillComboUnicaOpcion("uiSemanaPlanilla", "0", "-- No Existen Fechas -- ");
                }
            })
            break;
    }
}

function fillComboQuincenas() {
    let numeroMes = parseInt(document.getElementById("uiMesPlanilla").value);
    let anio = parseInt(document.getElementById("uiAnioPlanilla").value);
    if (anio != undefined) {
        fetchGet("ProgramacionQuincenal/GetListaQuincenas/?anio=" + anio.toString() + "&numeroMes=" + numeroMes.toString(), "json", function (rpta) {
            if (rpta != undefined && rpta != null && rpta.length != 0) {
                FillCombo(rpta, "uiCodigoQuincenaPlanilla", "codigoQuincenaPlanilla", "periodo", "- seleccione -", "-1");
            } else {
                FillComboUnicaOpcion("uiCodigoQuincenaPlanilla", "-1", "-- No Existen Fechas -- ");
            }
        });
    } else {
        alert("anio no definido")
    }
}


function LimpiarTransacciones()
{
}


function GuardarDatos() {
    let errores = ValidarDatos("frmGuardaTransaccion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let codigoOperacion = parseInt(document.getElementById("cboOperacion").value);
    // Si la operación es Bonos Extras, validar que se haya seleccionado el tipo de bono extra
    if (codigoOperacion == PLANILLA_BONOS_EXTRAS) {
        if (!document.querySelector('input[name="CodigoBonoExtra"]:checked')) {
            MensajeError("Debe seleccionar el tipo de bonos extras");
            return;
        }
    } else {
        if (codigoOperacion == OPERACION_ESPECIALES_1) {// validar que se haya seleccionado el tipo de especiales 1
            if (!document.querySelector('input[name="TipoEspeciales1"]:checked')) {
                MensajeError("Debe seleccionar el tipo de especiales 1");
                return;
            }
        } 
    }
    if (!document.querySelector('input[name="CodigoTipoDocumento"]:checked')) {
        MensajeError("Debe seleccionar un tipo de documento");
        return;
    }
    let checkEfectivo = document.getElementById("uiEfectivo");
    let checkDeposito = document.getElementById("uiDeposito");
    let checkCheque = document.getElementById("uiCheque");
    if (checkEfectivo.checked == false && checkDeposito.checked == false && checkCheque.checked == false) {
        MensajeError("Debe seleccionar una forma de pago");
        return;
    }

    let frmGuardar = document.getElementById("frmGuardaTransaccion");
    let frm = new FormData(frmGuardar);
    if (checkEfectivo.checked == true) {
        frm.set("Efectivo", "1");
    } else {
        frm.set("Efectivo", "0");
    }
    if (checkDeposito.checked == true) {
        frm.set("Deposito", "1");
    } else {
        frm.set("Deposito", "0");
    }
    if (checkCheque.checked == true) {
        frm.set("Cheque", "1");
    } else {
        frm.set("Cheque", "0");
    }

    let codigoTipoOperacion = parseInt(frm.get("CodigoTipoOperacion"));
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Transaccion/GuardarDatos/?complemento=0", "text", frm, function (data) {
            if (!/^[0-9]+$/.test(data)) {
                MensajeError(data);
            } else {
                let setSemanaAnterior = parseInt(document.getElementById("uiSetSemanaAnterior").value);
                if (setSemanaAnterior == 0) {
                    PrintConstanciaIngresos(data, codigoTipoOperacion);
                    setTimeout(() => {
                        Exito("Transaccion", "Index", true);
                    }, 1000);
                } else {
                    Exito("Transaccion", "Index", true);
                }
            }
        })
    })
}

/*function ImprimirConstanciaIngresos(codigoOperacion, numeroRecibo, fechaRecibo, nombreEntidad, nombreOperacion, monto, recursos, usuarioCreacion, ruta, fechaImpresion) {
    let html = getHtmlConstanciaIngreso(codigoOperacion, numeroRecibo, fechaRecibo, nombreEntidad, nombreOperacion, monto, recursos, usuarioCreacion, ruta, fechaImpresion);
    document.getElementById("html-content-holder").innerHTML = html;

    html2canvas(document.getElementById("html-content-holder")).then(function (canvas) {
        var anchorTag = document.createElement("a");
        document.body.appendChild(anchorTag);
        anchorTag.download = "filename.jpg";
        anchorTag.href = canvas.toDataURL();
        anchorTag.target = '_blank';
        anchorTag.click();
    });
}*/


/*function ImprimirConstanciaEgresos(numeroRecibo, fechaRecibo, nombreEntidad, nombreOperacion, monto, usuarioCreacion, fechaImpresion) {
    let html = getHtmlConstanciaEgreso(numeroRecibo, fechaRecibo, nombreEntidad, nombreOperacion, monto, usuarioCreacion, fechaImpresion);

    document.getElementById("html-content-holder").innerHTML = html;
    html2canvas(document.getElementById("html-content-holder")).then(function (canvas) {
        var anchorTag = document.createElement("a");
        document.body.appendChild(anchorTag);
        anchorTag.download = "filename.jpg";
        anchorTag.href = canvas.toDataURL();
        anchorTag.target = '_blank';
        anchorTag.click();
    });
}**/


function intelligenceSearch() {
    // Incluir el radioButton al inicio, por eso se comienza por la columna 1
    let objGlobalConfigTransaccion = {
        url: "Transaccion/ListarEntidadesGenericas",
        cabeceras: ["codigo", "nombre entidad", "codigo categoria", "categoria", "codigo operacion", "codigoArea","codigoOperacionEntidad","codigoCanalVenta"],
        propiedades: ["codigoEntidad", "nombreEntidad", "codigoCategoriaEntidad", "nombreCategoria","codigoOperacionCaja","codigoArea","codigoOperacionEntidad","codigoCanalVenta"],
        divContenedorTabla: "divContenedorTabla",
        ocultarColumnas: true,
        hideColumns: [
            {   "targets": [3], 
                "visible": false
            }, {
                "targets": [5],
                "visible": false
            }, {
                "targets": [6],
                "visible": false
            }, {
                "targets": [7],
                "visible": false
            }, {
                "targets": [8],
                "visible": false
            }],
        divPintado: "divTabla",
        radio: true,
        paginar: true,
        eventoradio: "Entidades",
        slug: "codigoEntidad"

    }
    pintar(objGlobalConfigTransaccion);
}


function getDataRowRadioEntidades(obj) {
    // Incluir el radioButton al inicio, por eso se comienza por la columna 1
    document.getElementById('div-captura-proveedor').style.display = 'none';
    let elementNombreProveedor = document.getElementById('uiNombreProveedor');
    elementNombreProveedor.value = "";
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('change', 'tr', 'input:radio', function () {
        let codigoOperacion = parseInt(document.getElementById("cboOperacion").value);
        let esBonoExtrasPorComisiones = document.getElementById("uiOptionPorComisiones").checked;
        let esBonoQuintalaje = document.getElementById("uiOptionBonoQuintalaje").checked;
        let esBonoFeriadosODomingos = document.getElementById("uiOptionBonosFeriadosODomingos").checked;
        let esBonoExtrasOtros = document.getElementById("uiOptionBonosOtros").checked;

        let rowIdx = table.row(this).index();
        let codigoEntidad = table.cell(rowIdx, 1).data();
        let codigoCategoriaEntidad = parseInt(table.cell(rowIdx, 3).data());
        let codigoOperacionEntidad = parseInt(table.cell(rowIdx, 7).data());

        if (codigoOperacionEntidad != 0) {
            if (codigoOperacionEntidad != codigoOperacion) {
                table.$("input[type=radio]").prop("checked", false);
                MensajeError("Operación incorrecta para la entidad seleccionada");
            } else {
                set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                set("uiCodigoArea", table.cell(rowIdx, 6).data());
                set("uiCodigoCanalVenta", table.cell(rowIdx, 8).data());
                setSueldosIndirectosDonPepe(false.false);

                if (codigoCategoriaEntidad == PROVEEDOR_MATERIA_PRIMA ||
                    codigoCategoriaEntidad == PROVEEDOR_MATERIAL_DE_EMPAQUE ||
                    codigoCategoriaEntidad == PROVEEDOR_SERVICIOS_DE_FABRICACION ||
                    codigoCategoriaEntidad == PROVEEDOR_INSUMOS_DE_FABRICACION ||
                    codigoCategoriaEntidad == PROVEEDOR_SERVICIOS_VARIOS ||
                    codigoCategoriaEntidad == PROVEEDOR_INSUMOS_VARIOS) {
                    document.getElementById('div-captura-proveedor').style.display = 'block';
                    elementNombreProveedor.classList.add('obligatorio');
                }
            }// fin if

        } else {

            switch (codigoOperacion) {
                case PLANILLA_BONOS_EXTRAS:
                    if ((esBonoExtrasPorComisiones == true) && (codigoCategoriaEntidad == CATEGORIA_VENDEDOR || codigoCategoriaEntidad == CATEGORIA_RUTERO_LOCAL || codigoCategoriaEntidad == CATEGORIA_RUTERO_INTERIOR || codigoCategoriaEntidad == CATEGORIA_CAFETERIA || codigoCategoriaEntidad == CATEGORIA_SUPERMERCADOS) || codigoCategoriaEntidad == CATEGORIA_EMPLEADO) {
                        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                        set("uiCodigoArea", table.cell(rowIdx, 6).data());
                        set("uiCodigoCanalVenta", table.cell(rowIdx, 8).data());
                    } else {
                        if (esBonoExtrasPorComisiones == false && esBonoExtrasOtros == false && esBonoQuintalaje == false && esBonoFeriadosODomingos == false) {
                            table.$("input[type=radio]").prop("checked", false);
                            MensajeError("Seleccione el tipo de bonos extras");
                            
                        } else {
                            if (esBonoExtrasOtros == true || esBonoQuintalaje == true || esBonoFeriadosODomingos == true) {
                                set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                                set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                                set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                                set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                                set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                                set("uiCodigoArea", table.cell(rowIdx, 6).data());
                            } else {
                                table.$("input[type=radio]").prop("checked", false);
                                //$(this).prop("checked", true);
                                Warning("la entidad seleccionada es incorrecta");
                            }
                        }
                    }
                    break;
                case OPERACION_ESPECIALES_1:
                    let esEspecialesPorCliente = document.getElementById("uiOptionEspecialCliente").checked;
                    let esEspecialesOtrosIngresos = document.getElementById("uiOptioneEspecialOtrosIngresos").checked;
                    if (esEspecialesPorCliente == true || esEspecialesOtrosIngresos == true) {
                        if (esEspecialesPorCliente == true) {
                            if (codigoCategoriaEntidad == CLIENTES_ESPECIALES_1) {
                                set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                                set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                                set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                                set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                                set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                                set("uiCodigoArea", table.cell(rowIdx, 6).data());
                            } else {
                                table.$("input[type=radio]").prop("checked", false);
                                Warning("la entidad seleccionada es incorrecta");
                            }
                        } else {
                            set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                            set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                            set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                            set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                            set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                            set("uiCodigoArea", table.cell(rowIdx, 6).data());
                        }
                    } else {
                        table.$("input[type=radio]").prop("checked", false);
                        Warning("Seleccione el tipo de especiales 1, antes de seleccionar la entidad");
                    }
                    break;
                case VENTAS_EN_RUTA:
                    if (codigoCategoriaEntidad == CATEGORIA_VENDEDOR || codigoCategoriaEntidad == CATEGORIA_RUTERO_LOCAL || codigoCategoriaEntidad == CATEGORIA_RUTERO_INTERIOR || codigoCategoriaEntidad == CATEGORIA_CAFETERIA || codigoCategoriaEntidad == CATEGORIA_SUPERMERCADOS || codigoCategoriaEntidad == CLIENTES_ESPECIALES_2) {
                        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                        set("uiCodigoArea", table.cell(rowIdx, 6).data());
                        set("uiCodigoCanalVenta", table.cell(rowIdx, 8).data());
                        fetchGet("Vendedores/GetRutasDelVendedor/?codigoCategoriaEntidad=" + codigoCategoriaEntidad.toString() + "&codigoVendedor=" + codigoEntidad, "json", function (rpta) {
                            if (rpta != undefined && rpta != null && rpta.length != 0) {
                                FillCombo(rpta, "uiRutaVendedor", "ruta", "ruta", "- seleccione -", "-1");
                            } else {
                                FillComboUnicaOpcion("uiRutaVendedor", "-1", "-- Sin Rutas -- ");
                            }
                        })

                        let elementRutaVendedor = document.getElementById("uiRutaVendedor");
                        if (codigoCategoriaEntidad == CLIENTES_ESPECIALES_2) {
                            document.getElementById('div-ventas-en-ruta').style.display = 'none';
                            elementRutaVendedor.classList.remove('obligatorio');
                        } else {
                            document.getElementById('div-ventas-en-ruta').style.display = 'block';
                            elementRutaVendedor.classList.add('obligatorio');
                        }
                    } else {
                        table.$("input[type=radio]").prop("checked", false);
                        MensajeError("Categoría de la entidad es incorrecta para esta operación");
                    }
                    break;
                case GASTOS_INDIRECTOS:
                    if (codigoCategoriaEntidad == CATEGORIA_SOCIO && codigoEntidad == ENTIDAD_DON_PEPE) {
                        setSueldosIndirectosDonPepe(true, true);
                    } else {
                        let elementCheckSueldoIndirecto = document.getElementById("uiEsSueldosIndirectos");
                        setSueldosIndirectosDonPepe(false, elementCheckSueldoIndirecto.checked);
                    }
                    set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                    set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                    set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                    set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                    set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                    set("uiCodigoArea", table.cell(rowIdx, 6).data());
                    break;
                case ANTICIPO_LIQUIDABLE:
                    if (codigoCategoriaEntidad == CATEGORIA_EMPLEADO || codigoCategoriaEntidad == CATEGORIA_VENDEDOR || codigoCategoriaEntidad == CATEGORIA_RUTERO_LOCAL ||
                        codigoCategoriaEntidad == CATEGORIA_RUTERO_INTERIOR || codigoCategoriaEntidad == CATEGORIA_CAFETERIA || codigoCategoriaEntidad == CATEGORIA_SUPERMERCADOS) {
                        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                        set("uiCodigoArea", table.cell(rowIdx, 6).data());
                    } else {
                        table.$("input[type=radio]").prop("checked", false);
                        MensajeError("Categoría de la entidad es incorrecta para esta operación");
                    }
                    break;
                case DEVOLUCION_ANTICIPIO_LIQUIDABLE:
                    if (codigoCategoriaEntidad == CATEGORIA_EMPLEADO || codigoCategoriaEntidad == CATEGORIA_VENDEDOR || codigoCategoriaEntidad == CATEGORIA_RUTERO_LOCAL ||
                        codigoCategoriaEntidad == CATEGORIA_RUTERO_INTERIOR || codigoCategoriaEntidad == CATEGORIA_CAFETERIA || codigoCategoriaEntidad == CATEGORIA_SUPERMERCADOS) {
                        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                        set("uiCodigoArea", table.cell(rowIdx, 6).data());
                    } else {
                        table.$("input[type=radio]").prop("checked", false);
                        MensajeError("Categoría de la entidad es incorrecta para esta operación");
                    }
                    break;
                case PRESTAMO:
                    if (codigoCategoriaEntidad == CATEGORIA_EMPLEADO || codigoCategoriaEntidad == CATEGORIA_VENDEDOR || codigoCategoriaEntidad == CATEGORIA_RUTERO_LOCAL ||
                        codigoCategoriaEntidad == CATEGORIA_RUTERO_INTERIOR || codigoCategoriaEntidad == CATEGORIA_CAFETERIA || codigoCategoriaEntidad == CATEGORIA_SUPERMERCADOS ||
                        codigoCategoriaEntidad == CATEGORIA_EMPLEADO_INDIRECTO || codigoCategoriaEntidad == CATEGORIA_PERSONA_EXTERNA) {
                        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                        set("uiCodigoArea", table.cell(rowIdx, 6).data());
                    } else {
                        table.$("input[type=radio]").prop("checked", false);
                        MensajeError("Categoría de la entidad es incorrecta para esta operación");
                    }
                    break;
                case ABONO_A_PRESTAMO:
                    if (codigoCategoriaEntidad == CATEGORIA_EMPLEADO || codigoCategoriaEntidad == CATEGORIA_VENDEDOR || codigoCategoriaEntidad == CATEGORIA_RUTERO_LOCAL ||
                        codigoCategoriaEntidad == CATEGORIA_RUTERO_INTERIOR || codigoCategoriaEntidad == CATEGORIA_CAFETERIA || codigoCategoriaEntidad == CATEGORIA_SUPERMERCADOS ||
                        codigoCategoriaEntidad == CATEGORIA_EMPLEADO_INDIRECTO || codigoCategoriaEntidad == CATEGORIA_PERSONA_EXTERNA) {
                        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                        set("uiCodigoArea", table.cell(rowIdx, 6).data());
                    } else {
                        table.$("input[type=radio]").prop("checked", false);
                        MensajeError("Categoría de la entidad es incorrecta para esta operación");
                    }
                    break;
                case RETIRO_SOCIOS:
                    if (codigoCategoriaEntidad == CATEGORIA_SOCIO) {
                        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                        set("uiCodigoArea", table.cell(rowIdx, 6).data());
                    } else {
                        table.$("input[type=radio]").prop("checked", false);
                        MensajeError("Categoría de la entidad es incorrecta para esta operación");
                    }
                    break;
                case DEVOLUCION_SOCIOS:
                    if (codigoCategoriaEntidad == CATEGORIA_SOCIO) {
                        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                        set("uiCodigoArea", table.cell(rowIdx, 6).data());
                    } else {
                        table.$("input[type=radio]").prop("checked", false);
                        MensajeError("Categoría de la entidad es incorrecta para esta operación");
                    }
                    break;
                case ANTICIPO_SALARIO:
                    if (codigoCategoriaEntidad == CATEGORIA_EMPLEADO) {
                        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                        set("uiCodigoArea", table.cell(rowIdx, 6).data());
                    } else {
                        table.$("input[type=radio]").prop("checked", false);
                        MensajeError("Categoría de la entidad es incorrecta para esta operación");
                    }
                    break;
                case BACK_TO_BACK:
                    if (codigoCategoriaEntidad == CATEGORIA_EMPLEADO) {
                        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                        set("uiCodigoArea", table.cell(rowIdx, 6).data());
                    } else {
                        table.$("input[type=radio]").prop("checked", false);
                        MensajeError("Categoría de la entidad es incorrecta para esta operación");
                    }
                    break;
                default:
                    set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                    set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                    set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                    set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                    set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                    set("uiCodigoArea", table.cell(rowIdx, 6).data());
                    setSueldosIndirectosDonPepe(false.false);

                    /*if (codigoOperacionEntidad != 0) {
                        if (codigoOperacionEntidad != codigoOperacion) {
                            table.$("input[type=radio]").prop("checked", false);
                            MensajeError("Operación incorrecta para la entidad seleccionada");
                        } else {
                            set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                            set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                            set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                            set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                            set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                            set("uiCodigoArea", table.cell(rowIdx, 6).data());
                            setSueldosIndirectosDonPepe(false.false);
    
                            if (codigoCategoriaEntidad == PROVEEDOR_MATERIA_PRIMA ||
                                codigoCategoriaEntidad == PROVEEDOR_MATERIAL_DE_EMPAQUE ||
                                codigoCategoriaEntidad == PROVEEDOR_SERVICIOS_DE_FABRICACION ||
                                codigoCategoriaEntidad == PROVEEDOR_INSUMOS_DE_FABRICACION ||
                                codigoCategoriaEntidad == PROVEEDOR_SERVICIOS_VARIOS ||
                                codigoCategoriaEntidad == PROVEEDOR_INSUMOS_VARIOS) {
                                document.getElementById('div-captura-proveedor').style.display = 'block';
                                elementNombreProveedor.classList.add('obligatorio');
                            }
                        }// fin if
    
                    } else {
                        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                        set("uiCodigoArea", table.cell(rowIdx, 6).data());
                        setSueldosIndirectosDonPepe(false.false);
                    }*/
                    break;
            } // fin switch
        }// fin else
    });
}


function addEntidad() {
    setI("uiTitlePopupEntidad", "Registro de Nueva Entidad");
    document.getElementById("ShowPopupEntidad").click();
    set("uiNewEntidadNombre", "");
    set("uiNewCodigoCliente", "0");
    document.getElementById('div-tabla-clientes').style.display = 'none';
    fillCategoriaEntidad();
}

function mostrarTodosLosClientes(obj) {
    set("uiNewCodigoCliente", "0");
    set("uiNewEntidadNombre", "");
    let codigoCategoriaEntidad = parseInt(obj.value);
    if (codigoCategoriaEntidad == CLIENTES_ESPECIALES_1) {
        document.getElementById('div-tabla-clientes').style.display = 'block';
        document.getElementById("uiNewEntidadNombre").readOnly = true;
        intelligenceSearchCliente();
    } else {
        document.getElementById('div-tabla-clientes').style.display = 'none';
        document.getElementById("uiNewEntidadNombre").readOnly = false;
    }
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
        eventoradio: "Clientes"
    }
    pintar(objGlobalConfigTransaccion);
}

function fillCategoriaEntidad() {
    fetchGet("EntidadCategoria/GetCategoriasParaRegistrarEntidad", "json", function (rpta) {
        FillCombo(rpta, "uiNewEntidadCategoria", "codigoCategoriaEntidad", "nombreCategoriaEntidad", "- seleccione -", "-1");
    })
}

function getDataRowRadioClientes(obj) {
    let table = $('#tablaClientes').DataTable();
    $('#tablaClientes tbody').on('change', 'tr', 'input:radio', function () {
        let rowIdx = table.row(this).index();
        set("uiNewCodigoCliente", table.cell(rowIdx, 1).data());
        set("uiNewEntidadNombre", table.cell(rowIdx, 2).data());
    });
}

function GuardarEntidad(obj) {
    let errores = ValidarDatos("frmNewEntidad")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let codigoOperacionCaja = 0;
    let frmGuardar = document.getElementById("frmNewEntidad");
    let frm = new FormData(frmGuardar);
    let nombreEntidad = frm.get("NombreEntidad");
    let codigoCategoriaEntidad = frm.get("CodigoCategoriaEntidad");
    if (codigoCategoriaEntidad == CLIENTES_ESPECIALES_1) {
        codigoOperacionCaja = OPERACION_ESPECIALES_1;
    } else {
        if (codigoCategoriaEntidad == CLIENTES_ESPECIALES_2) {
            codigoOperacionCaja = OPERACION_ESPECIALES_2;
        } 
    }
    let codigoArea = 0;
    let codigoTemporal1 = 0;
    let codigoTemporal2 = 0;
    let objCategoria = document.getElementById('uiNewEntidadCategoria');
    let nombreCategoria = objCategoria.options[objCategoria.selectedIndex].text;
    
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Entidad/GuardarEntidad/?idUsuario=" + obj, "text", frm, function (data) {
            //if (typeof (data) === 'number') { // Valida si data es de tipo numerico, no valida que el contenido de data contiene solo números
            // Valida si el contenido de data contenga solo números
            if (/^[0-9]+$/.test(data)) {
                document.getElementById("uiClosePopupEntidad").click();
                let table = $('#tabla').DataTable();
                table.row.add([
                    "<input type='radio' name='radio' class='table-row-selected chkSelected' value='" + data + "' onclick = 'getDataRowRadioEntidades(this)'></input>",
                    data,
                    nombreEntidad,
                    codigoCategoriaEntidad,
                    nombreCategoria,
                    codigoOperacionCaja,
                    codigoArea,
                    codigoTemporal1,
                    codigoTemporal2

                ]).draw();

            } else {
                MensajeError(data);
            }
        })
    })
}

function getDataRowRadioGenerico() {

}


function closePopupVendedores() {
    $("#staticBackdropVendedores").hide();
}


function getDataRowRadioVendedores(obj) {
    let table = $('#tablaVendedores').DataTable();
    $('#tablaVendedores tbody').on('change', 'tr', 'input:radio', function () {
        let rowIdx = table.row(this).index();
        set("uiCodigoVendedor", table.cell(rowIdx, 1).data());
        set("uiNombreVendedor", table.cell(rowIdx, 2).data());
        set("uiNumeroRuta", table.cell(rowIdx, 3).data());
        set("uiCodigoCategoriaVendedor", table.cell(rowIdx, 5).data());
    });
    document.getElementById("uiClosePopupVendedores").click();
}

function setDataEntidadPorNombreVendedor(obj) {
    let checkEsVendedor = document.getElementById('uiEsVendedor').checked;
    if (checkEsVendedor == true) {
        let codigoVendedor = get("uiCodigoVendedor");
        let nombreVendedor = get("uiNombreVendedor");
        let codigoCategoriaVendedor = get("uiCodigoCategoriaVendedor");
        set("uiCodigoArea", "0");
        set("uiCodigoEntidad", codigoVendedor);
        set("uiCodigoCategoriaEntidad", codigoCategoriaVendedor);
        set("uiNombreEntidad", nombreVendedor);
    } else {
        set("uiCodigoArea", "0");
        set("uiCodigoEntidad", "");
        set("uiCodigoCategoriaEntidad", "0");
        set("uiNombreEntidad", "");
    }
}

function listarRuteros() {
    let objGlobalConfigTransaccion = {
        url: "Transaccion/GetListaVendedores",
        cabeceras: ["codigo", "nombre del vendedor", "ruta","canal de venta", "categoria"],
        propiedades: ["codigoVendedor", "nombreVendedor", "ruta","canalVenta","codigoCategoriaEntidad"],
        divContenedorTabla: "divContenedorTablaRuteros",
        divPintado: "divTablaRuteros",
        idtabla: "tablaVendedores",
        radio: true,
        paginar: true,
        eventoradio: "Vendedores",
        slug: "ruta"
    }
    pintar(objGlobalConfigTransaccion);
}

function setComboConsumidorConcedeIVA()
{
    // Clear el Combo del Nit de la empresa a quien se le concede el IVA
    let select = document.getElementById("cboNitConsumidor");
    select.options.length = 0;
    select.options[select.options.length] = new Option('No Aplica', '1000');
}

function clearDataFactura() {
    setComboConsumidorConcedeIVA();
    set("uiSerieFactura","");
    set("uiNumeroDocumento", "");
}

function fillShowDataAdicionalPlanilla(obj) {
    let codigoOperacion = parseInt(document.getElementById("cboOperacion").value);
    if (codigoOperacion == PLANILLA_PAGO || codigoOperacion == PLANILLA_BONO_14 || codigoOperacion == PLANILLA_AGUINALDO) {
        let elementCodigoTipoPago = document.getElementById("uiCodigoTipoPago");
        elementCodigoTipoPago.selectedIndex = 0;
        if (codigoOperacion == PLANILLA_PAGO) {
            elementCodigoTipoPago.classList.add('obligatorio');
        }
        let elementAnioPlanilla = document.getElementById("uiAnioPlanilla");
        let elementTipoPlanilla = document.getElementById("uiCodigoFrecuenciaPago");
        let elementMesPlanilla = document.getElementById("uiMesPlanilla");
        let elementSemanaPlanilla = document.getElementById("uiSemanaPlanilla");
        let elementQuincenaPlanilla = document.getElementById("uiCodigoQuincenaPlanilla");
        elementAnioPlanilla.classList.add('obligatorio');
        elementTipoPlanilla.classList.add('obligatorio');
        let arrayDate = getFechaSistema();
        let numeroMes = arrayDate[0]["mes"];
        // obtiene el codigo del tipo de planilla
        let valor = parseInt(obj);
        switch (valor) {
            case TIPO_PLANILLA_MENSUAL:
                fillMesesPlanilla(numeroMes);
                document.getElementById('uiContainerMesPlanilla').style.display = 'block';
                document.getElementById('uiContainerSemanaPlanilla').style.display = 'none';
                document.getElementById('uiContainerQuincenaPlanilla').style.display = 'none';
                FillComboUnicaOpcion("uiCodigoQuincenaPlanilla", "-1", "-- No Aplica -- ");
                elementSemanaPlanilla.classList.remove('obligatorio');
                elementQuincenaPlanilla.classList.remove('obligatorio');
                elementMesPlanilla.classList.add('obligatorio');
                break;
            case TIPO_PLANILLA_QUINCENAL:
                fillMesesPlanilla(numeroMes);
                document.getElementById('uiContainerSemanaPlanilla').style.display = 'none';
                document.getElementById('uiContainerMesPlanilla').style.display = 'block';
                if (codigoOperacion == PLANILLA_AGUINALDO || codigoOperacion == PLANILLA_BONO_14) {
                    document.getElementById('uiContainerQuincenaPlanilla').style.display = 'none';
                    elementQuincenaPlanilla.classList.remove('obligatorio');
                    elementMesPlanilla.classList.add('obligatorio');
                } else {
                    document.getElementById('uiContainerQuincenaPlanilla').style.display = 'block';
                    elementMesPlanilla.classList.remove('obligatorio');
                    elementQuincenaPlanilla.classList.add('obligatorio');
                    fillComboQuincenas();
                }
                break;
            case TIPO_PLANILLA_SEMANAL:
                fillMesesPlanilla(numeroMes);
                elementQuincenaPlanilla.classList.remove('obligatorio');
                if (codigoOperacion == PLANILLA_AGUINALDO || codigoOperacion == PLANILLA_BONO_14) {
                    document.getElementById('uiContainerMesPlanilla').style.display = 'block';
                    elementSemanaPlanilla.classList.remove('obligatorio');
                    elementMesPlanilla.classList.add('obligatorio');
                } else {
                    document.getElementById('uiContainerSemanaPlanilla').style.display = 'block';
                    document.getElementById('uiContainerMesPlanilla').style.display = 'none';
                    document.getElementById('uiContainerQuincenaPlanilla').style.display = 'none';
                    FillComboUnicaOpcion("uiCodigoQuincenaPlanilla", "-1", "-- No Aplica -- ");
                    elementSemanaPlanilla.classList.add('obligatorio');
                    fillComboSemanasPlanilla();
                }
                break;
            default:
                document.getElementById('uiContainerMesPlanilla').style.display = 'none';
                document.getElementById('uiContainerSemanaPlanilla').style.display = 'none';
                document.getElementById('uiContainerQuincenaPlanilla').style.display = 'none';
                FillComboUnicaOpcion("uiCodigoQuincenaPlanilla", "-1", "-- No Aplica -- ");
                elementMesPlanilla.classList.remove('obligatorio');
                elementSemanaPlanilla.classList.remove('obligatorio');
                elementQuincenaPlanilla.classList.remove('obligatorio');
                break;
        }
    } else {
        FillComboUnicaOpcion("uiAnioPlanilla", "-1", "-- No Aplica -- ");
        document.getElementById('uiCodigoFrecuenciaPago').selectedIndex = 0;
        FillComboUnicaOpcion("uiMesPlanilla", "0", "-- No Aplica -- ");
        FillComboUnicaOpcion("uiSemanaPlanilla", "0", "-- No Aplica -- ");
        FillComboUnicaOpcion("uiCodigoQuincenaPlanilla", "-1", "-- No Aplica -- ");
    }
}

function setDataAdicionalPlanilla(codigoFrecuenciaPago, data) {
    let codigoOperacion = parseInt(document.getElementById("cboOperacion").value);
    let elementCodigoTipoPago = document.getElementById("uiCodigoTipoPago");
    elementCodigoTipoPago.selectedIndex = 0;
    if (codigoOperacion == PLANILLA_PAGO) {
        elementCodigoTipoPago.classList.add('obligatorio');
    }
    let elementAnioPlanilla = document.getElementById("uiAnioPlanilla");
    let elementTipoPlanilla = document.getElementById("uiCodigoFrecuenciaPago");
    let elementMesPlanilla = document.getElementById("uiMesPlanilla");
    let elementSemanaPlanilla = document.getElementById("uiSemanaPlanilla");
    let elementQuincenaPlanilla = document.getElementById("uiCodigoQuincenaPlanilla");
    elementAnioPlanilla.classList.add('obligatorio');
    elementTipoPlanilla.classList.add('obligatorio');
    let arrayDate = getFechaSistema();
    let numeroMes = arrayDate[0]["mes"];

    switch (codigoFrecuenciaPago) {
        case TIPO_PLANILLA_MENSUAL:
            fillMesesPlanilla(numeroMes);
            document.getElementById('uiContainerMesPlanilla').style.display = 'block';
            document.getElementById('uiContainerSemanaPlanilla').style.display = 'none';
            document.getElementById('uiContainerQuincenaPlanilla').style.display = 'none';
            FillComboUnicaOpcion("uiCodigoQuincenaPlanilla", "-1", "-- No Aplica -- ");
            elementSemanaPlanilla.classList.remove('obligatorio');
            elementQuincenaPlanilla.classList.remove('obligatorio');
            elementMesPlanilla.classList.add('obligatorio');
            // Set Mes de la Planilla
            document.getElementById('uiMesPlanilla').value = data.mesPlanilla.toString();
            document.getElementById('uiCodigoTipoPago').value = data.codigoTipoPago.toString();
            break;
        case TIPO_PLANILLA_QUINCENAL:
            fillMesesPlanilla(numeroMes);
            document.getElementById('uiContainerSemanaPlanilla').style.display = 'none';
            document.getElementById('uiContainerMesPlanilla').style.display = 'block';
            if (codigoOperacion == PLANILLA_AGUINALDO || codigoOperacion == PLANILLA_BONO_14) {
                document.getElementById('uiContainerQuincenaPlanilla').style.display = 'none';
                elementQuincenaPlanilla.classList.remove('obligatorio');
                elementMesPlanilla.classList.add('obligatorio');
                document.getElementById('uiMesPlanilla').value = data.mesPlanilla.toString();
                document.getElementById('uiCodigoTipoPago').value = data.codigoTipoPago.toString();
            } else {
                document.getElementById('uiContainerQuincenaPlanilla').style.display = 'block';
                elementMesPlanilla.classList.remove('obligatorio');
                elementQuincenaPlanilla.classList.add('obligatorio');
                let numeroMes = parseInt(document.getElementById("uiMesPlanilla").value);
                let anio = parseInt(document.getElementById("uiAnioPlanilla").value);
                fetchGet("ProgramacionQuincenal/GetListaQuincenas/?anio=" + anio.toString() + "&numeroMes=" + numeroMes.toString(), "json", function (rpta) {
                    if (rpta != undefined && rpta != null && rpta.length != 0) {
                        FillCombo(rpta, "uiCodigoQuincenaPlanilla", "codigoQuincenaPlanilla", "periodo", "- seleccione -", "-1");
                        document.getElementById('uiCodigoQuincenaPlanilla').value = data.codigoQuincenaPlanilla.toString();
                    } else {
                        FillComboUnicaOpcion("uiCodigoQuincenaPlanilla", "-1", "-- No Existen Fechas -- ");
                    }
                });
                document.getElementById('uiCodigoTipoPago').value = data.codigoTipoPago.toString();
            }
            break;
        case TIPO_PLANILLA_SEMANAL:
            fillMesesPlanilla(numeroMes);
            elementQuincenaPlanilla.classList.remove('obligatorio');
            if (codigoOperacion == PLANILLA_AGUINALDO || codigoOperacion == PLANILLA_BONO_14) {
                document.getElementById('uiContainerMesPlanilla').style.display = 'block';
                elementSemanaPlanilla.classList.remove('obligatorio');
                elementMesPlanilla.classList.add('obligatorio');
            } else {
                document.getElementById('uiContainerSemanaPlanilla').style.display = 'block';
                document.getElementById('uiContainerMesPlanilla').style.display = 'none';
                document.getElementById('uiContainerQuincenaPlanilla').style.display = 'none';
                FillComboUnicaOpcion("uiCodigoQuincenaPlanilla", "-1", "-- No Aplica -- ");
                elementSemanaPlanilla.classList.add('obligatorio');
                // Set Semana de planilla
                let anio = document.getElementById("uiAnioPlanilla").value;
                let numeroSemanaActual = parseInt(document.getElementById("uiNumeroSemanaActualSistema").value);
                fetchGet("ProgramacionSemanal/GetListaSemanasPlanilla/?anio=" + anio.toString() + "&numeroSemana=" + numeroSemanaActual.toString(), "json", function (rpta) {
                    if (rpta != undefined && rpta != null) {
                        FillCombo(rpta, "uiSemanaPlanilla", "numeroSemana", "periodo", "- seleccione -", "-1");
                        document.getElementById('uiSemanaPlanilla').value = data.semanaPlanilla.toString();
                    }
                })
                document.getElementById('uiCodigoTipoPago').value = data.codigoTipoPago.toString();
            }
            break;
        default:
            document.getElementById('uiContainerMesPlanilla').style.display = 'none';
            document.getElementById('uiContainerSemanaPlanilla').style.display = 'none';
            document.getElementById('uiContainerQuincenaPlanilla').style.display = 'none';
            FillComboUnicaOpcion("uiCodigoQuincenaPlanilla", "-1", "-- No Aplica -- ");
            elementMesPlanilla.classList.remove('obligatorio');
            elementSemanaPlanilla.classList.remove('obligatorio');
            elementQuincenaPlanilla.classList.remove('obligatorio');
            break;
    }// fin switch
}


function setDataAdicionaTipoDocumento(obj) {
    let element = document.getElementById("cboEmpresas");
    let checkConcedeIva = document.getElementById("uiNitEmpresaConcedeIva");
    fillComboEmpresasConcecionIVA(checkConcedeIva);
    checkConcedeIva.checked = false;
    document.getElementById('div-documento-vale').style.display = 'none';
    document.getElementById('div-documento-factura').style.display = 'none';
    document.getElementById('div-forma-pago-deposito').style.display = 'none';
    document.getElementById("uiNitEmpresaConcedeIva").checked = false;
    let codigoTipoDocumento = parseInt(obj, 10);
    switch (codigoTipoDocumento) {
        case VALE:
            document.getElementById('div-documento-vale').style.display = 'block';
            element.classList.remove('obligatorio');
            break;
        case FACTURA:
            document.getElementById('div-documento-factura').style.display = 'block';
            element.classList.add('obligatorio');
            break;
        case BOLETA_PAGO:
            document.getElementById('div-forma-pago-deposito').style.display = 'block';
            element.classList.remove('obligatorio');
            break;
        default:
    }
}

function fillComboEmpresasConcecionIVA(obj) {
    if (obj.checked) {
        let element = document.getElementById("cboNitConsumidor");
        element.classList.add('obligatorio');
        fetchGet("Transaccion/FillEmpresasConcecionIVA", "json", function (rpta) {
            FillCombo(rpta, "cboNitConsumidor", "nit", "nombre", "- seleccione -", "-1");
        })
    } else {
        let element = document.getElementById("cboNitConsumidor");
        element.classList.remove('obligatorio');
        let select = document.getElementById("cboNitConsumidor");
        select.options.length = 0;
        select.options[select.options.length] = new Option('No Aplica', '1000');
    }
}

function mostrarVendedores() {
    setI("uiTitlePopupVendedores", "Listado de Vendedores");
    document.getElementById("ShowPopupRuteros").click();
    listarRuteros();
}

function emptyComboTipoCuentaPorCobrar() {
    let elementTipoCuentaPorCobrar = document.getElementById("uiTipoCuentaPorCobrar");
    let elementFechaPrestamo = document.getElementById("uiFechaPrestamo");
    let elementFechaInicioPago = document.getElementById("uiFechaInicioPago");
    elementTipoCuentaPorCobrar.classList.remove('obligatorio');
    elementFechaPrestamo.classList.remove('obligatorio');
    elementFechaInicioPago.classList.remove('obligatorio');
    set("uiFechaPrestamo", "");
    set("uiFechaInicioPago", "");
    document.getElementById('div-tipo-cuenta-por-cobrar').style.display = 'none';
    FillComboUnicaOpcion("uiTipoCuentaPorCobrar", "-1", "-- No Aplica -- ");
}

function fillComboTipoCuentaPorCobrar() {
    let elementTipoCuentaPorCobrar = document.getElementById("uiTipoCuentaPorCobrar");
    let elementFechaPrestamo = document.getElementById("uiFechaPrestamo");
    let elementFechaInicioPago = document.getElementById("uiFechaInicioPago");
    elementTipoCuentaPorCobrar.classList.add('obligatorio');
    elementFechaPrestamo.classList.add('obligatorio');
    elementFechaInicioPago.classList.add('obligatorio');
    document.getElementById('div-tipo-cuenta-por-cobrar').style.display = 'block';
    fetchGet("CuentasPorCobrar/GetListTiposCuentasPorCobrar", "json", function (rpta) {
        FillCombo(rpta, "uiTipoCuentaPorCobrar", "codigoTipoCuentaPorCobrar", "nombre", "- seleccione -", "-1");
    });
}




function RegistrarCorreccion() {
    let errores = ValidarDatos("frmActualizarTransaccion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let codigoOperacion = parseInt(document.getElementById("cboOperacion").value);
    // Si la operación es Bonos Extras, validar que se haya seleccionado el tipo de bono extra
    if (codigoOperacion == PLANILLA_BONOS_EXTRAS) {
        if (!document.querySelector('input[name="CodigoBonoExtra"]:checked')) {
            MensajeError("Debe seleccionar el tipo de bonos extras");
            return;
        }
    } else {
        if (codigoOperacion == OPERACION_ESPECIALES_1) {// validar que se haya seleccionado el tipo de especiales 1
            if (!document.querySelector('input[name="TipoEspeciales1"]:checked')) {
                MensajeError("Debe seleccionar el tipo de especiales 1");
                return;
            }
        }
    }
    if (!document.querySelector('input[name="CodigoTipoDocumento"]:checked')) {
        MensajeError("Debe seleccionar un tipo de documento");
        return;
    }
    let checkEfectivo = document.getElementById("uiEfectivo");
    let checkDeposito = document.getElementById("uiDeposito");
    let checkCheque = document.getElementById("uiCheque");
    if (checkEfectivo.checked == false && checkDeposito.checked == false && checkCheque.checked == false) {
        MensajeError("Debe seleccionar una forma de pago");
        return;
    }

    let frmGuardar = document.getElementById("frmActualizarTransaccion");
    let frm = new FormData(frmGuardar);
    if (checkEfectivo.checked == true) {
        frm.set("Efectivo", "1");
    } else {
        frm.set("Efectivo", "0");
    }
    if (checkDeposito.checked == true) {
        frm.set("Deposito", "1");
    } else {
        frm.set("Deposito", "0");
    }
    if (checkCheque.checked == true) {
        frm.set("Cheque", "1");
    } else {
        frm.set("Cheque", "0");
    }

    Confirmacion("Corrección de Transacción", "¿Está Seguro(a) de realizar la corrección de esta transacción?", function (rpta) {
        fetchPost("Transaccion/RegistrarCorreccion", "text", frm, function (data) {
            if (data == "OK") {
                Exito("Transaccion", "CorreccionTransaccion", true);
            } else {
                MensajeError(data);
            }
        })
    })
}



function ActualizarTransaccion(){
    let errores = ValidarDatos("frmActualizarTransaccion")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let codigoOperacion = parseInt(document.getElementById("cboOperacion").value);
    // Si la operación es Bonos Extras, validar que se haya seleccionado el tipo de bono extra
    if (codigoOperacion == PLANILLA_BONOS_EXTRAS) {
        if (!document.querySelector('input[name="CodigoBonoExtra"]:checked')) {
            MensajeError("Debe seleccionar el tipo de bonos extras");
            return;
        }
    } else {
        if (codigoOperacion == OPERACION_ESPECIALES_1) {// validar que se haya seleccionado el tipo de especiales 1
            if (!document.querySelector('input[name="TipoEspeciales1"]:checked')) {
                MensajeError("Debe seleccionar el tipo de especiales 1");
                return;
            }
        }
    }
    if (!document.querySelector('input[name="CodigoTipoDocumento"]:checked')) {
        MensajeError("Debe seleccionar un tipo de documento");
        return;
    }
    let checkEfectivo = document.getElementById("uiEfectivo");
    let checkDeposito = document.getElementById("uiDeposito");
    let checkCheque = document.getElementById("uiCheque");
    if (checkEfectivo.checked == false && checkDeposito.checked == false && checkCheque.checked == false) {
        MensajeError("Debe seleccionar una forma de pago");
        return;
    }

    let frmGuardar = document.getElementById("frmActualizarTransaccion");
    let frm = new FormData(frmGuardar);
    if (checkEfectivo.checked == true) {
        frm.set("Efectivo", "1");
    } else {
        frm.set("Efectivo", "0");
    }
    if (checkDeposito.checked == true) {
        frm.set("Deposito", "1");
    } else {
        frm.set("Deposito", "0");
    }
    if (checkCheque.checked == true) {
        frm.set("Cheque", "1");
    } else {
        frm.set("Cheque", "0");
    }

    Confirmacion("Corrección de Transacción", "¿Está Seguro(a) de realizar la corrección de esta transacción?", function (rpta) {
        fetchPost("Transaccion/ActualizarTransaccion", "text", frm, function (data) {
            if (data == "OK") {
                Exito("Transaccion", "Index", true);
            } else {
                MensajeError(data);
            }
        })
    })
}


