using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

[CreateAssetMenu(menuName = "VinTools/Custom Tiles/ Custom Rule Tile")]
public class CustomRuleTile : RuleTile<CustomRuleTile.Neighbor> {
    public bool alwaysConnect;
    public TileBase[] tilesToConnect;

    public List<TileBase> tiles1Left;
    public List<TileBase> tiles2Left;
    public List<TileBase> tiles1Right;
    public List<TileBase> tiles2Right;
    public List<TileBase> tiles1Down;
    public List<TileBase> tiles2Down;
    public List<TileBase> tiles1Up;
    public List<TileBase> tiles2Up;

    private List<List<TileBase>> tileSet;
    
    void InitTiles()
    {
        tileSet.Add(tiles1Left);
        tileSet.Add(tiles2Left);
        tileSet.Add(tiles1Right);
        tileSet.Add(tiles2Right);
        tileSet.Add(tiles1Down);
        tileSet.Add(tiles2Down);
        tileSet.Add(tiles1Up);
        tileSet.Add(tiles2Up);
    }

    void Init()
    {
        InitTiles();
    }

    void Start()
    {
        Init();
    }


    public bool checkSelf;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Any = 3;
        public const int Specified = 4;
        public const int Nothing = 5;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.This: return Check_This(tile);
            case Neighbor.NotThis: return Check_NotThis(tile);
            case Neighbor.Any: return Check_Any(tile);
            case Neighbor.Specified: return Check_Specified(tile);
            case Neighbor.Nothing: return Check_Nothing(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }

    bool Check_This(TileBase tile)
    {
        if (!alwaysConnect) return tile == this;
        else return tilesToConnect.Contains(tile) || tile == this;

        // .Contains required "using System.Linq;"
    }

    bool Check_NotThis(TileBase tile)
    {
        return tile != this;
    }

    bool Check_Any(TileBase tile)
    {
        if (checkSelf) return tile != null;
        else return tile != null && tile != this;
    }
    bool Check_Specified(TileBase tile)
    {
        return tilesToConnect.Contains(tile);

        // .Contains required "using System.Linq;"
    }
    bool Check_Nothing(TileBase tile)
    {
        return tile == null;
    }
}