window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    if (nameController == "PlanillaTemporal") {
        switch (nameAction) {
            case "PagosBackToBack":
                //MostrarFechaOperacionReporteCxC();
                FillAnioPlanilla();
                break;
            default:
                break;
        }// fin switch
    }// fin if
    else {
        if (nameController == "DepositosBTB") {
            switch (nameAction) {
                case "RegistroDepositosBTB":
                    FillAnioPlanilla();
                    break;
                case "ConsultaDepositosBTB":
                    fillAniosReporte("uiFiltroAnioPlanilla",false);
                    fillAniosReporte("uiFiltroAnioOperacion",true);
                    MostrarBTBDepositosConsulta(-1,-1,-1,-1);
                    break;
                case "EdicionDepositosBTB":
                    fillAniosReporte("uiFiltroAnioPlanilla", false);
                    fillAniosReporte("uiFiltroAnioOperacion", true);
                    MostrarBTBDepositosEdicion(-1, -1, -1);
                    break;
                default:
                        break;
            }// fin switch
        }//fin if
    }// fin else
}

function redirectPagina() {
    Redireccionar("PlanillaTemporal", "PagosBackToBack");
}

function FillAnioPlanilla() {
    let arrayDate = getFechaSistema();
    let anioActual = arrayDate[0]["anio"];
    let numeroMes = arrayDate[0]["mes"];
    let select = document.getElementById("uiFiltroAnioPlanilla");
    let anioAnterior = anioActual - 1;
    if (numeroMes == 1) {
        let data = [{ "value": anioAnterior.toString(), "text": anioAnterior.toString() }, { "value": anioActual.toString(), "text": anioActual.toString() }]
        FillCombo(data, "uiFiltroAnioPlanilla", "value", "text", "- seleccione -", "-1");
        select.selectedIndex = 0;

    } else {
        let data = [{ "value": anioActual.toString(), "text": anioActual.toString() }]
        FillCombo(data, "uiFiltroAnioPlanilla", "value", "text", "- seleccione -", "-1");
        select.selectedIndex = 0;
    }
}

function fillAniosReporte(idcontrol, bandera) {
    fetchGet("ProgramacionSemanal/GetAniosProgramacionSemanal", "json", function (rpta) {
        if (rpta != undefined && rpta != null && rpta.length != 0) {
            FillCombo(rpta, idcontrol, "anio", "anio", "- seleccione -", "-1");
            if (bandera == true) {
                fillSemanasReporte();
            }
        } else {
            FillComboUnicaOpcion(idcontrol, "-1", "-- No Existen Años -- ");
        }
    })
}

function fillSemanasReporte() {
    let arrayDate = getFechaSistema();
    let anioActual = arrayDate[0]["anio"];
    let numeroSemanaActual = parseInt(document.getElementById("uiNumeroSemanaActualSistema").value);
    let anioReporte = parseInt(get("uiFiltroAnioOperacion"));
    if (anioReporte != anioActual) {
        numeroSemanaActual = 53;
    }
    fetchGet("ProgramacionSemanal/GetSemanasAnteriores/?anio=" + anioReporte.toString() + "&numeroSemanaActual=" + numeroSemanaActual.toString() + "&numeroSemanas=55&incluirSemanaActual=1", "json", function (rpta) {
        if (rpta != undefined && rpta != null && rpta.length != 0) {
            FillCombo(rpta, "uiFiltroSemanaOperacion", "numeroSemana", "periodo", "- seleccione -", "-1");
        } else {
            FillComboUnicaOpcion("uiFiltroSemanaOperacion", "-1", "-- No Existen Fechas -- ");
        }
    })
}

function fillCuentasBancarias(obj) {
    let valor = parseInt(obj.value);
    fetchGet("CuentaBancaria/GetCuentasBancariasTesoreria/?codigoBanco=" + valor.toString(), "json", function (rpta) {
        if (rpta.length > 0) {
            FillCombo(rpta, "uiNumeroCuenta", "numeroCuenta", "numeroCuentaDescriptivo", "- seleccione cuenta -", "-1");
        } else {
            FillComboUnicaOpcion("uiNumeroCuenta", "-1", "-- No existe cuenta -- ");
        }
    })
}

