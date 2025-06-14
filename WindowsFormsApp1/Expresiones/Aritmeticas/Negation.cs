using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Expresiones.Aritmeticas
{
    internal class Negation : Expresions
    {
        public override object value { get; set; }

        public Negation(int tvalue)
        {
            this.value = -tvalue; //El valor que se pase se cambia de signo
        }

        public override void Execute()
        {

        }

        public override bool SemanticCheck(List<Error> errors, Entorno entornopasado)
        {
            return true;
        }

        public override ExpresionsTypes Type(Entorno entorno)
        {
            return ExpresionsTypes.Numero;
        }
    }
}
