
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class Not : Expresions
    {
        public override object value { get; set; }
        public int line;
        public Not(bool value, int line)
        {
            this.value = !value;
            this.line = line;
        }
        public override void Execute()
        {

        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            return true;
        }
        public override ExpresionsTypes Type(Entorno entorno)
        {
            return ExpresionsTypes.Bool;
        }
    }
}