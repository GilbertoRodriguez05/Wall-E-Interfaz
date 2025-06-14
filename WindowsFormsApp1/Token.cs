
namespace WindowsFormsApp1
{
    public class Token
    {
        public TokenTypes types;
        public string lexeme;
        public object literal;
        public int line;

        public Token(TokenTypes types, string lexeme, object literal, int line)
        {
            this.types = types;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }

        public string Tostring()
        {
            return types + " " + lexeme + " " + literal;
        }
    }
    public enum TokenTypes
    {
        Suma, Resta, Multiplicacion, Division, Potencia, Modulo, Espacio, Coma,

        Mayor, MayorIgual, Menor, MenorIgual, Igual, Equal, Declaracion, Distinto, Negacion, And, Or, AbreParentesis, CierraParentesis, AbreCorchete, CierraCorchete,

        Identificador, Cadena, Numero, True, False, SaltoLinea, Variable, Label, GoTo, EOF,
        Spawn, Color, Size, DrawLine, DrawCircle, DrawRectangle, Fill, GetActualX, GetActualY, GetCanvasSize, GetColorCount,
        IsBrushColor, IsBrushSize, IsCanvasColor, IsColor
    }
    public enum ExpresionsTypes
    {
        Numero, Cadena, Bool, Funcion, Error
    }
}