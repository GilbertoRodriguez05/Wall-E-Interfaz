using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public abstract class AST
    {
        public abstract bool SemanticCheck(List<Error> errors, Entorno entorno);
        public abstract void Execute();
    }
}