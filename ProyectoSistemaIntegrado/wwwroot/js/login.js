function Ingresar() {
    let idIsuario = getN("Usuario");
    let contrasenia = getN("Contrasenia");
    fetchGet("Login/login?Usuario=" + idIsuario + "&contrasenia=" + contrasenia, "json", function (data) {
        if (data.idUsuario == null) {
            MensajeError("Usuario o contraseña incorrecta");
        } else {
            //Exito("Bienvenido al sistema");
            document.location.href = setURL("Home/Index");
        }
    })
}


function IngresarKeyPress(){
    document.onkeypress = function (event) {
        if (event.key === "Enter") {
            Ingresar();
        }
    }
}