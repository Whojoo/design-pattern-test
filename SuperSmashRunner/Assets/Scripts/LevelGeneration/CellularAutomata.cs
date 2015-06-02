using UnityEngine;
using System.Collections.Generic;

/*
    0  = air
    1  = land
    2  = coins
    3  = wall
    4  = question block
    5  = breakable block
    
    10 = floor left edge
    11 = floor right edge
    12 = mountain left edge
    13 = mountain right edge
    14 = floor left wall
    15 = floor right wall
    16 = mountain left wall
    17 = mountain right wall
    */

public class Block
{
    public Vector2 Start { get; set; }
    public Vector2 End { get; set; }

    public Rect GetRect()
    {
        int y1 = (int)(Start.y + 0.5f);
        int y2 = (int)(Start.y - 0.5f);
        return new Rect((int)Start.x, y1, (int)End.x, y2);
    }

    public Vector2 GetPosition()
    {
        return Start + (End - Start) * 0.5f;
    }

    public Vector2 GetScale()
    {
        return End - Start + new Vector2(0, 0.5f);
    }

    public Vector2 GetRandomPosition()
    {
        System.Random randy = ServiceLocator.GetRandomService().GetRandomizer();
        Vector2 addition = End - Start;
        addition *= (float)randy.NextDouble();

        return Start + addition;
    }
}

/**
 * Adjusted CelularAutomata 
 * 
 * @author Thomas & Kevin
 */
public class CellularAutomata
{
    public int[,] land;
    private int[] floor;
    private int width;
    private int height;
    System.Random random;

    public int beginFloorHeight = 0;

    public CellularAutomata(int pWidth, int pHeight)
    {
        this.width = pWidth;
        this.height = pHeight;
        this.random = ServiceLocator.GetRandomService().GetRandomizer();
        this.land = new int[this.width, this.height];
        this.floor = new int[this.width];
    }

    public int[,] GenerateMap(int iterations)
    {
        generateLand();
        processN(iterations);
        removeDoubleBlocks();
        fixWalls();

        return land;
    }

    public int GetFirstY()
    {
        return height - floor[0];
    }

    public List<Block> GetBlocks()
    {
        List<Block> toReturn = new List<Block>();
        int height = land.GetLength(1) - 1;

        for (int y = 0; y < land.GetLength(1); y++)
        {
            for (int x = 0; x < land.GetLength(0); x++)
            {
                //Any number above 0 is a platform.
                if (land[x, y] > 0)
                {
                    //Save the start.
                    Block newBlock = new Block();
                    newBlock.Start = new Vector2(x, height - y);

                    //Keep looping through x until you are at the end or until you meet a 0.
                    while (++x < land.GetLength(0) && land[x, y] > 0) ;

                    //Add the End with x decremented by 1 (we stopped at the end or a 0).
                    newBlock.End = new Vector2(--x, height - y);

                    //Add to array.
                    toReturn.Add(newBlock);
                }
            }
        }

        return toReturn;
    }

    /**
     * @param args the command line arguments
     */
    //public static void main(string[] args)
    //{
    //    celularautomata test = new celularautomata();
    //    test.processn(10);
    //    test.removedoubleblocks();
    //    test.generateplatforms();
    //    test.makebreakableplatforms();
    //    test.placemissingwalls();
    //    test.fixwalls();
    //    test.repaint();
    //}

    /**
     * Update the land according to our rules
     * @param n number of updates
     */
    public void processN(int n)
    {
        for (int i = 0; i < n; i++)
        {
            // First get the floor
            getFloor();

            // Then update the land according to our rules
            land = updateLand(land);
        }
    }

    /**
     * Use this to get the lowest tiles (floor)
     */
    private void getFloor()
    {
        for (int i = 0; i < width; i++)
        {
            int j = height - 1;
            while (land[i, j] != 1 && j > 0)
            {
                j--;
            }
            if (j > 0)
            {
                floor[i] = j;
            }
            else
            {
                floor[i] = -1;
            }
        }
    }

