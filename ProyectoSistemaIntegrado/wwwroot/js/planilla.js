window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    let arrayDate = getFechaSistema();
    let anio = arrayDate[0]["anio"];
    if (nameController == "PlanillaTemporal") {
        switch (nameAction) {
            case "PagosDescuentos":
                //MostrarFechaOperacionReporteCxC();
                MostrarCuentasPorCobrarPlanilla();
                break;
            case "ConsultaPagosDescuentos":
                FillComboEmpresa();
                set("uiFiltroAnio", anio.toString());
                MostrarCuentasPorCobrarEnTesoreriaConsulta(anio, -1, -1);
                break;
            case "ConsultaDevolucionesBTB":
                FillComboEmpresa();
                set("uiFiltroAnio", anio.toString());
                MostrarDevolucionesBTBEnTesoreriaConsulta(anio, -1, -1);
                break;
            default:
                break;
        }// fin switch
    }// fin if
    else {
        if (nameController == "ReportesTesoreria") {
            switch (nameAction) {
                case "DesglocePagoPlanillas":
                    FillAnioPlanilla();
                    FillComboUnicaOpcion("uiFiltroSemana", "-1", "-- No existen semanas -- ");
                    FillComboUnicaOpcion("uiFiltroDiaOperacion", "-1", "-- No existe días -- ");
                    FillComboUnicaOpcion("uiFiltroReporteCaja", "-1", "-- No existe reportes -- ");
                    MostrarTransacciones();
                    break;
                default:

                    break;
            }
        } else {
            if (nameController == "Planilla") {
                switch (nameAction) {
                    case "ListEmpleados":
                        fillCombosFiltroEmpleados();
                        mostrarEmpleados(-1, -1, -1, -1, 0, 0);
                        break;
                    case "EditEmpleado":
                        let codigoEmpleado = url1.searchParams.get("codigoEmpleado");
                        fillCombosEditEmpleado(codigoEmpleado);
                        break;
                    default:
                        break;
                }
            }

        }// fin else
    }// fin else
}

/*
 * Informacion de Empleados para la Planilla
 */

function clickAmpliarFoto() {
    $("#pop").on("click", function () {
        $('#imagepreview').attr('src', $('#uiEditFoto').attr('src')); // here asign the image to the modal when the user click the enlarge link
        $('#imagepreview').css('transform', 'scale(0.9)');
        $('#imagemodal').modal('show'); // imagemodal is the id attribute assigned to the bootstrap modal, then i use the show function
    });
}

function fillCombosFiltroEmpleados() {
    fetchGet("Empleado/GetListFillCombosConsulta", "json", function (rpta) {
        var listaEmpresas = rpta.listaEmpresas;
        let listaAreas = rpta.listaAreas;
        let listaPuestos = rpta.listaPuestos;
        let listaEstadosEmpleado = rpta.listaEstadosEmpleado;
        FillCombo(listaEmpresas, "uiFiltroEmpresa", "codigoEmpresa", "nombreComercial", "- Todos -", "-1");
        FillCombo(listaAreas, "uiFiltroArea", "codigoArea", "nombre", "- Todos -", "-1");
        FillCombo(listaPuestos, "uiFiltroPuesto", "codigoPuesto", "nombre", "- Todos -", "-1");
        FillCombo(listaEstadosEmpleado, "uiFiltroEstados", "codigoEstadoEmpleado", "estadoEmpleado", "- Todos -", "-1");
    })
}

function mostrarEmpleados(codigoEmpresa, codigoArea, codigoPuesto, codigoEstado, btb, saldoPrestamo) {
    objConfiguracion = {
        url: "Empleado/BuscarEmpleados/?codigoEmpresa=" + codigoEmpresa.toString() + "&codigoArea=" + codigoArea.toString() + "&codigoPuesto=" + codigoPuesto.toString() + "&codigoEstado=" + codigoEstado.toString() + "&btb=" + btb.toString() + "&saldoPrestamo=" + saldoPrestamo.toString(),
        cabeceras: ["Código Empresa", "Empresa", "Código Empleado", "Nombre", "CUI", "Area", "Sección", "Puesto", "Ubicación", "Jornada","Fecha Ingreso","Fecha Egreso", "Estado", "Editar", "Anular"],
        propiedades: ["codigoEmpresa", "empresa", "codigoEmpleado", "nombreCompleto", "cui", "area", "seccion", "puesto", "ubicacion", "jornada","fechaIngresoStr","fechaEgresoStr", "estadoEmpleado", "permisoEditar", "permisoAnular"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        editar: true,
        funcioneditar: "Empleado",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "visible": false
            }, {
                "targets": [13],
                "visible": false
            }, {
                "targets": [14],
                "visible": false
            }],
        slug: "codigoEmpleado"
    }
    pintar(objConfiguracion);
}

function buscarEmpleados() {
    let objEmpresa = document.getElementById("uiFiltroEmpresa");
    let objArea = document.getElementById("uiFiltroArea");
    let objPuesto = document.getElementById("uiFiltroPuesto");
    let objEstado = document.getElementById("uiFiltroEstados");
    let codigoEmpresa = parseInt(objEmpresa.options[objEmpresa.selectedIndex].value);
    let codigoArea = parseInt(objArea.options[objArea.selectedIndex].value);
    let codigoPuesto = parseInt(objPuesto.options[objPuesto.selectedIndex].value);
    let codigoEstado = parseInt(objEstado.options[objEstado.selectedIndex].value);
    let checkBackToBack = document.getElementById("uiBackToBack");
    let checkSaldoPrestamo = document.getElementById("uiSaldoPrestamo");
    let btb = checkBackToBack.checked ? 1 : 0;
    let saldoPrestamo = checkSaldoPrestamo.checked ? 1 : 0;
    mostrarEmpleados(codigoEmpresa, codigoArea, codigoPuesto, codigoEstado, btb, saldoPrestamo);
}

function EditarEmpleado(obj) {
    Redireccionar("Planilla", "EditEmpleado/?codigoEmpleado=" + obj);
}

