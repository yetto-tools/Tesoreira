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

function setEmptyDataTransaccion(checkEspeciales2, fechaOperacionStr) {
    document.getElementById("uiNitEmpresaConcedeIva").checked = false;
    setComboConsumidorConcedeIVA();
    FillComboUnicaOpcion("uiNumeroCuenta", "-1", "-- No existe cuenta -- ");
    clearDataFormulario();
    let selectOption = document.getElementById("cboOperacion");
    selectOption.selectedIndex = 0;
    let setSemanaAnterior = parseInt(document.getElementById("uiSetSemanaAnterior").value);
    if (setSemanaAnterior == 1) {
        fillComboSemanaAnterior();
    }
    if (checkEspeciales2 == true) {
        document.querySelector('#cboOperacion').value = VENTAS_EN_RUTA.toString();
        showControls(VENTAS_EN_RUTA.toString());
        setCheckedValueOfRadioButtonGroup('FechaStr', fechaOperacionStr);
        document.querySelector('#uiEsEspeciales2').checked = true;
        document.getElementById("uiEsEspeciales2").onClick = showTablaEspeciales2(true);
    }
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
    let elementFechaDocumento = document.getElementById("uiFechaDocumento");
    let elementMontoEfectivo = document.getElementById("uiMontoEfectivo");
    let elementMontoCheques = document.getElementById("uiMontoCheques");
    
    elementBanco.selectedIndex = 0;
    elementBanco.classList.remove('obligatorio');
    elementNumeroCuenta.classList.remove('obligatorio');
    elementNumeroBoleta.classList.remove('obligatorio');
    elementFechaDocumento.classList.remove('obligatorio');
    elementMontoEfectivo.classList.remove('obligatorio');
    elementMontoCheques.classList.remove('obligatorio');

    FillComboUnicaOpcion("uiNumeroCuenta", "-1", "-- No existe cuenta -- ");
    elementNumeroBoleta.value = "";
    elementFechaDocumento.value = "";
    elementMontoEfectivo.value = "";
    elementMontoCheques.value = "";
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
    FillComboUnicaOpcion("uiProveedorEntidad", "-1", "-- No existe proveedor -- ");
    set("uiCodigoOperacionCaja", "0");
    document.getElementById("uiTitleTipoDocumentoDeposito").innerHTML = "Número de Documento";
    document.getElementById("uiOptionTipoDocumentoNumeroVoucher").checked = false;
    //document.getElementById("uiNumeroBoleta").readOnly = true;
    document.getElementById("uiOptionTipoTransaccionNF").checked = true;

    clearDataTransaccion();
    // inicializa los datos de planilla
    document.getElementById('uiEsAjustePlanilla').value = "0";
    let elementTipoPlanillaPago = document.getElementById('div-tipo-planilla-pago');
    let elementPlanillaPago = document.getElementById('div-planilla-pago');
    elementTipoPlanillaPago.style.display = 'none'
    elementTipoPlanillaPago.checked = false;
    elementPlanillaPago.style.display = 'none';
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
    document.getElementById("uiTransferencia").disabled = false;
    document.getElementById("uiBoletaProval").disabled = false;
    document.getElementById("uiNingunTipoDocumento").disabled = false;

    document.getElementById("uiVale").checked = false;
    document.getElementById("uiFactura").checked = false;
    document.getElementById("uiBoletaDeposito").checked = false;
    document.getElementById("uiTransferencia").checked = false;
    document.getElementById("uiBoletaProval").checked = false;
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

    document.getElementById('divTablaEspeciales1').style.display = 'none';
    let tableEspeciales1 = $('#tablaEspeciales1').DataTable();
    tableEspeciales1.$("input[type=radio]").prop("checked", false);
    tableEspeciales1.$("input[type=search]").val('');
    tableEspeciales1.search('').draw();

    // Especiales 2
    document.getElementById("uiEsEspeciales2").checked = false;
    document.getElementById('divTablaEspeciales2').style.display = 'none';
    let tableEspeciales2 = $('#tablaEspeciales2').DataTable();
    tableEspeciales2.$("input[type=radio]").prop("checked", false);
    tableEspeciales2.$("input[type=search]").val('');
    tableEspeciales2.search('').draw();

    // Back To Back
    document.getElementById('divTablaBackToBack').style.display = 'none';
    let tableBackToBack = $('#tablaBackToBack').DataTable();
    tableBackToBack.$("input[type=radio]").prop("checked", false);
    tableBackToBack.$("input[type=search]").val('');
    tableBackToBack.search('').draw();

    // Vendedores
    document.getElementById('divTablaVendedores').style.display = 'none';
    let tableVendedores = $('#tablaVendedores').DataTable();
    tableVendedores.$("input[type=radio]").prop("checked", false);
    tableVendedores.$("input[type=search]").val('');
    tableVendedores.search('').draw();

    // Montos
    document.getElementById('uiCalculadora').value = "";
    document.getElementById('uiMontoTransaccion').value = "";

    // entidades
    let table = $('#tabla').DataTable();
    table.$("input[type=radio]").prop("checked", false);
    table.$("input[type=search]").val('');
    table.search('').draw();
    document.getElementById('divTabla').style.display = 'block';

    // Saldo Cuentas por Cobrar
    document.getElementById('uiSaldoAnteriorCuentaPorCobrar').value = "0.00";
    document.getElementById('uiMontoDevolucion').value = "0.00";
    document.getElementById('uiSaldoActualCuentaPorCobrar').value = "0.00";

    // Gastos Generales
    document.getElementById('div-operacion-gasto').style.display = 'none';
    FillComboUnicaOpcion("uiEntidadGasto", "-1", "-- No existe entidad gasto -- ");
    FillComboUnicaOpcion("uiProveedorEntidad", "-1", "-- No existe proveedor gasto -- ");
    FillComboUnicaOpcion("uiCategoriaEntidadGastos", "-1", "-- No existe categoria gasto -- ");
}

