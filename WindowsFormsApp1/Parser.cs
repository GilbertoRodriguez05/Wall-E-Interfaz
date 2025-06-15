using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;
using WindowsFormsApp1.Expresiones.Aritmeticas;

namespace WindowsFormsApp1
{
    class Parser
    {
        Entorno entorno;
        Canvas canvas;
        List<Token> tokens = new List<Token>();
        List<Error> errors = new List<Error>();
        public int Current = 0;
        public string code;
        Spawn spawn;
        bool SpawnCheck;

        public Parser(List<Token> tokens, List<Error> errors, Entorno entorno, Canvas canvas)
        {
            this.tokens = tokens;
            this.errors = errors;
            this.entorno = entorno;
            this.canvas = canvas;
            SpawnCheck = false;
        }

        public AST Main()
        {
            Expresions x = null;
            Expresions y = null;
            if (Match(TokenTypes.Spawn))
            {
                if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba (", 1));

                Expresions expr1 = Expresions();
                x = expr1;

                if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma"));

                Expresions expr2 = Expresions();
                y = expr2;

                if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un parentesis cerrado", 1));
            }
            else
            {
                errors.Add(new Error(TypeOfError.Expected, "Se esperaba Spawn", 1));
            }

            if (!Match(TokenTypes.SaltoLinea)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un salto de linea", 1));
            spawn = new Spawn(x, y, canvas, 1);
            SpawnCheck = true;
            return Block();

        }
        public Expresions Expresions()
        {
            return Or();
        }
        public Expresions Or()
        {
            Expresions expresion = And();
            while (Match(TokenTypes.Or))
            {
                Expresions right = And();
                expresion = new Or(expresion, right, Previous().line);
            }
            return expresion;
        }
        public Expresions And()
        {
            Expresions expresion = Igualdad();
            while (Match(TokenTypes.And))
            {
                Expresions right = Igualdad();
                expresion = new And(expresion, right, Previous().line);
            }
            return expresion;
        }
        public Expresions Igualdad()
        {
            Expresions expresion = Comparation();
            while (Match(TokenTypes.Equal, TokenTypes.Distinto))
            {
                Token Operator = Previous();
                Expresions right = Comparation();
                
                if (Operator.types == TokenTypes.Distinto) expresion = new Diferent(expresion, right, Previous().line);
                else if (Operator.types == TokenTypes.Equal) expresion = new Equal(expresion, right, Previous().line);
            }
            return expresion;
        }
        public Expresions Comparation()
        {
            Expresions expresion = Term();
            while (Match(TokenTypes.Mayor, TokenTypes.Menor, TokenTypes.MayorIgual, TokenTypes.MenorIgual))
            {
                
                Token Operator = Previous();
                Expresions right = Term();
                if (Operator.types == TokenTypes.Mayor) expresion = new GreaterThan(expresion, right, Previous().line);
                else if (Operator.types == TokenTypes.Menor) expresion = new LessThan(expresion, right, Previous().line);
                else if (Operator.types == TokenTypes.MayorIgual) expresion = new GreaterOrEqual(expresion, right, Previous().line);
                else if (Operator.types == TokenTypes.MenorIgual) expresion = new LessOrEqual(expresion, right, Previous().line);
            }
            return expresion;
        }
        public Expresions Term()
        {
            Expresions expresion = Factor();
            while (Match(TokenTypes.Suma, TokenTypes.Resta))
            {
                Token Operator = Previous();
                Expresions right = Factor();
                
                if (Operator.types == TokenTypes.Suma) expresion = new Addition(expresion, right, Previous().line);
                else if (Operator.types == TokenTypes.Resta) expresion = new Substraction(expresion, right, Previous().line);
            }
            return expresion;
        }
        public Expresions Factor()
        {
            Expresions expresion = Exponent();
            while (Match(TokenTypes.Multiplicacion, TokenTypes.Division))
            {
                Token Operator = Previous();
                Expresions right = Exponent();
                
                if (Operator.types == TokenTypes.Multiplicacion) expresion = new Multiplication(expresion, right, Previous().line);
                else if (Operator.types == TokenTypes.Division) expresion = new Divition(expresion, right, Previous().line);
            }
            return expresion;
        }
        public Expresions Exponent()
        {
            Expresions expresions = Negation();
            while (Match(TokenTypes.Potencia, TokenTypes.Modulo))
            {
                Token Operator = Previous();
                Expresions right = Negation();

                if (Operator.types == TokenTypes.Potencia) expresions = new Pow(expresions, right, Previous().line);
                else if (Operator.types == TokenTypes.Modulo) expresions = new Module(expresions, right, Previous().line);
            }
            return expresions;
        }
        public Expresions Negation()
        {
            if (Match(TokenTypes.Negacion, TokenTypes.Resta))
            {
                Token operador = Previous();
                Expresions right = Negation();

                if (operador.types == TokenTypes.Negacion) return new Not((bool)right.value, Previous().line);
                else if (operador.types == TokenTypes.Resta)
                {
                    return new Negation(Convert.ToInt32(right.value), Previous().line);
                }
            }
            return Primary();
        }
        public Expresions Primary()
        {
            if (Match(TokenTypes.Numero)) return new Numero(Convert.ToInt32(Previous().literal));
            else if (Match(TokenTypes.Cadena)) return new Cadena(Previous().lexeme);
            else if (Match(TokenTypes.GetActualX)) return GetActualX();
            else if (Match(TokenTypes.GetActualY)) return GetActualY();
            else if (Match(TokenTypes.GetCanvasSize)) return GetCanvasSize();
            else if (Match(TokenTypes.GetColorCount)) return GetColorCount();
            else if (Match(TokenTypes.IsBrushColor)) return IsBrushColor();
            else if (Match(TokenTypes.IsBrushSize)) return IsBrushSize();
            else if (Match(TokenTypes.IsCanvasColor)) return IsCanvasColor();
            else if (Match(TokenTypes.True)) return new Booleano(true);
            else if (Match(TokenTypes.False)) return new Booleano(false);
            else if (Match(TokenTypes.AbreParentesis))
            {
                Expresions expresion = Expresions();
                if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba )", Previous().line));
                return new Grouping(expresion, Previous().line);
            }
            else if (Match(TokenTypes.Identificador))
            {
                Token variable = Previous();
                Expresions expr = new Variable(variable.lexeme, entorno, Previous().line);
                return expr;
            }
            else
            {

                errors.Add(new Error(TypeOfError.Invalid, "Expresion invalida ", Previous().line));
                throw new Exception("Expresion invalida " + Previous().line);
            }
        }


        public bool Match(params TokenTypes[] types)
        {
            foreach (TokenTypes item in types)
            {
                if (Check(item))
                {
                    code += tokens[Current].lexeme + " ";
                    Move();
                    return true;
                }
            }
            return false;
        }
        public bool Check(TokenTypes types)
        {
            if (IsAtEnd()) return false;
            return Actual().types == types;
        }
        public bool Next(params TokenTypes[] types)
        {
            Token token = tokens[Current + 1];
            return token != null && types.Contains(token.types);
        }
        public Token Move()
        {
            if (!IsAtEnd()) Current++;
            return Previous();
        }
        public bool IsAtEnd()
        {
            if (Current == tokens.Count - 1) return true;
            else return false;
        }
        public Token Actual()
        {
            return tokens[Current];
        }
        public Token Previous()
        {
            return tokens[Current - 1];
        }
        public bool IsKeyword(Token token)
        {
            if (Scanner.KeyWords.ContainsValue(token.types))
            {
                Move();
                return true;
            }
            return false;
        }
        public AST VarDeclaration(Expresions expresions)
        {
            VarDeclaration var = null;
            Token operador = Previous();
            Expresions initializer = Expresions();
            var = new VarDeclaration(expresions, initializer, operador, entorno);
            var.Execute();
            return var;
        }
        public AST Declaration()
        {
            AST ast = null;
            if (Check(TokenTypes.Identificador))
            {
                Expresions expresions = Expresions();
                ast = new ExpresionEvaluator(expresions, Actual().line);
                if (Match(TokenTypes.Declaracion))
                {
                    ast = VarDeclaration(expresions);
                }
            }
            else
            {
                errors.Add(new Error(TypeOfError.Invalid, "Expresion invalida", Previous().line));
            }
            return ast;
        }
        public bool sincronizar(Error error, TokenTypes final = TokenTypes.EOF)
        {
            errors.Add(error);
            while (!Match(final))
            {
                if (Actual().types == TokenTypes.EOF) return true;
                else Move();
            }
            return false;
        }


        public AST Color()  
        {
            Cadena color = null;
            if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

            if (Match(TokenTypes.Cadena)) color = new Cadena(Previous().lexeme);
            else errors.Add(new Error(TypeOfError.Expected, "Se esperaba una cadena", Previous().line));

            if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));

