namespace LogicSimulator.Core;

public enum SignalType : byte
{
    /// <summary>
    /// Низкий сигнал
    /// </summary>
    Low,

    /// <summary>
    /// Высокий сигнал
    /// </summary>
    High,

    /// <summary>
    /// Сигнал не определён, т.е. 0 или 1
    /// </summary>
    Undefined,

    /// <summary>
    /// Передний фронт, т.е. 0 => 1
    /// </summary>
    PosEdge,

    /// <summary>
    /// Задний фронт, т.е. 1 => 0
    /// </summary>
    NegEdge,

    /// <summary>
    /// Сигнал высокого импеданса
    /// </summary>
    HighImp,
}