function setValueNumeroDocumentoDeposito(obj) {
    let codigoTipoDocumentoDeposito = parseInt(obj.value);
    document.getElementById("uiNumeroBoleta").readOnly = false;
    switch (codigoTipoDocumentoDeposito) {
        case 1:
            document.getElementById("uiTitleTipoDocumentoDeposito").innerHTML = "Número de Boleta";
            break;
        case 2:
            document.getElementById("uiTitleTipoDocumentoDeposito").innerHTML = "Número de Documento";
            break;
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
        });

        document.getElementById('divTablaEspeciales1').style.display = 'none';
        document.getElementById('divTabla').style.display = 'block';
        let table = $('#tabla').DataTable();
        table.$("input[type=radio]").prop("checked", false);
        table.$("input[type=search]").val('');
        table.search('').draw();

    } else {
        elementOtrosIngresos.classList.remove('obligatorio');
        document.getElementById('div-otros-ingresos').style.display = 'none';

        document.getElementById('divTabla').style.display = 'none';
        document.getElementById('divTablaEspeciales1').style.display = 'block';
        let tableEspeciales1 = $('#tablaEspeciales1').DataTable();
        tableEspeciales1.$("input[type=radio]").prop("checked", false);
        tableEspeciales1.$("input[type=search]").val('');
        tableEspeciales1.search('').draw();
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
                setCheckedValueOfRadioButtonGroup('CodigoTipoDocumento', data.codigoTipoDocumento.toString());


                document.querySelector('#uiEfectivo').checked = data.efectivo == 1 ? true : false;
                document.querySelector('#uiDeposito').checked = data.deposito == 1 ? true : false;
                document.querySelector('#uiCheque').checked = data.cheque == 1 ? true : false;


                set("uiCodigoEntidad", data.codigoEntidad);
                set("uiCodigoCategoriaEntidad", data.codigoCategoriaEntidad.toString());
                set("uiCategoriaEntidad", data.categoriaEntidad);
                set("uiCodigoCanalVenta", data.codigoCanalVenta.toString());
                set("uiNombreEntidad", data.nombreEntidad);
                set("uiMontoTransaccion", Number(data.monto).toFixed(2).toString());

                // Cuentas por Cobrar
                set("uiSaldoAnteriorCuentaPorCobrar", Number(data.montoSaldoAnteriorCxC).toFixed(2).toString());
                set("uiMontoDevolucion", Number(data.monto).toFixed(2).toString());
                set("uiSaldoActualCuentaPorCobrar", Number(data.montoSaldoActualCxC).toFixed(2).toString());

                set("uiObservaciones", data.observaciones);
                set("uiCodigoReporte", data.codigoReporte.toString());
                set("uiCodigoArea", data.codigoArea.toString());
                set("uiCodigoOperacionCaja", data.codigoOperacionCaja.toString());
                set("uiNumeroRecibo", data.numeroRecibo.toString());
                set("uiNumeroReciboReferencia", data.numeroReciboReferencia.toString());
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
    let elementFechaDocumento = document.getElementById("uiFechaDocumento");
    let elementMontoEfectivo = document.getElementById("uiMontoEfectivo");
    let elementMontoCheques = document.getElementById("uiMontoCheques");
    elementBanco.classList.add('obligatorio');
    elementNumeroCuenta.classList.add('obligatorio');
    elementNumeroBoleta.classList.add('obligatorio');
    elementFechaDocumento.classList.add('obligatorio');
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
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';

            // Default
            setCheckedValueOfRadioButtonGroup('CodigoTipoDocumentoDeposito', "2");

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
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('divTablaBackToBack').style.display = 'block';
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
        case COMPRAS_MATERIA_PRIMA:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'block';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            fillEntidadesGasto(COMPRAS_MATERIA_PRIMA);
            break;
        case COMPRAS_MATERIAL_EMPAQUE:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'block';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            fillEntidadesGasto(COMPRAS_MATERIAL_EMPAQUE);
            break;
        case GASTOS_EN_COMBUSTIBLES:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'block';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            fillEntidadesGasto(GASTOS_EN_COMBUSTIBLES);
            break;
        case GASTOS_INDIRECTOS:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-tipo-gasto-indirecto').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'block';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            fillEntidadesGasto(GASTOS_INDIRECTOS);
            break;
        case GASTOS_ADMINISTRATIVOS:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'block';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            fillEntidadesGasto(GASTOS_ADMINISTRATIVOS);
            break;
        case GASTOS_MANTENIMIENTO_VEHICULOS:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'block';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            fillEntidadesGasto(GASTOS_MANTENIMIENTO_VEHICULOS);
            break;
        case VIATICO_DEFINITIVO:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'block';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            fillEntidadesGasto(VIATICO_DEFINITIVO);
            break;
        case GASTOS_MANTENIMIENTO_MAQUINARIA:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'block';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            fillEntidadesGasto(GASTOS_MANTENIMIENTO_MAQUINARIA);
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
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('divTablaVendedores').style.display = 'block';
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
            document.getElementById('divTabla').style.display = 'none';
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
            document.getElementById('div-saldo-cuenta-por-cobrar').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('uiTitleDevolucionCxC').innerHTML = 'Monto Préstamo';
            fillComboTipoCuentaPorCobrar();
            break;
        case ABONO_A_PRESTAMO:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-saldo-cuenta-por-cobrar').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('uiTitleDevolucionCxC').innerHTML = 'Monto Abono';
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
            document.getElementById('div-saldo-cuenta-por-cobrar').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('uiTitleDevolucionCxC').innerHTML = 'Monto Devolución';
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
        case ANTICIPO_LIQUIDABLE:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-saldo-cuenta-por-cobrar').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('uiTitleDevolucionCxC').innerHTML = 'Monto Anticipo';
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
        case ANTICIPO_SALARIO:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-saldo-cuenta-por-cobrar').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('uiTitleDevolucionCxC').innerHTML = 'Monto Anticipo';
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
        case DEVOLUCION_ANTICIPO_SALARIO:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-saldo-cuenta-por-cobrar').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('uiTitleDevolucionCxC').innerHTML = 'Monto Devolución';
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
        case RESERVA_EGRESO_FORMACION:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-saldo-cuenta-por-cobrar').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'none';
            set("uiCodigoCategoriaEntidad", CATEGORIA_EMPRESA.toString());
            set("uiCodigoEntidad", EMPRESA_PANIFICADORA_AMERICANA_INDIVIDUAL.toString());
            set("uiCodigoArea", "0");
            set("uiNombreEntidad", "Panificadora Americana Individual");
            set("uiObservaciones", "");
            document.getElementById("uiNingunTipoDocumento").checked = true;
            document.getElementById('uiNingunTipoDocumento').onClick = setDataAdicionaTipoDocumento("0");
            document.getElementById("uiEfectivo").checked = true;
            break;
        case RESERVA_INGRESO_EJECUCION:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-saldo-cuenta-por-cobrar').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'none';
            set("uiCodigoCategoriaEntidad", CATEGORIA_EMPRESA.toString());
            set("uiCodigoEntidad", EMPRESA_PANIFICADORA_AMERICANA_INDIVIDUAL.toString());
            set("uiCodigoArea", "0");
            set("uiNombreEntidad", "Panificadora Americana Individual");
            set("uiObservaciones", "");
            document.getElementById("uiNingunTipoDocumento").checked = true;
            document.getElementById('uiNingunTipoDocumento').onClick = setDataAdicionaTipoDocumento("0");
            document.getElementById("uiEfectivo").checked = true;
            break;
        default:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-saldo-cuenta-por-cobrar').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'none';
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
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';

            // Seleccionar el tipo de documento BOLETA DE DEPOSITO
            switch (data.codigoTipoDocumento) {
                case BOLETA_PAGO:
                    document.getElementById('uiBoletaDeposito').checked = true;
                    document.getElementById('uiBoletaDeposito').onClick = setDataAdicionaTipoDocumento("3");
                    break;
                case TRANSFERENCIA:
                    document.getElementById('uiTransferencia').checked = true;
                    document.getElementById('uiTransferencia').onClick = setDataAdicionaTipoDocumento("4");
                    break;
                case PROVAL:
                    document.getElementById('uiBoletaProval').checked = true;
                    document.getElementById('uiBoletaProval').onClick = setDataAdicionaTipoDocumento("5");
                    break;
                default:
                    document.getElementById('uiNingunTipoDocumento').checked = true;
                    document.getElementById('uiNingunTipoDocumento').onClick = setDataAdicionaTipoDocumento("0");
                    break;
            }

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
            document.getElementById('uiFechaDocumento').value = data.fechaDocumentoStr;
            switch (data.codigoTipoDocumentoDeposito) {
                case NUMERO_BOLETA:
                    document.getElementById('uiTitleTipoDocumentoDeposito').innerHTML = "Número de Boleta";
                    document.getElementById('uiNumeroBoleta').value = data.numeroBoleta;
                    break;
                case NUMERO_VAUCHER:
                    document.getElementById('uiTitleTipoDocumentoDeposito').innerHTML = "Número de Documento";
                    document.getElementById('uiNumeroBoleta').value = data.numeroVoucher;
                    break;
                default:
                    document.getElementById('uiTitleTipoDocumentoDeposito').innerHTML = "";
                    document.getElementById('uiNumeroBoleta').value = "";
                    break;
            }
            break;
        case BACK_TO_BACK:
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('divTablaBackToBack').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            break;
        case COMPRAS_MATERIA_PRIMA:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'block';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            fillEntidadesGasto(COMPRAS_MATERIA_PRIMA);
            break;
        case COMPRAS_MATERIAL_EMPAQUE:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'block';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            fillEntidadesGasto(COMPRAS_MATERIAL_EMPAQUE);
            break;
        case GASTOS_EN_COMBUSTIBLES:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'block';
            document.getElementById('uiVale').checked = true;
            document.getElementById('uiVale').onClick = setDataAdicionaTipoDocumento("1");
            document.getElementById("uiEfectivo").checked = true;
            fillEntidadesGasto(GASTOS_EN_COMBUSTIBLES);
            break;
        case GASTOS_INDIRECTOS:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-tipo-gasto-indirecto').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            fillEntidadesGasto(GASTOS_INDIRECTOS);
            break;
        case GASTOS_ADMINISTRATIVOS:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            fillEntidadesGasto(GASTOS_ADMINISTRATIVOS);
            break;
        case GASTOS_MANTENIMIENTO_VEHICULOS:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'block';
            fillEntidadesGasto(GASTOS_MANTENIMIENTO_VEHICULOS);
            break;
        case VIATICO_DEFINITIVO:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'block';
            fillEntidadesGasto(VIATICO_DEFINITIVO);
            break;
        case GASTOS_MANTENIMIENTO_MAQUINARIA:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-operacion-gasto').style.display = 'block';
            fillEntidadesGasto(GASTOS_MANTENIMIENTO_MAQUINARIA);
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
            document.getElementById('divTabla').style.display = 'none';
            document.getElementById('divTablaVendedores').style.display = 'block';
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
            document.getElementById('divTabla').style.display = 'none';
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

                    document.getElementById('divTablaEspeciales1').style.display = 'none';
                    document.getElementById('divTabla').style.display = 'block';
                    let table = $('#tabla').DataTable();
                    table.$("input[type=radio]").prop("checked", false);
                    table.$("input[type=search]").val('');
                    table.search('').draw();
                })
            } else {
                document.getElementById('div-otros-ingresos').style.display = 'none';
                elementOtrosIngresos.classList.remove('obligatorio');

                document.getElementById('divTabla').style.display = 'none';
                document.getElementById('divTablaEspeciales1').style.display = 'block';
                let tableEspeciales1 = $('#tablaEspeciales1').DataTable();
                tableEspeciales1.$("input[type=radio]").prop("checked", false);
                tableEspeciales1.$("input[type=search]").val('');
                tableEspeciales1.search('').draw();
            }

            break;
        case PRESTAMO:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-saldo-cuenta-por-cobrar').style.display = 'block';
            document.getElementById('uiTitleDevolucionCxC').innerHTML = 'Monto Préstamo';

            let elementTipoCuentaPorCobrar = document.getElementById("uiTipoCuentaPorCobrar");
            elementTipoCuentaPorCobrar.classList.add('obligatorio');
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
            document.getElementById('div-saldo-cuenta-por-cobrar').style.display = 'block';
            document.getElementById('uiTitleDevolucionCxC').innerHTML = 'Monto Abono';
            // Forma de Pago
            document.getElementById("uiEfectivo").disabled = false;
            document.getElementById("uiDeposito").disabled = true;
            document.getElementById("uiCheque").disabled = true;
            break;
        case ANTICIPO_LIQUIDABLE:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-saldo-cuenta-por-cobrar').style.display = 'block';
            document.getElementById('uiTitleDevolucionCxC').innerHTML = 'Monto Anticipo';
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
            document.getElementById('div-saldo-cuenta-por-cobrar').style.display = 'block';
            document.getElementById('uiTitleDevolucionCxC').innerHTML = 'Monto Devolución';
            // Forma de Pago
            document.getElementById("uiEfectivo").disabled = false;
            document.getElementById("uiDeposito").disabled = true;
            document.getElementById("uiCheque").disabled = true;
            break;
        case ANTICIPO_SALARIO:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-saldo-cuenta-por-cobrar').style.display = 'block';
            document.getElementById('uiTitleDevolucionCxC').innerHTML = 'Monto Anticipo';
            // Forma de Pago
            document.getElementById("uiEfectivo").disabled = false;
            document.getElementById("uiDeposito").disabled = true;
            document.getElementById("uiCheque").disabled = true;
            break;
        case DEVOLUCION_ANTICIPO_SALARIO:
            document.getElementById('divTabla').style.display = 'block';
            document.getElementById('div-planilla-pago').style.display = 'none';
            document.getElementById('uiContainerTipoPago').style.display = 'none';
            document.getElementById('div-tipo-bonos-extra').style.display = 'none';
            document.getElementById('div-tipo-especiales1').style.display = 'none';
            document.getElementById('div-bonos-extra').style.display = 'none';
            document.getElementById('div-saldo-cuenta-por-cobrar').style.display = 'block';
            document.getElementById('uiTitleDevolucionCxC').innerHTML = 'Monto Devolución';
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
            document.getElementById('div-tipo-cuenta-por-cobrar').style.display = 'none';
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


