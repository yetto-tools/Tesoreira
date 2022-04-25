window.onload = function () {
    let url_string = window.location.href;
    let url = window.location.pathname.split('/');
    let nameController = url[1];
    let nameAction = url[2];
    let url1 = new URL(url_string);
    if (nameController == "SiteMap") {
        switch (nameAction) {
            case "Index":
                MostrarConfiguracionMenu(-1,-1);
                break;
            case "Roles":
                MostrarRoles();
                break;
            case "ConfigRoles":
                let codigoRol = parseInt(url1.searchParams.get("codigoRol"));
                MostrarDataRol(codigoRol);
                break;
            default:
                break;
        }// fin switch
    }// fin if
}


function MostrarConfiguracionMenu(codigoSistema, nivel) {
    let objConfiguracion = {
        url: "SiteMap/GetConfiguracion/?codigoSistema=" + codigoSistema.toString() + "&nivel=" + nivel.toString(),
        cabeceras: ["Código", "Código Sistema", "Sistema", "Título", "Descripción", "Controller", "Action", "Nivel", "Nivel", "Padre", "Editar", "Anular"],
        propiedades: ["codigoSitemap", "codigoSistema", "nombreSistema", "titulo", "descripcion", "nombreController", "nombreAction", "nivel", "nombreNivel", "tituloPadre", "permidoEditar", "permisoAnular"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        editar: true,
        funcioneditar: "ItemMenu",
        eliminar: true,
        funcioneliminar: "ItemMenu",
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [1],
                "visible": false
            }, {
                "targets": [7],
                "visible": false
            }, {
                "targets": [10],
                "visible": false
            }, {
                "targets": [11],
                "visible": false
            }],
        slug: "codigoSitemap"
    }
    pintar(objConfiguracion);
}


function AgregarNuevoItem() {
    setI("uiTitlePopupNewItem", "Nuevo Item");
    fillComboSistema("uiNewCodigoSistema");
    fillComboSitemapPadre("uiNewCodigoSitemapPadre", -1, 2);
    set("uiNewTitulo", "");
    set("uiNewDescripcion", "");
    set("uiNewNombreController", "");
    set("uiNewNombreAction", "");
    document.getElementById("ShowPopupNewItem").click();
}

function GuardarItemMenu() {
    let errores = ValidarDatos("frmNewItemMenu")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmNewItemMenu");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("SiteMap/GuardarItemMenu", "text", frm, function (data) {
            if (data == "OK") {
                document.getElementById("uiClosePopupNewItem").click();
                MostrarConfiguracionMenu(-1, -1);
            } else {
                MensajeError(data);
            }
        })
    })
}

function fillComboSistema(idControl) {
    fetchGet("Sistema/GetAllSistemas", "json", function (data) {
        if (data == null || data == undefined) {
            FillComboUnicaOpcion(idControl, "-1", "-- No Existen sistemas -- ");
        } else {
            FillCombo(data, idControl, "codigoSistema", "nombreSistema", "- seleccione -", "-1");
        }
    })
}

function onChangeComboSistema(obj) {
    let codigoSistema = parseInt(obj.value);
    let nivel = parseInt(document.getElementById("uiNewNivel").value);
    fillComboSitemapPadre("uiNewCodigoSitemapPadre", codigoSistema, nivel);
}

function onChangeEditComboSistema(obj) {
    let codigoSistema = parseInt(obj.value);
    let nivel = parseInt(document.getElementById("uiEditNivel").value);
    fillComboSitemapPadre("uiEditCodigoSitemapPadre", codigoSistema, nivel);
}

function onChangeNivel(obj) {
    let nivel = parseInt(obj.value);
    let codigoSistema = parseInt(document.getElementById("uiNewCodigoSistema").value);
    fillComboSitemapPadre("uiNewCodigoSitemapPadre", codigoSistema, nivel);
}

function onChangeEditNivel(obj) {
    let nivel = parseInt(obj.value);
    let codigoSistema = parseInt(document.getElementById("uiEditCodigoSistema").value);
    fillComboSitemapPadre("uiEditCodigoSitemapPadre", codigoSistema, nivel);
}

function fillComboSitemapPadre(idControl, codigoSistema, nivel) {
    if (nivel == 0) {
        FillComboUnicaOpcion(idControl, "14", "-- Raiz -- ");
    } else {
        fetchGet("SiteMap/GetSiteMapsPadre/?codigoSistema=" + codigoSistema.toString() + "&nivel=" + nivel.toString(), "json", function (data) {
            if (data == null || data == undefined) {
                FillComboUnicaOpcion(idControl, "-1", "-- No Existen datos -- ");
            } else {
                FillCombo(data, idControl, "codigoSitemap", "titulo", "- seleccione -", "-1");
            }
        })
    }
}


