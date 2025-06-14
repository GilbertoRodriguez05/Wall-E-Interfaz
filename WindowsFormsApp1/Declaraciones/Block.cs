using System.Collections.Generic;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public class Block : AST
    {
        public List<AST> declarations { get; }
        public Block(List<AST> declarations)
        {
            this.declarations = declarations;
        }
        public override void Execute()
        {
            foreach (AST item in declarations)
            {
                item.Execute();
                if(item is GoTo gotu)
                {
                    bool jumpOccurred = PilaCompartida.Pop(); // Always pop first
                    if (jumpOccurred)
                    {
                        break; // Exit block early only if THIS GoTo caused a jump
                    }
                }
                
            }
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            foreach (AST item in declarations)
            {
                bool valid = item.SemanticCheck(errors, entorno);
                if (!valid) return false;
            }
            return true;
        }
    }
}

public static class PilaCompartida
{
    private static Stack<bool> _pila = new Stack<bool>();

    public static void Push(bool item)
    {
        _pila.Push(item);
    }

    public static void Clear() => _pila.Clear();

    public static bool Pop()
    {
        return _pila.Pop();
    }

    public static bool Peek()
    {
        return _pila.Peek();
    }

    public static int Count
    {
        get { return _pila.Count; }
    }
}