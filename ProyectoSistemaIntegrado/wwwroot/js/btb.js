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
                    MostrarBTBDepositosEdicion(-1, -1, -1, -1);
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

//function fillCuentasBancariasDataTable(codigoBanco) {
//    let valor = parseInt(obj.value);
//    fetchGet("CuentaBancaria/GetCuentasBancariasTesoreria/?codigoBanco=" + valor.toString(), "json", function (rpta) {
//        if (rpta.length > 0) {
//            //FillCombo(rpta, "uiNumeroCuenta", "numeroCuenta", "numeroCuentaDescriptivo", "- seleccione cuenta -", "-1");
//        } else {
//            //FillComboUnicaOpcion("uiNumeroCuenta", "-1", "-- No existe cuenta -- ");
//        }
//    })
//}



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
        propiedades: ["codigoEmpresa", "nombreEmpresa", "codigoEmpleado", "nombreCompleto", "codigoOperacion", "operacion", "codigoFrecuenciaPago", "frecuenciaPago", "tipoBTB", "bonoDecreto372001", "salarioDiario", "existePagoBTB", "montoDevolucionBTB"],
        divContenedorTabla: "divContenedorTabla",
        displaydecimals: ["bonoDecreto372001","salarioDiario", "montoDevolucionBTB"],
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
                "validate": "decimal-2",
                "onclick": true,
                "nameclick": "clickMontoPorDevolver"
            }, {
                "header": "Monto Descuento",
                "value": "montoDescuento",
                "name": "MontoDescuento",
                "align": "text-right",
                "validate": "decimal-2",
                "onclick": true,
                "nameclick": "clickMontoPorDevolver"
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
                "targets": [9],
                "visible": true,
                "className": "dt-body-right"
            }, {
                "targets": [10],
                "visible": true,
                "className": "dt-body-right"
            }, {
                "targets": [11],
                "visible": false
            }, {
                "targets": [12],
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

// Cambiar para bloquer los reportes ya finalizados
function FillFechasDeCorte(anioOperacion, semanaOperacion) {
    //fetchGet("CorteCajaSemanal/GetReportesCajaEnProceso/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString(), "json", function (rpta) {
    fetchGet("CorteCajaSemanal/GetReportesCajaConsulta/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString(), "json", function (rpta) {
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

//sumarcolumna: true,
//columnasumalist: [14, 17]
function MostrarEmpleadosBackToBackDepositos(codigoTipoPlanilla, anioPlanilla, mesPlanilla) {
    objConfiguracion = {
        url: "PlanillaTemporal/GetEmpleadosBackToBackBoletaDeposito/?codigoTipoPlanilla=" + codigoTipoPlanilla.toString() + "&anioPlanilla=" + anioPlanilla.toString() + "&mesPlanilla=" + mesPlanilla.toString(),
        cabeceras: ["Código Empresa", "Empresa", "Código Empleado", "Nombre Empleado", "Código Operación", "Operación", "Codigo Frecuencia Pago", "Frecuencia de Pago", "Tipo BTB", "Bono Decreto 37-2001", "Salario Diario", "Banco", "Cuenta", "Día", "Monto Depósito", "habilitarCheck"],
        propiedades: ["codigoEmpresa", "nombreEmpresa", "codigoEmpleado", "nombreCompleto", "codigoOperacion", "operacion", "codigoFrecuenciaPago", "frecuenciaPago", "codigoTipoBTB", "bonoDecreto372001", "salarioDiario", "comboBancos", "comboCuentas", "comboDias", "montoDevolucionBTB","habilitarCheck"],
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
                "validate": "solonumeros",
                "onclick" : false
            }, {
                "header": "Monto Depósito",
                "value": "montoDevolucionBTB",
                "name": "MontoDevolucionBTB",
                "align": "text-right",
                "validate": "decimal-2",
                "onclick": false
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
                "visible": false,
                "className": "dt-body-right"
            }, {
                "targets": [15],
                "visible": false
            }],
        classColumn: [
            {
                "propiedadname": "comboBancos",
                "classname": "option-banco",
            }],
        slug: "codigoEmpresa",
        excluir: true,
        fieldNameExcluir: "habilitarCheck"
    }
    pintar(objConfiguracion);
}



//$('#tabla tbody').on('click', '.option-editar', function () {
function FillComboCuentasBancarias(obj) {
    let opcionInicio = "<select name=NumeroCuenta id=uiNumeroCuenta class=select-cuenta-bancaria><option value=-1>-- Error--</option></select>";
    let table = $('#tabla').DataTable();
    //$("#tabla td").on('click', '.option-banco', function () {
    $("#tabla tbody").on('click','.option-banco', function () {
        //let $tdCurrent = $(this).parent();
        let $tdNumeroCuenta = $(this).closest('td').next('td');
        table.cell($tdNumeroCuenta).data(opcionInicio);
        fetchGet("CuentaBancaria/GetComboCuentasBancarias/?codigoBanco=" + obj.value.toString(), "text", function (data) {
            if (data == null ||  data == undefined || data == "") {
                data = "<select name=NumeroCuenta id=uiNumeroCuenta class=select-cuenta-bancaria><option value=-1>-- Error--</option></select>";
                table.cell($tdNumeroCuenta).data(data);
            } else {
                table.cell($tdNumeroCuenta).data(data);
            }
        })
    });
}

function Excluir(obj) {


}

function Excluir2(obj) {
    let table = $('#tabla').DataTable();
    $("#tabla").on('click', 'tr', function () {
        //var $td = $(this).parent();
        var data = table.row(this).data();
        if (typeof data != 'undefined') {
            if ($(this).find('.option-check').prop('checked')) {  //update the cell data with the checkbox state
                data.check = 1;
            } else {
                data.check = 0;
            }
        }
        //table.cell($td).invalidate().draw();
        //var $td = $(this).parent();
        //$td.find('input').attr('value', this.value);
        //table.cell($td).invalidate().draw();
        /*$td.find('checkbox').each(function (i) {
            let isSelected = $(this).is(':checked');
            if (isSelected) {
                $(this).prop("checked", false);
            } else {
                $(this).prop("checked", true);
            }
        });**/
    });
    
}

function Excluir1(obj) {
    //alert(obj);
    let table = $('#tabla').DataTable();
    //$("#tabla").on('click', function () {
    $("#tabla tr td input[type='checkbox']").on('click', function () {
        //var $td = $(this).children().find("td").prop("id");
        //let id = $td.find('checkbox').attr('id');
        let id = $(this).attr("id");
        //$('#' + id).checked = obj.checked == true ? true : false;
        //let jj = $('#' + id); //.attr('checked', obj.checked == true ? false : true);

        //$('#' + id).attr('checked', false);

        //let rows = table.rows().nodes;
        $('#' + id).prop('checked');
        //$('input[type="checkbox"]', rows).prop('checked', obj.checked == true ? false : true);
        //let test = $('input[id="' + id + '"]').prop('checked', obj.checked == true ? false : true);
        //let test = $('input[id="' + id + '"]').prop('checked');
        //$td.find('input')('input:checkbox').prop("checked", obj.checked == true ? false : true);

        //alert($('#' + id).checked);
        //let id = $(this).parent().attr("id");
        
        //console.log("test");
        //let rowIdx = table.row(this).index();
        //let rows = table.cell(rowIdx, 18).node();
        //let element = table.cell(rowIdx, 18).data();

        //let id = table.cell(rowIdx, 18).attr("id");


        //var tr = $(this).closest('td');

        //var $td = $(this).parent();
        //$td.find('input').attr('value', this.value);
        //table.cell($td).invalidate().draw();

        //var $td = $(this).parent();
        //table.cell($td).invalidate().draw();
        //let rows = $(this);
        //var tr = $(this).closest('tr');
        //var rows = table.row(this).node;
        //let rows = table.rows().nodes()[0];


        

        //let rows = table.rows({ selected: true }).nodes();

        //let rows = table.row($td).node();


        

        //alert(table.row(this).data());
        //table.clear().draw();
        //table.draw();

        //var $td = $(this).parent();
        //table.cell($td).invalidate().draw();


        //$td.find('input')('input:checkbox').prop("checked", obj.checked == true ? false : true);
        //$td.find('input').prop('checked')
        // desabilita el checkbox
        //$td.find('input').prop("disabled", true);

        //$td.find('input').prop("checked", true);


        //$td.find('input').Attr('Checked');
        //$td.find('input').removeAttr('Checked');

        //checked


        //$td.find('input').prop("checked", obj.checked == true ? true : false);
        //$(this.node()).find('input:checkbox').prop("checked", obj.checked == true ? false : true);
        //var $td = $(this).parent();
        //$td.find('input').attr('Checked', obj.checked == true ? false : true);
        //$(this).change();
        
        //$td.find('input').removeAttr('Checked');
    });

}

function selectAllCheckBox(obj, elemento) {
    let table = $('#tabla').DataTable();
    $('#tabla').on('click', function () {
        // Get all rows with search applied
        let rows = table.rows().nodes();
        // Check/uncheck checkboxes for all rows in the table
        $('input[type="checkbox"]', rows).prop('checked', elemento.checked);
    });

    // Listen for click on toggle checkbox
/*    $('#' + obj).click(function (event) {
        if (this.checked) {
            // Iterate each checkbox
            $('.row-excluir:checkbox').each(function () {
                this.checked = false;
            });
        } else {
            $('.row-excluir:checkbox').each(function () {
                this.checked = true;
            });
        }*/



        //$('.row-excluir:checkbox').each(function () {
        //    this.removeAttr('Checked');
        //});

    //});

    /*$(document).ready(function () {
        $('input[name="all"],input[name="title"]').bind('click', function () {
            var status = $(this).is(':checked');
            $('input[type="checkbox"]', $(this).parent('li')).attr('checked', status);
        });
    });*/

    /*$('#' + obj).click(function (event) {
        var $td = $(this).parent();
        //$td.find('input').attr('value', this.value);
        $td.find('input').attr('value', this.value);
        table.cell($td).invalidate().draw();

        var checkBoxes = $("input[name=check]");
        checkBoxes.prop("checked", !checkBoxes.prop("checked"));
    });*/

    

    /*let status = false;
    $('#' + obj).click(function (event) {
        $('.row-excluir:checkbox').each(function () {
            status = $(this).is(':checked');
            this.checked = status;
            //$('input[type="checkbox"]', $(this).parent('td')).attr('checked', status);
        });
    });*/

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

    let codigoFrecuenciaPago = 1; // 1. Mensual
    let codigoOperacion = 20; // 20. Depósito Bancario
    let table = $('#tabla').DataTable();
    var data = table.rows().data();
    let arrayProperties = new Array();
    let obj = null;
    let numeroBoleta = "";
    let monto = "";
    let codigoBanco = "";
    let numeroCuenta = "";
    let diaOperacion = "";
    let boletasCompletas = true;
    let excluir = 0;
    data.each(function (value, index) {
        excluir = table.cell(index, 18).nodes().to$().find('input').prop('checked') == true ? 1 : 0;
        if (excluir == 0) {
            numeroBoleta = table.cell(index, 16).nodes().to$().find('input').val();
            monto = table.cell(index, 17).nodes().to$().find('input').val();
            codigoBanco = table.cell(index, 11).nodes().to$().find('option:selected').val();
            numeroCuenta = table.cell(index, 12).nodes().to$().find('option:selected').val();
            diaOperacion = table.cell(index, 13).nodes().to$().find('option:selected').val();
            if (/^[0-9]+$/.test(numeroBoleta) && (/^\d*(\.\d{1})?\d{0,1}$/.test(monto)) && codigoBanco != "-1" && numeroCuenta != "-1" && diaOperacion != "-1")
            {
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
                    CodigoBancoDeposito: table.cell(index, 11).nodes().to$().find('option:selected').val(),
                    NumeroCuenta: table.cell(index, 12).nodes().to$().find('option:selected').val(),
                    DiaOperacion: table.cell(index, 13).nodes().to$().find('option:selected').val(),
                    NumeroBoleta: numeroBoleta,
                    Monto: table.cell(index, 17).nodes().to$().find('input').val()
                };
                arrayProperties.push(obj);
            } else {
                boletasCompletas = false;
            }
        }
    });
    if (Array.isArray(arrayProperties) && arrayProperties.length > 0) {
        let jsonData = JSON.stringify(arrayProperties);
        Confirmacion(undefined, undefined, function (rpta) {
            fetchPostJson("DepositosBTB/GuardarDepositosBTB", "text", jsonData, function (data) {
                if (data == "OK") {
                    if (boletasCompletas == true) {
                        Exito("DepositosBTB", "RegistroDepositosBTB", true, "Boletas seleccionadas se registraron completamente")
                    } else {
                        Exito("DepositosBTB", "RegistroDepositosBTB", true, "Algunas boletas seleccionadas no se registraron")
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
                "visible": true
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
        fetchGet("Banco/GetAllBancos", "json", function (rpta) {
            if (rpta.length > 0) {
                FillCombo(rpta, "uiEditCodigoBancoDeposito", "codigoBanco", "nombre", "- seleccione -", "-1");
                document.getElementById("uiEditCodigoBancoDeposito").value = codigoBancoDeposito;
                fetchGet("CuentaBancaria/GetCuentasBancarias/?codigoBanco=" + codigoBancoDeposito, "json", function (rpta1) {
                    if (rpta1.length > 0) {
                        FillComboAddFirstOption(rpta1, "uiEditNumeroCuenta", "numeroCuenta", "numeroCuenta", "- seleccione cuenta -", "-1");
                        document.getElementById("uiEditNumeroCuenta").value = numeroCuenta;
                    } else {
                        FillComboUnicaOpcion("uiEditNumeroCuenta", "-1", "-- No existe cuenta -- ");
                    }
                })
            } else {
                FillComboUnicaOpcion("uiEditCodigoBancoDeposito", "-1", "-- No existe cuenta -- ");
            }
            set("uiEditCodigoDepositosBTB", codigoDepositoBTB);
            set("uiEditCodigoEmpleado", codigoEmpleado);
            set("uiEditNombreEmpleado", nombreEmpleado);
            set("uiEditNumeroBoleta", numeroBoleta);
            set("uiEditMonto", monto);
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