function EditarItemMenu(obj) {
    let codigoSiteMap = parseInt(obj);
    setI("uiTitlePopupEditItem", "Edición Item Menú");
    document.getElementById("ShowPopupEditItem").click();
    fetchGet("Sistema/GetAllSistemas", "json", function (data) {
        if (data == null || data == undefined) {
            FillComboUnicaOpcion("uiEditCodigoSistema", "-1", "-- No Existen sistemas -- ");
        } else {
            FillCombo(data, "uiEditCodigoSistema", "codigoSistema", "nombreSistema", "- seleccione -", "-1");
            fetchGet("SiteMap/GetSiteMapsPadre/?codigoSistema=-1&nivel=2", "json", function (data) {
                if (data == null || data == undefined) {
                    FillComboUnicaOpcion("uiEditCodigoSitemapPadre", "-1", "-- No Existen datos -- ");
                } else {
                    FillCombo(data, "uiEditCodigoSitemapPadre", "codigoSitemap", "titulo", "- seleccione -", "-1");
                    fetchGet("SiteMap/GetDataItemMenu/?codigoSiteMap=" + codigoSiteMap.toString(), "json", function (data) {
                        if (data != null) {
                            set("uiEditCodigoSiteMap", data.codigoSitemap);
                            document.querySelector('#uiEditCodigoSistema').value = data.codigoSistema;
                            set("uiEditTitulo", data.titulo);
                            set("uiEditDescripcion", data.descripcion);
                            set("uiEditNombreController", data.nombreController);
                            set("uiEditNombreAction", data.nombreAction);
                            document.querySelector('#uiEditNivel').value = data.nivel;
                            document.querySelector('#uiEditCodigoSitemapPadre').value = data.codigoSitemapPadre;

                        } else {
                            MensajeError("Error en la búsqueda de información");
                        }
                    })
                }
            })
        }
    })
}

function ActualizarItemMenu() {
    let errores = ValidarDatos("frmEditItemMenu")
    if (errores != "") {
        MensajeError(errores);
        return;
    }
    let frmGuardar = document.getElementById("frmEditItemMenu");
    let frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("SiteMap/ActualizarItemMenu", "text", frm, function (data) {
            if (data == "OK") {
                document.getElementById("uiClosePopupEditItem").click();
                MostrarConfiguracionMenu(-1, -1);
            } else {
                MensajeError(data);
            }
        })
    })
}


function EliminarItemMenu(obj) {
    let codigoSiteMap = parseInt(obj);
    Confirmacion("Anulación de Elemento del Menu", "¿Está seguro de eliminar este Elemento del Menu?", function (rpta) {
        fetchGet("SiteMap/AnularItemMenu/?codigoSitemap=" + codigoSiteMap.toString(), "text", function (data) {
            if (data == "OK") {
                MostrarConfiguracionMenu(-1, -1);
            } else {
                MensajeError(data);
            }
        });
    });
}


function MostrarRoles() {
    let objConfiguracion = {
        url: "SiteMap/GetAllRoles",
        cabeceras: ["Código", "Nombre Rol", "Descripción","Editar","Anular"],
        propiedades: ["codigoRol", "nombre", "descripcion","permisoEditar","permisoAnular"],
        divContenedorTabla: "divContenedorTabla",
        divPintado: "divTabla",
        editar: true,
        funcioneditar: "ConfiguracionRol",
        eliminar: true,
        paginar: true,
        ocultarColumnas: true,
        hideColumns: [
            {
                "targets": [3],
                "visible": false
            }, {
                "targets": [4],
                "visible": false
            }],
        slug: "codigoRol"
    }
    pintar(objConfiguracion);
}

function EditarConfiguracionRol(obj) {
    let codigoRol = parseInt(obj);
    Redireccionar("SiteMap", "ConfigRoles/?codigoRol=" + codigoRol.toString());
}

function MostrarDataRol(codigoRol) {
    fetchGet("SiteMap/GetDataRol/?codigoRol=" + codigoRol, "json", function (data) {
        if (data == null || data == undefined || data.length == 0) {
            MensajeError("Error en la búsqueda de datos del rol");
        } else {
            set("uiCodigoRol", data.codigoRol);
            set("uiNombreRol", data.nombre);
            set("uiDescripcionRol", data.descripcion);
        }
    });
}


