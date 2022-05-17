function get(idcontrol) {
    return document.getElementById(idcontrol).value;
}

function set(idcontrol, valor) {
    document.getElementById(idcontrol).value = valor;
}

function setN(namecontrol, valor, idformulario) {
    if (idformulario == undefined)
        document.getElementsByName(namecontrol)[0].value = valor;
    else {
        document.querySelector("#" + idformulario + "[name='" + namecontrol + "']").value = valor;
    }
}

function getI(idcontrol) {
    return document.getElementById(idcontrol).innerHTML;
}

function setI(idcontrol, valor) {
    if (document.getElementById(idcontrol))
        document.getElementById(idcontrol).innerHTML = valor;
}

function getN(namecontrol) {
    return document.getElementsByName(namecontrol)[0].value;
}

function setURL(url) {
    var raiz = document.getElementById("hdfOculto").value;
    document.getElementById("divLoading").style.display = "block";
    //http://localhost
    var urlCompleta = window.location.protocol + "//" + window.location.host + "/" + raiz
        + url
    return urlCompleta;
}

function setCheckedValueOfRadioButtonGroup(pName, pValue) {
    let radios = document.getElementsByName(pName);
    for (let j = 0; j < radios.length; j++) {
        if (radios[j].value == pValue) {
            radios[j].checked = true;
            break;
        }
    }
}

function FillCombo(data, idcontrol, propiedadid, propiedadnombre, textoprimeraopcion = "--Seleccione--", valueprimeraopcion = "-1") {
    try {
        let contenido = "";
        let objActual;
        if (data.length > 1)
            contenido += "<option value = '" + valueprimeraopcion + "'>" + textoprimeraopcion + "</option>"
        for (let i = 0; i < data.length; i++) {
            objActual = data[i];
            contenido += "<option value = '" + objActual[propiedadid] + "'>" + objActual[propiedadnombre] + "</option>"
        }
        document.getElementById(idcontrol).innerHTML = contenido;
    }
    catch (e) {
        alert("Ocurrió un error GET (" + idcontrol + ")-" + e.MensajeError);
    }
}

function FillComboSelectOption(data, idcontrol, propiedadid, propiedadnombre, textoprimeraopcion = "--Seleccione--", valueprimeraopcion = "-1") {
    try {
        let contenido = "";
        let objActual;
        for (let i = 0; i < data.length; i++) {
            objActual = data[i];
            contenido += "<option value = '" + objActual[propiedadid] + "'>" + objActual[propiedadnombre] + "</option>"
        }
        document.getElementById(idcontrol).innerHTML = contenido;
    }
    catch (e) {
        alert("Ocurrió un error GET (" + idcontrol + ")-" + e.MensajeError);
    }
}


function FillComboUnicaOpcion(idcontrol, valueprimeraopcion, textoprimeraopcion) {
    let contenido = "";
    contenido += "<option selected = 'selected' value = '" + valueprimeraopcion + "'>" + textoprimeraopcion + "</option>"
    valueInt = parseInt(valueprimeraopcion, 0);
    document.getElementById(idcontrol).selectedIndex = "0";
    document.getElementById(idcontrol).innerHTML = contenido;
}


async function fetchGet(url, tipoRespuesta, callback) {
    try {
        var raiz = document.getElementById("hdfOculto").value;
        document.getElementById("divLoading").style.display = "block";
        //http://localhost
        var urlCompleta = window.location.protocol + "//" + window.location.host + "/" + raiz
            + url

        var res = await fetch(urlCompleta)
        if (tipoRespuesta == "json") {
            res = await res.json();
        }
        else {
            if (tipoRespuesta == "text") {
                res = await res.text();
            } else {
                if (tipoRespuesta == "pdf") {
                    res = await res.arrayBuffer();
                }
            }
        }
        callback(res);
        document.getElementById("divLoading").style.display = "none";

    }
    catch (e) {
        callback(null);
        //alert("Ocurrió un error GET-" + e.MensajeError);
        document.getElementById("divLoading").style.display = "none";
    }
}

async function fetchGetDownload(url, callback) {
    try {
        var raiz = document.getElementById("hdfOculto").value;
        document.getElementById("divLoading").style.display = "block";
        //http://localhost
        var urlCompleta = window.location.protocol + "//" + window.location.host + "/" + raiz
            + url

        var res = await fetch(urlCompleta)
        res = await res.arrayBuffer();
        callback(res);
    }
    catch (e) {
        callback(null);
        document.getElementById("divLoading").style.display = "none";
    }
}

async function fetchPost(url, tipoRespuesta, frm, callback) {
    document.getElementById("divLoading").style.display = "block";
    try {

        var raiz = document.getElementById("hdfOculto").value;
        var urlCompleta = window.location.protocol + "//" + window.location.host + "/" + raiz
            + url

        var res = await fetch(urlCompleta, {
            method: "POST",
            body: frm
        });

        if (tipoRespuesta == "json")
            res = await res.json();
        else if (tipoRespuesta == "text")
            res = await res.text();
        callback(res)
        document.getElementById("divLoading").style.display = "none";
    }
    catch (e) {
        alert("Ocurrió un error POST-" + e.MensajeError);
        document.getElementById("divLoading").style.display = "none";
    }
}


async function fetchPostJson(url, tipoRespuesta, frm, callback) {
    document.getElementById("divLoading").style.display = "block";
    try {

        var raiz = document.getElementById("hdfOculto").value;
        var urlCompleta = window.location.protocol + "//" + window.location.host + "/" + raiz
            + url

        var res = await fetch(urlCompleta, {
            method: "POST",
            cache: 'no-cache',
            headers: {
                'Content-Type': 'application/json'
            },
            body: frm
        });

        if (tipoRespuesta == "json")
            res = await res.json();
        else if (tipoRespuesta == "text")
            res = await res.text();
        callback(res)
        document.getElementById("divLoading").style.display = "none";
    }
    catch (e) {
        alert("Ocurrió un error POST-" + e.MensajeError);
        document.getElementById("divLoading").style.display = "none";
    }
}

/*"drawCallback": function () {
    var api = this.api();
    $(api.table().footer()).html(
        //api.column(6, { page: 'current' }).data().sum()
        //api.column(6, { page: 'current' }).nodes().sum()
    );
}
*/

