// Runtime/Scripts/Utils/TripoSRResponse.cs
using System;

[Serializable]
public class TripoSRResponse
{
    public ModelMesh model_mesh;
    public Timings timings;
}

[Serializable]
public class ModelMesh
{
    public string url;
    public string format;
    public string file_name;
    public int file_size;
}

[Serializable]
public class Timings
{
    public float inference_time;
    public float total_time;
}