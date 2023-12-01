using System;
using System.Collections.Generic;

public class ResponseFormat
{
    public string type { get; set; }
}

public class Message
{
    public string role { get; set; }
    public string content { get; set; }
}

public class ParametersProperties
{
    public string Type { get; set; }
    public Properties Properties { get; set; }
}

public class Properties
{
    public Salles Salles { get; set; }
}

public class Salles
{
    public string Type { get; set; }
    public Item Items { get; set; }
}

public class Item
{
    public string Type { get; set; }
    public Properties1 Properties { get; set; }
    public List<string> Required { get; set; }
}

public class Properties1
{
    public Name Name { get; set; }
    public Capacity Capacity { get; set; }
    public PictureUrl PictureUrl { get; set; }
    public WellnessValue WellnessValue { get; set; }
    public Temperature Temperature { get; set; }
    public Humidity Humidity { get; set; }
    public Justification Justification { get; set; }
}

public class Name
{
    public string Type { get; set; }
    public string Description { get; set; }
}

public class Capacity
{
    public string Type { get; set; }
    public string Description { get; set; }
}

public class PictureUrl
{
    public string Type { get; set; }
    public string Description { get; set; }
}

public class WellnessValue
{
    public string Type { get; set; }
    public string Description { get; set; }
}

public class Temperature
{
    public string Type { get; set; }
    public string Description { get; set; }
}

public class Humidity
{
    public string Type { get; set; }
    public string Description { get; set; }
}

public class LastSync
{
    public string Type { get; set; }
    public string Description { get; set; }
}

public class Justification
{
    public string Type { get; set; }
    public string Description { get; set; }
}

public class Functions
{
    public string name { get; set; }
    public string description { get; set; }
    public ParametersParameters parameters { get; set; }
}

public class ParametersParameters
{
    public string type { get; set; }
    public Properties2 properties { get; set; }
    public List<string> required { get; set; }
}

public class Properties2
{
    public Salles Salles { get; set; }
}

public class RootObject
{
    public string model { get; set; }
    public ResponseFormat response_format { get; set; }
    public List<Message> messages { get; set; }
    public List<Functions> functions { get; set; }
    public string function_call { get; set; }
    public double temperature { get; set; }
}