function fillCombosEditEmpleado(codigoEmpleado) {
    fetchGet("Empleado/FillCombosNewEmpleado", "json", function (rpta) {
        let listaAreas = rpta.listaAreas;
        let listaSecciones = rpta.listaSecciones;
        let listaPuestos = rpta.listaPuestos;
        let listaTipoBTB = rpta.listaTiposBackToBack;
        FillCombo(listaAreas, "cboArea", "codigoArea", "nombre", "- seleccione -", "-1");
        FillCombo(listaSecciones, "cboSeccion", "codigoSeccion", "nombre", "- seleccione -", "0");
        FillCombo(listaPuestos, "cboPuesto", "codigoPuesto", "nombre", "- seleccione -", "-1");
        FillRadioButtonListTipoBTB(listaTipoBTB);
        MostrarDataEmpleado(codigoEmpleado)
    })
}

function MostrarDataEmpleado(codigoEmpleado) {
    fetchGet("Empleado/GetDataEmpleado/?codigoEmpleado=" + codigoEmpleado, "json", function (rpta) {
        if (rpta == null || rpta == undefined) {
            alert("vacio");
        } else {
            set("uiCodigoEmpresa", rpta.codigoEmpresa);
            set("uiEmpresa", rpta.empresa);
            set("uiCodigoEmpleado", rpta.codigoEmpleado);
            set("uiCui", rpta.cui);
            set("uiPrimerNombre", rpta.primerNombre);
            set("uiSegundoNombre", rpta.segundoNombre);
            set("uiTercerNombre", rpta.tercerNombre);
            set("uiPrimerApellido", rpta.primerApellido);
            set("uiSegundoApellido", rpta.segundoApellido);
            set("uiApellidoCasada", rpta.apellidoCasada);
            set("uiFechaNacimiento", rpta.fechaNacimientoStr);
            document.querySelector('#uiGenero').value = rpta.codigoGenero;
            set("uiCorreoElectronico", rpta.correoElectronico);
            set("uiNumeroAfiliacion", rpta.numeroAfiliacion);

            document.querySelector('#cboArea').value = rpta.codigoArea;
            document.querySelector('#cboSeccion').value = rpta.codigoSeccion;
            document.querySelector('#cboPuesto').value = rpta.codigoPuesto;
            document.querySelector('#cboUbicacion').value = rpta.codigoUbicacion;

            setCheckedValueOfRadioButtonGroup('CodigoTipoCuenta', rpta.codigoTipoCuenta);
            set("uiNumeroCuenta", rpta.numeroCuenta);
            set("uiMontoDevengado", rpta.montoDevengado);
            setCheckedValueOfRadioButtonGroup('CodigoJornada', rpta.codigoJornada);
            setCheckedValueOfRadioButtonGroup('CodigoFrecuenciaPago', rpta.codigoFrecuenciaPago);
            document.querySelector('#uiIGSS').checked = rpta.igss == 1 ? true : false;
            document.querySelector('#uiBonoDeLey').checked = rpta.bonoDeLey == 1 ? true : false;
            document.querySelector('#uiRetencionISR').checked = rpta.retencionIsr == 1 ? true : false;
            document.querySelector('#uiEmpleadoExterno').checked = rpta.empleadoExterno == 1 ? true : false;
            setCheckedValueOfRadioButtonGroup('CodigoTipoBackToBack', rpta.codigoTipoBackToBack);
            set("uiSalarioDiario", rpta.salarioDiario);
            set("uiBonoDecreto372001", rpta.bonoDecreto372001);
            set("uiFechaIngreso", rpta.fechaIngresoStr);
            set("uiFechaEgreso", rpta.fechaEgresoStr);

            /* Visualizar la Foto del Empleado */
            let img = document.getElementById("uiEditFoto");
            let base64 = rpta.foto;
            if (base64 == "") {
                if (rpta.codigoGenero == "F") {
                    base64 = "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W2+9Fffee2/8/Oc/j+23394nc5KkMv96ZHqe6TnnnJN9O2TQoEHZN0W+euC2+YCPzwLnAvPxfcffsmXLsoMzLXSvv/56/O53v4sDDjggGjZsaNiQJG1wjRo1ih49esRNN92UnTNpYZs4cWJ29sybN898wMdngXOB+fg2xDdr1qzsmTpffPFFdtCmu1qmH5I3a9YsatasGVWrVs2+9uLulpKkr57Nls6FatWqRa1ataJFixZx4IEHxjXXXBODBw/OzpPp06d/40O2nb98fBY4F5iPrwx8ixYtyg7iPn36xAknnJB9BaZNmzax6aabZrd5Tg9WNchIUnE8SDu976f3/3QOpGezpYdq9+/fP95//33nLx+fBc4F5uPLRV/6l9T0Y/O//OUvcfXVV8dPf/rT2GeffbLFbsstt4ymTZtmn9j5XZ0k5W9pWWvQoEF206sdd9wxe58/6aSTspthPf300/H555/HihUrnL98fBY4F5iPLx996fcMH374YXao33rrrXHZZZfFaaedFkcccUR069YtOnToEK1atcr+5dbdLyUpdyopKckWtfTc0F122SX7LfRxxx0Xv/rVr+L3v/99PPHEE/HBBx9k7/XpZljOXz4+C5wXkI+vQH3pX2ZnzJiRfVr38ssvxyOPPBL9+vWLK6+8MhsMTjzxxOz5dLvuumv2DKD0W7v0Gwqf3ElS6d+2P91QZIsttogf/vCH0b179zj22GPjrLPOyt6T77zzzvjrX/8aQ4cOjTFjxsSCBQti9erVzjc+PgucF5CPjy9i1apVsXDhwpgyZUr2fLq33347Xnrppew5QA8//HDcfffdccMNN8Sll14aZ555ZvYvwfvtt1/2Fc30L8RpCKlcubKhTJLfpVWokH2zoWXLlrHttttGly5dsuUsPV8tfQvi4osvzt5P77nnnnjggQfiqaeeyh4jk/5hLd0NMr1fr+ui5nzj47PAeQH5+PjW+peGiaVLl8b8+fOzT/DSXcyGDRuW/ctwem7QgAEDsiHkySefzJ4llBa+G2+8Ma666qpsWEn/opx+l3fkkUdmXwXabbfdsq9wfnXjlfQbvfR1ofRpX/qtnrtqSiqr556l95mvV79+/ew9KL0Xpd+WpU/E0m+H02/M0g1BOnfuHF27ds3eu9J7WLpx1Omnnx7nn39+XH755dnjXm6++eb4wx/+EI8++mgM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zz6LwJGPXYJFAkUas7NbWBYYkjpGEQV2LmjrcNppp0VwyC4hqaLsbrBTuP/++0epcj7wCRbPOuus+N92fovHR2EadrUI/ClbT/DfuHHj+PekITZv3jzafbBIQIXE9u3bR+DVvXv31KtXr9SvX78oynHZZZel6667Ls5Jjh07Nn5v7IAReLFwQeBPiuLcuXNjQYMgbP78+dFf8uOPP47fFQEZv2N+0/y2nV99+vTpM4DTp0+fPn369OnTp0+fPgM4L6A+ffr06dOnT58+ffr0GcA5wfr06dOnT58+ffr06dNnAKdPnz59+vTp06dPnz59BnBeQH369OnTp0+fPn369OkzgHOC9enTp0+fPn369OnTp88ATp8+ffr06dOnT58+ffoM4LyA+vTp06dPnz59+vTp02cA5wTr06dPnz59+vTp06dPX24BnBOiT58+ffr06dOnT58+fYXhM4DTp0+fPn369OnTp0+fPgM4J1ifPn369OnTp0+fPn36DOD06dOnT58+ffr06dOnzwDOC6hPnz59+vTp06dPnz59BnBOsD59+vTp06dPnz59+vQZwOnTp0+fPn369OnTp0+fAZwXUJ8+ffr06dOnT58+ffoM4Jxgffr06dOnT58+ffr06TOA06dPnz59+vTp06dPnz4DOC+gPn369OnTp0+fPn369BnAOcH69OnTp0+fPn369OnTZwCnT58+ffr06dOnT58+fQZwXkB9+vTp06dPnz59+vTpM4BzgvXp06dPnz59+vTp06fPAE6fPn369OnTp0+fPn36DOCcYH369OnTp0+fPn369OkzgNOnT58+ffr06dOnT58+fQZw+vTp06dPnz59+vTp02cA5wTr06dPnz59+vTp06dPX7YBnBOiT58+ffr06dOnT58+fYXhM4DTp0+fPn369OnTp0+fPgM4J1ifPn369OnTp0+fPn36DOD06dOnT58+ffr06dOnzwDOC6hPnz59+vTp06dPnz59BnBOsD59+vTp06dPnz59+vQZwOnTp0+fPn369OnTp0+fAZwXUJ8+ffr06dOnT58+ffoM4Jxgffr06dOnT58+ffr06TOA06dPnz59+vTp06dPnz4DOC+gPn369OnTp0+fPn369BnAOcH69OnTp0+fPn369OnTZwCnT58+ffr06dOnT58+fQZwXkB9+vTp06dPnz59+vTpM4BzgvXp06dPnz59+vTp06fPAE6fPn369OnTp0+fPn36DOCcYH369OnTp0+fPn369OkzgNOnT58+ffr06dOnT58+fQZw+vTp06dPnz59+vTp02cA5wTr06dPnz59+vTp06dPX7YBnBOiT58+ffr06dOnT58+fYXhM4DTp0+fPn369OnTp0+fPgM4J1ifPn369OnTp0+fPn36DOD06dOnT58+ffr06dOnzwDOC6hPnz59+vTp06dPnz59BnBOsD59+vTp06dPnz59+vQZwOnTp0+fPn369OnTp0+fAZwXUJ8+ffr06dOnT58+ffoM4Jxgffr06dOnT58+ffr06TOA06dPnz59+vTp06dPnz4DOC+gPn369OnTp0+fPn369BnAOcH69OnTp0+fPn369OnTZwCnT58+ffr06dOnT58+fQZwXkB9+vTp06dPnz59+vTpM4BzgvXp06dPnz59+vTp06fPAE6fPn369OnTp0+fPn36DOCcYH369OnTp0+fPn369OkzgNOnT58+ffr06dOnT58+fQZw+vTp06dPnz59+vTp02cA5wTr06dPnz59+vTp06dPX7YBnBOiT58+ffr06dOnT58+fYXhM4DTp0+fPn369OnTp0+fPgM4J1ifPn369OnTp0+fPn36DOD06dOnT58+ffr06dOnzwDOC6hPnz59+vTp06dPnz59BnBOsD59+vTp06dPnz59+vQZwOnTp0+fPn369OnTp0+fAZwXUJ8+ffr06dOnT58+ffoM4Jxgffr06dOnT58+ffr06TOA06dPnz59+vTp06dPnz4DOC+gPn369OnTp0+fPn369BnAOcH69OnTp0+fPn369OnTZwCnT58+ffr06dOnT58+fQZwXkB9+vTp06dPnz59+vTpM4BzgvXp06dPnz59+vTp06fPAE6fPn369OnTp0+fPn36DOCcYH369OnTp0+fPn369OkzgNOnT58+ffr06dOnT58+fel/AXOJGfFgwy//AAAAAElFTkSuQmCC";
                } else {
                    base64 = "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eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJYTVAgQ29yZSA0LjQuMC1FeGl2MiI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIiB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIiB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iIHhtbG5zOkdJTVA9Imh0dHA6Ly93d3cuZ2ltcC5vcmcveG1wLyIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bXBNTTpEb2N1bWVudElEPSJnaW1wOmRvY2lkOmdpbXA6YzJhMDg5YTMtYmU0ZS00NTMzLTkxZjQtMTVmOGI3OTNkMmIyIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOmQ4ZjAxNjEzLTdlN2EtNGUyMC1iZGQ1LWFkZGRmYTE0MWJhNyIgeG1wTU06T3JpZ2luYWxEb2N1bWVudElEPSJ4bXAuZGlkOjE5MTQ1N2I4LTAyYmItNDQzNy1iMWU5LTVjOWZlYjJlZTA2ZiIgZGM6Rm9ybWF0PSJpbWFnZS9qcGVnIiBHSU1QOkFQST0iMi4wIiBHSU1QOlBsYXRmb3JtPSJXaW5kb3dzIiBHSU1QOlRpbWVTdGFtcD0iMTY0Njg2MzMxODM5MTQ4MiIgR0lNUDpWZXJzaW9uPSIyLjEwLjI4IiB4bXA6Q3JlYXRvclRvb2w9IkdJTVAgMi4xMCI+IDx4bXBNTTpIaXN0b3J5PiA8cmRmOlNlcT4gPHJkZjpsaSBzdEV2dDphY3Rpb249InNhdmVkIiBzdEV2dDpjaGFuZ2VkPSIvIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOmI3NGI5Mjk5LTQ3OGMtNDhkYi04NGRkLTdlNDBjN2ViMjBlMyIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iR2ltcCAyLjEwIChXaW5kb3dzKSIgc3RFdnQ6d2hlbj0iMjAyMi0wMy0wOVQxNjowMTo1OCIvPiA8L3JkZjpTZXE+IDwveG1wTU06SGlzdG9yeT4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPD94cGFja2V0IGVuZD0idyI/Pv/iArBJQ0NfUFJPRklMRQABAQAAAqBsY21zBDAAAG1udHJSR0IgWFlaIAfmAAMACQAOACIANWFjc3BNU0ZUAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD21gABAAAAANMtbGNtcwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADWRlc2MAAAEgAAAAQGNwcnQAAAFgAAAANnd0cHQAAAGYAAAAFGNoYWQAAAGsAAAALHJYWVoAAAHYAAAAFGJYWVoAAAHsAAAAFGdYWVoAAAIAAAAAFHJUUkMAAAIUAAAAIGdUUkMAAAIUAAAAIGJUUkMAAAIUAAAAIGNocm0AAAI0AAAAJGRtbmQAAAJYAAAAJGRtZGQAAAJ8AAAAJG1sdWMAAAAAAAAAAQAAAAxlblVTAAAAJAAAABwARwBJAE0AUAAgAGIAdQBpAGwAdAAtAGkAbgAgAHMAUgBHAEJtbHVjAAAAAAAAAAEAAAAMZW5VUwAAABoAAAAcAFAAdQBiAGwAaQBjACAARABvAG0AYQBpAG4AAFhZWiAAAAAAAAD21gABAAAAANMtc2YzMgAAAAAAAQxCAAAF3v//8yUAAAeTAAD9kP//+6H///2iAAAD3AAAwG5YWVogAAAA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";
                }
            }

            let buffer = Uint8Array.from(atob(base64), c => c.charCodeAt(0));
            let blob = new Blob([buffer], { type: "image/jpg" });
            let url = URL.createObjectURL(blob);
            img.src = url;
        }
    });
}

