using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A simple Quad Tree normally used for collision detection.
/// Use Clear just before the collision checks followed by Insert on all objects.
/// This way you keep the tree updated.
/// 
/// Huge thanks to Steven Lambert.
/// Link: http://gamedev.tutsplus.com/tutorials/implementation/quick-tip-use-quadtrees-to-detect-likely-collisions-in-2d-space/
/// </summary>
public class Quadtree
{
    /// <summary>
    /// Enum used for a quadrant's index.
    /// </summary>
    public enum Index : int
    {
        TopLeft,
        BottomLeft,
        BottomRight,
        TopRight
    }

    //Node constants.
    private const int MaxLevel = 2; //Don't split more than MaxLevel times.
    private const int MaxLeaves = 5;//Don't exceed this limit, unless already at MaxLevel
    private const int Nodes = 4;    //Number of nodes.

    //Readonly bounds.
    public readonly Rect Bounds;

    //Other variables.
    private int level;
    private Quadtree[] nodes;
    private List<Block> leaves;
    private int nextIndexForEnemySpawn;

    /// <summary>
    /// Creates a (possible) sub-node.
    /// </summary>
    /// <param name="level">The node's depth. Top node is 0.</param>
    /// <param name="bounds">The node's bounds. Top node's bounds should be the world size.</param>
    public Quadtree(int level, Rect bounds)
    {
        nextIndexForEnemySpawn = 0;
        this.level = level;
        Bounds = bounds;
        nodes = new Quadtree[Nodes];
        leaves = new List<Block>();
    }

    /// <summary>
    /// Creates the top node.
    /// </summary>
    /// <param name="bounds">The world's size.</param>
    public Quadtree(Rect bounds)
        : this(0, bounds)
    {
    }

    /// <summary>
    /// Clears this node and all it's child nodes.
    /// If this is the Top node, then the entire tree gets cleared.
    /// The Top node is still available for use.
    /// </summary>
    public void Clear()
    {
        nextIndexForEnemySpawn = 0;

        //Do we have any sub-notes?
        if (nodes[0] != null)
        {
            //Clear all sub-notes.
            for (int i = 0; i < Nodes; i++)
            {
                nodes[i].Clear();
                nodes[i] = null;
            }
        }
        //Bottom node, clear leaves.
        else
        {
            leaves.Clear();
        }
    }

    public List<int> GetIndex(Rect rect)
    {
        List<int> toReturn = new List<int>();

        for (int i = 0; i < nodes.Length; i++)
            if (rect.Overlaps(nodes[i].Bounds))
                toReturn.Add(i);

        return toReturn;
    }

    public Block GetNextEnemySpawnBlock()
    {
        Block toReturn;

        if (nodes[0] == null) //Is bottom node, return something from the leaves.
        {
            if (leaves.Count == 0)
                return null;

            toReturn = leaves[GetNextIndexForEnemySpawn(leaves.Count)];
        }
        else //Not bottom node, call one of the child nodes.
        {
            while ((toReturn = nodes[GetNextIndexForEnemySpawn(Nodes)].GetNextEnemySpawnBlock()) == null) ;
        }

        return toReturn;
    }

    private int GetNextIndexForEnemySpawn(int limit)
    {
        int toReturn = nextIndexForEnemySpawn;
        nextIndexForEnemySpawn = (nextIndexForEnemySpawn + 1) % limit;

        return toReturn;
    }

    /// <summary>
    /// Insert a component in the tree.
    /// </summary>
    /// <param name="block"></param>
    public void Insert(Block block)
    {
        //Get the Rect to work with.
        Rect rect = block.GetRect();

        //Do we have any child node?
        if (nodes[0] != null)
        {
            foreach (var index in GetIndex(rect))
            {
                nodes[index].Insert(block);
                return;
            }
        }

        //Object doesnt fit in a child, add it to this leaf.
        leaves.Add(block);

        //Do we need to split?
        if (nodes[0] == null && level < MaxLevel && leaves.Count > MaxLeaves)
        {
            Split();

            //For each leave.
            foreach (var leave in leaves)
                //Take each index it fits in.
                foreach (var index in GetIndex(leave.GetRect()))
                    //And insert it.
                    nodes[index].Insert(leave);

            //And finally clear the leaves list.
            leaves.Clear();
        }
    }

    /// <summary>
    /// Inserts a list of blocks into the tree.
    /// </summary>
    /// <param name="blocks">A list of blocks which implements IList</param>
    public void Insert(IList<Block> blocks)
    {
        //Insert all blocks.
        foreach (var block in blocks)
        {
            Insert(block);
        }
    }

    /// <summary>
    /// Creates the 4 child nodes.
    /// </summary>
    private void Split()
    {
        //Reset nextIndexForEnemySpawn.
        nextIndexForEnemySpawn = 0;

        //Calculate the half width and half height.
        int hw = (int)(Bounds.width * 0.5f);
        int hh = (int)(Bounds.height * 0.5f);

        int x = (int)Bounds.x;
        int y = (int)Bounds.y;
        int newLevel = level + 1;

        //Create the 4 new nodes.
        nodes[0] = new Quadtree(newLevel, new Rect(
            x, y, hw, hh));
        nodes[1] = new Quadtree(newLevel, new Rect(
            x, y + hh, hw, hh));
        nodes[2] = new Quadtree(newLevel, new Rect(
            x + hw, y + hh, hw, hh));
        nodes[3] = new Quadtree(newLevel, new Rect(
            x + hw, y, hw, hh));
    }
}