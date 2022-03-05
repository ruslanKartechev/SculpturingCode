using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sculpturing.Tools;
public class ColorCan : MonoBehaviour, IClickable
{

    private Color myColor;
    private PaintinTool tool;
    private ColorPalet palet;
   
    public void Init(PaintinTool _tool, ColorPalet _palet)
    {
        palet = _palet;
        tool = _tool;
    }
    public void SetColor(Color color)
    {
        myColor = color;
        GetComponent<Renderer>().materials[1].SetColor("_BaseColor", color);
   
    }
    public Color GetColor()
    {
        return myColor;
    }
    public void OnClick()
    {
        tool.SetColor(myColor);
        palet.OnColorChosen(this);
    }



}
