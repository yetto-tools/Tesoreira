window.onload = function () {
    listarClientes();
}

function listarClientes() {
    pintar({
        url: "Cliente/listarClientes",
        cabeceras: ["empresa", "codigo cliente", "nombre", "nit", "codigo vendedor"],
        propiedades: ["codigoEmpresa", "codigo", "nombre", "nit", "codigoVendedor"]
    })
}