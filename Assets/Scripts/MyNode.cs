using UnityEngine;
using UnityEngine.UI;

public class Scene
{
    public int Id {  get; set; }
    public int Background { get; set; }
    public int Music { get; set; }
    public Replic[] Replics { get; set; }
    public Choise[] Choises { get; set; }
    public int Target {  get; set; }
}

public class Replic
{
    public string Text { get; set; }
    public string Name { get; set; }
    public int Color { get; set; }
    public int Sound { get; set; }
    public Sprite[] Sprite { get; set; }
}

public class Sprite
{
    public string Name { get; set; }
    public int Number { get; set; }
}

public class Choise
{
    public int Key { get; set; }
    public string Text { get; set; }
    public int TargetID { get; set; }
}
