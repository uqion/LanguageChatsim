using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

//Custom dictionary with intent as key, cutom class NodeList as value (NodeList is a class that allows us to serialize a list of Nodes)
[Serializable]
public class NodeDictionary : SerializableDictionaryBase<string, NodeList>  {}


