// Runtime/Scripts/Models/TripoSRConfig.cs

using System;

namespace TripoSR.SDK
{
    public class TripoSRConfig
    {
        public string ApiKey { get; set; }
    }


    [Serializable]
    public class TripoSRRequest
    {
        public string image_url;
        public string output_format = "glb";
        public bool do_remove_background = true;
        public float foreground_ratio = 0.9f;
        public int mc_resolution = 256;
    }
}
