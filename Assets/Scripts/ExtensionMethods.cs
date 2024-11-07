using System.Collections.Generic;
using System.Net;
using UnityRandom = UnityEngine.Random;
using Random = System.Random;

public static class ExtensionMethods
{
    public static Random rng = new Random();

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


    public static void Shuffle2<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = rng.Next(i, count);//UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public static void UnityShuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityRandom.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public static uint ToUint(this IPAddress ipAddress)
    {
        var ipBytes = ipAddress.GetAddressBytes();
        var ip = (uint)ipBytes[0] << 24;
        ip += (uint)ipBytes[1] << 16;
        ip += (uint)ipBytes[2] << 8;
        ip += (uint)ipBytes[3];
        return ip;
    }

    public static IPAddress ToIpAddress(this uint address)
    {
        return new IPAddress(new byte[] {
                (byte)((address>>24) & 0xFF) ,
                (byte)((address>>16) & 0xFF) ,
                (byte)((address>>8)  & 0xFF) ,
                (byte)( address & 0xFF)});
    }

    //This just allows us to create a copy of this array rather than use a reference, as arrays are always reference types
    public static TArrayType[] CreateCopy<TArrayType>(this TArrayType[] arrayType)
    {
        TArrayType[] newArrayCopy = new TArrayType[arrayType.Length];
        arrayType.CopyTo(newArrayCopy, 0);

        return newArrayCopy;
    }
}
