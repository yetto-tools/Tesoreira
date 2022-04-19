window.onload = function () {
    ListarCategoriaEntidades();
}

var objCategoriaEntidad;

function ListarCategoriaEntidades()
{
    objCategoriaEntidad = {
        url: "EntidadCategoria/GetAllCategoriaEntidades",
        divPintador = "divContenedor",
        cabeceras: ["codigo", "nombre", "descripcion", "estado"],
        propiedades: ["codigoCategoriaEntidad", "nombre", "descripcion", "estado"]
    }
    pintar(objCategoriaEntidad);

}

function BuscarCategoriaEntidad()
{
    let nombreCategoriaEntidad = get("uiNombreBusqueda");
    objCategoriaEntidad.url = "EntidadCategoria/filtrarCategoriaEntidades/?nombreCategoria=" + nombreCategoriaEntidad;
    pintar(objCategoriaEntidad);
}

function LimpiarCategoriaEntidad()
{
    ListarCategoriaEntidades();
    set("uiNombreBusqueda", "");

}