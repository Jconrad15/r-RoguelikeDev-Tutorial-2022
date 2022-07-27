using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_Node<T>
{
    public T data;

    // Nodes leading OUT from this node.
    public Path_Edge<T>[] edges;

}