function FillRadioButtonListTipoBTB(res) {
    let objConfiguracion = {
        propiedadnombre: "CodigoTipoBackToBack",
        propiedades: ["codigoTipoBackToBack", "nombre"],
        propiedadid: "uiCodigoTipoBTB",
        divContenedorTabla: "divContainerTablaRadioButtonListBTB",
        divPintado: "divTablaRadioButtonListTipoBTB"
    }
    pintarRadioButtonListWithoutLabel(objConfiguracion, res);
}


function ActualizarEmpleadoPlanilla() {
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
        fetchPost("Empleado/ActualizarEmpleadoPlanilla", "text", frm, function (data) {
            if (data == "OK") {
                Exito("Planilla", "ListEmpleados", true);
            } else {
                MensajeError(data);
            }
        })
    })
}

function setFiltroDesglocePagoPlanillas()
{
    FillAnioPlanilla();
    FillComboUnicaOpcion("uiFiltroSemana", "-1", "-- No existen semanas -- ");
    FillComboUnicaOpcion("uiFiltroDiaOperacion", "-1", "-- No existe días -- ");
    let selectArea = document.getElementById("uiFiltroArea");
    selectArea.selectedIndex = 0;
    set("uiMontoDevengado", "");
    MostrarTransacciones();
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
        //let data = [{ "value": anioActual.toString(), "text": anioActual.toString() }]
        let data = [{ "value": "-1", "text": "- seleccione -" }, { "value": anioActual.toString(), "text": anioActual.toString() }]
        FillComboSelectOption(data, "uiFiltroAnioPlanilla", "value", "text", "- seleccione -", "-1");
        select.selectedIndex = 0;
    }
}


