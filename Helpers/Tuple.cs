using System;

[Serializable]
public class Tuple<T1, T2>
{
    public T1 Value1 { get; set; }
    public T2 Value2 { get; set; }

    public Tuple(T1 val1, T2 val2)
    {
        Value1 = val1;
        Value2 = val2;
    }

    public Tuple()
    {

    }
}

[Serializable]
public class Tuple<T1, T2, T3> : Tuple<T1, T2>
{
    public T3 Value3 { get; set; }

    public Tuple()
    {

    }

    public Tuple(T1 val1, T2 val2, T3 val3)
        : base(val1, val2)
    {
        Value3 = val3;
    }
}


[Serializable]
public class Tuple<T1, T2, T3, T4> : Tuple<T1, T2, T3>
{
    public T4 Value4 { get; set; }

    public Tuple()
    {

    }

    public Tuple(T1 val1, T2 val2, T3 val3,T4 val4)
        : base(val1, val2,val3)
    {
        Value4 = val4;
    }
}