function fillEditCuentasBancarias(obj) {
    let valor = parseInt(obj.value);
    fetchGet("CuentaBancaria/GetCuentasBancariasTesoreria/?codigoBanco=" + valor.toString(), "json", function (rpta) {
        if (rpta.length > 0) {
            FillCombo(rpta, "uiEditNumeroCuenta", "numeroCuenta", "numeroCuentaDescriptivo", "- seleccione cuenta -", "-1");
        } else {
            FillComboUnicaOpcion("uiEditNumeroCuenta", "-1", "-- No existe cuenta -- ");
        }
    })
}

function CalcularMontosPagosBTB() {
    let codigoTipoPlanilla = parseInt(document.getElementById("uiFiltroTipoPlanilla").value);
    let anioPlanilla = parseInt(document.getElementById("uiFiltroAnioPlanilla").value);
    let mesPlanilla = parseInt(document.getElementById("uiFiltroMes").value);
    MostrarEmpleadosBackToBack(codigoTipoPlanilla, anioPlanilla, mesPlanilla);
}

function MostrarEmpleadosBackToBack(codigoTipoPlanilla, anioPlanilla, mesPlanilla) {
    objConfiguracion = {
        url: "PlanillaTemporal/GetEmpleadosBackToBackPlanilla/?codigoTipoPlanilla=" + codigoTipoPlanilla.toString() + "&anioPlanilla=" + anioPlanilla.toString() + "&mesPlanilla=" + mesPlanilla.toString(),
        cabeceras: ["Código Empresa", "Empresa", "Código Empleado", "Nombre Empleado", "Código Operación", "Operación", "Codigo Frecuencia Pago", "Frecuencia de Pago", "Tipo BTB", "Bono Decreto 37-2001", "Salario Diario", "ExistePagoBTB","Monto Calculado", ],
        propiedades: ["codigoEmpresa", "nombreEmpresa", "codigoEmpleado", "nombreCompleto", "codigoOperacion", "operacion", "codigoFrecuenciaPago", "frecuenciaPago", "codigoTipoBTB", "bonoDecreto372001", "salarioDiario", "existePagoBTB", "montoDevolucionBTB"],
        divContenedorTabla: "divContenedorTabla",
        displaydecimals: ["monto"],
        divPintado: "divTabla",
        paginar: true,
        addTextBox: true,
        excluir: true,
        ExcluirEnabled: "disabled",
        fieldNameExcluir: "existePagoBTB",
        propertiesColumnTextBox: [
            {
                "header": "Monto Devolución",
                "value": "montoDevolucionBTB",
                "name": "MontoDevolucionBTB",
                "align": "text-right",
                "validate": "decimal-2"
            }, {
                "header": "Monto Descuento",
                "value": "montoDescuento",
                "name": "MontoDescuento",
                "align": "text-right",
                "validate": "decimal-2"
            }],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "visible": false
            }, {
                "targets": [4],
                "visible": false
            }, {
                "targets": [6],
                "visible": false
            }, {
                "targets": [8],
                "visible": true,
                "className": "dt-body-right"
            }],
        slug: "codigoEmpresa",
        sumarcolumna: true,
        columnasumalist: [12,13,14]
    }
    pintar(objConfiguracion);
}

function clickMontoPorDevolver(obj) {
    let table = $('#tabla').DataTable();
    $("#tabla td input").on('change', function () {
        var $td = $(this).parent();
        $td.find('input').attr('value', this.value);
        table.cell($td).invalidate().draw();

        let column = table.column(13);
        let parser = new DOMParser();
        let xmlDoc = "";
        let abstracts = "";

        let total = column.data().reduce(function (a, b) {
            let valorA = a;
            let valorB = b;
            if (!/^\d*(\.\d{1})?\d{0,1}$/.test(valorA)) {
                xmlDoc = parser.parseFromString(valorA, "text/xml");
                abstracts = xmlDoc.querySelectorAll("input");
                abstracts.forEach(a => {
                    valorA = a.getAttribute('value');
                });
            }

            if (!/^\d*(\.\d{1})?\d{0,1}$/.test(valorB)) {
                xmlDoc = parser.parseFromString(valorB, "text/xml");
                abstracts = xmlDoc.querySelectorAll("input");
                abstracts.forEach(a => {
                    valorB = a.getAttribute('value');
                });
            }

            if (isNaN(valorA) || isNaN(valorB)) {
                alert("Error en la sumatoria de columna");
            }

            return parseFloat(valorA) + parseFloat(valorB);  // calculate the mark column
        });

        const formatterQuetzales = new Intl.NumberFormat('qut', {
            minimumFractionDigits: 2 // 2 decimales
        });

        $(column.footer()).html(
            formatterQuetzales.format(total)
        );

    });
}