/* Revisar si se esta usando */
function fillComboProgramacion(obj) {
    let codigoFrecuenciaPago = parseInt(obj.value);
    let anio = parseInt(document.getElementById("uiFiltroAnioPlanilla").value);
    let numeroMes = parseInt(document.getElementById("uiFiltroMes").value);
    let numeroSemana = 1;
    let elementSemanaOrQuincena = document.getElementById("uiCodigoGenerico");
    let elementMeses = document.getElementById("uiFiltroMes");
    elementMeses.selectedIndex = 0;
    switch (codigoFrecuenciaPago) {
        case TIPO_PLANILLA_MENSUAL:
            FillComboUnicaOpcion("uiCodigoGenerico", "-1", "-- No Aplica -- ");
            elementSemanaOrQuincena.classList.remove('obligatorio');
            elementSemanaOrQuincena.classList.add('disabled')
            elementMeses.classList.add('obligatorio');
            break;
        case TIPO_PLANILLA_QUINCENAL:
            elementSemanaOrQuincena.classList.add('obligatorio');
            elementSemanaOrQuincena.classList.remove('disabled')
            elementMeses.classList.add('obligatorio');
            fetchGet("ProgramacionQuincenal/GetListaQuincenas/?anio=" + anio.toString() + "&numeroMes=" + numeroMes.toString(), "json", function (rpta) {
                if (rpta != undefined && rpta != null && rpta.length != 0) {
                    FillCombo(rpta, "uiCodigoGenerico", "codigoQuincenaPlanilla", "periodo", "- seleccione -", "-1");
                } else {
                    FillComboUnicaOpcion("uiCodigoGenerico", "-1", "-- No Existen Fechas -- ");
                }
            })
            break;
        case TIPO_PLANILLA_SEMANAL:
            elementSemanaOrQuincena.classList.add('obligatorio');
            elementSemanaOrQuincena.classList.add('disabled')
            elementMeses.classList.remove('obligatorio');
            fetchGet("ProgramacionSemanal/GetListaSemanasPlanilla/?anio=" + anio.toString() + "&numeroSemana=" + numeroSemana.toString(), "json", function (rpta) {
                if (rpta != undefined && rpta != null && rpta.length != 0) {
                    FillCombo(rpta, "uiCodigoGenerico", "numeroSemana", "periodo", "- seleccione -", "-1");
                } else {
                    FillComboUnicaOpcion("uiCodigoGenerico", "0", "-- No Existen Fechas -- ");
                }
            })
            break;
    }
}


