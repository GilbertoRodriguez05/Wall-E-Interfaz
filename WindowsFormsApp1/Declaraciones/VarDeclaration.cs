
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    class VarDeclaration : AST
    {
        public Expresions name { get; set; }
        public Expresions value { get; set; }
        public Token operador { get; set; }
        public Entorno entorno { get; set; }
        public VarDeclaration(Expresions name, Expresions value, Token operador, Entorno entorno)
        {
            this.name = name;
            this.value = value;
            this.operador = operador;
            this.entorno = entorno;
        }
        public override void Execute()
        {
            value.Execute();
            name.Execute();
            entorno.SetValue(name.ToString(), value.value);
            return;    
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            value.SemanticCheck(errors, entorno);
            entorno.SetType(name.ToString(), value.Type(entorno));
            return true;
        }
    }
}