function FillReportesDeCaja() {
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnioOperacion").value);
    let semanaOperacion = parseInt(document.getElementById("uiFiltroSemanaOperacion").value);
    FillFechasDeCorte(anioOperacion, semanaOperacion);
}

function FillFechasDeCorte(anioOperacion, semanaOperacion) {
    fetchGet("CorteCajaSemanal/GetReportesCajaEnProceso/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString(), "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            FillComboUnicaOpcion("uiFiltroReporteCaja", "-1", "-- No existe reportes -- ");
        } else {
            FillCombo(rpta, "uiFiltroReporteCaja", "codigoReporte", "fechaCorteStr", "- seleccione -", "-1");
        }
    })
}


function CargarDevolucionBTB() {
    let codigoTipoPlanilla = parseInt(document.getElementById("uiFiltroTipoPlanilla").value);
    let anioPlanilla = parseInt(document.getElementById("uiFiltroAnioPlanilla").value);
    let mesPlanilla = parseInt(document.getElementById("uiFiltroMes").value);
    let codigoFrecuenciaPago = 1; // 1. Mensual
    let codigoOperacion = 65; // 65. Back to Back (Devolución)
    let table = $('#tabla').DataTable();
    var data = table.rows().data();
    let arrayProperties = new Array();
    let obj = null;
    let excluir = 0;
    data.each(function (value, index) {
        excluir = table.cell(index, 15).nodes().to$().find('input').prop('checked') == true ? 1 : 0;
        if (excluir == 0) {
            obj = {
                CodigoEmpresa: table.cell(index, 0).data(),
                CodigoEmpleado: table.cell(index, 2).data(),
                codigoTipoPlanilla: codigoTipoPlanilla,
                CodigoFrecuenciaPago: codigoFrecuenciaPago,
                CodigoOperacion: codigoOperacion,
                Anio: anioPlanilla,
                Mes: mesPlanilla,
                MontoCalculado: table.cell(index, 12).data(),
                MontoPlanillaExcel: table.cell(index, 13).nodes().to$().find('input').val(),
                MontoDescuento: table.cell(index, 14).nodes().to$().find('input').val()
            };
            arrayProperties.push(obj);
        }
    });
    if (Array.isArray(arrayProperties) && arrayProperties.length) {
        let jsonData = JSON.stringify(arrayProperties);
        Confirmacion(undefined, undefined, function (rpta) {
            fetchPostJson("PlanillaTemporal/GuardarDevolucionesBTB", "text", jsonData, function (data) {
                if (data == "OK") {
                    redirectPagina();
                } else {
                    MensajeError(data);
                }
            })
        })
    } else {
        Warning("No existen devoluciones o descuentos a ser registrados en cuentas por cobrar");
    }
}

function CalcularMontosDepositosBTB() {
    let codigoTipoPlanilla = parseInt(document.getElementById("uiFiltroTipoPlanilla").value);
    let anioPlanilla = parseInt(document.getElementById("uiFiltroAnioPlanilla").value);
    let mesPlanilla = parseInt(document.getElementById("uiFiltroMes").value);
    MostrarEmpleadosBackToBackDepositos(codigoTipoPlanilla, anioPlanilla, mesPlanilla);

}