/* Revisar si se esta usando */
function fillComboProgramacionQuincenal(obj) {
    let codigoFrecuenciaPago = parseInt(document.getElementById("uiFiltroFrecuenciaPago").value);
    let anio = parseInt(document.getElementById("uiFiltroAnioPlanilla").value);
    let numeroMes = parseInt(obj.value);
    if (codigoFrecuenciaPago == TIPO_PLANILLA_QUINCENAL) {
        fetchGet("ProgramacionQuincenal/GetListaQuincenas/?anio=" + anio.toString() + "&numeroMes=" + numeroMes.toString(), "json", function (rpta) {
            if (rpta != undefined && rpta != null && rpta.length != 0) {
                FillCombo(rpta, "uiCodigoGenerico", "codigoQuincenaPlanilla", "periodo", "- seleccione -", "-1");
            } else {
                FillComboUnicaOpcion("uiCodigoGenerico", "-1", "-- No Existen Fechas -- ");
            }
        })
    }
}


function FillComboEmpresa() {
    fetchGet("Empresa/GetAllEmpresas", "json", function (rpta) {
        FillCombo(rpta, "uiFiltroEmpresa", "codigoEmpresa", "nombreComercial", "- seleccione -", "-1");
    })
}

function FillComboDiasOperacion() {
    let anio = parseInt(document.getElementById("uiFiltroAnioPlanilla").value);
    let numeroSemana = parseInt(document.getElementById("uiFiltroSemana").value);
    fetchGet("CorteCajaSemanal/GetReportesCajaEnProceso/?anioOperacion=" + anio.toString() + "&semanaOperacion=" + numeroSemana.toString(), "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            FillComboUnicaOpcion("uiFiltroReporteCaja", "-1", "-- No existe reportes -- ");
            document.getElementById("uiMontoParaDesgloce").value = 0;
        } else {
            FillCombo(rpta, "uiFiltroReporteCaja", "codigoReporte", "fechaCorteStr", "- seleccione -", "-1");
            MostrarMontoParaDesgloce();
        }
    })

    fetchGet("ProgramacionSemanal/GetDiasOperacion/?anio=" + anio.toString() + "&numeroSemana=" + numeroSemana.toString(), "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            FillComboUnicaOpcion("uiFiltroDiaOperacion", "-1", "-- No existe días -- ");
        } else {
            FillCombo(rpta, "uiFiltroDiaOperacion", "fechaStr", "dia", "- seleccione -", "-1");
        }
    })
}

function FillEmpleadosCuentasPorCobrarConfiguracion() {
    let codigoOperacion = parseInt(document.getElementById("uiFiltroTipoOperacion").value);
    let codigoFrecuenciaPago = parseInt(document.getElementById("uiFiltroFrecuenciaPago").value);
    switch (codigoOperacion) {
        case 1: // Back to Back
            MostrarCuentasPorCobrarBackToBackConfiguracionDevolucion();
            break;
        case 2: // Descuento
            MostrarCuentasPorCobrarConfiguracionDescuentos(codigoFrecuenciaPago);
            break;
        default: break;
    }
}

function MostrarCuentasPorCobrarEnTesoreria() {
    objConfiguracion = {
        url: "PlanillaTemporal/GetPagosDescuentos",
        cabeceras: ["Código", "Tipo Planilla", "Empresa", "Código Empleado", "Nombre Empleado","codigoOperacion","Operacion","Frecuencia Pago", "Año", "Mes","Periodo", "Monto Descuento/Devolución", "Anular"],
        propiedades: ["codigoPago", "tipoPlanilla", "nombreEmpresa", "codigoEmpleado", "nombreCompleto","codigoOperacion","operacion", "frecuenciaPago", "anio", "nombreMes","periodo","monto", "permisoAnular"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        displaydecimals: ["monto"],
        eliminar: true,
        funcioneliminar: "PagoDescuentoTemporal",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [5],
                "visible": false,
            }, {
                "targets": [11],
                "visible": true,
                "className": "dt-body-right"
            }, {
                "targets": [12],
                "visible": false
            }],
        slug: "codigoPago"
    }
    pintar(objConfiguracion);
}

function MostrarCuentasPorCobrarEnTesoreriaConsulta(anio, numeroMes, codigoEmpresa) {
    objConfiguracion = {
        url: "PlanillaTemporal/GetPagosDescuentosConsulta/?anio=" + anio.toString() + "&mes=" + numeroMes.toString() + "&codigoEmpresa=" + codigoEmpresa.toString(),
        cabeceras: ["Código", "Tipo Planilla", "Empresa", "Código Empleado", "Nombre Empleado","CodigoOperacion","Operación","Frecuencia Pago", "Año", "Mes", "Monto a Devolver"],
        propiedades: ["codigoPago", "tipoPlanilla", "nombreEmpresa", "codigoEmpleado", "nombreCompleto", "codigoOperacion", "operacion", "frecuenciaPago","anio", "nombreMes", "monto"],
        divContenedorTabla: "divContenedorTabla",
        displaydecimals: ["monto"],
        divPintado: "divTabla",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [5],
                "visible": false
            }, {
                "targets": [10],
                "visible": true,
                "className": "dt-body-right"
            }],
        slug: "codigoPago"
    }
    pintar(objConfiguracion);
}

