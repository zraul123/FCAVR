using System.Collections.Generic;

public interface IFactory<T>
{
    /// <summary>
    /// Creator method for the templatized class
    /// </summary>
    /// <param name="readData">The data for the object</param>
    /// <returns>The object T created with the data</returns>
    T createInstance(IDictionary<string, string> readData);
}