function MostrarEmpleadosBackToBackDepositos(codigoTipoPlanilla, anioPlanilla, mesPlanilla) {
    objConfiguracion = {
        url: "PlanillaTemporal/GetEmpleadosBackToBackBoletaDeposito/?codigoTipoPlanilla=" + codigoTipoPlanilla.toString() + "&anioPlanilla=" + anioPlanilla.toString() + "&mesPlanilla=" + mesPlanilla.toString(),
        cabeceras: ["Código Empresa", "Empresa", "Código Empleado", "Nombre Empleado", "Código Operación", "Operación", "Codigo Frecuencia Pago", "Frecuencia de Pago", "Tipo BTB", "Bono Decreto 37-2001", "Salario Diario","Monto Depósito"],
        propiedades: ["codigoEmpresa", "nombreEmpresa", "codigoEmpleado", "nombreCompleto", "codigoOperacion", "operacion", "codigoFrecuenciaPago", "frecuenciaPago", "codigoTipoBTB", "bonoDecreto372001", "salarioDiario","montoDevolucionBTB"],
        divContenedorTabla: "divContenedorTabla",
        displaydecimals: ["monto"],
        divPintado: "divTabla",
        paginar: true,
        addTextBox: true,
        propertiesColumnTextBox: [
            , {
                "header": "Número de Boleta",
                "value": "numeroBoleta",
                "name": "NumeroBoleta",
                "align": "text-center",
                "validate": "solonumeros"
            }, {
                "header": "Monto Depósito",
                "value": "montoDevolucionBTB",
                "name": "MontoDevolucionBTB",
                "align": "text-right",
                "validate": "decimal-2"
            }],
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "visible": false
            }, {
                "targets": [4],
                "visible": false
            }, {
                "targets": [6],
                "visible": false
            }, {
                "targets": [8],
                "visible": true,
                "className": "dt-body-right"
            }],
        slug: "codigoEmpresa",
        sumarcolumna: true,
        columnasumalist: [13]
    }
    pintar(objConfiguracion);
}

function CargarBoletasDepositadas() {
    let anioPlanilla = document.getElementById("uiFiltroAnioPlanilla").value;
    let mesPlanilla = document.getElementById("uiFiltroMes").value;
    let objMesPlanilla = document.getElementById("uiFiltroMes");
    let nombreMesPlanilla = objMesPlanilla.options[objMesPlanilla.selectedIndex].text;
    let codigoTipoPlanilla = document.getElementById("uiFiltroTipoPlanilla").value;
    let objTipoPlanilla = document.getElementById("uiFiltroTipoPlanilla");
    let tipoPlanilla = objTipoPlanilla.options[objTipoPlanilla.selectedIndex].text;
    setI("uiTitlePopupSemanaBoletasDeposito", "Parámetros de carga de Boletas de Depósito");
    set("uiAnioPlanilla", anioPlanilla);
    set("uiCodigoTipoPlanilla", codigoTipoPlanilla);
    set("uiTipoPlanilla", tipoPlanilla);
    set("uiMesPlanilla", mesPlanilla);
    set("uiNombreMesPlanilla", nombreMesPlanilla);
    FillComboUnicaOpcion("uiNumeroCuenta", "-1", "-- No Existen Cuentas -- ");
    FillComboUnicaOpcion("uiFiltroReporteCaja", "-1", "-- No existen reportes -- ");
    fillAniosReporte("uiFiltroAnioOperacion",true);
    document.getElementById("ShowPopupSemanaBoletasDeposito").click();
}



function GuardarBoletasDepositoBTB() {
    let errores = ValidarDatos("frmDepositosBTB")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let codigoTipoPlanilla = parseInt(document.getElementById("uiCodigoTipoPlanilla").value);
    let anioPlanilla = parseInt(document.getElementById("uiAnioPlanilla").value);
    let mesPlanilla = parseInt(document.getElementById("uiMesPlanilla").value);
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnioOperacion").value);
    let semanaOperacion = parseInt(document.getElementById("uiFiltroSemanaOperacion").value);
    let codigoReporte = parseInt(document.getElementById("uiFiltroReporteCaja").value);
    let codigoBancoDeposito = parseInt(document.getElementById("uiCodigoBancoDeposito").value);
    let numeroCuenta = document.getElementById("uiNumeroCuenta").value
    let codigoFrecuenciaPago = 1; // 1. Mensual
    let codigoOperacion = 20; // 20. Depósito Bancario
    let table = $('#tabla').DataTable();
    var data = table.rows().data();
    let arrayProperties = new Array();
    let obj = null;
    let numeroBoleta = "";
    let monto = "";
    let boletasCompletas = true;
    data.each(function (value, index) {
        numeroBoleta = table.cell(index, 12).nodes().to$().find('input').val();
        monto = table.cell(index, 13).nodes().to$().find('input').val();
        if (/^[0-9]+$/.test(numeroBoleta) && (/^\d*(\.\d{1})?\d{0,1}$/.test(monto))) {
            obj = {
                CodigoTipoPlanilla: codigoTipoPlanilla,
                AnioPlanilla: anioPlanilla,
                MesPlanilla: mesPlanilla,
                CodigoEmpresa: table.cell(index, 0).data(),
                CodigoEmpleado: table.cell(index, 2).data(),
                CodigoFrecuenciaPago: codigoFrecuenciaPago,
                CodigoOperacion: codigoOperacion,
                AnioOperacion: anioOperacion,
                CodigoReporte: codigoReporte,
                SemanaOperacion: semanaOperacion,
                CodigoBancoDeposito: codigoBancoDeposito,
                NumeroCuenta: numeroCuenta,
                NumeroBoleta: numeroBoleta,
                Monto: table.cell(index, 13).nodes().to$().find('input').val()
            };
            arrayProperties.push(obj);
        } else {
            boletasCompletas = false;
        }
    });
    if (Array.isArray(arrayProperties) && arrayProperties.length) {
        let jsonData = JSON.stringify(arrayProperties);
        Confirmacion(undefined, undefined, function (rpta) {
            fetchPostJson("DepositosBTB/GuardarDepositosBTB", "text", jsonData, function (data) {
                if (data == "OK") {
                    if (boletasCompletas == true) {
                        Exito("DepositosBTB", "RegistroDepositosBTB", true, "Registro de boletas de depósito completas")
                    } else {
                        Exito("DepositosBTB", "RegistroDepositosBTB", true, "Algunas boletas no se registraron")
                    }
                } else {
                    MensajeError(data);
                }
            })
        })
    } else {
        Warning("No existen depósitos bancarios");
    }
}