function MostrarCuentasPorCobrarPlanilla() {
    objConfiguracion = {
        url: "PlanillaTemporal/GetEmpleadosCuentasPorCobrarPlanilla",
        cabeceras: ["Código Empresa", "Empresa", "Código Entidad", "Nombre Entidad","codigoCategoria","Categoría","CodigoOperacionDescuento", "Operación", "Saldo Pendiente"],
        propiedades: ["codigoEmpresa", "nombreEmpresa", "codigoEmpleado", "nombreCompleto","codigoCategoria","nombreCategoria", "codigoOperacionDescuento", "operacion", "saldoPendiente"],
        divContenedorTabla: "divContenedorTabla",
        displaydecimals: ["saldoPendiente"],
        divPintado: "divTabla",
        paginar: true,
        addTextBox: true,
        propertiesColumnTextBox: [{
            "header": "Monto Descuento",
            "value": "montoDescuento",
            "name": "MontoDescuento",
            "align": "text-right",
            "validate": "decimal-2"
                                }],
        aceptar: true,
        funcionaceptar: "GuardarDescuentoDevolucion",
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
        slug: "codigoEmpresa"
    }
    pintar(objConfiguracion);
}

function clickGuardarDescuentoDevolucion() {
    //let anioOperacion = parseInt(document.getElementById("uiAnioOperacionReporteCxC").value);
    //let semanaOperacion = parseInt(document.getElementById("uiSemanaOperacionReporteCxC").value);
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', 'tr', 'input:button', function () {
        let rowIdx = table.row(this).index();
        let codigoEmpresa = table.cell(rowIdx, 0).data()
        let nombreEmpresa = table.cell(rowIdx, 1).data()
        let codigoEmpleado = table.cell(rowIdx, 2).data()
        let nombreEmpleado = table.cell(rowIdx, 3).data()
        let codigoCategoria = table.cell(rowIdx, 4).data()
        let codigoOperacion = table.cell(rowIdx, 6).data()

        let monto = table.cell(rowIdx, 9).nodes().to$().find('input').val()
        setI("uiTitlePopupAbonoPrestamo", "Abono a Préstamo");
        document.getElementById("ShowPopupAbonoPrestamo").click();
        set("uiCodigoEmpresa", codigoEmpresa);
        set("uiNombreEmpresa", nombreEmpresa);
        set("uiCodigoEmpleado", codigoEmpleado);
        set("uiNombreEmpleado", nombreEmpleado);
        set("uiCodigoOperacion", codigoOperacion);
        set("uiCodigoCategoria", codigoCategoria);
        //set("uiAnioOperacion", anioOperacion.toString());
        //set("uiSemanaOperacion", semanaOperacion.toString());
        set("uiMonto", monto);
    });

}

function GuardarAbonoPrestamoEnCxC() {
    let errores = ValidarDatos("frmAbonoPrestamo")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let frmGuardar = document.getElementById("frmAbonoPrestamo");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("PlanillaTemporal/GuardarDescuentoDevolucion", "text", frm, function (data) {
            if (data == "OK") {
                document.getElementById("uiClosePopupAbonoPrestamo").click();
                MostrarCuentasPorCobrarPlanilla();
            } else {
                MensajeError(data);
            }
        })
    })
}


function MostrarCuentasPorCobrarConfiguracionDescuentos(codigoFrecuenciaPago) {
    objConfiguracion = {
        url: "Planilla/GetEmpleadosCuentasPorCobrar/?codigoFrecuenciaPago=" + codigoFrecuenciaPago.toString(),
        cabeceras: ["Código Empresa", "Empresa", "Código Empleado", "Nombre Empleado", "Código Frecuencia Pago", "Frecuencia de Pago", "Saldo Préstamo","Monto a Descontar"],
        propiedades: ["codigoEmpresa", "nombreEmpresa", "codigoEmpleado", "nombreCompleto", "codigoFrecuenciaPago", "frecuenciaPago", "saldoPrestamo","montoDescuentoPrestamo"],
        divContenedorTabla: "divContenedorTabla",
        displaydecimals: ["saldoPrestamo","montoDescuentoPrestamo"],
        divPintado: "divTabla",
        paginar: true,
        addTextBox: true,
        propertiesColumnTextBox: [{
            "header": "Monto Descuento",
            "value": "montoDescuentoPrestamo",
            "name": "MontoDescuentoPrestamo",
            "align": "text-right",
            "validate": "decimal-2"
        }],
        ocultarColumnas: true,
        aceptar: true,
        funcionaceptar: "AceptarModificacionMontoDescuento",
        hideColumns: [
            {
                "targets": [0],
                "visible": false
            }, {
                "targets": [4],
                "visible": false
            }, {
                "targets": [6],
                "visible": true,
                "className": "dt-body-right"
            }, {
                "targets": [7],
                "visible": true,
                "className": "dt-body-right"
            }],
        slug: "codigoEmpresa"
    }
    pintar(objConfiguracion);
}

function clickAceptarModificacionMontoDescuento(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', 'tr', 'input:button', function () {
        let rowIdx = table.row(this).index();
        let codigoEmpresa = parseInt(table.cell(rowIdx, 0).data());
        let codigoEmpleado = table.cell(rowIdx, 2).data();
        let montoDescuentoPrestamo = table.cell(rowIdx, 8).nodes().to$().find('input').val();
        Confirmacion("Aceptación de Configuración", "¿Monto Correcto?", function (rpta) {
            fetchGet("Planilla/RegistrarConfiguracionPrestamo/?codigoEmpresa=" + codigoEmpresa + "&codigoEmpleado=" + codigoEmpleado + "&montoDescuentoPrestamo=" + montoDescuentoPrestamo, "text", function (data) {
                if (data != "OK") {
                    MensajeError(data);
                    return;
                } else {
                    FillEmpleadosCuentasPorCobrarConfiguracion();
                }
            })
        })
    });
}