    /**
     * Use this to update each tile according to our rules
     * @param land The land containing all the tiles
     * @return The updated land
     */
    private int[,] updateLand(int[,] land)
    {
        int[,] nextTo = nextToLand(land, 1);
        int[,] topBottom = betweenLand(land, 1);
        int[,] temp = land;

        int gapWidth = 0;
        int startGap = 0;

        beginFloorHeight = random.Next(5);
        beginFloorHeight = height - 1 - beginFloorHeight;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //rule one (no neighboors become air)
                if (nextTo[i, j] == 0)
                {
                    temp[i, j] = 0;
                }
                else if (nextTo[i, j] == 2)
                { //rule two (block needs to be at least 3)
                    temp[i, j] = 1;
                }
                else
                { //rule two continued
                    if (land[i, j] == 1)
                    {
                        if (nextTo[i, j] == 1)
                        {
                            if (i > 0 && nextTo[i - 1, j] == 2)
                            {
                                temp[i, j] = 1;
                            }
                            else if (i < width - 1 && nextTo[i + 1, j] == 2)
                            {
                                temp[i, j] = 1;
                            }
                            else
                            {
                                temp[i, j] = 0;
                            }
                        }
                    }
                }

                // rule three (when a block has no block above and no block below, look at the block above it. 
                // If it has a block above and a block below. Remove this block (Bigger gaps (in height) between platforms)
                if (land[i, j] == 1 && topBottom[i, j] == 0)
                {
                    if (j > 0 && topBottom[i, j - 1] == 2)
                    {
                        temp[i, j] = 0;
                    }
                }

                // rule four (two tiles above floor becomes air)
                if (floor[i] - 2 == j || floor[i] - 1 == j)
                {
                    temp[i, j] = 0;
                }

                // rule five (top 4 tiles become air)
                if (j <= 3)
                {
                    temp[i, j] = 0;
                }

                // rule six (first and last 20 tiles become straight floor)
                if (i < 20 || i > width - 20)
                {
                    if (j == beginFloorHeight)
                    {
                        temp[i, j] = 1;
                        floor[i] = j;
                    }
                    else
                    {
                        temp[i, j] = 0;
                    }
                }
            }

            // rule seven (gaps should not be too wide)
            if (floor[i] != -1 && i < (width - 1))
            {
                // sub rule (max height between floors are less when theres a gap between)
                temp = floorWithinJumpRange(startGap - 1, i, temp, 2);

                if (gapWidth > 4)
                {
                    temp[startGap, floor[startGap - 1]] = 1;
                    floor[startGap] = floor[startGap - 1];
                    temp[i - 1, floor[i]] = 1;
                    floor[i - 1] = floor[i];
                }
                gapWidth = 0;
            }
            else
            {
                if (gapWidth == 0)
                {
                    startGap = i;
                }
                gapWidth += 1;
            }
            // end gap rule