function GuardarDatos(nombreImpresora, numeroCopias) {
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

    let checkEspeciales2 = document.getElementById("uiEsEspeciales2");
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

    let fechaOperacionStr = frm.get("FechaStr");
    // Quitar las comas del valor del monto
    frm.set("monto", frm.get("monto").replaceAll(',', ''));

    let codigoTipoOperacion = parseInt(frm.get("CodigoTipoOperacion"));

    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Transaccion/GuardarDatos/?complemento=0", "text", frm, function (data) {
            if (!/^[0-9]+$/.test(data)) {
                MensajeError(data);
            } else {
                let setSemanaAnterior = parseInt(document.getElementById("uiSetSemanaAnterior").value);
                if (setSemanaAnterior == 0) {
                    PrintConstanciaIngresos(data, codigoTipoOperacion, nombreImpresora, numeroCopias);
                    setTimeout(() => {
                        setEmptyDataTransaccion(checkEspeciales2.checked, fechaOperacionStr);
                        Exito("Transaccion", "Index", false);
                    }, 1000);

                } else {
                    setEmptyDataTransaccion(checkEspeciales2.checked, fechaOperacionStr);
                    Exito("Transaccion", "Index", false);
                }
            }
        })
    })
}


/* Temporalmente obsoleto */
//function intelligenceSearch10() {
//    // Incluir el radioButton al inicio, por eso se comienza por la columna 1
//    let objGlobalConfigTransaccion = {
//        url: "Transaccion/ListarEntidadesGenericas",
//        cabeceras: ["codigo", "nombre entidad", "codigo categoria", "categoria", "codigo operacion", "codigoArea","codigoOperacionEntidad","codigoCanalVenta"],
//        propiedades: ["codigoEntidad", "nombreEntidad", "codigoCategoriaEntidad", "nombreCategoria","codigoOperacionCaja","codigoArea","codigoOperacionEntidad","codigoCanalVenta"],
//        divContenedorTabla: "divContenedorTabla",
//        ocultarColumnas: true,
//        hideColumns: [
//            {   "targets": [3], 
//                "visible": false
//            }, {
//                "targets": [5],
//                "visible": false
//            }, {
//                "targets": [6],
//                "visible": false
//            }, {
//                "targets": [7],
//                "visible": false
//            }, {
//                "targets": [8],
//                "visible": false
//            }],
//        divPintado: "divTabla",
//        radio: true,
//        paginar: true,
//        eventoradio: "Entidades",
//        slug: "codigoEntidad"