function clickSelectItemTree() {
    let contador = 0;
    $('#data').on("changed.jstree", function (e, data) {
        let texto = data.node.text;
        let valor = data.node.li_attr.value;

        alert('The selected node is: value: ' + valor + ' text: ' + texto);
        /*if (data.selected.length) {
            $(data.selected).each(function (idx) {
                let node = data.instance.get_node(data.selected[idx]);
                let valor = data.node.li_attr.value;
                alert('The selected node is: value: ' + valor + ' text: ' + node.text + ' contador: ' + contador.toString());
                contador = contador + 1;
            });
        }*/
    });
}

function clickMostrarCodigos() {
    var selectedNode = $("#jstree").jstree("get_selected");
    var node_info = $('#jstree').jstree("get_node", selectedNode[0]);

    // this node_info contains **children_d** an array of all child nodes' id .
    // **parents** an array of parent nodes
    alert(node_info.children_d.join(','));
    alert(node_info.parents.join(','));
    alert(node_info.id);

    //var childrens = $("#jstree").jstree("get_children_dom", selectedNode);
    //for (var i = 0; i < childrens.length; i++) {
    //    alert(childrens[i].innerText);
    //}

    //$('#jstree').on("select_node.jstree", function (e, data) { alert("node_id: " + data.node.id); });

    //filter(function () { return $(this).parents('li.jstree-checked').length == 0; }).map(function () { return this.id; }).get();
    
    /*var instance = $('#jstree').jstree(true);
    selected = instance.get_selected()[0];
    console.log(instance.get_node(selected).children);*/
    


    


    //$("#jstree").jstree("select_node", ["list of nodes go here"]);



    //let arrayProperties = new Array();


    //let selectedData = [];
    //let selectedIndexes;
    //selectedIndexes = $("#data").jstree("get_selected", true);
    //jQuery.each(selectedIndexes, function (index, value) {
    //    selectedData.push(selectedIndexes[index].id);
    //});
    //alert(selectedData);


    // bind to events triggered on the tree



    //let parents;
    //let selectedNodes = $('#data').jstree(true).get_selected('full', true);
    //let selectedNodes = $('#data').jstree(true).get_selected(true);
    //let selectedNodes = $('#data').jstree("get_selected", true);
    //let selectedNodes = $('#data').jstree(true).get_selected("full", true);
    //let selectedNodes = $('#data').jstree("get_undetermined", true);
    //let selectedNodes = $('#data').jstree("get_top_checked", true);
    

    
    
    //let selectedNodes = $('#data').jstree("get_bottom_checked", true);
    //$.each(selectedNodes, function () {
        //let datos2 = data.instance.get_node(this.parent);
        //let parentNode = $('#data').jstree(true).get_parent(this.parent);
        //select_node(parentNode);
        //arrayProperties.push(this.li_attr.value);
    //});

    //alert(arrayProperties)

    /*let ids = $('#data li.jstree-checked').
        filter(function () {
            return $(this).parents('li.jstree-checked').length == 0;
        })
        .map(function () {
            //return this.id;
            return this.id;
        }).get();

    alert(ids);*/
    //let selectedNodes = $('#data').jstree(true).get_selected(true);
    //for (let i = 0; i < selectedNodes.length; i++) {
        //let datos2 = data.instance.get_node(this.parent);
        //arrayProperties.push(selectedNodes[i].li_attr.value);

        //arrayProperties.push(selectedNode[i].parent);
        //arrayProperties.push(selectedNode[i].text);
        //id.push(selectedNode.id);
        //parent.push(selectedNode.parent);
        //text.push(selectedNode.text);
    //}
    //alert(arrayProperties)
    //arrayProperties.push(selectedNode.parent);

    /*let contador = 1;
    let arrayProperties = new Array();
    $('#data').bind('ready.jstree', function (e, data) {
        $(data.selected).each(function (idx) {
            let valor = data.node.li_attr.value;
            arrayProperties.push(valor);
            contador = contador + 1;
        });
    });*/
    /*$('#data').on('activate_node.jstree', function (e, data) {
        if (data.instance.is_leaf(data.node)) {
            alert("Leaf: " + data.node.text);
            alert("Parent: " + data.instance.get_node(data.node.parent).text);
        }
    });*/

    //alert(contador.toString());
}