            // rule eight (if next floor is more than 3 tiles up it should be lowered)
            temp = floorWithinJumpRange(i, i + 1, temp, 3);
        }
        return temp;

    }

    /**
     * Check if secondTile is between maxDifference, if not secondTile is lowered
     * @param firstTile the first tile on the x-axis (used to check difference with secondTile)
     * @param secondTile the second tile on the x-axis (used to check difference with firstTile)
     * @param temp the temp land for updating tiles
     * @param maxDifference the maximum difference between the two tiles
     * @return 
     */
    private int[,] floorWithinJumpRange(int firstTile, int secondTile, int[,] temp, int maxDifference)
    {
        if (firstTile >= 0 && secondTile < width - 1 && floor[firstTile] != -1 && floor[secondTile] != -1 && (floor[firstTile] - floor[secondTile]) > maxDifference)
        {
            int newFloorHeight = floor[firstTile] - maxDifference;
            temp[secondTile, floor[secondTile]] = 0;
            floor[secondTile] = newFloorHeight;
            temp[secondTile, newFloorHeight] = 1;
        }

        return temp;
    }

    /**
     * Remove this block if there is another block below
     */
    public void removeDoubleBlocks()
    {
        int[,] temp = land;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (land[i, j] == 1 && j > 0 && j < height - 1 && land[i, j + 1] == 1)
                {
                    temp[i, j] = 0;
                }
            }
        }
        land = temp;
    }

    /**
     * Generate question blocks. On top of floor or mountain if the size is bigger than 2 blocks.
     * Doesn't appear above edges of floor or mountain.
     */
    public void generatePlatforms()
    {
        int startY = -1;
        int count = 0;

        for (int i = 20; i < width - 20; i++)
        {
            int j = 0;
            while (j < height - 1 && land[i, j] != 1)
            {
                j++;
            }

            if (land[i, j] == 1 && land[i - 1, j] == 1 && land[i + 1, j] == 1)
            {
                if (startY == -1 || startY != j)
                {
                    startY = j;
                    count = 0;
                }

                count++;
            }
            else
            {
                if (count >= 3)
                {
                    while (count > 0)
                    {
                        if (startY - 4 > 0 && land[i - count - 1, startY - 4] != 1 && land[i + 1, startY - 4] != 1)
                        {
                            land[i - count, startY - 4] = 4;
                        }
                        count--;
                    }
                }

                startY = -1;
                count = 0;
            }
        }
    }

    /**
     * Used to get the neighbours on the right and left
     * @param land The map to get the neighbours from
     * @param tile The kind of tile you want to check for neighbours
     * @return the map with numbers as how many neighbours (land[i, j] = 1, so has 1 neighbour)
     */
    private int[,] nextToLand(int[,] land, int tile)
    {
        int[,] temp = new int[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                temp[i, j] = 0;
                if (i > 0)
                {
                    temp[i, j] += (land[i - 1, j] == tile) ? 1 : 0; // left
                }
                if (i < width - 1)
                {
                    temp[i, j] += (land[i + 1, j] == tile) ? 1 : 0; // right
                }
            }
        }
        return temp;
    }

    /**
     * Used to get the neighbours above and below
     * @param land The map to get the neighbours from
     * @param tile The kind of tile you want to check for neighbours
     * @return the map with numbers as how many neighbours (land[i, j] = 1, so has 1 neighbour)
     */
    private int[,] betweenLand(int[,] land, int tile)
    {
        int[,] temp = new int[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                temp[i, j] = 0;
                if (j > 0)
                {
                    temp[i, j] += (land[i, j - 1] == tile) ? 1 : 0; // top
                }
                if (j < height - 1)
                {
                    temp[i, j] += (land[i, j + 1] == tile) ? 1 : 0; // bottom
                }
            }
        }
        return temp;
    }

    /**
     * If it is a question block which is more than 4 wide, it becomes a breakable platform
     */
    public void makeBreakablePlatforms()
    {
        int[,] nextTo = nextToLand(land, 4);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (land[i, j] == 4 && i > 1 && i < width - 1 && land[i - 1, j] == 5)
                {
                    land[i, j] = 5;
                }
                else if (land[i, j] == 4 && i > 1 && i < width - 1 && land[i, j] == 4 && nextTo[i, j] == 2 && nextTo[i - 1, j] == 2 && nextTo[i + 1, j] == 2)
                {
                    land[i - 2, j] = 5;
                    land[i - 1, j] = 5;
                    land[i, j] = 5;
                    land[i + 1, j] = 5;
                    land[i + 2, j] = 5;
                }
            }
        }
    }

    /**
     * Put a wall below every land
     */
    public void placeMissingWalls()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (j < height - 1 && (land[i, j] == 1 || land[i, j] == 3) && (land[i, j + 1] != 1 && land[i, j + 1] != 3))
                {
                    setWall(i, j);
                }
            }
        }
    }

    /**
     * This fixes the walls and puts edges on the platforms and floor
     */
    public void fixWalls()
    {
        int[,] temp = copyArray(land);

        for (int i = 19; i < width - 18; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (land[i, j] == 1)
                {
                    if (land[i - 1, j] != 1)
                    {
                        if (floor[i] == j)
                        {
                            temp[i, j] = 10;
                            int y = j + 1;
                            while (y < height && temp[i, y] == 3)
                            {
                                temp[i, y] = 14;
                                y++;
                            }
                        }
                        else
                        {
                            temp[i, j] = 12;
                            int y = j + 1;
                            while (y < height && temp[i, y] == 3)
                            {
                                temp[i, y] = 16;
                                y++;
                            }
                        }
                    }
                    else if (land[i + 1, j] != 1)
                    {
                        if (floor[i] == j)
                        {
                            temp[i, j] = 11;
                            int y = j + 1;
                            while (y < height && temp[i, y] == 3)
                            {
                                temp[i, y] = 15;
                                y++;
                            }
                        }
                        else
                        {
                            temp[i, j] = 13;
                            int y = j + 1;
                            while (y < height && temp[i, y] == 3)
                            {
                                temp[i, y] = 17;
                                y++;
                            }
                        }
                    }
                }
            }
        }
        land = temp;
    }

    /**
     * Helper function to check if there is no tile below with tileID
     * @param i the parameter on the x-axis
     * @param j the parameter on the y-axis
     * @param tileID the ID of the tile you want to check
     * @return 
     */
    private bool checkNoSpecificTileBelow(int i, int j, int tileID)
    {
        for (int x = (j + 1); x < height; x++)
        {
            if (land[i, x] == tileID)
            {
                return false;
            }
        }

        return true;
    }

    /**
     * Helper function to place a wall below a block untill you reach the bottom of the world
     * @param i the parameter on the x-axis
     * @param j the parameter on the y-axis
     */
    private void setWall(int i, int j)
    {
        for (int y = (j + 1); y < height; y++)
        {
            if (land[i, y] != 1)
            {
                land[i, y] = 3;
            }
        }
    }

    /**
     * Generate all the blocks in the beginning (seed)
     */
    private void generateLand()
    {
        if (land != null)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    land[i, j] = ((random.NextDouble() + ((double)j / (height * 1.7))) > 0.90 ? 1 : 0);
                }
            }
        }
    }

    /**
     * Copy the content of an array and not the reference
     * @param original original array to copy the content from
     * @return a new array with the same content as the original
     */
    public int[,] copyArray(int[,] original)
    {
        if (original == null)
        {
            return null;
        }

        int[,] result = (int[,])original.Clone();

        return result;
    }
}
