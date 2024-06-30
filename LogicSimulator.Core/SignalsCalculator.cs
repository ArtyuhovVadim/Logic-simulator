namespace LogicSimulator.Core;

public static class SignalsCalculator
{
    private static readonly SignalType[,] AndSignalsMap =
    {
        /*                           SignalType.Low   SignalType.High        SignalType.Undefined   SignalType.PosEdge     SignalType.NegEdge     SignalType.HighImp */
        /* SignalType.Low       */ { SignalType.Low,  SignalType.Low,        SignalType.Low,        SignalType.Low,        SignalType.Low,        SignalType.Low     },
        /* SignalType.High      */ { SignalType.Low,  SignalType.High,       SignalType.Undefined,  SignalType.PosEdge,    SignalType.NegEdge,    SignalType.HighImp },
        /* SignalType.Undefined */ { SignalType.Low,  SignalType.Undefined,  SignalType.Undefined,  SignalType.Undefined,  SignalType.Undefined,  SignalType.Low     },
        /* SignalType.PosEdge   */ { SignalType.Low,  SignalType.PosEdge,    SignalType.Undefined,  SignalType.PosEdge,    SignalType.Undefined,  SignalType.Low     },
        /* SignalType.NegEdge   */ { SignalType.Low,  SignalType.NegEdge,    SignalType.Undefined,  SignalType.Undefined,  SignalType.NegEdge,    SignalType.Low     },
        /* SignalType.HighImp   */ { SignalType.Low,  SignalType.HighImp,    SignalType.Low,        SignalType.Low,        SignalType.Low,        SignalType.Low     }
    };

    private static readonly SignalType[,] OrSignalsMap =
    {
        /*                           SignalType.Low         SignalType.High   SignalType.Undefined   SignalType.PosEdge     SignalType.NegEdge     SignalType.HighImp */
        /* SignalType.Low       */ { SignalType.Low,        SignalType.High,  SignalType.Undefined,  SignalType.PosEdge,    SignalType.NegEdge,    SignalType.HighImp },
        /* SignalType.High      */ { SignalType.High,       SignalType.High,  SignalType.High,       SignalType.High,       SignalType.High,       SignalType.High    },
        /* SignalType.Undefined */ { SignalType.Undefined,  SignalType.High,  SignalType.Undefined,  SignalType.Undefined,  SignalType.Undefined,  SignalType.High    },
        /* SignalType.PosEdge   */ { SignalType.PosEdge,    SignalType.High,  SignalType.Undefined,  SignalType.PosEdge,    SignalType.Undefined,  SignalType.High    },
        /* SignalType.NegEdge   */ { SignalType.NegEdge,    SignalType.High,  SignalType.Undefined,  SignalType.Undefined,  SignalType.NegEdge,    SignalType.High    },
        /* SignalType.HighImp   */ { SignalType.HighImp,    SignalType.High,  SignalType.High,       SignalType.High,       SignalType.High,       SignalType.HighImp }
    };

    private static readonly SignalType[,] XorSignalsMap =
    {
        /*                           SignalType.Low         SignalType.High        SignalType.Undefined   SignalType.PosEdge     SignalType.NegEdge     SignalType.HighImp */
        /* SignalType.Low       */ { SignalType.Low,        SignalType.High,       SignalType.Undefined,  SignalType.PosEdge,    SignalType.NegEdge,    SignalType.HighImp },
        /* SignalType.High      */ { SignalType.High,       SignalType.Low,        SignalType.Undefined,  SignalType.NegEdge,    SignalType.PosEdge,    SignalType.HighImp },
        /* SignalType.Undefined */ { SignalType.Undefined,  SignalType.Undefined,  SignalType.Undefined,  SignalType.Undefined,  SignalType.Undefined,  SignalType.HighImp },
        /* SignalType.PosEdge   */ { SignalType.PosEdge,    SignalType.NegEdge,    SignalType.Undefined,  SignalType.Undefined,  SignalType.Undefined,  SignalType.HighImp },
        /* SignalType.NegEdge   */ { SignalType.NegEdge,    SignalType.PosEdge,    SignalType.Undefined,  SignalType.Undefined,  SignalType.Undefined,  SignalType.HighImp },
        /* SignalType.HighImp   */ { SignalType.HighImp,    SignalType.HighImp,    SignalType.HighImp,    SignalType.HighImp,    SignalType.HighImp,    SignalType.HighImp }
    };

    private static readonly SignalType[] NotSignalsMap =
    [
        /* SignalType.Low    SignalType.High  SignalType.Undefined   SignalType.PosEdge   SignalType.NegEdge   SignalType.HighImp */
           SignalType.High,  SignalType.Low,  SignalType.Undefined,  SignalType.NegEdge,  SignalType.PosEdge,  SignalType.HighImp
    ];

    public static SignalType CalculateAsAnd(SignalType a, SignalType b) => AndSignalsMap[(int)a, (int)b];
    public static SignalType CalculateAsNand(SignalType a, SignalType b) => CalculateAsNot(CalculateAsAnd(a, b));

    public static SignalType CalculateAsOr(SignalType a, SignalType b) => OrSignalsMap[(int)a, (int)b];
    public static SignalType CalculateAsNor(SignalType a, SignalType b) => CalculateAsNot(CalculateAsOr(a, b));

    public static SignalType CalculateAsXor(SignalType a, SignalType b) => XorSignalsMap[(int)a, (int)b];
    public static SignalType CalculateAsXnor(SignalType a, SignalType b) => CalculateAsNot(CalculateAsXor(a, b));

    public static SignalType CalculateAsNot(SignalType a) => NotSignalsMap[(int)a];
}