function MostrarCuentasPorCobrarBackToBackPlanilla(codigoTipoPlanilla, anioPlanilla, mesPlanilla) {
    objConfiguracion = {
        url: "PlanillaTemporal/GetEmpleadosBackToBackPlanilla/?codigoTipoPlanilla=" + codigoTipoPlanilla.toString() + "&anioPlanilla=" + anioPlanilla.toString() + "&mesPlanilla=" + mesPlanilla.toString(),
        cabeceras: ["Código Empresa", "Empresa", "Código Empleado", "Nombre Empleado","Código Operación","Operación", "Codigo Frecuencia Pago","Frecuencia de Pago","Tipo BTB","Bono Decreto 37-2001","Salario Diario"],
        propiedades: ["codigoEmpresa", "nombreEmpresa", "codigoEmpleado", "nombreCompleto","codigoOperacion","operacion", "codigoFrecuenciaPago","frecuenciaPago","codigoTipoBTB","bonoDecreto372001", "salarioDiario"],
        divContenedorTabla: "divContenedorTabla",
        displaydecimals: ["monto"],
        divPintado: "divTabla",
        paginar: true,
        addTextBox: true,
        propertiesColumnTextBox: [
            {
                "header": "Bono Decreto 37-2001",
                "value": "bonoDecreto372001",
                "name": "BonoDecreto372001",
                "align": "text-right",
                "validate": "decimal-2"
            }, {
                "header": "Monto Devolución",
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
        slug: "codigoEmpresa"
    }
    pintar(objConfiguracion);
}


function MostrarCuentasPorCobrarBackToBackConfiguracionDevolucion() {
    objConfiguracion = {
        url: "Planilla/GetEmpleadosBackToBack",
        cabeceras: ["Código Empresa", "Empresa", "Código Empleado", "Nombre Empleado", "Codigo Frecuencia Pago", "Frecuencia de Pago","Bono Decreto 37-2001", "Salario Diario"],
        propiedades: ["codigoEmpresa", "nombreEmpresa", "codigoEmpleado", "nombreCompleto", "codigoFrecuenciaPago", "frecuenciaPago", "bonoDecreto372001", "salarioDiario"],
        divContenedorTabla: "divContenedorTabla",
        displaydecimals: ["salarioDiario"],
        divPintado: "divTabla",
        paginar: true,
        addTextBox: true,
        propertiesColumnTextBox: [{
            "header": "Bono Decreto 37-2001",
            "value": "bonoDecreto372001",
            "name": "BonoDecreto372001",
            "align": "text-right",
            "validate": "decimal-2"
        }, {
            "header": "Salario Diario",
            "value": "salarioDiario",
            "name": "SalarioDiario",
            "align": "text-right",
            "validate": "decimal-2"
        }],
        ocultarColumnas: true,
        aceptar: true,
        funcionaceptar: "AceptarModificacionMontosCalculoBTB",
        hideColumns: [
            {
                "targets": [0],
                "visible": false
            }, {
                "targets": [4],
                "visible": false
            }, {
                "targets": [6],
                "visible": true,
                "className": "dt-body-right"
            }, {
                "targets": [7],
                "visible": true,
                "className": "dt-body-right"
            }],
        slug: "codigoEmpresa"
    }
    pintar(objConfiguracion);
}

function clickAceptarModificacionMontosCalculoBTB(obj) {
    let table = $('#tabla').DataTable();
    $('#tabla tbody').on('click', 'tr', 'input:button', function () {
        let rowIdx = table.row(this).index();
        let codigoEmpresa = parseInt(table.cell(rowIdx, 0).data());
        let codigoEmpleado = table.cell(rowIdx, 2).data();
        let montoDecreto372001 = table.cell(rowIdx, 8).nodes().to$().find('input').val();
        let montoSalarioDiario = table.cell(rowIdx, 9).nodes().to$().find('input').val();
        Confirmacion("Aceptación de Configuración", "¿Monto Correcto?", function (rpta) {
            fetchGet("Planilla/RegistrarConfiguracionDevolucionBTB/?codigoEmpresa=" + codigoEmpresa + "&codigoEmpleado=" + codigoEmpleado + "&montoSalarioDiario=" + montoSalarioDiario + "&montoBonoDecreto372001=" + montoDecreto372001, "text", function (data) {
                if (data != "OK") {
                    MensajeError(data);
                    return;
                } else {
                    FillEmpleadosCuentasPorCobrarConfiguracion();
                }
            })
        })
    });
}

function EliminarPagoDescuentoTemporal(obj) {
    Confirmacion("Anulación de Pago", "¿Desea anular el pago registrado?", function (rpta) {
        fetchGet("PlanillaTemporal/AnularPagoDescuento/?codigoPago=" + obj.toString(), "text", function (data) {
            if (data != "OK") {
                MensajeError(data);
                return;
            } else {
                MostrarCuentasPorCobrarEnTesoreria();
            }
        })
    })
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
        listarTransaccionesCajaChica();
    })
}

function FillFiltroSemanas(obj) {
    let anio = parseInt(obj.value);
    let arrayDate = getFechaSistema();
    let anioActual = arrayDate[0]["anio"];
    let numeroSemanaActual = parseInt(document.getElementById("uiNumeroSemanaActualSistema").value);
    if (anio != anioActual) {
        numeroSemanaActual = 53;
    }
    fetchGet("ProgramacionSemanal/GetSemanasAnteriores/?anio=" + anio.toString() + "&numeroSemanaActual=" + numeroSemanaActual.toString() + "&numeroSemanas=6&incluirSemanaActual=1", "json", function (rpta) {
        if (rpta != undefined && rpta != null && rpta.length != 0) {
            FillCombo(rpta, "uiFiltroSemana", "numeroSemana", "periodo", "- seleccione -", "-1");
        } else {
            FillComboUnicaOpcion("uiFiltroSemana", "-1", "-- No existen semanas -- ");
        }
    })
}


