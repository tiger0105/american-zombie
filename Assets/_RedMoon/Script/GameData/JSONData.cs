using System.Collections;
using System.Collections.Generic;

public class Board {
	public int id;
	public int width;
	public int height;
	public int level;
	public int persons;
	public string style;
	public int runtime;
}

public enum ITEM_ID {
	ITEM_OBJECTS = 0,
	ITEM_GEM = 1,
	ITEM_FAKEGEM = 2,
	ITEM_FLAG = 3,
	ITEM_WALL = 4,
	ITEM_EMPTY = 5
}

public class BoxData {
	public int id;
	public ITEM_ID ItemID;
	public int posX;
	public int posY;
	public ArrayList Objects = new ArrayList();
	public ArrayList Extras = new ArrayList();
}

public class BoardData
{
	public BoardData(int w, int h)
	{
		nWidth = w;
		nHeight = h;
		boxData = new BoxData[w][];

		for (int i = 0; i < w; i++)
			boxData[i] = new BoxData[h];
	}

	public int nWidth;
	public int nHeight;
	public BoxData[][] boxData;
}