function BuscarBoletasDepositosBTB() {
    let anioPlanilla = parseInt(document.getElementById("uiFiltroAnioPlanilla").value);
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnioOperacion").value);
    let semanaOperacion = parseInt(document.getElementById("uiFiltroSemanaOperacion").value);
    let codigoReporte = parseInt(document.getElementById("uiFiltroReporteCaja").value);
    MostrarBTBDepositosConsulta(anioPlanilla, anioOperacion, semanaOperacion, codigoReporte);
}

function MostrarBTBDepositosConsulta(anioPlanilla, anioOperacion, semanaOperacion, codigoReporte) {
    objConfiguracion = {
        url: "DepositosBTB/GetDepositosBTB/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString() + "&anioPlanilla=" + anioPlanilla.toString() + "&codigoReporte=" + codigoReporte.toString(),
        cabeceras: ["Código Pago", "Año Planilla", "Mes", "Empresa", "Código Empleado", "Nombre Empleado", "Año operación", "Semana Operación", "Periodo", "Código Reporte", "Número Boleta", "Monto", "Creado por","Fecha Creación"],
        propiedades: ["codigoDepositoBTB", "anioPlanilla", "nombreMesPlanilla", "nombreEmpresa", "codigoEmpleado", "nombreEmpleado", "anioOperacion", "semanaOperacion", "periodo","codigoReporte","numeroBoleta", "monto", "usuarioIng","fechaIngStr"],
        divContenedorTabla: "divContenedorTabla",
        displaydecimals: ["monto"],
        divPintado: "divTabla",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
             {
                "targets": [11],
                "visible": true,
                "className": "dt-body-right"
            }],
        slug: "codigoDepositoBTB",
        sumarcolumna: true,
        columnasumalist: [11]
    }
    pintar(objConfiguracion);
}

function BuscarBoletasDepositosBTBEdicion() {
    let anioPlanilla = parseInt(document.getElementById("uiFiltroAnioPlanilla").value);
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnioOperacion").value);
    let semanaOperacion = parseInt(document.getElementById("uiFiltroSemanaOperacion").value);
    let codigoReporte = parseInt(document.getElementById("uiFiltroReporteCaja").value);
    MostrarBTBDepositosEdicion(anioPlanilla, anioOperacion, semanaOperacion, codigoReporte);
}