            return new Colores(color, canvas, Previous().line);
        }
        public AST Size()
        {
            Numero size = null;
            if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

            if (Match(TokenTypes.Numero)) size = new Numero(Convert.ToInt32(Previous().literal));
            else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

            if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));

            return new Sizes(size, canvas, Previous().line);
        }
        public AST DrawLine()
        {
            Expresions intX = null;
            Expresions intY = null;
            Expresions Distance = null;
            if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

            Expresions expr1 = Expresions();
            intX = expr1;

            if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

            Expresions expr2 = Expresions();
            intY = expr2;

            if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

            Expresions expr3 = Expresions();
            Distance = expr3;

            if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));

            return new DrawLine(intX, intY, Distance, canvas, Previous().line);
        }
        public AST DrawCircle()
        {
            Expresions intX = null;
            Expresions intY = null;
            Expresions radius = null;
            if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

            Expresions expr1 = Expresions();
            intX = expr1;

            if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

            Expresions expr2 = Expresions();
            intY = expr2;

            if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

            Expresions expr3 = Expresions();
            radius = expr3;

            if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));

            return new DrawCircle(intX, intY, radius, canvas, Previous().line);
        }
        public AST DrawRectangle()
        {
            Expresions intX = null;
            Expresions intY = null;
            Expresions width = null;
            Expresions height = null;
            Expresions distance = null;
            if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

            Expresions expr1 = Expresions();
            intX = expr1;

            if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

            Expresions expr2 = Expresions();
            intY = expr2;

            if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

            Expresions expr3 = Expresions();
            width = expr3;

            if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

            Expresions expr4 = Expresions();
            height = expr4;

            if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

            Expresions expr5 = Expresions();
            distance = expr5;

            if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));

            return new DrawRectangle(intX, intY, distance, width, height, canvas, Previous().line);
        }
        public AST Fill()
        {
            if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));
            if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));
            return new Fill(canvas);
        }
        public Expresions GetActualX()
        {
            if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));
            if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));
            return new GetActualX(canvas);
        }
        public Expresions GetActualY()
        {
            if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));
            if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));
            return new GetActualY(canvas);
        }
        public Expresions GetCanvasSize()
        {
            if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));
            if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));
            return new GetCanvasSize(canvas);
        }
        public Expresions GetColorCount()
        {
            Expresions color = null;
            Expresions x1 = null;
            Expresions y1 = null;
            Expresions x2 = null;
            Expresions y2 = null;
            if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

            Expresions expr1 = Expresions();
            color = expr1;

            if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

            Expresions expr2 = Expresions();
            x1 = expr2;

            if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

            Expresions expr3 = Expresions();
            y1 = expr3;

            if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

            Expresions expr4 = Expresions();
            x2 = expr4;

            if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

            Expresions expr5 = Expresions();
            y2 = expr5;

            if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));

            return new GetColorCount(color, x1, y1, x2, y2, canvas, Previous().line);
        }
        public Expresions IsBrushColor()
        {
            Expresions color = null;
            if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

            Expresions expr1 = Expresions();
            color = expr1;

            if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));

            return new IsBrushColor(color, canvas, Previous().line);
        }
        public Expresions IsBrushSize()
        {
            Expresions k = null;
            if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

            Expresions expr1 = Expresions();
            k = expr1;

            if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));

            return new IsBrushSize(k, canvas, Previous().line);
        }
        public Expresions IsCanvasColor()
        {
            Cadena color = null;
            Numero v = null;
            Numero h = null;
            if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

            if (Match(TokenTypes.Cadena)) color = new Cadena(Previous().lexeme);
            else errors.Add(new Error(TypeOfError.Expected, "Se esperaba una cadena", Previous().line));

            if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

            if (Match(TokenTypes.Numero)) v = new Numero(Convert.ToInt32(Previous().literal));
            else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

            if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

            if (Match(TokenTypes.Numero)) h = new Numero(Convert.ToInt32(Previous().literal));
            else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

            if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));

            return new IsCanvasColor(color, v, h, canvas, Previous().line);
        }
        public AST Label()
        {
            Label label = null;
            Move();
            string name = Previous().lexeme;
            label = new Label(name, Block(), entorno, Previous().line);
            return label;
        }
        public AST GoTo()
        {
            string label = "";
            if (!Match(TokenTypes.AbreCorchete)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba [", Previous().line));

            if (Match(TokenTypes.Identificador)) label = Previous().lexeme;
            else errors.Add(new Error(TypeOfError.Expected, "Se esparaba un label", Previous().line));

            if (!Match(TokenTypes.CierraCorchete)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba ]", Previous().line));

            if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba (", Previous().line));
            Expresions Condition = Expresions();
            if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba )", Previous().line));

            return new GoTo(label, Condition, entorno, Previous().line);
        }
        public Block Block()
        {
            List<AST> Declarations = new List<AST>();
            if (SpawnCheck)
            {
                Declarations.Add(spawn);
                SpawnCheck = false;
            }
            do
            {
                if (Actual().types is TokenTypes.EOF) break;
                try
                {
                    if (Match(TokenTypes.SaltoLinea)) continue;
                    else if (Match(TokenTypes.Spawn)) sincronizar(new Error(TypeOfError.Invalid, "No pueden haber 2 Spawn", Previous().line), TokenTypes.SaltoLinea);
                    else if (Match(TokenTypes.GetActualX)) Declarations.Add(new GetActualX(canvas));
                    else if (Match(TokenTypes.GetActualY)) Declarations.Add(new GetActualY(canvas));
                    else if (Match(TokenTypes.Color)) Declarations.Add(Color());
                    else if (Match(TokenTypes.Size)) Declarations.Add(Size());
                    else if (Match(TokenTypes.DrawLine)) Declarations.Add(DrawLine());
                    else if (Match(TokenTypes.DrawCircle)) Declarations.Add(DrawCircle());
                    else if (Match(TokenTypes.DrawRectangle)) Declarations.Add(DrawRectangle());
                    else if (Match(TokenTypes.Fill)) Declarations.Add(Fill());
                    else if (Match(TokenTypes.GetActualX)) Declarations.Add(GetActualX());
                    else if (Match(TokenTypes.GetActualY)) Declarations.Add(GetActualY());
                    else if (Match(TokenTypes.GetCanvasSize)) Declarations.Add(GetCanvasSize());
                    else if (Match(TokenTypes.GetColorCount)) Declarations.Add(GetColorCount());
                    else if (Match(TokenTypes.IsBrushColor)) Declarations.Add(IsBrushColor());
                    else if (Match(TokenTypes.IsBrushSize)) Declarations.Add(IsBrushSize());
                    else if (Match(TokenTypes.IsCanvasColor)) Declarations.Add(IsCanvasColor());
                    else if (Match(TokenTypes.GoTo)) Declarations.Add(GoTo());
                    else if (Check(TokenTypes.Identificador))
                    {
                        if (Next(TokenTypes.Declaracion)) Declarations.Add(Declaration());
                        else Declarations.Add(Label());
                    }
                    else throw new Error(TypeOfError.Invalid, "Declaracion invalida", Actual().line);

                    if (IsAtEnd()) break;
                    else if ((Check(TokenTypes.SaltoLinea) && Next(TokenTypes.EOF))) break;
                    else if (!Match(TokenTypes.SaltoLinea)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un salto de linea", Previous().line));
                }
                catch (Error error)
                {
                    if (sincronizar(error, TokenTypes.SaltoLinea)) break;
                }
            } while (!(Actual().types == TokenTypes.EOF));

            return new Block(Declarations);
        }
    }
}