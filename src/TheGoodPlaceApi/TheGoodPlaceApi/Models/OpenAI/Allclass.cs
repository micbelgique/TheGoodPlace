using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

//roomsopenaidescriptionoffunction
public class Salles
{
    public string Type { get; set; } = "array";
    public Item Items { get; set; } = new Item(typeof(Properties));
}

public class Item
{
    public string Type { get; set; } = "object";
    public Properties Properties { get; set; } = new Properties();

    public List<string> Required { get; set; }

    public Item(Type type)
    {
        this.Required = type.GetProperties().Where(x => Attribute.IsDefined(x, typeof(RequiredAttribute)))
                                            .Select(x => x.Name.ToLower())
                                            .ToList();
    }
}


// How to use : 
public class Properties
{
    [Required]
    public Name Name { get; set; } = new Name { Type = "string", Description = "Nom de la salle" };
    [Required]
    public Capacity Capacity { get; set; } = new Capacity { Type = "string", Description = "Nombre de personne que peut accueillir la salle" };
    [Required]
    public WellnessValue WellnessValue { get; set; } = new WellnessValue { Type = "string", Description = "Nombre de 1 à 100 du bien-être, plus les température, l'humidité et la pression sont propices à travailler plus sa wellnessvalue sera grande" };
    [Required]
    public Temperature Temperature { get; set; } = new Temperature { Type = "string", Description = "La température de la salle en Celsius" };
    [Required]
    public Humidity Humidity { get; set; } = new Humidity { Type = "string", Description = "Le niveau d'humidité de la salle en %" };
    [Required]
    public Justification Justification { get; set; } = new Justification { Type = "string", Description = "La justification du score de bien-être de calcul en une phrase" };
    [Required]
    public PictureUrl PictureUrl { get; set; } = new PictureUrl { Type = "string", Description = "L'url de l'image de la salle" };

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
