using UnityEngine;
using System;

public delegate void Callback();

public delegate void Callback<T>(T arg);

public delegate void Callback<T0, T1>(T0 gameObject, T1 args) where T0 : Component where T1 : EventArgs;

public delegate void Callback<T0, T1, T2>(T0 gameObject, T1 args, T2 tempList) where T0 : Component where T1 : EventArgs where T2 : class;

public delegate void Callback<T0, T1, T2, T3>(T0 gameObject, T1 args, T2 tempList, T3 lockList) where T0 : Component where T1 : EventArgs where T2 : class where T3 : class;
