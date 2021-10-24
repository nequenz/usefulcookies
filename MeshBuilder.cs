using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder
{

    private static byte[,] CapturedBody = null;

    private static float VoxelThickness = 0.4f;

    private static float VoxelSize = 1.0f;

    private static List<Vector3> Vertices = new List<Vector3>();

    private static List<int> Indecies = new List<int>();

    private static List<Color> ColorsList = new List<Color>();

    private static Mesh ParamMesh = null;




    public static void setVoxelSize(float S)
    {
        VoxelSize = S;
    }



    public static float getVoxelSize()
    {
        return VoxelSize;
    }



    public static void setVoxelThicknes(float T)
    {
        VoxelThickness = T;
    }



    public static float getVoxelThickness()
    {
        return VoxelThickness;
    }



    public static void takeBody(byte[,] ParamBody,Mesh TMesh)
    {
        ParamMesh       = TMesh;
        CapturedBody    = ParamBody;

        Vertices.Clear();
        Indecies.Clear();
        ColorsList.Clear();
    }



    public static void rebuildMesh()
    {

        if (CapturedBody == null || ParamMesh == null) return;

        int Length = (int)Mathf.Sqrt(CapturedBody.Length);


        ParamMesh.Clear();

        for (int x = 0; x < Length; x++)
        {
            for(int y = 0; y < Length; y++)
            {
                if (getVoxel(x,y) == 0) continue;
                else if(getVoxel(x,y) == 1)
                {
                    //BOTTOM SIDE
                    if (getVoxel(x, y - 1) == 0) addMeshSide(new Vector2Int(x, y), Quaternion.Euler(0, 0, 90), Color.black, false);

                    //UPPER SIDE
                    if (getVoxel(x, y + 1) == 0) addMeshSide(new Vector2Int(x, y), Quaternion.Euler(0, 0, 270), Color.black, false);

                    //RIGHT SIDE
                    if (getVoxel(x + 1, y) == 0) addMeshSide(new Vector2Int(x, y), Quaternion.Euler(0, 0, 180), Color.black, false);

                    //LEFT SIDE
                    if (getVoxel(x - 1, y) == 0) addMeshSide(new Vector2Int(x, y), Quaternion.Euler(0, 0, 0), Color.black, false);

                }
                else
                {
                    addMeshSide(new Vector2Int(x, y), Quaternion.Euler(0, 0, 0), Voxelerator.getColorByValue(getVoxel(x, y)), true);
                }

            }
        }
  

        //set normals and color
        Vector3[]   Normals = new Vector3[Vertices.Count];

        for (int i = 0; i < Vertices.Count; i++)
        {
            Normals[i] = Vector3.forward;
        }

        ParamMesh.vertices  = Vertices.ToArray();
        ParamMesh.triangles = Indecies.ToArray();
        ParamMesh.normals   = Normals;
        ParamMesh.colors    = ColorsList.ToArray();
        ParamMesh.RecalculateNormals();

        ParamMesh = null;

        Vertices.Clear();
        Indecies.Clear();
        ColorsList.Clear();
    }



    public static void rebuildMesh(byte[,] ParamBody, Mesh TMesh)
    {
        CapturedBody = ParamBody;
        ParamMesh = TMesh;
        rebuildMesh();
    }




    private static byte getVoxel(int X,int Y)
    {
        int Length = (int)Mathf.Sqrt(CapturedBody.Length);

        if (X >= 0 && X < Length && Y >= 0 && Y < Length) return CapturedBody[X, Y];

        return 0;
    }


    private static void addMeshSide(Vector2Int Pos,Quaternion ERot,Color iColor,bool Solid=false)
    {
        //Indecies
        int Top = (Vertices.Count);
        if (Top < 0) Top = 0;

        Indecies.Add(Top + 0);
        Indecies.Add(Top + 2);
        Indecies.Add(Top + 1);

        Indecies.Add(Top + 2);
        Indecies.Add(Top + 3);
        Indecies.Add(Top + 1);

    
        //Rotation center of line
        float CenterOffset = -(VoxelSize / 2);
        float X = Pos.x * VoxelSize;
        float Y = Pos.y * VoxelSize;

        float LocalVoxelThickness = VoxelThickness;

        if (Solid) LocalVoxelThickness = VoxelSize;

        Vertices.Add(new Vector3(CenterOffset                       , CenterOffset                  , 0));
        Vertices.Add(new Vector3(CenterOffset + LocalVoxelThickness , CenterOffset                  , 0));
        Vertices.Add(new Vector3(CenterOffset                       , CenterOffset + VoxelSize      , 0));
        Vertices.Add(new Vector3(CenterOffset + LocalVoxelThickness , CenterOffset + VoxelSize      , 0));

        ColorsList.Add(iColor);
        ColorsList.Add(iColor);
        ColorsList.Add(iColor);
        ColorsList.Add(iColor);

        float Length = Mathf.Sqrt(CapturedBody.Length) * VoxelSize;

        for (int i= Vertices.Count - 4; i<Vertices.Count; i++)
        {
            Vector3 V   = Vertices[i];
            V           = ERot * V;
            V.x += X - (Length / 2.0f) + VoxelSize/2;
            V.y += Y - (Length / 2.0f) + VoxelSize/2;
            Vertices[i] = V;
        }
   

    }





}
