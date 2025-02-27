/*
	* Coder :
	* Last Update :
	* Information
*/
namespace project02
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class Wrapper<T>
    {
        public T[] array;
    }

    [System.Serializable]
    public class BaseInformation
    {
        public string index;
    }
}
