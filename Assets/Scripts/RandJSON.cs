using System;
using System.Runtime.Serialization;
using SimpleJSON;

public static class RandJSON
{
    public static int JSONInt(JSONNode node)
    {
        int returnInt = int.MaxValue;

        // Check if it's an array of min/max
        if (node.IsArray)
        {
            if (node.AsArray.Count != 2)
                throw new JSONNodeIncorrectFormatException(node.ToString() + "\n\nNode should have exactly two members.");

            if (!node[0].IsNumber || !node[1].IsNumber)
                throw new JSONNodeNotANumberException(node.ToString());

            // Get a random range between first and second members (inclusive and exclusive respectively)
            returnInt = UnityEngine.Random.Range(node[0].AsInt, node[1].AsInt);
        }
        else
        {
            if (!node.IsNumber)
                throw new JSONNodeNotANumberException(node.ToString());

            // Load the int
            returnInt = node.AsInt;
        }

        return returnInt;
    }

    public static float JSONFloat(JSONNode node)
    {
        float returnFloat = float.MaxValue;

        // Check if it's an array of min/max
        if (node.IsArray)
        {
            if (node.AsArray.Count != 2)
                throw new JSONNodeIncorrectFormatException(node.ToString() + "\n\nNode should have exactly two members.");

            if (!node[0].IsNumber || !node[1].IsNumber)
                throw new JSONNodeNotANumberException(node.ToString());

            // Get a random range between first and second members (both inclusive)
            returnFloat = UnityEngine.Random.Range(node[0].AsFloat, node[1].AsFloat);
        }
        else
        {
            if (!node.IsNumber)
                throw new JSONNodeNotANumberException(node.ToString());

            // Load the float
            returnFloat = node.AsFloat;
        }

        return returnFloat;
    }
}

#region Exceptions

public class JSONNodeNotANumberException : Exception
{
    public JSONNodeNotANumberException()
    {
    }

    public JSONNodeNotANumberException(string message) : base(message)
    {
    }

    public JSONNodeNotANumberException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected JSONNodeNotANumberException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

public class JSONNodeIncorrectFormatException : Exception
{
    public JSONNodeIncorrectFormatException()
    {
    }

    public JSONNodeIncorrectFormatException(string message) : base(message)
    {
    }

    public JSONNodeIncorrectFormatException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected JSONNodeIncorrectFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

#endregion