//{url:"", nombreColumna: }
//var objConfiguracionGlobal;
function pintar(objConfiguracion) {
    let objConfiguracionGlobal = objConfiguracion;
    if (objConfiguracionGlobal.divContenedorTabla == undefined)
        objConfiguracionGlobal.divContenedorTabla == "divContenedorTabla";
    if (objConfiguracionGlobal.divPintado == undefined)
        objConfiguracionGlobal.divPintado == "divTabla";
    if (objConfiguracionGlobal.editar == undefined)
        objConfiguracionGlobal.editar = false;
    if (objConfiguracionGlobal.funcioneditar == undefined)
        objConfiguracionGlobal.funcioneditar = "";
    if (objConfiguracionGlobal.eliminar == undefined)
        objConfiguracionGlobal.eliminar = false;
    if (objConfiguracionGlobal.eliminarreporte == undefined)
        objConfiguracionGlobal.eliminarreporte = false;
    if (objConfiguracionGlobal.imprimir == undefined)
        objConfiguracionGlobal.imprimir = false;
    if (objConfiguracionGlobal.autorizar == undefined)
        objConfiguracionGlobal.autorizar = false;
    if (objConfiguracionGlobal.funcionrechazar == undefined)
        objConfiguracionGlobal.funcionrechazar = "";
    if (objConfiguracionGlobal.reporte == undefined)
        objConfiguracionGlobal.reporte = false;
    if (objConfiguracionGlobal.funcionreporte == undefined)
        objConfiguracionGlobal.funcionreporte = "";
    if (objConfiguracionGlobal.pdfvalue == undefined)
        objConfiguracionGlobal.pdfvalue = "none";
    if (objConfiguracionGlobal.excel == undefined)
        objConfiguracionGlobal.excel = false;
    if (objConfiguracionGlobal.funcionexcel == undefined)
        objConfiguracionGlobal.funcionexcel = "";
    if (objConfiguracionGlobal.excelvalue == undefined)
        objConfiguracionGlobal.excelvalue = "none"
    if (objConfiguracionGlobal.web == undefined)
        objConfiguracionGlobal.web = false;
    if (objConfiguracionGlobal.webvalue == undefined)
        objConfiguracionGlobal.webvalue = "none"

    if (objConfiguracionGlobal.slug == undefined)
        objConfiguracionGlobal.slug = ""
    if (objConfiguracionGlobal.radio == undefined)
        objConfiguracionGlobal.radio = false;
    if (objConfiguracionGlobal.check == undefined)
        objConfiguracionGlobal.check = false;
    if (objConfiguracionGlobal.checkid == undefined)
        objConfiguracionGlobal.checkid = "default";
    if (objConfiguracionGlobal.checkheader == undefined)
        objConfiguracionGlobal.checkheader = "";
    if (objConfiguracionGlobal.funcioncheck == undefined)
        objConfiguracionGlobal.funcioncheck = "";
    if (objConfiguracionGlobal.idtabla == undefined)
        objConfiguracionGlobal.idtabla = "tabla"
    if (objConfiguracionGlobal.paginar == undefined)
        objConfiguracionGlobal.paginar = false;
    if (objConfiguracionGlobal.eventoradio == undefined)
        objConfiguracionGlobal.eventoradio = "Generico";
    if (objConfiguracionGlobal.datesWithoutTime == undefined)
        objConfiguracionGlobal.datesWithoutTime = [];
    if (objConfiguracionGlobal.displaydecimals == undefined)
        objConfiguracionGlobal.displaydecimals = [];
    if (objConfiguracionGlobal.sortFieldDate == undefined)
        objConfiguracionGlobal.sortFieldDate = [];
    if (objConfiguracionGlobal.ocultarColumnas == undefined)
        objConfiguracionGlobal.ocultarColumnas = false;
    if (objConfiguracionGlobal.hideColumns == undefined)
        objConfiguracionGlobal.hideColumns = [];
    if (objConfiguracionGlobal.autoWidth == undefined)
        objConfiguracionGlobal.autoWidth = true;
    if (objConfiguracionGlobal.generar == undefined)
        objConfiguracionGlobal.generar = false;
    if (objConfiguracionGlobal.arqueo == undefined)
        objConfiguracionGlobal.arqueo = false;
    if (objConfiguracionGlobal.revision == undefined)
        objConfiguracionGlobal.revision = false;
    if (objConfiguracionGlobal.funcionrevision == undefined)
        objConfiguracionGlobal.funcionrevision = "clickRevision";
    if (objConfiguracionGlobal.fieldNameRevision == undefined)
        objConfiguracionGlobal.fieldNameRevision = "";
    if (objConfiguracionGlobal.excluir == undefined)
        objConfiguracionGlobal.excluir = false;
    if (objConfiguracionGlobal.aceptar == undefined)
        objConfiguracionGlobal.aceptar = false;
    if (objConfiguracionGlobal.funcionaceptar == undefined)
        objConfiguracionGlobal.funcionaceptar = "";
    if (objConfiguracionGlobal.actualizar == undefined)
        objConfiguracionGlobal.actualizar = false;
    if (objConfiguracionGlobal.funcionactualizar == undefined)
        objConfiguracionGlobal.funcionactualizar = "";
    if (objConfiguracionGlobal.alerta == undefined)
        objConfiguracionGlobal.alerta = false;
    if (objConfiguracionGlobal.verdetalle == undefined)
        objConfiguracionGlobal.verdetalle = false;
    if (objConfiguracionGlobal.funcioneliminar == undefined)
        objConfiguracionGlobal.funcioneliminar = "";
    if (objConfiguracionGlobal.funciondetalle == undefined)
        objConfiguracionGlobal.funciondetalle = "";
    if (objConfiguracionGlobal.fieldNameExcluir == undefined)
        objConfiguracionGlobal.fieldNameExcluir = "";
    if (objConfiguracionGlobal.ExcluirEnabled == undefined)
        objConfiguracionGlobal.ExcluirEnabled = "enabled";
    if (objConfiguracionGlobal.addTextBox == undefined)
        objConfiguracionGlobal.addTextBox = false;
    if (objConfiguracionGlobal.aceptarmultiplesparametros == undefined)
        objConfiguracionGlobal.aceptarmultiplesparametros = false;
    if (objConfiguracionGlobal.sumarcolumna == undefined)
        objConfiguracionGlobal.sumarcolumna = false;
    let parser = new DOMParser();
    let xmlDoc = "";
    let abstracts = "";
    fetchGet(objConfiguracion.url, "json", async function (res) {
        var contenido = "";
        contenido += "<div id='" + objConfiguracionGlobal.divContenedorTabla + "'>";
        contenido += generarTabla(res, objConfiguracionGlobal);
        contenido += "</div>";
        setI(objConfiguracionGlobal.divPintado, contenido);
        if (objConfiguracion.paginar == true) {
            if (objConfiguracionGlobal.ocultarColumnas == false) {
                $("#" + objConfiguracion.idtabla).DataTable();
            } else {
                //if (objConfiguracionGlobal.sumarcolumna == true && objConfiguracionGlobal.columnasuma > 0) {
                if (objConfiguracionGlobal.sumarcolumna == true) {
                    $("#" + objConfiguracion.idtabla).DataTable({
                        "autoWidth": objConfiguracionGlobal.autoWidth,
                        "columnDefs": objConfiguracionGlobal.hideColumns, "order": [[0, "desc"]],
                        "footerCallback": function (row, data, start, end, display) {
                            let api = this.api();
                            objConfiguracionGlobal.columnasumalist.map(( columna ) => {
                                let total = api
                                    .column(columna)  // get column 1 , which is mark
                                    .data()
                                    .reduce(function (a, b) {
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

                                $(api.column(columna).footer()).html(  // update the footer using the  total of mark column
                                    formatterQuetzales.format(total)
                                );
                            });
                        }
                    });
                } else {
                    $("#" + objConfiguracion.idtabla).DataTable({
                        "autoWidth": objConfiguracionGlobal.autoWidth,
                        "columnDefs": objConfiguracionGlobal.hideColumns, "order": [[0, "desc"]]
                    });
                }
            }

        }
    })
}

function generarTabla(res, objConfiguracionGlobal) {
    let contenido = "";
    //console.log(JSON.stringify(objConfiguracionGlobal));
    let cabeceras = objConfiguracionGlobal.cabeceras;
    let nombrePropiedades = objConfiguracionGlobal.propiedades;
    let datesWithoutTime = objConfiguracionGlobal.datesWithoutTime;
    let displaydecimals = objConfiguracionGlobal.displaydecimals;
    let sortFieldDate = objConfiguracionGlobal.sortFieldDate;

    let countColumns = 0;
    let nregistros = res.length;
    if (nregistros > 0) {
        contenido += "<table id='" + objConfiguracionGlobal.idtabla + "' class='table'>";
        contenido += "<thead class='table-dark'>"
        contenido += "<tr>";
        if (objConfiguracionGlobal.radio == true) {
            contenido += "<td>  </td>";
            countColumns++;
        }

        for (let i = 0; i < cabeceras.length; i++) {
            contenido += "<td>" + cabeceras[i] + "</td>";
            countColumns++;
        }
        if (objConfiguracionGlobal.editar == true ||
            objConfiguracionGlobal.eliminar == true ||
            objConfiguracionGlobal.autorizar == true ||
            objConfiguracionGlobal.imprimir == true ||
            objConfiguracionGlobal.reporte == true ||
            objConfiguracionGlobal.generar == true ||
            objConfiguracionGlobal.arqueo == true ||
            objConfiguracionGlobal.revision == true ||
            objConfiguracionGlobal.excluir == true ||
            objConfiguracionGlobal.aceptar == true ||
            objConfiguracionGlobal.actualizar == true ||
            objConfiguracionGlobal.verdetalle == true ||
            objConfiguracionGlobal.addTextBox == true ||
            objConfiguracionGlobal.alerta == true ||
            objConfiguracionGlobal.excel == true ||
            objConfiguracionGlobal.web == true ||
            objConfiguracionGlobal.check == true) {

            if (objConfiguracionGlobal.addTextBox == true) {
                objConfiguracionGlobal.propertiesColumnTextBox.map(({ header }) => {
                    contenido += `<td>${header}</td>`
                    countColumns++;
                })
            }
            if (objConfiguracionGlobal.alerta == true) {
                contenido += "<td>Alerta</td>"
                countColumns++;
            }
            if (objConfiguracionGlobal.check == true) {
                contenido += "<td>" + objConfiguracionGlobal.checkheader + "</td>";
                countColumns++;
            }

            if (objConfiguracionGlobal.excluir == true) {
                contenido += "<td>Excluir</td>"
                countColumns++;
            }
            if (objConfiguracionGlobal.editar == true) {
                contenido += "<td></td>"
                countColumns++;
            }
            if (objConfiguracionGlobal.eliminar == true) {
                contenido += "<td></td>"
                countColumns++;
            }
            if (objConfiguracionGlobal.autorizar == true) {
                contenido += "<td></td>"
                countColumns++;
            }
            if (objConfiguracionGlobal.imprimir == true) {
                contenido += "<td></td>"
                countColumns++;
            }
            if (objConfiguracionGlobal.reporte == true) {
                contenido += "<td></td>"
                countColumns++;
            }

            if (objConfiguracionGlobal.generar == true) {
                contenido += "<td></td>"
                countColumns++;
            }

            if (objConfiguracionGlobal.arqueo == true) {
                contenido += "<td>Arqueo</td>"
                countColumns++;
            }

            if (objConfiguracionGlobal.revision == true) {
                contenido += "<td></td>"
                countColumns++;
            }

            if (objConfiguracionGlobal.aceptar == true) {
                contenido += "<td></td>"
                countColumns++;
            }

            if (objConfiguracionGlobal.actualizar == true) {
                contenido += "<td></td>"
                countColumns++;
            }

            if (objConfiguracionGlobal.verdetalle == true) {
                contenido += "<td></td>"
                countColumns++;
            }
            if (objConfiguracionGlobal.excel == true) {
                contenido += "<td></td>"
                countColumns++;
            }
            if (objConfiguracionGlobal.web == true) {
                contenido += "<td></td>"
                countColumns++;
            }
        }

        contenido += "</tr>";
        contenido += "</thead>";

        contenido += "<tbody id='tbody" + objConfiguracionGlobal.idtabla + "'>";
        let obj;
        let valor = "";
        let nombrePropiedadActual;
        //let clases, resultado;
        //let parameter1, parameter2, parameter3;
        for (let i = 0; i < nregistros; i++) {
            obj = res[i]
            if (objConfiguracionGlobal.checkid == "default") {
                contenido += `<tr>`;
            } else {
                if (obj[objConfiguracionGlobal.checkid] == 1) {
                    contenido += `<tr style="background-color:#f0f8ff;">`;
                } else {
                    contenido += `<tr>`;
                }
            }
            if (objConfiguracionGlobal.radio == true) {
                if (typeof (obj[objConfiguracionGlobal.slug]) === 'number')
                    valor = obj[objConfiguracionGlobal.slug].toString();
                else
                    valor = obj[objConfiguracionGlobal.slug];
                contenido += "<td> <input type='radio' name='radio' class='table-row-selected chkSelected' value='" + valor + "' onclick='getDataRowRadio" + objConfiguracionGlobal.eventoradio + "(this)'> </td>";
            }
            for (let j = 0; j < nombrePropiedades.length; j++) {
                nombrePropiedadActual = nombrePropiedades[j]
                // Quita el tiempo a las fechas
                if (datesWithoutTime.find(el => el === nombrePropiedadActual) != undefined) {
                    contenido += "<td class='chkSelected'>" + (new Date(obj[nombrePropiedadActual])).toLocaleDateString() + "</td>";
                } else {
                    if (sortFieldDate.find(el => el === nombrePropiedadActual) != undefined) {
                        if (nombrePropiedadActual != "") {
                            let ukDatea = obj[nombrePropiedadActual].split('/');
                            let codigo = (ukDatea[2] + ukDatea[1] + ukDatea[0]) * 1;
                            contenido += "<td class='chkSelected'><span hidden> " + codigo.toString() + "</span>" + obj[nombrePropiedadActual] + "</td>";
                        } else {
                            contenido += "<td class='chkSelected'>" + obj[nombrePropiedadActual] + "</td>";
                        }
                    } else {
                        // formato a los montos
                        if (displaydecimals.find(el => el === nombrePropiedadActual) != undefined) {
                            contenido += "<td class='chkSelected'>" + obj[nombrePropiedadActual].toFixed(2) + "</td>";
                        } else {
                            contenido += "<td class='chkSelected'>" + obj[nombrePropiedadActual] + "</td>";
                        }
                    }
                }
            }
            if (objConfiguracionGlobal.editar == true ||
                objConfiguracionGlobal.check == true ||
                objConfiguracionGlobal.eliminar == true ||
                objConfiguracionGlobal.autorizar == true ||
                objConfiguracionGlobal.imprimir == true ||
                objConfiguracionGlobal.reporte == true ||
                objConfiguracionGlobal.generar == true ||
                objConfiguracionGlobal.arqueo == true ||
                objConfiguracionGlobal.revision == true ||
                objConfiguracionGlobal.excluir == true ||
                objConfiguracionGlobal.aceptar == true ||
                objConfiguracionGlobal.actualizar == true ||
                objConfiguracionGlobal.verdetalle == true || 
                objConfiguracionGlobal.addTextBox == true ||
                objConfiguracionGlobal.alerta == true ||
                objConfiguracionGlobal.excel == true ||
                objConfiguracionGlobal.web == true ||
                objConfiguracionGlobal.check == true) {

                let slug = objConfiguracionGlobal.slug;
                let pdfvalue = objConfiguracionGlobal.pdfvalue;
                let excelvalue = objConfiguracionGlobal.excelvalue;
                let webvalue = objConfiguracionGlobal.webvalue;
                if (objConfiguracionGlobal.addTextBox == true) {
                    objConfiguracionGlobal.propertiesColumnTextBox.map(({ value, name, align, validate }) => {
                        contenido += "<td style='padding: 2px;'>";
                        contenido += `<input type='text' class="form-control ${align} ${validate}" id="ui${name}${i}" name='${name}' maxlength="20" multiline="false" type="text"  value ='${obj[value]}' autocomplete = "off" onclick="clickMontoPorDevolver(this)" />`;
                        contenido += "</td>";
                    });
                }
                if (objConfiguracionGlobal.alerta == true) {
                    contenido += "<td style='padding: 2px;' class='option-alerta'>";
                    if (obj["codigoTransaccionAnt"] > 0 || obj["correccion"] == 1) {
                        contenido += `<button class="btn">`;
                        if (typeof obj[slug] === 'string') {
                            contenido += `<i onclick="clickAlerta${objConfiguracionGlobal.funcionalerta}('${obj[slug]}')" class="btn btn-danger">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="22" height="22" fill="currentColor" class="bi bi-exclamation-circle" viewBox="0 0 16 16">
                                  <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                                  <path d="M7.002 11a1 1 0 1 1 2 0 1 1 0 0 1-2 0zM7.1 4.995a.905.905 0 1 1 1.8 0l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 4.995z"/>
                                  </svg>
                                  </i>`;
                        } else {
                            contenido += `<i onclick="clickAlerta${objConfiguracionGlobal.funcionalerta}(${obj[slug]})" class="btn btn-danger">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="22" height="22" fill="currentColor" class="bi bi-exclamation-circle" viewBox="0 0 16 16">
                                  <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                                  <path d="M7.002 11a1 1 0 1 1 2 0 1 1 0 0 1-2 0zM7.1 4.995a.905.905 0 1 1 1.8 0l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 4.995z"/>
                                  </svg>
                                  </i>`;
                        }
                        contenido += `</button>`;
                    } 
                    contenido += "</td>";
                }


                if (objConfiguracionGlobal.check == true) {
                    contenido += `<td class='option-check'>`;
                    if (objConfiguracionGlobal.checkid == "default") {
                        contenido += `<input type='checkbox' name='check' class='table-row-selected chkSelected' value='0' />`;
                    } else {
                        if (obj[objConfiguracionGlobal.checkid] == 1) {
                            contenido += `<input type='checkbox' name='check' class='table-row-selected chkSelected' value='1' checked onClick="clickCheck${objConfiguracionGlobal.funcioncheck}(this)" />`;
                        } else {
                            contenido += `<input type='checkbox' name='check' class='table-row-selected chkSelected' value='0' onClick="clickCheck${objConfiguracionGlobal.funcioncheck}(this)" />`;
                        }
                    }
                    contenido += `</td>`;
                }


                if (objConfiguracionGlobal.excluir == true) {
                    let fieldNameExcluir = objConfiguracionGlobal.fieldNameExcluir;
                    contenido += "<td style='padding: 2px; text-align:center' class='option-check'>";
                    if (obj[fieldNameExcluir] == 1) { //  1=Excluir (set checked)
                        contenido += `<input class="form-check-input" type="checkbox" value="${obj[fieldNameExcluir]}" id="flexCheckDefault" onClick="Excluir(this)" checked ${objConfiguracionGlobal.ExcluirEnabled}>`;
                    } else {
                        contenido += `<input class="form-check-input" type="checkbox" value="${obj[fieldNameExcluir]}" id="flexCheckDefault" onClick="Excluir(this)">`;
                    }
                    contenido += "</td>";
                }

                if (objConfiguracionGlobal.verdetalle == true) {
                    contenido += "<td style='padding: 2px;' class='option-detalle'>";
                    contenido += `<button type="button" class="btn btn-link" onclick="VerDetalle${objConfiguracionGlobal.funciondetalle}(${obj[slug]})">Detalle</button>`;
                    contenido += "</td>";
                }
                
                if (objConfiguracionGlobal.editar == true) {
                    contenido += "<td style='padding: 2px;' class='option-editar'>";
                    if (obj["permisoEditar"] == 1) {
                        contenido += `<button class="btn">`;
                        if (typeof obj[slug] === 'string') {
                            contenido += `<i onclick="Editar${objConfiguracionGlobal.funcioneditar}('${obj[slug]}')" class="btn btn-primary"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil" viewBox="0 0 16 16">
                            <path d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168l10-10zM11.207 2.5 13.5 4.793 14.793 3.5 12.5 1.207 11.207 2.5zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293l6.5-6.5zm-9.761 5.175-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325z" />
                            </svg></i>`;
                            contenido += `</button>`;
                        } else {
                            contenido += `<i onclick="Editar${objConfiguracionGlobal.funcioneditar}(${obj[slug]})" class="btn btn-primary"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil" viewBox="0 0 16 16">
                            <path d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168l10-10zM11.207 2.5 13.5 4.793 14.793 3.5 12.5 1.207 11.207 2.5zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293l6.5-6.5zm-9.761 5.175-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325z" />
                            </svg></i>`;
                            contenido += `</button>`;
                        }
                    } else {
                        contenido += `<button class="btn">`;
                        contenido += `<i class="btn btn-secondary"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil" viewBox="0 0 16 16">
                            <path d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168l10-10zM11.207 2.5 13.5 4.793 14.793 3.5 12.5 1.207 11.207 2.5zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293l6.5-6.5zm-9.761 5.175-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325z" />
                            </svg></i>`;
                        contenido += `</button>`;
                    }
                    contenido += "</td>";
                }

                if (objConfiguracionGlobal.eliminar == true) {
                    contenido += "<td style='padding: 2px;' class='option-eliminar'>";
                    if (obj["permisoAnular"] == 1) {
                            contenido += `<button class="btn">`;
                            if (typeof obj[slug] === 'string') {
                                contenido += `<i onclick="Eliminar${objConfiguracionGlobal.funcioneliminar}('${obj[slug]}')" class="btn btn-danger">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash-fill" viewBox="0 0 16 16">
                                  <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z" />
                                  </svg></i>`;
                            } else {
                                contenido += `<i onclick="Eliminar${objConfiguracionGlobal.funcioneliminar}(${obj[slug]})" class="btn btn-danger">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash-fill" viewBox="0 0 16 16">
                                  <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z" />
                                  </svg></i>`;
                            }
                            contenido += `</button>`;
                    } else {
                        contenido += `<button class="btn">`;
                        contenido += `<i class="btn btn-secondary">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash-fill" viewBox="0 0 16 16">
                            <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z" />
                            </svg></i>`;
                        contenido += `</button>`;
                    }
                    contenido += "</td>";
                }

                if (objConfiguracionGlobal.autorizar == true) {
                    contenido += "<td style='padding: 2px;' class='option-autorizar'>";
                    if (obj["permisoAutorizar"] == 1) {
                        contenido += `<button class="btn">`;
                        if (typeof obj[slug] === 'string') {
                            contenido += `<i onclick="click${objConfiguracionGlobal.funcionautorizar}('${obj[slug]}')" class="btn btn-outline-primary">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="22" height="22" fill="currentColor" class="bi bi-check-circle" viewBox="0 0 16 16">
                                  <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                                  <path d="M10.97 4.97a.235.235 0 0 0-.02.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-1.071-1.05z"/>
                                  </svg>
                                   </i>`;
                        } else {
                            contenido += `<i onclick="click${objConfiguracionGlobal.funcionautorizar}(${obj[slug]})" class="btn btn-outline-primary">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="22" height="22" fill="currentColor" class="bi bi-check-circle" viewBox="0 0 16 16">
                                  <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                                  <path d="M10.97 4.97a.235.235 0 0 0-.02.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-1.071-1.05z"/>
                                  </svg>
                                  </i>`;
                        }
                        contenido += `</button>`;
                    } else {
                        contenido += `<button class="btn">`;
                        contenido += `<i class="btn btn-secondary">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="22" height="22" fill="currentColor" class="bi bi-check-circle" viewBox="0 0 16 16">
                                  <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                                  <path d="M10.97 4.97a.235.235 0 0 0-.02.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-1.071-1.05z"/>
                                  </svg>
                                  </svg>
                                     </i>`;
                        contenido += `</button>`;
                    }
                    contenido += "</td>";
                }

                if (objConfiguracionGlobal.imprimir == true) {
                    contenido += "<td style='padding: 2px;' class='option-imprimir'>";
                    contenido += `<button class="btn">`;
                    contenido += `<i onclick="Imprimir(${obj[slug]},this)" id="id${+ obj[slug]}" class="btn btn-success"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-printer-fill" viewBox="0 0 16 16">
                            <path d="M5 1a2 2 0 0 0-2 2v1h10V3a2 2 0 0 0-2-2H5zm6 8H5a1 1 0 0 0-1 1v3a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1v-3a1 1 0 0 0-1-1z"/>
                            <path d="M0 7a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v3a2 2 0 0 1-2 2h-1v-2a2 2 0 0 0-2-2H5a2 2 0 0 0-2 2v2H2a2 2 0 0 1-2-2V7zm2.5 1a.5.5 0 1 0 0-1 .5.5 0 0 0 0 1z"/>
                            </svg></i>`;
                    contenido += `</button>`;
                    contenido += "</td>";
                }
                if (objConfiguracionGlobal.reporte == true) {
                    contenido += "<td style='padding: 2px;' class='option-reporte'>";
                    if (obj[slug] != 0) {
                        contenido += `<button class="btn">`;
                        //contenido += `<i onclick="GenerarPdf${objConfiguracionGlobal.funcionreporte}(${obj[slug]})" id="idpdf${+ obj[slug]}" class="btn btn-light">
                        contenido += `<i onclick="GenerarPdf${objConfiguracionGlobal.funcionreporte}(${obj[slug]})" id="idpdf${+ obj[slug]}" class="btn btn-primary">
                              <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" class="bi bi-file-earmark-pdf" viewBox="0 0 16 16">
                              <path d="M14 14V4.5L9.5 0H4a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2zM9.5 3A1.5 1.5 0 0 0 11 4.5h2V14a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1h5.5v2z"/>
                              <path d="M4.603 14.087a.81.81 0 0 1-.438-.42c-.195-.388-.13-.776.08-1.102.198-.307.526-.568.897-.787a7.68 7.68 0 0 1 1.482-.645 19.697 19.697 0 0 0 1.062-2.227 7.269 7.269 0 0 1-.43-1.295c-.086-.4-.119-.796-.046-1.136.075-.354.274-.672.65-.823.192-.077.4-.12.602-.077a.7.7 0 0 1 .477.365c.088.164.12.356.127.538.007.188-.012.396-.047.614-.084.51-.27 1.134-.52 1.794a10.954 10.954 0 0 0 .98 1.686 5.753 5.753 0 0 1 1.334.05c.364.066.734.195.96.465.12.144.193.32.2.518.007.192-.047.382-.138.563a1.04 1.04 0 0 1-.354.416.856.856 0 0 1-.51.138c-.331-.014-.654-.196-.933-.417a5.712 5.712 0 0 1-.911-.95 11.651 11.651 0 0 0-1.997.406 11.307 11.307 0 0 1-1.02 1.51c-.292.35-.609.656-.927.787a.793.793 0 0 1-.58.029zm1.379-1.901c-.166.076-.32.156-.459.238-.328.194-.541.383-.647.547-.094.145-.096.25-.04.361.01.022.02.036.026.044a.266.266 0 0 0 .035-.012c.137-.056.355-.235.635-.572a8.18 8.18 0 0 0 .45-.606zm1.64-1.33a12.71 12.71 0 0 1 1.01-.193 11.744 11.744 0 0 1-.51-.858 20.801 20.801 0 0 1-.5 1.05zm2.446.45c.15.163.296.3.435.41.24.19.407.253.498.256a.107.107 0 0 0 .07-.015.307.307 0 0 0 .094-.125.436.436 0 0 0 .059-.2.095.095 0 0 0-.026-.063c-.052-.062-.2-.152-.518-.209a3.876 3.876 0 0 0-.612-.053zM8.078 7.8a6.7 6.7 0 0 0 .2-.828c.031-.188.043-.343.038-.465a.613.613 0 0 0-.032-.198.517.517 0 0 0-.145.04c-.087.035-.158.106-.196.283-.04.192-.03.469.046.822.024.111.054.227.09.346z"/>
                              </svg></i>`;
                        contenido += `</button>`;
                    } else {
                        contenido += `<button class="btn">`;
                        //contenido += `<i id="id${+ obj[slug]}" class="btn btn-light">
                        contenido += `<i id="id${+ obj[slug]}" class="btn btn-secondary">
                              <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" class="bi bi-file-earmark-pdf" viewBox="0 0 16 16">
                              <path d="M14 14V4.5L9.5 0H4a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2zM9.5 3A1.5 1.5 0 0 0 11 4.5h2V14a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1h5.5v2z"/>
                              <path d="M4.603 14.087a.81.81 0 0 1-.438-.42c-.195-.388-.13-.776.08-1.102.198-.307.526-.568.897-.787a7.68 7.68 0 0 1 1.482-.645 19.697 19.697 0 0 0 1.062-2.227 7.269 7.269 0 0 1-.43-1.295c-.086-.4-.119-.796-.046-1.136.075-.354.274-.672.65-.823.192-.077.4-.12.602-.077a.7.7 0 0 1 .477.365c.088.164.12.356.127.538.007.188-.012.396-.047.614-.084.51-.27 1.134-.52 1.794a10.954 10.954 0 0 0 .98 1.686 5.753 5.753 0 0 1 1.334.05c.364.066.734.195.96.465.12.144.193.32.2.518.007.192-.047.382-.138.563a1.04 1.04 0 0 1-.354.416.856.856 0 0 1-.51.138c-.331-.014-.654-.196-.933-.417a5.712 5.712 0 0 1-.911-.95 11.651 11.651 0 0 0-1.997.406 11.307 11.307 0 0 1-1.02 1.51c-.292.35-.609.656-.927.787a.793.793 0 0 1-.58.029zm1.379-1.901c-.166.076-.32.156-.459.238-.328.194-.541.383-.647.547-.094.145-.096.25-.04.361.01.022.02.036.026.044a.266.266 0 0 0 .035-.012c.137-.056.355-.235.635-.572a8.18 8.18 0 0 0 .45-.606zm1.64-1.33a12.71 12.71 0 0 1 1.01-.193 11.744 11.744 0 0 1-.51-.858 20.801 20.801 0 0 1-.5 1.05zm2.446.45c.15.163.296.3.435.41.24.19.407.253.498.256a.107.107 0 0 0 .07-.015.307.307 0 0 0 .094-.125.436.436 0 0 0 .059-.2.095.095 0 0 0-.026-.063c-.052-.062-.2-.152-.518-.209a3.876 3.876 0 0 0-.612-.053zM8.078 7.8a6.7 6.7 0 0 0 .2-.828c.031-.188.043-.343.038-.465a.613.613 0 0 0-.032-.198.517.517 0 0 0-.145.04c-.087.035-.158.106-.196.283-.04.192-.03.469.046.822.024.111.054.227.09.346z"/>
                              </svg></i>`;
                        contenido += `</button>`;
                    }
                    contenido += "</td>";
                }

                if (objConfiguracionGlobal.generar == true) {
                    //let bloqueado = objConfiguracionGlobal.bloqueado; //  Indica si el boton debe estar habilitado o no
                    contenido += "<td style='padding: 2px;' class='option-generar'>";
                    if (obj["bloqueado"] == 0) {
                        contenido += `<button type="button" class="btn btn-primary" onclick="GenerarReporte(${obj[slug]})">Generar</button>`;
                    } else {
                        contenido += `<button type="button" class="btn btn-primary disabled">Generar</button>`;
                    }
                    contenido += "</td>";
                }

                if (objConfiguracionGlobal.arqueo == true) {
                    contenido += "<td style='padding: 2px;' class='option-arqueo'>";
                    if (obj["permisoArqueo"] == 0) {
                        contenido += `<button type="button" class="btn btn-primary" onclick="GenerarArqueo(${obj[slug]})">Arqueo</button>`;
                    } else {
                        contenido += `<button type="button" class="btn btn-primary disabled">Arqueo</button>`;
                    }
                    contenido += "</td>";
                }

                if (objConfiguracionGlobal.revision == true) {
                    contenido += "<td style='padding: 2px;' class='option-revision'>";
                    if (obj["permisoCorregir"] == 1) {
                        contenido += `<button type="button" class="btn btn-primary" onclick="${objConfiguracionGlobal.funcionrevision}(${obj[slug]})">${objConfiguracionGlobal.nombrecolumnarevision}</button>`;
                    } else {
                        contenido += `<button type="button" class="btn btn-secondary">${objConfiguracionGlobal.nombrecolumnarevision}</button>`;
                    }
                    contenido += "</td>";
                }

                if (objConfiguracionGlobal.aceptar == true) {
                    contenido += "<td style='padding: 2px;' class='option-aceptar'>";
                    if (obj[slug] != 0) {
                        contenido += `<button class="btn">`;
                        contenido += `<i onclick="click${objConfiguracionGlobal.funcionaceptar}(${obj[slug]})" id="ok${+ obj[slug]}" class="btn btn-outline-primary">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="22" height="22" fill="currentColor" class="bi bi-check-circle" viewBox="0 0 16 16">
                                  <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                                  <path d="M10.97 4.97a.235.235 0 0 0-.02.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-1.071-1.05z"/>
                                  </svg>
                                  </i>`;
                        contenido += `</button>`;
                    } else {
                        contenido += `<button class="btn">`;
                        contenido += `<i id="ok${+ obj[slug]}" class="btn btn-outline-secondary">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="22" height="22" fill="currentColor" class="bi bi-check-circle" viewBox="0 0 16 16">
                                  <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                                  <path d="M10.97 4.97a.235.235 0 0 0-.02.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-1.071-1.05z"/>
                                  </svg>
                                  </i>`;
                        contenido += `</button>`;
                    }
                    contenido += "</td>";
                }


                if (objConfiguracionGlobal.actualizar == true) {
                    contenido += "<td style='padding: 2px;' class='option-aceptar'>";
                    if (obj["permisoActualizar"] == 1) {
                        contenido += `<button class="btn">`;
                        contenido += `<i onclick="clickActualizar${objConfiguracionGlobal.funcionactualizar}(${obj[slug]})" id="update${+ obj[slug]}" class="btn btn-outline-primary">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="22" height="22" fill="currentColor" class="bi bi-check-circle" viewBox="0 0 16 16">
                                  <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                                  <path d="M10.97 4.97a.235.235 0 0 0-.02.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-1.071-1.05z"/>
                                  </svg>
                                  </i>`;
                        contenido += `</button>`;
                    } else {
                        contenido += `<button class="btn">`;
                        contenido += `<i id="ok${+ obj[slug]}" class="btn btn-outline-secondary">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="22" height="22" fill="currentColor" class="bi bi-check-circle" viewBox="0 0 16 16">
                                  <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                                  <path d="M10.97 4.97a.235.235 0 0 0-.02.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-1.071-1.05z"/>
                                  </svg>
                                  </i>`;
                        contenido += `</button>`;
                    }
                    contenido += "</td>";
                }

                //let pdfvalue = objConfiguracionGlobal.pdfvalue;

                if (objConfiguracionGlobal.excel == true) {
                    contenido += "<td style='padding: 2px;' class='option-excel'>";
                    if (excelvalue != "none") {
                        if (obj[excelvalue] == 1) {
                            contenido += `<button class="btn">`;
                            contenido += `<i onclick="GenerarExcel${objConfiguracionGlobal.funcionexcel}(${obj[slug]})" id="idexcel${+ obj[slug]}" class="btn btn-light" style="font-size:22px; color:green">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="28" height="28" fill="currentColor" class="fa fa-file-excel-o" viewBox="0 0 16 16">
                                  <path d="M5.18 4.616a.5.5 0 0 1 .704.064L8 7.219l2.116-2.54a.5.5 0 1 1 .768.641L8.651 8l2.233 2.68a.5.5 0 0 1-.768.64L8 8.781l-2.116 2.54a.5.5 0 0 1-.768-.641L7.349 8 5.116 5.32a.5.5 0 0 1 .064-.704z" />
                                  <path d="M4 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H4zm0 1h8a1 1 0 0 1 1 1v12a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1z" />
                                  </svg>
                                  </i>`;
                            contenido += `</button>`;
                        }
                        else {
                            //contenido += `<i id="id${+ obj[slug]}" class="btn btn-light" style="font-size:22px; color:green">
                            //      </i>`;
                        }

                    } else {
                        //contenido += `<i id="id${+ obj[slug]}" class="btn btn-light" style="font-size:22px; color:green">
                        //          </i>`;
                    }
                    contenido += "</td>";
                }

                if (objConfiguracionGlobal.web == true) {
                    contenido += "<td style='padding: 2px;' class='option-excel'>";
                    if (webvalue != "none") {
                        if (obj[webvalue] == 1) {
                            contenido += `<button class="btn">`;
                            contenido += `<i onclick="GenerarHTML(${obj[slug]})" id="idweb${+ obj[slug]}" class="btn btn-light" style="font-size:22px; color:red">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="28" height="28" fill="currentColor" class="bi bi-filetype-html" viewBox="0 0 16 16">
                                  <path fill-rule="evenodd" d="M14 4.5V11h-1V4.5h-2A1.5 1.5 0 0 1 9.5 3V1H4a1 1 0 0 0-1 1v9H2V2a2 2 0 0 1 2-2h5.5L14 4.5Zm-9.736 7.35v3.999h-.791v-1.714H1.79v1.714H1V11.85h.791v1.626h1.682V11.85h.79Zm2.251.662v3.337h-.794v-3.337H4.588v-.662h3.064v.662H6.515Zm2.176 3.337v-2.66h.038l.952 2.159h.516l.946-2.16h.038v2.661h.715V11.85h-.8l-1.14 2.596H9.93L8.79 11.85h-.805v3.999h.706Zm4.71-.674h1.696v.674H12.61V11.85h.79v3.325Z"/>
                                  </svg>
                                  </i>`;
                            contenido += `</button>`;
                        }
                        else {
                            //contenido += `<i id="id${+ obj[slug]}" class="btn btn-light" style="font-size:22px; color:red">
                            //      </i>`;
                        }
                    } else {
                        //contenido += `<i id="id${+ obj[slug]}" class="btn btn-light" style="font-size:22px; color:red">
                        //          </i>`;
                    }
                    contenido += "</td>";
                }

            }
            contenido += "</tr>";
        }

        contenido += "</tbody>";
        if (objConfiguracionGlobal.sumarcolumna == true) {
            contenido += "<tfoot>"
            contenido += "<tr>"
            for (let j = 0; j < countColumns; j++) {
                contenido += "<td class='table-secondary'>"
                contenido += "</td>"
            }
            contenido += "</tr>"
        }
        contenido += "</tfoot>"
        contenido += "</table>";
    }

    return contenido;

}

function pintarRadioButtonList(objConfiguracion, res) {
    if (objConfiguracion.divContenedorTabla == undefined)
        objConfiguracion.divContenedorTabla == "divContainerTablaRadioButtonList";
    if (objConfiguracion.divPintado == undefined)
        objConfiguracion.divPintado == "divTablaRadioButtonList";

    var contenido = "";
    contenido += "<div id='" + objConfiguracion.divContenedorTabla + "'>";
    contenido += FillRadioButtonList(res, objConfiguracion);
    contenido += "</div>";
    setI(objConfiguracion.divPintado, contenido);
}

function FillRadioButtonList(data, objConfiguracion) {
    let contenido = "";
    try {
        let propiedadid = objConfiguracion.propiedadid;
        let propiedadnombre = objConfiguracion.propiedadnombre;
        let propiedades = objConfiguracion.propiedades;
        let objActual;

        contenido += "<table id='" + propiedadid + "' border='0'>";
        contenido += "<tr>";
        for (let i = 0; i < data.length; i++) {
            contenido += "<td style='padding: 0.5rem;'>";
            objActual = data[i];
            contenido += "<input type='radio' id='" + propiedadid + "_" + i.toString() + "' name='" + propiedadnombre + "' value = '" + objActual[propiedades[0]] + "' checked />"
            contenido += "<label for= '" + propiedadid + "_" + i.toString() + "'><span style='padding: 8px;'>" + objActual[propiedades[1]] + "</span></label>";
            contenido += "</td>";
        }
        contenido += "</tr>";
        contenido += "<tr>";
        for (let i = 0; i < data.length; i++) {
            contenido += "<td style='padding: 0.5rem;'>";
            objActual = data[i];
            contenido += "<label><span style='padding: 8px;'>" + objActual[propiedades[0]] + "</span></label>";
            contenido += "</td>";
        }
        contenido += "</tr>";
        contenido += "</table>";
    }
    catch (e) {
        alert("Ocurrió un error GET (" + idcontrol + ")-" + e.MensajeError);
    }

    return contenido;
}

function pintarRadioButtonListWithoutLabel(objConfiguracion, res) {
    if (objConfiguracion.divContenedorTabla == undefined)
        objConfiguracion.divContenedorTabla == "divContainerTablaRadioButtonList";
    if (objConfiguracion.divPintado == undefined)
        objConfiguracion.divPintado == "divTablaRadioButtonList";

    var contenido = "";
    contenido += "<div id='" + objConfiguracion.divContenedorTabla + "'>";
    contenido += FillRadioButtonListWithoutLabel(res, objConfiguracion);
    contenido += "</div>";
    setI(objConfiguracion.divPintado, contenido);
}

function FillRadioButtonListWithoutLabel(data, objConfiguracion) {
    let contenido = "";
    try {
        let propiedadid = objConfiguracion.propiedadid;
        let propiedadnombre = objConfiguracion.propiedadnombre;
        let propiedades = objConfiguracion.propiedades;
        let objActual;

        contenido += "<table id='" + propiedadid + "' border='0'>";
        contenido += "<tr>";
        for (let i = 0; i < data.length; i++) {
            contenido += "<td style='padding: 0.5rem;'>";
            objActual = data[i];
            contenido += "<input type='radio' id='" + propiedadid + "_" + i.toString() + "' name='" + propiedadnombre + "' value = '" + objActual[propiedades[0]] + "' checked />"
            contenido += "<label for= '" + propiedadid + "_" + i.toString() + "'><span style='padding: 8px;'>" + objActual[propiedades[1]] + "</span></label>";
            contenido += "</td>";
        }
        contenido += "</tr>";
        contenido += "</table>";
    }
    catch (e) {
        alert("Ocurrió un error GET (" + idcontrol + ")-" + e.MensajeError);
    }

    return contenido;
}


function MensajeError(titulo = "Error", texto="Ocurrió un error") {
    Swal.fire({
        icon: 'error',
        title: titulo,
        text: texto
    })
}

function Confirmacion(titulo="Confirmación", texto="¿Desea guardar los cambios?", callback) {
 return Swal.fire({
        title: titulo,
        text: texto,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: "Si",
        cancelButtonText: "No"
    }).then((result) => {
        if (result.isConfirmed) {
            callback();
        }
    })
}

function Exito(nameController, nameAction, enable, titulo = "Se guardó correctamente") {
    Swal.fire({
        position: 'top-end',
        icon: 'success',
        title: titulo,
        showConfirmButton: false,
        timer: 1500
    }).then(function () {
        if (enable == true) {
            Redireccionar(nameController, nameAction);
        }
    })
}

function Warning(titulo = "Advertencia") {
    Swal.fire(titulo);
}

/*const searchButton = document.getElementById('search-button');
const searchInput = document.getElementById('search-input');
searchButton.addEventListener('click', () => {
    const inputValue = searchInput.value;
    alert(inputValue);
});*/

function LimpiarDatos(idFormulario) {
    let elementsName = document.querySelectorAll("#" + idFormulario + "[name]");
    let elementoActual;
    let elementoName;
    for (let i = 0; i < elementoName.length; i++) {
        elementoActual = elementoName[i]
        elementoName = elementoActual.name;
        if (elementoActual.tagName.toUpperCase() == "SELECT") {
            document.getElementById(elementoActual.id).selectedIndex = 0;
        } else {
            setN(elementoName, "", idFormulario);
        }
    }
}

function validarKeyPress(idFormulario) {
    // Se recorre todos los controles
    let elementosNames = document.querySelectorAll("#" + idFormulario + " [name]");
    let control, nombreclases, clases, resultado;
    for (let i = 0; i < elementosNames.length; i++) {
        control = elementosNames[i];
        //form-control ob (se obtiene la clase completa)
        nombreclases = control.className;
        //["form-control","ob"]
        clases = nombreclases.split(" ");
        // solo numero decimal con 2 decimales
        resultado = clases.filter(p => p == "decimal-2")
        if (resultado.length > 0) {
            elementosNames[i].onkeypress = function (e) {
                let cadena = e.target.value + String.fromCharCode(e.keycode)
                //alert(cadena);
                if (!/^\d*(\.\d{1})?\d{0,1}$/.test(cadena)) {
                    e.preventDefault();
                }
            }
        }
    }
}

function ValidarDatos(idFormulario) {
    let error = "";
    // Se recorre todos los controles
    let elementosNames = document.querySelectorAll("#" + idFormulario + " [name]");
    let control;
    let nombreclases;
    let clases;
    let resultado;
    //let nameControl;
    for (let i = 0; i < elementosNames.length; i++) {
        control = elementosNames[i];
        //form-control ob (se obtiene la clase completa)
        nombreclases = control.className;
        //["form-control","ob"]
        clases = nombreclases.split(" ");
        resultado = clases.filter(p => p == "obligatorio")
        if (resultado.length > 0) {
            if (control.tagName.toUpperCase() == "INPUT" || control.tagName.toUpperCase() == "TEXTAREA") {
                if (control.value.trim() == "") {
                    error = "Debe ingresar el campo " + control.name;
                }
            } else {
                if (control.tagName.toUpperCase() == "SELECT") {
                    //if (control.selectedIndex == -1) {
                    if (control.value == "-1" || control.value == "0") {
                        error = "Debe seleccionar el campo " + control.name;
                    }
                }
            }
        }

        //Radio Button (Selector CSS)
        /*let radios = document.querySelectorAll("#" + idFormulario + " [type='radio']");
        for (let i = 0; i < radios.length; i++) {
            radios[i].checked = false
        }*/

        // Solo numeros obligatorios
        resultado = clases.filter(p => p == "solonumeros")
        if (resultado.length > 0) {
            if (!/^[0-9]+$/.test(control.value)) {
                error = "el campo " + control.name + " solo debe tener números del 0 al 9";
            }
        }

        // Solo numeros opcionales
        resultado = clases.filter(p => p == "solonumeros-opcional")
        if (resultado.length > 0) {
            if (!/^[0-9]+$/.test(control.value)) {
                if (control.value != "") {
                    error = "el campo " + control.name + " solo debe tener números del 0 al 9";
                }
            }
        }

        // solo numero decimal con 2 decimales
        resultado = clases.filter(p => p == "decimal-2")
        if (resultado.length > 0) {
            if (!/^\d*(\.\d{1})?\d{0,1}$/.test(control.value)) {
                error = "el campo " + control.name + " el cambo debe ser de 2 decimales";
            }
        }

        // seleccionar un registro de un conjunto de registros de una tabla
        /*resultado = clases.filter(p => p == "table-row-selected")
        if (resultado.length > 0) {

            let table = document.getElementById("table");
            let inputs = table.getElementsByTagName("input");
            let selected = false;
            let rowChecked = false;
            for (let i = 0; i < inputs.length; i++) {
                if (inputs[i].type == 'radio') {
                    rowChecked = inputs[i].checked;
                    if (rowChecked) {
                        selected = true;
                        break;
                    }
                }
            }
            if (selected = false)
                error = "Seleccione el nombre de la entidad del listado";
        }*/


    }


    return error;
}


function Redireccionar(nameController, nameAction) {
    let url = nameController + "/" + nameAction;
    let raiz = document.getElementById("hdfOculto").value;
    let urlCompleta = window.location.protocol + "//" + window.location.host + "/" + raiz
        + url
    window.location.href = urlCompleta;
}


function unCheckAll(bx) {
    var cbs = document.getElementsByTagName('input');
    for (var i = 0; i < cbs.length; i++) {
        if (cbs[i].type == 'checkbox') {
            cbs[i].checked = bx.checked;
        }
    }
}


function ValidarSeleccionRow(idTabla) {
    let table = document.getElementById(idTabla);
    let inputs = table.getElementsByTagName("input");
    let selected = false;
    let rowChecked = false;
    for (let i = 0; i < inputs.length; i++) {
        if (inputs[i].type == 'radio') {
            rowChecked = inputs[i].checked;
            if (rowChecked) {
                selected = true;
                break;
            }
        }
    }
    return selected;
}

function getFechaSistema() {
    let fechaSistemaStr = document.getElementById("uiFechaSistema").value;
    let arr = fechaSistemaStr.split('-');
    let anio = parseInt(arr[0]);
    let mes = parseInt(arr[1]);
    let dia = parseInt(arr[2]);
    data = [{ "dia": dia, "mes": mes, "anio": anio }]
    return data;
}