//    }
//    pintar(objGlobalConfigTransaccion);
//}


//"className": "dt-body-right"
function intelligenceSearch() {
    fetchGet("Entidad/GetAllEntidadesGenericas", "json", function (rpta) {
        if (rpta != null) {
            let listaEntidadesGenericas = rpta.listaEntidadesGenericas;
            let listaEntidadesEspeciales1 = rpta.listaEntidadesEspeciales1;
            let listaEntidadesEspeciales2 = rpta.listaEntidadesEspeciales2;
            let listaEntidadesBackToBack = rpta.listaEntidadesBackToBack;
            let listaEntidadesVendedores = rpta.listaEntidadesVendedores;

            let objConfigEntidadesGenericas = {
                cabeceras: ["codigo", "nombre entidad", "codigo categoria", "categoria", "codigo operacion", "codigoArea", "codigoOperacionEntidad", "codigoCanalVenta"],
                propiedades: ["codigoEntidad", "nombreEntidad", "codigoCategoriaEntidad", "nombreCategoria", "codigoOperacionCaja", "codigoArea", "codigoOperacionEntidad", "codigoCanalVenta"],
                divContenedorTabla: "divContenedorTabla",
                divPintado: "divTabla",
                ocultarColumnas: true,
                hideColumns: [
                    {
                        "targets": [0],
                        "className": "dt-body-center"
                    }, {
                        "targets": [3],
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
                radio: true,
                paginar: true,
                eventoradio: "Entidades",
                slug: "codigoEntidad"
            }
            pintarEntidades(objConfigEntidadesGenericas, listaEntidadesGenericas);

            let objConfigEspeciales1 = {
                cabeceras: ["codigo", "nombre entidad", "codigo categoria", "categoria", "codigo operacion", "codigoArea", "codigoOperacionEntidad", "codigoCanalVenta"],
                propiedades: ["codigoEntidad", "nombreEntidad", "codigoCategoriaEntidad", "nombreCategoria", "codigoOperacionCaja", "codigoArea", "codigoOperacionEntidad", "codigoCanalVenta"],
                divContenedorTabla: "divContenedorTablaEspeciales1",
                divPintado: "divTablaEspeciales1",
                idtabla: "tablaEspeciales1",
                ocultarColumnas: true,
                hideColumns: [
                    {
                        "targets": [0],
                        "className": "dt-body-center"
                    }, {
                        "targets": [3],
                        "visible": false
                    }, {
                        "targets": [4],
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
                radio: true,
                paginar: true,
                eventoradio: "EntidadesEspeciales1",
                slug: "codigoEntidad",
                autoWidth: false
            }
            pintarEntidades(objConfigEspeciales1, listaEntidadesEspeciales1);

            let objConfigEspeciales2 = {
                cabeceras: ["codigo", "nombre entidad", "codigo categoria", "categoria", "codigo operacion", "codigoArea", "codigoOperacionEntidad", "codigoCanalVenta"],
                propiedades: ["codigoEntidad", "nombreEntidad", "codigoCategoriaEntidad", "nombreCategoria", "codigoOperacionCaja", "codigoArea", "codigoOperacionEntidad", "codigoCanalVenta"],
                divContenedorTabla: "divContenedorTablaEspeciales2",
                divPintado: "divTablaEspeciales2",
                idtabla: "tablaEspeciales2",
                ocultarColumnas: true,
                hideColumns: [
                    {
                        "targets": [0],
                        "className": "dt-body-center"
                    }, {
                        "targets": [3],
                        "visible": false
                    }, {
                        "targets": [4],
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
                radio: true,
                paginar: true,
                eventoradio: "EntidadesEspeciales2",
                slug: "codigoEntidad",
                autoWidth: false
            }
            pintarEntidades(objConfigEspeciales2, listaEntidadesEspeciales2);

            let objConfigBackToBack = {
                cabeceras: ["codigo", "nombre entidad", "codigo categoria", "categoria", "codigo operacion", "codigoArea", "codigoOperacionEntidad", "codigoCanalVenta", "Mes", "Año","Monto a Devolver"],
                propiedades: ["codigoEntidad", "nombreEntidad", "codigoCategoriaEntidad", "nombreCategoria", "codigoOperacionCaja", "codigoArea", "codigoOperacionEntidad", "codigoCanalVenta", "mesPlanillaBTB", "anioPlanillaBTB", "montoDevolucionBTB"],
                divContenedorTabla: "divContenedorTablaBackToBack",
                divPintado: "divTablaBackToBack",
                idtabla: "tablaBackToBack",
                ocultarColumnas: true,
                hideColumns: [
                    {
                        "targets": [0],
                        "className": "dt-body-center"
                    }, {
                        "targets": [3],
                        "visible": false
                    }, {
                        "targets": [4],
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
                    }, {
                        "targets": [9],
                        "visible": true
                    }, {
                        "targets": [10],
                        "className": "dt-body-center",
                        "visible": true
                    }, {
                        "targets": [11],
                        "className": "dt-body-right",
                        "visible": true
                    }],
                radio: true,
                paginar: true,
                eventoradio: "EntidadesBackToBack",
                slug: "codigoEntidad",
                autoWidth: false
            }
            pintarEntidades(objConfigBackToBack, listaEntidadesBackToBack);

            let objConfigVendedores = {
                cabeceras: ["codigo", "nombre entidad", "codigo categoria", "categoria", "codigo operacion", "codigoArea", "codigoOperacionEntidad", "codigoCanalVenta"],
                propiedades: ["codigoEntidad", "nombreEntidad", "codigoCategoriaEntidad", "nombreCategoria", "codigoOperacionCaja", "codigoArea", "codigoOperacionEntidad", "codigoCanalVenta"],
                divContenedorTabla: "divContenedorTablaVendedores",
                divPintado: "divTablaVendedores",
                idtabla: "tablaVendedores",
                ocultarColumnas: true,
                hideColumns: [
                    {
                        "targets": [0],
                        "className": "dt-body-center"
                    }, {
                        "targets": [3],
                        "visible": false
                    }, {
                        "targets": [4],
                        "visible": true
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
                radio: true,
                paginar: true,
                eventoradio: "EntidadesVendedores",
                slug: "codigoEntidad",
                autoWidth: false
            }
            pintarEntidades(objConfigVendedores, listaEntidadesVendedores);

        } else {
            Warning("Error en la obtención de entidades");
        }
    });
}


function getDataRowRadioEntidades(obj) {
    let codigoTipoOperacion = parseInt(Array.from(document.getElementsByName("CodigoTipoOperacion")).find(r => r.checked).value, 10);
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
                    if (esBonoExtrasPorComisiones == false && esBonoExtrasOtros == false && esBonoQuintalaje == false && esBonoFeriadosODomingos == false) {
                        table.$("input[type=radio]").prop("checked", false);
                        MensajeError("Seleccione el tipo de bonos extras");
                    } else {
                        //if ((esBonoExtrasPorComisiones == true) && (codigoCategoriaEntidad == CATEGORIA_VENDEDOR || codigoCategoriaEntidad == CATEGORIA_RUTERO_LOCAL || codigoCategoriaEntidad == CATEGORIA_RUTERO_INTERIOR || codigoCategoriaEntidad == CATEGORIA_CAFETERIA || codigoCategoriaEntidad == CATEGORIA_SUPERMERCADOS)) {
                        if ((esBonoExtrasPorComisiones == true) && (codigoCategoriaEntidad == CATEGORIA_VENDEDOR || codigoCategoriaEntidad == CATEGORIA_RUTERO_LOCAL || codigoCategoriaEntidad == CATEGORIA_RUTERO_INTERIOR || codigoCategoriaEntidad == CATEGORIA_CAFETERIA || codigoCategoriaEntidad == CATEGORIA_SUPERMERCADOS) || codigoCategoriaEntidad == CATEGORIA_EMPLEADO) {
                            set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                            set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                            set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                            set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                            set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                            set("uiCodigoArea", table.cell(rowIdx, 6).data());
                            set("uiCodigoCanalVenta", table.cell(rowIdx, 8).data());
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
                    if (codigoCategoriaEntidad == CATEGORIA_EMPLEADO || codigoCategoriaEntidad == CATEGORIA_EMPLEADO_INDIRECTO || codigoCategoriaEntidad == CATEGORIA_VENDEDOR || codigoCategoriaEntidad == CATEGORIA_RUTERO_LOCAL ||
                        codigoCategoriaEntidad == CATEGORIA_RUTERO_INTERIOR || codigoCategoriaEntidad == CATEGORIA_CAFETERIA || codigoCategoriaEntidad == CATEGORIA_SUPERMERCADOS) {
                        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                        set("uiCodigoArea", table.cell(rowIdx, 6).data());
                        MostrarSaldoCuentaPorCobrar(codigoTipoOperacion, ANTICIPO_LIQUIDABLE, codigoEntidad, codigoCategoriaEntidad);
                    } else {
                        table.$("input[type=radio]").prop("checked", false);
                        MensajeError("Categoría de la entidad es incorrecta para esta operación");
                    }
                    break;
                case DEVOLUCION_ANTICIPIO_LIQUIDABLE:
                    if (codigoCategoriaEntidad == CATEGORIA_EMPLEADO || codigoCategoriaEntidad == CATEGORIA_EMPLEADO_INDIRECTO || codigoCategoriaEntidad == CATEGORIA_VENDEDOR || codigoCategoriaEntidad == CATEGORIA_RUTERO_LOCAL ||
                        codigoCategoriaEntidad == CATEGORIA_RUTERO_INTERIOR || codigoCategoriaEntidad == CATEGORIA_CAFETERIA || codigoCategoriaEntidad == CATEGORIA_SUPERMERCADOS) {
                        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                        set("uiCodigoArea", table.cell(rowIdx, 6).data());
                        MostrarSaldoCuentaPorCobrar(codigoTipoOperacion, DEVOLUCION_ANTICIPIO_LIQUIDABLE, codigoEntidad, codigoCategoriaEntidad);
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
                        MostrarSaldoCuentaPorCobrar(codigoTipoOperacion, PRESTAMO, codigoEntidad, codigoCategoriaEntidad);
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
                        MostrarSaldoCuentaPorCobrar(codigoTipoOperacion, ABONO_A_PRESTAMO, codigoEntidad, codigoCategoriaEntidad);
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
                        MostrarSaldoCuentaPorCobrar(codigoTipoOperacion, ANTICIPO_SALARIO, codigoEntidad, codigoCategoriaEntidad);
                    } else {
                        table.$("input[type=radio]").prop("checked", false);
                        MensajeError("Categoría de la entidad es incorrecta para esta operación");
                    }
                    break;
                case DEVOLUCION_ANTICIPO_SALARIO:
                    if (codigoCategoriaEntidad == CATEGORIA_EMPLEADO) {
                        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
                        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
                        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
                        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
                        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
                        set("uiCodigoArea", table.cell(rowIdx, 6).data());
                        MostrarSaldoCuentaPorCobrar(codigoTipoOperacion, DEVOLUCION_ANTICIPO_SALARIO, codigoEntidad, codigoCategoriaEntidad);
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


function getDataRowRadioEntidadesEspeciales1(obj) {
    // Incluir el radioButton al inicio, por eso se comienza por la columna 1
    document.getElementById('div-captura-proveedor').style.display = 'none';
    let elementNombreProveedor = document.getElementById('uiNombreProveedor');
    elementNombreProveedor.value = "";
    let table = $('#tablaEspeciales1').DataTable();
    $('#tablaEspeciales1 tbody').on('change', 'tr', 'input:radio', function () {
        let rowIdx = table.row(this).index();
        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
        set("uiCodigoArea", table.cell(rowIdx, 6).data());
        set("uiCodigoCanalVenta", table.cell(rowIdx, 8).data());
        //document.getElementById('div-ventas-en-ruta').style.display = 'none';
    });
}


function getDataRowRadioEntidadesEspeciales2(obj) {
    // Incluir el radioButton al inicio, por eso se comienza por la columna 1
    document.getElementById('div-captura-proveedor').style.display = 'none';
    let elementNombreProveedor = document.getElementById('uiNombreProveedor');
    elementNombreProveedor.value = "";
    let table = $('#tablaEspeciales2').DataTable();
    $('#tablaEspeciales2 tbody').on('change', 'tr', 'input:radio', function () {
        let rowIdx = table.row(this).index();
        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
        set("uiCodigoArea", table.cell(rowIdx, 6).data());
        set("uiCodigoCanalVenta", table.cell(rowIdx, 8).data());
        //document.getElementById('div-ventas-en-ruta').style.display = 'none';
        let elementRutaVendedor = document.getElementById("uiRutaVendedor");
        elementRutaVendedor.classList.remove('obligatorio');
        FillComboUnicaOpcion("uiRutaVendedor", "332", "332");
    });
}

function getDataRowRadioEntidadesBackToBack(obj) {
    // Incluir el radioButton al inicio, por eso se comienza por la columna 1
    document.getElementById('div-captura-proveedor').style.display = 'none';
    let elementNombreProveedor = document.getElementById('uiNombreProveedor');
    elementNombreProveedor.value = "";
    let table = $('#tablaBackToBack').DataTable();
    $('#tablaBackToBack tbody').on('change', 'tr', 'input:radio', function () {
        let rowIdx = table.row(this).index();
        set("uiCodigoEntidad", table.cell(rowIdx, 1).data());
        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
        set("uiCodigoCategoriaEntidad", table.cell(rowIdx, 3).data());
        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
        set("uiCodigoArea", table.cell(rowIdx, 6).data());
        set("uiCodigoCanalVenta", table.cell(rowIdx, 8).data());
        set("uiMontoTransaccion", table.cell(rowIdx, 11).data());
        //document.getElementById('div-ventas-en-ruta').style.display = 'none';
    });
}

function getDataRowRadioEntidadesVendedores(obj) {
    // Incluir el radioButton al inicio, por eso se comienza por la columna 1
    document.getElementById('div-captura-proveedor').style.display = 'none';
    let elementNombreProveedor = document.getElementById('uiNombreProveedor');
    elementNombreProveedor.value = "";
    let table = $('#tablaVendedores').DataTable();
    $('#tablaVendedores tbody').on('change', 'tr', 'input:radio', function () {
        let rowIdx = table.row(this).index();
        let codigoEntidad = table.cell(rowIdx, 1).data();
        let codigoCategoriaEntidad = table.cell(rowIdx, 3).data();
        set("uiCodigoEntidad", codigoEntidad);
        set("uiNombreEntidad", table.cell(rowIdx, 2).data());
        set("uiCodigoCategoriaEntidad", codigoCategoriaEntidad);
        set("uiCategoriaEntidad", table.cell(rowIdx, 4).data());
        set("uiCodigoOperacionCaja", table.cell(rowIdx, 5).data());
        set("uiCodigoArea", table.cell(rowIdx, 6).data());
        set("uiCodigoCanalVenta", table.cell(rowIdx, 8).data());
        fetchGet("Vendedores/GetRutasDelVendedor/?codigoCategoriaEntidad=" + codigoCategoriaEntidad + "&codigoVendedor=" + codigoEntidad, "json", function (rpta) {
            if (rpta != undefined && rpta != null && rpta.length != 0) {
                FillCombo(rpta, "uiRutaVendedor", "ruta", "ruta", "- seleccione -", "-1");
            } else {
                FillComboUnicaOpcion("uiRutaVendedor", "-1", "-- Sin Rutas -- ");
            }
        })

        let elementRutaVendedor = document.getElementById("uiRutaVendedor");
        document.getElementById('div-ventas-en-ruta').style.display = 'block';
        elementRutaVendedor.classList.add('obligatorio');
    });
}


function showTablaEspeciales2(obj) {
    if (obj == true) {
        document.getElementById('divTabla').style.display = 'none';
        document.getElementById('divTablaEspeciales2').style.display = 'block';
        document.getElementById('divTablaVendedores').style.display = 'none';

    } else {
        document.getElementById('divTabla').style.display = 'none';
        document.getElementById('divTablaEspeciales2').style.display = 'none';
        document.getElementById('divTablaVendedores').style.display = 'block';
    }
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
    set("uiNewCodigoCliente", "0");
    set("uiNewPrimerNombre", "");
    set("uiNewSegundoNombre", "");
    set("uiNewPrimerApellido", "");
    set("uiNewSegundoApellido", "");
    let elementNombreCompleto = document.getElementById("uiNewEntidadNombre");
    let elementPrimerNombre = document.getElementById("uiNewPrimerNombre");
    let elementPrimerApellido = document.getElementById("uiNewPrimerApellido");
    let elementGenero = document.getElementById("uiNewGenero");
    let codigoCategoriaEntidad = parseInt(obj.value);
    switch (codigoCategoriaEntidad) {
        case CLIENTES_ESPECIALES_1:
            document.getElementById('div-tabla-clientes').style.display = 'block';
            document.getElementById('div-empleado-indirecto').style.display = 'none';
            elementNombreCompleto.readOnly = true;
            elementNombreCompleto.classList.add('obligatorio');
            elementPrimerNombre.classList.remove('obligatorio');
            elementPrimerApellido.classList.remove('obligatorio');
            elementGenero.classList.remove('obligatorio');
            intelligenceSearchCliente();
            break;
        case CATEGORIA_EMPLEADO_INDIRECTO:
            document.getElementById('div-tabla-clientes').style.display = 'none';
            elementNombreCompleto.readOnly = true;
            elementNombreCompleto.classList.remove('obligatorio');
            elementPrimerNombre.classList.add('obligatorio');
            elementPrimerApellido.classList.add('obligatorio');
            elementGenero.classList.add('obligatorio');
            document.getElementById('div-empleado-indirecto').style.display = 'block';
            break;
        default:
            document.getElementById('div-tabla-clientes').style.display = 'none';
            document.getElementById('div-empleado-indirecto').style.display = 'none';
            elementNombreCompleto.readOnly = false;
            elementNombreCompleto.classList.add('obligatorio');
            elementPrimerNombre.classList.remove('obligatorio');
            elementPrimerApellido.classList.remove('obligatorio');
            elementGenero.classList.remove('obligatorio');
            break;
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
    if (nombreEntidad == "") {
        let primerNombre = frm.get("PrimerNombre");
        let primerApellido = frm.get("PrimerApellido");
        nombreEntidad = primerNombre + " " + primerApellido;
    }
    let codigoCategoriaEntidad = frm.get("CodigoCategoriaEntidad");
    if (codigoCategoriaEntidad == CLIENTES_ESPECIALES_1) {
        codigoOperacionCaja = OPERACION_ESPECIALES_1;
    } else {
        if (codigoCategoriaEntidad == CLIENTES_ESPECIALES_2) {
            codigoOperacionCaja = OPERACION_ESPECIALES_2;
        } 
    }
    let codigoArea = 0;
    let codigoOperacionEntidad = 0;
    let codigoCanalVenta = 0;
    let objCategoria = document.getElementById('uiNewEntidadCategoria');
    let nombreCategoria = objCategoria.options[objCategoria.selectedIndex].text;
    
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Entidad/GuardarEntidad/?idUsuario=" + obj, "text", frm, function (data) {
            //if (typeof (data) === 'number') { // Valida si data es de tipo numerico, no valida que el contenido de data contiene solo números
            // Valida si el contenido de data contenga solo números
            if (/^[0-9]+$/.test(data)) {
                document.getElementById("uiClosePopupEntidad").click();
                let table = $('#tabla').DataTable();
                if (codigoCategoriaEntidad == CLIENTES_ESPECIALES_1) {
                    table = $('#tablaEspeciales1').DataTable();
                } else {
                    if (codigoCategoriaEntidad == CLIENTES_ESPECIALES_2) {
                        table = $('#tablaEspeciales2').DataTable();
                    } 
                }
                table.row.add([
                    "<input type='radio' name='radio' class='table-row-selected chkSelected' value='" + data + "' onclick = 'getDataRowRadioEntidades(this)'></input>",
                    data,
                    nombreEntidad,
                    codigoCategoriaEntidad,
                    nombreCategoria,
                    codigoOperacionCaja,
                    codigoArea,
                    codigoOperacionEntidad,
                    codigoCanalVenta

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
    let table = $('#tablaRuteros').DataTable();
    $('#tablaRuteros tbody').on('change', 'tr', 'input:radio', function () {
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
        idtabla: "tablaRuteros",
        radio: true,
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [5],
                "visible": false
            }],
        eventoradio: "Vendedores",
        slug: "ruta",
        autoWidth: false
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
            clearDataDepositosBancarios();
            document.getElementById('div-forma-pago-deposito').style.display = 'block';
            element.classList.remove('obligatorio');
            break;
        case TRANSFERENCIA:
            clearDataDepositosBancarios();
            document.getElementById('div-forma-pago-deposito').style.display = 'block';
            element.classList.remove('obligatorio');
            break;
        case PROVAL:
            clearDataDepositosBancarios();
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
    elementTipoCuentaPorCobrar.classList.remove('obligatorio');
    document.getElementById('div-tipo-cuenta-por-cobrar').style.display = 'none';
    FillComboUnicaOpcion("uiTipoCuentaPorCobrar", "-1", "-- No Aplica -- ");
}

function fillComboTipoCuentaPorCobrar() {
    let elementTipoCuentaPorCobrar = document.getElementById("uiTipoCuentaPorCobrar");
    elementTipoCuentaPorCobrar.classList.add('obligatorio');
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

    // Quitar las comas del valor del monto
    frm.set("monto", frm.get("monto").replaceAll(',', ''));

    Confirmacion("Corrección de Transacción", "¿Está Seguro(a) de realizar la corrección de esta transacción?", function (rpta) {
        fetchPost("Transaccion/RegistrarCorreccion", "text", frm, function (data) {
            if (data == "OK") {
                Exito("Transaccion", "Revision", true);
            } else {
                MensajeError(data);
            }
        })
    })
}



function ActualizarTransaccion(nombreImpresora, numeroCopias){
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

    // Quitar las comas del valor del monto
    frm.set("monto", frm.get("monto").replaceAll(',', ''));

    let codigoTipoOperacion = parseInt(frm.get("CodigoTipoOperacion"));
    Confirmacion("Corrección de Transacción", "¿Está Seguro(a) de realizar la corrección de esta transacción?", function (rpta) {
        fetchPost("Transaccion/ActualizarTransaccion", "text", frm, function (data) {
            if (!/^[0-9]+$/.test(data)) {
                MensajeError(data);
            } else {
                PrintConstanciaIngresos(data, codigoTipoOperacion, nombreImpresora, numeroCopias);
                setTimeout(() => {
                    Exito("Transaccion", "Index", true);
                }, 1000);
            }
        })
    })
}

function myCalculadora() {
    let elementCalculadora = document.getElementById("uiCalculadora");
    let elementMonto = document.getElementById("uiMontoTransaccion");
    document.onkeypress = function (event) {
        if (event.key === "Enter") {
            try {
                if (elementCalculadora.value !== "") {
                    elementMonto.value = parseFloat(eval(elementCalculadora.value)).toFixed(2);
                    elementMonto.focus();
                    elementMonto.click();
                }
            } catch (e) {
                elementMonto.value = "0.00";
            }
            // Si la operacion es de cuentas por cobrar, se debe de llamar a calcularSaldoCuentaPorCobrar
            let codigoOperacion = parseInt(document.getElementById("cboOperacion").value);
            if (codigoOperacion == DEVOLUCION_ANTICIPIO_LIQUIDABLE ||
                codigoOperacion == DEVOLUCION_ANTICIPO_SALARIO ||
                codigoOperacion == ABONO_A_PRESTAMO ||
                codigoOperacion == ANTICIPO_LIQUIDABLE ||
                codigoOperacion == ANTICIPO_SALARIO ||
                codigoOperacion == PRESTAMO) {
                calcularSaldoCuentaPorCobrar();
            }// fin if
        }
    }
}


function MostrarSaldoCuentaPorCobrar(codigoTipoOperacion, codigoOperacion, codigoEntidad, codigoCategoriaEntidad) {
    fetchGet("CuentasPorCobrar/GetMontoCuentaPorCobrar/?codigoTipoOperacion=" + codigoTipoOperacion.toString() + "&codigoOperacion=" + codigoOperacion.toString() + "&codigoEntidad=" + codigoEntidad + "&codigoCategoriaEntidad=" + codigoCategoriaEntidad.toString(), "json", function (data) {
        document.getElementById("uiSaldoAnteriorCuentaPorCobrar").value = Number(data).toFixed(2);
        calcularSaldoCuentaPorCobrar();
    })
}


function sumarMontos() {
    let montoEfectivoStr = document.getElementById("uiMontoEfectivo").value;
    let montoChequesStr = document.getElementById("uiMontoCheques").value;
    let montoEfectivo = 0.00;
    let montoCheques = 0.00;
    if (montoEfectivoStr !== "") {
        montoEfectivo = parseFloat(montoEfectivoStr);
    } else {
        document.getElementById("uiMontoEfectivo").value = "0";
    }
    if (montoChequesStr !== "") {
        montoCheques = parseFloat(montoChequesStr);
    } else {
        document.getElementById("uiMontoCheques").value = "0";
    }
    if (montoEfectivo > 0 || montoCheques > 0) {
        let montoTotal = montoEfectivo + montoCheques;
        document.getElementById("uiMontoTransaccion").value = formatRegional(Number(montoTotal).toFixed(2));
    }
}

function calcularSaldoCuentaPorCobrar() {
    let codigoOperacion = parseInt(document.getElementById('cboOperacion').value);
    let valor = (document.getElementById("uiMontoTransaccion").value).replaceAll(',', '');
    if (codigoOperacion == DEVOLUCION_ANTICIPIO_LIQUIDABLE ||
        codigoOperacion == DEVOLUCION_ANTICIPO_SALARIO ||
        codigoOperacion == ABONO_A_PRESTAMO ||
        codigoOperacion == ANTICIPO_LIQUIDABLE ||
        codigoOperacion == ANTICIPO_SALARIO ||
        codigoOperacion == PRESTAMO) {

        let codigoTipoOperacion = parseInt(Array.from(document.getElementsByName("CodigoTipoOperacion")).find(r => r.checked).value, 10);
        if (valor !== "" && /^\d*(\.\d{1})?\d{0,1}$/.test(valor)) {
            document.getElementById("uiMontoTransaccion").value = formatRegional(valor);
            document.getElementById("uiMontoDevolucion").value = valor;
        } else {
            document.getElementById("uiMontoDevolucion").value = "0.00";
        }

        let montoSaldoAnterior = parseFloat(document.getElementById("uiSaldoAnteriorCuentaPorCobrar").value).toFixed(2);
        let montoDevolucion = parseFloat(document.getElementById("uiMontoDevolucion").value).toFixed(2);
        let montoSaldoActual = 0.00;
        switch (codigoTipoOperacion) {
            case 1:
                montoSaldoActual = parseFloat(montoSaldoAnterior) - parseFloat(montoDevolucion);
                break;
            case -1:
                montoSaldoActual = parseFloat(montoSaldoAnterior) + parseFloat(montoDevolucion);
                break;
            default:
                break
        }

        document.getElementById("uiSaldoActualCuentaPorCobrar").value = Number(montoSaldoActual).toFixed(2);
    } else {
        if (valor !== "" && /^\d*(\.\d{1})?\d{0,1}$/.test(valor)) {
            document.getElementById("uiMontoTransaccion").value = formatRegional(valor);
        }
    }
}


function focusMontoCheques() {
    let elementMontoCheques = document.getElementById("uiMontoCheques");
    elementMontoCheques.focus();
}


function focusMontroTransaccion() {
    sumarMontos();
    let elementMontoTransaccion = document.getElementById("uiMontoTransaccion");
    elementMontoTransaccion.focus();
    elementMontoTransaccion.click();
}


function focusObservaciones() {
    let elementObservaciones = document.getElementById("uiObservaciones");
    elementObservaciones.focus();
}

function fillEntidadesGasto(codigoOperacion) {
    fetchGet("Entidad/GetEntidadesGasto/?codigoOperacion=" + codigoOperacion.toString(), "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            FillComboUnicaOpcion("uiEntidadGasto", "-1", "-- No existe entidad -- ");
        } else {
            FillCombo(rpta, "uiEntidadGasto", "codigoEntidad", "nombre", "- seleccione -", "-1");
            FillCombo(rpta, "uiCategoriaEntidadGastos", "codigoEntidad", "codigoCategoriaEntidad", "- seleccione -", "-1");
        }
    })
}

function fillProveedoresEntidad() {
    let elementNombreProveedor = document.getElementById('uiNombreProveedor');
    let objEntidadGasto = document.getElementById("uiEntidadGasto");
    let codigoEntidad = parseInt(document.getElementById("uiEntidadGasto").value);
    let objCategoriaEntidadGasto = document.getElementById("uiCategoriaEntidadGastos");
    objCategoriaEntidadGasto.value = codigoEntidad;

    set("uiCodigoEntidad", codigoEntidad.toString());
    set("uiNombreEntidad", objEntidadGasto.options[objEntidadGasto.selectedIndex].text);
    set("uiCodigoCategoriaEntidad", objCategoriaEntidadGasto[objCategoriaEntidadGasto.selectedIndex].text);

    fetchGet("EntidadProveedores/GetProveedores/?codigoEntidad=" + codigoEntidad.toString(), "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            FillComboUnicaOpcion("uiProveedorEntidad", "-1", "-- No existe proveedor -- ");
            elementNombreProveedor.classList.remove('obligatorio');
        } else {
            FillCombo(rpta, "uiProveedorEntidad", "codigoProveedor", "nombreProveedor", "- seleccione -", "-1");
            elementNombreProveedor.classList.add('obligatorio');
        }
    })
}

function setNombreProveedorEntidad() {
    let objProveedorEntidad = document.getElementById('uiProveedorEntidad');
    if (objProveedorEntidad.value != "-1") {
        let elementNombreProveedor = document.getElementById('uiNombreProveedor');
        elementNombreProveedor.value = objProveedorEntidad.options[objProveedorEntidad.selectedIndex].text;
    } else {
        elementNombreProveedor.value = "";
    }
}