function MostrarBTBDepositosEdicion(anioPlanilla, anioOperacion, semanaOperacion, codigoReporte) {
    objConfiguracion = {
        url: "DepositosBTB/GetDepositosBTB/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString() + "&anioPlanilla=" + anioPlanilla.toString() + "&codigoReporte=" + codigoReporte.toString(),
        cabeceras: ["Código","codigoBancoDeposito","Banco","Cuenta", "Año Planilla", "Mes", "Empresa", "Código Empleado", "Nombre Empleado", "Año operación", "Semana Operación", "Periodo", "Número Boleta", "Monto", "Creado por", "Fecha Creación"],
        propiedades: ["codigoDepositoBTB", "codigoBancoDeposito","bancoDeposito", "numeroCuenta", "anioPlanilla", "nombreMesPlanilla", "nombreEmpresa", "codigoEmpleado", "nombreEmpleado", "anioOperacion", "semanaOperacion", "periodo", "numeroBoleta", "monto", "usuarioIng", "fechaIngStr"],
        divContenedorTabla: "divContenedorTabla",
        displaydecimals: ["monto"],
        divPintado: "divTabla",
        editar: true,
        funcioneditar: "DepositoBTB",
        eliminar: true,
        funcioneliminar: "DepositoBTB",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [1],
                "visible": false
            }, {
                "targets": [13],
                "visible": true,
                "className": "dt-body-right"
            }],
        slug: "codigoDepositoBTB",
        sumarcolumna: true,
        columnasumalist: [10]
    }
    pintar(objConfiguracion);
}

function EditarDepositoBTB(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-editar', function () {
        let rowIdx = table.row(this).index();
        let codigoDepositoBTB = obj;
        let codigoBancoDeposito = table.cell(rowIdx, 1).data();
        let numeroCuenta = table.cell(rowIdx, 3).data();
        let codigoEmpleado = table.cell(rowIdx, 7).data();
        let nombreEmpleado = table.cell(rowIdx, 8).data();
        let numeroBoleta = table.cell(rowIdx, 12).data();
        
        let monto = table.cell(rowIdx, 13).data();

        setI("uiTitlePopupEditDepositosBTB", "Edición de Depósitos BTB");
        document.getElementById("ShowPopupEditDepositoBTB").click();
        document.getElementById("uiEditCodigoBancoDeposito").value = codigoBancoDeposito;
        set("uiEditCodigoDepositosBTB", codigoDepositoBTB);
        set("uiEditCodigoEmpleado", codigoEmpleado);
        set("uiEditNombreEmpleado", nombreEmpleado);
        set("uiEditNumeroBoleta", numeroBoleta);
        set("uiEditMonto", monto);
        fetchGet("CuentaBancaria/GetCuentasBancariasTesoreria/?codigoBanco=" + codigoBancoDeposito, "json", function (rpta) {
            if (rpta.length > 0) {
                FillCombo(rpta, "uiEditNumeroCuenta", "numeroCuenta", "numeroCuentaDescriptivo", "- seleccione cuenta -", "-1");
                document.getElementById("uiEditNumeroCuenta").value = numeroCuenta;
            } else {
                FillComboUnicaOpcion("uiEditNumeroCuenta", "-1", "-- No existe cuenta -- ");
            }
        })
    });

}


function ActualizarDepositoBTB() {
    let errores = ValidarDatos("frmEditDepositosBTB")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let frmGuardar = document.getElementById("frmEditDepositosBTB");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("DepositosBTB/ActualizarDeposito", "text", frm, function (data) {
            if (data == "OK") {
                document.getElementById("uiClosePopupEditDepositosBTB").click();
                BuscarBoletasDepositosBTBEdicion();
            } else {
                MensajeError(data);
            }
        });
    });
}


function EliminarDepositoBTB(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', '.option-eliminar', function () {
        let rowIdx = table.row(this).index();
        let codigoDepositoBTB = parseInt(table.cell(rowIdx, 0).data());
        Confirmacion(undefined, undefined, function (rpta) {
            fetchGet("DepositosBTB/AnularDeposito/?codigoDepositoBTB=" + codigoDepositoBTB, "text", function (data) {
                if (data == "OK") {
                    BuscarBoletasDepositosBTBEdicion();
                } else {
                    MensajeError(data);
                }
            });
        });
    });
}

/*function MostrarFechaOperacionReporteCxC() {
    fetchGet("PlanillaTemporal/GetFechaReporteCxCPagoBTBYDescuento", "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            alert("No existe datos");
        } else {
            document.getElementById("uiAnioOperacionReporteCxC").value = rpta.anioOperacion;
            document.getElementById("uiSemanaOperacionReporteCxC").value = rpta.semanaOperacion;
            document.getElementById("uiDescripcionReporteCxC").innerText = "El pago BTB será incluido en el reporte de Cuentas por Cobrar año " + rpta.anioOperacion + " y semana " + rpta.semanaOperacion;
        }
    })
}*/