using CapaDatos.RRHH;
using CapaEntidad.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.RRHH
{
    public class PersonaBL
    {
        public List<PersonaCLS> GetAllPersonas(int noIncluidoEnPlanilla)
        {
            PersonaDAL obj = new PersonaDAL();
            return obj.GetAllPersonas(noIncluidoEnPlanilla);
        }
        public PersonaCLS GetDataPersona(string cui)
        {
            PersonaDAL obj = new PersonaDAL();
            return obj.GetDataPersona(cui);
        }

        public string GuardarPersona(PersonaCLS objPersona, string usuarioIng)
        {
            PersonaDAL obj = new PersonaDAL();
            return obj.GuardarPersona(objPersona, usuarioIng);
        }

        public string GuardarPersonaIndirecta(PersonaCLS objPersona, string usuarioIng)
        {
            PersonaDAL obj = new PersonaDAL();
            return obj.GuardarPersonaIndirecta(objPersona, usuarioIng);
        }

        public string ActualizarPersona(PersonaCLS objPersona, string usuarioAct)
        {
            PersonaDAL obj = new PersonaDAL();
            return obj.ActualizarPersona(objPersona, usuarioAct);
        }

    }
}