function GuardarDesglocePlanilla() {
    let errores = ValidarDatos("frmDesglocePlanilla")
    if (errores != "") {
        MensajeError(errores);
        return;
    }

    let frmGuardar = document.getElementById("frmDesglocePlanilla");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Transaccion/GuardarDatos/?complemento=1", "text", frm, function (data) {
            if (!/^[0-9]+$/.test(data)) {
                MensajeError(data);
            } else {
                setFiltroDesglocePagoPlanillas();
            }
        })
    })
}

function MostrarTransacciones() {
    let objConfiguracion = {
        url: "Transaccion/BuscarTransaccionesDesglocePagoPlanillas",
        cabeceras: ["Código", "Código Operación", "Operación", "Código Cuenta por Cobrar", "Número Recibo", "Fecha Recibo", "Entidad", "Categoría", "Area", "Monto", "Estado", "Fecha Transacción", "Creado por", "Anular", "Editar", "Signo", "Número Recibo", "Ruta", "Fecha Impresión", "Recursos"],
        propiedades: ["codigoTransaccion", "codigoOperacion", "operacion", "codigoCuentaPorCobrar", "numeroRecibo", "fechaReciboStr", "nombreEntidad", "categoriaEntidad","area", "monto", "estado", "fechaIngStr", "usuarioIng", "permisoAnular", "permisoEditar", "signo", "numeroReciboStr", "ruta", "fechaImpresionStr", "recursos"],
        displaydecimals: ["monto"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        eliminar: true,
        funcioneliminar: "TransaccionComplemento",
        aceptar: true,
        funcionaceptar: "AceptarTransaccionComplemento",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [1],
                "visible": false
            }, {
                "targets": [3],
                "visible": false
            }, {
                "targets": [9],
                "className": "dt-body-right"
            }, {
                "targets": [13],
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
            }, {
                "targets": [17],
                "visible": false
            }, {
                "targets": [18],
                "visible": false
            }, {
                "targets": [19],
                "visible": false
            }],
        slug: "codigoTransaccion"
    }
    pintar(objConfiguracion);
}


function EliminarTransaccionComplemento(obj){
    Confirmacion("Anulación de Operación", "¿Está seguro de anular la operación?", function (rpta) {
        fetchGet("Transaccion/AnularTransaccionComplementoContabilidad/?codigoTransaccion=" + obj.toString(), "text", function (data) {
            if (data != "OK") {
                MensajeError(data);
                return;
            } else {
                MostrarTransacciones();
            }
        })
    })
}


function clickAceptarTransaccionComplemento(obj) {
    Confirmacion("Visto Bueno de Operación", "¿La operación es correcta?", function (rpta) {
        fetchGet("Transaccion/AceptarTransaccionComplementoContabilidad/?codigoTransaccion=" + obj.toString(), "text", function (data) {
            if (data != "OK") {
                MensajeError(data);
                return;
            } else {
                MostrarTransacciones();
            }
        })
    })
}


function MostrarMontoParaDesgloce() {
    let anioOperacion = parseInt(document.getElementById("uiFiltroAnioPlanilla").value);
    let semanaOperacion = parseInt(document.getElementById("uiFiltroSemana").value);
    let codigoReporte = parseInt(document.getElementById("uiFiltroReporteCaja").value);
    if (codigoReporte != -1) {
        fetchGet("Transaccion/GetMontoPlanillaParaDesglosar/?anioOperacion=" + anioOperacion.toString() + "&semanaOperacion=" + semanaOperacion.toString() + "&codigoReporte=" + codigoReporte.toString(), "text", function (data) {
            document.getElementById("uiMontoParaDesgloce").value = data;
        })
    } else {
        document.getElementById("uiMontoParaDesgloce").value = 0;
    }
}

/*function MostrarFechaOperacionReporteCxC() {
    fetchGet("PlanillaTemporal/GetFechaReporteCxCPagoBTBYDescuento", "json", function (rpta) {
        if (rpta == null || rpta == undefined || rpta.length == 0) {
            alert("No existe datos");
        } else {
            document.getElementById("uiAnioOperacionReporteCxC").value = rpta.anioOperacion;
            document.getElementById("uiSemanaOperacionReporteCxC").value = rpta.semanaOperacion;
            document.getElementById("uiDescripcionReporteCxC").innerText = "El abono por préstamo será incluido en el reporte de Cuentas por Cobrar año " + rpta.anioOperacion + " y semana " + rpta.semanaOperacion;
        }
    })
}*/


function BuscarPagosBTBDevolucionTesoreria() {
    let anio = parseInt(document.getElementById("uiFiltroAnio").value);
    let objMes = document.getElementById("uiFiltroMes");
    let objEmpresa = document.getElementById("uiFiltroEmpresa");
    let numeroMes = parseInt(objMes.options[objMes.selectedIndex].value);
    let codigoEmpresa = parseInt(objEmpresa.options[objEmpresa.selectedIndex].value);
    MostrarDevolucionesBTBEnTesoreriaConsulta(anio, numeroMes, codigoEmpresa);
}

function MostrarDevolucionesBTBEnTesoreriaConsulta(anio, numeroMes, codigoEmpresa) {
    objConfiguracion = {
        url: "PlanillaTemporal/GetPagosBackToBackRealizadosEnPlanilla/?anio=" + anio.toString() + "&mes=" + numeroMes.toString() + "&codigoEmpresa=" + codigoEmpresa.toString(),
        cabeceras: ["Código", "Tipo Planilla", "Empresa", "Código Empleado", "Nombre Empleado", "CodigoOperacion", "Operación", "Frecuencia Pago", "Año", "Mes", "Monto a Devolver"],
        propiedades: ["codigoPago", "tipoPlanilla", "nombreEmpresa", "codigoEmpleado", "nombreCompleto", "codigoOperacion", "operacion", "frecuenciaPago", "anio", "nombreMes", "monto"],
        divContenedorTabla: "divContenedorTabla",
        displaydecimals: ["monto"],
        divPintado: "divTabla",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [0],
                "className": "dt-body-center",
                "visible": true
            }, {
                "targets": [5],
                "visible": false
            }, {
                "targets": [10],
                "visible": true,
                "className": "dt-body-right"
            }],
        slug: "codigoPago"
    }
    pintar(objConfiguracion);
}