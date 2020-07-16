using System.Collections.Generic;
using UnityEngine;

class Block08 : MonoBehaviour, IBlocktype
{        UnityEngine.GameObject _thisObject;
    public UnityEngine.GameObject thisObject { get => _thisObject; set => _thisObject = value; }

    public B3DScript b3dObject;
    public List<Vector3> vertices;
    public List<Vector2> UV;
    public List<string> material = new List<string>();
    public List<string> loops = new List<string>();

    public byte[] GetBytes()
    {
        throw new System.NotImplementedException();
    }

    public void Read(byte[] buffer, ref int pos)
    {
        //UV = new List<Vector2>();
        pos += 16;
        int i_null = System.BitConverter.ToInt32(buffer, pos);
        pos += 4;
        int j_null, format;

        List<int> faces_all = new List<int>();

        List<int> matNum = new List<int>();
        Mesh curMesh = new Mesh();
        curMesh.Clear();
        curMesh.subMeshCount = i_null;
        List<Material> mats = new List<Material>();

        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();


        for (int i = 0; i < i_null; i++)
        {
            //normals = null;
            List<int> faces = new List<int>();
            List<int> faces_old = new List<int>();
            format = System.BitConverter.ToInt32(buffer, pos);
            pos += 4;
            pos += 8;


            matNum.Add(System.BitConverter.ToInt32(buffer, pos));
            pos += 4;

            j_null = System.BitConverter.ToInt32(buffer, pos);
            pos += 4;
            string loop = "f: " + format + " | ";
            if ((format == 178) || (format == 50))
            {
                for (int j = 0; j < j_null; j++)
                {
                    int num = System.BitConverter.ToInt32(buffer, pos);
                    vertices.Add(vertices[num]);
                    faces_old.Add(vertices.Count - 1);
                    pos += 4;
                    UV.Add(new Vector2(System.BitConverter.ToSingle(buffer, pos + 0), System.BitConverter.ToSingle(buffer, pos + 4)));
                    pos += 8;
                    loop += faces_old[faces_old.Count - 1] + ' ' + UV[UV.Count - 1].ToString() + ' ' + System.BitConverter.ToSingle(buffer, pos + 0) + ' ' + System.BitConverter.ToSingle(buffer, pos + 4) + ' ' + System.BitConverter.ToSingle(buffer, pos + 8) + " ";
                    pos += 12;

                }
            }
            else if ((format == 3) || (format == 2) || (format == 131))
            {
                //List<int> faces3 = new List<int>();
                for (int j = 0; j < j_null; j++)
                {
                    int num = System.BitConverter.ToInt32(buffer, pos);
                    vertices.Add(vertices[num]);
                    faces_old.Add(vertices.Count - 1);
                    pos += 4;
                    UV.Add(new Vector2(System.BitConverter.ToSingle(buffer, pos + 0), System.BitConverter.ToSingle(buffer, pos + 4)));
                    pos += 8;
                    loop += faces_old[faces_old.Count - 1] + ' ' + UV[UV.Count - 1].ToString() + " ";

                }
            }
            else if ((format == 176) || (format == 48) || (format == 179) || (format == 51))
            {
                for (int j = 0; j < j_null; j++)
                {
                    faces_old.Add(System.BitConverter.ToInt32(buffer, pos));
                    pos += 4;
                    loop += faces_old[faces_old.Count - 1] + ' ' + System.BitConverter.ToSingle(buffer, pos + 0) + ' ' + System.BitConverter.ToSingle(buffer, pos + 4) + ' ' + System.BitConverter.ToSingle(buffer, pos + 8) + " ";

                    pos += 12;
                }
            }
            else if ((format == 177))
            {
                for (int j = 0; j < j_null; j++)
                {
                    faces_old.Add(System.BitConverter.ToInt32(buffer, pos));
                    pos += 4;
                    loop += faces_old[faces_old.Count - 1] + ' ' + System.BitConverter.ToSingle(buffer, pos + 0) + " ";
                    pos += 4;

                }
            }
            else
            {
                for (int j = 0; j < j_null; j++)
                {
                    faces_old.Add(System.BitConverter.ToInt32(buffer, pos));
                    pos += 4;
                    loop += faces_old[faces_old.Count - 1] + " ";
                }
            }
            loops.Add(loop);


            if ((format == 178))
            {
                faces.AddRange(new int[] { faces_old[0], faces_old[2], faces_old[1], faces_old[3], faces_old[1], faces_old[2] });
            }
            else
                for (int k = 0; k < faces_old.Count - 2; k++)
                {

                    if (k % 2 == 0)
                    {
                        if ((format == 178))
                        {
                            faces.Add(faces_old[k + 1]);
                            faces.Add(faces_old[k + 0]);
                            faces.Add(faces_old[k + 3]);

                        }
                        else if (format == 255)//debug
                        {
                            faces.Add(faces_old[k + 0]);
                            faces.Add(faces_old[k + 1]);
                            faces.Add(faces_old[k + 2]);
                        }

                        else
                        {
                            faces.Add(faces_old[k + 0]);
                            faces.Add(faces_old[k + 2]);
                            faces.Add(faces_old[k + 1]);

                        }
                    }
                    else
                    {
                        if ((format == 0) || (format == 16) || (format == 1) || (format == 48) || (format == 50) || (format == 2))
                        {
                            faces.Add(faces_old[0]);
                            faces.Add(faces_old[k + 2]);
                            faces.Add(faces_old[k + 1]);
                        }
                        else if (format == 255)//debug
                        {
                            faces.Add(faces_old[k - 1]);
                            faces.Add(faces_old[k + 2]);
                            faces.Add(faces_old[k + 1]);
                        }
                        else
                        {
                            faces.Add(faces_old[k + 0]);
                            faces.Add(faces_old[k + 1]);
                            faces.Add(faces_old[k + 2]);
                        }
                    }
                }
            mats.Add(b3dObject.GetComponent<Materials>().maths[b3dObject.TexInts[matNum[i]]]);
            material.Add(b3dObject.GetComponent<Materials>().material[b3dObject.TexInts[matNum[i]]]);
            curMesh.vertices = vertices.ToArray();
            curMesh.SetTriangles(faces, i);

            faces_all.AddRange(faces);
        }
        //bt.MatNum = b3dObject.TexInts[matNum[matNum.Count - 1]];
        gameObject.GetComponent<Renderer>().materials = mats.ToArray();

        //if (normals != null)
        // curMesh.normals = normals.ToArray();
        curMesh.uv = UV.ToArray();


        curMesh.RecalculateBounds();
        gameObject.GetComponent<MeshFilter>().mesh = curMesh;
    }
}